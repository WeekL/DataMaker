using Mysql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMaker.bean
{
    /// <summary>
    /// sql语句的底层封装，一个table对象等于一条mysql insert语句
    /// </summary>
    abstract class Table
    {
        //存储列名和对应值的字典，用于拼装sql语句
        protected Dictionary<string, string> sqlDictionary;
        private string tableName;//表名
        private string id;//数据id

        public Table(string tableName)
        {
            this.tableName = tableName;
            id = StringUtil.createRandomGUID();
            sqlDictionary = new Dictionary<string, string>();
            setParam("id", id);
        }

        public string getId()
        {
            return id;
        }

        /// <summary>
        /// 添加列名和值
        /// </summary>
        /// <param name="key">列名</param>
        /// <param name="value">值</param>
        public void setParam(string key, string value)
        {
            sqlDictionary.Add(key, value);
        }

        //初始化sqlHead，应等于各表的静态变量head
        public abstract string initHead();

        /// <summary>
        /// 拼装insert语句的头部
        /// </summary>
        /// <returns>"INSERT INTO TABLENAME(column1,column2,...) VALUES"</returns>
        public virtual string buildSQLHead()
        {
            StringBuilder builder = new StringBuilder("INSERT INTO " + tableName + "(");
            //builder.Append( + sHead + ",");
            foreach (string key in sqlDictionary.Keys)
            {
                builder.Append(key + ",");
            }
            builder.Replace(",", ")", builder.Length - 1, 1);
            builder.Append(" VALUES");
            return builder.ToString();
        }

        /// <summary>
        /// 拼装insert语句的值
        /// </summary>
        /// <returns>"("value1","value2",...)"</returns>
        public string buildSQLValue()
        {
            StringBuilder builder = new StringBuilder("(");
            //builder.Append( + sValue + ",");
            foreach (string key in sqlDictionary.Keys)
            {
                try
                {
                    string value = "";
                    if (sqlDictionary.TryGetValue(key, out value))
                    {
                        builder.Append("\"" + value + "\",");
                    }
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Dictionary不存在对应key的值：" + key);
                }
            }
            builder.Replace(",", ")", builder.Length - 1, 1);
            string result = builder.ToString();
            return result;
        }

        /// <summary>
        /// 拼装完成的insert语句，已弃用，由AddDeviceHelper类统一调度拼装
        /// </summary>
        /// <returns>"INSERT INTO TABLENAME(column1,column2,...) VALUES("value1","value2",...)"</returns>
        public string buildSqlStr()
        {
            //string sql = this.buildSQLHead() + buildSQLValue();
            string sql = initHead() + buildSQLValue();
            Console.WriteLine("-----------------------buildSqlStr-----表名: " + tableName + " ----- sql: " + sql);
            return sql;
        }

        /// <summary>
        /// 执行insert操作，已弃用，由AddDeviceHelper统一调度执行
        /// </summary>
        /// <param name="db"></param>
        public virtual void excute(MySQLDBHelper db)
        {
            db.ExcuteNoneQuery(buildSqlStr());
            Console.WriteLine("====================添加一条数据：" + id);
        }
    }
}
