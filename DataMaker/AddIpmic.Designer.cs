namespace DataMaker
{
    partial class AddIpmic
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
            this.AddIpmicIpAddresstextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AddIpmicNumtextBox = new System.Windows.Forms.TextBox();
            this.AddIpmicbutton = new System.Windows.Forms.Button();
            this.AddIpmicprogressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.AddIpmicNotextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP起始地址:";
            // 
            // AddIpmicIpAddresstextBox
            // 
            this.AddIpmicIpAddresstextBox.Location = new System.Drawing.Point(96, 20);
            this.AddIpmicIpAddresstextBox.Name = "AddIpmicIpAddresstextBox";
            this.AddIpmicIpAddresstextBox.Size = new System.Drawing.Size(100, 21);
            this.AddIpmicIpAddresstextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "数量:";
            // 
            // AddIpmicNumtextBox
            // 
            this.AddIpmicNumtextBox.Location = new System.Drawing.Point(96, 49);
            this.AddIpmicNumtextBox.Name = "AddIpmicNumtextBox";
            this.AddIpmicNumtextBox.Size = new System.Drawing.Size(100, 21);
            this.AddIpmicNumtextBox.TabIndex = 3;
            // 
            // AddIpmicbutton
            // 
            this.AddIpmicbutton.Location = new System.Drawing.Point(81, 134);
            this.AddIpmicbutton.Name = "AddIpmicbutton";
            this.AddIpmicbutton.Size = new System.Drawing.Size(75, 23);
            this.AddIpmicbutton.TabIndex = 4;
            this.AddIpmicbutton.Text = "添加";
            this.AddIpmicbutton.UseVisualStyleBackColor = true;
            this.AddIpmicbutton.Click += new System.EventHandler(this.AddIpmicbutton_Click);
            // 
            // AddIpmicprogressBar
            // 
            this.AddIpmicprogressBar.Location = new System.Drawing.Point(31, 105);
            this.AddIpmicprogressBar.Name = "AddIpmicprogressBar";
            this.AddIpmicprogressBar.Size = new System.Drawing.Size(165, 23);
            this.AddIpmicprogressBar.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "主机号起始:";
            // 
            // AddIpmicNotextBox
            // 
            this.AddIpmicNotextBox.Location = new System.Drawing.Point(96, 78);
            this.AddIpmicNotextBox.Name = "AddIpmicNotextBox";
            this.AddIpmicNotextBox.Size = new System.Drawing.Size(100, 21);
            this.AddIpmicNotextBox.TabIndex = 7;
            // 
            // AddIpmic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 169);
            this.Controls.Add(this.AddIpmicNotextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AddIpmicprogressBar);
            this.Controls.Add(this.AddIpmicbutton);
            this.Controls.Add(this.AddIpmicNumtextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AddIpmicIpAddresstextBox);
            this.Controls.Add(this.label1);
            this.Name = "AddIpmic";
            this.Text = "AddIpmic";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AddIpmicIpAddresstextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AddIpmicNumtextBox;
        private System.Windows.Forms.Button AddIpmicbutton;
        private System.Windows.Forms.ProgressBar AddIpmicprogressBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AddIpmicNotextBox;
    }
}