using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using System.Diagnostics;
using System.Reflection;

using System.Linq.Expressions;

using System.Windows.Forms;

using System.Collections.Concurrent;

namespace Asmodat.Abbreviate
{
    public static class Expressions
    {
        public static string nameofDeclaringType(Expression<Action> EAMethod)
        {
            if (EAMethod == null) return null;
            return ((MethodCallExpression)EAMethod.Body).Method.DeclaringType.FullName;
        }

        public static string nameofMethod(Expression<Action> EAMethod)
        {
            if (EAMethod == null) return null;
            return ((MethodCallExpression)EAMethod.Body).Method.Name;
        }

        public static string nameofFull(Expression<Action> EAMethod)
        {
            if (EAMethod == null) return null;

            MethodInfo Info = ((MethodCallExpression)EAMethod.Body).Method;
           
            return Info.DeclaringType.FullName + "." + Info.Name;
        }

        public static string nameofFull<TResult>(Expression<Func<TResult>> expression)
        {
            if (expression == null) return null;

            MethodInfo Info = ((MethodCallExpression)expression.Body).Method;

            return Info.DeclaringType.FullName + "." + Info.Name;
        }


        public static TReference GetReference<TResult, TReference>(Expression<Func<TResult>> expression)
        {
            Expression exp = expression.Body;

            Stack<MemberInfo> SMInfo = new Stack<MemberInfo>();

            while (exp is MemberExpression)
            {
                var memeber = exp as MemberExpression;
                SMInfo.Push(memeber.Member);
                exp = memeber.Expression;
            }


            var constant = exp as ConstantExpression;
            var reference = constant.Value;

            if (reference is TReference)
                return (TReference)reference;

            else throw new Exception("Not found");
        }
    }
}
/*
 

        private object GetReference<TResult>(Expression<Func<TResult>> expression)
        {
            Expression exp = expression.Body;

            Stack<MemberInfo> SMInfo = new Stack<MemberInfo>();

            while (exp is MemberExpression)
            {
                var memeber = exp as MemberExpression;
                SMInfo.Push(memeber.Member);
                exp = memeber.Expression;
            }


            var constant = exp as ConstantExpression;
            var reference = constant.Value;


            //while (SMInfo.Count > 0)
            //{
            //    var info = SMInfo.Pop();
            //    if (info.MemberType == MemberTypes.Property)
            //        reference = reference.GetType().GetProperty(info.Name).GetValue(reference, null);
            //    else if (info.MemberType == MemberTypes.Field)
            //        reference = reference.GetType().GetField(info.Name).GetValue(reference);
            //}


            return reference;
        }

 */