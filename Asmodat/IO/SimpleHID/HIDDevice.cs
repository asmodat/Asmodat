// This source is subject to Microsoft Public License (Ms-PL).
// Please see http://simplehidlibrary.codeplex.com/ for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Microsoft.Win32.SafeHandles;
using SimpleHID.Report;

namespace SimpleHID
{
  /// <summary>
  /// USB HID device
  /// </summary>
  public class HIDDevice : IDisposable
  {
    #region Private stuff

    private IntPtr handle;
    private FileStream fileStream;
    private NativeMethods.HidCaps hidCaps;
    private NativeMethods.HidP_Button_Caps[] outputButtonCaps;
    private NativeMethods.HidP_Value_Caps[] outputValueCaps;
    private NativeMethods.HidP_Button_Caps[] inputButtonCaps;
    private NativeMethods.HidP_Value_Caps[] inputValueCaps;
    private NativeMethods.HidP_Button_Caps[] featureButtonCaps;
    private NativeMethods.HidP_Value_Caps[] featureValueCaps;
    private IntPtr preparedData;

    #endregion

    #region Public properties

    /// <summary>
    /// 
    /// </summary>
    public List<string> HidCaps
    {
      get
      {
        var texts = new List<string>();
        texts.Add(string.Format("Usage: 0x{0:X4}", hidCaps.Usage));
        texts.Add(string.Format("UsagePage: 0x{0:X4}", hidCaps.UsagePage));
        texts.Add(string.Format("InputReportByteLength: 0x{0:X4}", hidCaps.InputReportByteLength));
        texts.Add(string.Format("OutputReportByteLength: 0x{0:X4}", hidCaps.OutputReportByteLength));
        texts.Add(string.Format("FeatureReportByteLength: 0x{0:X4}", hidCaps.FeatureReportByteLength));
        texts.Add(string.Format("NumberLinkCollectionNodes: 0x{0:X4}", hidCaps.NumberLinkCollectionNodes));
        texts.Add(string.Format("NumberInputButtonCaps: 0x{0:X4}", hidCaps.NumberInputButtonCaps));
        texts.Add(string.Format("NumberInputValueCaps: 0x{0:X4}", hidCaps.NumberInputValueCaps));
        texts.Add(string.Format("NumberInputDataIndices: 0x{0:X4}", hidCaps.NumberInputDataIndices));
        texts.Add(string.Format("NumberOutputButtonCaps: 0x{0:X4}", hidCaps.NumberOutputButtonCaps));
        texts.Add(string.Format("NumberOutputValueCaps: 0x{0:X4}", hidCaps.NumberOutputValueCaps));
        texts.Add(string.Format("NumberOutputDataIndices: 0x{0:X4}", hidCaps.NumberOutputDataIndices));
        texts.Add(string.Format("NumberFeatureButtonCaps: 0x{0:X4}", hidCaps.NumberFeatureButtonCaps));
        texts.Add(string.Format("NumberFeatureValueCaps: 0x{0:X4}", hidCaps.NumberFeatureValueCaps));
        texts.Add(string.Format("NumberFeatureDataIndices: 0x{0:X4}", hidCaps.NumberFeatureDataIndices));
        return texts;
      }
    }

    /// <summary>
    /// OutputButtonCaps
    /// </summary>
    public List<ButtonBaseItem> OutputButtonCaps
    {
      get { return ConvertToBaseItemList(outputButtonCaps); }
    }

    /// <summary>
    /// OutputValueCaps
    /// </summary>
    public List<ValueBaseItem> OutputValueCaps
    {
      get { return ConvertToBaseItemList(outputValueCaps); }
    }

    /// <summary>
    /// InputButtonCaps
    /// </summary>
    public List<ButtonBaseItem> InputButtonCaps
    {
      get { return ConvertToBaseItemList(inputButtonCaps); }
    }

    /// <summary>
    /// InputValueCaps
    /// </summary>
    public List<ValueBaseItem> InputValueCaps
    {
      get { return ConvertToBaseItemList(inputValueCaps); }
    }

    /// <summary>
    /// FeatureButtonCaps
    /// </summary>
    public List<ButtonBaseItem> FeatureButtonCaps
    {
      get { return ConvertToBaseItemList(featureButtonCaps); }
    }

    /// <summary>
    /// FeatureValueCaps
    /// </summary>
    public List<ValueBaseItem> FeatureValueCaps
    {
      get { return ConvertToBaseItemList(featureValueCaps); }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// ctor
    /// </summary>
    public HIDDevice()
    {
    }

    #endregion

    #region Init

    /// <summary>
    /// Connect to device
    /// </summary>
    public void Init(string devicePath)
    {
      handle = NativeMethods.CreateFile(devicePath, NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE, NativeMethods.FILE_SHARE_READ | NativeMethods.FILE_SHARE_WRITE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, NativeMethods.FILE_FLAG_OVERLAPPED, IntPtr.Zero);
      if (handle != NativeMethods.InvalidHandleValue || handle == null)
      {
        if (NativeMethods.HidD_GetPreparsedData(handle, out preparedData))
        {
          try
          {
            if (NativeMethods.HidP_GetCaps(preparedData, out hidCaps) != NativeMethods.HIDP_STATUS_SUCCESS)
            {
              throw new Win32Exception();
            }

            var numberOutputButtonCaps = hidCaps.NumberOutputButtonCaps;
            if (numberOutputButtonCaps != 0)
            {
              outputButtonCaps = new NativeMethods.HidP_Button_Caps[numberOutputButtonCaps];
              if (NativeMethods.HidP_GetButtonCaps(NativeMethods.HIDP_REPORT_TYPE.HidP_Output, outputButtonCaps, ref numberOutputButtonCaps, preparedData) != NativeMethods.HIDP_STATUS_SUCCESS)
              {
                throw new Win32Exception();
              }
            }
            else
            {
              outputButtonCaps = null;
            }

            var numberOutputValueCaps = hidCaps.NumberOutputValueCaps;
            if (numberOutputValueCaps != 0)
            {
              outputValueCaps = new NativeMethods.HidP_Value_Caps[numberOutputValueCaps];
              if (NativeMethods.HidP_GetValueCaps(NativeMethods.HIDP_REPORT_TYPE.HidP_Output, outputValueCaps, ref numberOutputValueCaps, preparedData) != NativeMethods.HIDP_STATUS_SUCCESS)
              {
                throw new Win32Exception();
              }
            }
            else
            {
              outputValueCaps = null;
            }

            var numberInputButtonCaps = hidCaps.NumberInputButtonCaps;
            if (numberInputButtonCaps != 0)
            {
              inputButtonCaps = new NativeMethods.HidP_Button_Caps[numberInputButtonCaps];
              if (NativeMethods.HidP_GetButtonCaps(NativeMethods.HIDP_REPORT_TYPE.HidP_Input, inputButtonCaps, ref numberInputButtonCaps, preparedData) != NativeMethods.HIDP_STATUS_SUCCESS)
              {
                throw new Win32Exception();
              }
            }
            else
            {
              inputButtonCaps = null;
            }

            var numberInputValueCaps = hidCaps.NumberInputValueCaps;
            if (numberInputValueCaps != 0)
            {
              inputValueCaps = new NativeMethods.HidP_Value_Caps[numberInputValueCaps];
              if (NativeMethods.HidP_GetValueCaps(NativeMethods.HIDP_REPORT_TYPE.HidP_Input, inputValueCaps, ref numberInputValueCaps, preparedData) != NativeMethods.HIDP_STATUS_SUCCESS)
              {
                throw new Win32Exception();
              }
            }
            else
            {
              inputValueCaps = null;
            }

            var numberFeatureButtonCaps = hidCaps.NumberFeatureButtonCaps;
            if (numberFeatureButtonCaps != 0)
            {
              featureButtonCaps = new NativeMethods.HidP_Button_Caps[numberFeatureButtonCaps];
              if (NativeMethods.HidP_GetButtonCaps(NativeMethods.HIDP_REPORT_TYPE.HidP_Feature, featureButtonCaps, ref numberFeatureButtonCaps, preparedData) != NativeMethods.HIDP_STATUS_SUCCESS)
              {
                throw new Win32Exception();
              }
            }
            else
            {
              featureButtonCaps = null;
            }

            var numberFeatureValueCaps = hidCaps.NumberFeatureValueCaps;
            if (numberFeatureValueCaps != 0)
            {
              featureValueCaps = new NativeMethods.HidP_Value_Caps[numberFeatureValueCaps];
              if (NativeMethods.HidP_GetValueCaps(NativeMethods.HIDP_REPORT_TYPE.HidP_Feature, featureValueCaps, ref numberFeatureValueCaps, preparedData) != NativeMethods.HIDP_STATUS_SUCCESS)
              {
                throw new Win32Exception();
              }
            }
            else
            {
              featureValueCaps = null;
            }

            fileStream = new FileStream(new SafeFileHandle(handle, false), FileAccess.Read | FileAccess.Write, hidCaps.InputReportByteLength, true);
          }
          catch (Exception)
          {
            throw new HIDDeviceException();
          }
        }
        else
        {
          throw new HIDDeviceException();
        }
      }
      else
      {
        handle = IntPtr.Zero;
        throw new HIDDeviceException();
      }
    }

    #endregion

    #region Input

    /// <summary>
    /// Create raw data for an Input Report with a specific Id
    /// </summary>
    /// <param name="reportId">InputReport Id</param>
    /// <returns>Raw data</returns>
    private byte[] CreateRawInputReport(byte reportId)
    {
      var data = new byte[hidCaps.InputReportByteLength];
      if (NativeMethods.HidP_InitializeReportForID(NativeMethods.HIDP_REPORT_TYPE.HidP_Input, reportId, preparedData, data, data.Length) != NativeMethods.HIDP_STATUS_SUCCESS)
      {
        throw new Win32Exception();
      }
      return data;
    }

    /// <summary>
    /// 
    /// </summary>
    private byte[] ReadRawInputReport()
    {
      var firstInputReportId = inputButtonCaps[0].ReportID;
      var data = CreateRawInputReport(firstInputReportId);
      try
      {
        fileStream.Read(data, 0, data.Length);
      }
      catch (Exception)
      {
        throw new HIDDeviceException();
      }
      return data;
    }

    /// <summary>
    /// 
    /// </summary>
    public List<ButtonBaseItem> ReadInputReport()
    {
      var data = ReadRawInputReport();
      int length = 100;
      var uu = new NativeMethods.HIDP_DATA[length];
      if (NativeMethods.HidP_GetData(NativeMethods.HIDP_REPORT_TYPE.HidP_Input, uu, ref length, preparedData, data,
                                           data.Length) != NativeMethods.HIDP_STATUS_SUCCESS)
      {
        throw new Win32Exception();
      }

      var ret = new List<ButtonBaseItem>();
      for (var i = 0; i < length; i++ )
      {
        foreach (var inputButtonCap in inputButtonCaps)
        {
          if (inputButtonCap.NotRange.DataIndex == uu[i].DataIndex)
          {
            if (inputButtonCap.IsRange)
            {
              var newItem = new ButtonRangeItem();
              newItem.TopLevelUsagePage = hidCaps.UsagePage;
              newItem.UsagePage = inputButtonCap.UsagePage;
              newItem.ReportId = inputButtonCap.ReportID;
              newItem.UsageMin = inputButtonCap.Range.UsageMin;
              newItem.UsageMax = inputButtonCap.Range.UsageMax;
              ret.Add(newItem);
            }
            else
            {
              var newItem = new ButtonItem();
              newItem.TopLevelUsagePage = hidCaps.UsagePage;
              newItem.UsagePage = inputButtonCap.UsagePage;
              newItem.ReportId = inputButtonCap.ReportID;
              newItem.Usage = inputButtonCap.NotRange.Usage;
              ret.Add(newItem);
            }
          }
        }
      }
      return ret;
    }

    #endregion

    #region Output

    /// <summary>
    /// Create raw data for an Output Report with a specific Id
    /// </summary>
    /// <param name="reportId">OutputReport Id</param>
    /// <returns>Raw data</returns>
    private byte[] CreateRawOutputReport(byte reportId)
    {
      var data = new byte[hidCaps.OutputReportByteLength];
      if (NativeMethods.HidP_InitializeReportForID(NativeMethods.HIDP_REPORT_TYPE.HidP_Output, reportId, preparedData, data, data.Length) != NativeMethods.HIDP_STATUS_SUCCESS)
      {
        throw new Win32Exception();
      }
      return data;
    }

    /// <summary>
    /// Write binary data
    /// </summary>
    private void WriteRawOutputReport(byte[] data)
    {
//      if (NativeMethods.HidD_SetOutputReport(handle, data, data.Length) == false)
//      {
//        throw new HIDDeviceException();
//      }


      try
      {
        fileStream.Write(data, 0, data.Length);
        fileStream.Flush();
      }
      catch (Exception)
      {
        throw new HIDDeviceException();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void WriteOutputReport(List<ButtonBaseItem> usages)
    {
      var hidItems = new NativeMethods.HIDP_DATA[100];
      int lenght = 0;

      byte reportId = 0;

      foreach (var usage in usages)
      {
        foreach (var outputButtonCap in outputButtonCaps)
        {
          var notRangeItem = usage as ButtonItem;
          if (notRangeItem != null)
          {
            if (notRangeItem.ReportId == outputButtonCap.ReportID && notRangeItem.UsagePage == outputButtonCap.UsagePage && notRangeItem.Usage == outputButtonCap.NotRange.Usage)
            {
              if (lenght == 0)
              {
                reportId = outputButtonCap.ReportID;
              }
              hidItems[lenght].DataIndex = outputButtonCap.NotRange.DataIndex;
              hidItems[lenght].On = true;
              lenght++;
            }
          }
          // TODO: Handle ButtonRangeItem
        }
      }

      var data = CreateRawOutputReport(reportId);
      if (NativeMethods.HidP_SetData(NativeMethods.HIDP_REPORT_TYPE.HidP_Output, hidItems, ref lenght, preparedData, data,
                                            data.Length) != NativeMethods.HIDP_STATUS_SUCCESS)
      {
        throw new Win32Exception();
      }
      
      WriteRawOutputReport(data);

//      // TODO: Do some LINQ'ing here
//      foreach (var outputButtonCap in outputButtonCaps)
//      {
//        if (outputButtonCap.IsRange == false && usagePage == outputButtonCap.UsagePage && usage == outputButtonCap.NotRange.Usage)
//        {
//          var data = CreateRawOutputReport(outputButtonCap.ReportID);
//
//          var b = new NativeMethods.HIDP_DATA[1];
//          b[0].DataIndex = outputButtonCap.NotRange.DataIndex;
//          b[0].On = true;
//          int lenght = 1;
//
//          if (NativeMethods.HidP_SetData(NativeMethods.HIDP_REPORT_TYPE.HidP_Output, b, ref lenght, preparedData, data,
//                                               data.Length) != NativeMethods.HIDP_STATUS_SUCCESS)
//          {
//            throw new Win32Exception();
//          }
//
//          WriteRawOutputReport(data);
//          return;
//        }
//      }
//      throw new Exception("Could not find UsagePage/Usage");
    }

    #endregion

    #region Helpers

    private List<ButtonBaseItem> ConvertToBaseItemList(NativeMethods.HidP_Button_Caps[] buttonCaps)
    {
      var ret = new List<ButtonBaseItem>();
      if (buttonCaps != null)
      {
        foreach (var buttonCap in buttonCaps)
        {
          if (buttonCap.IsRange)
          {
            ret.Add(
              new ButtonRangeItem
                {
                  TopLevelUsagePage = hidCaps.UsagePage,
                  ReportId = buttonCap.ReportID,
                  UsagePage = buttonCap.UsagePage,
                  UsageMin = buttonCap.Range.UsageMin,
                  UsageMax = buttonCap.Range.UsageMax
                }
            );
          }
          else
          {
            ret.Add(
              new ButtonItem
                {
                  TopLevelUsagePage = hidCaps.UsagePage,
                  ReportId = buttonCap.ReportID, 
                  UsagePage = buttonCap.UsagePage, 
                  Usage = buttonCap.NotRange.Usage
                }
            );
          }
        }
      }
      return ret;
    }

    private List<ValueBaseItem> ConvertToBaseItemList(NativeMethods.HidP_Value_Caps[] valueCaps)
    {
      var ret = new List<ValueBaseItem>();
      if (valueCaps != null)
      {
        foreach (var valueCap in valueCaps)
        {
          if (valueCap.IsRange)
          {
            ret.Add(
              new ValueRangeItem
              {
                // TODO
              }
            );
          }
          else
          {
            ret.Add(
              new ValueItem
              {
                // TODO
              }
            );
          }
        }
      }
      return ret;
    }

    #endregion

    #region IDisposable implementation

    /// <summary>
    /// Releasing Unmanaged Resources
    /// </summary>
    public void Dispose()
    {
      NativeMethods.HidD_FreePreparsedData(ref preparedData);

      fileStream.Close();
      fileStream = null;
      NativeMethods.CloseHandle(handle);
    }

    #endregion
  }
}