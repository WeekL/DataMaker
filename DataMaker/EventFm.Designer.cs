namespace DataMaker
{
    partial class EventFm
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
            this.Eventbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DevTypecomboBox = new System.Windows.Forms.ComboBox();
            this.EventTypecomboBox = new System.Windows.Forms.ComboBox();
            this.EventGradecomboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.GroupcomboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TimecomboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.LinkagecomboBox = new System.Windows.Forms.ComboBox();
            this.EventprogressBar = new System.Windows.Forms.ProgressBar();
            this.label7 = new System.Windows.Forms.Label();
            this.RelationDevcomboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.RolecomboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Eventbutton
            // 
            this.Eventbutton.Location = new System.Drawing.Point(99, 291);
            this.Eventbutton.Name = "Eventbutton";
            this.Eventbutton.Size = new System.Drawing.Size(75, 23);
            this.Eventbutton.TabIndex = 0;
            this.Eventbutton.Text = "策略配置";
            this.Eventbutton.UseVisualStyleBackColor = true;
            this.Eventbutton.Click += new System.EventHandler(this.Eventbutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "事件类型:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "事件等级:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "设备类型:";
            // 
            // DevTypecomboBox
            // 
            this.DevTypecomboBox.FormattingEnabled = true;
            this.DevTypecomboBox.Location = new System.Drawing.Point(99, 18);
            this.DevTypecomboBox.Name = "DevTypecomboBox";
            this.DevTypecomboBox.Size = new System.Drawing.Size(135, 20);
            this.DevTypecomboBox.TabIndex = 4;
            this.DevTypecomboBox.SelectedIndexChanged += new System.EventHandler(this.DevTypecomboBox_SelectedIndexChanged);
            // 
            // EventTypecomboBox
            // 
            this.EventTypecomboBox.FormattingEnabled = true;
            this.EventTypecomboBox.Location = new System.Drawing.Point(99, 48);
            this.EventTypecomboBox.Name = "EventTypecomboBox";
            this.EventTypecomboBox.Size = new System.Drawing.Size(135, 20);
            this.EventTypecomboBox.TabIndex = 5;
            this.EventTypecomboBox.SelectedIndexChanged += new System.EventHandler(this.EventTypecomboBox_SelectedIndexChanged);
            // 
            // EventGradecomboBox
            // 
            this.EventGradecomboBox.FormattingEnabled = true;
            this.EventGradecomboBox.Location = new System.Drawing.Point(99, 76);
            this.EventGradecomboBox.Name = "EventGradecomboBox";
            this.EventGradecomboBox.Size = new System.Drawing.Size(135, 20);
            this.EventGradecomboBox.TabIndex = 6;
            this.EventGradecomboBox.SelectedIndexChanged += new System.EventHandler(this.EventGradecomboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "组合类型:";
            // 
            // GroupcomboBox
            // 
            this.GroupcomboBox.FormattingEnabled = true;
            this.GroupcomboBox.Location = new System.Drawing.Point(99, 105);
            this.GroupcomboBox.Name = "GroupcomboBox";
            this.GroupcomboBox.Size = new System.Drawing.Size(135, 20);
            this.GroupcomboBox.TabIndex = 8;
            this.GroupcomboBox.SelectedIndexChanged += new System.EventHandler(this.GroupcomboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "时间模板:";
            // 
            // TimecomboBox
            // 
            this.TimecomboBox.FormattingEnabled = true;
            this.TimecomboBox.Location = new System.Drawing.Point(99, 131);
            this.TimecomboBox.Name = "TimecomboBox";
            this.TimecomboBox.Size = new System.Drawing.Size(135, 20);
            this.TimecomboBox.TabIndex = 10;
            this.TimecomboBox.SelectedIndexChanged += new System.EventHandler(this.TimecomboBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "联动模板:";
            // 
            // LinkagecomboBox
            // 
            this.LinkagecomboBox.FormattingEnabled = true;
            this.LinkagecomboBox.Location = new System.Drawing.Point(99, 158);
            this.LinkagecomboBox.Name = "LinkagecomboBox";
            this.LinkagecomboBox.Size = new System.Drawing.Size(135, 20);
            this.LinkagecomboBox.TabIndex = 12;
            this.LinkagecomboBox.SelectedIndexChanged += new System.EventHandler(this.LinkagecomboBox_SelectedIndexChanged);
            // 
            // EventprogressBar
            // 
            this.EventprogressBar.Location = new System.Drawing.Point(36, 262);
            this.EventprogressBar.Name = "EventprogressBar";
            this.EventprogressBar.Size = new System.Drawing.Size(198, 23);
            this.EventprogressBar.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 196);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "关联设备:";
            // 
            // RelationDevcomboBox
            // 
            this.RelationDevcomboBox.FormattingEnabled = true;
            this.RelationDevcomboBox.Location = new System.Drawing.Point(99, 193);
            this.RelationDevcomboBox.Name = "RelationDevcomboBox";
            this.RelationDevcomboBox.Size = new System.Drawing.Size(135, 20);
            this.RelationDevcomboBox.TabIndex = 15;
            this.RelationDevcomboBox.SelectedIndexChanged += new System.EventHandler(this.RelationDevcomboBox_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 221);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "角色:";
            // 
            // RolecomboBox
            // 
            this.RolecomboBox.FormattingEnabled = true;
            this.RolecomboBox.Location = new System.Drawing.Point(99, 218);
            this.RolecomboBox.Name = "RolecomboBox";
            this.RolecomboBox.Size = new System.Drawing.Size(135, 20);
            this.RolecomboBox.TabIndex = 17;
            this.RolecomboBox.SelectedIndexChanged += new System.EventHandler(this.RolecomboBox_SelectedIndexChanged);
            // 
            // EventFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 326);
            this.Controls.Add(this.RolecomboBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.RelationDevcomboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.EventprogressBar);
            this.Controls.Add(this.LinkagecomboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TimecomboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.GroupcomboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EventGradecomboBox);
            this.Controls.Add(this.EventTypecomboBox);
            this.Controls.Add(this.DevTypecomboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Eventbutton);
            this.Name = "EventFm";
            this.Text = "Event";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Eventbutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox DevTypecomboBox;
        private System.Windows.Forms.ComboBox EventTypecomboBox;
        private System.Windows.Forms.ComboBox EventGradecomboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox GroupcomboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox TimecomboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox LinkagecomboBox;
        private System.Windows.Forms.ProgressBar EventprogressBar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox RelationDevcomboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox RolecomboBox;
    }
}