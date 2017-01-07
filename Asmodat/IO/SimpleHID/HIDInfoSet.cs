// This source is subject to Microsoft Public License (Ms-PL).
// Please see http://simplehidlibrary.codeplex.com/ for details.
// All other rights reserved.

namespace SimpleHID
{
  /// <summary>
  /// Class containg information about a connected USB HID device
  /// </summary>
  public class HIDInfoSet
  {
    /// <summary>
    /// Identifies the manufacturer's product
    /// </summary>
    public string ProductString { get; private set; }

    /// <summary>
    /// Serial number
    /// </summary>
    public string SerialNumberString { get; private set; }

    /// <summary>
    /// Full device file path
    /// </summary>
    public string DevicePath { get; private set; }

    /// <summary>
    /// Vendor ID
    /// </summary>
    public short Vid { get; private set; }

    /// <summary>
    /// Product ID
    /// </summary>
    public short Pid { get; private set; }

    /// <summary>
    /// Product Version
    /// </summary>
    public short Version { get; private set; }

    /// <summary>
    /// ctor
    /// </summary>
    internal HIDInfoSet(string productString, string serialNumberString, string devicePath, short vid, short pid, short version)
    {
      ProductString = productString;
      SerialNumberString = serialNumberString;
      DevicePath = devicePath;
      Vid = vid;
      Pid = pid;
      Version = version;
    }
  }
}
