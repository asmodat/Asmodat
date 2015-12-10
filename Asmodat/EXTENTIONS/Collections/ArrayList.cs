using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Extensions.Collections
{
    

    public static class ArrayListEx
    {
        public static bool IsNullOrEmpty(this ArrayList source)
        {
            if (source == null || source.Count <= 0)
                return true;
            
            return false;
        }




    }
}
