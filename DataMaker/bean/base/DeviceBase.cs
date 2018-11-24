using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mysql;
using DataMaker.util;

namespace DataMaker.bean
{
    class DeviceBase : Table
    {
        private static string head;
        private string deviceCode;
        private Organization org;
        public Asset asset;


        public DeviceBase()
            : base("sys_deviceBase")
        {
            setParam("FatherCode", "");
            org = QuickLoadUtil.curOrg;
            setParam("enable", "1");
            setParam("OrgId", org.id);
            if (org.areaList.Count > 0)
            {
                setParam("Area", org.areaList[0]);
            }
            if (org.pointList.Count > 0)
            {
                setParam("SitePoint", org.pointList[0][0]);
            }
            asset = new Asset();
        }

        public void setDeviceCode(string code)
        {
            deviceCode = code;
            asset.setNum(code);
            setParam("DeviceCode",code);
        }

        public string getDeviceCode()
        {
            return deviceCode;
        }

        public void setFatherCode(string code)
        {
            sqlDictionary["FatherCode"] = code;
        }

        public void setName(string name)
        {
            asset.setName(name);
            setParam("name", name);
        }

        public void setDeviceMainType(string type)
        {
            setParam("deviceMainType", type);
        }

        public void setDeviceType(string type)
        {
            setParam("deviceType", type);
        }

        public override string initHead()
        {
            if (head == null)
            {
                head = buildSQLHead();
            }
            return head;
        }

        public new void excute(MySQLDBHelper db)
        {
            asset.excute(db);
            db.ExcuteNoneQuery(buildSqlStr());
            Console.WriteLine("====================创建设备基类表数据:id = " + getId());
        }
    }

    //不使用
    class DevState : Table
    {
        public string assetstate = "1";

        public DevState(string fid) : base("dev_state")
        {
            setParam("fid", fid);
            setParam("assetstate", "1");
        }

        public override string initHead()
        {
            return buildSQLHead();
        }
    }
}
