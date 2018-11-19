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
    public partial class AddVideoDevFm : Form
    {
        private string _NodeTag = null;
        private MySQLDBHelper _Mysql = null;
        public AddVideoDevFm()
        {
            InitializeComponent();
        }
        public AddVideoDevFm(string NodeTag, MySQLDBHelper Mysql)
        {
            _NodeTag = NodeTag;
            _Mysql = Mysql;
            InitializeComponent();
        }

        //添加视频主机
        private void AddVideoDevbutton_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            ThreadStart start = new ThreadStart(doWork);
            Thread thread = new Thread(start);
            thread.Start();
        }

        private void doWork()
        {
            int dataCount = Int32.Parse(AddDevNumtextBox.Text);//数据生成数量
            this.Invoke((EventHandler)delegate { this.AddDevprogressBar.Minimum = 0; });
            this.Invoke((EventHandler)delegate { this.AddDevprogressBar.Step = 1; });
            this.Invoke((EventHandler)delegate { this.AddDevprogressBar.Maximum = dataCount; });
            //AddDevprogressBar.Maximum = dataCount;
            string name = "视频主机";//资产名称的前缀
            string deviceCode = StringUtil.getDateTimeNum();//资产编码和设备唯一标识码的前缀

            string[] sArray = AddIpAddresstextBox.Text.Split('.');
            int Port = Int32.Parse(AddPorttextBox.Text);
            int IPCount = Int32.Parse(sArray[3]);//ip尾号
            int channelCount = Int32.Parse(AddChanneltextBox.Text);//通道数量

            for (int i = 1; i <= dataCount;)
            {
                int left = dataCount - i;
                int count = left > AddDeviceHelper.ONCE_INSERT ? AddDeviceHelper.ONCE_INSERT : left;
                List<Device> devices = new List<Device>();
                for (int j = 0; j < count; j++,i++)
                {
                    VideoDevice video = new VideoDevice();
                    video.setIp(string.Format("{0}.{1}.{2}.{3}", sArray[0], sArray[1], sArray[2], IPCount++));
                    video.setPort(Port.ToString());
                    video.setDeviceCode(deviceCode + i.ToString());
                    video.setDeviceName(name + i.ToString());
                    video.setChannelCount(channelCount);
                    devices.Add(video);

                    this.Invoke((EventHandler)delegate { this.AddDevprogressBar.Value += AddDevprogressBar.Step; });
                }
                AddDeviceHelper.multiExcute(LoginMysqlFm.getNewDbHelper(), devices);
                //AddDevprogressBar.Value += AddDevprogressBar.Step;//让进度条增加一次
            }
            MessageBox.Show("添加成功");
        }
    }
}
