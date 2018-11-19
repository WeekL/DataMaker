using DataMaker.util;
using Mysql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMaker.bean
{
    class VideoDevice : Device
    {
        private static string head;

        public VideoDevice()
            : base("video_DeviceBase")
        {
            //sHead = "devType,UserName,Pwd";
            //sValue = "'3','admin','1'";
            setParam("devType", "3");
            setUserName("admin");
            setPwd("1");
            deviceBase.setDeviceMainType("110");
            deviceBase.setDeviceType("111");
        }

        public void setChannelCount(int count)
        {
            List<Device> childs = new List<Device>();
            for (int i = 0; i < count; i++)
            {
                Channel channel = new Channel();
                channel.setDeviceCode("0" + this.deviceBase.getDeviceCode() + i.ToString());
                channel.setDeviceName("视频通道" + i.ToString());
                channel.setFatherCode(getId());
                //channel.excute(db);
                childs.Add(channel);
            }
            childList.Add(childs);
        }

        public override string initHead()
        {
            if (head == null)
            {
                head = base.buildSQLHead();
            }
            return head;
        }
    }

    class Channel : Device
    {
        private static string head;

        public Channel() 
            : base("video_Channel")
        {
            //sHead = "DevType,UserName,Pwd,DevIP,DevPort,DevNum";
            //sValue = "'0','admin','1','192.168.0.0','8000','1'";
            setUserName("admin");
            setPwd("1");
            setIp("192.168.0.0");
            setPort("8000");
            setParam("DevType", "0");
            setParam("DevNum", "1");
            deviceBase.setDeviceMainType("110");
            deviceBase.setDeviceType("131");
        }

        public override string initHead()
        {
            if (head == null)
            {
                head = base.buildSQLHead();
            }
            return head;
        }
    }
}
