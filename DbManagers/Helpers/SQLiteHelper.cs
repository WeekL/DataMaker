using System;
using System.Data;
using System.Data.SQLite;
using CommonHelperLib;

namespace DbManagers.Helpers
{
    public class SqLiteHelper
    {
        /// <summary>
        /// 根据提供的数据库文件，创建并返回一个数据库连接
        /// </summary>
        /// <returns></returns>
        public static SQLiteConnection Open(string dbfile)
        {
            var conn = new SQLiteConnection();
            var builder = new SQLiteConnectionStringBuilder();
            try
            {
                builder.DataSource = dbfile;
                conn.ConnectionString = builder.ToString();
                conn.Open();
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in open SQLite With Param!" + ex.Message);
            }
            return conn;
        }

        /// <summary>
        /// 尝试打开
        /// </summary>
        /// <returns></returns>
        public static bool TryOpen(string dbfile, out SQLiteConnection conn)
        {
            conn = new SQLiteConnection();
            var builder = new SQLiteConnectionStringBuilder();
            try
            {
                builder.DataSource = dbfile;
                conn.ConnectionString = builder.ToString();
                conn.Open();
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in open SQLite With Param!" + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 查询瓦片
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static SQLiteDataReader ExcuteReader(SQLiteConnection conn, string query, params SQLiteParameter[] parameters)
        {
            var command = new SQLiteCommand(query, conn);
            foreach (var p in parameters)
            {
                if ((p.Direction == ParameterDirection.InputOutput ||
                     p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
            }
            command.Parameters.AddRange(parameters); 
            return command.ExecuteReader();
        }

        public static object ExecuteScalar(SQLiteConnection conn, string query, params SQLiteParameter[] parameters)
        {
            object obj = null;
            try
            {
                var command = new SQLiteCommand(query, conn);

                foreach (var p in parameters)
                {
                    if ((p.Direction == ParameterDirection.InputOutput ||
                         p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                }
                command.Parameters.AddRange(parameters);
                obj = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in SqLiteHelper.ExecuteScalar" + ex.Message);
            }
            return obj;
        }

        public static int ExecuteNonquery(SQLiteConnection conn, string query, params SQLiteParameter[] parameters)
        {
            int obj = 0;
            try
            {
                var command = new SQLiteCommand(query, conn);
                foreach (var p in parameters)
                {
                    if ((p.Direction == ParameterDirection.InputOutput ||
                         p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                }
                command.Parameters.AddRange(parameters);
                obj = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in SqLiteHelper.ExecuteNonquery" + ex.Message);
            }
            return obj;
        }
    }
}
