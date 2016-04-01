using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using System.Data;

using System.Windows.Forms;

using System.Collections;

using System.Diagnostics;

using System.Globalization;

using System.Linq.Expressions;
using Asmodat.Debugging;

namespace Asmodat.Abbreviate
{
    public static partial class Objects
    {

        public static string fullname<T>(Expression<Func<T>> labda)
        {
            MemberExpression MExpression = labda.Body as MemberExpression;
            if (MExpression == null) throw new ArgumentException("You must bas lambde of the form: '()=> Object'");


            return MExpression.Member.DeclaringType.FullName + "." + MExpression.Member.Name;
        }

        public static string nameof(object source)
        {
            if (source == null) return null;

            Type TSource = source.GetType();
            PropertyInfo PInfo = TSource.GetProperty("Name", typeof(string));

            if (PInfo == null) return null;

            object oValue = PInfo.GetValue(source, null);

            if (oValue == null) return null;

            return oValue.ToString();
        }

        public static string nameofcaller(int deep = 2)
        {
            StackFrame SFrame = new StackFrame(1);
            return SFrame.GetMethod().Name;
        }


        public static string nameofmethod(int stackDeep = 2)
        {
            try
            {
                StackFrame SFrame = new StackFrame(stackDeep);
                return SFrame.GetMethod().Name;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// This statioc method returns property name of property passed by lambda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="labda"></param>
        /// <returns></returns>
        public static string nameofproperty<T>(Expression<Func<T>> labda)
        {
            MemberExpression MExpression = labda.Body as MemberExpression;
            if (MExpression == null) throw new ArgumentException("You must bas lambde of the form: '()=> Class.Property'");

            string result = string.Empty;
            do
            {
                result = MExpression.Member.Name + "." + result;
                MExpression = MExpression.Expression as MemberExpression;

            } while (System.String.IsNullOrEmpty(result));


            return result.Remove(result.Length - 1);
        }

        /// <summary>
        /// This can be used to return name of parameter, Example:
        /// int x = 1;
        /// string foo(int coordinate) { return nameofmember(()=>coordinate); }
        /// void foo2() { if(foo(x) != "x") throw new Exception("this must be x...")}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static string nameofmember<T>(Expression<Func<T>> lambda)
        {
            MemberExpression MExpression = (MemberExpression)lambda.Body;

            return MExpression.Member.Name;
        }

    }
}
