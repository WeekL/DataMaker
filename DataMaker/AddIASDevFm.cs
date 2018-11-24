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
    public partial class AddIASDevFm : Form
    {
        private string _NodeTag = null;
        private MySQLDBHelper _Mysql = null;
        public AddIASDevFm()
        {
            InitializeComponent();
        }
        public AddIASDevFm(string NodeTag, MySQLDBHelper Mysql)
        {
            _NodeTag = NodeTag;
            _Mysql = Mysql;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            ThreadStart start = new ThreadStart(doWork);
            Thread thread = new Thread(start);
            thread.IsBackground = true;
            thread.Start();
        }

        private void doWork()
        {
            int dataCount = Int32.Parse(AddDevNumtextBox.Text);
            IASprogressBar.Maximum = dataCount;
            IASprogressBar.Value = 0;
            IASprogressBar.Step = 1;
            string[] sArray = AddIpAddresstextBox.Text.Split('.');
            int Port = Int32.Parse(AddPorttextBox.Text);
            int IPCount = Int32.Parse(sArray[3]);
            string deviceCode = StringUtil.getDateTimeNum();//资产编号起始
            string name = "智能分析";
            for (int i = 0; i < dataCount;)
            {
                int left = dataCount - i;
                int count = left > AddDeviceHelper.ONCE_INSERT ? AddDeviceHelper.ONCE_INSERT : left;
                List<Device> devices = new List<Device>();
                for (int j = 0; j < count; j++,i++)
                {
                    InsightDevice insight = new InsightDevice();
                    insight.setIp(string.Format("{0}.{1}.{2}.{3}", sArray[0], sArray[1], sArray[2], IPCount));
                    insight.setPort(Port++.ToString());
                    insight.setDeviceCode(deviceCode + i.ToString());
                    insight.setDeviceName(name + i.ToString());
                    insight.setIasChnCount(4);
                    devices.Add(insight);

                    IASprogressBar.Value += IASprogressBar.Step;//让进度条增加一次
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
