Imports DisableDevice.NativeMethods
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Text

Public NotInheritable Class DeviceHelper

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Enable or disable a device.
    ''' </summary>
    ''' <param name="classGuid">The class guid of the device. Available in the device manager.</param>
    ''' <param name="instanceId">The device instance id of the device. Available in the device manager.</param>
    ''' <param name="enable">True to enable, False to disable.</param>
    ''' <remarks>Will throw an exception if the device is not Disableable.</remarks>
    Public Shared Sub SetDeviceEnabled(ByVal classGuid As Guid, ByVal instanceId As String, ByVal enable As Boolean)
        Dim diSetHandle As SafeDeviceInfoSetHandle = Nothing
        Try
            ' Get the handle to a device information set for all devices matching classGuid that are present on the 
            ' system.
            diSetHandle = SetupDiGetClassDevs(classGuid, Nothing, Nothing, SetupDiGetClassDevsFlags.Present)
            ' Get the device information data for each matching device.
            Dim diData() As DeviceInfoData = GetDeviceInfoData(diSetHandle)
            ' Find the index of our instance. i.e. the touchpad mouse - I have 3 mice attached...
            Dim index As Integer = GetIndexOfInstance(diSetHandle, diData, instanceId)
            ' Disable...
            EnableDevice(diSetHandle, diData(index), enable)
        Finally
            If diSetHandle IsNot Nothing Then
                If diSetHandle.IsClosed = False Then
                    diSetHandle.Close()
                End If
                diSetHandle.Dispose()
            End If
        End Try
    End Sub

    Private Shared Function GetDeviceInfoData(ByVal handle As SafeDeviceInfoSetHandle) As DeviceInfoData()
        Dim data As New List(Of DeviceInfoData)
        Dim did As New DeviceInfoData
        Dim didSize As Integer = Marshal.SizeOf(did)
        did.Size = didSize
        Dim index As Integer
        Do While SetupDiEnumDeviceInfo(handle, index, did)
            data.Add(did)
            index += 1
            did = New DeviceInfoData
            did.Size = didSize
        Loop
        Return data.ToArray
    End Function

    ' Find the index of the particular DeviceInfoData for the instanceId.
    Private Shared Function GetIndexOfInstance(ByVal handle As SafeDeviceInfoSetHandle, _
                                                ByVal diData() As DeviceInfoData, _
                                                ByVal instanceId As String) As Integer
        Const ERROR_INSUFFICIENT_BUFFER As Integer = 122
        For index As Integer = 0 To diData.Length - 1
            Dim sb As New StringBuilder(1)
            Dim requiredSize As Integer
            Dim result As Boolean = SetupDiGetDeviceInstanceId(handle, diData(index), sb, sb.Capacity, requiredSize)
            If result = False AndAlso Marshal.GetLastWin32Error = ERROR_INSUFFICIENT_BUFFER Then
                sb.Capacity = requiredSize
                result = SetupDiGetDeviceInstanceId(handle, diData(index), sb, sb.Capacity, requiredSize)
            End If
            If result = False Then Throw New Win32Exception
            If instanceId.Equals(sb.ToString) Then
                Return index
            End If
        Next
        Return -1 ' not found
    End Function

    ' enable/disable...
    Private Shared Sub EnableDevice(ByVal handle As SafeDeviceInfoSetHandle, _
                               ByVal diData As DeviceInfoData, ByVal enable As Boolean)
        Dim params As New PropertyChangeParameters
        ' The size is just the size of the header, but we've flattened the structure.
        ' The header comprises the first two fields, both integer.
        params.Size = 8
        params.DiFunction = DiFunction.PropertyChange
        params.Scope = Scopes.Global
        If enable Then
            params.StateChange = StateChangeAction.Enable
        Else
            params.StateChange = StateChangeAction.Disable
        End If

        Dim result As Boolean = SetupDiSetClassInstallParams(handle, diData, params, Marshal.SizeOf(params))
        If result = False Then Throw New Win32Exception
        result = SetupDiCallClassInstaller(DiFunction.PropertyChange, handle, diData)
        If result = False Then
            Dim err As Integer = Marshal.GetLastWin32Error
            Select Case err
                Case Is = SetupApiError.NotDisableable
                    Throw New ArgumentException("That device can't be disabled! Look in the device manager!")
                Case SetupApiError.NoAssociatedClass To SetupApiError.OnlyValidateViaAuthenticode
                    Throw New Win32Exception("SetupAPI error: " & DirectCast(err, SetupApiError).ToString)
                Case Else
                    Throw New Win32Exception
            End Select
        End If
    End Sub

End Class
