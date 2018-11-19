using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mysql
{
    public abstract class DBHelper
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBHelper()
        {
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="fileName">数据库文件</param>
        /// <returns>0表示成功，-1表示连接失败</returns>
        public abstract int Open(string connection, string fileName);

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// 根据SQL语句获得DataTable
        /// </summary>
        /// <param name="selectSQL">被查询的SQL语句</param>
        /// <param name="tableName">DataTable的名称</param>
        /// <returns>执行查询的SQL语句获得的数据</returns>
        public abstract DataTable GetDataTable(string selectSQL);

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="updateSQL">要执行的SQL语句</param>
        public abstract int ExcuteNoneQuery(string updateSQL);

        /// <summary>
        /// 获取某一个表中的最大的主键值
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="colName">主键列名，必须是整型</param>
        /// <returns></returns>
        public int GetMaxID(string tableName, string colName)
        {
            string sqlString = "Select Max(" + colName + ") As MaxID From " + tableName;
            DataTable dt = GetDataTable(sqlString);

            int maxID = -1;
            if (dt.Rows.Count == 0 || dt.Rows[0]["MaxID"] == DBNull.Value)
            {
                maxID = 0;
            }
            else
            {
                maxID = Convert.ToInt32(dt.Rows[0]["MaxID"]);
            }
            dt.Dispose();
            GC.Collect();
            return maxID;
        }

        private string GetMaxID(string tableName, string colName, string BeginId, string EndId)
        {
            string sqlString = "Select Max(" + colName + ") As MaxID From " + tableName;
            sqlString += " where colName>" + BeginId + " and colName<" + EndId;
            DataTable dt = GetDataTable(sqlString);

            int maxID = -1;
            if (dt.Rows.Count == 0 || dt.Rows[0]["MaxID"] == DBNull.Value)
            {
                maxID = 0;
            }
            else
            {
                maxID = Convert.ToInt32(dt.Rows[0]["MaxID"]);
            }
            dt.Dispose();
            GC.Collect();
            return maxID.ToString();
        }
    }
}
