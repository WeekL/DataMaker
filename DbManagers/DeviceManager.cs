using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Media;
using CommonHelperLib;
using DbManagers.Helpers;
using DbManagers.Models;
using IPPhoneModel;
using IPPhoneModel.EnumTypes;
using IPPhoneModel.ObjectTypes;
using MySql.Data.MySqlClient;

namespace DbManagers
{
    /// <summary>
    /// 设备管理器类
    /// </summary>
    public static class DeviceManager
    {
        /// <summary>
        /// 根据提供的组id获取设备信息，id为-1表示获取所有
        /// </summary>
        /// <param name="devices">out 设备集合</param>
        /// <param name="groupid">组id</param>
        public static void GetDevices(out List<Device> devices, int groupid = -1, string geoid=null,string deviceid=null)
        {
            LogHelper.MainLog("GetDevices1  time:"+DateTime.Now.TimeOfDay);
            devices = new List<Device>();
            var points = new List<GeoPoint>();
            DataSet set;

            #region sql语句

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT * FROM (SELECT A.DeviceID,A.DeviceName,A.DeviceType,");
            sqlBuilder.Append("A.DeviceRegCode,A.FactoryNum,A.PhoneLevel,A.DeviceTypeInfo,");
            sqlBuilder.Append("A.DeviceIP,A.DevicePort,A.SoftVersion,A.HardVersion,A.Manufacturer,A.GeoId,A.GroupID,");
            sqlBuilder.Append("B.ExtensionID AS ExtenID,B.ExtensionNo AS ExtenNO,B.StateID AS StateID,B.PhoneState,B.PanelNum ");
            sqlBuilder.Append("FROM ipvt_deviceinfotable AS A left join ipvt_extensionmessagetable AS B ");
            sqlBuilder.Append("on A.ExtensionID=B.ExtensionID where 1=1");

            if (groupid != -1)
            {
                sqlBuilder.Append(" AND A.GroupID=" + groupid);
            } 
            if (!string.IsNullOrEmpty(geoid))
            {
                sqlBuilder.Append(" AND (A.GroupID=" + geoid + " or A.DeviceIP='" + deviceid + "' )");
            }

            sqlBuilder.Append(") AS C ");
            sqlBuilder.Append(@" LEFT JOIN ipvt_geoinfotable AS D ON C.GeoId=D.GeoID 
                          LEFT JOIN ipvt_panelinfotable as E on C.ExtenID=E.ExtensionID ORDER BY ExtenID,E.PanelNum");

            #endregion

            #region 获取设备集

            try
            {
                set = CustomMySqlHelper.ExecuteDataSet(sqlBuilder.ToString());

                if (set.Tables.Count > 0)
                {
                    DataRowCollection rows = set.Tables[0].Rows;

                    foreach (DataRow row in rows)
                    {
                        int id = EvaluationHelper.ObjectToInt(row["DeviceID"]);
                        if (!devices.Exists(m => m.Id == id))
                        {
                            var device = new Device();
                            device.Name = EvaluationHelper.ObjectToString(row["DeviceName"]);
                            device.Id = EvaluationHelper.ObjectToInt(row["DeviceID"]);
                            device.Type = EvaluationHelper.ObjectToInt(row["DeviceType"]);
                            device.Ip = EvaluationHelper.ObjectToString(row["DeviceIP"]);
                            device.PhoneLevel = EvaluationHelper.ObjectToInt(row["PhoneLevel"]);
                            device.RegistCode = EvaluationHelper.ObjectToString(row["DeviceRegCode"]);
                            device.SoftVersion = EvaluationHelper.ObjectToString(row["SoftVersion"]);
                            device.HardVersion = EvaluationHelper.ObjectToString(row["HardVersion"]);

                            device.DeviceTypeInfo = EvaluationHelper.ObjectToString(row["DeviceTypeInfo"]);
                            if (string.IsNullOrEmpty(device.DeviceTypeInfo) ||
                                device.DeviceTypeInfo == "IP-Center" || device.DeviceTypeInfo == "IP-Phone")
                            {
                                device.Generation = 1;
                            }
                            else
                            {
                                device.Generation = 2;
                            }
                            device.ColorString = GetColor(device.DeviceTypeInfo);

                            if (row["GroupID"] != DBNull.Value)
                            {
                                device.GroupId = EvaluationHelper.ObjectToInt(row["GroupID"]);
                            }

                            device.Port = EvaluationHelper.ObjectToInt(row["DevicePort"]);
                            device.Extension = new Extension
                            {
                                Number = EvaluationHelper.ObjectToString(row["ExtenNO"]),
                                Id = EvaluationHelper.ObjectToInt(row["ExtenID"]),
                                State = StateManager.GetState(EvaluationHelper.ObjectToInt(row["StateID"])),
                                PhoneState = EvaluationHelper.ObjectToInt(row["PhoneState"]) == 0
                                    ? State.STAT_INVALID
                                    : State.STAT_DEVICE_TALKING,
                                PanelNum = EvaluationHelper.ObjectToString(row["PanelNum"])
                            };

                            if (string.IsNullOrEmpty(device.Name)) //如果名称为空给设备添加默认名称=分机号
                            {
                                device.Name = EvaluationHelper.ObjectToString(row["ExtenNO"]);
                            }

                            GeoPoint pnt = new GeoPoint();
                            if (row["GeoId"] != DBNull.Value)
                            {
                                pnt.Id = EvaluationHelper.ObjectToInt(row["GeoId"]);
                                bool flag = false;
                                foreach (var p in points)
                                {
                                    if (pnt.Equals(p))
                                    {
                                        pnt = p;
                                        flag = true;
                                        break;
                                    }
                                }

                                if (!flag)
                                {
                                    pnt.Id = EvaluationHelper.ObjectToInt(row["GeoID"]);
                                    pnt.Name = EvaluationHelper.ObjectToString(row["Name"]);
                                    pnt.Address = EvaluationHelper.ObjectToString(row["FormattedAddress"]);
                                    pnt.Phone = EvaluationHelper.ObjectToString(row["phone"]);
                                    pnt.Latitude = EvaluationHelper.ObjectToDouble(row["Latitude"]);
                                    pnt.Longitude = EvaluationHelper.ObjectToDouble(row["Longitude"]);
                                    pnt.Note = EvaluationHelper.ObjectToString(row["Note"]);
                                }
                            }
                            device.GeoPoint = pnt;
                            device.Manufacturer = EvaluationHelper.ObjectToString(row["Manufacturer"]);

                            if (!string.IsNullOrEmpty(EvaluationHelper.ObjectToString(row["PanelID"])))
                            {
                                var panel = new PanelDevice(); //面板对象
                                panel.Id = EvaluationHelper.ObjectToInt(row["PanelID"]); //面板号
                                panel.Number = EvaluationHelper.ObjectToInt(row["PanelNum1"]); //面板号
                                int stateNum = EvaluationHelper.ObjectToInt(row["PanelState"]); //面板状态号
                                switch (stateNum)
                                {
                                    case 0:
                                        panel.LineState = State.STAT_DEVICE_ONLINE; //在线
                                        break;

                                    case 1:
                                        panel.LineState = State.STAT_DEVICE_OFFLINE; //离线
                                        break;
                                }

                                int alarmState = EvaluationHelper.ObjectToInt(row["TamperAlarm"]);
                                if (alarmState == 2)
                                {
                                    panel.State = State.STAT_ANTI_DISMANTLE_ALARM; //防拆警报
                                }
                                else
                                {
                                    panel.State = panel.LineState;
                                }
                                panel.Name = EvaluationHelper.ObjectToString(row["PanelName"]); //面板名
                                if (string.IsNullOrEmpty(panel.Name))
                                {
                                    panel.Name = string.Format("面板{0}", panel.Number);
                                }
                                if (device.Extension.State == State.STAT_DEVICE_OFFLINE)
                                {
                                    panel.State = State.STAT_DEVICE_OFFLINE;
                                }
                                else if (panel.State == State.STAT_ANTI_DISMANTLE_ALARM)
                                {
                                    device.Extension.AlarmState = State.STAT_ANTI_DISMANTLE_ALARM;
                                }
                                device.Panels.Add(panel);
                            }

                            devices.Add(device);
                        }
                        else 
                        {
                            if (!string.IsNullOrEmpty(EvaluationHelper.ObjectToString(row["PanelID"])))
                            {
                                Device device = devices.Find(m => m.Id == id);
                                var panel = new PanelDevice(); //面板对象
                                panel.Id = EvaluationHelper.ObjectToInt(row["PanelID"]); //面板号
                                panel.Number = EvaluationHelper.ObjectToInt(row["PanelNum1"]); //面板号
                                int stateNum = EvaluationHelper.ObjectToInt(row["PanelState"]); //面板状态号
                                switch (stateNum)
                                {
                                    case 0:
                                        panel.LineState = State.STAT_DEVICE_ONLINE; //在线
                                        break;

                                    case 1:
                                        panel.LineState = State.STAT_DEVICE_OFFLINE; //离线
                                        break;
                                }

                                int alarmState = EvaluationHelper.ObjectToInt(row["TamperAlarm"]);
                                if (alarmState == 2)
                                {
                                    panel.State = State.STAT_ANTI_DISMANTLE_ALARM; //防拆警报
                                }
                                else
                                {
                                    panel.State = panel.LineState;
                                }

                                panel.Name = EvaluationHelper.ObjectToString(row["PanelName"]); //面板名
                                if (string.IsNullOrEmpty(panel.Name))
                                {
                                    panel.Name = string.Format("面板{0}", panel.Number);
                                }
                                if (device.Extension.State == State.STAT_DEVICE_OFFLINE)
                                {
                                    panel.State = State.STAT_DEVICE_OFFLINE;
                                }
                                else if (panel.State == State.STAT_ANTI_DISMANTLE_ALARM)
                                {
                                    device.Extension.AlarmState = State.STAT_ANTI_DISMANTLE_ALARM;
                                }
                                device.Panels.Add(panel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error int GetDevices!" + ex);
            }


            #endregion
            LogHelper.MainLog("GetDevices2  time:" + DateTime.Now.TimeOfDay);

            //#region 获取设备的面板信息

            //foreach (Device dev in devices)
            //{
            //    List<PanelDevice> list;
            //    GetPanels(dev.Extension.Id, out list);
            //    //LogHelper.MainLog(string.Format("GetDevices(int groupid = -1)，ExtensionNumber:{1} --- PanelNum:{0}", list.Count, dev.Extension.Number));
            //    foreach (PanelDevice panel in list)
            //    {
            //        if (dev.Extension.State == State.STAT_DEVICE_OFFLINE)
            //        {
            //            panel.State = State.STAT_DEVICE_OFFLINE;
            //        }
            //        else if (panel.State == State.STAT_ANTI_DISMANTLE_ALARM)
            //        {
            //            dev.Extension.AlarmState = State.STAT_ANTI_DISMANTLE_ALARM;
            //        }
            //        dev.Panels.Add(panel);
            //    }
            //}

            //#endregion
            //LogHelper.MainLog("GetDevices3  time:" + DateTime.Now.TimeOfDay);

            //#region 获取转移信息

            //foreach (Device dev in devices)
            //{
            //    if (dev.Type == 0) //刷选寻呼话筒
            //    {
            //        List<Transfer> trans;
            //        TransferTableManager.GetTransferTableByExtension(out trans, dev.Extension); //转移信息集合
            //        foreach (Transfer tran in trans)
            //        {
            //            dev.TransferTable.Add(tran);
            //        }
            //    }
            //}
            //#endregion
            //LogHelper.MainLog("GetDevices4  time:" + DateTime.Now.TimeOfDay);

            points.Clear();
        }

        /// <summary>
        /// 获取指定分机id的面板信息
        /// </summary>
        /// <param name="extensionId">分机id</param>
        /// <param name="list">out 面板信息集合</param>
        public static void GetPanels(int extensionId, out List<PanelDevice> list)
        {
            list = new List<PanelDevice>();

            //查询面板信息的sql语句
            string sqlStr = "select * from ipvt_panelinfotable where ExtensionID=?id order by PanelNum";
            var param = new MySqlParameter[1]; //参数对象
            param[0] = new MySqlParameter("?id", extensionId);

            //获取reader对象
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sqlStr, param);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var panel = new PanelDevice(); //面板对象
                        panel.Id = EvaluationHelper.ObjectToInt(reader["PanelID"]); //面板号
                        panel.Number = EvaluationHelper.ObjectToInt(reader["PanelNum"]); //面板号
                        int stateNum = EvaluationHelper.ObjectToInt(reader["PanelState"]); //面板状态号
                        switch (stateNum)
                        {
                            case 0:
                                panel.LineState = State.STAT_DEVICE_ONLINE; //在线
                                break;

                            case 1:
                                panel.LineState = State.STAT_DEVICE_OFFLINE; //离线
                                break;
                        }

                        int alarmState = EvaluationHelper.ObjectToInt(reader["TamperAlarm"]);
                        if (alarmState == 2)
                        {
                            panel.State = State.STAT_ANTI_DISMANTLE_ALARM; //防拆警报
                        }
                        else
                        {
                            panel.State = panel.LineState;
                        }

                        panel.Name = EvaluationHelper.ObjectToString(reader["PanelName"]); //面板名
                        if (string.IsNullOrEmpty(panel.Name))
                        {
                            panel.Name = string.Format("面板{0}", panel.Number);
                        }

                        list.Add(panel); //将面板信息对象加入设备面板集合
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error int GetPanels(int extensionId)!" + ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close(); //读取完关闭reader对象
                }
            }
        }

        /// <summary>
        /// 根据提供的分机号获取设备信息
        /// </summary>
        /// <param name="extenNo"></param>
        /// <returns></returns>
        public static Device GetDevice(string extenNo, string geoid=null)
        {
            Device device = null;
            
            #region 获取设备信息
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT * FROM (SELECT A.DeviceID,A.DeviceName,A.DeviceType,");
            sqlBuilder.Append("A.DeviceRegCode,A.FactoryNum,A.PhoneLevel,A.DeviceTypeInfo,");
            sqlBuilder.Append("A.DeviceIP,A.DevicePort,A.SoftVersion,A.HardVersion,A.Manufacturer,A.GeoId,A.GroupID,");
            sqlBuilder.Append("B.ExtensionID AS ExtenID,B.ExtensionNo AS ExtenNO,B.StateID AS StateID,B.PhoneState ");
            sqlBuilder.Append("FROM ipvt_deviceinfotable AS A,ipvt_extensionmessagetable AS B ");
            sqlBuilder.Append("WHERE B.ExtensionNo=?extenNo AND A.ExtensionID=B.ExtensionID ");
            if (!string.IsNullOrEmpty(geoid))
            {
                sqlBuilder.Append(" AND A.GroupID= " + geoid);
            }
            sqlBuilder.Append(") AS C ");
            //sqlBuilder.Append(" LEFT JOIN ipvt_geoinfotable AS D ON C.GeoId=D.GeoID ");
            sqlBuilder.Append(@" LEFT JOIN ipvt_geoinfotable AS D ON C.GeoId=D.GeoID 
                          LEFT JOIN ipvt_panelinfotable as E on C.ExtenID=E.ExtensionID ORDER BY ExtenID,E.PanelNum");


            var param = new MySqlParameter[1];
            param[0] = new MySqlParameter("?extenNo", Convert.ToInt32(extenNo));

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sqlBuilder.ToString(), param);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (device == null)
                        {
                            device = new Device();
                            device.Name = EvaluationHelper.ObjectToString(reader["DeviceName"]);
                            device.Id = EvaluationHelper.ObjectToInt(reader["DeviceID"]);
                            device.Type = EvaluationHelper.ObjectToInt(reader["DeviceType"]);
                            device.Ip = EvaluationHelper.ObjectToString(reader["DeviceIP"]);
                            device.PhoneLevel = EvaluationHelper.ObjectToInt(reader["PhoneLevel"]);
                            device.RegistCode = EvaluationHelper.ObjectToString(reader["DeviceRegCode"]);
                            device.SoftVersion = EvaluationHelper.ObjectToString(reader["SoftVersion"]);
                            device.HardVersion = EvaluationHelper.ObjectToString(reader["HardVersion"]);

                            device.DeviceTypeInfo = EvaluationHelper.ObjectToString(reader["DeviceTypeInfo"]);
                            if (string.IsNullOrEmpty(device.DeviceTypeInfo) ||
                                device.DeviceTypeInfo == "IP-Center" || device.DeviceTypeInfo == "IP-Phone")
                            {
                                device.Generation = 1;
                            }
                            else
                            {
                                device.Generation = 2;
                            }
                            device.ColorString = GetColor(device.DeviceTypeInfo);

                            if (reader["GroupID"] != DBNull.Value)
                            {
                                device.GroupId = EvaluationHelper.ObjectToInt(reader["GroupID"]);
                            }

                            device.Port = EvaluationHelper.ObjectToInt(reader["DevicePort"]);
                            device.Extension = new Extension
                            {
                                Number = EvaluationHelper.ObjectToString(reader["ExtenNO"]),
                                Id = EvaluationHelper.ObjectToInt(reader["ExtenID"]),
                                State = StateManager.GetState(EvaluationHelper.ObjectToInt(reader["StateID"])),
                                PhoneState = EvaluationHelper.ObjectToInt(reader["PhoneState"]) == 0
                                    ? State.STAT_INVALID
                                    : State.STAT_DEVICE_TALKING
                            };

                            if (string.IsNullOrEmpty(device.Name)) //如果名称为空给设备添加默认名称=分机号
                            {
                                device.Name = EvaluationHelper.ObjectToString(reader["ExtenNO"]);
                            }

                            if (reader["GeoId"] != DBNull.Value)
                            {
                                var pnt = new GeoPoint();
                                pnt.Id = EvaluationHelper.ObjectToInt(reader["GeoID"]);
                                pnt.Name = EvaluationHelper.ObjectToString(reader["Name"]);
                                pnt.Address = EvaluationHelper.ObjectToString(reader["FormattedAddress"]);
                                pnt.Phone = EvaluationHelper.ObjectToString(reader["phone"]);
                                pnt.Latitude = EvaluationHelper.ObjectToDouble(reader["Latitude"]);
                                pnt.Longitude = EvaluationHelper.ObjectToDouble(reader["Longitude"]);
                                pnt.Note = EvaluationHelper.ObjectToString(reader["Note"]);
                                device.GeoPoint = pnt;
                            }
                            else
                            {
                                device.GeoPoint = new GeoPoint();
                            }
                            device.Manufacturer = EvaluationHelper.ObjectToString(reader["Manufacturer"]);

                            if (!string.IsNullOrEmpty(EvaluationHelper.ObjectToString(reader["PanelID"])))
                            {
                                var panel = new PanelDevice(); //面板对象
                                panel.Id = EvaluationHelper.ObjectToInt(reader["PanelID"]); //面板号
                                panel.Number = EvaluationHelper.ObjectToInt(reader["PanelNum"]); //面板号
                                int stateNum = EvaluationHelper.ObjectToInt(reader["PanelState"]); //面板状态号
                                switch (stateNum)
                                {
                                    case 0:
                                        panel.LineState = State.STAT_DEVICE_ONLINE; //在线
                                        break;

                                    case 1:
                                        panel.LineState = State.STAT_DEVICE_OFFLINE; //离线
                                        break;
                                }

                                int alarmState = EvaluationHelper.ObjectToInt(reader["TamperAlarm"]);
                                if (alarmState == 2)
                                {
                                    panel.State = State.STAT_ANTI_DISMANTLE_ALARM; //防拆警报
                                }
                                else
                                {
                                    panel.State = panel.LineState;
                                }
                                panel.Name = EvaluationHelper.ObjectToString(reader["PanelName"]); //面板名
                                if (string.IsNullOrEmpty(panel.Name))
                                {
                                    panel.Name = string.Format("面板{0}", panel.Number);
                                }
                                if (device.Extension.State == State.STAT_DEVICE_OFFLINE)
                                {
                                    panel.State = State.STAT_DEVICE_OFFLINE;
                                }
                                else if (panel.State == State.STAT_ANTI_DISMANTLE_ALARM)
                                {
                                    device.Extension.AlarmState = State.STAT_ANTI_DISMANTLE_ALARM;
                                }
                                device.Panels.Add(panel);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(EvaluationHelper.ObjectToString(reader["PanelID"])))
                            {
                                var panel = new PanelDevice(); //面板对象
                                panel.Id = EvaluationHelper.ObjectToInt(reader["PanelID"]); //面板号
                                panel.Number = EvaluationHelper.ObjectToInt(reader["PanelNum"]); //面板号
                                int stateNum = EvaluationHelper.ObjectToInt(reader["PanelState"]); //面板状态号
                                switch (stateNum)
                                {
                                    case 0:
                                        panel.LineState = State.STAT_DEVICE_ONLINE; //在线
                                        break;

                                    case 1:
                                        panel.LineState = State.STAT_DEVICE_OFFLINE; //离线
                                        break;
                                }

                                int alarmState = EvaluationHelper.ObjectToInt(reader["TamperAlarm"]);
                                if (alarmState == 2)
                                {
                                    panel.State = State.STAT_ANTI_DISMANTLE_ALARM; //防拆警报
                                }
                                else
                                {
                                    panel.State = panel.LineState;
                                }
                                panel.Name = EvaluationHelper.ObjectToString(reader["PanelName"]); //面板名
                                if (string.IsNullOrEmpty(panel.Name))
                                {
                                    panel.Name = string.Format("面板{0}", panel.Number);
                                }
                                if (device.Extension.State == State.STAT_DEVICE_OFFLINE)
                                {
                                    panel.State = State.STAT_DEVICE_OFFLINE;
                                }
                                else if (panel.State == State.STAT_ANTI_DISMANTLE_ALARM)
                                {
                                    device.Extension.AlarmState = State.STAT_ANTI_DISMANTLE_ALARM;
                                }
                                device.Panels.Add(panel);
                            }
                        }
                        //break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error int GetDevice(int extenNo)!" + ex.StackTrace);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            #endregion

            //#region 获取设备的面板信息

            //if (device != null)
            //{
            //    List<PanelDevice> list;
            //    GetPanels(device.Extension.Id, out list);
            //    //LogHelper.MainLog(string.Format("in GetDevice(string extenNo)，ExtensionNumber:{1} --- PanelNum:{0}", list.Count, device.Extension.Number));
            //    foreach (PanelDevice panel in list)
            //    {
            //        if (panel.State == State.STAT_ANTI_DISMANTLE_ALARM)
            //        {
            //            device.Extension.AlarmState = State.STAT_ANTI_DISMANTLE_ALARM;
            //        }
            //        device.Panels.Add(panel);
            //    }
            //}

            //#endregion

            return device;
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="device"></param>
        public static void Update(Device device)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append(
                "UPDATE ipvt_deviceinfotable SET DeviceName=?dname,DeviceRegCode=?dregcode,FactoryNum=?dfnum,");
            sqlBuilder.Append(
                "DeviceIP=?dip,DevicePort=?dport,SoftVersion=?dsver,HardVersion=?dhver,Manufacturer=?dmft,GroupID=?dgrpid,GeoId=?geoid ");
            sqlBuilder.Append("WHERE DeviceId=?id");

            var param = new MySqlParameter[11];
            param[0] = new MySqlParameter("?dname", device.Name);
            param[1] = new MySqlParameter("?dregcode", string.Empty);
            param[2] = new MySqlParameter("?dfnum", string.Empty);
            param[3] = new MySqlParameter("?dip", device.Ip);
            param[4] = new MySqlParameter("?dport", device.Port);
            param[5] = new MySqlParameter("?dsver", device.SoftVersion);
            param[6] = new MySqlParameter("?dhver", device.HardVersion);
            param[7] = new MySqlParameter("?dmft", device.Manufacturer);
            if (device.GroupId == 0)
            {
                param[8] = new MySqlParameter("?dgrpid", null);
            }
            else
            {
                param[8] = new MySqlParameter("?dgrpid", device.GroupId);
            }

            if (device.GeoPoint.Id != 0)
            {
                param[9] = new MySqlParameter("?geoid", device.GeoPoint.Id);
            }
            else
            {
                param[9] = new MySqlParameter("?geoid", null);
            }

            param[10] = new MySqlParameter("?id", device.Id);

            CustomMySqlHelper.ExecuteNonQuery(sqlBuilder.ToString(), param);
        }

        /// <summary>
        /// 更新设备所属组
        /// </summary>
        /// <param name="did"></param>
        /// <param name="gid"></param>
        public static void UpdateDeviceGroup(int did,int gid)
        {
            if (did > 0)
            {
                string sql = "UPDATE ipvt_deviceinfotable SET GroupID=?gid where DeviceId=?id";
                var param = new MySqlParameter[2];
                param[0] = new MySqlParameter("?gid", gid);
                param[1] = new MySqlParameter("?id", did);

                CustomMySqlHelper.ExecuteNonQuery(sql, param);
            }
        }

        /// <summary>
        /// 删除网点组信息
        /// </summary>
        public static void ClearGeo(int gid)
        {
            string sql = string.Format(@"DELETE FROM ipvt_geoinfotable where GeoID={0};
                           UPDATE ipvt_deviceinfotable SET GeoId=NULL where GeoId={0}", gid);
            //var param = new MySqlParameter[2];
            //param[0] = new MySqlParameter("?gid", gid);
            //param[1] = new MySqlParameter("?gid2", gid);

            int i= CustomMySqlHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 删除设备网点信息
        /// </summary>
        public static void ClearDeviceGeo(int did)
        {
            string sql = @" UPDATE ipvt_deviceinfotable SET GeoId=NULL where DeviceId=?id";
            var param = new MySqlParameter[1];
            param[0] = new MySqlParameter("?id", did);

            CustomMySqlHelper.ExecuteNonQuery(sql, param);
        }

        /// <summary>
        /// 根据ip获取对应的分机号
        /// </summary>
        /// <param name="ip"></param>
        public static string GetExtensionNoByIp(string ip)
        {
            string result = "";
            if (string.IsNullOrEmpty(ip)) return result;

            string sql = "SELECT ExtensionNo FROM ipvt_extensionmessagetable WHERE DeviceIP=?ip";
            var ps = new MySqlParameter[1];
            ps[0]=new MySqlParameter("?ip",ip);

            try
            {
                result = CustomMySqlHelper.ExecuteScalar(sql, ps).ToString();
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.ToString());
            }

            return result;
        }

        /// <summary>
        /// 获取主叫uuid
        /// </summary>
        /// <param name="caller">主叫分机号</param>
        /// <param name="callee">被叫分机号</param>
        /// <returns>uuid/string.Empty</returns>
        public static string GetUuid(string caller, string callee)
        {
            string sql = "SELECT call_uuid FROM ipvt_talkinginfotable where caller=?caller AND callee=?callee";

            var ps = new MySqlParameter[2];
            ps[0] = new MySqlParameter("?caller", caller);
            ps[1] = new MySqlParameter("?callee", callee);

            var result = CustomMySqlHelper.ExecuteScalar(sql, ps);
            return result != null ? result.ToString() : string.Empty;
        }

        /// <summary>
        /// 通过单个分机号获取uuid
        /// </summary>
        /// <param name="exten">分机号</param>
        /// <returns></returns>
        public static string GetUuidByNumber(string exten)
        {
            string sql = "SELECT uuid from ipvt_channelsinfotable where ExtensionNO=?extenNo ORDER BY ChannelID DESC";

            var ps = new MySqlParameter[1];
            ps[0] = new MySqlParameter("?extenNo", exten);
            var result = CustomMySqlHelper.ExecuteScalar(sql, ps);
            return result != null ? result.ToString():string.Empty;
        }

        /// <summary>
        /// 通过单个分机号获取正在等待的分机信息
        /// </summary>
        /// <param name="exten">分机号</param>
        /// <returns></returns>
        public static Channelsinfo GetChannelsinfo(string exten)
        {
            string sql = "SELECT uuid,ExtensionNO,application,application_data from ipvt_channelsinfotable where ExtensionNO=?extenNo";

            var ps = new MySqlParameter[1];
            ps[0] = new MySqlParameter("?extenNo", exten);
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql, ps);
                while (reader.Read())
                {
                    Channelsinfo model = new Channelsinfo();
                    model.Uuid = EvaluationHelper.ObjectToString(reader["uuid"]);
                    model.ExtNo = EvaluationHelper.ObjectToString(reader["ExtensionNO"]);
                    model.ApplicationInfo = EvaluationHelper.ObjectToString(reader["application"]);
                    model.ApplicationDate = EvaluationHelper.ObjectToString(reader["application_data"]);
                    return model;
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return null;
        }
        /// <summary>
        /// 通过单个分机号获取正在等待的分机信息
        /// </summary>
        /// <returns></returns>
        public static void GetChannelsinfo(out List<Channelsinfo> infoes)
        {
            infoes = new List<Channelsinfo>();
            string sql = "SELECT uuid,ExtensionNO,application,application_data from ipvt_channelsinfotable";

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                while (reader.Read())
                {
                    Channelsinfo model = new Channelsinfo();
                    model.Uuid = EvaluationHelper.ObjectToString(reader["uuid"]);
                    model.ExtNo = EvaluationHelper.ObjectToString(reader["ExtensionNO"]);
                    model.ApplicationInfo = EvaluationHelper.ObjectToString(reader["application"]);
                    model.ApplicationDate = EvaluationHelper.ObjectToString(reader["application_data"]);
                    infoes.Add(model);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 通过单个分机号获取Direction
        /// </summary>
        /// <param name="exten">分机号</param>
        /// <returns></returns>
        public static string GetApplicationByNumber(string exten)
        {
            string sql = "SELECT application from ipvt_channelsinfotable where ExtensionNO=?extenNo";

            var ps = new MySqlParameter[1];
            ps[0] = new MySqlParameter("?extenNo", exten);
            var result = CustomMySqlHelper.ExecuteScalar(sql, ps);
            return result != null ? result.ToString() : string.Empty;
        }

        /// <summary>
        /// 获取正在通话的集合
        /// </summary>
        /// <returns></returns>
        public static void GeTalkInfos(ref List<TalkInfo> list)
        {
            //list = new List<TalkInfo>();
            string sql = "SELECT * FROM ipvt_talkinginfotable";

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var info = new TalkInfo();
                        info.From = reader["caller"].ToString();
                        info.To = reader["callee"].ToString();

                        list.Add(info);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in GeTalkInfos()!ex:" + ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 获取寻呼话筒简要信息
        /// </summary>
        /// <returns></returns>
        public static void GetPhones(out List<Device> devices)
        {
            devices = new List<Device>();
            string sql;
            sql = "select A.DeviceID,A.DeviceName,A.DeviceType,A.DeviceIP,A.ExtensionID,B.ExtensionNo" +
                  " from ipvt_deviceinfotable AS A,ipvt_extensionmessagetable AS B" +
                  " WHERE A.DeviceType=0 and A.ExtensionID=B.ExtensionID";

            try
            {
                DataSet set = CustomMySqlHelper.ExecuteDataSet(sql);
                if (set.Tables.Count > 0)
                {
                    DataRowCollection rows = set.Tables[0].Rows;

                    foreach (DataRow row in rows)
                    {
                        var device = new Device();
                        device.Name = row["DeviceName"].ToString();
                        device.Id = EvaluationHelper.ObjectToInt(row["DeviceID"]);
                        device.Type = EvaluationHelper.ObjectToInt(row["DeviceType"]);
                        device.Ip = EvaluationHelper.ObjectToString(row["DeviceIP"]);

                        device.Extension = new Extension
                        {
                            Number = EvaluationHelper.ObjectToString(row["ExtensionNo"]),
                            Id = EvaluationHelper.ObjectToInt(row["ExtensionID"]),
                        };

                        if (string.IsNullOrEmpty(device.Name)) //如果名称为空给设备添加默认名称=分机号
                        {
                            device.Name = EvaluationHelper.ObjectToString(row["ExtensionNo"]);
                        }
                        devices.Add(device);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error int GetPanels(int extensionId)!" + ex);
            }
        }

        /// <summary>
        /// 获取未使用的分机号集合
        /// </summary>
        /// <returns></returns>
        public static void GetExtensionNoList(out List<string> list)
        {
            list = new List<string>();
            string sql = "SELECT ExtensionNO from ipvt_extensionmessagetable WHERE CurrentState=0";
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                while (reader.Read())
                {
                    if (reader["ExtensionNO"]!=null)
                        list.Add(reader["ExtensionNO"].ToString());
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close(); //读取完关闭reader对象
                }
            }
        }

        #region 更新设备信息里的集合信息
        /// <summary>
        /// 更新设备信息里的集合信息
        /// </summary>
        /// <param name="device">设备</param>
        /// <param name="i">0：面板  1：转移  2：厂商技术人员  3：工程技术人员  4：监控中心人员 </param>
        public static void UpdateList(Device device,int i)
        {
            if (device == null) return;

            switch (i)
            {
                case 0:
                    UpdatePanels(device.Panels);
                    break;

                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    break;
            }
        }
        /// <summary>
        /// 更新面板信息
        /// </summary>
        /// <param name="panels"></param>
        private static bool UpdatePanels(IEnumerable<PanelDevice> panels)
        {
            var list = new List<SqlTextModel>();
            foreach (var panel in panels)
            {
                string sqlStr = "update ipvt_panelinfotable set PanelName=?name where PanelID=?id";
                var parameters = new MySqlParameter[2];
                parameters[0] = new MySqlParameter("?name", panel.Name);
                parameters[1] = new MySqlParameter("?id", panel.Id);
                list.Add(new SqlTextModel {SqlString = sqlStr, MySqlParams = parameters});
            }

            return CustomMySqlHelper.ExecuteSqlList(list);
        }

        #endregion

        /// <summary>
        /// 更新寻呼话筒行政级别
        /// </summary>
        /// <param name="did"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int UpdatePhoneLevel(int did,int level)
        {
            string sql = "update ipvt_deviceinfotable set PhoneLevel=?level where DeviceID=?id";

            var ps = new MySqlParameter[2];
            ps[0] = new MySqlParameter("?level", level);
            ps[1] = new MySqlParameter("?id", did);

            return CustomMySqlHelper.ExecuteNonQuery(sql,ps);
        }

        /// <summary>
        /// 清除异常残留数据
        /// </summary>
        /// <returns></returns>
        public static bool ClearOldData()
        {
            var sqlList = new List<SqlTextModel>();
            sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_talkinginfotable" });
            sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_channelsinfotable" });
            sqlList.Add(new SqlTextModel { SqlString = "UPDATE ipvt_extensionmessagetable SET PhoneState = 0" });
            return CustomMySqlHelper.ExecuteSqlList(sqlList);
        }

        /// <summary>
        /// 获取报警信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetAlarmType(int id)
        {
            if (id > 0)
            {
                string sql = "select AlarmType from ipvt_alarmtypetable where AlarmTypeID=?id";
                var result = CustomMySqlHelper.ExecuteScalar(sql, new MySqlParameter("?id", id));
                return EvaluationHelper.ObjectToString(result);
            }
            return null;
        }

        //根据类型获取颜色
        private static string GetColor(string type)
        {

            if (type == "IP-Center") //一代中心
            {
                return "#FF1C9B08";
            }
            if (type == "IP-Phone") //一代设备
            {
                return "#FFDD850E";
            }
            if (type == "CenterDevice") //三代中心
            {
                return "#FF0D0DC6";
            }
            if (string.IsNullOrEmpty(type)) //一代空类型设备
            {
                return "#FFE02914";
            }
            return "#FF171719";
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool DelDeviceInfo(Device device)
        {
            if (device != null)
            {
                List<SqlTextModel> list = new List<SqlTextModel>();

                string s1 = "DELETE FROM ipvt_panelinfotable WHERE ExtensionID=?id;";
                string s2 = "UPDATE ipvt_extensionmessagetable SET CurrentState = 0,DeviceIP = NULL,StateID = NULL,Date = NULL,Time = NULL,PhoneState=0 WHERE ExtensionID=?id;";
                string s3 = "DELETE FROM ipvt_deviceinfotable WHERE DeviceID=?id;";

                list.Add(new SqlTextModel() { SqlString = s1, MySqlParams = new[] { new MySqlParameter("?id", device.Extension.Id) } });
                list.Add(new SqlTextModel() { SqlString = s2, MySqlParams = new[] { new MySqlParameter("?id", device.Extension.Id) } });
                list.Add(new SqlTextModel() { SqlString = s3, MySqlParams = new[] { new MySqlParameter("?id", device.Id) } });

                return CustomMySqlHelper.ExecuteSqlList(list);
            }
            return false;
        }


        /// <summary>
        /// 获取指定分机id的面板名称
        /// </summary>
        /// <param name="num">面板号</param>
        /// <param name="id">分机id</param>
        public static string GetPanelName(string id, string num)
        {
            string sql = @"select a.PanelName from ipvt_panelinfotable as a inner join ipvt_extensionmessagetable as b
                             on a.ExtensionID=b.ExtensionID where a.PanelNum=?num and b.ExtensionNO=?id  ";
            MySqlParameter[] pars =
            {
                new MySqlParameter("?num", num),
                new MySqlParameter("?id", EvaluationHelper.StringToInt(id))
            };
            var result = CustomMySqlHelper.ExecuteScalar(sql, pars);
            return EvaluationHelper.ObjectToString(result);
        }
    }
}
