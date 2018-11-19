using System;
using System.Collections.Generic;
using System.Text;
using CommonHelperLib;
using DbManagers.Helpers;
using DbManagers.Models;
using IPPhoneModel.EnumTypes;
using IPPhoneModel.ObjectTypes;
using MySql.Data.MySqlClient;

namespace DbManagers
{
    //权限管理
    public class JurisdictionManager
    {
        /// <summary>
        /// 获取权限集合
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="list">out</param>
        /// <returns></returns>
        public static void GetJurList(int gid, out List<JurisdictionEnum> list)
        {
            if (gid <= 0)
            {
                gid = 1;
            }
            list = new List<JurisdictionEnum>();
            if (gid > 0)
            {
                string sql = "SELECT JurisdictionID from ipvt_jurisdiction_grouptable where GroupID=?gid";

                var ps = new MySqlParameter("?gid", gid);

                var reader = CustomMySqlHelper.ExecuteDataReader(sql, ps);
                try
                {
                    while (reader.Read())
                    {
                        var jid = EvaluationHelper.ObjectToInt(reader[0]);
                        if (jid > 0)
                        {
                            list.Add(GetJurisdictionEnum(jid));
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog("error in GetJurList(int gid)!" + ex.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close(); //读取完关闭reader对象
                    }
                }
            }
        }
        /// <summary>
        /// 转换（未完全）
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        private static JurisdictionEnum GetJurisdictionEnum(int jid)
        {
            return (JurisdictionEnum)jid;
        }

        /// <summary>
        /// 获取权限组
        /// </summary>
        /// <returns></returns>
        public static void GetJurGroups(out List<JurisdictionGroup> list)
        {
            list = new List<JurisdictionGroup>();
            string sql = "select GroupID,GroupName,Description,DefaultGroup from ipvt_jurgrouptable";

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                while (reader.Read())
                {
                    var group = new JurisdictionGroup();
                    group.GroupId = EvaluationHelper.ObjectToInt(reader["GroupID"]);
                    group.Default = EvaluationHelper.ObjectToInt(reader["DefaultGroup"]);
                    group.GroupName = reader["GroupName"].ToString();
                    group.Description = reader["Description"].ToString();

                    list.Add(group);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in JurisdictionManager.GetJurGroups()!" + ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close(); //读取完关闭reader对象
                }
            }

            foreach (JurisdictionGroup group in list)
            {
                List<Jurisdiction> js;
                GetJurisdictions(group.GroupId, out js);
                if (js != null)
                {
                    foreach (Jurisdiction jur in js)
                    {
                        group.Jurisdictions.Add(jur);
                    }
                }
            }
        }

        /// <summary>
        /// 获取权限集合
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="list">out</param>
        /// <returns></returns>
        public static void GetJurisdictions(int gid,out List<Jurisdiction> list)
        {
            list = new List<Jurisdiction>();
            if (gid > 0)
            {
                var builder = new StringBuilder();
                builder.Append("select a.JurisdictionID,b.JurisdictionName,b.Description ");
                builder.Append("from ipvt_jurisdiction_grouptable as a ");
                builder.Append("LEFT JOIN ipvt_jurisdictiontable as b ");
                builder.Append("on a.JurisdictionID=b.JurisdictionID ");
                builder.Append("where a.GroupID=?gid");

                var ps = new MySqlParameter("?gid", gid);
                MySqlDataReader reader = null;
                try
                {
                    reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString(), ps);
                    while (reader.Read())
                    {
                        var jur = new Jurisdiction();
                        jur.Id = EvaluationHelper.ObjectToInt(reader["JurisdictionID"]);
                        jur.JurName = reader["JurisdictionName"].ToString();
                        jur.Description = reader["Description"].ToString();

                        list.Add(jur);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog("error in GetJurList(int gid)!" + ex.Message);
                }
                finally
                {
                    if (reader != null)  reader.Close(); //读取完关闭reader对象
                }
            }
        }

        /// <summary>
        /// 获取所有权限
        /// </summary>
        public static void GetJurisdictions(out List<Jurisdiction> list)
        {
            list = new List<Jurisdiction>();
            string sql = "select JurisdictionID,JurisdictionName,Description from ipvt_jurisdictiontable";

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sql);
                while (reader.Read())
                {
                    var jur = new Jurisdiction();
                    jur.Id = EvaluationHelper.ObjectToInt(reader["JurisdictionID"]);
                    jur.JurName = reader["JurisdictionName"].ToString();
                    jur.Description = reader["Description"].ToString();

                    list.Add(jur);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in GetJurList(int gid)!" + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 更新权限组
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool UpdateGroup(JurisdictionGroup group)
        {
            var models = new List<SqlTextModel>();
            if (group != null && group.GroupId > 0 && !string.IsNullOrEmpty(group.GroupName))
            {
                //更新权限组表组信息
                string gsql = "update ipvt_jurgrouptable set GroupName=?name,Description=?des where GroupID=?id";
                var ps = new MySqlParameter[3];
                ps[0] = new MySqlParameter("?name", group.GroupName);
                ps[1] = new MySqlParameter("?des", group.Description);
                ps[2] = new MySqlParameter("?id", group.GroupId);

                models.Add(new SqlTextModel {SqlString = gsql, MySqlParams = ps});
                //删除权限组-权限表指定组的旧对应数据
                string delsql = "delete from ipvt_jurisdiction_grouptable where GroupID=?id";
                var delps = new[] {new MySqlParameter("?id", group.GroupId)};

                models.Add(new SqlTextModel { SqlString = delsql, MySqlParams = delps });
                //添加权限组-权限表指定组的新对应数据
                foreach (Jurisdiction jur in group.Jurisdictions)
                {
                    string insertsql =
                        "insert into ipvt_jurisdiction_grouptable(GroupID,JurisdictionID) values(?gid,?jid)";
                    var inps = new MySqlParameter[2];
                    inps[0] = new MySqlParameter("?gid", group.GroupId);
                    inps[1] = new MySqlParameter("?jid", jur.Id);

                    models.Add(new SqlTextModel { SqlString = insertsql, MySqlParams = inps });
                }

                return CustomMySqlHelper.ExecuteSqlList(models);//事务批量执行
            }

            return false;
        }

        /// <summary>
        /// 插入权限组
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool InsertGroup(JurisdictionGroup group)
        {
            var models = new List<SqlTextModel>();
            if (group != null && !string.IsNullOrEmpty(group.GroupName))
            {
                //插入组信息
                string gsql =
                    "insert into ipvt_jurgrouptable(GroupName,Description) values(?name,?des);SELECT @@IDENTITY";
                var ps = new MySqlParameter[2];
                ps[0] = new MySqlParameter("?name", group.GroupName);
                ps[1] = new MySqlParameter("?des", group.Description);

                int newid = EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(gsql, ps));

                if (newid > 0)
                {
                    //添加权限组-权限表指定组的新对应数据
                    foreach (Jurisdiction jur in group.Jurisdictions)
                    {
                        string insertsql =
                            "insert into ipvt_jurisdiction_grouptable(GroupID,JurisdictionID) values(?gid,?jid)";
                        var inps = new MySqlParameter[2];
                        inps[0] = new MySqlParameter("?gid", newid);
                        inps[1] = new MySqlParameter("?jid", jur.Id);

                        models.Add(new SqlTextModel { SqlString = insertsql, MySqlParams = inps });
                    }

                    return CustomMySqlHelper.ExecuteSqlList(models);//事务批量执行
                }
            }

            return false;
        }

        /// <summary>
        /// 删除权限组
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="commongid"></param>
        /// <returns></returns>
        public static bool DeleteGroup(int gid,int commongid)
        {
            var models = new List<SqlTextModel>();
            if (gid>0)
            {
                //删除组信息
                string gsql = "delete from ipvt_jurgrouptable where GroupID=?id";
                var ps = new[] { new MySqlParameter("?id", gid) };

                models.Add(new SqlTextModel { SqlString = gsql, MySqlParams = ps });

                //删除权限组-权限表指定组的旧对应数据
                string delsql = "delete from ipvt_jurisdiction_grouptable where GroupID=?id";
                var delps = new[] { new MySqlParameter("?id", gid) };

                models.Add(new SqlTextModel { SqlString = delsql, MySqlParams = delps });

                string updatesql = "update ipvt_userregmessagetable set RoleID=?rid where RoleID=?gid";
                var inps = new MySqlParameter[2];
                inps[0] = new MySqlParameter("?rid", commongid);
                inps[1] = new MySqlParameter("?gid", gid);

                models.Add(new SqlTextModel { SqlString = updatesql, MySqlParams = inps });

                return CustomMySqlHelper.ExecuteSqlList(models);//事务批量执行
            }

            return false;
        }
    }
}
