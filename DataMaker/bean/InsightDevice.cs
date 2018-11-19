using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mysql;

namespace DataMaker.bean
{
    class InsightDevice : Device
    {
        private static string head;

        public InsightDevice()
            : base("ias_devicebase_mc")
        {
            //sHead = "BrandId,ChannelCount,ClientId,UserName,Pwd";
            //sValue = "'BRAND_IAS_MC_HYLINUX','4','default','admin','1'";
            setParam("BrandId", "BRAND_IAS_MC_HYLINUX");
            setParam("ChannelCount", "4");
            setParam("ClientId", "default");
            setUserName("admin");
            setPwd("1");
            deviceBase.setDeviceMainType("530");
            deviceBase.setDeviceType("531");
        }

        public override string initHead()
        {
            if (head == null)
            {
                head = base.buildSQLHead();
            }
            return head;
        }

        public void setIasChnCount(int count)
        {
            List<Device> childs = new List<Device>();
            for (int i = 0; i < count; i++)
            {
                IasChn chn = new IasChn();
                chn.setFatherCode(getId());
                chn.setDeviceCode("0" + this.deviceBase.getDeviceCode() + i.ToString());
                chn.setDeviceName("智能分析通道" + i.ToString());
                childs.Add(chn);
            }
            childList.Add(childs);
        }

        public new void setIp(string ip)
        {
            setParam("IPAdress", ip);
        }

        public new void setPort(string port)
        {
            setParam("Port", port);
        }

        //public new void excute(MySQLDBHelper db)
        //{
        //    base.excute(db);
        //    for (int i = 0; i < chnCount; i++)
        //    {
        //        IasChn chn = new IasChn();
        //        chn.setFatherCode(getId());
        //        chn.setDeviceCode("0" + this.deviceBase.getDeviceCode() + i.ToString());
        //        chn.setDeviceName("智能分析通道" + i.ToString());
        //        chn.excute(db);
        //    }
        //}
    }

    class IasChn : Device
    {
        private static string head;
        public IasChn()
            : base("ias_devicebase_chn")
        {
            deviceBase.setDeviceMainType("530");
            deviceBase.setDeviceType("533");
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
