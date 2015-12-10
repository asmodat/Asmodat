using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

using System.Drawing;
using System.Windows.Forms;

namespace Asmodat.Extensions.Windows.Forms
{


    public  static partial class controlEx
    {
        public static void SetFontStyle(this Control control, FontStyle style)
        {
            if (control == null)
                return;
            control.Font = new Font(control.Font, style);
        }

    }
}
