using Mysql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DataMaker
{
    public partial class AddIpmic : Form
    {
        private string _NodeTag = null;
        private MySQLDBHelper _Mysql = null;
        public AddIpmic()
        {
            InitializeComponent();
        }
        public AddIpmic(string NodeTag, MySQLDBHelper Mysql)
        {
            _NodeTag = NodeTag;
            _Mysql = Mysql;
            InitializeComponent();
        }
        private void AddIpmicbutton_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            ThreadStart start = new ThreadStart(doWork);
            Thread thread = new Thread(start);
            thread.Start();
        }
        private void doWork()
        {
            int DevCount = 0;
            int IpNo = Int32.Parse(AddIpmicNotextBox.Text);
            AddIpmicprogressBar.Maximum = Int32.Parse(AddIpmicNumtextBox.Text);
            AddIpmicprogressBar.Value = 0;
            AddIpmicprogressBar.Step = 1;
            string[] sArray = AddIpmicIpAddresstextBox.Text.Split('.');
            int IPCount = Int32.Parse(sArray[3]);
            string DevID = null;
            //string Ctrlid = string.Empty;
            string mysql = string.Empty;
            //DataTable dt = _Mysql.GetDataTable(mysql);
            //string fbsconfigId = (string)dt.Rows[0]["id"];
            //string fbsconfigBrandCode = (string)dt.Rows[0]["BrandCode"];
            for (DevCount = 1; DevCount <= Int32.Parse(AddIpmicNumtextBox.Text); DevCount++)
            {
                string DevName = "性能测试IP对讲寻呼话筒_" + DevCount.ToString();
                string DeviceType = "553";
                DevID = Guid.NewGuid().ToString().Replace("-", "");
                //写sys_devicebaseIP对讲寻呼话筒
                mysql = "INSERT INTO sys_devicebase (id,bczt,name,DeviceType,OrgID) VALUES ('" + DevID + "',1,'" + DevName + "','" + DeviceType + "','" + _NodeTag + "')";
                _Mysql.ExcuteNoneQuery(mysql);
                //写call_micconfig对讲寻呼话筒
                string DevIP = string.Format("{0}.{1}.{2}.{3}", sArray[0], sArray[1], sArray[2], IPCount);
                //Ctrlid = Guid.NewGuid().ToString("N").ToUpper().Substring(1, 16);
                mysql = "INSERT INTO call_devicebase_mic (id,bczt,fid,DevNum,Param,DevPort,DevIP,ResourceID,showOrder) VALUES ('" + Guid.NewGuid().ToString().Replace("-", "") + "',0,'" + DevID + "','" + IpNo.ToString() + "','Brand_CALL_MC_HYG3','5060','" + DevIP + "','" + IpNo.ToString() + "','1')";
                _Mysql.ExcuteNoneQuery(mysql);

                IPCount++;
                IpNo++;
                AddIpmicprogressBar.Value += AddIpmicprogressBar.Step;//让进度条增加一次
            }
            MessageBox.Show("添加成功");
        }
    }    
}
