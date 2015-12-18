using Asmodat.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using AsmodatMath;
using Asmodat.Debugging;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Security.Cryptography;

namespace Asmodat.Cryptography
{

    /// <summary>
    /// Asmodat uniqe identifier
    /// </summary>
    public static class AUID
    {

        public static string NewString(int length)
        {

            try
            {
                string guid = "";

                while (guid.Length < length)
                {
                    byte[] ba = Guid.NewGuid().ToByteArray();
                    ulong[] ula = ba.ToULongArray();

                    ulong result = 0;
                    if (ula[0] < ulong.MaxValue / 2 && ula[1] < ulong.MaxValue / 2)
                        result = ula[0] + ula[1];
                    else
                        result = ula[1];

                    guid += result.ToArbitrary();
                }

                return guid.Substring(0, length);
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                return null;
            }

        }

        public static string NewString()
        {
            
                try
                {
                    TickTime start = TickTime.Now;
                    string time = TickTime.NowTicks.ToString("X");
                    string random = AMath.Random(1, 1000000).ToString("X");
                    string span = start.Span().ToString("X");

                    return string.Format("{0}{1}{2}", time, random, span);
                }
                catch(Exception ex)
                {
                    ex.ToOutput();
                    return null;
                }
            
        }



        public static string NewStringMD5()
        {
            
                try
                {
                    return AUID.NewString().MD5_ComputeHashString().Replace("-", "");// AMD5.ComputeHash(AUID.NewString()).Replace("-","");
                }
                catch (Exception ex)
                {
                    ex.ToOutput();
                    return null;
                }
            
        }
    }
}
