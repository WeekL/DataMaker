using Mysql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMaker.bean
{
    abstract class Table
    {
        private Dictionary<string, string> sqlDictionary;
        private string tableName;
        private string id;

        //protected string sHead = "";
        //protected string sValue = "";

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

        public void setParam(string key, string value)
        {
            sqlDictionary.Add(key, value);
        }

        //初始化sqlHead，应等于各表的静态变量head
        public abstract string initHead();

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

        public string buildSqlStr()
        {
            //string sql = this.buildSQLHead() + buildSQLValue();
            string sql = initHead() + buildSQLValue();
            Console.WriteLine("-----------------------buildSqlStr-----表名: " + tableName + " ----- sql: " + sql);
            return sql;
        }

        //弃用，用AddDeviceHelper统一调度
        public virtual void excute(MySQLDBHelper db)
        {
            db.ExcuteNoneQuery(buildSqlStr());
            Console.WriteLine("====================添加一条数据：" + id);
        }
    }
}
