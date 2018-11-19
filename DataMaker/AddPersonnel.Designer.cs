namespace DataMaker
{
    partial class AddPersonnel
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
            this.AddPersonneltextBox = new System.Windows.Forms.TextBox();
            this.AddPersonnelprogressBar = new System.Windows.Forms.ProgressBar();
            this.AddPersonnelbutton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.loginNametextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AddUsertextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "添加人员数:";
            // 
            // AddPersonneltextBox
            // 
            this.AddPersonneltextBox.Location = new System.Drawing.Point(94, 5);
            this.AddPersonneltextBox.Name = "AddPersonneltextBox";
            this.AddPersonneltextBox.Size = new System.Drawing.Size(100, 21);
            this.AddPersonneltextBox.TabIndex = 1;
            this.AddPersonneltextBox.Text = "1000";
            // 
            // AddPersonnelprogressBar
            // 
            this.AddPersonnelprogressBar.Location = new System.Drawing.Point(25, 88);
            this.AddPersonnelprogressBar.Name = "AddPersonnelprogressBar";
            this.AddPersonnelprogressBar.Size = new System.Drawing.Size(168, 23);
            this.AddPersonnelprogressBar.TabIndex = 2;
            // 
            // AddPersonnelbutton
            // 
            this.AddPersonnelbutton.Location = new System.Drawing.Point(66, 117);
            this.AddPersonnelbutton.Name = "AddPersonnelbutton";
            this.AddPersonnelbutton.Size = new System.Drawing.Size(75, 23);
            this.AddPersonnelbutton.TabIndex = 3;
            this.AddPersonnelbutton.Text = "确定";
            this.AddPersonnelbutton.UseVisualStyleBackColor = true;
            this.AddPersonnelbutton.Click += new System.EventHandler(this.AddPersonnelbutton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "帐号起始:";
            // 
            // loginNametextBox
            // 
            this.loginNametextBox.Location = new System.Drawing.Point(94, 55);
            this.loginNametextBox.Name = "loginNametextBox";
            this.loginNametextBox.Size = new System.Drawing.Size(100, 21);
            this.loginNametextBox.TabIndex = 5;
            this.loginNametextBox.Text = "100000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "添加用户数:";
            // 
            // AddUsertextBox
            // 
            this.AddUsertextBox.Location = new System.Drawing.Point(94, 30);
            this.AddUsertextBox.Name = "AddUsertextBox";
            this.AddUsertextBox.Size = new System.Drawing.Size(100, 21);
            this.AddUsertextBox.TabIndex = 7;
            this.AddUsertextBox.Text = "200";
            // 
            // AddPersonnel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 146);
            this.Controls.Add(this.AddUsertextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.loginNametextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AddPersonnelbutton);
            this.Controls.Add(this.AddPersonnelprogressBar);
            this.Controls.Add(this.AddPersonneltextBox);
            this.Controls.Add(this.label1);
            this.Name = "AddPersonnel";
            this.Text = "AddPersonnel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AddPersonneltextBox;
        private System.Windows.Forms.ProgressBar AddPersonnelprogressBar;
        private System.Windows.Forms.Button AddPersonnelbutton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox loginNametextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AddUsertextBox;
    }
}