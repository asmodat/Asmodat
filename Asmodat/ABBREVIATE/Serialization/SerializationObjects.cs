using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat;

using System.Reflection;

using System.Data;

using System.Windows.Forms;

using System.Collections;

namespace Asmodat.Abbreviate
{
    public static class SerializationObjects
    {
        private static string sNodeKeyID = "<Asmodat Node Key ID>";
        
        public static Dictionary<object, object> ExtractDictionary(object source, Dictionary<Type, List<string>> properties, int deep = int.MaxValue, bool invoked = false)
        {
            
            Type TSource = source.GetType();
            string sSourceName = Asmodat.Abbreviate.Objects.nameof(source);

            Dictionary<object, object> DO2Data = new Dictionary<object, object>();
            if (sSourceName == null)
                return null;

            Dictionary<object, object> DPIOProperties = Objects.GetProperties(source, false);
            if (DPIOProperties == null || DPIOProperties.Count <= 0 || deep <= 0) return null;

            foreach (var v in DPIOProperties)
            {
                if (v.Value == null) continue;
                Type TValue = v.Value.GetType();

                if (properties.ContainsKey(TSource) && properties[TSource].Contains(v.Key))
                {
                    DO2Data.Add(v.Key, v.Value);
                    DO2Data.Add((v.Key + sNodeKeyID), null);
                }
                else if (deep > 1)
                {
                    if (v.Value is IEnumerable && v.Value is ICollection)
                    {
                        var vSubZeroData = new Dictionary<object, object>();

                        var vOEnum = (v.Value as IEnumerable);
                        foreach (object o in vOEnum)
                        {
                            var vSubData = ExtractDictionary(o, properties, deep - 1);

                            if (vSubData != null && vSubData.Count > 0)
                                vSubZeroData.Add(Objects.nameof(o), vSubData);
                        }

                        if (vSubZeroData != null && vSubZeroData.Count > 0)
                            DO2Data.Add(v.Key, vSubZeroData);
                    }

                    var vSubOneData = ExtractDictionary(v.Value, properties, deep - 1);
                    if (vSubOneData != null && vSubOneData.Count > 0)
                        DO2Data.Add(v.Key, vSubOneData);
                }
            }


            if (DO2Data != null && DO2Data.Count > 0)
                return DO2Data;
            else return null;
        }


        public static void InjectDictionary(Dictionary<object, object> source, object destination, bool invoked = false)
        {
            foreach (var v in source)
            {


                object obj = Objects.GetProperty(destination, v.Key.ToString(), true, invoked);
                if (obj != null)
                {
                    if (v.Value is IDictionary)
                        InjectDictionary((Dictionary<object, object>)v.Value, obj, invoked);
                    else
                        Objects.SetProperty(obj, v.Key.ToString(), v.Value, false, invoked);
                }
                else if (destination is ICollection && destination is IEnumerable)
                {

                    var vOEnum = (destination as IEnumerable);
                    foreach (object o in vOEnum)
                    {
                        string sNameof = Objects.nameof(o);
                        if (sNameof != v.Key.ToString()) continue;
                        if (v.Value is IDictionary)
                        {
                            Dictionary<object, object> DO2VVal = (Dictionary<object, object>)v.Value;
                            foreach (var v2 in DO2VVal)
                            {
                                if(DO2VVal.ContainsKey(v2.Key + sNodeKeyID))
                                    Objects.SetProperty(o, v2.Key.ToString(), v2.Value, false, invoked);
                                else if(v2.Value is IDictionary) 
                                    InjectDictionary((Dictionary<object, object>)v2.Value, o, invoked);
                            }
                        }
                    }
                }
            }
        }
    }
}
