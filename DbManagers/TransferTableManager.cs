using System;
using System.Collections.Generic;
using System.Data;
using CommonHelperLib;
using DbManagers.Helpers;
using IPPhoneModel.ObjectTypes;
using MySql.Data.MySqlClient;

namespace DbManagers
{
    /// <summary>
    /// 转移列表管理器
    /// </summary>
    public static class TransferTableManager
    {
        /// <summary>
        /// 获取指定分机的呼叫转移列表
        /// </summary>
        /// <param name="transfers"></param>
        /// <param name="exten"></param>
        /// <returns></returns>
        public static void GetTransferTableByExtension(out List<Transfer> transfers, Extension exten)
        {
            transfers = new List<Transfer>();
            if (exten != null)
            {
                DataSet set;
                string sql =
                    "SELECT TransferID,TransferLevel,TransferExtensionNo," +
                    "TransferLevel FROM ipvt_transferinfotable WHERE ExtensionNo=" +
                    exten.Number + " ORDER BY TransferLevel ASC";
                try
                {
                    set = CustomMySqlHelper.ExecuteDataSet(sql);
                    DataRowCollection rows = set.Tables[0].Rows;
                    if (rows != null)
                    {
                        foreach (DataRow row in rows)
                        {
                            Transfer tran = new Transfer();
                            tran.Id = EvaluationHelper.ObjectToInt(row["TransferID"]);
                            tran.Level = EvaluationHelper.ObjectToInt(row["TransferLevel"]);
                            tran.ExtensionNo = exten.Number;
                            tran.TransferExtensionNo = row["TransferExtensionNo"].ToString();
                            transfers.Add(tran);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 获取最大级别的转移信息
        /// </summary>
        /// <param name="extenNo">主分机</param>
        /// <param name="transferExtenNo">转移分机</param>
        /// <returns></returns>
        public static Transfer GetMaxLevelTransfer(string extenNo, string transferExtenNo)
        {
            Transfer transfer = null;
            DataSet set;
            string sql =
                "SELECT TransferID,TransferExtensionNo," +
                "TransferLevel FROM ipvt_transferinfotable WHERE ExtensionNo=?en " +
                "AND TransferExtensionNo=?ten " +
                "ORDER BY TransferLevel DESC LIMIT 1";
            var param = new MySqlParameter[2];
            param[0] = new MySqlParameter("?en", extenNo);
            param[1] = new MySqlParameter("?ten", transferExtenNo);

            try
            {
                set = CustomMySqlHelper.ExecuteDataSet(sql, param);
                DataRowCollection rows = set.Tables[0].Rows;
                if (rows != null)
                {
                    if (rows.Count == 1)
                    {
                        transfer = new Transfer();
                        transfer.Id = EvaluationHelper.ObjectToInt(rows[0]["TransferID"]);
                        transfer.Level = EvaluationHelper.ObjectToInt(rows[0]["TransferLevel"]);
                        transfer.ExtensionNo = extenNo;
                        transfer.TransferExtensionNo = rows[0]["TransferExtensionNo"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.ToString());
            }
            return transfer;
        }

        /// <summary>
        /// 替换转移分机号
        /// </summary>
        /// <param name="oldNo"></param>
        /// <param name="newNo"></param>
        public static void ReplaceTransferNo(string oldNo,string newNo)
        {
            string sql = "Update ipvt_transferinfotable set " +
                         "TransferExtensionNo=?newNo " +
                         "where TransferExtensionNo=?oldNo";

            var ps = new MySqlParameter[2];
            ps[0] = new MySqlParameter("?newNo", newNo);
            ps[1] = new MySqlParameter("?oldNo", oldNo);

            CustomMySqlHelper.ExecuteNonQuery(sql, ps);
        }

        /// <summary>
        /// 获取转移设置信息
        /// </summary>
        /// <param name="list"></param>
        public static void GetTransInfo(out List<TranSetupInfo> list)
        {
            list = new List<TranSetupInfo>();

            string sql = "select ID,TranType,StartTime,EndTime," +
                         "(SELECT ExtensionNO FROM ipvt_extensionmessagetable where ExtensionID=StairTran) as Stair," +
                         "(SELECT ExtensionNO FROM ipvt_extensionmessagetable where ExtensionID=SecondTran) as Second," +
                         "(SELECT ExtensionNO FROM ipvt_extensionmessagetable where ExtensionID=ThreeTran) as Three," +
                         "IsEnable,Description from ipvt_transetuptable";
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                while (reader.Read())
                {
                    var model = new TranSetupInfo();
                    model.Id = EvaluationHelper.ObjectToInt(reader["ID"]);
                    model.TranType = EvaluationHelper.ObjectToInt(reader["TranType"]);
                    model.StartTime = EvaluationHelper.ObjectToInt(reader["StartTime"]);
                    model.EndTime = EvaluationHelper.ObjectToInt(reader["EndTime"]);
                    model.IsEnable = EvaluationHelper.ObjectToInt(reader["IsEnable"]);
                    model.StairTran = EvaluationHelper.ObjectToString(reader["Stair"]);
                    model.SecondTran = EvaluationHelper.ObjectToString(reader["Second"]);
                    model.ThreeTran = EvaluationHelper.ObjectToString(reader["Three"]);
                    model.Description = EvaluationHelper.ObjectToString(reader["Description"]);
                    list.Add(model);
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
    }
}
