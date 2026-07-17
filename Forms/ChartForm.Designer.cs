namespace SmartInventory.Forms
{
    partial class ChartForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            panel1 = new Panel();
            lblTotal = new Label();
            panel2 = new Panel();
            chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chart).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(lblTotal);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 726);
            panel1.Name = "panel1";
            panel1.Size = new Size(624, 35);
            panel1.TabIndex = 1;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(12, 11);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(46, 15);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "總計為:";
            // 
            // panel2
            // 
            panel2.Controls.Add(chart);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(624, 726);
            panel2.TabIndex = 2;
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            chart.ChartAreas.Add(chartArea1);
            chart.Dock = DockStyle.Fill;
            legend1.Name = "Legend1";
            chart.Legends.Add(legend1);
            chart.Location = new Point(0, 0);
            chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart.Series.Add(series1);
            chart.Size = new Size(624, 726);
            chart.TabIndex = 0;
            chart.Text = "chart1";
            // 
            // ChartForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 761);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "ChartForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "各分類庫存統計";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chart).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private Label lblTotal;
    }
}