using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

using System.Windows.Forms;
using System.ComponentModel;
using Asmodat;
using System.Runtime.InteropServices;
using System.Drawing;
using Asmodat.Types;

using Asmodat.Extensions.Drawing;

namespace Asmodat.FormsControls
{
    public partial class ThreadedPictureBox : PictureBox
    {

        public Control Invoker { get; private set; }


        public new void Update()
        {
            if (Invoker == null || !this.IsHandleCreated)
            {
                base.Update();
                return;
            }

            Invoker.Invoke((MethodInvoker)(() =>
            {
                base.Update();
            }));
        }

        public new int Width
        {
            get
            {
                if (Invoker == null || !this.IsHandleCreated)
                    return base.Width;

                int var = -1;

                try
                {
                    Invoker.Invoke((MethodInvoker)(() =>
                {
                    var = base.Width;
                }));
                }
                catch
                {

                }

                return var;
            }
            set
            {
                if (Invoker == null || !this.IsHandleCreated) base.Width = value;
                else
                    try
                    {
                        Invoker.Invoke((MethodInvoker)(() =>
                        {

                            base.Width = value;

                        }));
                    }
                    catch
                    {
                    }
            }
        }

        public new int Height
        {
            get
            {
                if (Invoker == null || !this.IsHandleCreated) return base.Height;

                int var = -1;

                Invoker.Invoke((MethodInvoker)(() =>
                {
                    var = base.Height;
                }));

                return var;
            }
            set
            {
                if (Invoker == null || !this.IsHandleCreated) base.Height = value;
                else
                    try
                    {
                        Invoker.Invoke((MethodInvoker)(() =>
                        {

                            base.Height = value;

                        }));
                    }
                    catch
                    {
                    }
            }
        }

        public new Image Image
        {
            get
            {
                if (Invoker == null || !this.IsHandleCreated) return base.Image;

                Image var = null;

                Invoker.Invoke((MethodInvoker)(() =>
                {
                    var = base.Image;
                }));

                return var;
            }
            set
            {
                if (!UpdateTimeout.IsTriggered)
                    return;

                try
                {

                    if (Invoker == null || !this.IsHandleCreated)
                    {
                        if (base.Image != null)
                            base.Image.Dispose();

                        base.Image = value;
                    }
                    else
                        try
                        {
                            Invoker.Invoke((MethodInvoker)(() =>
                            {
                                if (base.Image != null)
                                    base.Image.Dispose();

                                base.Image = value;

                            }));
                        }
                        catch
                        {
                        }
                }
                finally
                {
                    UpdateTimeout.Reset();
                }

            }
        }


        public Bitmap Bitmap
        {
            get
            {

                return (Bitmap)this.Image;
            }
            set
            {
                if (!UpdateTimeout.IsTriggered)
                    return;

                this.Image = value.AForge_ResizeFast(this.Width, this.Height);// Image.FromHbitmap(value.AForge_ResizeFast(this.Width, this.Height).GetHbitmap());//.Format(System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
        }
    }
}
