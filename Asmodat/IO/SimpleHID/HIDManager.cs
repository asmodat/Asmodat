// This source is subject to Microsoft Public License (Ms-PL).
// Please see http://simplehidlibrary.codeplex.com/ for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleHID
{
  /// <summary>
  /// USB HID helper class
  /// </summary>
  public class HIDManager
  {
    #region Public static methods

    /// <summary>
    /// Return a list information of connected USB HID devices
    /// </summary>
    static public IEnumerable<HIDInfoSet> GetInfoSets()
    {
      var devicePathList = new List<HIDInfoSet>();
      Guid gHid;
      NativeMethods.HidD_GetHidGuid(out gHid);
      var hInfoSet = NativeMethods.SetupDiGetClassDevs(ref gHid, null, IntPtr.Zero, NativeMethods.DIGCF_DEVICEINTERFACE | NativeMethods.DIGCF_PRESENT);
      if (hInfoSet == NativeMethods.InvalidHandleValue)
      {
        throw new Win32Exception();
      }
      var oInterface = new NativeMethods.DeviceInterfaceData();
      oInterface.Size = Marshal.SizeOf(oInterface);
      int nIndex = 0;
      while (NativeMethods.SetupDiEnumDeviceInterfaces(hInfoSet, 0, ref gHid, (uint)nIndex, ref oInterface))
      {
        var strDevicePath = GetDevicePath(hInfoSet, ref oInterface);
        var handle = Open(strDevicePath);
        if (handle != NativeMethods.InvalidHandleValue)
        {
          var productString = GetName(handle);
          var serialNumberString = GetSerialNumber(handle);
          NativeMethods.HIDD_ATTRIBUTES attributes;
          GetAttr(handle, out attributes);
          devicePathList.Add(new HIDInfoSet(productString, serialNumberString, strDevicePath, attributes.VendorID, attributes.ProductID, attributes.VersionNumber));
        }
        nIndex++;	
      }
      if (NativeMethods.SetupDiDestroyDeviceInfoList(hInfoSet) == false)
      {
        throw new Win32Exception();
      }
      
      return devicePathList;
    }

    #endregion

    #region Private helper methods

    private static IntPtr Open(string devicePath)
    {
      return NativeMethods.CreateFile(devicePath, NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE, NativeMethods.FILE_SHARE_READ | NativeMethods.FILE_SHARE_WRITE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, NativeMethods.FILE_FLAG_OVERLAPPED, IntPtr.Zero);
    }

    private static void GetAttr(IntPtr handle, out NativeMethods.HIDD_ATTRIBUTES attributes)
    {
      attributes = new NativeMethods.HIDD_ATTRIBUTES();
      attributes.Size = Marshal.SizeOf(attributes);
      if (NativeMethods.HidD_GetAttributes(handle, ref attributes) == false)
      {
        throw new Win32Exception();
      }
    }

    private static string GetName(IntPtr handle)
    {
      var manufacturerString = new StringBuilder(128);
      if (NativeMethods.HidD_GetProductString(handle, manufacturerString, manufacturerString.Capacity) == false)
      {
        throw new Win32Exception();
      }
      return manufacturerString.ToString();
    }

    private static string GetSerialNumber(IntPtr handle)
    {
      var serialNumberString = new StringBuilder(128);
      if (NativeMethods.HidD_GetSerialNumberString(handle, serialNumberString, serialNumberString.Capacity) == false)
      {
        // Some devices has no serial number
        // Just return an empty string
        return string.Empty;
      }
      return serialNumberString.ToString();
    }

    /// <summary>
    /// Get USB HID device path
    /// </summary>
    private static string GetDevicePath(IntPtr hInfoSet, ref NativeMethods.DeviceInterfaceData oInterface)
    {
      uint nRequiredSize = 0;
      if (NativeMethods.SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, IntPtr.Zero, 0, ref nRequiredSize, IntPtr.Zero) == false)
      {
        // TODO: Find a solution
        // Tip: http://stackoverflow.com/questions/1054748/setupdigetdeviceinterfacedetail-unexplainable-error
        //throw new HIDDeviceException();
      }
      var oDetail = new NativeMethods.DeviceInterfaceDetailData();
      oDetail.Size = Marshal.SizeOf(typeof(IntPtr)) == 8 ? 8 : 5; // x86/x64 magic...
      if (NativeMethods.SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, ref oDetail, nRequiredSize, ref nRequiredSize, IntPtr.Zero) == false)
      {
        throw new Win32Exception();
      }
      return oDetail.DevicePath;
    }

    #endregion
  }
}