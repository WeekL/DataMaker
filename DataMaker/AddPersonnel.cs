using Mysql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Automation;
namespace DataMaker
{
    public partial class AddPersonnel : Form
    {
        private string _NodeTag = null;
        private MySQLDBHelper _Mysql = null;
        public AddPersonnel()
        {
            InitializeComponent();
        }
        public AddPersonnel(string NodeTag, MySQLDBHelper Mysql)
        {
            _NodeTag = NodeTag;
            _Mysql = Mysql;
            InitializeComponent();
        }
        private List<string> GenerateChineseWords(int count)
        {
            List<string> chineseWords = new List<string>();
            Random rm = new Random();
            Encoding gb = Encoding.GetEncoding("gb2312");

            for (int i = 0; i < count; i++)
            {
                // 获取区码(常用汉字的区码范围为16-55)  
                int regionCode = rm.Next(16, 56);
                // 获取位码(位码范围为1-94 由于55区的90,91,92,93,94为空,故将其排除)  
                int positionCode;
                if (regionCode == 55)
                {
                    // 55区排除90,91,92,93,94  
                    positionCode = rm.Next(1, 90);
                }
                else
                {
                    positionCode = rm.Next(1, 95);
                }

                // 转换区位码为机内码  
                int regionCode_Machine = regionCode + 160;// 160即为十六进制的20H+80H=A0H  
                int positionCode_Machine = positionCode + 160;// 160即为十六进制的20H+80H=A0H  

                // 转换为汉字  
                byte[] bytes = new byte[] { (byte)regionCode_Machine, (byte)positionCode_Machine };
                chineseWords.Add(gb.GetString(bytes));
            }

            return chineseWords;
        }  
        private void AddPersonnelbutton_Click(object sender, EventArgs e)
        {
            int Count = 0;
            string Name = string.Empty;
            List<string> lstStr = null;
            int loginName = Int32.Parse(loginNametextBox.Text);
            AddPersonnelprogressBar.Maximum = Int32.Parse(AddPersonneltextBox.Text);
            AddPersonnelprogressBar.Value = 0;
            AddPersonnelprogressBar.Step = 1;
            for (Count = 1; Count <= Int32.Parse(AddPersonneltextBox.Text); Count++)
            {
                string PersonnelID = Guid.NewGuid().ToString().Replace("-", "");
                lstStr = GenerateChineseWords(3);
                Name = lstStr[0] + lstStr[1] + lstStr[2];
                string mysql = "INSERT INTO hr_member (id,bczt,Name,OrgId,AccountType,National,PoliticalLandscape,PostStatus) VALUES ('" + PersonnelID + "','1','" + Name + "','" + _NodeTag + "','2','1','1','1')";
                _Mysql.ExcuteNoneQuery(mysql);
                if(Count <= Int32.Parse(AddUsertextBox.Text))
                {
                    mysql = "INSERT INTO usr_account (id,bczt,username,loginname,password,MemberId) VALUES ('" + Guid.NewGuid().ToString().Replace("-", "") + "','1','" + Name + "','" + loginName.ToString() + "','/fOMjRWQGQ2mGvPcfFNWdA==','" + PersonnelID + "')";
                    _Mysql.ExcuteNoneQuery(mysql);
                    loginName++;
                }
                AddPersonnelprogressBar.Value += AddPersonnelprogressBar.Step;//让进度条增加一次
            }
            MessageBox.Show("添加成功");
        }
    }
}
