using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using CommonHelperLib;
using DbManagers.Helpers;
using IPPhoneModel.EnumTypes;
using IPPhoneModel.ObjectTypes;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace DbManagers
{
    //IP对接使用日志
    public class RecordManager
    {
        /// <summary>
        /// 写日志到数据库
        /// </summary>
        /// <param name="log">日志对象</param>
        /// <returns>受影响的行数</returns>
        public static int WriteLog(LogMessage log)
        {
            int t = 0;
            try
            {
                if (log != null)
                {
                    string sql = "INSERT INTO ipvt_logtable (LogTime, CurrentIP, LogType, LogMsg, UserName, IsRead, " +
                                 "FromNo, ToNo, CallDuration, RecordFilePath, ExtensionNo, call_uuid, PanelNum, cs_server_ip, cs_server_port) " +
                                 "VALUES (?time, ?ip, ?type, ?msg, ?uid, ?isRead, ?from, ?to, ?duration, ?extenNo, ?RecordFilePath, ?call_uuid, ?PanelNum,?cs_server_ip,?cs_server_port)";
                    var ps = new MySqlParameter[15];
                    ps[0] = new MySqlParameter("?time", log.LogTime);
                    ps[1] = new MySqlParameter("?ip", log.LogIp);
                    ps[2] = new MySqlParameter("?type", log.LogType);
                    ps[3] = new MySqlParameter("?msg", log.LogMsg);
                    ps[4] = new MySqlParameter("?uid", log.OperaterId);
                    ps[5] = new MySqlParameter("?isRead", log.IsRead);
                    ps[6] = new MySqlParameter("?from", Convert.ToInt32(log.FromNo));
                    ps[7] = new MySqlParameter("?to", Convert.ToInt32(log.ToNo));
                    ps[8] = new MySqlParameter("?duration", log.CallDuration);
                    ps[9] = new MySqlParameter("?extenNo", string.IsNullOrEmpty(log.ExtensionNo) ? 0 : Convert.ToInt32(log.ExtensionNo));
                    ps[10] = new MySqlParameter("?RecordFilePath", log.RecordFilePath);
                    ps[11] = new MySqlParameter("?call_uuid", log.call_uuid);
                    ps[12] = new MySqlParameter("?PanelNum", log.Callpanelnum);
                    ps[13] = new MySqlParameter("?cs_server_ip", log.cs_server_ip);
                    ps[14] = new MySqlParameter("?cs_server_port", log.cs_server_port);

                    new Thread(() => { t = CustomMySqlHelper.ExecuteNonQuery(sql, ps); }).Start();
                    return t;
                }
                return t;
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.StackTrace);
                return t;
            }
        }

        private static int DateTimeToInt(DateTime LogTime)
        {
            DateTime dateStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            int timeStamp = Convert.ToInt32((LogTime - dateStart).TotalSeconds);
            return timeStamp;
        }

        private static DateTime IntToDateTime(int intTime)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = ((long)intTime * 10000000);
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime targetDt = dtStart.Add(toNow);
            return targetDt;
        }


        /// <summary>
        /// 从数据库获取日志信息
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="type">日志类型，默认所有</param>
        /// <returns></returns>
        public static void GetLogByType(out List<LogMessage> logs, LogTypeEnum type = LogTypeEnum.All, string ExtenNo = null)
        {
            logs = new List<LogMessage>();
            var builder = new StringBuilder();
            builder.Append("select * from ipvt_logtable");

            if (type != LogTypeEnum.All)
            {
                switch (type)
                {
                    case LogTypeEnum.System: //系统日志10
                        builder.Append(" where LogType=10");
                        break;

                    case LogTypeEnum.Operation: //操作日志11
                        builder.Append(" where LogType=11");
                        break;

                    case LogTypeEnum.Call: //呼叫日志12
                        builder.Append(" where LogType=12");
                        break;

                    case LogTypeEnum.Video: //视频日志13
                        builder.Append(" where LogType=13");
                        break;

                    case LogTypeEnum.Alarm: //报警日志14
                        builder.Append(" where LogType=14");
                        break;

                    case LogTypeEnum.OrdinaryMsg: //报警日志15
                        builder.Append(" where LogType=15");
                        break;
                    case LogTypeEnum.MissedCalled: //未接来电16
                        builder.Append(" where LogType=16");
                        break;
                    case LogTypeEnum.PanelMsg: //面板消息17
                        builder.Append(" where LogType=17");
                        break;
                }
            }
            if (!string.IsNullOrEmpty(ExtenNo))
            {
                builder.Append(string.Format(" and FromNo={0} ", Convert.ToInt32(ExtenNo)));
            }
            builder.Append(" order by LogID desc LIMIT 0,50 ");

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString());
                if (reader != null)
                {
                    while (reader.Read()) //读到数据转换为Log对象
                    {
                        var log = new LogMessage();
                        log.Id = EvaluationHelper.ObjectToInt(reader["LogID"]);
                        log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                        log.LogIp = reader["CurrentIP"].ToString();
                        log.LogMsg = reader["LogMsg"].ToString();
                        log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                        log.FromNo = reader["FromNo"].ToString();
                        log.ToNo = reader["ToNo"].ToString();
                        log.ExtensionNo = reader["ExtensionNo"].ToString();
                        log.LogType = EvaluationHelper.ObjectToInt(reader["LogType"]);
                        log.CallDuration = reader["CallDuration"].ToString();
                        log.OperaterId = reader["UserName"].ToString();
                        log.RecordFilePath = reader["RecordFilePath"].ToString();
                        log.call_uuid = EvaluationHelper.ObjectToString(reader["call_uuid"]);
                        log.Callpanelnum = EvaluationHelper.ObjectToString(reader["PanelNum"]);
                        log.cs_server_ip = EvaluationHelper.ObjectToString(reader["cs_server_ip"]);
                        log.cs_server_port = EvaluationHelper.ObjectToString(reader["cs_server_port"]);
                        log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);

                        logs.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("GetLogByType() error!" + ex);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 从数据库获取日志总量
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="type">日志类型，默认所有</param>
        /// <returns></returns>
        public static void  GetLogCount(out int count, int index, LogTypeEnum type = LogTypeEnum.All, string FromNo = null, string ToNo = null, DateTime? StartTime = null, DateTime? EndTime = null)
        {
            count = 0;
            var builder = new StringBuilder();
            builder.Append("select * from ipvt_logtable");
            string str = "";
            if (type != LogTypeEnum.All)
            {
                switch (type)
                {
                    case LogTypeEnum.System: //系统日志10
                        str = " where LogType=10";
                        break;

                    case LogTypeEnum.Operation: //操作日志11
                        str = " where LogType=11";
                        break;

                    case LogTypeEnum.Call: //呼叫日志12
                        str = " where LogType=12";
                        break;

                    case LogTypeEnum.Video: //视频日志13
                        str = " where LogType=13";
                        break;

                    case LogTypeEnum.Alarm: //报警日志14
                        str = " where LogType=14";
                        break;

                    case LogTypeEnum.OrdinaryMsg: //报警日志15
                        str = " where LogType=15";
                        break;
                    case LogTypeEnum.MissedCalled: //未接来电16
                        str = " where LogType=16";
                        break;
                    case LogTypeEnum.PanelMsg: //面板消息17
                        str = " where LogType=17";
                        break;
                }
            }
            if (!string.IsNullOrEmpty(FromNo))
            {
                str += string.Format(" and FromNo={0} and ToNo!=FromNo ", Convert.ToInt32(FromNo));
            }
            if (!string.IsNullOrEmpty(ToNo))
            {
                str += string.Format(" and ToNo={0} ", Convert.ToInt32(ToNo));
            }
            if (StartTime.HasValue)
            {
                str += string.Format(" and LogTime>'{0}' ", StartTime);
            }
            if (EndTime.HasValue)
            {
                str += string.Format(" and LogTime<'{0}' ", EndTime.Value.AddDays(1));
            }

            string sqlcount = string.Format(@" select count(*) as count from ipvt_logtable                      
                        {0}", str);
            MySqlDataReader readercount = CustomMySqlHelper.ExecuteDataReader(sqlcount);
            try
            {
                while (readercount.Read())
                {
                    count = EvaluationHelper.ObjectToInt(readercount["count"]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetCallLogCount!" + ex.Message);
            }
            finally
            {
                if (readercount != null) readercount.Close(); //读取完关闭reader对象
            }
        }


        /// <summary>
        /// 从数据库获取日志信息
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="type">日志类型，默认所有</param>
        /// <returns></returns>
        public static void GetLogByType(ref List<LogMessage> logs, int index, LogTypeEnum type = LogTypeEnum.All, string FromNo = null, string ToNo = null, DateTime? StartTime = null, DateTime? EndTime = null)
        {
            //logs = new List<LogMessage>();
            var builder = new StringBuilder();
            builder.Append("select * from ipvt_logtable");
            string str = "";
            if (type != LogTypeEnum.All)
            {
                switch (type)
                {
                    case LogTypeEnum.System: //系统日志10
                        str = " where LogType=10";
                        break;

                    case LogTypeEnum.Operation: //操作日志11
                        str = " where LogType=11";
                        break;

                    case LogTypeEnum.Call: //呼叫日志12
                        str = " where LogType=12";
                        break;

                    case LogTypeEnum.Video: //视频日志13
                        str = " where LogType=13";
                        break;

                    case LogTypeEnum.Alarm: //报警日志14
                        str = " where LogType=14";
                        break;

                    case LogTypeEnum.OrdinaryMsg: //报警日志15
                        str = " where LogType=15";
                        break;
                    case LogTypeEnum.MissedCalled: //未接来电16
                        str = " where LogType=16";
                        break;
                    case LogTypeEnum.PanelMsg: //面板消息17
                        str = " where LogType=17";
                        break;
                }
            }
            if (!string.IsNullOrEmpty(FromNo))
            {
                str += string.Format(" and FromNo={0} and ToNo!=FromNo ", Convert.ToInt32(FromNo));
            }
            if (!string.IsNullOrEmpty(ToNo))
            {
                str += string.Format(" and ToNo={0} ", Convert.ToInt32(ToNo));
            }
            if (StartTime.HasValue)
            {
                str += string.Format(" and LogTime>'{0}' ", StartTime);
            }
            if (EndTime.HasValue)
            {
                str += string.Format(" and LogTime<'{0}' ", EndTime.Value.AddDays(1));
            }

            builder.Append(str);
            builder.Append(string.Format(" ORDER BY LogTime DESC LIMIT {0},{1}", (index - 1) * 100, 100));

            LogHelper.MainLog(builder.ToString());
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString());
                if (reader != null)
                {
                    while (reader.Read()) //读到数据转换为Log对象
                    {
                        var log = new LogMessage();
                        log.Id = EvaluationHelper.ObjectToInt(reader["LogID"]);
                        log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                        log.LogIp = reader["CurrentIP"].ToString();
                        log.LogMsg = reader["LogMsg"].ToString();
                        log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                        log.FromNo = reader["FromNo"].ToString();
                        log.ToNo = reader["ToNo"].ToString();
                        log.ExtensionNo = reader["ExtensionNo"].ToString();
                        log.LogType = EvaluationHelper.ObjectToInt(reader["LogType"]);
                        log.CallDuration = reader["CallDuration"].ToString();
                        log.OperaterId = reader["UserName"].ToString();
                        log.RecordFilePath = reader["RecordFilePath"].ToString();
                        log.call_uuid = EvaluationHelper.ObjectToString(reader["call_uuid"]);
                        log.Callpanelnum = EvaluationHelper.ObjectToString(reader["PanelNum"]);
                        log.cs_server_ip = EvaluationHelper.ObjectToString(reader["cs_server_ip"]);
                        log.cs_server_port = EvaluationHelper.ObjectToString(reader["cs_server_port"]);
                        log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);

                        logs.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("GetLogByType() error!" + ex);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 从数据库获取日志信息(报警日志，一般消息)
        /// </summary>
        /// <returns></returns>
        public static void GetMessages(out List<LogMessage> logs, LogTypeEnum type = LogTypeEnum.All, string number = null)
        {
            logs = new List<LogMessage>();
            var builder = new StringBuilder();
            builder.Append("select * from ipvt_logtable");
            if (type == LogTypeEnum.All)
            {
                builder.Append(" where LogType=14 or LogType=15");
            }
            else if(type == LogTypeEnum.Alarm)
            {
                builder.Append(" where LogType=14");
            }
            else if (type == LogTypeEnum.OrdinaryMsg)
            {
                builder.Append(" where LogType=15");
            }
            else if (type == LogTypeEnum.MissedCalled)   //yk20170119
            {
                builder.Append(" where LogType=16 and ExtensionNo=?num");
            }
            else if (type == LogTypeEnum.PanelMsg)
            {
                builder.Append(" where LogType=17");
            }
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString(), new[] { new MySqlParameter("?num", string.IsNullOrEmpty(number) ? 0 : Convert.ToInt32(number)) });
                if (reader != null)
                {
                    while (reader.Read()) //读到数据转换为Log对象
                    {
                        var log = new LogMessage();
                        log.Id = EvaluationHelper.ObjectToInt(reader["LogID"]);
                        log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                        log.LogIp = reader["CurrentIP"].ToString();
                        log.LogMsg = reader["LogMsg"].ToString();
                        log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                        log.FromNo = reader["FromNo"].ToString();
                        log.ToNo = reader["ToNo"].ToString();
                        log.ExtensionNo = reader["ExtensionNo"].ToString();
                        log.LogType = EvaluationHelper.ObjectToInt(reader["LogType"]);
                        log.CallDuration = reader["CallDuration"].ToString();
                        log.OperaterId = reader["UserName"].ToString();
                        log.RecordFilePath = reader["RecordFilePath"].ToString();
                        log.call_uuid = EvaluationHelper.ObjectToString(reader["call_uuid"]);
                        log.Callpanelnum = EvaluationHelper.ObjectToString(reader["PanelNum"]);
                        log.cs_server_ip = EvaluationHelper.ObjectToString(reader["cs_server_ip"]);
                        log.cs_server_port = EvaluationHelper.ObjectToString(reader["cs_server_port"]);
                        log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);

                        logs.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("GetLogByType() error!" + ex);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 删除日志记录
        /// </summary>
        /// <param name="id">日志id</param>
        /// <returns>受影响的行数</returns>
        public static int DeleteLog(int id)
        {
            int result = 0;
            string sql = "delete from ipvt_logtable where LogID=?id";

            var ps = new MySqlParameter[1];
            ps[0]=new MySqlParameter("?id",id);

            try
            {
                result = CustomMySqlHelper.ExecuteNonQuery(sql, ps);
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("DeleteLog error!" + ex);
            }

            return result;
        }

        /// <summary>
        /// 删除过期数据
        /// </summary>
        /// <param name="days">过期天数</param>
        public static void RemoveStaledata(int days)
        {
            if (days < 179)
            {
                days = 179;
            }
            DateTime time = DateTime.Now.Subtract(new TimeSpan(days, 0, 0, 0)).Date;
            string selectSql = @"select a.RecordFilePath from ipvt_logtable a
                                    INNER JOIN (select * from ipvt_logtable where LogTime>?time limit 1) b 
                                    where a.LogID<=b.LogID";

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(selectSql, new MySqlParameter("?time", time));
                while (reader.Read())
                {
                    string path = EvaluationHelper.ObjectToString(reader["RecordFilePath"]);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.Message);
            }
            finally
            {
                if (reader != null) { reader.Close(); }
            }
//            string sql = @"DELETE a from ipvt_logtable a
//                                    INNER JOIN (select * from ipvt_logtable where unix_timestamp(LogTime)>unix_timestamp(?time) limit 1) b 
//                                    where a.LogID<=b.LogID";
//            int result = CustomMySqlHelper.ExecuteNonQuery(sql, new MySqlParameter("?time",time));
//            LogHelper.MainLog(string.Format("删除{0}条过期日志！", result));
        }

        #region 未接来电处理相关
        /// <summary>
        /// 获取未接来电信息
        /// </summary>
        /// <param name="number">分机号</param>
        /// <param name="logs"></param>
        public static void GetMissedCall(string number,out List<LogMessage> logs)
        {
            logs = new List<LogMessage>();
            string sql;
            if (string.IsNullOrEmpty(number))
            {
                sql = "select * from ipvt_logtable";
            }
            else
            {
                sql = "select * from ipvt_logtable where ExtensionNo=?num";
            }

            MySqlDataReader reader = CustomMySqlHelper.ExecuteDataReader(sql,
                new[] { new MySqlParameter("?num", string.IsNullOrEmpty(number) ? 0 : Convert.ToInt32(number)) });

            try
            {
                while (reader.Read())
                {
                    LogMessage log = new LogMessage();
                    log.Id = EvaluationHelper.ObjectToInt(reader["LogID"]);
                    log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                    log.LogIp = reader["CurrentIP"].ToString();
                    log.LogMsg = reader["LogMsg"].ToString();
                    log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                    log.FromNo = reader["FromNo"].ToString();
                    log.ToNo = reader["ToNo"].ToString();
                    log.ExtensionNo = reader["ExtensionNo"].ToString();
                    log.LogType = EvaluationHelper.ObjectToInt(reader["LogType"]);
                    log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);
                    log.OperaterId = reader["UserName"].ToString();

                    logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetMissedCall!" + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 写未接来电信息
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static bool InsertMissedCall(LogMessage log)
        {
            if (log != null)
            {
                string sql = "insert into ipvt_missedcalltable(CallerNumber,PanelNumber,MsgTime,MsgContent,CurrentIp,ExtensionNo,UserName) " +
                    "values(?from,?panel,?time,?msg,?ip,?no,?name)";

                MySqlParameter[] ps = new MySqlParameter[7];
                ps[0] = new MySqlParameter("?from", Convert.ToInt32(log.FromNo));
                ps[1] = new MySqlParameter("?panel", log.PanelNum);
                ps[2] = new MySqlParameter("?time", log.LogTime);
                ps[3] = new MySqlParameter("?msg", log.LogMsg);
                ps[4] = new MySqlParameter("?ip", log.LogIp);
                ps[5] = new MySqlParameter("?no", log.ExtensionNo);
                ps[6] = new MySqlParameter("?name", log.OperaterId);

                int result = CustomMySqlHelper.ExecuteNonQuery(sql, ps);

                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据id设置未接来电信息已处理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SetMissedCallIsRead(int id)
        {
            string sql = "update ipvt_missedcalltable set Processed=1 where MID=?id";
            MySqlParameter[] ps= {new MySqlParameter("?id",id)};

            int result = CustomMySqlHelper.ExecuteNonQuery(sql, ps);
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据id设置日志为已处理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SetLogIsRead(int id)
        {
            string sql = "update ipvt_logtable set IsRead=1 where LogID=?id";
            MySqlParameter[] ps = { new MySqlParameter("?id", id) };

            int result = CustomMySqlHelper.ExecuteNonQuery(sql, ps);
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据分机号和面板号设置已处理
        /// </summary>
        /// <returns></returns>
        public static bool SetIsRead(string from,int panelNum)
        {
            string sql = "update ipvt_missedcalltable set Processed=1 where CallerNumber=?from and PanelNumber=?panel";
            MySqlParameter[] ps = { new MySqlParameter("?from", from), new MySqlParameter("?panel", panelNum) };

            int result = CustomMySqlHelper.ExecuteNonQuery(sql, ps);
            if (result > 0)
            {
                return true;
            }
            return false;
        }


        #endregion

        /// <summary>
        /// 获取日志类型
        /// </summary>
        public static void GetLogType(out Dictionary<int,string> dictionary)
        {
            dictionary = new Dictionary<int, string>();
            string sql = "select TypeID,TypeName from ipvt_typetable";
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                while (reader.Read())
                {
                    dictionary.Add(EvaluationHelper.ObjectToInt(reader["TypeID"]),
                        EvaluationHelper.ObjectToString(reader["TypeName"]));
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.ToString());
            }
            finally
            {
                if (reader != null) { reader.Close(); }
            }
        }

        /// <summary>
        /// 获取相应类型日志数量
        /// </summary>
        /// <param name="type">日志类型</param>
        public static int GetLogNumByType(LogTypeEnum type)
        {
            var builder = new StringBuilder();
            builder.Append("select COUNT(LogID) from ipvt_logtable");

            if (type != LogTypeEnum.All)
            {
                switch (type)
                {
                    case LogTypeEnum.System: //系统日志10
                        builder.Append(" where LogType=10");
                        break;

                    case LogTypeEnum.Operation: //操作日志11
                        builder.Append(" where LogType=11");
                        break;

                    case LogTypeEnum.Call: //呼叫日志12
                        builder.Append(" where LogType=12");
                        break;

                    case LogTypeEnum.Video: //视频日志13
                        builder.Append(" where LogType=13");
                        break;

                    case LogTypeEnum.Alarm: //报警日志14
                        builder.Append(" where LogType=14");
                        break;

                    case LogTypeEnum.OrdinaryMsg: //普通消息15
                        builder.Append(" where LogType=15");
                        break;

                    case LogTypeEnum.MissedCalled: //未接来电16
                        builder.Append(" where LogType=16");
                        break;

                    case LogTypeEnum.PanelMsg: //面板消息17
                        builder.Append(" where LogType=17");
                        break;
                }
            }
           return EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(builder.ToString()));
        }


        /// <summary>
        /// 从数据库获取日志信息
        /// </summary>
        /// <param name="logs">输出</param>
        /// <param name="type">日志类型</param>
        /// <param name="from">开始索引</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public static void GetLogByType(out List<LogMessage> logs, LogTypeEnum type,int from,int count)
        {
            logs = new List<LogMessage>();
            var builder = new StringBuilder();
            builder.Append("select * from ipvt_logtable");

            if (type != LogTypeEnum.All)
            {
                switch (type)
                {
                    case LogTypeEnum.System: //系统日志10
                        builder.Append(" where LogType=10");
                        break;

                    case LogTypeEnum.Operation: //操作日志11
                        builder.Append(" where LogType=11");
                        break;

                    case LogTypeEnum.Call: //呼叫日志12
                        builder.Append(" where LogType=12");
                        break;

                    case LogTypeEnum.Video: //视频日志13
                        builder.Append(" where LogType=13");
                        break;

                    case LogTypeEnum.Alarm: //报警日志14
                        builder.Append(" where LogType=14");
                        break;

                    case LogTypeEnum.OrdinaryMsg: //报警日志14
                        builder.Append(" where LogType=15");
                        break;

                    case LogTypeEnum.PanelMsg: //面板消息17
                        builder.Append(" where LogType=17");
                        break;
                }
            }
            builder.Append(string.Format(" LIMIT {0},{1}",from,count));

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString());
                if (reader != null)
                {
                    while (reader.Read()) //读到数据转换为Log对象
                    {
                        var log = new LogMessage();
                        log.Id = EvaluationHelper.ObjectToInt(reader["LogID"]);
                        log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                        log.LogIp = reader["CurrentIP"].ToString();
                        log.LogMsg = reader["LogMsg"].ToString();
                        log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                        log.FromNo = reader["FromNo"].ToString();
                        log.ToNo = reader["ToNo"].ToString();
                        log.ExtensionNo = reader["ExtensionNo"].ToString();
                        log.LogType = EvaluationHelper.ObjectToInt(reader["LogType"]);
                        log.CallDuration = reader["CallDuration"].ToString();
                        log.OperaterId = reader["UserName"].ToString();
                        log.RecordFilePath = reader["RecordFilePath"].ToString();
                        log.call_uuid = EvaluationHelper.ObjectToString(reader["call_uuid"]);
                        //log.Callpanelnum = EvaluationHelper.ObjectToString(reader["PanelNum"]);
                        log.cs_server_ip = EvaluationHelper.ObjectToString(reader["cs_server_ip"]);
                        log.cs_server_port = EvaluationHelper.ObjectToString(reader["cs_server_port"]);
                        log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);

                        logs.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("GetLogByType() error!" + ex);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 获取未接来电信息
        /// </summary>
        /// <param name="number">分机号</param>
        /// <param name="from">开始索引</param>
        /// <param name="count">数量</param>
        /// <param name="logs"></param>
        public static void GetMissedCall(string number, int from, int count, out List<LogMessage> logs)
        {
            logs = new List<LogMessage>();
            string sql;
            if (string.IsNullOrEmpty(number))
            {
                sql = string.Format("select * from ipvt_logtable LIMIT {0},{1}", from, count);
            }
            else
            {
                sql = string.Format("select * from ipvt_logtable where ExtensionNo=?num LIMIT {0},{1}", from, count);
            }

            MySqlDataReader reader = CustomMySqlHelper.ExecuteDataReader(sql,
                new[] { new MySqlParameter("?num", string.IsNullOrEmpty(number) ? 0 : Convert.ToInt32(number)) });

            try
            {
                while (reader.Read())
                {
                    LogMessage log = new LogMessage();
                    log.Id = EvaluationHelper.ObjectToInt(reader["LogID"]);
                    log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                    log.LogIp = reader["CurrentIP"].ToString();
                    log.LogMsg = reader["LogMsg"].ToString();
                    log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                    log.FromNo = reader["FromNo"].ToString();
                    log.ToNo = reader["ToNo"].ToString();
                    log.ExtensionNo = reader["ExtensionNo"].ToString();
                    log.LogType = EvaluationHelper.ObjectToInt(reader["LogType"]);
                    log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);
                    //log.CallDuration = reader["CallDuration"].ToString();
                    log.OperaterId = reader["UserName"].ToString();
                    //log.RecordFilePath = reader["RecordFilePath"].ToString();

                    logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetMissedCall!" + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }


        /// <summary>
        /// 从数据库获取日志信息
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        public static void GetLogByDate(out List<LogMessage> logs, DateTime? start, DateTime? end)
        {
            logs = new List<LogMessage>();
            var builder = new StringBuilder();
            builder.Append("select * from ipvt_logtable where LogType=12");
            MySqlParameter[] pars =
            {
                new MySqlParameter("?starttime", start),
                new MySqlParameter("?endtime", !end.HasValue ? end : end.Value.Date.AddDays(1))
            };
            if (start.HasValue && !end.HasValue)
            {
                builder.Append(" and LogTime>=?starttime");
            }
            if (!start.HasValue && end.HasValue)
            {
                builder.Append(" and LogTime<?endtime");
            }
            if (start.HasValue && end.HasValue)
            {
                builder.Append(" and LogTime BETWEEN ?starttime and ?endtime");
            }

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString(), pars);
                if (reader != null)
                {
                    while (reader.Read()) //读到数据转换为Log对象
                    {
                        var log = new LogMessage();
                        log.Id = EvaluationHelper.ObjectToInt(reader["LogID"]);
                        log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                        log.LogIp = reader["CurrentIP"].ToString();
                        log.LogMsg = reader["LogMsg"].ToString();
                        log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                        log.FromNo = reader["FromNo"].ToString();
                        log.ToNo = reader["ToNo"].ToString();
                        log.ExtensionNo = reader["ExtensionNo"].ToString();
                        log.LogType = EvaluationHelper.ObjectToInt(reader["LogType"]);
                        log.CallDuration = reader["CallDuration"].ToString();
                        log.OperaterId = reader["UserName"].ToString();
                        log.RecordFilePath = reader["RecordFilePath"].ToString();
                        log.call_uuid = EvaluationHelper.ObjectToString(reader["call_uuid"]);
                        //log.Callpanelnum = EvaluationHelper.ObjectToString(reader["PanelNum"]);
                        log.cs_server_ip = EvaluationHelper.ObjectToString(reader["cs_server_ip"]);
                        log.cs_server_port = EvaluationHelper.ObjectToString(reader["cs_server_port"]);
                        log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);

                        logs.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("GetLogByType() error!" + ex);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 删除日志记录
        /// </summary>
        /// <param name="id">日志id</param>
        /// <returns>受影响的行数</returns>
        public static int DeleteLogByDate(DateTime? start, DateTime? end)
        {
            int result = 0;
             var builder = new StringBuilder();
            builder.Append("delete from ipvt_logtable where LogType=12");

            MySqlParameter[] pars =
            {
                new MySqlParameter("?starttime", start),
                new MySqlParameter("?endtime", !end.HasValue ? end : end.Value.Date.AddDays(1))
            };
            if (start.HasValue && !end.HasValue)
            {
                builder.Append(" and LogTime>=?starttime");
            }
            if (!start.HasValue && end.HasValue)
            {
                builder.Append(" and LogTime<?endtime");
            }
            if (start.HasValue && end.HasValue)
            {
                builder.Append(" and LogTime BETWEEN ?starttime and ?endtime");
            }

            try
            {
                result = CustomMySqlHelper.ExecuteNonQuery(builder.ToString(), pars);
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("DeleteLog error!" + ex);
            }

            return result;
        }

        /// <summary>
        /// 获取通话记录(按分机号查询)
        /// </summary>
        /// <param name="number">分机号</param>
        /// <param name="from">开始索引</param>
        /// <param name="count">数量</param>
        /// <param name="logs"></param>
        public static void GetCallLog(string number, string number2, int from, int count, out List<LogMessage> logs, DateTime? dtpFrom, DateTime? dtpTo)
        {
            string sql1 = "";
            string sql2 = "";
            string sql3 = "";
            string sql4 = "";
            if (dtpFrom.HasValue)
            {
                sql1 += string.Format("and a.LogTime>='{0}' ", dtpFrom.Value);
            }
            if (dtpTo.HasValue)
            {
                sql1 += string.Format("and a.LogTime<='{0}' ", dtpTo.Value);
            }
            if (!string.IsNullOrEmpty(number2))   //该查询条件放最后
            {
                if (number != number2)
                {
                    sql2 += string.Format("(a.FromNo like '%{1}%' and a.ToNo={0}) ",
                        Convert.ToInt32(number), number2);
                    sql3 += string.Format("(a.FromNo={0} and a.ToNo like '%{1}%') ",
                        Convert.ToInt32(number), number2);
                }
                else
                {
                    sql2 += string.Format(" a.ToNo={0} ", Convert.ToInt32(number));
                    sql3 += string.Format(" a.ToNo={0} ", Convert.ToInt32(number));
                }
            }
            else
            {
                sql2 += string.Format("a.ToNo={0} ",
                        Convert.ToInt32(number));
                sql3 += string.Format("a.FromNo={0} ",
                    Convert.ToInt32(number));
            }
            sql4 += string.Format(" ({0} or {1}) ",
                sql2, sql3);
            logs = new List<LogMessage>();
            string limit = string.Format(" LIMIT {0},{1}", from, count);
            string sql = string.Format(@"
                (select a.*,c.DeviceName as FromName,e.DeviceName as ToName from ipvt_logtable as a
LEFT JOIN ipvt_extensionmessagetable as b on a.FromNo=b.ExtensionNO
LEFT JOIN ipvt_deviceinfotable as c on b.ExtensionID=c.ExtensionID
LEFT JOIN ipvt_extensionmessagetable as d on a.ToNo=d.ExtensionNO
LEFT JOIN ipvt_deviceinfotable as e on d.ExtensionID=e.ExtensionID
 where a.LogType=12 {0} and {2} and a.ToNo!=a.FromNo)
union all
(
select a.*,c.DeviceName as FromName,e.DeviceName as ToName from ipvt_logtable as a
LEFT JOIN ipvt_extensionmessagetable as b on a.FromNo=b.ExtensionNO
LEFT JOIN ipvt_deviceinfotable as c on b.ExtensionID=c.ExtensionID
LEFT JOIN ipvt_extensionmessagetable as d on a.ToNo=d.ExtensionNO
LEFT JOIN ipvt_deviceinfotable as e on d.ExtensionID=e.ExtensionID
 where a.LogType=12 {0} and {3} 
) 
order by LogTime desc {1}",
                sql1, limit, sql2, sql3);

            MySqlDataReader reader = CustomMySqlHelper.ExecuteDataReader(sql);
            try
            {
                while (reader.Read())
                {
                    LogMessage log = new LogMessage();
                    log.FromNo = EvaluationHelper.ObjectToString(reader["FromNo"]);
                    log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                    log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                    log.LogMsg = EvaluationHelper.ObjectToString(reader["LogMsg"]);
                    log.LogIp = reader["CurrentIP"].ToString();
                    log.ToNo = EvaluationHelper.ObjectToString(reader["ToNo"]);
                    log.ExtensionNo = EvaluationHelper.ObjectToString(reader["ExtensionNo"]);
                    log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);
                    log.OperaterId = EvaluationHelper.ObjectToString(reader["UserName"]);
                    log.CallDuration = EvaluationHelper.ObjectToString(reader["CallDuration"]);
                    log.call_uuid = EvaluationHelper.ObjectToString(reader["call_uuid"]);
                    log.Callpanelnum = EvaluationHelper.ObjectToString(reader["PanelNum"]);
                    log.cs_server_ip = EvaluationHelper.ObjectToString(reader["cs_server_ip"]);
                    log.cs_server_port = EvaluationHelper.ObjectToString(reader["cs_server_port"]);
                    log.FromName = EvaluationHelper.ObjectToString(reader["FromName"]);
                    log.ToName = EvaluationHelper.ObjectToString(reader["ToName"]);
                    logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetMissedCall!" + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 获取呼出记录
        /// </summary>
        /// <param name="number">分机号</param>
        /// <param name="from">开始索引</param>
        /// <param name="count">数量</param>
        /// <param name="logs"></param>
        public static void GetCallOut(string number, int from, int count, out List<LogMessage> logs, out int SumCount)
        {
            SumCount = 0;
            string sqlcount = string.Format(@"select ceil(count(*)/{1}) as count from                       
                        (select * from ipvt_logtable as b where b.LogType=12 and b.FromNo={0} ) 
                        as c", Convert.ToInt32(number), count);
            MySqlDataReader readercount = CustomMySqlHelper.ExecuteDataReader(sqlcount);
            try
            {
                while (readercount.Read())
                {
                    SumCount = EvaluationHelper.ObjectToInt(readercount["count"]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetCallLogCount!" + ex.Message);
            }
            finally
            {
                if (readercount != null) readercount.Close(); //读取完关闭reader对象
            }

            logs = new List<LogMessage>();
            string sql = string.Format(@"select a.*,c.DeviceName as FromName,e.DeviceName as ToName from ipvt_logtable as a
LEFT JOIN ipvt_extensionmessagetable as b on a.FromNo=b.ExtensionNO
LEFT JOIN ipvt_deviceinfotable as c on b.ExtensionID=c.ExtensionID
LEFT JOIN ipvt_extensionmessagetable as d on a.ToNo=d.ExtensionNO
LEFT JOIN ipvt_deviceinfotable as e on d.ExtensionID=e.ExtensionID
where a.LogType=12 and a.FromNo={0} LIMIT {1},{2}", Convert.ToInt32(number), from, count);

            MySqlDataReader reader = CustomMySqlHelper.ExecuteDataReader(sql);

            try
            {
                while (reader.Read())
                {
                    LogMessage log = new LogMessage();
                    log.FromNo = EvaluationHelper.ObjectToString(reader["FromNo"]);
                    log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                    log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                    log.LogMsg = EvaluationHelper.ObjectToString(reader["LogMsg"]);
                    log.LogIp = reader["CurrentIP"].ToString();
                    log.ToNo = EvaluationHelper.ObjectToString(reader["ToNo"]);
                    log.ExtensionNo = EvaluationHelper.ObjectToString(reader["ExtensionNo"]);
                    log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);
                    log.OperaterId = EvaluationHelper.ObjectToString(reader["UserName"]);
                    log.CallDuration = EvaluationHelper.ObjectToString(reader["CallDuration"]);
                    log.call_uuid = EvaluationHelper.ObjectToString(reader["call_uuid"]);
                    log.Callpanelnum = EvaluationHelper.ObjectToString(reader["PanelNum"]);
                    log.cs_server_ip = EvaluationHelper.ObjectToString(reader["cs_server_ip"]);
                    log.cs_server_port = EvaluationHelper.ObjectToString(reader["cs_server_port"]);
                    log.FromName = EvaluationHelper.ObjectToString(reader["FromName"]);
                    log.ToName = EvaluationHelper.ObjectToString(reader["ToName"]);

                    logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetMissedCall!" + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 获取呼出记录(按分机号查询)
        /// </summary>
        /// <param name="number">分机号</param>
        /// <param name="from">开始索引</param>
        /// <param name="count">数量</param>
        /// <param name="logs"></param>
        public static void GetCallOut(string number, string number2, int from, int count, out List<LogMessage> logs, out int SumCount,DateTime? dtpFrom,DateTime?dtpTo)
        {
            SumCount = 0;
            string sql1 = "";
            if (!string.IsNullOrEmpty(number))
            {
                sql1 += string.Format("and a.FromNo={0} ", Convert.ToInt32(number));
            }
            if (!string.IsNullOrEmpty(number2))
            {
                sql1 += string.Format("and a.ToNo={0} ", Convert.ToInt32(number2));
            }
            if (dtpFrom.HasValue)
            {
                sql1 += string.Format("and a.LogTime>='{0}' ", dtpFrom.Value);
            }
            if (dtpTo.HasValue)
            {
                sql1 += string.Format("and a.LogTime<='{0}' ", dtpTo.Value);
            }
            string sqlcount = string.Format(@"select ceil(count(*)/{1}) as count from                       
                        (select * from ipvt_logtable as a where a.LogType=12 {0} ) 
                        as c", sql1, count);
            MySqlDataReader readercount = CustomMySqlHelper.ExecuteDataReader(sqlcount);
            try
            {
                while (readercount.Read())
                {
                    SumCount = EvaluationHelper.ObjectToInt(readercount["count"]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetCallLogCount!" + ex.Message);
            }
            finally
            {
                if (readercount != null) readercount.Close(); //读取完关闭reader对象
            }

            logs = new List<LogMessage>();
            string limit = SumCount == 1 ? "" : ((SumCount - 1) * 7 == from ? string.Format("LIMIT {0}", count) : string.Format(" LIMIT {0},{1}", from, count));
            string sql = string.Format(@"(select a.*,c.DeviceName as FromName,e.DeviceName as ToName from ipvt_logtable as a
LEFT JOIN ipvt_extensionmessagetable as b on a.FromNo=b.ExtensionNO
LEFT JOIN ipvt_deviceinfotable as c on b.ExtensionID=c.ExtensionID
LEFT JOIN ipvt_extensionmessagetable as d on a.ToNo=d.ExtensionNO
LEFT JOIN ipvt_deviceinfotable as e on d.ExtensionID=e.ExtensionID
where a.LogType=12 {0} order by a.LogTime desc LIMIT {1}", sql1, limit);

            MySqlDataReader reader = CustomMySqlHelper.ExecuteDataReader(sql);

            try
            {
                while (reader.Read())
                {
                    LogMessage log = new LogMessage();
                    log.FromNo = EvaluationHelper.ObjectToString(reader["FromNo"]);
                    log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                    log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                    log.LogMsg = EvaluationHelper.ObjectToString(reader["LogMsg"]);
                    log.LogIp = reader["CurrentIP"].ToString();
                    log.ToNo = EvaluationHelper.ObjectToString(reader["ToNo"]);
                    log.ExtensionNo = EvaluationHelper.ObjectToString(reader["ExtensionNo"]);
                    log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);
                    log.OperaterId = EvaluationHelper.ObjectToString(reader["UserName"]);
                    log.CallDuration = EvaluationHelper.ObjectToString(reader["CallDuration"]);
                    log.call_uuid = EvaluationHelper.ObjectToString(reader["call_uuid"]);
                    log.Callpanelnum = EvaluationHelper.ObjectToString(reader["PanelNum"]);
                    log.cs_server_ip = EvaluationHelper.ObjectToString(reader["cs_server_ip"]);
                    log.cs_server_port = EvaluationHelper.ObjectToString(reader["cs_server_port"]);
                    log.FromName = EvaluationHelper.ObjectToString(reader["FromName"]);
                    log.ToName = EvaluationHelper.ObjectToString(reader["ToName"]);

                    logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetMissedCall!" + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 获取未接记录
        /// </summary>
        /// <param name="number">分机号</param>
        /// <param name="from">开始索引</param>
        /// <param name="count">数量</param>
        /// <param name="logs"></param>
        public static void GetMissCall(string number, int from, int count, out List<LogMessage> logs, out int SumCount)
        {
            SumCount = 0;
            string sqlcount = string.Format(@"select ceil(count(*)/{0}) as count from                       
                        (select * from ipvt_logtable as b where b.LogType=16 ) as c", count);
            MySqlDataReader readercount = CustomMySqlHelper.ExecuteDataReader(sqlcount);
            try
            {
                while (readercount.Read())
                {
                    SumCount = EvaluationHelper.ObjectToInt(readercount["count"]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetCallLogCount!" + ex.Message);
            }
            finally
            {
                if (readercount != null) readercount.Close(); //读取完关闭reader对象
            }

            logs = new List<LogMessage>();
            string sql = string.Format(@"select a.*,c.DeviceName,d.PanelName from ipvt_logtable as a 
LEFT JOIN ipvt_extensionmessagetable as b on a.ExtensionNo=b.ExtensionNO
LEFT JOIN ipvt_deviceinfotable as c on b.ExtensionID=c.ExtensionID
LEFT JOIN ipvt_panelinfotable as d on b.ExtensionID=d.ExtensionID and a.PanelNum=d.PanelNum
where a.LogType=16 order by a.LogTime desc LIMIT {0},{1}", from, count);

            MySqlDataReader reader = CustomMySqlHelper.ExecuteDataReader(sql);

            try
            {
                while (reader.Read())
                {
                    LogMessage log = new LogMessage();
                    log.FromNo = EvaluationHelper.ObjectToString(reader["FromNo"]);
                    log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                    log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                    log.LogMsg = EvaluationHelper.ObjectToString(reader["LogMsg"]);
                    log.LogIp = reader["CurrentIP"].ToString();
                    log.ToNo = EvaluationHelper.ObjectToString(reader["ToNo"]);
                    log.ExtensionNo = EvaluationHelper.ObjectToString(reader["ExtensionNo"]);
                    log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);
                    log.OperaterId = EvaluationHelper.ObjectToString(reader["UserName"]);
                    log.CallDuration = EvaluationHelper.ObjectToString(reader["CallDuration"]);
                    log.call_uuid = EvaluationHelper.ObjectToString(reader["call_uuid"]);
                    log.Callpanelnum = EvaluationHelper.ObjectToString(reader["PanelNum"]);
                    log.cs_server_ip = EvaluationHelper.ObjectToString(reader["cs_server_ip"]);
                    log.cs_server_port = EvaluationHelper.ObjectToString(reader["cs_server_port"]);
                    log.ExtensionName = EvaluationHelper.ObjectToString(reader["DeviceName"]);
                    log.panelName = EvaluationHelper.ObjectToString(reader["PanelName"]);

                    logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetMissedCall!" + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 获取未接记录(按分机号查询)
        /// </summary>
        /// <param name="number">分机号</param>
        /// <param name="from">开始索引</param>
        /// <param name="count">数量</param>
        /// <param name="logs"></param>
        public static void GetMissCall(string number, string number2, int from, int count, out List<LogMessage> logs, DateTime? dtpFrom, DateTime? dtpTo)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(number2))
            {
                sql1 += string.Format("and a.ExtensionNo like '%{0}%' ", string.IsNullOrEmpty(number2) ? 0 : Convert.ToInt32(number2));
            }
            if (dtpFrom.HasValue)
            {
                sql1 += string.Format("and a.LogTime>='{0}' ", dtpFrom.Value);
            }
            if (dtpTo.HasValue)
            {
                sql1 += string.Format("and a.LogTime<='{0}' ", dtpTo.Value);
            }
//            string sqlcount = string.Format(@"select ceil(count(*)/{1}) as count from
//                        (select * from ipvt_logtable as a where a.LogType=16 {0}
//                        ) as c", sql1, count);
//            MySqlDataReader readercount = CustomMySqlHelper.ExecuteDataReader(sqlcount);
//            try
//            {
//                while (readercount.Read())
//                {
//                    SumCount = EvaluationHelper.ObjectToInt(readercount["count"]);
//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.MainLog("Error in GetCallLogCount!" + ex.Message);
//            }
//            finally
//            {
//                if (readercount != null) readercount.Close(); //读取完关闭reader对象
//            }

            logs = new List<LogMessage>();
            string limit = string.Format("order by LogTime desc LIMIT {0},{1})", from, count);
            string sql = string.Format(@"(select a.*,c.DeviceName,d.PanelName from ipvt_logtable as a 
LEFT JOIN ipvt_extensionmessagetable as b on a.ExtensionNo=b.ExtensionNO
LEFT JOIN ipvt_deviceinfotable as c on b.ExtensionID=c.ExtensionID
LEFT JOIN ipvt_panelinfotable as d on b.ExtensionID=d.ExtensionID and a.PanelNum=d.PanelNum
where a.LogType=16 {0} {1}", sql1, limit);

            MySqlDataReader reader = CustomMySqlHelper.ExecuteDataReader(sql);

            try
            {
                while (reader.Read())
                {
                    LogMessage log = new LogMessage();
                    log.FromNo = EvaluationHelper.ObjectToString(reader["FromNo"]);
                    log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                    log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                    log.LogMsg = EvaluationHelper.ObjectToString(reader["LogMsg"]);
                    log.LogIp = reader["CurrentIP"].ToString();
                    log.ToNo = EvaluationHelper.ObjectToString(reader["ToNo"]);
                    log.ExtensionNo = EvaluationHelper.ObjectToString(reader["ExtensionNo"]);
                    log.PanelNum = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PanelNum"].ToString()) ? "0" : reader["PanelNum"]);
                    log.OperaterId = EvaluationHelper.ObjectToString(reader["UserName"]);
                    log.CallDuration = EvaluationHelper.ObjectToString(reader["CallDuration"]);
                    log.call_uuid = EvaluationHelper.ObjectToString(reader["call_uuid"]);
                    log.Callpanelnum = EvaluationHelper.ObjectToString(reader["PanelNum"]);
                    log.cs_server_ip = EvaluationHelper.ObjectToString(reader["cs_server_ip"]);
                    log.cs_server_port = EvaluationHelper.ObjectToString(reader["cs_server_port"]);
                    log.ExtensionName = EvaluationHelper.ObjectToString(reader["DeviceName"]);
                    log.panelName = EvaluationHelper.ObjectToString(reader["PanelName"]);

                    logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetMissedCall!" + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }


        /// <summary>
        /// 从数据库获取日志信息
        /// </summary>
        /// <param name="logs">输出</param>
        /// <param name="type">日志类型</param>
        /// <param name="from">开始索引</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public static void GetLogByType(out List<LogMessage> logs, LogTypeEnum type, int from, int count, DateTime? start, DateTime? end)
        {
            logs = new List<LogMessage>();
            var builder = new StringBuilder();
            builder.Append("select * from ipvt_logtable");

            if (type != LogTypeEnum.All)
            {
                switch (type)
                {
                    case LogTypeEnum.System: //系统日志10
                        builder.Append(" where LogType=10");
                        break;

                    case LogTypeEnum.Operation: //操作日志11
                        builder.Append(" where LogType=11");
                        break;

                    case LogTypeEnum.Call: //呼叫日志12
                        builder.Append(" where LogType=12");
                        break;

                    case LogTypeEnum.Video: //视频日志13
                        builder.Append(" where LogType=13");
                        break;

                    case LogTypeEnum.Alarm: //报警日志14
                        builder.Append(" where LogType=14");
                        break;

                    case LogTypeEnum.OrdinaryMsg: //普通消息15
                        builder.Append(" where LogType=15");
                        break;

                    case LogTypeEnum.PanelMsg: //面板消息17
                        builder.Append(" where LogType=17");
                        break;
                }
            }

            MySqlParameter[] pars =
            {
                new MySqlParameter("?starttime", !start.HasValue ? start :start.Value.Date),
                new MySqlParameter("?endtime", !end.HasValue ? end : end.Value.Date.AddDays(1))
            };
            if (start.HasValue && !end.HasValue)
            {
                builder.Append(" and LogTime>=?starttime");
            }
            if (!start.HasValue && end.HasValue)
            {
                builder.Append(" and LogTime<?endtime");
            }
            if (start.HasValue && end.HasValue)
            {
                builder.Append(" and LogTime BETWEEN ?starttime and ?endtime");
            }
            builder.Append(string.Format(" order by LogTime desc LIMIT {0},{1}", from, count));

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString(),pars);
                if (reader != null)
                {
                    while (reader.Read()) //读到数据转换为Log对象
                    {
                        var log = new LogMessage();
                        log.Id = EvaluationHelper.ObjectToInt(reader["LogID"]);
                        log.LogTime = EvaluationHelper.ObjectToDateTime(reader["LogTime"]);
                        log.LogIp = reader["CurrentIP"].ToString();
                        log.LogMsg = reader["LogMsg"].ToString();
                        log.IsRead = EvaluationHelper.ObjectToBool(reader["IsRead"]);
                        log.FromNo = reader["FromNo"].ToString();
                        log.ToNo = reader["ToNo"].ToString();
                        log.ExtensionNo = reader["ExtensionNo"].ToString();
                        log.LogType = EvaluationHelper.ObjectToInt(reader["LogType"]);
                        log.CallDuration = reader["CallDuration"].ToString();
                        log.OperaterId = reader["UserName"].ToString();
                        log.RecordFilePath = reader["RecordFilePath"].ToString();
                        log.call_uuid = EvaluationHelper.ObjectToString(reader["call_uuid"]);
                        log.Callpanelnum = EvaluationHelper.ObjectToString(reader["PanelNum"]);
                        log.cs_server_ip = EvaluationHelper.ObjectToString(reader["cs_server_ip"]);
                        log.cs_server_port = EvaluationHelper.ObjectToString(reader["cs_server_port"]);

                        logs.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("GetLogByType() error!" + ex);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 获取相应类型日志数量
        /// </summary>
        /// <param name="type">日志类型</param>
        public static int GetLogNumByType(LogTypeEnum type, DateTime? start, DateTime? end)
        {
            var builder = new StringBuilder();
            builder.Append("select COUNT(LogID) from ipvt_logtable");


            MySqlParameter[] pars =
                {
                    new MySqlParameter("?starttime", !start.HasValue ? start :start.Value.Date),
                    new MySqlParameter("?endtime", !end.HasValue ? end : end.Value.Date.AddDays(1))
                };
            if (type != LogTypeEnum.All)
            {
                switch (type)
                {
                    case LogTypeEnum.System: //系统日志10
                        builder.Append(" where LogType=10");
                        break;

                    case LogTypeEnum.Operation: //操作日志11
                        builder.Append(" where LogType=11");
                        break;

                    case LogTypeEnum.Call: //呼叫日志12
                        builder.Append(" where LogType=12");
                        break;

                    case LogTypeEnum.Video: //视频日志13
                        builder.Append(" where LogType=13");
                        break;

                    case LogTypeEnum.Alarm: //报警日志14
                        builder.Append(" where LogType=14");
                        break;

                    case LogTypeEnum.OrdinaryMsg: //普通消息15
                        builder.Append(" where LogType=15");
                        break;

                    case LogTypeEnum.MissedCalled: //未接来电16
                        builder.Append(" where LogType=16");
                        break;

                    case LogTypeEnum.PanelMsg: //面板消息17
                        builder.Append(" where LogType=17");
                        break;
                }
                if (start.HasValue && !end.HasValue)
                {
                    builder.Append(" and LogTime>=?starttime");
                }
                if (!start.HasValue && end.HasValue)
                {
                    builder.Append(" and LogTime<?endtime");
                }
                if (start.HasValue && end.HasValue)
                {
                    builder.Append(" and LogTime BETWEEN ?starttime and ?endtime");
                }
            }
            return EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(builder.ToString(),pars));
        }

        public static void DeleteLogByUuid(string Uuid)
        {
            string sql = @"delete from ipvt_logtable where (call_uuid=?uuid and LogType=16)";
            MySqlParameter[] ps = 
            {
                new MySqlParameter("?uuid",Uuid)
            };
            CustomMySqlHelper.ExecuteNonQuery(sql, ps);
        }
    }
}
