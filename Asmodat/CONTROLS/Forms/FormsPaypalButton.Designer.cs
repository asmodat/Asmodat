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
            this.BtnPayPalDonate.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnPayPalDonate.BackgroundImage = global::Asmodat.Properties.Resources.paypal_donate_button_100;
            this.BtnPayPalDonate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnPayPalDonate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnPayPalDonate.Location = new System.Drawing.Point(0, 0);
            this.BtnPayPalDonate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnPayPalDonate.Name = "BtnPayPalDonate";
            this.BtnPayPalDonate.Size = new System.Drawing.Size(135, 36);
            this.BtnPayPalDonate.TabIndex = 0;
            this.BtnPayPalDonate.UseVisualStyleBackColor = false;
            this.BtnPayPalDonate.Click += new System.EventHandler(this.BtnPayPalDonate_Click);
            // 
            // FormsPaypalButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtnPayPalDonate);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormsPaypalButton";
            this.Size = new System.Drawing.Size(135, 36);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnPayPalDonate;
    }
}
