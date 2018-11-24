using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mysql
{
    public class MySQLDBHelper:DBHelper
    {
        #region 自定义变量
        private MySqlConnection _dataConnection = null; //当前数据库的连接
        private MySqlCommand _command = null; //当前连接的命令
        private MySqlDataAdapter _adapter;
        #endregion

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="userName">登录名</param>
        /// <param name="passWord">密码</param>
        /// <param name="database">数据库名</param>
        /// <returns>连接字符串</returns>
        public static string GetConnectString(string ip, string userName, string passWord, string database)
        {
            string connectSQL = "server=" + ip + ";user id=" + userName + "; password=" + passWord
                + "; database=" + database + "; pooling=true;charset=utf8";
            return connectSQL;
        }


        /// <summary>
        /// 构造数据库
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="fileName">数据库文件</param>
        public MySQLDBHelper()
            : base()
        {
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="fileName">数据库文件</param>
        /// <returns>0表示成功，-1表示失败</returns>
        public override int Open(string connection, string fileName)
        {
            int success = 0;
            _adapter = new MySqlDataAdapter();
            _dataConnection = new MySqlConnection(connection);
            _command = new MySqlCommand("", _dataConnection);
            try
            {
                _dataConnection.Open();
                success = 0;
            }
            catch (Exception e)
            {
                string exceptionInfo = "";
                exceptionInfo = "异常对象：" + e.Source
                    + "\n 异常信息：" + e.Message
                    + "\n 异常方法：" + e.TargetSite
                    + "\n 异常堆栈：" + e.StackTrace;
                Console.WriteLine(exceptionInfo);
                success = -1;
            }
            return success;
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public override void Close()
        {
            if (_dataConnection == null) return;

            _dataConnection.Close();
            _dataConnection = null;
        }

        /// <summary>
        /// 根据SQL语句获得DataTable
        /// </summary>
        /// <param name="selectSQL">被查询的SQL语句</param>
        /// <param name="tableName">DataTable的名称</param>
        /// <returns>执行查询的SQL语句获得的数据，null表示获取失败</returns>
        public override DataTable GetDataTable(string selectSQL)
        {
            DataTable dataTable = null;
            if (ReConnect())
            {
                dataTable = new DataTable();
                _command.CommandText = selectSQL;
                _adapter.SelectCommand = _command;
                _adapter.Fill(dataTable);
            }
            return dataTable;
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="updateSQL">要执行的SQL语句</param>
        /// <returns>影响的行数，-1表示失败</returns>
        public override int ExcuteNoneQuery(string updateSQL)
        {
            int amount = 0;
            try
            {
                if (ReConnect())
                {
                    _command.CommandText = updateSQL;
                    if (_command.CommandText.IndexOf("COUNT(*)") > -1 || _command.CommandText.IndexOf("count(*)") > -1)
                    {
                        amount = Convert.ToInt32(_command.ExecuteScalar());
                    }
                    else
                    {
                        amount = _command.ExecuteNonQuery();
                    }
                }

                else
                {
                    amount = -1;
                }
            }
            catch (Exception e)
            {
                string exceptionInfo = "";
                exceptionInfo = "异常对象：" + e.Source
                    + "\n 异常信息：" + e.Message
                    + "\n 异常方法：" + e.TargetSite
                    + "\n 异常堆栈：" + e.StackTrace;
                Console.WriteLine(exceptionInfo);
            }
            return amount;
        }

        /// <summary>
        /// 断线重连
        /// </summary>
        /// <returns>重连是否成功</returns>
        public bool ReConnect()
        {
            bool isSuccess = true; // 重连是否成功
            if (_dataConnection == null)
            {
                _dataConnection = new MySqlConnection(_dataConnection.ConnectionString);
            }
            _dataConnection.Ping();

            if (_dataConnection.State == ConnectionState.Closed)
            {
                _dataConnection.Clone();
                _dataConnection.Open();
            }

            if (_dataConnection.Ping())
            {
                isSuccess = true;
            }
            else
            {
                // 完全断线了
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
