namespace Asmodat.FormsControls
{
    partial class LoginControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TbCntrlMain = new System.Windows.Forms.TabControl();
            this.TbPageLogin = new System.Windows.Forms.TabPage();
            this.TbPgChangePassword = new System.Windows.Forms.TabPage();
            this.PnlLogin = new System.Windows.Forms.Panel();
            this.GpbxLoginMessage = new System.Windows.Forms.GroupBox();
            this.BtnLogin = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TTbxPassword = new Asmodat.FormsControls.ThreadedTextBox();
            this.TTbxUsername = new Asmodat.FormsControls.ThreadedTextBox();
            this.TRTbxLoginMessage = new Asmodat.FormsControls.ThreadedRichTextBox();
            this.TbCntrlMain.SuspendLayout();
            this.TbPageLogin.SuspendLayout();
            this.PnlLogin.SuspendLayout();
            this.GpbxLoginMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // TbCntrlMain
            // 
            this.TbCntrlMain.Controls.Add(this.TbPageLogin);
            this.TbCntrlMain.Controls.Add(this.TbPgChangePassword);
            this.TbCntrlMain.Location = new System.Drawing.Point(3, 3);
            this.TbCntrlMain.Name = "TbCntrlMain";
            this.TbCntrlMain.SelectedIndex = 0;
            this.TbCntrlMain.Size = new System.Drawing.Size(594, 394);
            this.TbCntrlMain.TabIndex = 0;
            // 
            // TbPageLogin
            // 
            this.TbPageLogin.Controls.Add(this.PnlLogin);
            this.TbPageLogin.Location = new System.Drawing.Point(4, 25);
            this.TbPageLogin.Name = "TbPageLogin";
            this.TbPageLogin.Padding = new System.Windows.Forms.Padding(3);
            this.TbPageLogin.Size = new System.Drawing.Size(586, 365);
            this.TbPageLogin.TabIndex = 0;
            this.TbPageLogin.Text = "Login";
            this.TbPageLogin.UseVisualStyleBackColor = true;
            // 
            // TbPgChangePassword
            // 
            this.TbPgChangePassword.Location = new System.Drawing.Point(4, 25);
            this.TbPgChangePassword.Name = "TbPgChangePassword";
            this.TbPgChangePassword.Padding = new System.Windows.Forms.Padding(3);
            this.TbPgChangePassword.Size = new System.Drawing.Size(586, 365);
            this.TbPgChangePassword.TabIndex = 1;
            this.TbPgChangePassword.Text = "Change Password";
            this.TbPgChangePassword.UseVisualStyleBackColor = true;
            // 
            // PnlLogin
            // 
            this.PnlLogin.BackColor = System.Drawing.SystemColors.Control;
            this.PnlLogin.Controls.Add(this.TTbxPassword);
            this.PnlLogin.Controls.Add(this.TTbxUsername);
            this.PnlLogin.Controls.Add(this.GpbxLoginMessage);
            this.PnlLogin.Controls.Add(this.BtnLogin);
            this.PnlLogin.Controls.Add(this.label2);
            this.PnlLogin.Controls.Add(this.label1);
            this.PnlLogin.Location = new System.Drawing.Point(6, 6);
            this.PnlLogin.Name = "PnlLogin";
            this.PnlLogin.Size = new System.Drawing.Size(574, 353);
            this.PnlLogin.TabIndex = 0;
            // 
            // GpbxLoginMessage
            // 
            this.GpbxLoginMessage.Controls.Add(this.TRTbxLoginMessage);
            this.GpbxLoginMessage.Location = new System.Drawing.Point(10, 12);
            this.GpbxLoginMessage.Name = "GpbxLoginMessage";
            this.GpbxLoginMessage.Size = new System.Drawing.Size(552, 109);
            this.GpbxLoginMessage.TabIndex = 11;
            this.GpbxLoginMessage.TabStop = false;
            this.GpbxLoginMessage.Text = "Message";
            // 
            // BtnLogin
            // 
            this.BtnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLogin.Location = new System.Drawing.Point(169, 279);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(216, 51);
            this.BtnLogin.TabIndex = 10;
            this.BtnLogin.Text = "LOGIN";
            this.BtnLogin.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(230, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 25);
            this.label2.TabIndex = 9;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(226, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Username";
            // 
            // TTbxPassword
            // 
            this.TTbxPassword.AutoscrollFocusDisable = true;
            this.TTbxPassword.AutoscrollLeft = false;
            this.TTbxPassword.AutoscrollTop = false;
            this.TTbxPassword.DisplayMode = Asmodat.FormsControls.ThreadedTextBox.Mode.Text;
            this.TTbxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TTbxPassword.Location = new System.Drawing.Point(19, 234);
            this.TTbxPassword.Name = "TTbxPassword";
            this.TTbxPassword.Size = new System.Drawing.Size(537, 30);
            this.TTbxPassword.TabIndex = 13;
            this.TTbxPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TTbxPassword.UseSystemPasswordChar = true;
            // 
            // TTbxUsername
            // 
            this.TTbxUsername.AutoscrollFocusDisable = true;
            this.TTbxUsername.AutoscrollLeft = false;
            this.TTbxUsername.AutoscrollTop = false;
            this.TTbxUsername.DisplayMode = Asmodat.FormsControls.ThreadedTextBox.Mode.Text;
            this.TTbxUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TTbxUsername.Location = new System.Drawing.Point(19, 173);
            this.TTbxUsername.Name = "TTbxUsername";
            this.TTbxUsername.Size = new System.Drawing.Size(537, 30);
            this.TTbxUsername.TabIndex = 12;
            this.TTbxUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TRTbxLoginMessage
            // 
            this.TRTbxLoginMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TRTbxLoginMessage.AutoscrollFocusDisable = true;
            this.TRTbxLoginMessage.AutoscrollLeft = false;
            this.TRTbxLoginMessage.AutoscrollTop = false;
            this.TRTbxLoginMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TRTbxLoginMessage.Location = new System.Drawing.Point(9, 21);
            this.TRTbxLoginMessage.Name = "TRTbxLoginMessage";
            this.TRTbxLoginMessage.ReadOnly = true;
            this.TRTbxLoginMessage.Size = new System.Drawing.Size(537, 82);
            this.TRTbxLoginMessage.TabIndex = 8;
            this.TRTbxLoginMessage.Text = "Please enter your Login and Password in order to access this system.";
            // 
            // LoginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.Controls.Add(this.TbCntrlMain);
            this.Name = "LoginControl";
            this.Size = new System.Drawing.Size(600, 400);
            this.TbCntrlMain.ResumeLayout(false);
            this.TbPageLogin.ResumeLayout(false);
            this.PnlLogin.ResumeLayout(false);
            this.PnlLogin.PerformLayout();
            this.GpbxLoginMessage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TbCntrlMain;
        private System.Windows.Forms.TabPage TbPageLogin;
        private System.Windows.Forms.TabPage TbPgChangePassword;
        private System.Windows.Forms.Panel PnlLogin;
        private ThreadedTextBox TTbxPassword;
        private ThreadedTextBox TTbxUsername;
        private System.Windows.Forms.GroupBox GpbxLoginMessage;
        private ThreadedRichTextBox TRTbxLoginMessage;
        private System.Windows.Forms.Button BtnLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
