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
    public partial class CascadeFm : Form
    {
        private MySQLDBHelper _Mysql = null;
        public CascadeFm()
        {
            InitializeComponent();
        }
        public CascadeFm(MySQLDBHelper Mysql)
        {
            _Mysql = Mysql;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string mysql = null;
            string[] sArray = IPtextBox.Text.Split('.');
            int IPCount = Int32.Parse(sArray[3]);
            for (int i = 1; i <= Int32.Parse(CounttextBox.Text); i++)
            {
                string DevIP = string.Format("{0}.{1}.{2}.{3}", sArray[0], sArray[1], sArray[2], IPCount);
                string Region = DevIP + "下级";
                mysql = mysql = "INSERT INTO uplink_config (NodeType,ScheduledServiceIP,RealTimeServiceIP,Description,Region,id) VALUES ('1','" + DevIP + "','" + DevIP + "','" + DevIP + "','" + Region + "','" + Guid.NewGuid().ToString().Replace("-", "") + "')";

                _Mysql.ExcuteNoneQuery(mysql);
                IPCount++;
            }
            
        }
    }
}
