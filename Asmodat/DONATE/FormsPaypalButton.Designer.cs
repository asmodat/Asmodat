namespace Asmodat.Donate
{
    partial class FormsPaypalButton
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
            this.BtnPayPalDonate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnPayPalDonate
            // 
            this.BtnPayPalDonate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnPayPalDonate.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnPayPalDonate.Location = new System.Drawing.Point(0, 0);
            this.BtnPayPalDonate.Name = "BtnPayPalDonate";
            this.BtnPayPalDonate.Size = new System.Drawing.Size(100, 28);
            this.BtnPayPalDonate.TabIndex = 0;
            this.BtnPayPalDonate.Text = "DONATE";
            this.BtnPayPalDonate.UseVisualStyleBackColor = false;
            this.BtnPayPalDonate.Click += new System.EventHandler(this.BtnPayPalDonate_Click);
            // 
            // FormsPaypalButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtnPayPalDonate);
            this.Name = "FormsPaypalButton";
            this.Size = new System.Drawing.Size(101, 29);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnPayPalDonate;
    }
}
