using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mysql;

namespace DataMaker.bean
{
    class AlarmDevice : Device
    {
        private static string head;

        public AlarmDevice() : base("alm_DeviceBase")
        {
            //sHead = "BrandCode,ClientId,SectorNum,SubSystemNumber,DevicePort,UserName,Pwd";
            //sValue = "'Brand_ALM_MC_DaHua','Client_ALM_DaHua_0001','4','4','8000','admin','1'";
            setParam("BrandCode", "Brand_ALM_MC_DaHua");
            setParam("ClientId", "Client_ALM_DaHua_0001");
            setParam("SectorNum", "4");
            setParam("SubSystemNumber", "4");
            setUserName("admin");
            setParam("Paw","2");
            deviceBase.setDeviceMainType("510");
            deviceBase.setDeviceType("511");
        }

        public override string initHead()
        {
            if (head == null)
            {
                head = base.buildSQLHead();
            }
            return head;
        }

        public void setSubSysCount(int count)
        {
            List<Device> childs = new List<Device>();
            for (int i = 0; i < count; i++)
            {
                SubSystem sub = new SubSystem();
                sub.setDeviceCode("0" + this.deviceBase.getDeviceCode() + i.ToString());
                sub.setDeviceName("防区分组" + i.ToString());
                sub.setFatherCode(getId());
                childs.Add(sub);
            }
            childList.Add(childs);
        }

        public void setSectorCount(int count)
        {
            List<Device> childs = new List<Device>();
            for (int i = 0; i < count; i++)
            {
                Sector sector = new Sector();
                sector.setDeviceCode("1" + this.deviceBase.getDeviceCode() + i.ToString());
                sector.setDeviceName("防区" + i.ToString());
                sector.setFatherCode(getId());
                childs.Add(sector);
            }
            childList.Add(childs);
        }
        public new void setIp(string ip)
        {
            setParam("DeviceIP", ip);
        }

        public new void setPort(string port)
        {
            setParam("DevicePort", port);
        }

        //public new void excute(MySQLDBHelper db)
        //{
        //    base.excute(db);
        //    for (int i = 0; i < subCount; i++)
        //    {
        //        SubSystem sub = new SubSystem();
        //        sub.setDeviceCode("0" + this.deviceBase.getDeviceCode() + i.ToString());
        //        sub.setDeviceName("防区分组" + i.ToString());
        //        sub.setFatherCode(getId());
        //        sub.excute(db);
        //        Sector sector = new Sector();
        //        sector.setDeviceCode("1" + this.deviceBase.getDeviceCode() + i.ToString());
        //        sector.setDeviceName("防区" + i.ToString());
        //        sector.setFatherCode(getId());
        //        sector.excute(db);
        //    }
        //}
    }

    class SubSystem : Device
    {
        private static string head;
        public SubSystem()
            : base("alm_SubSystem")
        {
            //sHead = "SubSystemNo";
            //sValue = "'1'";
            setParam("SubSystemNo", "1");
            deviceBase.setDeviceMainType("510");
            deviceBase.setDeviceType("512");
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

    class Sector : Device
    {
        private static string head;
        public Sector()
            : base("alm_Sector")
        {
            //sHead = "showOrder,ArmNumber";
            //sValue = "'4','1'";
            setParam("showOrder", "4");
            setParam("ArmNumber", "1");
            deviceBase.setDeviceMainType("510");
            deviceBase.setDeviceType("513");
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
