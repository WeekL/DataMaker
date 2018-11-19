using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using CommonHelperLib;
using DbManagers.Models;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Threading;

namespace DbManagers.Helpers
{
    /// <summary>
    /// MySqlHelper类：
    /// </summary>
    public static class CustomMySqlHelper
    {
        /// <summary>
        /// 数据库连接串
        /// </summary>
        public static string ConnectionString { get; set; }

        private static Mutex mutex = new Mutex();

        /// <summary>
        /// 验证数据库连接串是否可以连接
        /// </summary>
        /// <param name="conString"></param>
        /// <returns></returns>
        public static bool TryConnection(string conString)
        {
            using (MySqlConnection con=new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    con.Close();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #region **基本处理**

        /// <summary>
        /// 批量操作每批次记录数
        /// </summary>
        public static int BatchSize = 2000;

        /// <summary>
        /// 超时时间
        /// </summary>
        public static int CommandTimeOut = 3000;

        #region 静态方法

        private static bool PrepareCommand(MySqlCommand command, MySqlConnection connection,
            MySqlTransaction transaction, CommandType commandType, string commandText, MySqlParameter[] parms)
        {
            if (connection.State != ConnectionState.Open && !string.IsNullOrEmpty(connection.ConnectionString))
            {
                try
                {
                    mutex.WaitOne();
                    connection.Open();

                    command.Connection = connection;
                    command.CommandTimeout = CommandTimeOut;
                    // 设置命令文本(存储过程名或SQL语句)
                    command.CommandText = commandText;
                    // 分配事务
                    if (transaction != null)
                    {
                        command.Transaction = transaction;
                    }
                    // 设置命令类型.
                    command.CommandType = commandType;
                    if (parms != null && parms.Length > 0)
                    {
                        //预处理MySqlParameter参数数组，将为NULL的参数赋值为DBNull.Value;
                        foreach (MySqlParameter parameter in parms)
                        {
                            if ((parameter.Direction == ParameterDirection.InputOutput ||
                                 parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                            {
                                parameter.Value = DBNull.Value;
                            }
                        }
                        command.Parameters.AddRange(parms);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog(string.Format("error in PrepareCommand! connectString:{0},ex:{1}",
                        connection.ConnectionString, ex.StackTrace));
                    LogHelper.MainLog(ex.Message);
                    return false;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
            else
            {
                return false;
            }
            return true;//mysqladmin flush-hosts
        }

        #region ExecuteNonQuery

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(string commandText, params MySqlParameter[] parms)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                return ExecuteNonQuery(connection, CommandType.Text, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, string commandText, params MySqlParameter[] parms)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                return ExecuteNonQuery(connection, CommandType.Text, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                return ExecuteNonQuery(connection, commandType, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(MySqlConnection connection, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            return ExecuteNonQuery(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(MySqlTransaction transaction, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            return ExecuteNonQuery(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回影响的行数
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回影响的行数</returns>
        private static int ExecuteNonQuery(MySqlConnection connection, MySqlTransaction transaction,
            CommandType commandType, string commandText, params MySqlParameter[] parms)
        {
            int retval = 0;
            try
            {
                var command = new MySqlCommand();
                bool b = PrepareCommand(command, connection, transaction, commandType, commandText, parms);
                if (b)
                {
                    try
                    {
                        retval = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.MainLog(ex.ToString());
                    }
                }
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.StackTrace);
            }
            return retval;
        }

        #endregion ExecuteNonQuery

        #region ExecuteScalar

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static T ExecuteScalar<T>(string connectionString, string commandText, params MySqlParameter[] parms)
        {
            object result = ExecuteScalar(connectionString, commandText, parms);
            if (result != null)
            {
                return (T)Convert.ChangeType(result, typeof(T));
            }
            return default(T);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(string commandText, params MySqlParameter[] parms)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                return ExecuteScalar(connection, CommandType.Text, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(string connectionString, string commandText, params MySqlParameter[] parms)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                return ExecuteScalar(connection, CommandType.Text, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                return ExecuteScalar(connection, commandType, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);

                    command.Fill(ds, "ds");
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(MySqlConnection connection, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            return ExecuteScalar(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(MySqlTransaction transaction, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            return ExecuteScalar(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        private static object ExecuteScalar(MySqlConnection connection, MySqlTransaction transaction,
            CommandType commandType, string commandText, params MySqlParameter[] parms)
        {
            object retval = null;
            var command = new MySqlCommand();
            bool b = PrepareCommand(command, connection, transaction, commandType, commandText, parms);
            if (b)
            {
                try
                {
                    retval = command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog(ex.ToString());
                }
            }
            command.Parameters.Clear();
            return retval;
        }

        #endregion ExecuteScalar

        #region ExecuteDataReader //use

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        public static MySqlDataReader ExecuteDataReader(string commandText, params MySqlParameter[] parms)
        {
            var connection = new MySqlConnection(ConnectionString);

            return ExecuteDataReader(connection, null, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="connectionString">数据库连接</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        public static MySqlDataReader ExecuteDataReader(string connectionString, string commandText,
            params MySqlParameter[] parms)
        {
            var connection = new MySqlConnection(ConnectionString);

            return ExecuteDataReader(connection, null, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="connectionString">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        public static MySqlDataReader ExecuteDataReader(string connectionString, CommandType commandType,
            string commandText, params MySqlParameter[] parms)
        {
            var connection = new MySqlConnection(ConnectionString);
            return ExecuteDataReader(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        public static MySqlDataReader ExecuteDataReader(MySqlConnection connection, CommandType commandType,
            string commandText, params MySqlParameter[] parms)
        {
            return ExecuteDataReader(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        public static MySqlDataReader ExecuteDataReader(MySqlTransaction transaction, CommandType commandType,
            string commandText, params MySqlParameter[] parms)
        {
            return ExecuteDataReader(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回只读数据集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回只读数据集</returns>
        public static MySqlDataReader ExecuteDataReader(MySqlConnection connection, MySqlTransaction transaction,
            CommandType commandType, string commandText, params MySqlParameter[] parms)
        {
            try
            {
                var command = new MySqlCommand();
                bool b = PrepareCommand(command, connection, transaction, commandType, commandText, parms);
                if (b)
                {
                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.StackTrace);
            }
            return null;
        }

        #endregion

        #region ExecuteDataRow

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>,返回结果集中的第一行</returns>
        public static DataRow ExecuteDataRow(string connectionString, string commandText, params MySqlParameter[] parms)
        {
            DataTable dt = ExecuteDataTable(connectionString, CommandType.Text, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>,返回结果集中的第一行</returns>
        public static DataRow ExecuteDataRow(string connectionString, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            DataTable dt = ExecuteDataTable(connectionString, commandType, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>,返回结果集中的第一行</returns>
        public static DataRow ExecuteDataRow(MySqlConnection connection, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            DataTable dt = ExecuteDataTable(connection, commandType, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一行
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>,返回结果集中的第一行</returns>
        public static DataRow ExecuteDataRow(MySqlTransaction transaction, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            DataTable dt = ExecuteDataTable(transaction, commandType, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        #endregion ExecuteDataRow

        #region ExecuteDataTable

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public static DataTable ExecuteDataTable(string connectionString, string commandText,
            params MySqlParameter[] parms)
        {
            return ExecuteDataSet(connectionString, CommandType.Text, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            return ExecuteDataSet(connectionString, commandType, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public static DataTable ExecuteDataTable(MySqlConnection connection, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            return ExecuteDataSet(connection, commandType, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public static DataTable ExecuteDataTable(MySqlTransaction transaction, CommandType commandType,
            string commandText, params MySqlParameter[] parms)
        {
            return ExecuteDataSet(transaction, commandType, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 执行SQL语句,返回结果集中的第一个数据表
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="tableName">数据表名称</param>
        /// <returns>返回结果集中的第一个数据表</returns>
        public static DataTable ExecuteEmptyDataTable(string connectionString, string tableName)
        {
            return
                ExecuteDataSet(connectionString, CommandType.Text,
                    string.Format("select * from {0} where 1=-1", tableName)).Tables[0];
        }

        #endregion ExecuteDataTable

        #region ExecuteDataSet

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        public static DataSet ExecuteDataSet(string commandText, params MySqlParameter[] parms)
        {
            return ExecuteDataSet(ConnectionString, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        public static DataSet ExecuteDataSet(string connectionString, string commandText, params MySqlParameter[] parms)
        {
            return ExecuteDataSet(connectionString, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        public static DataSet ExecuteDataSet(string connectionString, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                return ExecuteDataSet(connection, commandType, commandText, parms);
            }
        }

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        public static DataSet ExecuteDataSet(MySqlConnection connection, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            return ExecuteDataSet(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        public static DataSet ExecuteDataSet(MySqlTransaction transaction, CommandType commandType, string commandText,
            params MySqlParameter[] parms)
        {
            return ExecuteDataSet(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 执行SQL语句,返回结果集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL语句或存储过程名称</param>
        /// <param name="parms">查询参数</param>
        /// <returns>返回结果集</returns>
        private static DataSet ExecuteDataSet(MySqlConnection connection, MySqlTransaction transaction,
            CommandType commandType, string commandText, params MySqlParameter[] parms)
        {
            var command = new MySqlCommand();
            var ds = new DataSet();
            bool b = PrepareCommand(command, connection, transaction, commandType, commandText, parms);

            if (b)
            {
                try
                {
                    var adapter = new MySqlDataAdapter(command);

                    adapter.Fill(ds);
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog("MySqlDataAdapter Fill to DataSet error! in ExecuteDataSet()!" + ex);
                }
                if (commandText.IndexOf("@", StringComparison.Ordinal) > 0)
                {
                    commandText = commandText.ToLower();
                    int index = commandText.IndexOf("where ", StringComparison.Ordinal);
                    if (index < 0)
                    {
                        index = commandText.IndexOf("\nwhere", StringComparison.Ordinal);
                    }
                    if (index > 0)
                    {
                        ds.ExtendedProperties.Add("SQL", commandText.Substring(0, index - 1));
                        //将获取的语句保存在表的一个附属数组里，方便更新时生成CommandBuilder
                    }
                    else
                    {
                        ds.ExtendedProperties.Add("SQL", commandText); //将获取的语句保存在表的一个附属数组里，方便更新时生成CommandBuilder
                    }
                }
                else
                {
                    ds.ExtendedProperties.Add("SQL", commandText); //将获取的语句保存在表的一个附属数组里，方便更新时生成CommandBuilder
                }

                foreach (DataTable dt in ds.Tables)
                {
                    dt.ExtendedProperties.Add("SQL", ds.ExtendedProperties["SQL"]);
                }
            }

            connection.Close();
            command.Parameters.Clear();
            return ds;
        }

        #endregion ExecuteDataSet

        #endregion 静态方法

        #endregion

        /// <summary>
        /// 执行批量SQL语句
        /// </summary>
        /// <param name="sqlList"></param>
        public static bool ExecuteSqlList(List<SqlTextModel> sqlList)
        {
            bool b;
            using (var con = new MySqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    MySqlTransaction transaction;
                    var command = con.CreateCommand();
                    // 开始一个事务
                    transaction = con.BeginTransaction();

                    try
                    {
                        command.Transaction = transaction;
                        //在一个事务里执行多条sql语句
                        for (int i = 0; i < sqlList.Count; i++)
                        {
                            command.CommandText = sqlList[i].SqlString;

                            if (sqlList[i].MySqlParams != null && sqlList[i].MySqlParams.Length > 0)
                            {
                                //预处理MySqlParameter参数数组，将为NULL的参数赋值为DBNull.Value;
                                foreach (MySqlParameter parameter in sqlList[i].MySqlParams)
                                {
                                    if ((parameter.Direction == ParameterDirection.InputOutput ||
                                         parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                                    {
                                        parameter.Value = DBNull.Value;
                                    }
                                }
                                command.Parameters.AddRange(sqlList[i].MySqlParams);
                            }

                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }

                        // 提交事务
                        transaction.Commit();
                        b = true;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.MainLog(ex.ToString());
                        //执行sql过程中出错，即事务回滚
                        transaction.Rollback();
                        b = false;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog("Open mysql error in ExecuteSqlList()" + ex);
                    b = false;
                }
            }
            return b;
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static int ExecuteSqlTran(ArrayList SQLStringList)
        {
            int val = 0;
            if (SQLStringList.Count > 0)
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    MySqlTransaction tx = conn.BeginTransaction();
                    cmd.Transaction = tx;
                    string sql = "";
                    try
                    {
                        for (int n = 0; n < SQLStringList.Count; n++)
                        {
                            string strsql = SQLStringList[n].ToString();
                            if (strsql.Trim().Length > 1)
                            {
                                cmd.CommandText = strsql;
                                sql = strsql;
                                //val = cmd.ExecuteNonQuery();
                                val += cmd.ExecuteNonQuery();
                            }
                        }
                        tx.Commit();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException E)
                    {
                        //val = -1;
                        val = 0;
                        tx.Rollback();
                        throw new Exception(E.Message);
                    }
                }
            }
            return val;
        }

        /// <summary>
        /// 执行插入语句，获取自增id
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="parms"></param>
        public static int ExecuteSqlToGetId(string[] sqls, params MySqlParameter[] parms)
        {
            int result = 0;
            if (sqls.Length != 2) return result;

            using (var con = new MySqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    var command = con.CreateCommand();
                    for (int i = 0; i < 2; i++)
                    {
                        command.CommandText = sqls[i];

                        if (i == 0)
                        {
                            //预处理MySqlParameter参数数组，将为NULL的参数赋值为DBNull.Value;
                            foreach (MySqlParameter parameter in parms)
                            {
                                if ((parameter.Direction == ParameterDirection.InputOutput ||
                                     parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                                {
                                    parameter.Value = DBNull.Value;
                                }
                            }
                            command.Parameters.AddRange(parms);
                            command.ExecuteNonQuery();
                        }
                        if (i == 1)
                        {
                            result = EvaluationHelper.ObjectToInt(command.ExecuteScalar());
                        }
                        command.Parameters.Clear();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog("error in ExecuteSqlToGetId!" + ex);
                }

            }
            return result;
        }

    }
}
