using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

using System.Windows.Forms;
using System.ComponentModel;
using Asmodat;

namespace Asmodat.FormsControls
{
    public partial class ThreadedComboBox : ComboBox
    {
        public void InitMode()
        {
            this.TextChanged += ThreadedComboBox_TextChanged;
        }

        void ThreadedComboBox_TextChanged(object sender, EventArgs e)
        {
            this.ModeTest();
        }

        public enum Mode
        {
            None = 0,
            Text = 1,
            Integer = 3,
            Double = 4,
            DateTime = 5,
        }

        private Mode _DisplayMode = Mode.Text;
        public Mode DisplayMode { get { return _DisplayMode; } set { _DisplayMode = value; } }

        public int IntegerDefault = 0;
        public int IntegerMin = int.MinValue;
        public int IntegerMax = int.MaxValue;

        public int Decimals = 0;
        public double DoubleDefault = 0;
        public double DoubleMin = double.MinValue;
        public double DoubleMax = double.MaxValue;

        public string Unit = "";

        public void SetDoubleMode(double Min = double.MinValue, double Max = double.MaxValue, double Default = 0, int Decimals = 2, string Unit = "")
        {
            DoubleMin = Min;
            DoubleMax = Max;
            DoubleDefault = Default;
            this.Decimals = Decimals;
            DisplayMode = Mode.Double;
            this.Unit = Unit;
            this.InitMode();
        }


        public bool IsNullOrEmpty(params string[] data)
        {
            foreach (string s in data)
                if (System.String.IsNullOrEmpty(s)) return true;

            return false;
        }

        public string GetTextValue
        {
            get
            {
                string text = this.Text;
                if (!this.IsNullOrEmpty(text, Unit))
                    text = text.Replace(Unit, "");

                return text;
            }
        }

        public void ModeTest()
        {
            string text = this.GetTextValue;

            switch (DisplayMode)
            {
                case Mode.Double:
                    {
                        double value;
                        try
                        {
                            value = double.Parse(text);
                            value = Math.Round(value, Decimals);

                            if (value > DoubleMax || value < DoubleMin)
                                value = DoubleDefault;
                        }
                        catch { value = DoubleDefault; }

                        this.Text = value + Unit;
                    };
                    break;
                default: return;

            }
        }
    }
}
