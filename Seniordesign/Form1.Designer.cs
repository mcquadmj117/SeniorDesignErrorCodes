
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
            this.SuspendLayout();
            // 
            // load
            // 
            this.load.Location = new System.Drawing.Point(446, 32);
            this.load.Name = "load";
            this.load.Size = new System.Drawing.Size(198, 23);
            this.load.TabIndex = 0;
            this.load.Text = "Load_Gamers_From_Excel";
            this.load.UseVisualStyleBackColor = true;
            this.load.Click += new System.EventHandler(this.load_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(958, 105);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(75, 23);
            this.stop.TabIndex = 1;
            this.stop.Text = "Stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Visible = false;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(155, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start_WMI";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(52, 155);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1028, 147);
            this.listBox1.TabIndex = 3;
            // 
            // Load_Results_Into_Excel
            // 
            this.Load_Results_Into_Excel.Location = new System.Drawing.Point(519, 319);
            this.Load_Results_Into_Excel.Name = "Load_Results_Into_Excel";
            this.Load_Results_Into_Excel.Size = new System.Drawing.Size(75, 23);
            this.Load_Results_Into_Excel.TabIndex = 4;
            this.Load_Results_Into_Excel.Text = "Load_Results_Into_Excel";
            this.Load_Results_Into_Excel.UseVisualStyleBackColor = true;
            this.Load_Results_Into_Excel.Visible = false;
            this.Load_Results_Into_Excel.Click += new System.EventHandler(this.Load_Results_Into_Excel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label1.Location = new System.Drawing.Point(26, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "InfoLabel";
            // 
            // End_Session
            // 
            this.End_Session.Location = new System.Drawing.Point(519, 378);
            this.End_Session.Name = "End_Session";
            this.End_Session.Size = new System.Drawing.Size(97, 38);
            this.End_Session.TabIndex = 6;
            this.End_Session.Text = "End_Session";
            this.End_Session.UseVisualStyleBackColor = true;
            this.End_Session.Visible = false;
            this.End_Session.Click += new System.EventHandler(this.End_Session_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 453);
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
    }
}

