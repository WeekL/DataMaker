namespace DataMaker
{
    partial class LoginMysqlFm
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
            this.LoginMysqlbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.UserNametextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PwdtextBox = new System.Windows.Forms.TextBox();
            this.ipAddressControl1 = new IPAddressControlLib.IPAddressControl();
            this.BaseDatatextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoginMysqlbutton
            // 
            this.LoginMysqlbutton.Location = new System.Drawing.Point(12, 94);
            this.LoginMysqlbutton.Name = "LoginMysqlbutton";
            this.LoginMysqlbutton.Size = new System.Drawing.Size(75, 23);
            this.LoginMysqlbutton.TabIndex = 0;
            this.LoginMysqlbutton.Text = "连接";
            this.LoginMysqlbutton.UseVisualStyleBackColor = true;
            this.LoginMysqlbutton.Click += new System.EventHandler(this.LoginMysqlbutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "IpAddress:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(242, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(67, 21);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "3306";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "UserName:";
            // 
            // UserNametextBox
            // 
            this.UserNametextBox.Location = new System.Drawing.Point(73, 62);
            this.UserNametextBox.Name = "UserNametextBox";
            this.UserNametextBox.Size = new System.Drawing.Size(116, 21);
            this.UserNametextBox.TabIndex = 6;
            this.UserNametextBox.Text = "root";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(205, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Pwd:";
            // 
            // PwdtextBox
            // 
            this.PwdtextBox.Location = new System.Drawing.Point(242, 61);
            this.PwdtextBox.Name = "PwdtextBox";
            this.PwdtextBox.Size = new System.Drawing.Size(67, 21);
            this.PwdtextBox.TabIndex = 8;
            this.PwdtextBox.Text = "123456";
            // 
            // ipAddressControl1
            // 
            this.ipAddressControl1.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddressControl1.Location = new System.Drawing.Point(73, 30);
            this.ipAddressControl1.MinimumSize = new System.Drawing.Size(116, 21);
            this.ipAddressControl1.Name = "ipAddressControl1";
            this.ipAddressControl1.ReadOnly = false;
            this.ipAddressControl1.Size = new System.Drawing.Size(116, 21);
            this.ipAddressControl1.TabIndex = 1;
            // 
            // BaseDatatextBox
            // 
            this.BaseDatatextBox.Location = new System.Drawing.Point(209, 94);
            this.BaseDatatextBox.Name = "BaseDatatextBox";
            this.BaseDatatextBox.Size = new System.Drawing.Size(100, 21);
            this.BaseDatatextBox.TabIndex = 9;
            this.BaseDatatextBox.Text = "basisdata";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(120, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "BaseDataName:";
            // 
            // LoginMysqlFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 130);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BaseDatatextBox);
            this.Controls.Add(this.PwdtextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.UserNametextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ipAddressControl1);
            this.Controls.Add(this.LoginMysqlbutton);
            this.MaximizeBox = false;
            this.Name = "LoginMysqlFm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginMysqlFm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoginMysqlbutton;
        private IPAddressControlLib.IPAddressControl ipAddressControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox UserNametextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PwdtextBox;
        private System.Windows.Forms.TextBox BaseDatatextBox;
        private System.Windows.Forms.Label label5;
    }
}