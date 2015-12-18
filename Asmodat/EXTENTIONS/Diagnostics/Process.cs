using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Asmodat.Extensions.Diagnostics
{
    public static partial class ProcessEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull(this Process process)
        {
            if (process == null)
                return true;
            else
                return false;
        }

        public static string GetStandardOutput(this Process process)
        {
            if (process.IsNull())
                return null;

            string result = string.Empty;
            using (StreamReader SReader = process.StandardOutput)
                result = SReader.ReadToEnd();

            return result;
        }

        public static string GetStandardError(this Process process)
        {
            if (process.IsNull())
                return null;

            string result = string.Empty;
            using (StreamReader SReader = process.StandardError)
                result = SReader.ReadToEnd();

            return result;
        }


        /// <summary>
        /// Reurns application standard output or error
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static string GetStandardResult(this Process process)
        {
            if (process.IsNull())
                return null;


            string result = process.GetStandardOutput();

            if (result.IsNullOrEmpty())
                result = process.GetStandardError();

            return result;
        }


    }
}
