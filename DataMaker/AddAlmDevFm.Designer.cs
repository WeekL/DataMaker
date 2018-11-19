namespace DataMaker
{
    partial class AddAlmDevFm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.IpAddresstextBox = new System.Windows.Forms.TextBox();
            this.PorttextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AddNumtextBox = new System.Windows.Forms.TextBox();
            this.AddAlmDevbutton = new System.Windows.Forms.Button();
            this.AddAlmDevprogressBar = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IpAddress:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // IpAddresstextBox
            // 
            this.IpAddresstextBox.Location = new System.Drawing.Point(75, 35);
            this.IpAddresstextBox.Name = "IpAddresstextBox";
            this.IpAddresstextBox.Size = new System.Drawing.Size(100, 21);
            this.IpAddresstextBox.TabIndex = 2;
            // 
            // PorttextBox
            // 
            this.PorttextBox.Location = new System.Drawing.Point(93, 61);
            this.PorttextBox.Name = "PorttextBox";
            this.PorttextBox.Size = new System.Drawing.Size(82, 21);
            this.PorttextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "添加数量:";
            // 
            // AddNumtextBox
            // 
            this.AddNumtextBox.Location = new System.Drawing.Point(75, 88);
            this.AddNumtextBox.Name = "AddNumtextBox";
            this.AddNumtextBox.Size = new System.Drawing.Size(100, 21);
            this.AddNumtextBox.TabIndex = 5;
            // 
            // AddAlmDevbutton
            // 
            this.AddAlmDevbutton.Location = new System.Drawing.Point(48, 154);
            this.AddAlmDevbutton.Name = "AddAlmDevbutton";
            this.AddAlmDevbutton.Size = new System.Drawing.Size(75, 23);
            this.AddAlmDevbutton.TabIndex = 6;
            this.AddAlmDevbutton.Text = "添加";
            this.AddAlmDevbutton.UseVisualStyleBackColor = true;
            this.AddAlmDevbutton.Click += new System.EventHandler(this.AddAlmDevbutton_Click);
            // 
            // AddAlmDevprogressBar
            // 
            this.AddAlmDevprogressBar.Location = new System.Drawing.Point(15, 115);
            this.AddAlmDevprogressBar.Name = "AddAlmDevprogressBar";
            this.AddAlmDevprogressBar.Size = new System.Drawing.Size(160, 23);
            this.AddAlmDevprogressBar.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "资产编号起始:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(91, 9);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(84, 21);
            this.textBox1.TabIndex = 9;
            // 
            // AddAlmDevFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 185);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AddAlmDevprogressBar);
            this.Controls.Add(this.AddAlmDevbutton);
            this.Controls.Add(this.AddNumtextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PorttextBox);
            this.Controls.Add(this.IpAddresstextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddAlmDevFm";
            this.Text = "AddAlmDevFm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox IpAddresstextBox;
        private System.Windows.Forms.TextBox PorttextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AddNumtextBox;
        private System.Windows.Forms.Button AddAlmDevbutton;
        private System.Windows.Forms.ProgressBar AddAlmDevprogressBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
    }
}