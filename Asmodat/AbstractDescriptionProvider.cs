using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace Asmodat
{
    /// <summary>
    /// This clase can be used to firce compile to use abstract class
    /// 
    /// using Asmodat
    /// using System.ComponentModel;
    /// [TypeDescriptionProvider(typeof(AbstractDescriptionProvider>AbstractControl,UserControl>))]
    /// public abstract partal SomeClass...
    /// 
    /// 
    /// This class can solve problems like: The variable is either undeclared or was never assigned.
    /// </summary>
    /// <typeparam name="TAbstract"></typeparam>
    /// <typeparam name="TBase"></typeparam>
    public class AbstractDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        public AbstractDescriptionProvider() : base(TypeDescriptor.GetProvider(typeof(TAbstract))) { }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if(objectType == typeof(TAbstract))
                return typeof(TBase);

            return base.GetReflectionType(objectType, instance);
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(TAbstract))
                objectType = typeof(TBase);

            return base.CreateInstance(provider, objectType, argTypes, args);
        }

    }
}
