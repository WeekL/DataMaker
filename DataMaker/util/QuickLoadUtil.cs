using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mysql;
using DataMaker.bean;
using System.Collections;
using System.Data;

namespace DataMaker.util
{
    //预加载的类，用于提前获取需要的数据
    class QuickLoadUtil
    {
        public static Organization curOrg;

        public static List<string> areaList;
        public static List<List<string>> sitePoints;

        protected QuickLoadUtil()
        {
            //禁止被实例化
        }

        public static void attach(MySQLDBHelper db, string orgId)
        {
            if (curOrg!= null && orgId == curOrg.id)
            {
                return;
            }

            curOrg = new Organization(orgId);

            //获取机构信息
            DataTable orgTable = db.GetDataTable("SELECT Code,name from org_Dept WHERE id='" + orgId + "'");
            if (orgTable.Rows.Count == 0)
            {
                return;
            }
            curOrg.num = orgTable.Rows[0]["Code"].ToString();//机构编号
            curOrg.name = orgTable.Rows[0]["name"].ToString();//机构名称
            orgTable.Dispose();

            //获取机构的区域点位信息
            areaList = new List<string>();
            sitePoints = new List<List<string>>();

            //查找机构下的区域
            DataTable dt = db.GetDataTable("SELECT id from Institution_area_BaseInfo WHERE jigoumingchen='" + orgId + "'");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                areaList.Add(dt.Rows[i]["id"].ToString());
            }
            curOrg.areaList = areaList;
            dt.Dispose();
            //查找区域下的点位
            for (int i = 0; i < areaList.Count; i++)
            {
                DataTable position = db.GetDataTable("SELECT id from Institution_area_Position WHERE AreaId='" + areaList[i] + "'");
                List<string> pointList = new List<string>();
                for (int j = 0; j < position.Rows.Count; j++)
                {
                    pointList.Add(dt.Rows[j]["id"].ToString());
                }
                sitePoints.Add(pointList);
                position.Dispose();
            }
            curOrg.pointList = sitePoints;
        }

        public static void detach()
        {
            curOrg = null;
        }
    }
}
