namespace DataMaker
{
    partial class AddIASDevFm
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
            this.IASprogressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.AddIpAddresstextBox = new System.Windows.Forms.TextBox();
            this.AddPorttextBox = new System.Windows.Forms.TextBox();
            this.AddDevNumtextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IpAddress:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // IASprogressBar
            // 
            this.IASprogressBar.Location = new System.Drawing.Point(32, 99);
            this.IASprogressBar.Name = "IASprogressBar";
            this.IASprogressBar.Size = new System.Drawing.Size(159, 23);
            this.IASprogressBar.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "添加个数:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(70, 128);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AddIpAddresstextBox
            // 
            this.AddIpAddresstextBox.Location = new System.Drawing.Point(91, 19);
            this.AddIpAddresstextBox.Name = "AddIpAddresstextBox";
            this.AddIpAddresstextBox.Size = new System.Drawing.Size(100, 21);
            this.AddIpAddresstextBox.TabIndex = 5;
            // 
            // AddPorttextBox
            // 
            this.AddPorttextBox.Location = new System.Drawing.Point(91, 44);
            this.AddPorttextBox.Name = "AddPorttextBox";
            this.AddPorttextBox.Size = new System.Drawing.Size(100, 21);
            this.AddPorttextBox.TabIndex = 6;
            // 
            // AddDevNumtextBox
            // 
            this.AddDevNumtextBox.Location = new System.Drawing.Point(91, 70);
            this.AddDevNumtextBox.Name = "AddDevNumtextBox";
            this.AddDevNumtextBox.Size = new System.Drawing.Size(100, 21);
            this.AddDevNumtextBox.TabIndex = 7;
            // 
            // AddIASDevFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 159);
            this.Controls.Add(this.AddDevNumtextBox);
            this.Controls.Add(this.AddPorttextBox);
            this.Controls.Add(this.AddIpAddresstextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.IASprogressBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddIASDevFm";
            this.Text = "AddIASDevFm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar IASprogressBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox AddIpAddresstextBox;
        private System.Windows.Forms.TextBox AddPorttextBox;
        private System.Windows.Forms.TextBox AddDevNumtextBox;
    }
}