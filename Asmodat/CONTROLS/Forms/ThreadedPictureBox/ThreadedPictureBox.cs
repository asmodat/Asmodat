using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Windows.Forms;
using System.ComponentModel;
using Asmodat;
using Asmodat.Types;
using Asmodat.Extensions.Windows.Forms;
using System.Drawing;
using Asmodat.Extensions.Drawing;

namespace Asmodat.FormsControls
{
    public partial class ThreadedPictureBox : PictureBox
    {

        /// <summary>
        /// By default 50ms / 20fps
        /// </summary>
        public TickTimeout UpdateTimeout { get; set; } = new TickTimeout(50, TickTime.Unit.ms);

        private Control _Invoker = null;
        public Control Invoker
        {
            get
            {
                if (_Invoker == null)
                    _Invoker = this.GetFirstParent();

                return _Invoker;
            }
        }

        public new void Update()
        {
            Invoker.TryInvokeMethodAction(() => { base.Update(); });
        }


        public new int Width
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Width; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.Width = value; });
            }
        }

        public new int Height
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Height; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.Height = value; });
            }
        }

        public new Image Image
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Image; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.Image = value; });
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

                this.Image = value.AForge_ResizeFast(this.Width, this.Height);
            }
        }
    }
}
