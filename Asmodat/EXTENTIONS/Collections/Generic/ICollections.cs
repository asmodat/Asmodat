using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

using System.Drawing;

namespace Asmodat.Extensions.Collections.Generic
{
    

    public static class ICollectionsEx
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }


        


    }
}
/*public static ICollection<T> TryGetSubCollection<T>(this ICollection<T> source, int offset)
        {
            if (source == null) return null;

            if (offset < 0) offset = 0;
            if (offset >= source.Count) new List<T>();

            return source.TryGetSubCollection(offset, source.Count - offset);
        }

        public static ICollection<T> TryGetSubCollection<T>(this ICollection<T> source, int offset, int length)
        {
            if (source == null) return null;

            if (offset < 0) offset = 0;
            if (offset >= source.Count) new List<T>();
            if (offset + length > source.Count) length = source.Count - offset;


            source.ra
            return (ICollection<T>)source.Skip(offset).Take(length);
        }
        */
