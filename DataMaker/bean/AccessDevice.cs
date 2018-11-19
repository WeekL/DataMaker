using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mysql;
using DataMaker.util;

namespace DataMaker.bean
{
    class AccessDevice : Device
    {
        private static string head;

        private string ctlId;

        public AccessDevice()
            : base("acs_DeviceBase")
        {
            //sHead = "BrandCode,UserName,Pwd,ClientId";
            //sValue = "'Brand_ACS_MC_HY','andmin','1','" + QuickLoadAcs.acsCfgList[0] + "'";
            setParam("BrandCode", "Brand_ACS_MC_HY");
            setUserName("admin");
            setPwd("1");
            setParam("ClientId", QuickLoadAcs.acsCfgList[0]);
            deviceBase.setDeviceMainType("571");
            deviceBase.setDeviceType("581");
        }

        public override string initHead()
        {
            if (head == null)
            {
                head = base.buildSQLHead();
            }
            return head;
        }

        public void setDoorCount(int count)
        {
            List<Device> childs = new List<Device>();
            for (int i = 0; i < count; i++)
            {
                AcsRes door = new AcsRes();
                door.setFatherCode(getId());
                door.setDeviceCode("0" + this.deviceBase.getDeviceCode() + i.ToString());
                door.setDeviceName("门禁门" + i.ToString());
                door.setParam("Ctrlid", ctlId);
                setParam("Resourceid", "0800000" + i.ToString());
                door.setParam("Rsn", "门禁门" + i.ToString());
                door.setParam("Rsaliasname", "门禁门" + i.ToString());
                childs.Add(door);
            }
            childList.Add(childs);
        }

        public void setCtlId(string id)
        {
            ctlId = id;
            setParam("Ctrlid", ctlId);
        }

        public new void setDeviceName(string name)
        {
            base.setDeviceName(name);
            setParam("Name", name);
        }

        public new void setIp(string ip)
        {
            setParam("LocalIP", ip);
        }

        public new void setPort(string port)
        {
            setParam("RevPort", port);
        }

        //public new void excute(MySQLDBHelper db)
        //{            
        //    base.excute(db);
        //    for (int i = 0; i < doorCount; i++)
        //    {
        //        AcsRes door = new AcsRes();
        //        door.setFatherCode(getId());
        //        door.setDeviceCode("0" + this.deviceBase.getDeviceCode() + i.ToString());
        //        door.setDeviceName("门禁门" + i.ToString());
        //        door.setParam("Ctrlid", ctlId);
        //        setParam("Resourceid", "0800000" + i.ToString());
        //        door.setParam("Rsn", "门禁门" + i.ToString());
        //        door.setParam("Rsaliasname", "门禁门" + i.ToString());
        //        door.excute(db);
        //    }
        //}
    }

    class AcsRes : Device
    {

        private static string head;
        public AcsRes()
            : base("acs_ResourseBase")
        {
            //sHead = "ResourceType,ClientId";
            //sValue = "'0800','" + QuickLoadAcs.acsCfgList[0] + "'";
            setParam("ResourceType", "0800");
            setParam("ClientId", QuickLoadAcs.acsCfgList[0]);
            deviceBase.setDeviceMainType("571");
            deviceBase.setDeviceType("590");
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
