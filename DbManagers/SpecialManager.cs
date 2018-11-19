using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using CommonHelperLib;
using DbManagers.Helpers;
using IPPhoneModel.EnumTypes;
using IPPhoneModel.ObjectTypes;

namespace DbManagers
{
    // 全非注册模式
    public class SpecialManager:DbManagerBase
    {
        // 文件数据库路径
        private static string _dbPath = Path.Combine(RootPath,"phone.db");

        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="decies"></param>
        public static void GetDevices(out List<Device> decies)
        {
            decies = new List<Device>();
            if (HaveDb())
            {
                SQLiteDataReader reader = null;
                try
                {
                    string sql = "select * from device";
                    reader = SqLiteHelper.ExcuteReader(SqLiteHelper.Open(_dbPath), sql);
                    while (reader.Read())
                    {
                        Device device = new Device();
                        device.Id = EvaluationHelper.ObjectToInt(reader["did"]);
                        device.Ip = EvaluationHelper.ObjectToString(reader["ip"]);
                        device.Name = EvaluationHelper.ObjectToString(reader["name"]);
                        device.Type = EvaluationHelper.ObjectToInt(reader["type"]);
                        device.IsHaveVideo = EvaluationHelper.ObjectToInt(reader["video"]);
                        device.Generation = EvaluationHelper.ObjectToInt(reader["generation"]);
                        device.Extension.Number = EvaluationHelper.ObjectToString(reader["extno"]);
                        device.PanelNum = EvaluationHelper.ObjectToInt(reader["panelnum"]);

                        //device.StateString = "离线";
                        //device.StateString = "通话中";
                        device.StateString = "空闲";
                        //device.StateString = "呼叫中";
                        decies.Add(device);
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
        }

        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="logs"></param>
        public static void GetLogs(LogTypeEnum type,out List<LogMessage> logs)
        {
            logs = new List<LogMessage>();
            if (HaveDb())
            {
                int num = 0;
                switch (type)
                {
                    case LogTypeEnum.System: //系统日志10
                        num = 10;
                        break;

                    case LogTypeEnum.Operation: //操作日志11
                        num = 11;
                        break;

                    case LogTypeEnum.Call: //呼叫日志12
                        num = 12;
                        break;

                    case LogTypeEnum.Video: //视频日志13
                        num = 13;
                        break;

                    case LogTypeEnum.Alarm: //报警日志14
                        num = 14;
                        break;

                    case LogTypeEnum.OrdinaryMsg: //普通消息
                        num = 15;
                        break;

                    case LogTypeEnum.MissedCalled: //未接来电16
                        num = 16;
                        break;

                    case LogTypeEnum.PanelMsg: //面板消息17
                        num = 17;
                        break;
                }
                if (num != 0)
                {
                    SQLiteDataReader reader = null;
                    try
                    {
                        string sql = "select * from log where logtype=@type order by RowID DESC";
                        reader = SqLiteHelper.ExcuteReader(SqLiteHelper.Open(_dbPath), sql,new SQLiteParameter("@type",num));
                        while (reader.Read())
                        {
                            LogMessage log = new LogMessage();
                            log.LogTime = EvaluationHelper.ObjectToDateTime(reader["logtime"]);
                            log.LogMsg = EvaluationHelper.ObjectToString(reader["logmsg"]);
                            log.LogType = EvaluationHelper.ObjectToInt(reader["logtype"]);
                            log.OperaterId = EvaluationHelper.ObjectToString(reader["username"]);
                            log.FromNo = EvaluationHelper.ObjectToString(reader["caller"]);
                            log.PanelNum = EvaluationHelper.ObjectToInt(reader["callpanel"]);
                            logs.Add(log);
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
            }
        }

        /// <summary>
        /// 插入设备
        /// </summary>
        /// <param name="device"></param>
        public static int InsetDevice(Device device)
        {
            if (device != null)
            {
                HaveDb();
                string sql = "insert into device(did,ip,name,type,generation,extno,panelnum) values(@did,@ip,@name,@type,@gen,@ext,@pan)";
                SQLiteParameter[] ps = new SQLiteParameter[7];
                ps[0] = new SQLiteParameter("@did", device.Id);
                ps[1] = new SQLiteParameter("@ip", device.Ip);
                ps[2] = new SQLiteParameter("@name", device.Name);
                ps[3] = new SQLiteParameter("@type", device.Type);
                ps[4] = new SQLiteParameter("@gen", device.Generation);
                ps[5] = new SQLiteParameter("@ext", device.Extension.Number);
                ps[6] = new SQLiteParameter("@pan", device.PanelNum);

                return SqLiteHelper.ExecuteNonquery(SqLiteHelper.Open(_dbPath), sql,ps);
            }
            return -1;
        }
        /// <summary>
        /// 更新设备
        /// </summary>
        /// <param name="device"></param>
        public static int UpdateDevice(Device device)
        {
            if (device != null)
            {
                HaveDb();
                string sql = "update device set ip=@ip,name=@name,type=@type,generation=@gen,extno=@ext,panelnum=@pan where did=@did";
                SQLiteParameter[] ps = new SQLiteParameter[7];
                ps[0] = new SQLiteParameter("@did", device.Id);
                ps[1] = new SQLiteParameter("@ip", device.Ip);
                ps[2] = new SQLiteParameter("@name", device.Name);
                ps[3] = new SQLiteParameter("@type", device.Type);
                ps[4] = new SQLiteParameter("@gen", device.Generation);
                ps[5] = new SQLiteParameter("@ext", device.Extension.Number);
                ps[6] = new SQLiteParameter("@pan", device.PanelNum);

                return SqLiteHelper.ExecuteNonquery(SqLiteHelper.Open(_dbPath), sql,ps);
            }
            return -1;
        }
        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="device"></param>
        public static int DeleteDevice(Device device)
        {
            if (device != null)
            {
                HaveDb();
                string sql = "delete from device where  did=@did";

                return SqLiteHelper.ExecuteNonquery(SqLiteHelper.Open(_dbPath), sql,new SQLiteParameter("@did", device.Id));
            }
            return -1;
        }

        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="log"></param>
        public static int InsetLog(LogMessage log)
        {
            if (log != null)
            {
                HaveDb();
                string sql = "insert into log(logtime,logmsg,logtype,username,caller,callpanel) values(@time,@msg,@type,@name,@caller,@pan)";
                SQLiteParameter[] ps = new SQLiteParameter[6];
                ps[0] = new SQLiteParameter("@time", log.LogTime);
                ps[1] = new SQLiteParameter("@msg", log.LogMsg);
                ps[2] = new SQLiteParameter("@type", log.LogType);
                ps[3] = new SQLiteParameter("@name", log.OperaterId);
                ps[4] = new SQLiteParameter("@caller", Convert.ToInt32(log.FromNo));
                ps[5] = new SQLiteParameter("@pan", log.PanelNum);

                return SqLiteHelper.ExecuteNonquery(SqLiteHelper.Open(_dbPath), sql, ps);
            }
            return -1;
        }

        /// <summary>
        /// 删除过期日志
        /// </summary>
        /// <param name="time">过去时间</param>
        public static int DeleteLogs(DateTime time)
        {
            if (HaveDb())
            {
                string sql = "delete from log where logtime<@time";

                return SqLiteHelper.ExecuteNonquery(SqLiteHelper.Open(_dbPath), sql, new SQLiteParameter("@time", time));
            }
            return 0;
        }

        //判断数据库文件是否存在
        private static bool HaveDb()
        {
            if (!File.Exists(_dbPath))
            {
                try
                {
                    SQLiteConnection.CreateFile(_dbPath);
                    using (SQLiteConnection conn = new SQLiteConnection())
                    {
                        SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
                        builder.DataSource = _dbPath;

                        conn.ConnectionString = builder.ToString();
                        //创建数据表
                        string sql =
                            "CREATE TABLE [device] ([did] INT(1), [ip] VARCHAR(20), [name] VARCHAR(50), " +
                            "[type] INT(1) NOT NULL DEFAULT 1,[video] INT(1) NOT NULL DEFAULT 0," +
                            "[generation] VARCHAR DEFAULT 1, [extno] VARCHAR(20),[panelnum] INT(4) DEFAULT 0," +
                            "CONSTRAINT [] PRIMARY KEY ([did]) ON CONFLICT FAIL);";
                        sql +=
                            "CREATE TABLE [log] ([logtime] TIMESTAMP, [logmsg] VARCHAR(250), [logtype] INT(1)," +
                            "[username] VARCHAR(50),[caller] VARCHAR(20), [callpanel] INT(1) DEFAULT 0);";
                        conn.Open();

                        SQLiteCommand command = new SQLiteCommand(sql, conn);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog(ex.Message);
                }
                return false;
            }
            return true;
        }
    }
}
