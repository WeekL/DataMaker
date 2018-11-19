using System;
using System.Collections.Generic;
using System.Linq;
using CommonHelperLib;
using DbManagers.Helpers;
using DbManagers.Models;
using IPPhoneModel.ObjectTypes;
using MySql.Data.MySqlClient;

namespace DbManagers
{
    //语音文件管理理
    public class VoiceManager
    {
        /// <summary>
        /// 保存语音文件名到数据库
        /// </summary>
        /// <param name="ss"></param>
        public static bool SavaVoiceFile(string ss)
        {
            if (!string.IsNullOrEmpty(ss))
            {
                List<string> list;
                GetVoiceList(out list);
                if (!list.Contains(ss))
                {
                    string sql = "insert into ipvt_voicefiletable(VoiceFileName) values(?name)";
                    var ps = new MySqlParameter("?name", ss);
                    var num = CustomMySqlHelper.ExecuteNonQuery(sql, ps);
                    if (num > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 保存语音文件名到数据库
        /// </summary>
        /// <param name="ss"></param>
        public static bool SavaVoiceFiles(List<string> ss)
        {
            if (ss != null)
            {
                List<string> list;
                GetVoiceList(out list);
                var sqlmodels = new List<SqlTextModel>();

                for (int i = 0; i < ss.Count; i++)
                {
                    string name = ss[i].Split(';').ToList()[0];
                    string time = ss[i].Split(';').ToList()[1];
                    if (!list.Contains(name))
                    {
                        string sql = "insert into ipvt_voicefiletable(VoiceFileName,Description) values(?name,?filetime)";
                        var ps = new MySqlParameter[2];
                        ps[0] = new MySqlParameter("?name", name);
                        ps[1] = new MySqlParameter("?filetime", time);

                        var model = new SqlTextModel {SqlString = sql, MySqlParams = ps};
                        sqlmodels.Add(model);
                    }
                }
                return CustomMySqlHelper.ExecuteSqlList(sqlmodels);
            }
            return false;
        }

        /// <summary>
        /// 保存广播语音文件名到数据库
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="phoneId">中心分机id</param>
        public static bool SavaBroadcastVoices(List<string> ss,int phoneId)
        {
            if (ss != null && ss.Count>0)
            {
                Dictionary<int, string> dictionary = GetVoices(phoneId);
                var sqlmodels = new List<SqlTextModel>();

                foreach (int id in dictionary.Keys)
                {
                    if (!ss.Contains(dictionary[id]))
                    {
                        DeleteVoice(id);
                    }
                }

                for (int i = 0; i < ss.Count; i++)
                {
                    if (!dictionary.Values.Contains(ss[i]))
                    {
                        string sql =
                            "insert into ipvt_voicefiletable(VoiceFileName,VoiceType,PhoneNumber) values(?name,?type,?num)";
                        var ps = new MySqlParameter[3];
                        ps[0] = new MySqlParameter("?name", ss[i]);
                        ps[1] = new MySqlParameter("?type", 1);
                        ps[2] = new MySqlParameter("?num", phoneId);

                        var model = new SqlTextModel { SqlString = sql, MySqlParams = ps };
                        sqlmodels.Add(model);
                    }
                }
                return CustomMySqlHelper.ExecuteSqlList(sqlmodels);
            }
            return false;
        }
        //删除指定语音文件
        private static void DeleteVoice(int id)
        {
            if (id>0)
            {
                string sql =
                    "delete from ipvt_voicefiletable where ID=?id";
                CustomMySqlHelper.ExecuteNonQuery(sql, new MySqlParameter("?id", id));
            }
        }

        // 获取语音文件
        private static Dictionary<int,string> GetVoices(int number)
        {
            var list = new Dictionary<int,string>();
            if (number>0)
            {
                string sql = "SELECT ID,VoiceFileName from ipvt_voicefiletable where PhoneNumber=?num and VoiceType=1";
                MySqlDataReader reader = null;
                try
                {
                    reader = CustomMySqlHelper.ExecuteDataReader(sql, new MySqlParameter("?num", number));
                    while (reader.Read())
                    {
                        int id = EvaluationHelper.ObjectToInt(reader["ID"]);
                        if (!list.ContainsKey(id))
                        {
                            list.Add(id, reader["VoiceFileName"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog(ex.ToString());
                }
                finally
                {
                    if (reader != null) reader.Close(); //读取完关闭reader对象
                }
            }
            return list;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool DeleteFiles(List<string> list)
        {
            if (list != null)
            {
                var sqlmodels = new List<SqlTextModel>();

                for (int i = 0; i < list.Count; i++)
                {
                    string sql = "delete from ipvt_voicefiletable where VoiceFileName=?name and VoiceType=0";
                    var ps = new MySqlParameter("?name", list[i]);

                    var model = new SqlTextModel {SqlString = sql, MySqlParams = new[] {ps}};
                    sqlmodels.Add(model);
                }
                return CustomMySqlHelper.ExecuteSqlList(sqlmodels);
            }
            return false;
        }

        /// <summary>
        /// 获取语音文件名称集合
        /// </summary>
        public static void GetVoiceList(out List<string> list)
        {
            list = new List<string>();
            string sql = "SELECT VoiceFileName from ipvt_voicefiletable where VoiceType=0";
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                while (reader.Read())
                {
                    list.Add(reader["VoiceFileName"].ToString());
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.ToString());
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 获取语音文件名称集合
        /// </summary>
        public static void GetVoiceList(out List<PathAndNameModel> list)
        {
            list = new List<PathAndNameModel>();
            string sql = "SELECT * from ipvt_voicefiletable where VoiceType=0";
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                while (reader.Read())
                {
                    list.Add(new PathAndNameModel() { Name = reader["VoiceFileName"].ToString(), FileTime = reader["Description"].ToString() });
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.ToString());
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 获取语音文件名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetVoiceNameById(int id)
        {
            if (id < 0) return string.Empty;
            string sql = "SELECT VoiceFileName from ipvt_voicefiletable where ID=?id";
            
            var result = CustomMySqlHelper.ExecuteScalar(sql, new MySqlParameter("?id", id));
            return EvaluationHelper.ObjectToString(result);
        }

        /// <summary>
        /// 更新语音文件表对应的分机id
        /// </summary>
        /// <param name="oldnum"></param>
        /// <param name="newnum"></param>
        public static void UpdatePhoneNuber(string oldnum,string newnum)
        {
            try
            {
                string s1 = "select ExtensionID from ipvt_extensionmessagetable where ExtensionNo=?oldnum";
                string s2 = "select ExtensionID from ipvt_extensionmessagetable where ExtensionNo=?newnum";

                int oldid = EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(s1, new MySqlParameter("?oldnum", string.IsNullOrEmpty(oldnum) ? 0 : Convert.ToInt32(oldnum))));
                int newid = EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(s2, new MySqlParameter("?newnum", string.IsNullOrEmpty(newnum) ? 0 : Convert.ToInt32(newnum))));

                string sql = "update ipvt_voicefiletable set PhoneNumber=?newid where PhoneNumber=?oldid";
                CustomMySqlHelper.ExecuteNonQuery(sql, new[] { new MySqlParameter("?oldid", oldid), new MySqlParameter("?newid", newid) });
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error! "+ex.Message);
            }
        }

        public static bool JudgeVoiceExist(string name)
        {
            string sql =
                string.Format(@"select count(*) as count from ipvt_voicefiletable where VoiceFileName={0} and VoiceType=0",
                    name);
            MySqlDataReader readercount = CustomMySqlHelper.ExecuteDataReader(sql);
            try
            {
                int SumCount = 0;
                while (readercount.Read())
                {
                    SumCount = EvaluationHelper.ObjectToInt(readercount["count"]);
                }
                if(SumCount>0)
                    return true;
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetCallLogCount!" + ex.Message);
            }
            return false;
        }
    }
}
