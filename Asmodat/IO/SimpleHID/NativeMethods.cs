// This source is subject to Microsoft Public License (Ms-PL).
// Please see http://simplehidlibrary.codeplex.com/ for details.
// All other rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleHID
{
  /// <summary>
  /// WIN32 data structures and P/Invoke definitions
  /// </summary>
  internal class NativeMethods
  {
    #region setupapi.dll

    public const int DIGCF_PRESENT = 0x02;
    public const int DIGCF_DEVICEINTERFACE = 0x10;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeviceInterfaceData
    {
      public int Size;
      public Guid InterfaceClassGuid;
      public int Flags;
      public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeviceInterfaceDetailData
    {
      public int Size;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string DevicePath;
    }

    [DllImport("setupapi.dll", SetLastError = true)]
    public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr lpDeviceInfoSet, ref DeviceInterfaceData oInterfaceData, IntPtr lpDeviceInterfaceDetailData, uint nDeviceInterfaceDetailDataSize, ref uint nRequiredSize, IntPtr lpDeviceInfoData);

    [DllImport("setupapi.dll", SetLastError = true)]
    public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr lpDeviceInfoSet, ref DeviceInterfaceData oInterfaceData, ref DeviceInterfaceDetailData oDetailData, uint nDeviceInterfaceDetailDataSize, ref uint nRequiredSize, IntPtr lpDeviceInfoData);

    [DllImport("setupapi.dll", SetLastError = true)]
    public static extern bool SetupDiEnumDeviceInterfaces(IntPtr lpDeviceInfoSet, uint nDeviceInfoData, ref Guid gClass, uint nIndex, ref DeviceInterfaceData oInterfaceData);

    [DllImport("setupapi.dll", SetLastError = true)]
    public static extern bool SetupDiDestroyDeviceInfoList(IntPtr lpInfoSet);

    [DllImport("setupapi.dll", SetLastError = true)]
    public static extern IntPtr SetupDiGetClassDevs(ref Guid gClass, [MarshalAs(UnmanagedType.LPStr)] string strEnumerator, IntPtr hParent, uint nFlags);

    #endregion

    #region hid.dll

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HidCaps
    {
      public short Usage;
      public short UsagePage;
      public short InputReportByteLength;
      public short OutputReportByteLength;
      public short FeatureReportByteLength;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
      public short[] Reserved;
      public short NumberLinkCollectionNodes;
      public short NumberInputButtonCaps;
      public short NumberInputValueCaps;
      public short NumberInputDataIndices;
      public short NumberOutputButtonCaps;
      public short NumberOutputValueCaps;
      public short NumberOutputDataIndices;
      public short NumberFeatureButtonCaps;
      public short NumberFeatureValueCaps;
      public short NumberFeatureDataIndices;
    }

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff538924%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    public static extern void HidD_GetHidGuid(out Guid gHid);

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff539679%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    public static extern bool HidD_GetPreparsedData(IntPtr hFile, out IntPtr lpData);

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff539715%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    public static extern int HidP_GetCaps(IntPtr lpData, out HidCaps oCaps);

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff538893%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    public static extern bool HidD_FreePreparsedData(ref IntPtr pData);

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff538959%28v=vs.85%29.aspx
    [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern Boolean HidD_GetManufacturerString(IntPtr hFile, StringBuilder buffer, Int32 bufferLength);

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff539681%28v=vs.85%29.aspx
    [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern Boolean HidD_GetProductString(IntPtr hFile, StringBuilder buffer, Int32 bufferLength);

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff539683%28v=vs.85%29.aspx
    [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool HidD_GetSerialNumberString(IntPtr hDevice, StringBuilder buffer, Int32 bufferLength);

    [StructLayout(LayoutKind.Sequential)]
    public struct HIDD_ATTRIBUTES
    {
      public Int32 Size;
      public Int16 VendorID;
      public Int16 ProductID;
      public Int16 VersionNumber;
    }

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff538900%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    public static extern Boolean HidD_GetAttributes(IntPtr hFile, ref HIDD_ATTRIBUTES attributes);

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff539690%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    public static extern Boolean HidD_SetOutputReport(IntPtr hFile, byte[] lpReportBuffer, Int32 reportBufferLength);

    // from hidpi.h
    public const int HIDP_STATUS_SUCCESS = (0x0 << 28) | (0x11 << 16) | 0;
    public const int HIDP_STATUS_NULL = (0x8 << 28) | (0x11 << 16) | 1;
    public const int HIDP_STATUS_INVALID_PREPARSED_DATA = (0xC << 28) | (0x11 << 16) | 1;
    public const int HIDP_STATUS_INVALID_REPORT_TYPE = (0xC << 28) | (0x11 << 16) | 2;
    public const int HIDP_STATUS_INVALID_REPORT_LENGTH = (0xC << 28) | (0x11 << 16) | 3;
    public const int HIDP_STATUS_USAGE_NOT_FOUND = (0xC << 28) | (0x11 << 16) | 4;
    public const int HIDP_STATUS_VALUE_OUT_OF_RANGE = (0xC << 28) | (0x11 << 16) | 5;
    public const int HIDP_STATUS_BAD_LOG_PHY_VALUES = (0xC << 28) | (0x11 << 16) | 6;
    public const int HIDP_STATUS_BUFFER_TOO_SMALL = (0xC << 28) | (0x11 << 16) | 7;
    public const int HIDP_STATUS_INTERNAL_ERROR = (0xC << 28) | (0x11 << 16) | 8;
    public const int HIDP_STATUS_I8042_TRANS_UNKNOWN = (0xC << 28) | (0x11 << 16) | 9;
    public const int HIDP_STATUS_INCOMPATIBLE_REPORT_ID = (0xC << 28) | (0x11 << 16) | 0xA;
    public const int HIDP_STATUS_NOT_VALUE_ARRAY = (0xC << 28) | (0x11 << 16) | 0xB;
    public const int HIDP_STATUS_IS_VALUE_ARRAY = (0xC << 28) | (0x11 << 16) | 0xC;
    public const int HIDP_STATUS_DATA_INDEX_NOT_FOUND = (0xC << 28) | (0x11 << 16) | 0xD;
    public const int HIDP_STATUS_DATA_INDEX_OUT_OF_RANGE = (0xC << 28) | (0x11 << 16) | 0xE;
    public const int HIDP_STATUS_BUTTON_NOT_PRESSED = (0xC << 28) | (0x11 << 16) | 0xF;
    public const int HIDP_STATUS_REPORT_DOES_NOT_EXIST = (0xC << 28) | (0x11 << 16) | 0x10;
    public const int HIDP_STATUS_NOT_IMPLEMENTED = (0xC << 28) | (0x11 << 16) | 0x20;

    [StructLayout(LayoutKind.Sequential)]
    public struct HidP_Range
    {
      public short UsageMin;
      public short UsageMax;
      public short StringMin;
      public short StringMax;
      public short DesignatorMin;
      public short DesignatorMax;
      public short DataIndexMin;
      public short DataIndexMax;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HidP_NotRange
    {
      public short Usage;
      public short Reserved1;
      public short StringIndex;
      public short Reserved2;
      public short DesignatorIndex;
      public short Reserved3;
      public short DataIndex;
      public short Reserved4;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct HidP_Button_Caps
    {
      [FieldOffset(0)]
      public short UsagePage;
      [FieldOffset(2)]
      public byte ReportID;
      [FieldOffset(3), MarshalAs(UnmanagedType.U1)]
      public bool IsAlias;
      [FieldOffset(4)]
      public short BitField;
      [FieldOffset(6)]
      public short LinkCollection;
      [FieldOffset(8)]
      public short LinkUsage;
      [FieldOffset(10)]
      public short LinkUsagePage;
      [FieldOffset(12), MarshalAs(UnmanagedType.U1)]
      public bool IsRange;
      [FieldOffset(13), MarshalAs(UnmanagedType.U1)]
      public bool IsStringRange;
      [FieldOffset(14), MarshalAs(UnmanagedType.U1)]
      public bool IsDesignatorRange;
      [FieldOffset(15), MarshalAs(UnmanagedType.U1)]
      public bool IsAbsolute;
      [FieldOffset(16), MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
      public int[] Reserved;
      // The Structs in the Union			
      [FieldOffset(56)]
      public HidP_Range Range;
      [FieldOffset(56)]
      public HidP_NotRange NotRange;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct HidP_Value_Caps
    {
      [FieldOffset(0)]
      public short UsagePage;
      [FieldOffset(2)]
      public byte ReportID;
      [MarshalAs(UnmanagedType.I1)]
      [FieldOffset(3)]
      public bool IsAlias;
      [FieldOffset(4)]
      public short BitField;
      [FieldOffset(6)]
      public short LinkCollection;
      [FieldOffset(8)]
      public short LinkUsage;
      [FieldOffset(10)]
      public short LinkUsagePage;
      [MarshalAs(UnmanagedType.I1)]
      [FieldOffset(12)]
      public bool IsRange;
      [MarshalAs(UnmanagedType.I1)]
      [FieldOffset(13)]
      public bool IsStringRange;
      [MarshalAs(UnmanagedType.I1)]
      [FieldOffset(14)]
      public bool IsDesignatorRange;
      [MarshalAs(UnmanagedType.I1)]
      [FieldOffset(15)]
      public bool IsAbsolute;
      [MarshalAs(UnmanagedType.I1)]
      [FieldOffset(16)]
      public bool HasNull;
      [FieldOffset(17)]
      public System.Char Reserved;						// UCHAR  Reserved;
      [FieldOffset(18)]
      public short BitSize;
      [FieldOffset(20)]
      public short ReportCount;
      [FieldOffset(22)]
      public short Reserved2a;
      [FieldOffset(24)]
      public short Reserved2b;
      [FieldOffset(26)]
      public short Reserved2c;
      [FieldOffset(28)]
      public short Reserved2d;
      [FieldOffset(30)]
      public short Reserved2e;
      [FieldOffset(32)]
      public short UnitsExp;
      [FieldOffset(34)]
      public short Units;
      [FieldOffset(36)]
      public short LogicalMin;
      [FieldOffset(38)]
      public short LogicalMax;
      [FieldOffset(40)]
      public short PhysicalMin;
      [FieldOffset(42)]
      public short PhysicalMax;
      // The Structs in the Union			
      [FieldOffset(44)]
      public HidP_Range Range;
      [FieldOffset(44)]
      public HidP_NotRange NotRange;
    }

    public enum HIDP_REPORT_TYPE
    {
      HidP_Input,
      HidP_Output,
      HidP_Feature,
    }

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff539707%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    static public extern int HidP_GetButtonCaps(HIDP_REPORT_TYPE reportType, [In, Out] HidP_Button_Caps[] buttonCaps, ref short buttonCapsLength, IntPtr preparsedData);

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff539754%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    public static extern int HidP_GetValueCaps(HIDP_REPORT_TYPE reportType, [In, Out] HidP_Value_Caps[] valueCaps, ref short valueCapsLength, IntPtr preparsedData);

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff539757%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    static public extern int HidP_InitializeReportForID(HIDP_REPORT_TYPE reportType, byte reportID, IntPtr preparsedData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] report, int reportLength);

    [StructLayout(LayoutKind.Explicit)]
    public struct HIDP_DATA
    {
      [FieldOffset(0)]
      public short DataIndex;
      [FieldOffset(2)]
      public short Reserved;

      [FieldOffset(4)]
      public int RawValue;
      [FieldOffset(4), MarshalAs(UnmanagedType.U1)]
      public bool On;
    }

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff539792%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    static public extern int HidP_SetUsages(HIDP_REPORT_TYPE reportType, short usagePage, short linkCollection, [In, Out] HIDP_DATA[] usageList, ref int usageLength, IntPtr preparsedData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] report, int reportLength);

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff539783%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    static public extern int HidP_SetData(HIDP_REPORT_TYPE reportType, [In, Out] HIDP_DATA[] dataList, ref int dataLength, IntPtr preparsedData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] byte[] report, int reportLength);

    // http://msdn.microsoft.com/en-us/library/windows/hardware/ff539718%28v=vs.85%29.aspx
    [DllImport("hid.dll", SetLastError = true)]
    static public extern int HidP_GetData(HIDP_REPORT_TYPE reportType, [In, Out] HIDP_DATA[] dataList, ref int dataLength, IntPtr preparsedData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] byte[] report, int reportLength);

    #endregion

    #region kernel32.dll

    public const uint GENERIC_READ = 0x80000000;
    public const uint GENERIC_WRITE = 0x40000000;
    public const uint FILE_SHARE_WRITE = 0x2;
    public const uint FILE_SHARE_READ = 0x1;
    public const uint FILE_FLAG_OVERLAPPED = 0x40000000;
    public const uint OPEN_EXISTING = 3;
    public const uint OPEN_ALWAYS = 4;

    public static IntPtr NullHandle = IntPtr.Zero;
    public static IntPtr InvalidHandleValue = new IntPtr(-1);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPStr)] string strName, uint nAccess, uint nShareMode, IntPtr lpSecurity, uint nCreationFlags, uint nAttributes, IntPtr lpTemplate);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr hObject);

    #endregion
  }
}