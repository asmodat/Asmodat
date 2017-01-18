using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using AsmodatMath;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions;

namespace Asmodat.Cryptography
{
    /// <summary>
    /// Asmodat One To One Hex Encryption
    /// this class generates  hex shifted by random number of size N+1 using provided hex string of size N and vice versa
    /// </summary>
    public class A1T1X
    {
        private char[] dictionary = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        private Dictionary<char, int> translator = new Dictionary<char, int>()
        {
            {'0', 0 },
            {'1', 1 },
            {'2',  2},
            {'3',  3},
            {'4',  4},
            {'5',  5},
            {'6',  6},
            {'7',  7},
            {'8',  8},
            {'9',  9},
            {'a',  10},
            {'b',  11},
            {'c',  12},
            {'d',  13},
            {'e',  14},
            {'f',  15}
        };

        public string Encrypt(string hex)
        {

            int seed = AMath.Random(1, 16);
            hex = dictionary[seed] + hex;

            for (int i = 1; i < hex.Length; i++)
                hex = hex.SetSafe(dictionary[(translator[hex[i]] + seed) % 16], i);

            return hex;
        }

        public string Decrypt(string str)
        {
            if (str == null || str.Length <= 1)
                return null;

            int seed = translator[str[0]];
            
            for (int i = 1; i < str.Length; i++)
                str = str.SetSafe(dictionary[AMath.Mod((translator[str[i]] - seed), 16)], i);

            return str.Substring(1, str.Length - 1);
        }
    }
}
