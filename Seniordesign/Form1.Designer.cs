
namespace Seniordesign
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
            this.load = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Load_Results_Into_Excel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.End_Session = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.ResumeButton = new System.Windows.Forms.Button();
            this.RestartButton = new System.Windows.Forms.Button();
            this.CritNotListBox = new System.Windows.Forms.ListBox();
            this.CritNotLabel = new System.Windows.Forms.Label();
            this.PartStatLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // load
            // 
            this.load.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.load.Location = new System.Drawing.Point(211, 210);
            this.load.Name = "load";
            this.load.Size = new System.Drawing.Size(257, 30);
            this.load.TabIndex = 0;
            this.load.TabStop = false;
            this.load.Text = "Load_Gamers_From_Excel";
            this.load.UseVisualStyleBackColor = true;
            this.load.Click += new System.EventHandler(this.load_Click);
            // 
            // stop
            // 
            this.stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.stop.Location = new System.Drawing.Point(1039, 37);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(75, 32);
            this.stop.TabIndex = 1;
            this.stop.TabStop = false;
            this.stop.Text = "End";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Visible = false;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.button1.Location = new System.Drawing.Point(55, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 30);
            this.button1.TabIndex = 2;
            this.button1.TabStop = false;
            this.button1.Text = "Start_WMI";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(55, 292);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1028, 108);
            this.listBox1.TabIndex = 3;
            // 
            // Load_Results_Into_Excel
            // 
            this.Load_Results_Into_Excel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.Load_Results_Into_Excel.Location = new System.Drawing.Point(2, 489);
            this.Load_Results_Into_Excel.Name = "Load_Results_Into_Excel";
            this.Load_Results_Into_Excel.Size = new System.Drawing.Size(211, 50);
            this.Load_Results_Into_Excel.TabIndex = 4;
            this.Load_Results_Into_Excel.TabStop = false;
            this.Load_Results_Into_Excel.Text = "Load_Results_Into_Excel";
            this.Load_Results_Into_Excel.UseVisualStyleBackColor = true;
            this.Load_Results_Into_Excel.Visible = false;
            this.Load_Results_Into_Excel.Click += new System.EventHandler(this.Load_Results_Into_Excel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label1.Location = new System.Drawing.Point(276, 81);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 4);
            this.label1.Size = new System.Drawing.Size(88, 32);
            this.label1.TabIndex = 5;
            this.label1.Text = "InfoLabel";
            // 
            // End_Session
            // 
            this.End_Session.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.End_Session.Location = new System.Drawing.Point(992, 532);
            this.End_Session.Name = "End_Session";
            this.End_Session.Size = new System.Drawing.Size(138, 38);
            this.End_Session.TabIndex = 6;
            this.End_Session.TabStop = false;
            this.End_Session.Text = "End_Session";
            this.End_Session.UseVisualStyleBackColor = true;
            this.End_Session.Visible = false;
            this.End_Session.Click += new System.EventHandler(this.End_Session_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.PauseButton.Location = new System.Drawing.Point(785, 37);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(80, 32);
            this.PauseButton.TabIndex = 7;
            this.PauseButton.TabStop = false;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Visible = false;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // ResumeButton
            // 
            this.ResumeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.ResumeButton.Location = new System.Drawing.Point(891, 37);
            this.ResumeButton.Name = "ResumeButton";
            this.ResumeButton.Size = new System.Drawing.Size(75, 32);
            this.ResumeButton.TabIndex = 8;
            this.ResumeButton.TabStop = false;
            this.ResumeButton.Text = "Resume";
            this.ResumeButton.UseVisualStyleBackColor = true;
            this.ResumeButton.Visible = false;
            this.ResumeButton.Click += new System.EventHandler(this.ResumeButton_Click);
            // 
            // RestartButton
            // 
            this.RestartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.RestartButton.Location = new System.Drawing.Point(7, 2);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(251, 33);
            this.RestartButton.TabIndex = 9;
            this.RestartButton.TabStop = false;
            this.RestartButton.Text = "Restart App For New Session";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.RestartButton_Click);
            // 
            // CritNotListBox
            // 
            this.CritNotListBox.FormattingEnabled = true;
            this.CritNotListBox.Location = new System.Drawing.Point(231, 475);
            this.CritNotListBox.Name = "CritNotListBox";
            this.CritNotListBox.Size = new System.Drawing.Size(755, 95);
            this.CritNotListBox.TabIndex = 10;
            this.CritNotListBox.Visible = false;
            // 
            // CritNotLabel
            // 
            this.CritNotLabel.AutoSize = true;
            this.CritNotLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CritNotLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Underline);
            this.CritNotLabel.Location = new System.Drawing.Point(251, 455);
            this.CritNotLabel.Name = "CritNotLabel";
            this.CritNotLabel.Size = new System.Drawing.Size(131, 17);
            this.CritNotLabel.TabIndex = 11;
            this.CritNotLabel.Text = "Critical Notifications";
            this.CritNotLabel.Visible = false;
            // 
            // PartStatLabel
            // 
            this.PartStatLabel.AutoSize = true;
            this.PartStatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Underline);
            this.PartStatLabel.Location = new System.Drawing.Point(117, 272);
            this.PartStatLabel.Name = "PartStatLabel";
            this.PartStatLabel.Size = new System.Drawing.Size(194, 17);
            this.PartStatLabel.TabIndex = 12;
            this.PartStatLabel.Text = "Participant Connection Status";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.label2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label2.Location = new System.Drawing.Point(524, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 38);
            this.label2.TabIndex = 13;
            this.label2.Text = "Fx3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 593);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PartStatLabel);
            this.Controls.Add(this.CritNotLabel);
            this.Controls.Add(this.CritNotListBox);
            this.Controls.Add(this.RestartButton);
            this.Controls.Add(this.ResumeButton);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.End_Session);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Load_Results_Into_Excel);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.load);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button load;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button Load_Results_Into_Excel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button End_Session;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.Button ResumeButton;
        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.ListBox CritNotListBox;
        private System.Windows.Forms.Label CritNotLabel;
        private System.Windows.Forms.Label PartStatLabel;
        private System.Windows.Forms.Label label2;
    }
}

