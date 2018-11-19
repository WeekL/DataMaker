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
    public partial class AddACSDevFm : Form
    {
        private string _NodeTag = null;
        private MySQLDBHelper _Mysql = null;
        public AddACSDevFm()
        {
            InitializeComponent();
        }
        public AddACSDevFm(string NodeTag, MySQLDBHelper Mysql)
        {
            _NodeTag = NodeTag;
            _Mysql = Mysql;
            InitializeComponent();
        }
        private string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                result += Convert.ToString(b[i], 16);
            }
            return result;
        }
        private void AddACSDevFmbutton_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            ThreadStart start = new ThreadStart(doWork);
            Thread thread = new Thread(start);
            thread.Start();
        }

        private void doWork()
        {
            string name = "门禁主机";//资产名称的前缀
            string deviceCode = StringUtil.getDateTimeNum();//资产编号起始
            int dataCount = Int32.Parse(AddACSDevNumtextBox.Text);//数据生成数量
            string[] sArray = AddACSDevIpAddresstextBox.Text.Split('.');
            int Port = Int32.Parse(PorttextBox.Text);
            int IPCount = Int32.Parse(sArray[3]);//ip尾号
            long IOTCtrlid = Int64.Parse(AddACSDevUIDtextBox.Text);
            string ctlId = StringToHexString(IOTCtrlid.ToString(), Encoding.UTF8);
            int PortCount = Int32.Parse(PorttextBox.Text);

            AddACSDevprogressBar.Maximum = dataCount;
            AddACSDevprogressBar.Value = 0;
            AddACSDevprogressBar.Step = 1;

            for (int i = 1; i <= dataCount;)
            {
                int left = dataCount - i;
                int count = left > AddDeviceHelper.ONCE_INSERT ? AddDeviceHelper.ONCE_INSERT : left;
                List<Device> devices = new List<Device>();
                for (int j = 0; j < count; j++,i++)
                {
                    AccessDevice acs = new AccessDevice();
                    acs.setIp(string.Format("{0}.{1}.{2}.{3}", sArray[0], sArray[1], sArray[2], IPCount));
                    acs.setPort(Port.ToString());
                    acs.setDeviceCode(deviceCode + i.ToString());
                    acs.setCtlId(ctlId);
                    acs.setDeviceName(name + i.ToString());
                    acs.setDoorCount(4);//4扇门
                    devices.Add(acs);

                    PortCount++;
                    IOTCtrlid++;
                    //IPCount++;
                    AddACSDevprogressBar.Value += AddACSDevprogressBar.Step;//让进度条增加一次
                }
                AddDeviceHelper.multiExcute(LoginMysqlFm.getNewDbHelper(), devices);
            }
            MessageBox.Show("添加成功");
        }
    }
}
