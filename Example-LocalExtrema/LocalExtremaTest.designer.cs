namespace AlgorithmTestWF
{
    partial class LocalExtremaTest
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
            this.components = new System.ComponentModel.Container();
            this.zedGraph = new ZedGraph.ZedGraphControl();
            this.textBoxSearchWindowRadius = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxThresholdWindowRadius = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxThresholdValue = new System.Windows.Forms.TextBox();
            this.textBoxFirFilterLength = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.checkBoxEnableAlternateExtremaRule = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableThresholding = new System.Windows.Forms.CheckBox();
            this.comboBoxWindowingMode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelNumExtrema = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // zedGraph
            // 
            this.zedGraph.Location = new System.Drawing.Point(58, 35);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(1062, 464);
            this.zedGraph.TabIndex = 0;
            // 
            // textBoxSearchWindowRadius
            // 
            this.textBoxSearchWindowRadius.Location = new System.Drawing.Point(300, 570);
            this.textBoxSearchWindowRadius.Name = "textBoxSearchWindowRadius";
            this.textBoxSearchWindowRadius.Size = new System.Drawing.Size(100, 20);
            this.textBoxSearchWindowRadius.TabIndex = 1;
            this.textBoxSearchWindowRadius.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 572);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Search Window Radius (elements):";
            // 
            // textBoxThresholdWindowRadius
            // 
            this.textBoxThresholdWindowRadius.Location = new System.Drawing.Point(665, 562);
            this.textBoxThresholdWindowRadius.Name = "textBoxThresholdWindowRadius";
            this.textBoxThresholdWindowRadius.Size = new System.Drawing.Size(100, 20);
            this.textBoxThresholdWindowRadius.TabIndex = 3;
            this.textBoxThresholdWindowRadius.Text = "5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(454, 564);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(186, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Threshold Window Radius (elements):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(454, 586);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Threshold Value:";
            // 
            // textBoxThresholdValue
            // 
            this.textBoxThresholdValue.Location = new System.Drawing.Point(665, 587);
            this.textBoxThresholdValue.Name = "textBoxThresholdValue";
            this.textBoxThresholdValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxThresholdValue.TabIndex = 6;
            this.textBoxThresholdValue.Text = "2";
            // 
            // textBoxFirFilterLength
            // 
            this.textBoxFirFilterLength.Location = new System.Drawing.Point(300, 544);
            this.textBoxFirFilterLength.Name = "textBoxFirFilterLength";
            this.textBoxFirFilterLength.Size = new System.Drawing.Size(100, 20);
            this.textBoxFirFilterLength.TabIndex = 7;
            this.textBoxFirFilterLength.Text = "4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(89, 546);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(206, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Moving Average Widow Width (elements):";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // checkBoxEnableAlternateExtremaRule
            // 
            this.checkBoxEnableAlternateExtremaRule.AutoSize = true;
            this.checkBoxEnableAlternateExtremaRule.Location = new System.Drawing.Point(803, 545);
            this.checkBoxEnableAlternateExtremaRule.Name = "checkBoxEnableAlternateExtremaRule";
            this.checkBoxEnableAlternateExtremaRule.Size = new System.Drawing.Size(170, 17);
            this.checkBoxEnableAlternateExtremaRule.TabIndex = 9;
            this.checkBoxEnableAlternateExtremaRule.Text = "Enable Alternate Extrema Rule";
            this.checkBoxEnableAlternateExtremaRule.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableThresholding
            // 
            this.checkBoxEnableThresholding.AutoSize = true;
            this.checkBoxEnableThresholding.Location = new System.Drawing.Point(457, 542);
            this.checkBoxEnableThresholding.Name = "checkBoxEnableThresholding";
            this.checkBoxEnableThresholding.Size = new System.Drawing.Size(123, 17);
            this.checkBoxEnableThresholding.TabIndex = 10;
            this.checkBoxEnableThresholding.Text = "Enable Thresholding";
            this.checkBoxEnableThresholding.UseVisualStyleBackColor = true;
            // 
            // comboBoxWindowingMode
            // 
            this.comboBoxWindowingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWindowingMode.FormattingEnabled = true;
            this.comboBoxWindowingMode.Items.AddRange(new object[] {
            "Fixed",
            "Dynamic"});
            this.comboBoxWindowingMode.Location = new System.Drawing.Point(279, 606);
            this.comboBoxWindowingMode.Name = "comboBoxWindowingMode";
            this.comboBoxWindowingMode.Size = new System.Drawing.Size(121, 21);
            this.comboBoxWindowingMode.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(89, 609);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Windowing Mode:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(950, 614);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Number of Extrema Found:";
            // 
            // labelNumExtrema
            // 
            this.labelNumExtrema.AutoSize = true;
            this.labelNumExtrema.Location = new System.Drawing.Point(1089, 614);
            this.labelNumExtrema.Name = "labelNumExtrema";
            this.labelNumExtrema.Size = new System.Drawing.Size(13, 13);
            this.labelNumExtrema.TabIndex = 15;
            this.labelNumExtrema.Text = "5";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 659);
            this.Controls.Add(this.labelNumExtrema);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxWindowingMode);
            this.Controls.Add(this.checkBoxEnableThresholding);
            this.Controls.Add(this.checkBoxEnableAlternateExtremaRule);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxFirFilterLength);
            this.Controls.Add(this.textBoxThresholdValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxThresholdWindowRadius);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSearchWindowRadius);
            this.Controls.Add(this.zedGraph);
            this.Name = "MainForm";
            this.Text = "Extrema Algorithm Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraph;
        private System.Windows.Forms.TextBox textBoxSearchWindowRadius;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxThresholdWindowRadius;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxThresholdValue;
        private System.Windows.Forms.TextBox textBoxFirFilterLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBoxEnableAlternateExtremaRule;
        private System.Windows.Forms.CheckBox checkBoxEnableThresholding;
        private System.Windows.Forms.ComboBox comboBoxWindowingMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelNumExtrema;
    }
}

