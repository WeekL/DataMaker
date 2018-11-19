using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CommonHelperLib;
using DbManagers.Helpers;
using GoogleMapManagers.Models;
using IPPhoneModel;
using IPPhoneModel.ObjectTypes;
using MySql.Data.MySqlClient;

namespace DbManagers
{
    /// <summary>
    /// 网点管理器
    /// </summary>
    public static class GeoPointManager
    {
        public static void Get(LatLngCoordinate leftTop, LatLngCoordinate rightBottom, out List<GeoPoint> points)
        {
            points = new List<GeoPoint>();


            string sql =
                "SELECT GeoID,Name,FormattedAddress,Latitude,Longitude,Phone,Note FROM ipvt_geoinfotable WHERE Latitude>?latStart and Latitude<?latEnd and Longitude>?lngStart and Longitude<?lngEnd";

            var parameters = new MySqlParameter[4];
            parameters[0] = new MySqlParameter("?latStart", rightBottom.Latitude);
            parameters[1] = new MySqlParameter("?latEnd", leftTop.Latitude);
            parameters[2] = new MySqlParameter("?lngStart", leftTop.Longitude);
            parameters[3] = new MySqlParameter("?lngEnd", rightBottom.Longitude);

            try
            {
                DataSet set = CustomMySqlHelper.ExecuteDataSet(sql, parameters);
                if (set.Tables.Count > 0)
                {
                    DataRowCollection rows = set.Tables[0].Rows;
                    if (rows != null)
                    {
                        foreach (DataRow row in rows)
                        {
                            var pnt = new GeoPoint();
                            pnt.Id = EvaluationHelper.ObjectToInt(row["GeoID"]);
                            pnt.Name = Convert.ToString(row["Name"]);
                            pnt.Address = Convert.ToString(row["FormattedAddress"]);
                            pnt.Note = Convert.ToString(row["Note"]);
                            pnt.Phone = Convert.ToString(row["Phone"]);
                            pnt.Latitude = EvaluationHelper.ObjectToDouble(row["Latitude"]);
                            pnt.Longitude = EvaluationHelper.ObjectToDouble(row["Longitude"]);
                            points.Add(pnt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("GeoPointManager.Get error!" + ex);
            }

        }

        /// <summary>
        /// 从提供的网点id中，筛选出指定范围内的网点
        /// </summary>
        /// <param name="id">网点的id数组</param>
        /// <param name="leftTop">地理范围的左上角经纬度</param>
        /// <param name="rightBottom">地理范围的右下角经纬度</param>
        /// <param name="points">网点集</param>
        /// <returns></returns>
        public static void GetPoints(int[] id, LatLngCoordinate leftTop, LatLngCoordinate rightBottom, out List<GeoPoint> points)
        {
            points = new List<GeoPoint>();

            var sql =
                new StringBuilder(
                    "SELECT GeoID,Name,FormattedAddress,Latitude,Longitude,Phone FROM ipvt_geoinfotable WHERE Latitude>?latStart AND Latitude<?latEnd AND Longitude>?lngStart AND Longitude<?lngEnd");

            if (id.Length > 0)
            {
                sql.Append(" AND GeoID IN (");

                foreach (int i in id)
                {
                    sql.Append(i);
                    sql.Append(",");
                }

                sql.Remove(sql.Length - 1, 1);
                sql.Append(")");
            }

            var parameters = new MySqlParameter[4];
            parameters[0] = new MySqlParameter("?latStart", rightBottom.Latitude);
            parameters[0] = new MySqlParameter("?latEnd", leftTop.Latitude);
            parameters[0] = new MySqlParameter("?lngStart", leftTop.Longitude);
            parameters[0] = new MySqlParameter("?lngEnd", rightBottom.Longitude);

            try
            {
                DataSet set = CustomMySqlHelper.ExecuteDataSet(sql.ToString(), parameters);
                if (set.Tables.Count > 0)
                {
                    DataRowCollection rows = set.Tables[0].Rows;
                    if (rows != null)
                    {
                        foreach (DataRow row in rows)
                        {
                            GeoPoint pnt = new GeoPoint();
                            pnt.Id = EvaluationHelper.ObjectToInt(row["GeoID"]);
                            pnt.Name = Convert.ToString(row["Name"]);
                            pnt.Address = Convert.ToString(row["FormattedAddress"]);
                            //pnt.Ip = Convert.ToString(row["ip"]);
                            pnt.Phone = Convert.ToString(row["phone"]);
                            pnt.Latitude = EvaluationHelper.ObjectToDouble(row["Latitude"]);
                            pnt.Longitude = EvaluationHelper.ObjectToDouble(row["Longitude"]);
                            points.Add(pnt);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.MainLog("GeoPointManager.GetPoints error!" + ex);
            }
        }

        /// <summary>
        /// 根据提供的网点获取网点信息
        /// </summary>
        /// <param name="id">网点id数组</param>
        /// <returns></returns>
        public static List<GeoPoint> GetPoints(int[] id)
        {
            var points = new List<GeoPoint>();

            var sqlBuilder =
                new StringBuilder("SELECT GeoID,Name,FormattedAddress,Latitude,Longitude,Phone FROM ipvt_geoinfotable ");

            if (id.Length > 0)
            {
                sqlBuilder.Append(" WHERE GeoID IN (");

                foreach (int i in id)
                {
                    sqlBuilder.Append(i);
                    sqlBuilder.Append(",");
                }

                sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
                sqlBuilder.Append(")");
            }

            sqlBuilder.Append(" ORDER BY Latitude DESC");
            try
            {
                DataSet set = CustomMySqlHelper.ExecuteDataSet(sqlBuilder.ToString());
                if (set.Tables.Count > 0)
                {
                    DataRowCollection rows = set.Tables[0].Rows;
                    if (rows != null)
                    {
                        foreach (DataRow row in rows)
                        {
                            GeoPoint pnt = new GeoPoint();
                            pnt.Id = EvaluationHelper.ObjectToInt(row["GeoID"]);
                            pnt.Name = Convert.ToString(row["Name"]);
                            pnt.Address = Convert.ToString(row["FormattedAddress"]);
                            pnt.Phone = Convert.ToString(row["phone"]);
                            pnt.Latitude = EvaluationHelper.ObjectToDouble(row["Latitude"]);
                            pnt.Longitude = EvaluationHelper.ObjectToDouble(row["Longitude"]);

                            points.Add(pnt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.ToString());
            }

            return points;
        }

        /// <summary>
        /// 按关键字和页码搜索
        /// </summary>
        /// <param name="points">out</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="query">关键字</param>
        /// <param name="count">符合查询条件的总结果数</param>
        /// <param name="itemsCountOfPage">每页显示的结果数</param>
        /// <returns></returns>
        public static void Search(out List<GeoPoint> points, int pageIndex, int itemsCountOfPage, out int count, params string[] query)
        {
            points = new List<GeoPoint>();
            DataSet set = null;
            count = 0;
            if (query.Length > 0)
            {
                int len = query.Length;
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append("CREATE TEMPORARY TABLE ptemp(ID varchar(100), tempStr varchar(350));");
                sqlBuilder.Append(
                    "INSERT INTO ptemp SELECT GeoID,CONCAT(Name,FormattedAddress,Phone,Note) FROM ipvt_geoinfotable;");
                //||' '
                sqlBuilder.Append("SELECT COUNT(*) FROM ptemp WHERE");

                for (int i = 0; i < len;)
                {
                    sqlBuilder.Append(string.Format(" tempStr LIKE '%{0}%'", query[i]));
                    if (++i < len)
                    {
                        sqlBuilder.Append(" AND ");
                    }
                    else
                        sqlBuilder.Append(";");
                }

                sqlBuilder.Append(
                    "SELECT GeoID,Name,FormattedAddress,Latitude,Longitude,Phone,Note FROM ipvt_geoinfotable WHERE GeoID IN");
                sqlBuilder.Append("(");
                sqlBuilder.Append("SELECT ID FROM ptemp WHERE");

                for (int i = 0; i < len;)
                {
                    sqlBuilder.Append(string.Format(" ptemp.tempStr LIKE '%{0}%'", query[i]));
                    if (++i < len)
                    {
                        sqlBuilder.Append(" AND ");
                    }
                }
                sqlBuilder.Append(")");
                sqlBuilder.Append(string.Format(" LIMIT {0},{1}", pageIndex*itemsCountOfPage, itemsCountOfPage));

                set = CustomMySqlHelper.ExecuteDataSet(sqlBuilder.ToString());
            }
            try
            {
                if (set != null && set.Tables.Count == 2)
                {
                    count = EvaluationHelper.ObjectToInt(set.Tables[0].Rows[0][0]);
                    DataRowCollection rows = set.Tables[1].Rows;
                    foreach (DataRow row in rows)
                    {
                        GeoPoint np = new GeoPoint()
                        {
                            Id = EvaluationHelper.ObjectToInt(row["GeoID"]),
                            Name = Convert.ToString(row["Name"]),
                            Address = Convert.ToString(row["FormattedAddress"]),
                            Note = Convert.ToString(row["Note"]),
                            Latitude = EvaluationHelper.ObjectToDouble(row["Latitude"]),
                            Longitude = EvaluationHelper.ObjectToDouble(row["Longitude"])

                        };
                        points.Add(np);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog(ex.ToString());
            }
        }

        /// <summary>
        /// 按关键字搜索网点
        /// </summary>
        /// <param name="points"></param>
        /// <param name="query">查询关键字</param>
        /// <returns></returns>
        public static void Search(out List<GeoPoint> points, params string[] query)
        {
            points = new List<GeoPoint>();
            DataSet set;
            if (query.Length > 0)
            {
                int len = query.Length;
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append("CREATE TEMPORARY TABLE ptemp(ID varchar(100), tempStr varchar(350));");
                sqlBuilder.Append("INSERT INTO ptemp SELECT GeoID,CONCAT(Name,FormattedAddress,Phone,Note) FROM ipvt_geoinfotable;");
                sqlBuilder.Append("SELECT GeoID,Name,FormattedAddress,Latitude,Longitude,Phone,Note FROM ipvt_geoinfotable WHERE GeoID IN");
                sqlBuilder.Append("(");
                sqlBuilder.Append("SELECT ID FROM ptemp WHERE");

                for (int i = 0; i < len;)
                {
                    sqlBuilder.Append(string.Format(" ptemp.tempStr LIKE '%{0}%'", query[i]));
                    if (++i < len)
                    {
                        sqlBuilder.Append(" AND ");
                    }
                }
                sqlBuilder.Append(")");
                try
                {
                    set = CustomMySqlHelper.ExecuteDataSet(sqlBuilder.ToString());
                    DataRowCollection rows = set.Tables[0].Rows;
                    foreach (DataRow row in rows)
                    {
                        GeoPoint pnt = new GeoPoint();
                        pnt.Id = EvaluationHelper.ObjectToInt(row["GeoID"]);
                        pnt.Name = Convert.ToString(row["Name"]);
                        pnt.Address = Convert.ToString(row["FormattedAddress"]);
                        pnt.Note = Convert.ToString(row["Note"]);
                        pnt.Phone = Convert.ToString(row["Phone"]);
                        pnt.Latitude = EvaluationHelper.ObjectToDouble(row["Latitude"]);
                        pnt.Longitude = EvaluationHelper.ObjectToDouble(row["Longitude"]);
                        points.Add(pnt);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 更新网点信息
        /// </summary>
        /// <param name="point">需要更新的网点信息</param>
        public static void Update(GeoPoint point)
        {
            string sql =
                "UPDATE ipvt_geoinfotable SET Name=?name,FormattedAddress=?address,Phone=?phone,Note=?note WHERE GeoID=?id";
            var parameters = new MySqlParameter[5];
            parameters[0] = new MySqlParameter("?name", point.Name);
            parameters[1] = new MySqlParameter("?address", point.Address);
            parameters[2] = new MySqlParameter("?id", point.Id);
            parameters[3] = new MySqlParameter("?phone", point.Phone);
            parameters[4] = new MySqlParameter("?note", point.Note);

            CustomMySqlHelper.ExecuteNonQuery(sql, parameters);

        }

        /// <summary>
        /// 添加网点信息
        /// </summary>
        /// <param name="point">网点信息</param>
        public static void Add(ref GeoPoint point)
        {
            if(point==null)return;

            var sqlBuilder =
                new StringBuilder("INSERT INTO ipvt_geoinfotable(Name,FormattedAddress,Latitude,Longitude,Phone,Note) ");
            sqlBuilder.Append("VALUES(?name,?address,?lat,?lng,?phone,?note);");
            sqlBuilder.Append("SELECT @@IDENTITY");
            //string sql = " ";
            var parameters = new MySqlParameter[6];
            parameters[0] = new MySqlParameter("?name", point.Name);
            parameters[1] = new MySqlParameter("?address", point.Address);
            parameters[2] = new MySqlParameter("?lat", point.Latitude);
            parameters[3] = new MySqlParameter("?lng", point.Longitude);
            parameters[4] = new MySqlParameter("?phone", point.Phone);
            parameters[5] = new MySqlParameter("?note", point.Note);

            object obj = CustomMySqlHelper.ExecuteScalar(sqlBuilder.ToString(), parameters);
            if (obj != null)
            {
                point.Id = EvaluationHelper.ObjectToInt(obj);
            }
        }

        public static int GetCount()
        {
            int count = 0;
            string sqlcount = " select count(*) as count from ipvt_geoinfotable";
            MySqlDataReader readercount = CustomMySqlHelper.ExecuteDataReader(sqlcount);
            try
            {
                while (readercount.Read())
                {
                    count = EvaluationHelper.ObjectToInt(readercount["count"]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("Error in GetCallLogCount!" + ex.Message);
            }
            finally
            {
                if (readercount != null) readercount.Close(); //读取完关闭reader对象
            }
            return count;
        }

        public static void GetGeo(ref List<GeoPoint> listGeo)
        {
            string sql = "select * from ipvt_geoinfotable";
            MySqlDataReader rd = CustomMySqlHelper.ExecuteDataReader(sql);
            try
            {
                DataSet set = CustomMySqlHelper.ExecuteDataSet(sql);
                if (set.Tables.Count > 0)
                {
                    DataRowCollection rows = set.Tables[0].Rows;
                    if (rows != null)
                    {
                        foreach (DataRow row in rows)
                        {
                            var pnt = new GeoPoint();
                            pnt.Id = EvaluationHelper.ObjectToInt(row["GeoID"]);
                            pnt.Name = Convert.ToString(row["Name"]);
                            pnt.Address = Convert.ToString(row["FormattedAddress"]);
                            pnt.Note = Convert.ToString(row["Note"]);
                            pnt.Phone = Convert.ToString(row["Phone"]);
                            pnt.Latitude = EvaluationHelper.ObjectToDouble(row["Latitude"]);
                            pnt.Longitude = EvaluationHelper.ObjectToDouble(row["Longitude"]);
                            listGeo.Add(pnt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.MainLog("GeoPointManager.Get error!" + ex);
            }
        }
    }
}
