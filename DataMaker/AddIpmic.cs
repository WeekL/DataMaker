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
using DataMaker.bean;
using DataMaker.util;

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
            thread.IsBackground = true;
            thread.Start();
        }
        private void doWork()
        {
            int dataCount = Int32.Parse(AddIpmicNumtextBox.Text);
            int IpNo = Int32.Parse(AddIpmicNotextBox.Text);
            string[] sArray = AddIpmicIpAddresstextBox.Text.Split('.');
            int IPCount = Int32.Parse(sArray[3]);
            AddIpmicprogressBar.Maximum = dataCount;
            AddIpmicprogressBar.Value = 0;
            AddIpmicprogressBar.Step = 1;

            for (int i = 0; i < dataCount;)
            {
                int left = dataCount - i;
                int count = left > AddDeviceHelper.ONCE_INSERT ? AddDeviceHelper.ONCE_INSERT : left;
                List<Device> devices = new List<Device>();
                for (int j = 0; j < count; j++, i++)
                {
                    CallMic mic = new CallMic();
                    mic.setDeviceCode(IpNo + i.ToString());
                    mic.setDeviceName("对讲话筒" + i.ToString());
                    mic.setParam("ResourceID", IpNo++ + i.ToString());
                    mic.setIp(string.Format("{0}.{1}.{2}.{3}", sArray[0], sArray[1], sArray[2], IPCount++));
                    devices.Add(mic);

                    AddIpmicprogressBar.Value += AddIpmicprogressBar.Step;//让进度条增加一次
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
