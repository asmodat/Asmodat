using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;

using System.Drawing;

using Asmodat.Extensions;
using System.Runtime.CompilerServices;


namespace Asmodat.Extensions.Collections.Generic
{
    

    public static partial class ArrayEx
    {
        /// <summary>
        /// null filled array is not empty array, only array with any dimention equal to 0 can be empty, becouse there can be no elements stored
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<TKey>(this TKey[,] source)
        {
            if (source == null)
                return true;
            
            int i = 0, r = source.Rank;
            for (; i < r; i++)
                if (source.GetLength(i) <= 0)
                    return true;

            return false;
        }


        public static bool IsNullOrEmpty<TKey>(this TKey[,,] source)
        {
            if (source == null)
                return true;

            int i = 0, r = source.Rank;
            for (; i < r; i++)
                if (source.GetLength(i) <= 0)
                    return true;

            return false;
        }


        public static void PopulateDepth<TKey>(this TKey[,,] source, int x, int y, TKey[] values)
        {
            if (source.IsNullOrEmpty() || values.IsNullOrEmpty())
                return;

            int 
                w = source.Width(), 
                h = source.Height(), 
                d = source.Depth(),
                offset = (y * w + x ) * d,
                l = Math.Min(d, values.Length);

            Buffer.BlockCopy(values, 0, source, offset, l);
         
            return;
        }

        public static void PopulateWidth<TKey>(this TKey[,,] source, int y, TKey[,] values)
        {
            if (source.IsNullOrEmpty() || values.IsNullOrEmpty())
                return;

            int 
                w = source.Width(), 
                h = source.Height(), 
                d = source.Depth(),
                offset = (y * w) * d,
                l = Math.Min(w * d, values.Length);

            Buffer.BlockCopy(values, 0, source, offset, l);
            return;
        }



        /// <summary>
        /// IEquitable required
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsDefaultOrEmpty<T>(this T[,] source) where T : IEquatable<T>
        {
            if (source == null)
                return true;

            T _default = default(T);
            int x, y = 0, w = source.Width(), h = source.Height();
            for (x = 0; x < w; x++)
                for (y = 0; y < h; y++)
                    if (EqualityComparer<T>.Default.Equals(source[x, y], _default))
                        return false;

            return true;
        }



        public static int CountEquatables<T>(this T[,] source, T value) where T : IEquatable<T>
        {
            if (source.IsNullOrEmpty())
                return 0;

            int x, y = 0, w = source.Width(), h = source.Height(), sum = 0;
            for (x = 0; x < w; x++)
                for (y = 0; y < h; y++)
                    if (EqualityComparer<T>.Default.Equals(source[x, y], value))
                        ++sum;

            return sum;
        }

        public static int CountEquatables<T>(this T[] source, T value) where T : IEquatable<T>
        {
            if (source.IsNullOrEmpty())
                return 0;

            int i = 0, l = source.Length, sum = 0;
            for (i = 0; i < l; i++)
                if (EqualityComparer<T>.Default.Equals(source[i], value))
                    ++sum;

            return sum;
        }



        public static bool AllClassesAreNull<TKey>(this TKey[,] source) where TKey : class
        {
            
            if (source == null)
                return false;

            int x, y = 0, w = source.Width(), h = source.Height();

            for (x = 0; x < w; x++)
                for (y = 0; y < h; y++)
                    if (source[x, y] != null)
                        return false;

            return true;
        }

        public static bool AllClassesAreNotNull<TKey>(this TKey[,] source) where TKey : class
        {
            if (source == null)
                return false;

            int x, y = 0, w = source.Width(), h = source.Height();
            for (x = 0; x < w; x++)
                for (y = 0; y < h; y++)
                    if (source[x, y] == null)
                        return false;
                
            return true;
        }






        public static void Populate<TKey>(this TKey[,] source, TKey value)
        {
            if (source.IsNullOrEmpty())
                return;

            int x = 0, y = 0, w = source.Width(), h = source.Height();
            for (x = 0; x < w; x++)
                for (y = 0; y < h; y++)
                    source[x, y] = value;
        }

        public static void Clear<TKey>(this TKey[,] source)
        {
            if (source.IsNullOrEmpty())
                return;

            int x, y = 0, w = source.Width(), h = source.Height();
            for (x = 0; x < w; x++)
            {
                for (y = 0; y < h; y++)
                {
                    source[x, y] = default(TKey);
                }
            }
        }


        public static void PopulateClone<TKey>(this TKey[,] source, TKey value) where TKey : ICloneable
        {
            if (source.IsNullOrEmpty())
                return;

            if (value == null)
            {
                source.Clear();
                return;
            }

            int x, y = 0, w = source.Width(), h = source.Height();
            for (x = 0; x < w; x++)
                for (y = 0; y < h; y++)
                    source[x, y] = (TKey)value.Clone();
        }


       


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T2[,] ToNewTypeArray<T1, T2>(this T1[,] source)
        {
            if (source == null)
                return null;

            return new T2[source.Width(), source.Height()];
        }

        public static TKey[,] Copy<TKey>(this TKey[,] source) where TKey : ICloneable
        {
            if (source == null)
                return null;

            int x, y = 0, w = source.Width(), h = source.Height();
            TKey[,] result = new TKey[w, h];

            for (x = 0; x < w; x++)
            {
                for (y = 0; y < h; y++)
                {
                    if(source[x, y] != null)
                        result[x, y] = (TKey)source[x, y].Clone();
                }
            }

            return result;
        }


        public static List<T> TryToList<T>(this T[] source)
        {
            try
            {
                if (source == null)
                    return null;
                else
                    return source.ToList();
            }
            catch
            {
                return null;
            }
        }

      
        /// <summary>
        /// Safely resizes array if neaded
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] ToSafeArray<T>(this T[] source, int length)
        {
            return source.TryToList().ToSafeArray(length);
        }


        public static T[] GetSafeRange<T>(this T[] source, int offset)
        {
            if (source == null) return null;

            if (offset < 0) offset = 0;
            if (offset >= source.Length) return new T[0];

            return source.TryToList().GetSafeRange(offset, source.Length - offset).TryToArray();
        }

        public static T[] GetSafeRange<T>(this T[] source, int offset, int count)
        {
            return source.TryToList().GetSafeRange(offset, count).TryToArray();
        }




        public static TKey[] SubArray<TKey>(this TKey[] source, int offset)
        {
            if (source == null || offset < 0) return null;

            return source.SubArray(offset, source.Length - offset);
        }

        public static TKey[] SubArray<TKey>(this TKey[] source, int offset, int count)
        {
            if (source == null || offset < 0 || count < 0 || (offset + count) > source.Length)
                return null;

            if (source.Length == 0 || count == 0)
                return new TKey[0];

            TKey[] result = new TKey[count];

            Array.Copy(source, offset, result, 0, count);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualSize<TKey>(this TKey[,] source, TKey[,] cmp)
        {
            if (source == null || cmp == null || source.Rank != cmp.Rank)
                return false;

            int i = 0, r = source.Rank;
            for (; i < r; i++)
            {
                if (source.GetLength(i) != cmp.GetLength(i))
                    return false;
            }

            return true;
        }

        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualSize<T1,T2>(this T1[,] source, T2[,] cmp)
        {
            if (source == null || cmp == null || source.Rank != cmp.Rank)
                return false;

            for (int i = 0; i < source.Rank; i++)
            {
                if (source.GetLength(i) != cmp.GetLength(i))
                    return false;
            }

            return true;
        }*/


        /// <summary>
        /// [width,height]
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Width<TKey>(this TKey[,] source)
        {
            if (source == null)
                return 0;

            return source.GetLength(0);
        }
        /// <summary>
        /// [width,height]
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Height<TKey>(this TKey[,] source)
        {
            if (source == null)
                return 0;

            return source.GetLength(1);
        }
        /// <summary>
        /// [width,height,depp]
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Width<TKey>(this TKey[,,] source)
        {
            if (source == null)
                return 0;

            return source.GetLength(0);
        }
        /// <summary>
        /// [width,height,depp]
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Height<TKey>(this TKey[,,] source)
        {
            if (source == null)
                return 0;

            return source.GetLength(1);
        }

        /// <summary>
        /// [width,height,depth]
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Depth<TKey>(this TKey[,,] source)
        {
            if (source == null)
                return 0;

            return source.GetLength(2);
        }


        public static List<TKey> GetList<TKey>(this TKey[] source)
        {
            if (source == null)
                return null;
            
            return new List<TKey>(source);
        }


        

        public static void Clear<TKey>(this TKey[] source)
        {
            if (source.IsNullOrEmpty())
                return;

            Array.Clear(source, 0, source.Length);
        }


        public static T[] Copy<T>(this T[] source)
        {
            return source.Copy(0);
        }


        public static T[] Copy<T>(this T[] source, int offset)
        {
            if (source == null)
                return null;

            int l = (source.Length - offset);

            if (l <= 0)
                return new T[0];

            T[] copy = new T[l];

            Array.Copy(source,offset,copy,0, l);

            return copy;
        }

        


    }
}


/*

     public static TKey[] AddDistinct<TKey>(this TKey[] source, TKey value)
        {
            if (value == null)
                return source;

            List<TKey> list;
            if (source.IsNullOrEmpty())
                list = new List<TKey>();
            else list = new List<TKey>(source);

            list.AddDistinct(value);

            return list.ToArray();
        }

        public static TKey[] AddRangeDistinct<TKey>(this TKey[] source, List<TKey> values)
        {
            if (values.IsNullOrEmpty())
                return source;

            List<TKey> list;
            if (source.IsNullOrEmpty())
                list = new List<TKey>();
            else list = new List<TKey>(source);

            foreach (var v in values)
                list.AddDistinct(v);

            return list.ToArray();
        }

        public static TKey[] AddRangeDistinct<TKey>(this TKey[] source, TKey[] values)
        {
            if (values.IsNullOrEmpty())
                return source;

            return source.AddRangeDistinct(values.ToList());
        }

*/
