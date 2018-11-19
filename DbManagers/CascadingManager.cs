using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using CommonHelperLib;
using DbManagers.Helpers;
using IPPhoneModel.ObjectTypes;
using MySql.Data.MySqlClient;

namespace DbManagers
{
    /// <summary>
    /// 级联设置处理
    /// </summary>
    public class CascadingManager:DbManagerBase
    {
        private static string _dbPath = Path.Combine(RootPath, "db/call_limit.db");

        /// <summary>
        /// 获取级联集合
        /// </summary>
        /// <returns></returns>
        public static void GetCascadings(out List<Cascading> list)
        {
            list = new List<Cascading>();
            string mysql = "select CascadingID,Data_key,Data,Remark from ipvt_cascadingtable";
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(mysql);
                while (reader != null && reader.Read())
                {
                    var model = new Cascading();
                    model.Id = EvaluationHelper.ObjectToInt(reader["CascadingID"]);
                    model.Key = reader["Data_key"].ToString();
                    model.Ip = reader["Data"].ToString();
                    model.Remark = reader["Remark"].ToString();

                    var con = SqLiteHelper.Open(_dbPath);

                    string sql =
                        string.Format(
                            "select RowID from db_data where realm='hy_extern_domain' and data_key='{0}' and data='{1}'",
                            model.Key, model.Ip);
                    model.RowId = EvaluationHelper.ObjectToInt(SqLiteHelper.ExecuteScalar(con, sql));

                    list.Add(model);

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.ToString());
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns>成功返回true</returns>
        public static bool InsertCascading(Cascading model)
        {
            if (model != null)
            {
                var con = SqLiteHelper.Open(_dbPath);
                try
                {
                    string mysqlString =
                        "insert into ipvt_cascadingtable(Data_key,Data,Remark) values(?key,?data,?remark)";
                    var ps = new MySqlParameter[3];
                    ps[0] = new MySqlParameter("?key", model.Key);
                    ps[1] = new MySqlParameter("?data", model.Ip);
                    ps[2] = new MySqlParameter("?remark", model.Remark);

                    CustomMySqlHelper.ExecuteNonQuery(mysqlString, ps);//插入

                    string sqliteString =
                        string.Format("insert into db_data(realm,data_key,data) values('hy_extern_domain','{0}','{1}')",
                            model.Key, model.Ip);
                    SqLiteHelper.ExecuteNonquery(con, sqliteString);//SQLite插入

                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog(ex.ToString());
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns>成功返回true</returns>
        public static bool UpdataCascading(Cascading model)
        {
            if (model != null && model.Id != 0)
            {
                var con = SqLiteHelper.Open(_dbPath);
                try
                {
                    string mysqlString =
                        "update ipvt_cascadingtable set Data_key=?key,Data=?data,Remark=?remark where CascadingID=?id";
                    var ps = new MySqlParameter[4];
                    ps[0] = new MySqlParameter("?key", model.Key);
                    ps[1] = new MySqlParameter("?data", model.Ip);
                    ps[2] = new MySqlParameter("?remark", model.Remark);
                    ps[3] = new MySqlParameter("?id", model.Id);

                    CustomMySqlHelper.ExecuteNonQuery(mysqlString, ps); //更新

                    string sqliteString =
                        string.Format("update db_data set data_key='{0}',data='{1}' where RowID={2}",
                            model.Key, model.Ip,model.RowId);
                    SqLiteHelper.ExecuteNonquery(con, sqliteString); //SQLite更新

                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog(ex.ToString());
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns>成功返回true</returns>
        public static bool DeleteCascading(Cascading model)
        {
            if (model != null)
            {
                var con = SqLiteHelper.Open(_dbPath);
                try
                {
                    string mysqlString = "delete from ipvt_cascadingtable where CascadingID=?id";
                    var param = new MySqlParameter("?id", model.Id);
                    CustomMySqlHelper.ExecuteNonQuery(mysqlString, param);

                    string sqliteSql = string.Format("delete from db_data where RowID={0}", model.RowId);
                    SqLiteHelper.ExecuteNonquery(con, sqliteSql);

                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog(ex.ToString());
                }
                finally
                {
                    if (con != null) con.Close();
                }
            }
            return false;
        }


        /// <summary>
        /// 判断core.db是否异常
        /// </summary>
        public static void JudeCoreDb()
        {
            string path = Path.Combine(@"C:\Users\Administrator\Desktop\db", "core.db");
            //string path = Path.Combine(RootPath, "db", "core.db");
            if (File.Exists(path))
            {
                SQLiteConnection conn;
                bool b = SqLiteHelper.TryOpen(path,out conn);
                if (b)
                {
                    string sql = "select * from registrations order by RowID desc limit 1";
                    object obj = SqLiteHelper.ExecuteScalar(conn, sql);
                    if (obj == null)
                    {
                        string s = "delete from registrations";
                        //string s = "delete from registrations where RowID=(select RowID from registrations order by RowID desc limit 1)";
                        SqLiteHelper.ExecuteNonquery(conn, s);
                    }

                    sql = "select * from complete order by RowID desc limit 1";
                    obj = SqLiteHelper.ExecuteScalar(conn, sql);
                    if (obj == null)
                    {
                        string s = "delete from complete";
                        //string s = "delete from registrations where RowID=(select RowID from registrations order by RowID desc limit 1)";
                        SqLiteHelper.ExecuteNonquery(conn, s);
                    }

                    sql = "select * from interfaces order by RowID desc limit 1";
                    obj = SqLiteHelper.ExecuteScalar(conn, sql);
                    if (obj == null)
                    {
                        string s = "delete from interfaces";
                        //string s = "delete from registrations where RowID=(select RowID from registrations order by RowID desc limit 1)";
                        SqLiteHelper.ExecuteNonquery(conn, s);
                    }
                }
                else
                {
                    File.Delete(path);
                }
            }
        }
    }
}
