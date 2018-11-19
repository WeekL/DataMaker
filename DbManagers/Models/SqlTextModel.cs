using MySql.Data.MySqlClient;

namespace DbManagers.Models
{
    /// <summary>
    /// 批量执行sql语句模型类
    /// </summary>
    public class SqlTextModel
    {
        /// <summary>
        /// sql语句
        /// </summary>
        public string SqlString { get; set; }

        /// <summary>
        /// 参数数组
        /// </summary>
        public MySqlParameter[] MySqlParams { get; set; }
    }
}
