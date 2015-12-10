using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

using System.Collections.Concurrent;

using System.Timers;
using System.IO;

using System.Net;
using System.Net.Sockets;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

namespace Asmodat.Connect
{
    public partial class TcpAsyncCommon
    {
        /// <summary>
        /// Start Of Message indicator Tag
        /// </summary>
        public const string SOM = "";
        /// <summary>
        /// Enod Of Message indicator Tag
        /// </summary>
        public const string EOM = "\n";


        /// <summary>
        /// Default Uniqe Idenyfier Key
        /// </summary>
        public const string DefaultUID = "#DefaultID#";
    }
}
