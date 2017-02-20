using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Extensions.Objects;

namespace Asmodat.Extensions
{
    

    public static class TryCatchEx
    {
        /// <summary>
        /// Try-catch oneliner block
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_func">Function to be executed</param>
        /// <param name="_default">Default value to be returned if funcion throws exception</param>
        /// <returns></returns>
        public static T TryFunc<T>(this Func<T> _func, T _default = default(T))
        {
            try
            {
               return _func();
            }
            catch
            {
                return _default;
            }
        }

    /*   
      public static (int a, int b) Foo()
        {
            return (1,2);
        }//*/

        public static Tuple<T, Exception> TryFuncExc<T>(this Func<T> _func, T _default = default(T))
        {
            try
            {// 
                return Tuple.Create<T, Exception>(_func(), null); 
            }
            catch(Exception ex)
            {
                return Tuple.Create<T, Exception>(_default, ex);
            }
        }


        public static bool TryAction(this Action _action)
        {
            try
            {
                _action();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Exception TryActionExc(this Action _action)
        {
            try
            {
                _action();
                return null;
            }
            catch(Exception ex)
            {
                return ex;
            }
        }

    }
}
