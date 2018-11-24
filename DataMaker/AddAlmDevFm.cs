using DataMaker.bean;
using DataMaker.util;
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
    public partial class AddAlmDevFm : Form
    {
        private string _NodeTag = null;
        private MySQLDBHelper _Mysql = null;
        public AddAlmDevFm()
        {
            InitializeComponent();
        }
        public AddAlmDevFm(string NodeTag, MySQLDBHelper Mysql)
        {
            _NodeTag = NodeTag;
            _Mysql = Mysql;
            InitializeComponent();
        }
        private void AddAlmDevbutton_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            ThreadStart start = new ThreadStart(doWork);
            Thread thread = new Thread(start);
            thread.IsBackground = true;
            thread.Start();
        }

        private void doWork()
        {
            string name = "入侵报警主机";//资产名称的前缀
            long deviceCode = Int32.Parse(textBox1.Text);//资产编号起始
            int dataCount = Int32.Parse(AddNumtextBox.Text);//数据生成数量

            string[] sArray = IpAddresstextBox.Text.Split('.');
            int Port = Int32.Parse(PorttextBox.Text);
            int IPCount = Int32.Parse(sArray[3]);//ip尾号

            AddAlmDevprogressBar.Maximum = dataCount;
            AddAlmDevprogressBar.Step = 1;

            for (int i = 0; i < dataCount;)
            {
                int left = dataCount - i;
                int count = left > AddDeviceHelper.ONCE_INSERT ? AddDeviceHelper.ONCE_INSERT : left;
                List<Device> devices = new List<Device>();
                for (int j = 0; j < count; j++,i++)
                {
                    AlarmDevice alarm = new AlarmDevice();
                    alarm.setIp(string.Format("{0}.{1}.{2}.{3}", sArray[0], sArray[1], sArray[2], IPCount++));
                    alarm.setPort(Port.ToString());
                    alarm.setDeviceCode(deviceCode++.ToString());
                    alarm.setDeviceName(name + i.ToString());
                    alarm.setSectorCount(4);//一个报警主机带4个子系统和4个防区
                    alarm.setSubSysCount(4);
                    devices.Add(alarm);

                    AddAlmDevprogressBar.Value += AddAlmDevprogressBar.Step;//让进度条增加一次
                }
                AddDeviceHelper.multiExcute(LoginMysqlFm.getNewDbHelper(), devices);
            }
            MessageBox.Show("添加成功");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
            base.OnFormClosing(e);
        }
    }
}
