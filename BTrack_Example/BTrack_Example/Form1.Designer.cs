namespace BTrack_Example
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.labelDBs = new System.Windows.Forms.Label();
            this.labelBPM = new System.Windows.Forms.Label();
            this.AudioDevices = new System.Windows.Forms.ComboBox();
            this.labelBeatCount = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.labelCAM = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.labelTempo = new System.Windows.Forms.Label();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // labelDBs
            // 
            this.labelDBs.AutoSize = true;
            this.labelDBs.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelDBs.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelDBs.Location = new System.Drawing.Point(12, 9);
            this.labelDBs.Name = "labelDBs";
            this.labelDBs.Size = new System.Drawing.Size(0, 24);
            this.labelDBs.TabIndex = 0;
            // 
            // labelBPM
            // 
            this.labelBPM.AutoSize = true;
            this.labelBPM.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelBPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelBPM.Location = new System.Drawing.Point(108, 57);
            this.labelBPM.Name = "labelBPM";
            this.labelBPM.Size = new System.Drawing.Size(0, 24);
            this.labelBPM.TabIndex = 1;
            // 
            // AudioDevices
            // 
            this.AudioDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AudioDevices.FormattingEnabled = true;
            this.AudioDevices.Location = new System.Drawing.Point(12, 97);
            this.AudioDevices.Name = "AudioDevices";
            this.AudioDevices.Size = new System.Drawing.Size(760, 21);
            this.AudioDevices.TabIndex = 2;
            // 
            // labelBeatCount
            // 
            this.labelBeatCount.AutoSize = true;
            this.labelBeatCount.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelBeatCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelBeatCount.Location = new System.Drawing.Point(12, 33);
            this.labelBeatCount.Name = "labelBeatCount";
            this.labelBeatCount.Size = new System.Drawing.Size(0, 24);
            this.labelBeatCount.TabIndex = 3;
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(616, 128);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 4;
            this.buttonStart.Text = "START";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStop.Location = new System.Drawing.Point(697, 128);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 5;
            this.buttonStop.Text = "STOP";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // labelCAM
            // 
            this.labelCAM.AutoSize = true;
            this.labelCAM.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelCAM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCAM.Location = new System.Drawing.Point(12, 57);
            this.labelCAM.Name = "labelCAM";
            this.labelCAM.Size = new System.Drawing.Size(81, 24);
            this.labelCAM.TabIndex = 6;
            this.labelCAM.Text = "Camera:";
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(12, 128);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(92, 23);
            this.buttonReset.TabIndex = 7;
            this.buttonReset.Text = "RESET COUNT";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // labelTempo
            // 
            this.labelTempo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTempo.AutoSize = true;
            this.labelTempo.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTempo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelTempo.Location = new System.Drawing.Point(640, 9);
            this.labelTempo.Name = "labelTempo";
            this.labelTempo.Size = new System.Drawing.Size(0, 24);
            this.labelTempo.TabIndex = 8;
            // 
            // chart
            // 
            this.chart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea4.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart.Legends.Add(legend4);
            this.chart.Location = new System.Drawing.Point(11, 157);
            this.chart.Name = "chart";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chart.Series.Add(series4);
            this.chart.Size = new System.Drawing.Size(761, 392);
            this.chart.TabIndex = 9;
            this.chart.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.labelTempo);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.labelCAM);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.labelBeatCount);
            this.Controls.Add(this.AudioDevices);
            this.Controls.Add(this.labelBPM);
            this.Controls.Add(this.labelDBs);
            this.MinimumSize = new System.Drawing.Size(380, 320);
            this.Name = "Form1";
            this.Text = "Beat Tracking Example";
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDBs;
        private System.Windows.Forms.Label labelBPM;
        private System.Windows.Forms.ComboBox AudioDevices;
        private System.Windows.Forms.Label labelBeatCount;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label labelCAM;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Label labelTempo;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
    }
}

