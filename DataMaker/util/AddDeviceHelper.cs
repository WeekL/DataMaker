using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mysql;
using DataMaker.bean;

namespace DataMaker.util
{
    //添加设备数据辅助类，用于优化sql语句，提高性能
    class AddDeviceHelper
    {
        public static int ONCE_INSERT = 3;

        private AddDeviceHelper()
        {
            //禁止实例化
        }

        //多合一inset
        public static void multiExcute(MySQLDBHelper db,List<Device> devices)
        {
            List<List<List<Device>>> allChilds = new List<List<List<Device>>>();
            StringBuilder asset = new StringBuilder(devices[0].deviceBase.asset.initHead());
            StringBuilder assetState = new StringBuilder(devices[0].deviceBase.asset.state.initHead());
            StringBuilder deviceBase = new StringBuilder(devices[0].deviceBase.buildSQLHead());
            StringBuilder device = new StringBuilder(devices[0].initHead());
            foreach (Device d in devices)
            {
                asset.Append(d.deviceBase.asset.buildSQLValue() + ",");
                assetState.Append(d.deviceBase.asset.state.buildSQLValue() + ",");
                deviceBase.Append(d.deviceBase.buildSQLValue() + ",");
                device.Append(d.buildSQLValue() + ",");
                if (d.getChildList() != null && d.getChildList().Count > 0)
                {
                    allChilds.Add(d.getChildList());
                }
            }
            asset.Remove(asset.Length - 1, 1);
            assetState.Remove(assetState.Length - 1, 1);
            deviceBase.Remove(deviceBase.Length - 1, 1);
            device.Remove(device.Length - 1, 1);
            db.ExcuteNoneQuery(asset.ToString());
            db.ExcuteNoneQuery(assetState.ToString());
            db.ExcuteNoneQuery(deviceBase.ToString());
            db.ExcuteNoneQuery(device.ToString());
            Console.WriteLine("==========excute:-----Asset-----:" + asset.ToString());
            Console.WriteLine("==========excute:-----AssetState-----:" + assetState.ToString());
            Console.WriteLine("==========excute:-----DeviceBase-----:" + deviceBase.ToString());
            Console.WriteLine("==========excute:-----Device-----:" + device.ToString());
            if (allChilds != null && allChilds.Count > 0)
            {
                foreach (List<List<Device>> childList in allChilds)
                {
                    foreach (List<Device> childs in childList)
                    {
                        multiChildExcute(db, childs);
                    }
                }
            }
        }

        public static void multiChildExcute(MySQLDBHelper db, List<Device> devices)
        {
            StringBuilder asset = new StringBuilder(devices[0].deviceBase.asset.initHead());
            StringBuilder assetState = new StringBuilder(devices[0].deviceBase.asset.state.initHead());
            StringBuilder deviceBase = new StringBuilder(devices[0].deviceBase.buildSQLHead());
            StringBuilder device = new StringBuilder(devices[0].initHead());
            foreach (Device d in devices)
            {
                asset.Append(d.deviceBase.asset.buildSQLValue() + ",");
                assetState.Append(d.deviceBase.asset.state.buildSQLValue() + ",");
                deviceBase.Append(d.deviceBase.buildSQLValue() + ",");
                device.Append(d.buildSQLValue() + ",");
            }
            asset.Remove(asset.Length - 1, 1);
            assetState.Remove(assetState.Length - 1, 1);
            deviceBase.Remove(deviceBase.Length - 1, 1);
            device.Remove(device.Length - 1, 1);
            db.ExcuteNoneQuery(asset.ToString());
            db.ExcuteNoneQuery(assetState.ToString());
            db.ExcuteNoneQuery(deviceBase.ToString());
            db.ExcuteNoneQuery(device.ToString());
            Console.WriteLine("==========excute:-----Asset-----:" + asset.ToString());
            Console.WriteLine("==========excute:-----AssetState-----:" + assetState.ToString());
            Console.WriteLine("==========excute:-----DeviceBase-----:" + deviceBase.ToString());
            Console.WriteLine("==========excute:-----Device-----:" + device.ToString());
        }
    }
}
