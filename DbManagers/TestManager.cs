using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CommonHelperLib;
using DbManagers.Helpers;
using MySql.Data.MySqlClient;

namespace DbManagers
{
    /// <summary>
    /// 测试数据类
    /// </summary>
    public class TestManager
    {
        public static DataSet GetTestTable()
        {
            string sql = @"SELECT a.ExtensionNO,b.PanelNum
                              FROM ipvt_extensionmessagetable as a inner join ipvt_panelinfotable as b on a.ExtensionID=b.ExtensionID 
                                   inner join ipvt_deviceinfotable as c on a.ExtensionID=c.ExtensionID
                              WHERE a.CurrentState!=0 and c.DeviceType=1 order by a.ExtensionNO,b.PanelNum ";
            DataSet set = null;
            try
            {
                set = CustomMySqlHelper.ExecuteDataSet(sql);
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("error in GetTestTable()" + ex.Message);
            }
            return set;
        }
    }
}
