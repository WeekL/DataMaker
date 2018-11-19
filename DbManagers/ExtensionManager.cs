using System.Collections.Generic;
using IPPhoneModel;
using IPPhoneModel.ObjectTypes;
using System.Text;
using MySql.Data.MySqlClient;
using DbManagers.Helpers;

namespace DbManagers
{
    /// <summary>
    /// 分机信息管理器
    /// </summary>
    public class ExtensionManager
    {
        /// <summary>
        /// 根据提供的id获取分机信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Extension GetExtensionById(int id)
        {
            return null;
        }

        public static List<Extension> GetValidExtensions()
        {
            var extens = new List<Extension>();
            return extens;
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="extension"></param>
        public static void Update(Extension extension)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ipvt_extensionmessagetable SET PanelNum=?panelNum WHERE ExtensionID=?id ");

            var param = new MySqlParameter[2];
            param[0] = new MySqlParameter("?panelNum", extension.PanelNum);
            param[1] = new MySqlParameter("?id", extension.Id);

            CustomMySqlHelper.ExecuteNonQuery(sqlBuilder.ToString(), param);
        }
    }
}
