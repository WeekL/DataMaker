 using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
 using System.IO;
 using CommonHelperLib;
using DbManagers.Helpers;
using DbManagers.Models;
using IPPhoneModel.ObjectTypes;
using MySql.Data.MySqlClient;

namespace DbManagers
{
    //服务器高级设置数据库处理
    public class AdvancedSetupManager:DbManagerBase
    {
        /// <summary>
        /// 保存呼叫转移设置
        /// </summary>
        /// <param name="transfers">转移数据对象集合</param>
        public static bool SaveTransfer(ObservableCollection<Transfer> transfers)
        {
            return SaveToDb(transfers);
        }

        /// <summary>
        /// 保存紧急呼叫转移设置（写入数据库、本地xml文件：conf\directory\default\1.xml）
        /// </summary>
        /// <param name="transfers">转移数据对象集合</param>
        public static bool SetupEmergencyTransfer(ObservableCollection<Transfer> transfers)
        {
            SaveTranSetup(transfers, "1002");
            return SaveTransfer(transfers);
        }
        /// <summary>
        /// 保存业务呼叫转移设置（写入数据库、本地xml文件：conf\directory\default\2.xml）
        /// </summary>
        /// <param name="transfers"></param>
        public static bool SetupOperationTransfer(ObservableCollection<Transfer> transfers)
        {
            SaveTranSetup(transfers, "1001");
            return SaveTransfer(transfers);
        }

        private static void SaveTranSetup(ObservableCollection<Transfer> transfers,string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string one = string.Empty;
                string two = string.Empty;
                string three = string.Empty;

                foreach (Transfer transfer in transfers)
                {
                    if (transfer.Level == 1)
                    {
                        one = transfer.TransferExtensionNo;
                    }
                    if (transfer.Level == 2)
                    {
                        two = transfer.TransferExtensionNo;
                    }
                    if (transfer.Level == 3)
                    {
                        three = transfer.TransferExtensionNo;
                    }
                }
                //string sql = "UPDATE ipvt_transetuptable set StairTran=?one,SecondTran=?two,ThreeTran=?three,IsEnable=1 where ID=?id";
                string sql =
                    "UPDATE ipvt_transetuptable set StairTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?one)," +
                    "SecondTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?two)," +
                    "ThreeTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?three),IsEnable=1 where ID=?id";
                
                var ps = new MySqlParameter[4];
                ps[0] = new MySqlParameter("?one", string.IsNullOrEmpty(one) ? 0 : Convert.ToInt32(one));
                ps[1] = new MySqlParameter("?two", string.IsNullOrEmpty(two) ? 0 : Convert.ToInt32(two));
                ps[2] = new MySqlParameter("?three", string.IsNullOrEmpty(three) ? 0 : Convert.ToInt32(three));
                ps[3] = new MySqlParameter("?id", id);
                CustomMySqlHelper.ExecuteNonQuery(sql, ps);
            }
        }

        /// <summary>
        /// 自动写转移是更新转移设置表
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="id"></param>
        public static void SetupTranser(Transfer tran,string id)
        {
            if (tran != null && !string.IsNullOrEmpty(id))
            {
                string sql = "UPDATE ipvt_transetuptable set StairTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?num),IsEnable=1 where ID=?id";

                if (tran.Level == 2)
                {
                    sql = "UPDATE ipvt_transetuptable set SecondTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?num),IsEnable=1 where ID=?id";
                }
                if (tran.Level == 3)
                {
                    sql = "UPDATE ipvt_transetuptable set ThreeTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?num),IsEnable=1 where ID=?id";
                }
                var ps = new MySqlParameter[2];
                ps[0] = new MySqlParameter("?id", id);
                ps[1] = new MySqlParameter("?num", string.IsNullOrEmpty(tran.TransferExtensionNo) ? 0 : Convert.ToInt32(tran.TransferExtensionNo));
                CustomMySqlHelper.ExecuteNonQuery(sql, ps);
            }
        }

        /// <summary>
        /// 修改分时段转移时更新数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool SavePartingTran(List<PartingModel> list)
        {
            var sqlList = new List<SqlTextModel>();
            int index = 0;
            if (list.Count <= 0)
            {
                string sql = "UPDATE ipvt_transetuptable set IsEnable=0 where ID>=1003";
                sqlList.Add(new SqlTextModel(){SqlString = sql});
            }
            else if (list.Count <= 1)
            {
                string sql = "UPDATE ipvt_transetuptable set IsEnable=0 where ID>=1005";
                sqlList.Add(new SqlTextModel() { SqlString = sql });
            }
            else if (list.Count <= 2)
            {
                string sql = "UPDATE ipvt_transetuptable set IsEnable=0 where ID>=1007";
                sqlList.Add(new SqlTextModel() { SqlString = sql });
            }

            foreach (PartingModel model in list)
            {
                if (index >= 3)
                {
                    break;
                }
                int start = model.StartTime;
                int end = model.EndTime;
                if (model.BusinessList.Count >= 3)
                {
                    string sql = "UPDATE ipvt_transetuptable set StartTime=?start,EndTime=?end," +
                                 "StairTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?one)," +
                                 "SecondTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?two)," +
                                 "ThreeTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?three)," +
                                 "IsEnable=1 where ID=?id";
                    var ps = new MySqlParameter[6];
                    ps[0] = new MySqlParameter("?one",string.IsNullOrEmpty(model.BusinessList[0].TransferExtensionNo) ? 0 : Convert.ToInt32(model.BusinessList[0].TransferExtensionNo));
                    ps[1] = new MySqlParameter("?two",string.IsNullOrEmpty(model.BusinessList[1].TransferExtensionNo) ? 0 : Convert.ToInt32(model.BusinessList[1].TransferExtensionNo));
                    ps[2] = new MySqlParameter("?three",string.IsNullOrEmpty(model.BusinessList[2].TransferExtensionNo) ? 0 : Convert.ToInt32(model.BusinessList[2].TransferExtensionNo));
                    ps[3] = new MySqlParameter("?start", start);
                    ps[4] = new MySqlParameter("?end", end);
                    ps[5] = new MySqlParameter("?id", 1003 + index * 2);

                    sqlList.Add(new SqlTextModel() { SqlString = sql, MySqlParams = ps });
                }
                if (model.UrgencyList.Count >= 3)
                {
                    string sql = "UPDATE ipvt_transetuptable set StartTime=?start,EndTime=?end," +
                                 "StairTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?one)," +
                                 "SecondTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?two)," +
                                 "ThreeTran=(select ExtensionID from ipvt_extensionmessagetable where ExtensionNO=?three)," +
                                 "IsEnable=1 where ID=?id";
                    var ps = new MySqlParameter[6];
                    ps[0] = new MySqlParameter("?one",string.IsNullOrEmpty(model.UrgencyList[0].TransferExtensionNo) ? 0 : Convert.ToInt32(model.UrgencyList[0].TransferExtensionNo));
                    ps[1] = new MySqlParameter("?two",string.IsNullOrEmpty(model.UrgencyList[1].TransferExtensionNo) ? 0 : Convert.ToInt32(model.UrgencyList[1].TransferExtensionNo));
                    ps[2] = new MySqlParameter("?three",string.IsNullOrEmpty(model.UrgencyList[2].TransferExtensionNo) ? 0 : Convert.ToInt32(model.UrgencyList[2].TransferExtensionNo));
                    ps[3] = new MySqlParameter("?start", start);
                    ps[4] = new MySqlParameter("?end", end);
                    ps[5] = new MySqlParameter("?id", 1004 + index * 2);

                    sqlList.Add(new SqlTextModel() { SqlString = sql, MySqlParams = ps });
                }
                index++;
            }
            return CustomMySqlHelper.ExecuteSqlList(sqlList);
        }

        /// <summary>
        /// 生成删除语句
        /// </summary>
        /// <param name="tran">转移对象</param>
        public static SqlTextModel CreateSqlTextModel(Transfer tran)
        {
            SqlTextModel model = null;
            if (tran != null)
            {
                if (tran.Id != 0)
                {
                    string sqlStr = "delete from ipvt_transferinfotable where TransferID=?id";
                    var parameteres = new MySqlParameter[1];
                    parameteres[0] = new MySqlParameter("?id", tran.Id);

                    if (tran.ExtensionNo == "2")
                    {
                        model = new SqlTextModel { SqlString = sqlStr, MySqlParams = parameteres };
                    }
                    else if (tran.ExtensionNo == "1")
                    {
                        model = new SqlTextModel { SqlString = sqlStr, MySqlParams = parameteres };
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 存入数据库
        /// </summary>
        /// <param name="transfers">转移数据对象集合</param>
        /// <returns>true：成功  falase：失败</returns>
        private static bool SaveToDb(IEnumerable<Transfer> transfers)
        {
            var sqlList = new List<SqlTextModel>();

            foreach (Transfer tran in transfers)
            {
                string sqlStr =
                    "Update ipvt_transferinfotable SET " +
                    "TransferExtensionNo=?teno WHERE ExtensionNO=?no and TransferLevel=?level";
                var parameteres = new MySqlParameter[3];
                parameteres[0] = new MySqlParameter("?teno", tran.TransferExtensionNo);
                parameteres[1] = new MySqlParameter("?no", tran.ExtensionNo);
                parameteres[2] = new MySqlParameter("?level", tran.Level);

                sqlList.Add(new SqlTextModel {SqlString = sqlStr, MySqlParams = parameteres});
            }

            return CustomMySqlHelper.ExecuteSqlList(sqlList);
        }

        /// <summary>
        /// 清理数据库
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool ClearDateBase(User user)
        {
            if (UserManager.IsUserExist(user.Name, user.Password))
            {
                var sqlList = new List<SqlTextModel>();
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_deviceinfotable" });
                sqlList.Add(new SqlTextModel { SqlString = "UPDATE ipvt_extensionmessagetable SET CurrentState = 0,DeviceIP = NULL,StateID = NULL,Date = NULL,Time = NULL,PhoneState=0" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM	ipvt_panelinfotable" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM	ipvt_transferinfotable" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_logtable" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_talkinginfotable" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_channelsinfotable" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_callrecordtable" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_userregmessagetable" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_staffmessagetable" });

                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_cascadingtable" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_groupinfotable WHERE GroupID>0" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_jurgrouptable WHERE GroupID>3" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_jurisdiction_grouptable WHERE GroupID>3" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_voicefiletable" });
                sqlList.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_videolinkagetable" });
                sqlList.Add(new SqlTextModel { SqlString = "UPDATE ipvt_transetuptable SET StartTime=NULL,EndTime=NULL,StairTran=NULL,SecondTran=NULL,ThreeTran=NULL,IsEnable=0" });
                
                return CustomMySqlHelper.ExecuteSqlList(sqlList);
            }
            return false;
        }

        #region 输入/输出端口报警
        /// <summary>
        /// 获取输入/输出端口报警设置原型
        /// </summary>
        /// <param name="models"></param>
        public static void GetPortAlarmModels(out List<CommonModel> models)
        {
            models = new List<CommonModel>();
            string sql = "select AlarmTypeID,AlarmType from ipvt_alarmtypetable";

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                while (reader.Read())
                {
                    CommonModel model = new CommonModel();
                    model.Id = EvaluationHelper.ObjectToInt(reader["AlarmTypeID"]);
                    model.Content = EvaluationHelper.ObjectToString(reader["AlarmType"]);

                    models.Add(model);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in GetPortAlarmModels()" + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static bool SavePortAlarmModels(ObservableCollection<CommonModel> models)
        {
            List<SqlTextModel> list = new List<SqlTextModel>();
            if (models != null)
            {
                for (int i = 0; i < models.Count; i++)
                {
                    SqlTextModel model = new SqlTextModel();
                    model.SqlString = "update ipvt_alarmtypetable set AlarmType=?type where AlarmTypeID=?id";
                    model.MySqlParams=new[]
                    {
                        new MySqlParameter("?type", models[i].Content),
                        new MySqlParameter("?id", models[i].Id)
                    };

                    list.Add(model);
                }
            }
            if (list.Count > 0)
            {
                return CustomMySqlHelper.ExecuteSqlList(list);
            }

            return false;
        }

        #endregion

        /// <summary>
        /// 改变区号：调用存储过程清理旧分机/设备表，写新的分机号数据
        /// </summary>
        /// <param name="areacode">区号</param>
        /// <param name="oldareacode">区号</param>
        /// <returns></returns>
        public static bool UpdateAreacode(int areacode, int oldareacode)
        {
            List<SqlTextModel> list = new List<SqlTextModel>();
            int oldmin = oldareacode * 1000;
            int oldmax = oldareacode * 1000 + 1000;
            int min = areacode * 1000;
            if (IsTh)
            {
                //6位分机号，写3000个
                oldmin = oldareacode * 10000;
                oldmax = oldmin + 3000;
                min = areacode * 10000;
            }

            for (int i = oldmin; i < oldmax; i++)
            {
                string sql = "UPDATE ipvt_extensionmessagetable set ExtensionNO=?extno where ExtensionNO=?old";
                var ps = new MySqlParameter[2];
                ps[0] = new MySqlParameter("?extno", min);
                ps[1] = new MySqlParameter("?old", i);
                min++;
                list.Add(new SqlTextModel() {SqlString = sql, MySqlParams = ps});
            }
            //list.Add(new SqlTextModel { SqlString = "UPDATE ipvt_transetuptable SET StartTime=NULL,EndTime=NULL,StairTran=NULL,SecondTran=NULL,ThreeTran=NULL,IsEnable=0" });
            //list.Add(new SqlTextModel { SqlString = "DELETE FROM ipvt_transferinfotable" });  

            return CustomMySqlHelper.ExecuteSqlList(list);
        }

        /// <summary>
        /// 初始区号，写分机号
        /// </summary>
        /// <param name="areacode">区号</param>
        /// <returns></returns>
        public static bool WriteAreacode(int areacode)
        {
            int min = areacode * 1000;
            int max = areacode * 1000 + 1000;
            if (IsTh)
            {           
                //6位分机号，写3000个
                min = areacode * 10000;
                max = min + 3000;
            }

            //var result = CustomMySqlHelper.ExecuteNonQuery(CustomMySqlHelper.ConnectionString, 
            //    CommandType.StoredProcedure,"pro_ipvt_extensionmessagetable_do",
            //    new[] {new MySqlParameter("min_no", min), new MySqlParameter("max_no", max)});

            List<SqlTextModel> list = new List<SqlTextModel>();

            list.Add(new SqlTextModel() { SqlString = "DELETE FROM ipvt_deviceinfotable;" });
            list.Add(new SqlTextModel() { SqlString = "DELETE FROM ipvt_extensionmessagetable;" });

            for (int i = min; i < max; i++)
            {
                list.Add(new SqlTextModel() { SqlString = string.Format("INSERT INTO ipvt_extensionmessagetable(ExtensionNO) VALUES({0});", i) });
            }

            return CustomMySqlHelper.ExecuteSqlList(list);
        }

        /// <summary>
        /// 判断是否已存在分机号
        /// </summary>
        /// <returns></returns>
        public static bool IsHaveExtenNo()
        {
            string sql = "select count(*) from ipvt_extensionmessagetable";

            int result = EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(sql));
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 执行.sql文件
        /// </summary>
        /// <param name="path">.sql文件</param>
        public static bool ExcuteSqlFile(string path)
        {
            if (File.Exists(path))
            {
                FileInfo file = new FileInfo(path);
                string sql = file.OpenText().ReadToEnd();

                int result = CustomMySqlHelper.ExecuteNonQuery(sql);
                if (result >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取SDK配置信息
        /// </summary>
        /// <param name="models"></param>
        public static void GetSdkModels(out List<SdkSetupModel> models)
        {
            models=new List<SdkSetupModel>();
            string sql = "select SdkID,SdkIP,SdkPort from ipvt_sdkinfotable";

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        models.Add(new SdkSetupModel()
                        {
                            Id=EvaluationHelper.ObjectToInt(reader["SdkID"]),
                            Ip = EvaluationHelper.ObjectToString(reader["SdkIP"]),
                            Port = EvaluationHelper.ObjectToString(reader["SdkPort"])
                        });
                    }
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
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int InsertSdkSetupModel(SdkSetupModel model)
        {
            if (model == null) return -1;
            string sql = "insert into ipvt_sdkinfotable(SdkIP,SdkPort) values(?ip,?port);SELECT @@IDENTITY;";
            return EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(sql,
                new[] {new MySqlParameter("?ip", model.Ip), new MySqlParameter("?port", model.Port)}));
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int UpdateSdkSetupModel(SdkSetupModel model)
        {
            if (model != null && model.Id > 0)
            {
                string sql = "update ipvt_sdkinfotable set SdkIP=?ip,SdkPort=?port where SdkID=?id";
                return CustomMySqlHelper.ExecuteNonQuery(sql,
                    new[]
                {
                    new MySqlParameter("?ip", model.Ip), 
                    new MySqlParameter("?port", model.Port),
                    new MySqlParameter("?id", model.Id)
                });
            }
            return -1;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int DeleteSdkSetupModel(SdkSetupModel model)
        {
            if (model != null && model.Id > 0)
            {
            string sql = "delete from ipvt_sdkinfotable where SdkID=?id";
            return CustomMySqlHelper.ExecuteNonQuery(sql,
                new[] { new MySqlParameter("?id", model.Id)});

            }
            return -1;
        }
    }
}

