using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommonHelperLib;
using DbManagers.Helpers;
using DbManagers.Models;
using IPPhoneModel;
using IPPhoneModel.ObjectTypes;
using MySql.Data.MySqlClient;
using System.Data;

namespace DbManagers
{
    /// <summary>
    /// 组织机构处理
    /// </summary>
    public class GroupManager
    {
        /// <summary>
        /// 更新组信息
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public static bool UpdateGroups(List<DeviceGroup> groups)
        {
            bool result = false;
            if (groups != null)
            {
                var list = new List<SqlTextModel>();
                foreach (DeviceGroup group in groups)
                {
                    string sql = "update ipvt_groupinfotable set ParentID=?pid,GroupName=?name,GroupLevel=?level" +
                                 " where GroupID=?id";

                    var ps = new[] 
                { 
                    new MySqlParameter("?pid", group.ParentId), 
                    new MySqlParameter("?name", group.Name) ,
                    new MySqlParameter("?level", group.Level) ,
                    new MySqlParameter("?id", group.Id) 
                };

                    list.Add(new SqlTextModel() { SqlString = sql, MySqlParams = ps });
                }
                result = CustomMySqlHelper.ExecuteSqlList(list);

            }

            return result;
        }

        /// <summary>
        /// 插入分组
        /// </summary>
        /// <param name="group">组对象</param>
        /// <returns>自增的id，失败则返回0</returns>
        public static int InsertGroup(DeviceGroup group)
        {
            int result = 0;

            if (group != null)
            {
                string sql = "insert into ipvt_groupinfotable(ParentID,GroupName,GroupLevel) values(?pid,?name,?level);" +
                             "SELECT @@IDENTITY";

                var ps = new[] 
                { 
                    new MySqlParameter("?pid", group.ParentId), 
                    new MySqlParameter("?name", group.Name) ,
                    new MySqlParameter("?level", group.Level) 
                };

                result = EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(sql, ps));
            }
            return result;
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="group">组对象</param>
        /// <returns>自增的id，失败则返回0</returns>
        public static int DeleteGroup(DeviceGroup group)
        {
            string sql = "delete from ipvt_groupinfotable where GroupID=?id";
            if (group != null)
            {
                var ps = new[]
                {
                    new MySqlParameter("?pid", group.ParentId),
                    new MySqlParameter("?id", group.Id)
                };
                return CustomMySqlHelper.ExecuteNonQuery(sql, ps);
            }
            return 0;
        }

        /// <summary>
        /// 根据组id获取组信息
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static DeviceGroup GetGroup(int gid)
        {
            DeviceGroup group = null;
            string sql = "SELECT GroupID,GroupName,ParentID,GroupLevel FROM ipvt_groupinfotable where GroupID=?gid";
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql, new MySqlParameter("?gid", gid));
                while (reader.Read())
                {
                    group = new DeviceGroup();
                    group.Id = EvaluationHelper.ObjectToInt(reader["GroupID"]);
                    group.Name = Convert.ToString(reader["GroupName"]);
                    group.ParentId = EvaluationHelper.ObjectToInt(reader["ParentID"]);
                    group.Level = EvaluationHelper.ObjectToInt(reader["GroupLevel"]);
                    break;
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
            return group;
        }

        /// <summary>
        /// 获取设备分组信息
        /// </summary>
        /// <returns></returns>
        public static void GetDeviceGroups(out List<DeviceGroup> groups)
        {
            groups = new List<DeviceGroup>();
            DataSet set;
            string sql = "SELECT GroupID,GroupName,ParentID,GroupLevel FROM ipvt_groupinfotable";


            try
            {
                set = CustomMySqlHelper.ExecuteDataSet(sql);
                if (set.Tables.Count > 0)
                {
                    DataRowCollection rows = set.Tables[0].Rows;
                    var grps = new DeviceGroup[rows.Count];
                    int i = 0;

                    foreach (DataRow row in rows)
                    {
                        var grp = new DeviceGroup();
                        grp.Id = EvaluationHelper.ObjectToInt(row["GroupID"]);
                        grp.Name = Convert.ToString(row["GroupName"]);
                        grp.ParentId = EvaluationHelper.ObjectToInt(row["ParentID"]);
                        grp.Level = EvaluationHelper.ObjectToInt(row["GroupLevel"]);
                        grps[i++] = grp;
                    }

                    var roots = from r in grps where r.Id == -1 select r;

                    foreach (var r in roots)
                    {
                        FillGroup(r, grps);
                        groups.Add(r);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error int GetDeviceGroups()!" + ex);
            }
        }

        /// <summary>
        /// 获取设备分组信息
        /// </summary>
        /// <returns></returns>
        public static void GetGroups(ref List<DeviceGroup> groups)
        {
            try
            {
                string sql = "SELECT * FROM ipvt_groupinfotable";
                MySqlDataReader reader = null;
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var grp = new DeviceGroup();
                        grp.Id = EvaluationHelper.ObjectToInt(reader["GroupID"]);
                        grp.Name = Convert.ToString(reader["GroupName"]);
                        grp.ParentId = EvaluationHelper.ObjectToInt(reader["ParentID"]);
                        grp.Level = EvaluationHelper.ObjectToInt(reader["GroupLevel"]);
                        groups.Add(grp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error int GetGroups()!" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 递归填充分组数据
        /// </summary>
        /// <param name="grp"></param>
        /// <param name="data"></param>
        private static void FillGroup(DeviceGroup grp, DeviceGroup[] data)
        {
            if (grp != null && data != null)
            {
                int len = data.Length;
                for (int i = 0; i < len; ++i)
                {
                    if (data[i].ParentId == grp.Id)
                    {
                        grp.Children = grp.Children ?? new ObservableCollection<DeviceGroup>();
                        grp.Children.Add(data[i]);
                        FillGroup(data[i], data);
                    }
                    else
                        continue;
                }
            }
        }
    }
}
