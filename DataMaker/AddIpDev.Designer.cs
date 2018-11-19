namespace DataMaker
{
    partial class AddIpDev
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
            this.AddIpDevIpAddresstextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AddIpdevNumtextBox = new System.Windows.Forms.TextBox();
            this.AddIpdevbutton = new System.Windows.Forms.Button();
            this.AddIpdevprogressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.AddIpdevNotextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP起始地址:";
            // 
            // AddIpDevIpAddresstextBox
            // 
            this.AddIpDevIpAddresstextBox.Location = new System.Drawing.Point(94, 6);
            this.AddIpDevIpAddresstextBox.Name = "AddIpDevIpAddresstextBox";
            this.AddIpDevIpAddresstextBox.Size = new System.Drawing.Size(100, 21);
            this.AddIpDevIpAddresstextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "设备数量:";
            // 
            // AddIpdevNumtextBox
            // 
            this.AddIpdevNumtextBox.Location = new System.Drawing.Point(94, 33);
            this.AddIpdevNumtextBox.Name = "AddIpdevNumtextBox";
            this.AddIpdevNumtextBox.Size = new System.Drawing.Size(100, 21);
            this.AddIpdevNumtextBox.TabIndex = 3;
            // 
            // AddIpdevbutton
            // 
            this.AddIpdevbutton.Location = new System.Drawing.Point(82, 126);
            this.AddIpdevbutton.Name = "AddIpdevbutton";
            this.AddIpdevbutton.Size = new System.Drawing.Size(75, 23);
            this.AddIpdevbutton.TabIndex = 4;
            this.AddIpdevbutton.Text = "添加";
            this.AddIpdevbutton.UseVisualStyleBackColor = true;
            this.AddIpdevbutton.Click += new System.EventHandler(this.AddIpdevbutton_Click);
            // 
            // AddIpdevprogressBar
            // 
            this.AddIpdevprogressBar.Location = new System.Drawing.Point(28, 97);
            this.AddIpdevprogressBar.Name = "AddIpdevprogressBar";
            this.AddIpdevprogressBar.Size = new System.Drawing.Size(166, 23);
            this.AddIpdevprogressBar.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "主机号起始:";
            // 
            // AddIpdevNotextBox
            // 
            this.AddIpdevNotextBox.Location = new System.Drawing.Point(94, 65);
            this.AddIpdevNotextBox.Name = "AddIpdevNotextBox";
            this.AddIpdevNotextBox.Size = new System.Drawing.Size(100, 21);
            this.AddIpdevNotextBox.TabIndex = 7;
            // 
            // AddIpDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 149);
            this.Controls.Add(this.AddIpdevNotextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AddIpdevprogressBar);
            this.Controls.Add(this.AddIpdevbutton);
            this.Controls.Add(this.AddIpdevNumtextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AddIpDevIpAddresstextBox);
            this.Controls.Add(this.label1);
            this.Name = "AddIpDev";
            this.Text = "AddIpDev";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AddIpDevIpAddresstextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AddIpdevNumtextBox;
        private System.Windows.Forms.Button AddIpdevbutton;
        private System.Windows.Forms.ProgressBar AddIpdevprogressBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AddIpdevNotextBox;
    }
}