namespace Asmodat.CONTROLS.Forms
{
    partial class CefSharpBrowser
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
            this.TTbxNavigationBar = new Asmodat.FormsControls.ThreadedTextBox();
            this.SuspendLayout();
            // 
            // TTbxNavigationBar
            // 
            this.TTbxNavigationBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TTbxNavigationBar.Autosave_Text = false;
            this.TTbxNavigationBar.AutoscrollFocusDisable = true;
            this.TTbxNavigationBar.AutoscrollLeft = false;
            this.TTbxNavigationBar.AutoscrollTop = false;
            this.TTbxNavigationBar.default_Int32 = 0;
            this.TTbxNavigationBar.DisplayMode = Asmodat.FormsControls.ThreadedTextBox.Mode.Text;
            this.TTbxNavigationBar.EnableKeyControl = false;
            this.TTbxNavigationBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TTbxNavigationBar.Location = new System.Drawing.Point(3, 4);
            this.TTbxNavigationBar.Name = "TTbxNavigationBar";
            this.TTbxNavigationBar.Size = new System.Drawing.Size(895, 30);
            this.TTbxNavigationBar.TabIndex = 0;
            // 
            // CefSharpBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.TTbxNavigationBar);
            this.Name = "CefSharpBrowser";
            this.Size = new System.Drawing.Size(901, 409);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormsControls.ThreadedTextBox TTbxNavigationBar;
    }
}
