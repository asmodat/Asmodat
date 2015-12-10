namespace Asmodat.FormsControls
{
    partial class ThreadedChart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ChartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.LblCursorX = new System.Windows.Forms.Label();
            this.LblCursorY = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ChartMain)).BeginInit();
            this.SuspendLayout();
            // 
            // ChartMain
            // 
            chartArea1.Name = "ChartArea1";
            this.ChartMain.ChartAreas.Add(chartArea1);
            this.ChartMain.Cursor = System.Windows.Forms.Cursors.Cross;
            this.ChartMain.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.ChartMain.Legends.Add(legend1);
            this.ChartMain.Location = new System.Drawing.Point(0, 0);
            this.ChartMain.Margin = new System.Windows.Forms.Padding(0);
            this.ChartMain.Name = "ChartMain";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.ChartMain.Series.Add(series1);
            this.ChartMain.Size = new System.Drawing.Size(600, 400);
            this.ChartMain.TabIndex = 0;
            // 
            // LblCursorX
            // 
            this.LblCursorX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LblCursorX.AutoSize = true;
            this.LblCursorX.BackColor = System.Drawing.Color.White;
            this.LblCursorX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LblCursorX.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblCursorX.Location = new System.Drawing.Point(551, 0);
            this.LblCursorX.Name = "LblCursorX";
            this.LblCursorX.Size = new System.Drawing.Size(53, 20);
            this.LblCursorX.TabIndex = 1;
            this.LblCursorX.Text = "value";
            // 
            // LblCursorY
            // 
            this.LblCursorY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LblCursorY.AutoSize = true;
            this.LblCursorY.BackColor = System.Drawing.Color.White;
            this.LblCursorY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LblCursorY.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LblCursorY.Location = new System.Drawing.Point(3, 0);
            this.LblCursorY.Name = "LblCursorY";
            this.LblCursorY.Size = new System.Drawing.Size(53, 20);
            this.LblCursorY.TabIndex = 2;
            this.LblCursorY.Text = "value";
            // 
            // ThreadedChart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.Controls.Add(this.LblCursorY);
            this.Controls.Add(this.LblCursorX);
            this.Controls.Add(this.ChartMain);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ThreadedChart";
            this.Size = new System.Drawing.Size(600, 400);
            ((System.ComponentModel.ISupportInitialize)(this.ChartMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ChartMain;
        private System.Windows.Forms.Label LblCursorX;
        private System.Windows.Forms.Label LblCursorY;
    }
}
