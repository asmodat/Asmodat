using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asmodat.FormsControls
{
    public partial class LoginControl : UserControl
    {
        public LoginControl()
        {
            InitializeComponent();
            //TRTbxLoginMessage.Initialize(this);
            //TTbxUsername.Initialize(this);
            //TTbxPassword.Initialize(this);
        }

        public Button LoginButton
        {
            get
            {
                return this.BtnLogin;
            }
            set
            {
                this.BtnLogin = value;
            }
        }

        public string Username
        {
            get
            {
                return TTbxUsername.Text;
            }
            set
            {
                TTbxUsername.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return TTbxPassword.Text;
            }
            set
            {
                TTbxPassword.Text = value;
            }
        }


    }
}
