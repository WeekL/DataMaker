using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mysql;
using DataMaker.util;

namespace DataMaker.bean
{
    class Device : Table
    {
        public DeviceBase deviceBase;

        protected List<List<Device>> childList;

        public Device(string tableName)
            : base(tableName)
        {
            deviceBase = new DeviceBase();
            setParam("fid", deviceBase.getId());
            setParam("systemUpdateGUID", DateTime.Now.ToString());
            childList = new List<List<Device>>();
        }

        public void setIp(string ip)
        {
            setParam("DevIP", ip);
        }

        public void setPort(string port)
        {
            setParam("DevPort", port);
        }

        public void setUserName(string userName)
        {
            setParam("UserName", userName);
        }

        public void setPwd(string pwd)
        {
            setParam("Pwd", pwd);
        }

        public void setDeviceCode(string code)
        {
            deviceBase.setDeviceCode(code);
        }

        public void setDeviceName(string name)
        {
            deviceBase.setName(name);
        }

        public void setFatherCode(string code)
        {
            deviceBase.setFatherCode(code);
        }

        public override string initHead()
        {
            return buildSQLHead();
        }

        public virtual new void excute(MySQLDBHelper db)
        {
            deviceBase.excute(db);
            db.ExcuteNoneQuery(buildSqlStr());
            if (childList != null && childList.Count > 0)
            {
                foreach (List<Device> childs in childList)
                {
                    AddDeviceHelper.multiExcute(db, childs);
                }
            }
            Console.WriteLine("====================添加一条数据：" + this.getId());
        }

        public List<List<Device>> getChildList()
        {
            return childList;
        }
    }
}
