using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;

using System.Windows.Forms;
using System.ComponentModel;
using Asmodat;
using System.Drawing;

namespace Asmodat.FormsControls 
{
    public partial class ThreadedComboBox : ComboBox
    {
        public enum TextAlignType
        {
            Left = 0,
            Center = 1,
            Right = 2
        }

        public bool EnableAlignStyleColors { get; set; } = false;
        public bool EnableTextAlign { get; set; } = false;
        public TextAlignType TextAlign { get; set; } = TextAlignType.Center;


        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            string text = string.Empty;

            if (e.Index >= 0)
                text = this.Items[e.Index].ToString();

            TextFormatFlags flags;
            switch(this.TextAlign)
            {
                case TextAlignType.Left:
                    flags = TextFormatFlags.Left;
                    break;
                case TextAlignType.Center:
                    flags = TextFormatFlags.HorizontalCenter;
                    break;
                case TextAlignType.Right:
                    flags = TextFormatFlags.Right;
                    break;
                default:
                    throw new Exception("ThreadedComboBox udefined Align property.");
            }

            Color back = e.BackColor;
            Color fore = e.ForeColor;
            if (EnableAlignStyleColors)
            {
                if (this.DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    back = SystemColors.ControlLight;
                    fore = e.ForeColor;
                }

                if (this.DroppedDown)
                    back = SystemColors.ControlDark;
                else if (this.Focused)
                    fore = Color.Black;
            }

            e.Graphics.FillRectangle(new SolidBrush(back), e.Bounds);// );
            TextRenderer.DrawText(e.Graphics, text, e.Font, e.Bounds, fore, back, flags);
            e.DrawFocusRectangle();
        }
        
    }
}


/* if(e.Index == this.SelectedIndex && !this.DroppedDown)
              {
                  using (Brush back = new SolidBrush(SystemColors.Control))
                  {
                      Color c = this.ForeColor;
                      using (StringFormat format = new StringFormat())
                      {
                          using (Brush reverse = new SolidBrush(Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B)))
                          {
                              e.Graphics.FillRectangle(back, e.Bounds);
                              TextRenderer.DrawText(e.Graphics, text, e.Font, e.Bounds, e.ForeColor, flags);
                              //e.Graphics.DrawString(text, this.Font, reverse, e.Bounds, format);
                              e.DrawFocusRectangle();
                          }
                      }
                  }
              }*/


/*
public void AddItems(bool append = true, int index = 0, params string[] items) 
        {
            if (items.IsNullOrEmpty()) return;
            int index_Save = 0;

            if(this.IsHandleCreated)
            Invoker.Invoke((MethodInvoker)(() =>
            {
                index_Save = this.SelectedIndex;
                var item = this.SelectedItem;

                bool equals = Objects.EqualsItems(items, this.Items.Cast<object>().ToArray());

                if (!equals)
                {
                    if (!append) this.Items.Clear();

                    this.Items.AddRange(items);

                    if (index >= 0 && index < this.Items.Count)
                        this.SelectedIndex = index;
                    else if(index < 0 && index_Save < this.Items.Count)
                        this.SelectedIndex = index_Save;
                }
            }));


            
        }
*/
