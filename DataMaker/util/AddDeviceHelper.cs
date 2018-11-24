using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mysql;
using DataMaker.bean;
using System.Data;
using System.Data.SqlClient;

namespace DataMaker.util
{
    //添加设备数据辅助类，用于优化sql语句，提高性能
    class AddDeviceHelper
    {
        //执行一次INSERT添加的数据量
        public static int ONCE_INSERT = 100;

        private AddDeviceHelper()
        {
            //禁止实例化
        }

        //多合一insert
        public static void multiExcute(MySQLDBHelper db, List<Device> devices)
        {
            if (devices == null || devices.Count <= 0)
            {
                return;
            }
            //父设备列表<子设备种类<子设备列表<子设备>>>
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
            Console.WriteLine("==========excute:-----Asset--------:" + "asset.ToString()");
            db.ExcuteNoneQuery(asset.ToString());
            Console.WriteLine("==========excute:-----AssetState---:" + "assetState.ToString()");
            db.ExcuteNoneQuery(assetState.ToString());
            Console.WriteLine("==========excute:-----DeviceBase---:" + "deviceBase.ToString()");
            db.ExcuteNoneQuery(deviceBase.ToString());
            Console.WriteLine("==========excute:-----Device-------:" + "device.ToString()");
            db.ExcuteNoneQuery(device.ToString());
            if (allChilds != null && allChilds.Count > 0)
            {
                Console.WriteLine("====excute:开始添加子设备：设备种类：" + allChilds[0].Count);
                //子设备种类<子设备列表<子设备>>
                List<List<Device>> allChildDevices = new List<List<Device>>(allChilds[0].Count);
                for (int c = 0; c < allChilds[0].Count; c++)
                {
                    allChildDevices.Add(new List<Device>());
                }
                foreach (List<List<Device>> childDevices in allChilds)
                {
                    for (int i = 0; i < childDevices.Count; i++)
                    {
                        allChildDevices[i].AddRange(childDevices[i]);
                    }
                }
                int num = 1;
                foreach (List<Device> deviceList in allChildDevices)
                {
                    Console.WriteLine("-----添加第" + num++ + "种子设备----");
                    multiExcute(db, deviceList);
                }
                Console.WriteLine("==========excute:-----子设备添加完成-------\n\n");
            }
        }
    }
}
