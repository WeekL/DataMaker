using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mysql;
using System.Data;

namespace DataMaker.util
{
    class QuickLoadAcs
    {
        public static List<string> acsCfgList;

        private QuickLoadAcs() : base()
        {

        }

        public static void attach(MySQLDBHelper db, string orgId)
        {
            QuickLoadUtil.attach(db, orgId);//预加载机构相关信息
            DataTable dt = db.GetDataTable("SELECT id from acs_doorfbsconfig");
            acsCfgList = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                acsCfgList.Add(dt.Rows[i]["id"].ToString());
            }
            dt.Dispose();
        }
    }
}
