// This source is subject to Microsoft Public License (Ms-PL).
// Please see http://simplehidlibrary.codeplex.com/ for details.
// All other rights reserved.

namespace SimpleHID.Report
{
  /// <summary>
  /// Abstract base class
  /// </summary>
  public abstract class ButtonBaseItem
  {
    /// <summary>
    /// 
    /// </summary>
    public short TopLevelUsagePage { get; set; }

    /// <summary>
    /// The HID Usage Tables document defines many Usage Pages. There are Usage
    /// Pages for common device types including generic desktop controls (mouse, keyboard,
    /// joystick), digitizer, barcode scanner, camera control, and various game
    /// controls. A vendor can define Usage Pages using values from FF00h to FFFFh.
    /// </summary>
    public short UsagePage { get; set; }

    /// <summary>
    /// A HID can support multiple reports of the same type, with each
    /// report having its own Report ID and contents. This way, each report doesn’t
    /// have to include every piece of data. Sometimes the simplicity of using a single
    /// report outweighs the need to reduce the bandwidth used by longer reports,
    /// however.
    /// </summary>
    public byte ReportId { get; set; }
  }
}
