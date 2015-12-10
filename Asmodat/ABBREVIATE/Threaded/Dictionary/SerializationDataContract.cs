using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;

using System.Threading;

using System.IO;
using Asmodat.Types;

using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Asmodat.Abbreviate
{
    /// <summary>
    /// This is Sorted, thread safe, xml serializable dictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial class ThreadedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {
    }
}
