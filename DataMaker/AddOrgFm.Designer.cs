namespace DataMaker
{
    partial class AddOrgFm
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
            this.OrgNametextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AddOrgCounttextBox = new System.Windows.Forms.TextBox();
            this.AddOrgbutton = new System.Windows.Forms.Button();
            this.AddOrgprogressBar = new System.Windows.Forms.ProgressBar();
            this.SqlStringtextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "组织机构名称:";
            // 
            // OrgNametextBox
            // 
            this.OrgNametextBox.Location = new System.Drawing.Point(93, 25);
            this.OrgNametextBox.Name = "OrgNametextBox";
            this.OrgNametextBox.Size = new System.Drawing.Size(125, 21);
            this.OrgNametextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(224, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "添加数量:";
            // 
            // AddOrgCounttextBox
            // 
            this.AddOrgCounttextBox.Location = new System.Drawing.Point(279, 25);
            this.AddOrgCounttextBox.Name = "AddOrgCounttextBox";
            this.AddOrgCounttextBox.Size = new System.Drawing.Size(58, 21);
            this.AddOrgCounttextBox.TabIndex = 3;
            // 
            // AddOrgbutton
            // 
            this.AddOrgbutton.Location = new System.Drawing.Point(12, 389);
            this.AddOrgbutton.Name = "AddOrgbutton";
            this.AddOrgbutton.Size = new System.Drawing.Size(75, 23);
            this.AddOrgbutton.TabIndex = 4;
            this.AddOrgbutton.Text = "确定";
            this.AddOrgbutton.UseVisualStyleBackColor = true;
            this.AddOrgbutton.Click += new System.EventHandler(this.AddOrgbutton_Click);
            // 
            // AddOrgprogressBar
            // 
            this.AddOrgprogressBar.Location = new System.Drawing.Point(14, 71);
            this.AddOrgprogressBar.Name = "AddOrgprogressBar";
            this.AddOrgprogressBar.Size = new System.Drawing.Size(323, 23);
            this.AddOrgprogressBar.TabIndex = 5;
            // 
            // SqlStringtextBox
            // 
            this.SqlStringtextBox.Location = new System.Drawing.Point(14, 114);
            this.SqlStringtextBox.Multiline = true;
            this.SqlStringtextBox.Name = "SqlStringtextBox";
            this.SqlStringtextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SqlStringtextBox.Size = new System.Drawing.Size(997, 269);
            this.SqlStringtextBox.TabIndex = 6;
            // 
            // AddOrgFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 424);
            this.Controls.Add(this.SqlStringtextBox);
            this.Controls.Add(this.AddOrgprogressBar);
            this.Controls.Add(this.AddOrgbutton);
            this.Controls.Add(this.AddOrgCounttextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OrgNametextBox);
            this.Controls.Add(this.label1);
            this.Name = "AddOrgFm";
            this.Text = "AddOrg";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox OrgNametextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AddOrgCounttextBox;
        private System.Windows.Forms.Button AddOrgbutton;
        private System.Windows.Forms.ProgressBar AddOrgprogressBar;
        private System.Windows.Forms.TextBox SqlStringtextBox;
    }
}