using Mysql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataMaker
{
    public partial class AddOrgFm : Form
    {
        private string _NodeTag = null;
        private MySQLDBHelper _Mysql = null;
        public AddOrgFm()
        {
            InitializeComponent();
        }
        public AddOrgFm(string NodeTag,MySQLDBHelper Mysql)
        {
            _NodeTag = NodeTag;
            _Mysql = Mysql;
            InitializeComponent();
        }
        private void AddOrgbutton_Click(object sender, EventArgs e)
        {
            int count = 0;
            string pid = null;
            AddOrgprogressBar.Maximum = Int32.Parse(AddOrgCounttextBox.Text);
            AddOrgprogressBar.Value = 0;
            AddOrgprogressBar.Step = 1;
            for (count = 1; count <= Int32.Parse(AddOrgCounttextBox.Text); count++)
            {
                //string OrgName = OrgNametextBox.Text + "_" + Guid.NewGuid().ToString().Replace("-", "").Substring(0,16);
                string OrgName = OrgNametextBox.Text + "_" + count.ToString();
                string id = Guid.NewGuid().ToString().Replace("-", "");
               // if (count == 1) pid = "e0ec2383aa0e410fbc942ddbc6f45f4f";
                string mysql = "INSERT INTO org_dept(id,bczt,Code,DutyMemberId,Province,City,Addr,OrgType,name,pid,name1) VALUES ('" + id + "',1,'" + Guid.NewGuid().ToString().Replace("-", "") + "',NULL,'a2e455be772f4b0eb2a37f29b4755828','2b33927016314ec1ac0014ed15e23505','" + OrgName + "','2','" + OrgName + "',NULL,'" + OrgName + "');";
                SqlStringtextBox.Text += string.Format("{0}{1}", mysql, Environment.NewLine);
              
                _Mysql.ExcuteNoneQuery(mysql);
                AddOrgprogressBar.Value += AddOrgprogressBar.Step;//让进度条增加一次
            }
            MessageBox.Show("添加成功");
        }
    }
}
