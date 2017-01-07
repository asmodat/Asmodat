Public Class Form1

    ' class guid for mice.
    ' Device manager, right click a mouse (that can be disabled), click properties, 
    ' Details tab, select "device class guid" property:
    Dim mouseGuid As New Guid("{4d36e96f-e325-11ce-bfc1-08002be10318}") '"{4d36e96f-e325-11ce-bfc1-08002be10318}")
    ' instance id for some weird infrared mouse I have. "device instance path" in device manager:
    Dim instanceId As String = "HID\VID_0738&PID_1703\6&3AD7185F&0&0000" '"HID\IRDEVICE&COL08\2&D6067AB&0&0007"

    Private Sub btnEnable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnable.Click
        DeviceHelper.SetDeviceEnabled(mouseGuid, instanceId, True)
    End Sub

    Private Sub btnDisable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisable.Click
        DeviceHelper.SetDeviceEnabled(mouseGuid, instanceId, False)
    End Sub

End Class
