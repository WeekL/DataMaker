using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Media.Imaging;
using CommonHelperLib;
using DbManagers.Helpers;
using IPPhoneModel.ObjectTypes;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Collections;
using System.Data;


namespace DbManagers
{
    /// <summary>
    /// 用户管理类
    /// </summary>
    public class UserManager
    {
        //private static BitmapImage defaultAvatar;

        static UserManager()
        {
            try
            {
                //defaultAvatar = new BitmapImage(new Uri("pack://application:,,,/./res/avatar/headshow0.png"));
                //defaultAvatar.Freeze();
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("try to load defaultAvatar error!"+ex);
            }
        }


        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public static void GetUsers(out List<User> users, int type = 0)
        {
            users = new List<User>();

            #region
            var builder = new StringBuilder();
            builder.Append("SELECT a.UserID,a.UserName,a.Avatar,a.Password,a.RegTime,a.StaffID," +
                           "a.UserType,a.RoleID,a.DisplayName,a.IsLogin,a.LoginTime,");
            builder.Append("b.Department,b.Sex,b.StaffName,b.StaffNO,b.Telephone,b.PositionID,");
            builder.Append("d.GroupName,e.PositionName ");
            builder.Append("from ipvt_userregmessagetable as a LEFT JOIN ipvt_staffmessagetable as b on a.StaffID=b.StaffID ");
            builder.Append("LEFT JOIN ipvt_jurgrouptable as d on a.RoleID=d.GroupID ");
            builder.Append("LEFT JOIN ipvt_positiontable as e on b.PositionID=e.PositionID");
            builder.Append(" where a.IsEnable=1");

            if (type != 0)
            {
                builder.Append(" and (a.UserType=0 or a.UserType=2)"); 
            }
            #endregion

            MySqlDataReader reader = null;
            try
            {
                int count = 0;
                while (string.IsNullOrEmpty(CustomMySqlHelper.ConnectionString))//自动循环重复
                {
                    if (count > 2)
                    {
                        break;
                    }
                    count++;
                    Thread.Sleep(count*1000);
                }
                //string time = DateTime.Now.TimeOfDay.ToString();
                //string time3="";
                reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString());
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        users.Add(SerializeToUser(reader));
                        //time3 = DateTime.Now.TimeOfDay.ToString();
                    }
                }
                //string time2 = DateTime.Now.TimeOfDay.ToString();
                //MessageBox.Show(time + "\n" + time3 + "\n" + time2);
                ;
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(string.Format("Serialize to user in UserManager.GetUsers error!{0}", ex.StackTrace));
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 获取配置在本地的账号
        /// </summary>
        /// <returns></returns>
        public static User GetLocalAccount()
        {
            return new User() { Name = "Guest",DisplayName = "默认用户",Password = "000000", Avater = null };
        }

        /// <summary>
        /// 获取职位集
        /// </summary>
        public static void GetPositions(out List<Position> list)
        {
            list = new List<Position>();
            string sqlStr = "select PositionID,PositionName from ipvt_positiontable";
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sqlStr);
                while (reader.Read())
                {
                    var p = new Position();
                    p.PositionId = EvaluationHelper.ObjectToInt(reader["PositionID"]);
                    p.PositionName = reader["PositionName"].ToString();
                    list.Add(p);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in UserManager.GetPositions()!" + ex.Message);
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
        /// 获取角色集
        /// </summary>
        public static void GetRoles(out List<Role> list)
        {
            list = new List<Role>();
            string sqlStr = "select GroupID,GroupName from ipvt_jurgrouptable";
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sqlStr);
                while (reader.Read())
                {
                    var r = new Role();
                    r.RoleId = EvaluationHelper.ObjectToInt(reader["GroupID"]);
                    r.RoleName = reader["GroupName"].ToString();
                    list.Add(r);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in UserManager.GetRoles()!" + ex);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns>受影响的行数</returns>
        public static int SaveUser(User user)
        {
            if (user == null) return 0;
            
            //int num = SaveStaff(user.UserStaff);

            //user.UserStaff.Id = user.UserStaff.Id >= num ? user.UserStaff.Id : num;
            //if (num != 0)
            //{
            //    user.UserStaff.Id = num;
            //}

            if (user.Id != 0)
            {
                UpdateUser(user);
                return user.Id;
            }

            return InsertUser(user);
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns>受影响的行数</returns>
        public static int SavePosion(Position _position)
        {
            if (_position == null) return 0;

            if (_position.PositionId != 0)
            {
                UpdatePosion(_position);
                return _position.PositionId;
            }

            return InsertPosition(_position);
        }


        /// <summary>
        /// 插入新职位
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static int InsertPosition(Position p)
        {
            if (p == null) return 0;
            string sql = "insert into ipvt_positiontable(PositionName) "
                         + "values(?name);"
                         + "SELECT @@IDENTITY";
            var parameters = new MySqlParameter[1];
            parameters[0] = new MySqlParameter("?name", p.PositionName);;

            return EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(sql, parameters));
        }


        /// <summary>
        /// 更新职位
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static int UpdatePosion(Position p)
        {
            if (p != null)
            {
                string sql = "update ipvt_positiontable set PositionName=?name where 1=1 and " +
                             "PositionID=?positionId";
                var parameters = new MySqlParameter[2];
                parameters[0] = new MySqlParameter("?name", p.PositionName);
                parameters[1] = new MySqlParameter("?positionId", p.PositionId);
  
                return CustomMySqlHelper.ExecuteNonQuery(sql, parameters);
            }
            return 0;
        }


        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns>受影响的行数</returns>
        private static int UpdateUser(User user)
        {
            if (user != null)
            {
                string sql = "update ipvt_userregmessagetable set UserName=?name,Avatar=?avatar,Password=?pwd," +
                             "UserType=?type,RoleID=?roleId,StaffID=?sid,DisplayName=?disName where UserID=?userId";
                var parameters = new MySqlParameter[8];
                parameters[0] = new MySqlParameter("?name", user.Name);
                parameters[1] = new MySqlParameter("?avatar", ImageHelper.Bitmapimagetobytearray(user.Avater));
                parameters[2] = new MySqlParameter("?pwd", user.Password);
                parameters[3] = new MySqlParameter("?type", user.UserType);
                parameters[4] = new MySqlParameter("?roleId", user.UserRole.RoleId);
                parameters[5] = new MySqlParameter("?sid", user.UserStaff.Id);
                parameters[6] = new MySqlParameter("?disName", user.DisplayName);
                parameters[7] = new MySqlParameter("?userId", user.Id);

                return CustomMySqlHelper.ExecuteNonQuery(sql, parameters);
            }
            return 0;
        }

        /// <summary>
        /// 插入新用户
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns>受影响的行数</returns>
        private static int InsertUser(User user)
        {
            if (user == null) return 0;
            string sql = "insert into ipvt_userregmessagetable(UserName,Avatar,Password,UserType,RoleID,RegTime,StaffID,DisplayName) "
                         + "values(?name,?avatar,?pwd,?type,?roleId,?time,?staffId,?disName);"
                         + "SELECT @@IDENTITY";
            var parameters = new MySqlParameter[8];
            parameters[0] = new MySqlParameter("?name", user.Name);
            parameters[1] = new MySqlParameter("?avatar", ImageHelper.Bitmapimagetobytearray(user.Avater));
            parameters[2] = new MySqlParameter("?pwd", user.Password);
            parameters[3] = new MySqlParameter("?type", user.UserType);
            parameters[4] = new MySqlParameter("?roleId", user.UserRole.RoleId);
            parameters[5] = new MySqlParameter("?time", DateTime.Now);
            parameters[6] = new MySqlParameter("?disName", user.DisplayName);
            parameters[7] = new MySqlParameter("?staffId", user.UserStaff.Id);

            return EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(sql, parameters));
        }

        /// <summary>
        /// 保存员工
        /// </summary>
        /// <param name="sta">员工对象</param>
        /// <returns>受影响的行数</returns>
        public static int SaveStaff(Staff sta)
        {
            if (sta.Id != 0)
            {
                string insertStatffSqlStr = "update ipvt_staffmessagetable " +
                                            "set StaffName=?name,StaffNO=?staffNo,Sex=?sex," +
                                            "Department=?department,Telephone=?phone,PositionID=?positionId" +
                                            " where StaffID=?sid";
                var param = new MySqlParameter[7];
                param[0] = new MySqlParameter("?name", sta.Name);
                param[1] = new MySqlParameter("?staffNo", sta.EmployeeNumber);
                param[2] = new MySqlParameter("?sex", sta.Sex);
                param[3] = new MySqlParameter("?department", sta.Department);
                param[4] = new MySqlParameter("?phone", sta.Phone);
                param[5] = new MySqlParameter("?positionId", sta.StaffPosition.PositionId);
                param[6] = new MySqlParameter("?sid", sta.Id);

                return CustomMySqlHelper.ExecuteNonQuery(insertStatffSqlStr, param);
            }
            else if(!string.IsNullOrEmpty(sta.Name))
            {
                string sqlStr = "INSERT INTO ipvt_staffmessagetable(StaffName,StaffNO,Sex,Department,Telephone,PositionID) " +
                               "VALUES(?name,?staffNo,?sex,?department,?phone,?positionId);" +
                               "SELECT @@IDENTITY";
               // string sqlStr = "INSERT INTO ipvt_staffmessagetable(StaffName,StaffNO,Sex,Department,Telephone,PositionID) " +
               //"VALUES('" + sta.Name + "','" + sta.EmployeeNumber + "'," + sta.Sex + "," + sta.Department + "," + sta.Phone + "," + sta.StaffPosition.PositionId + ")";

                var param = new MySqlParameter[6];
                param[0] = new MySqlParameter("?name", sta.Name);
                param[1] = new MySqlParameter("?staffNo", sta.EmployeeNumber);
                param[2] = new MySqlParameter("?sex", sta.Sex);
                param[3] = new MySqlParameter("?department", sta.Department);
                param[4] = new MySqlParameter("?phone", sta.Phone);
                param[5] = new MySqlParameter("?positionId", sta.StaffPosition.PositionId);

                 //return CustomMySqlHelper.ExecuteSql(sqlStr);

                return EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(sqlStr, param));
            }
            return 0;
        }

        /// <summary>
        /// 删除用户(软删除)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>受影响的行数</returns>
        public static int RemoveUser(int uid)
        {
            string sql = "update ipvt_userregmessagetable set IsEnable=0 where UserID=?id";
            var param = new MySqlParameter[1];
            param[0]=new MySqlParameter("?id",uid);
            return CustomMySqlHelper.ExecuteNonQuery(sql, param);
        }

        /// <summary>
        /// 删除用户(绝对删除)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>受影响的行数</returns>
        public static int DeleteUser(int uid)
        {
            string sql = "delete from ipvt_userregmessagetable where UserID=?id";
            var param = new MySqlParameter[1];
            param[0] = new MySqlParameter("?id", uid);
            return CustomMySqlHelper.ExecuteNonQuery(sql, param);
        }

        /// <summary>
        /// 获取员工
        /// </summary>
        /// <returns></returns>
        public static void GetStaves(out List<Staff> list)
        {
            list = new List<Staff>();
            string sqlStr = "Select * from ipvt_staffmessagetable";
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sqlStr); //执行SQL
                while (reader.Read())
                {
                    var staff = new Staff();
                    staff.Id = EvaluationHelper.ObjectToInt(reader["StaffID"]);
                    staff.Name = reader["StaffName"].ToString();
                    staff.Sex = EvaluationHelper.ObjectToInt2(reader["Sex"]);
                    staff.EmployeeNumber = reader["StaffNO"].ToString();
                    staff.Phone = reader["Telephone"].ToString();
                    staff.Department = reader["Department"].ToString();
                    //
                    staff.StaffPosition.PositionId = EvaluationHelper.ObjectToInt(reader["PositionID"]);
                   
                    if (staff.StaffPosition.PositionId != 0) //如果职位id存在，尝试获取职位名称
                    {
                        staff.StaffPosition.PositionName = GetPositionNameById(staff.StaffPosition.PositionId);
                    }
                    
                    list.Add(staff);
                }

            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in UserManager.GetStaves()!" + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
        }

        /// <summary>
        /// 删除员工(绝对删除)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>受影响的行数</returns>
        public static int DeleteStaves(int uid)
        {
            ArrayList sql = new ArrayList();
            sql.Add("delete from ipvt_staffmessagetable where StaffID=" + uid + "");
            sql.Add("update ipvt_userregmessagetable set StaffID=null where StaffID=" + uid + "");
            return CustomMySqlHelper.ExecuteSqlTran(sql);
        }


        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>受影响的行数</returns>
        public static int DeletePosition(int id)
        {
            ArrayList al = new ArrayList();

            al.Add("delete from ipvt_positiontable where PositionID="+id+"");
            al.Add("update ipvt_staffmessagetable set PositionID=null where PositionID=" + id + "");

            //string sql = "delete from ipvt_positiontable where PositionID=?id";
            //var param = new MySqlParameter[1];
            //param[0] = new MySqlParameter("?id", id);


            return CustomMySqlHelper.ExecuteSqlTran(al);
            //return CustomMySqlHelper.ExecuteNonQuery(sql, param);
        }

        /// <summary>
        /// 通过id获取员工
        /// </summary>
        /// <returns></returns>
        public static Staff GetStaveById(int id)
        {
            string sqlStr = "Select * from ipvt_staffmessagetable where StaffID=?id";
            var param = new MySqlParameter[1];
            param[0] = new MySqlParameter("?id", id);
            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(sqlStr, param); //执行SQL
                while (reader.Read())
                {
                    var staff = new Staff();
                    staff.Id = EvaluationHelper.ObjectToInt(reader["StaffID"]);
                    staff.Name = reader["StaffName"].ToString();
                    staff.Sex = EvaluationHelper.ObjectToInt2(reader["Sex"]);
                    staff.EmployeeNumber = reader["StaffNO"].ToString();
                    staff.Phone = reader["Telephone"].ToString();
                    staff.Department = reader["Department"].ToString();
                    //
                    staff.StaffPosition.PositionId = EvaluationHelper.ObjectToInt(reader["PositionID"]);

                    if (staff.StaffPosition.PositionId != 0) //如果职位id存在，尝试获取职位名称
                    {
                        staff.StaffPosition.PositionName = GetPositionNameById(staff.StaffPosition.PositionId);
                    }
                    return staff;
                }

            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in UserManager.GetStaveById(int id)!" + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
            return new Staff();
        }


        /// <summary>
        /// 判断是否存在关联员工的用户
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static bool IsUserExistByStaffID(int sid)
        {
            bool IsExist = false;

            string sql = "select * from ipvt_userregmessagetable where StaffID= '" + sid + "'";

            try
            {
                DataSet ds = CustomMySqlHelper.Query(sql);
                if (ds.Tables[0].Rows.Count != 0) IsExist = true;
            }
            catch (Exception ex)
            {
            }

            return IsExist;
        }

        /// <summary>
        /// 判断是否存在关联员工的用户
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static bool IsStaffExistByPositionID(int pid)
        {
            bool IsExist = false;

            string sql = "select * from ipvt_staffmessagetable where PositionID= '" + pid + "'";

            try
            {
                DataSet ds = CustomMySqlHelper.Query(sql);
                if (ds.Tables[0].Rows.Count != 0) IsExist = true;
            }
            catch (Exception ex)
            {
            }

            return IsExist;
        }

        /// <summary>
        /// 通过id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static User GetUserById(int id)
        {
            var builder = new StringBuilder();
            builder.Append("SELECT a.UserID,a.UserName,a.Avatar,a.Password,a.RegTime," +
                           "a.StaffID,a.UserType,a.RoleID,a.DisplayName,a.IsLogin,a.LoginTime,");
            builder.Append("b.Department,b.Sex,b.StaffName,b.StaffNO,b.Telephone,b.PositionID,");
            builder.Append("d.GroupName,e.PositionName ");
            builder.Append("from ipvt_userregmessagetable as a LEFT JOIN ipvt_staffmessagetable as b on a.StaffID=b.StaffID ");
            builder.Append("LEFT JOIN ipvt_jurgrouptable as d on a.RoleID=d.GroupID ");
            builder.Append("LEFT JOIN ipvt_positiontable as e on b.PositionID=e.PositionID");
            builder.Append(" where a.UserID=?id and a.IsEnable=1");

            var prams = new MySqlParameter[1];
            prams[0] = new MySqlParameter("?id", id);

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString(), prams);
                while (reader.Read())
                {
                   return SerializeToUser(reader);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(string.Format("Serialize to user in UserManager.GetUserById error!{0}", ex));
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
            return new User();
        }




        /// <summary>
        /// 通过ID获取Position实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Position GetPositionById(int id)
        {
            var builder = new StringBuilder();
            builder.Append("SELECT * FROM ipvt_positiontable WHERE 1=1 AND ");
            builder.Append("PositionID=?id");

            var prams = new MySqlParameter[1];
            prams[0] = new MySqlParameter("?id", id);

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString(), prams);
                while (reader.Read())
                {
                    reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString(), prams);
                    while (reader.Read())
                    {
                        var _Position = new Position();
                        _Position.PositionId = EvaluationHelper.ObjectToInt(reader["PositionID"]);
                        _Position.PositionName = reader["PositionName"].ToString();

                        return _Position;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(string.Format("Serialize to position in UserManager.GetPositionById error!{0}", ex));
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
            return new Position();
        }





        /// <summary>
        /// 通过id获取职位名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetPositionNameById(int id)
        {
            string sqlStr = "select PositionName from ipvt_positiontable where PositionID=?id";
            var ps = new MySqlParameter[1];
            ps[0] = new MySqlParameter("?id",id);

            return CustomMySqlHelper.ExecuteScalar(sqlStr, ps) != null ? CustomMySqlHelper.ExecuteScalar(sqlStr, ps).ToString() : string.Empty;
        }

        /// <summary>
        /// 服务器登录，判断用户登陆账号密码是否正确
        /// </summary>
        /// <param name="id">账号</param>
        /// <param name="pwd">密码</param>
        /// <returns>true：存在 false：不存在</returns>
        public static bool IsUserExist(string id,string pwd)
        {
            //查询匹配的用户名、密码
            string sqlStr = "select COUNT(*) from ipvt_userregmessagetable where IsEnable=1 AND (UserType=1 OR UserType=2) " +
                            "AND UserName=?name AND Password=?pwd";

            var ps = new MySqlParameter[2];
            ps[0] = new MySqlParameter("?name", id);
            ps[1] = new MySqlParameter("?pwd", pwd);

            int num = EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(sqlStr, ps));//查询结果
            if (num > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断用户名是否已存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool JudgeUserNameIsExist(string name)
        {
            string sql = "select COUNT(*) from ipvt_userregmessagetable where UserName=?name";

            var ps = new MySqlParameter[1];
            ps[0] = new MySqlParameter("?name", name);

            int num = EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(sql, ps));//查询结果
            if (num > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 指定账户密码获取用户(服务器)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static User GetUserByNameAndPwd(string name, string pwd)
        {
            #region
            var builder = new StringBuilder();
            builder.Append("SELECT a.UserID,a.UserName,a.Avatar,a.Password,a.RegTime,a.StaffID," +
                           "a.UserType,a.RoleID,a.DisplayName,a.IsLogin,a.LoginTime,");
            builder.Append("b.Department,b.Sex,b.StaffName,b.StaffNO,b.Telephone,b.PositionID,");
            builder.Append("d.GroupName,e.PositionName ");
            builder.Append("from ipvt_userregmessagetable as a LEFT JOIN ipvt_staffmessagetable as b on a.StaffID=b.StaffID ");
            builder.Append("LEFT JOIN ipvt_jurgrouptable as d on a.RoleID=d.GroupID ");
            builder.Append("LEFT JOIN ipvt_positiontable as e on b.PositionID=e.PositionID ");
            builder.Append("where a.IsEnable=1 AND a.UserName=?name AND a.Password=?pwd and (a.UserType=1 OR a.UserType=2)");

            var ps = new MySqlParameter[2];
            ps[0] = new MySqlParameter("?name", name);
            ps[1] = new MySqlParameter("?pwd", pwd);
            #endregion

            MySqlDataReader reader = null;
            try
            {
                reader = CustomMySqlHelper.ExecuteDataReader(builder.ToString(), ps);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        return SerializeToUser(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(string.Format("Serialize to user in UserManager.GetUserByNameAndPwd error!{0}", ex));
            }
            finally
            {
                if (reader != null) reader.Close(); //读取完关闭reader对象
            }
            return null;
        }

        /// <summary>
        /// 序列化用户对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static User SerializeToUser(MySqlDataReader reader)
        {
            #region 用户基本信息

            var user = new User();
            user.Id = EvaluationHelper.ObjectToInt(reader["UserID"]); 
            user.Name = reader["UserName"].ToString();
            user.Password = reader["Password"].ToString();
            user.DisplayName = reader["DisplayName"].ToString();
            user.RegTime = EvaluationHelper.ObjectToDateTime(reader["RegTime"]);
            user.UserType = EvaluationHelper.ObjectToInt(reader["UserType"]);
            user.IsLogin = EvaluationHelper.ObjectToInt2(reader["IsLogin"]);
            user.LoginTime = EvaluationHelper.ObjectToDateTime(reader["LoginTime"]);
            user.Avater = null;
            if (reader["Avatar"] != DBNull.Value)
            {
                try
                {
                    BitmapImage img = ImageHelper.Bytearraytobitmapimage(reader["Avatar"] as byte[]);
                    img.Freeze(); //冻结bitmap，使其可以跨线程访问
                    user.Avater = img;
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog(
                        string.Format("Convert To BitmapImage produce error!UserName={0},ex={1}",
                            user.Name, ex));
                }
            }
            //角色
            user.UserRole.RoleId = EvaluationHelper.ObjectToInt(reader["RoleID"]); 
            user.UserRole.RoleName = reader["GroupName"].ToString();

            #region 用户对应的员工信息

            var us = new Staff();
            us.Department = reader["Department"].ToString();
            us.EmployeeNumber = reader["StaffNO"].ToString();
            us.Id = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["StaffID"].ToString()) ? "0" : reader["StaffID"]);
            us.Name = reader["StaffName"].ToString();
            us.Phone = reader["Telephone"].ToString();
            us.Sex = EvaluationHelper.ObjectToInt2(string.IsNullOrEmpty(reader["Sex"].ToString()) ? "false" : reader["Sex"]);
            //职位
            us.StaffPosition.PositionId = EvaluationHelper.ObjectToInt(string.IsNullOrEmpty(reader["PositionID"].ToString()) ? "0" : reader["PositionID"]);
            us.StaffPosition.PositionName = reader["PositionName"].ToString();

            #endregion

            user.UserStaff = us; //用户对应的员工信息对象赋值

            #endregion

            return user;
        }

        /// <summary>
        /// 判断指定id的用户指定的列的值
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="cname"></param>
        /// <returns></returns>
        public static object GetValue(int uid,string cname)
        {
            //查询匹配的用户名、密码
            string sqlStr = string.Format("select {0} from ipvt_userregmessagetable where UserID=?id and IsEnable=1",cname);

            var ps = new MySqlParameter[1];
            ps[0] = new MySqlParameter("?id", uid);

            return CustomMySqlHelper.ExecuteScalar(sqlStr, ps);//查询结果
        }

        /// <summary>
        /// 改变指定id的用户的登录标识
        /// </summary>
        /// <param name="id">|用户id</param>
        /// <param name="type">0：未登录 1：已登录</param>
        /// <returns></returns>
        public static int UpdateSetIsLogin(int id,int type)
        {
            string sql = "update ipvt_userregmessagetable set IsLogin=?type,LoginTime=?time where UserID=?id";

            var ps = new MySqlParameter[3];
            ps[0] = new MySqlParameter("?id", id);
            ps[1] = new MySqlParameter("?type", type);
            ps[2] = new MySqlParameter("?time", DateTime.Now);

            return CustomMySqlHelper.ExecuteNonQuery(sql, ps);
        }

        /// <summary>
        /// 添加admin用户
        /// </summary>
        /// <returns></returns>
        public static int SetAdminUser()
        {
            int i = 1;
            if (!JudgeUserNameIsExist("admin"))
            {
                i = SaveUser(new User
                {
                    Name = "admin",
                    Password = "admin",
                    DisplayName = "超级管理员",
                    UserRole = new Role {RoleId = 1},
                    UserType = 1
                });
            }
            return i;
        }

        /// <summary>
        /// 中心登录，判断用户登陆账号密码是否正确
        /// </summary>
        /// <param name="id">账号</param>
        /// <param name="pwd">密码</param>
        /// <param name="user">存在返回用户</param>
        /// <returns>true：存在 false：不存在</returns>
        public static bool IsPhoneUserExist(string id, string pwd,out User user)
        {
            user = new User();
            //查询匹配的用户名、密码
            string sqlStr = "select UserID from ipvt_userregmessagetable where IsEnable=1 AND (UserType=0 OR UserType=2) " +
                            "AND UserName=?name AND Password=?pwd";

            var ps = new MySqlParameter[2];
            ps[0] = new MySqlParameter("?name", id);
            ps[1] = new MySqlParameter("?pwd", pwd);

            int num = EvaluationHelper.ObjectToInt(CustomMySqlHelper.ExecuteScalar(sqlStr, ps));//查询结果
            if (num > 0)
            {
                user = GetUserById(num);
                return true;
            }
            return false;
        }
    }
}


//#define          宏定义

//#undef           未定义宏

//#include         文本包含

//#ifdef           如果宏被定义就进行编译

//#ifndef          如果宏未被定义就进行编译

//#endif           结束编译块的控制

//#if              表达式非零就对代码进行编译

//#else            作为其他预处理的剩余选项进行编译 

//#elif            这是一种#else和#if的组合选项

//#line            改变当前的行数和文件名称

//#error           输出一个错误信息 

//#pragma          为编译程序提供非常规的控制流信息
