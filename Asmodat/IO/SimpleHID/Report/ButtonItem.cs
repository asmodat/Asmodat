// This source is subject to Microsoft Public License (Ms-PL).
// Please see http://simplehidlibrary.codeplex.com/ for details.
// All other rights reserved.

namespace SimpleHID.Report
{
  /// <summary>
  /// Class describen an On/Off HID item
  /// </summary>
  public class ButtonItem : ButtonBaseItem
  {
    /// <summary>
    /// The Local Usage item is the Usage ID that works together with the Global
    /// Usage Page to describe the function of a control, data, or collection.
    /// </summary>
    public short Usage { get; set; }
  }
}
