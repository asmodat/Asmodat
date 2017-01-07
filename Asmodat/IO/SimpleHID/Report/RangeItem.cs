// This source is subject to Microsoft Public License (Ms-PL).
// Please see http://simplehidlibrary.codeplex.com/ for details.
// All other rights reserved.

namespace SimpleHID.Report
{
  /// <summary>
  /// Class describen an HID item with range capabilities
  /// </summary>
  public class ButtonRangeItem : ButtonBaseItem
  {
    /// <summary>
    /// TODO
    /// </summary>
    public short UsageMin { get; set; }

    /// <summary>
    /// TODO
    /// </summary>
    public short UsageMax { get; set; }
  }
}
