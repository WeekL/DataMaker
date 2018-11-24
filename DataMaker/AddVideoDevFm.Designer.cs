namespace DataMaker
{
    partial class AddVideoDevFm
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
            this.label3 = new System.Windows.Forms.Label();
            this.AddIpAddresstextBox = new System.Windows.Forms.TextBox();
            this.AddChanneltextBox = new System.Windows.Forms.TextBox();
            this.AddDevNumtextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.AddPorttextBox = new System.Windows.Forms.TextBox();
            this.AddDevprogressBar = new System.Windows.Forms.ProgressBar();
            this.AddVideoDevbutton = new System.Windows.Forms.Button();
            this.AddVideoDevcheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备起始地址:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "通道数:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "添加数量:";
            // 
            // AddIpAddresstextBox
            // 
            this.AddIpAddresstextBox.Location = new System.Drawing.Point(120, 27);
            this.AddIpAddresstextBox.Name = "AddIpAddresstextBox";
            this.AddIpAddresstextBox.Size = new System.Drawing.Size(118, 21);
            this.AddIpAddresstextBox.TabIndex = 3;
            this.AddIpAddresstextBox.Text = "192.168.2.1";
            // 
            // AddChanneltextBox
            // 
            this.AddChanneltextBox.Location = new System.Drawing.Point(120, 83);
            this.AddChanneltextBox.Name = "AddChanneltextBox";
            this.AddChanneltextBox.Size = new System.Drawing.Size(118, 21);
            this.AddChanneltextBox.TabIndex = 4;
            this.AddChanneltextBox.Text = "5";
            // 
            // AddDevNumtextBox
            // 
            this.AddDevNumtextBox.Location = new System.Drawing.Point(120, 111);
            this.AddDevNumtextBox.Name = "AddDevNumtextBox";
            this.AddDevNumtextBox.Size = new System.Drawing.Size(118, 21);
            this.AddDevNumtextBox.TabIndex = 5;
            this.AddDevNumtextBox.Text = "160000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "起始端口:";
            // 
            // AddPorttextBox
            // 
            this.AddPorttextBox.Location = new System.Drawing.Point(120, 55);
            this.AddPorttextBox.Name = "AddPorttextBox";
            this.AddPorttextBox.Size = new System.Drawing.Size(118, 21);
            this.AddPorttextBox.TabIndex = 7;
            this.AddPorttextBox.Text = "8000";
            // 
            // AddDevprogressBar
            // 
            this.AddDevprogressBar.Location = new System.Drawing.Point(43, 139);
            this.AddDevprogressBar.Name = "AddDevprogressBar";
            this.AddDevprogressBar.Size = new System.Drawing.Size(195, 23);
            this.AddDevprogressBar.TabIndex = 8;
            // 
            // AddVideoDevbutton
            // 
            this.AddVideoDevbutton.Location = new System.Drawing.Point(92, 167);
            this.AddVideoDevbutton.Name = "AddVideoDevbutton";
            this.AddVideoDevbutton.Size = new System.Drawing.Size(75, 23);
            this.AddVideoDevbutton.TabIndex = 9;
            this.AddVideoDevbutton.Text = "添加";
            this.AddVideoDevbutton.UseVisualStyleBackColor = true;
            this.AddVideoDevbutton.Click += new System.EventHandler(this.AddVideoDevbutton_Click);
            // 
            // AddVideoDevcheckBox
            // 
            this.AddVideoDevcheckBox.AutoSize = true;
            this.AddVideoDevcheckBox.Location = new System.Drawing.Point(244, 29);
            this.AddVideoDevcheckBox.Name = "AddVideoDevcheckBox";
            this.AddVideoDevcheckBox.Size = new System.Drawing.Size(96, 16);
            this.AddVideoDevcheckBox.TabIndex = 10;
            this.AddVideoDevcheckBox.Text = "地址是否相同";
            this.AddVideoDevcheckBox.UseVisualStyleBackColor = true;
            // 
            // AddVideoDevFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 190);
            this.Controls.Add(this.AddVideoDevcheckBox);
            this.Controls.Add(this.AddVideoDevbutton);
            this.Controls.Add(this.AddDevprogressBar);
            this.Controls.Add(this.AddPorttextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AddDevNumtextBox);
            this.Controls.Add(this.AddChanneltextBox);
            this.Controls.Add(this.AddIpAddresstextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddVideoDevFm";
            this.Text = "AddVideoDevFm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AddIpAddresstextBox;
        private System.Windows.Forms.TextBox AddChanneltextBox;
        private System.Windows.Forms.TextBox AddDevNumtextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AddPorttextBox;
        private System.Windows.Forms.ProgressBar AddDevprogressBar;
        private System.Windows.Forms.Button AddVideoDevbutton;
        private System.Windows.Forms.CheckBox AddVideoDevcheckBox;
    }
}