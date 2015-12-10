using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using System.Drawing;

using System.Reflection;

using System.Data;

using AsmodatMath;

namespace Asmodat.Abbreviate
{
    public partial class FormsControls
    {
        /// <summary>
        /// Colours textbox RGB depending on value
        /// </summary>
        /// <param name="Tbx"></param>
        /// <param name="value"></param>
        /// <param name="exception"></param>
        /// <param name="decimals"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="punctuation"></param>
        public static void ColourDouble(ref TextBox Tbx, double value, string exception, int decimals, double min, double max, char punctuation, double precision = 0)
        {
            

            Tbx.Text = Doubles.ToString(value, exception, decimals, min, max, punctuation);

            if (Tbx.Text == exception)
                Tbx.ForeColor = Color.Black;
            else if (AMath.Equ(value, 0, precision))
                Tbx.ForeColor = Color.DarkBlue;
            else if (value < 0)
                Tbx.ForeColor = Color.DarkRed;
            else if (value > 0)
                Tbx.ForeColor = Color.DarkGreen;
        }

        /// <summary>
        /// Colours texbox RGB depending on values comparison and assigns v1
        /// </summary>
        /// <param name="Tbx"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="exception"></param>
        /// <param name="decimals"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="punctuation"></param>
        public static void ColourDouble(ref TextBox Tbx, double v1, double v2, string exception, int decimals, double min, double max, char punctuation, double precision = 0)
        {
            FormsControls.ColourDouble(ref  Tbx, v1, v1, v2, exception, decimals, min, max, punctuation, precision);
            Tbx.Text = Doubles.ToString(v1, exception, decimals, min, max, punctuation);
        }

        /// <summary>
        /// Colours texbox RGB depending on cmp1,cmp2 comparison and assigns value
        /// </summary>
        /// <param name="Tbx"></param>
        /// <param name="value"></param>
        /// <param name="cmp1"></param>
        /// <param name="cmp2"></param>
        /// <param name="exception"></param>
        /// <param name="decimals"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="punctuation"></param>
        public static void ColourDouble(ref TextBox Tbx, double value, double cmp1, double cmp2, string exception, int decimals, double min, double max, char punctuation, double precision = 0)
        {
            Tbx.Text = Doubles.ToString(value, exception, decimals, min, max, punctuation);

            

            if (Tbx.Text == exception)
                Tbx.ForeColor = Color.Black;
            else if (AMath.Equ(cmp1, cmp2, precision))
                Tbx.ForeColor = Color.DarkBlue;
            else if (cmp1 < cmp2)
                Tbx.ForeColor = Color.DarkRed;
            else if (cmp1 > cmp2)
                Tbx.ForeColor = Color.DarkGreen;
        }

    }
}
