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

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

using System.Data.Entity;
using System.Data.Entity.Utilities;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;

using System.IO;

using Newtonsoft.Json;

namespace Asmodat.Abbreviate
{
    public static partial class Objects
    {
        /// <summary>
        /// Copies public properties, does not require serializable
        /// </summary>
        /// <param name="S"></param>
        /// <param name="T"></param>
        public static void CopyTo(this object S, object T)
        {
            foreach (var pS in S.GetType().GetProperties())
            {
                foreach (var pT in T.GetType().GetProperties())
                {
                    if (pT.Name != pS.Name) continue;
                    (pT.GetSetMethod()).Invoke(T, new object[] { pS.GetSetMethod().Invoke(S, null) });
                }
            }
        }

        /// <summary>
        /// Object Must be ICloneable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(T source)
        {
            if (Object.ReferenceEquals(source, null))
                return default(T);

            BinaryFormatter BFormatter = new BinaryFormatter();
            MemoryStream MStream = new MemoryStream();
            BFormatter.Serialize(MStream, source);
            MStream.Seek(0, SeekOrigin.Begin);

            return (T)BFormatter.Deserialize(MStream);
        }


        /// <summary>
        /// Cones object using json, look out for reference loops
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="LoopHandling"></param>
        /// <returns></returns>
        public static T CloneJson<T>(T source, ReferenceLoopHandling? LoopHandling = null)
        {
            if (Object.ReferenceEquals(source, null))
                return default(T);

            if (LoopHandling != null)
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = (ReferenceLoopHandling)LoopHandling }));
            else return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}
