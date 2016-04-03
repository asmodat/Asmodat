using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using System.Windows.Forms;
using Asmodat.Debugging;

namespace Asmodat.Extensions.Windows.Forms
{


    public  static partial class KeyEventArgsEx
    {
        public static bool IsEscape(this KeyEventArgs e) { return(e != null && e.KeyCode == Keys.Escape) ? true : false; }
        public static bool IsEnter(this KeyEventArgs e) { return (e != null && e.KeyCode == Keys.Enter) ?  true : false; }
        public static bool IsDelete(this KeyEventArgs e) { return (e != null && e.KeyCode == Keys.Delete) ? true : false; }

        public static bool IsDown(this KeyEventArgs e) { return (e != null && e.KeyCode == Keys.Down) ? true : false; }
        public static bool IsUp(this KeyEventArgs e) { return (e != null && e.KeyCode == Keys.Up) ? true : false; }
        public static bool IsLeft(this KeyEventArgs e) { return (e != null && e.KeyCode == Keys.Left) ? true : false; }
        public static bool IsRight(this KeyEventArgs e) { return (e != null && e.KeyCode == Keys.Right) ? true : false; }
        public static bool IsBack(this KeyEventArgs e) { return (e != null && e.KeyCode == Keys.Back) ? true : false; }

        
    }
}
