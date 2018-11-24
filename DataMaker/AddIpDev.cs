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
    public partial class AddIpDev : Form
    {
        private string _NodeTag = null;
        private MySQLDBHelper _Mysql = null;
        public AddIpDev()
        {
            InitializeComponent();
        }
        public AddIpDev(string NodeTag, MySQLDBHelper Mysql)
        {
            _NodeTag = NodeTag;
            _Mysql = Mysql;
            InitializeComponent();
        }

        private void AddIpdevbutton_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            ThreadStart start = new ThreadStart(doWork);
            Thread thread = new Thread(start);
            thread.IsBackground = true;
            thread.Start();
        }

        private void doWork()
        {
            int dataCount = Int32.Parse(AddIpdevNumtextBox.Text);//数据生成数量
            AddIpdevprogressBar.Maximum = dataCount;
            AddIpdevprogressBar.Value = 0;
            AddIpdevprogressBar.Step = 1;

            string name = "IP对讲主机";//资产名称的前缀
            string deviceCode = StringUtil.getDateTimeNum();//资产编号起始

            string[] sArray = AddIpDevIpAddresstextBox.Text.Split('.');
            int IPCount = Int32.Parse(sArray[3]);//ip尾号
            int IpNo = Int32.Parse(AddIpdevNotextBox.Text);

            for (int i = 0; i < dataCount;)
            {
                int left = dataCount - i;
                int count = left > AddDeviceHelper.ONCE_INSERT ? AddDeviceHelper.ONCE_INSERT : left;
                List<Device> devices = new List<Device>();
                for (int j = 0; j < count; j++,i++)
                {
                    IPCallDevice call = new IPCallDevice();
                    call.setIp(string.Format("{0}.{1}.{2}.{3}", sArray[0], sArray[1], sArray[2], IPCount++));
                    call.setPort("8080");
                    call.setDeviceCode(deviceCode + i.ToString());
                    call.setDeviceName(name + i.ToString());
                    call.setResourceId(IpNo++.ToString());
                    call.setPanelCount(6);
                    devices.Add(call);

                    AddIpdevprogressBar.Value += AddIpdevprogressBar.Step;//让进度条增加一次
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
