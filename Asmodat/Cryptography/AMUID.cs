using Asmodat.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using AsmodatMath;
using Asmodat.Resources;
using Asmodat.Debugging;
using Asmodat.Extensions.Security.Cryptography;

namespace Asmodat.Cryptography
{

    /// <summary>
    /// Asmodat machine uniqe identifier
    /// </summary>
    public static class AMUID
    {

        public static string Identifier
        {
            get
            {
                try
                {
                    string format = string.Format("{0}{1}{2}",
                        DrivesInfo.SystemDriveID,
                        ProcessorsInfo.ProcessorId,
                        NetworkAdaptersInfo.MACAddress);

                    format = format.Replace(":", "");

                    return format.MD5_ComputeHashString();// AMD5.ComputeHash(format);
                }
                catch(Exception ex)
                {
                    ex.ToOutput();
                    return null;
                }
            }
        }
    }
}
