using System;

namespace DbManagers
{
    public class DbManagerBase
    {
        /// <summary>
        /// 运行目录
        /// </summary>
        public static string RootPath = AppDomain.CurrentDomain.BaseDirectory;

        //6位3000个分机号
        public static bool IsTh { get; set; }
    }
}
