// This source is subject to Microsoft Public License (Ms-PL).
// Please see http://simplehidlibrary.codeplex.com/ for details.
// All other rights reserved.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SimpleHID
{
  /// <summary>
  /// Specialized Exception
  /// </summary>
  public class HIDDeviceException : Exception
  {
    /// <summary>
    /// 
    /// </summary>
    public int LastWin32Error { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public string Win32ErrorMessage { get; private set; }

    /// <summary>
    /// ctor
    /// </summary>
    public HIDDeviceException()
    {
      LastWin32Error = Marshal.GetLastWin32Error();
      Win32ErrorMessage = new Win32Exception(LastWin32Error).Message;
    }
  }
}
