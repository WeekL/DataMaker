namespace DataMaker
{
    partial class AddACSDevFm
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
            this.AddACSDevIpAddresstextBox = new System.Windows.Forms.TextBox();
            this.AddACSDevNumtextBox = new System.Windows.Forms.TextBox();
            this.AddACSDevFmbutton = new System.Windows.Forms.Button();
            this.AddACSDevprogressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.PorttextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.AddACSDevUIDtextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备IP地址:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "添加数量:";
            // 
            // AddACSDevIpAddresstextBox
            // 
            this.AddACSDevIpAddresstextBox.Location = new System.Drawing.Point(103, 16);
            this.AddACSDevIpAddresstextBox.Name = "AddACSDevIpAddresstextBox";
            this.AddACSDevIpAddresstextBox.Size = new System.Drawing.Size(100, 21);
            this.AddACSDevIpAddresstextBox.TabIndex = 2;
            // 
            // AddACSDevNumtextBox
            // 
            this.AddACSDevNumtextBox.Location = new System.Drawing.Point(103, 74);
            this.AddACSDevNumtextBox.Name = "AddACSDevNumtextBox";
            this.AddACSDevNumtextBox.Size = new System.Drawing.Size(100, 21);
            this.AddACSDevNumtextBox.TabIndex = 3;
            // 
            // AddACSDevFmbutton
            // 
            this.AddACSDevFmbutton.Location = new System.Drawing.Point(70, 183);
            this.AddACSDevFmbutton.Name = "AddACSDevFmbutton";
            this.AddACSDevFmbutton.Size = new System.Drawing.Size(75, 23);
            this.AddACSDevFmbutton.TabIndex = 4;
            this.AddACSDevFmbutton.Text = "确定";
            this.AddACSDevFmbutton.UseVisualStyleBackColor = true;
            this.AddACSDevFmbutton.Click += new System.EventHandler(this.AddACSDevFmbutton_Click);
            // 
            // AddACSDevprogressBar
            // 
            this.AddACSDevprogressBar.Location = new System.Drawing.Point(26, 154);
            this.AddACSDevprogressBar.Name = "AddACSDevprogressBar";
            this.AddACSDevprogressBar.Size = new System.Drawing.Size(177, 23);
            this.AddACSDevprogressBar.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "设备端口起始地址:";
            // 
            // PorttextBox
            // 
            this.PorttextBox.Location = new System.Drawing.Point(127, 45);
            this.PorttextBox.Name = "PorttextBox";
            this.PorttextBox.Size = new System.Drawing.Size(76, 21);
            this.PorttextBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "设备UID起始:";
            // 
            // AddACSDevUIDtextBox
            // 
            this.AddACSDevUIDtextBox.Location = new System.Drawing.Point(103, 107);
            this.AddACSDevUIDtextBox.Name = "AddACSDevUIDtextBox";
            this.AddACSDevUIDtextBox.Size = new System.Drawing.Size(100, 21);
            this.AddACSDevUIDtextBox.TabIndex = 9;
            // 
            // AddACSDevFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(227, 207);
            this.Controls.Add(this.AddACSDevUIDtextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PorttextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AddACSDevprogressBar);
            this.Controls.Add(this.AddACSDevFmbutton);
            this.Controls.Add(this.AddACSDevNumtextBox);
            this.Controls.Add(this.AddACSDevIpAddresstextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddACSDevFm";
            this.Text = "AddACSDevFm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AddACSDevIpAddresstextBox;
        private System.Windows.Forms.TextBox AddACSDevNumtextBox;
        private System.Windows.Forms.Button AddACSDevFmbutton;
        private System.Windows.Forms.ProgressBar AddACSDevprogressBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PorttextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AddACSDevUIDtextBox;
    }
}