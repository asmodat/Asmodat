﻿using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Asmodat.WPFControls
{
    public partial class ThreadedLabelledProgressBar : UserControl
    {
        public ThreadedLabel Label { get { return this.TL; } set { this.TL = value; } }


    }
}
