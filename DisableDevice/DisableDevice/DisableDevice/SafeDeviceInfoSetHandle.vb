Imports Microsoft.Win32.SafeHandles
Imports DisableDevice.NativeMethods

Friend Class SafeDeviceInfoSetHandle
    Inherits SafeHandleZeroOrMinusOneIsInvalid

    Sub New()
        MyBase.New(True)
    End Sub

    Protected Overrides Function ReleaseHandle() As Boolean
        NativeMethods.SetupDiDestroyDeviceInfoList(Me.handle)
    End Function

End Class
