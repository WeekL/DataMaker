using System;
using CommonHelperLib;
using DbManagers.Helpers;
using IPPhoneModel.ObjectTypes;
using MySql.Data.MySqlClient;

namespace DbManagers
{
    //视频联动
    public class VideoLinkageManager
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public static VideoLinkageModel GetVideoLinkageModel(int did,int pnum)
        {
            VideoLinkageModel model = null;
            if (did > 0 && pnum >= 0)//一代设备的面板号有0的
            {
                string sql = "SELECT * FROM ipvt_videolinkagetable WHERE DeviceId=?did AND PanelNumber=?pnum;";
                MySqlParameter[] ps = new MySqlParameter[2];
                ps[0] = new MySqlParameter("?did", did);
                ps[1] = new MySqlParameter("?pnum", pnum);

                MySqlDataReader reader = null;
                try
                {
                    reader = CustomMySqlHelper.ExecuteDataReader(sql, ps);
                    while (reader.Read())
                    {
                        model = new VideoLinkageModel();
                        model.Id = EvaluationHelper.ObjectToInt(reader["ID"]);
                        model.DeviceId = EvaluationHelper.ObjectToInt(reader["DeviceId"]);
                        model.PanelNumber = EvaluationHelper.ObjectToInt(reader["PanelNumber"]);
                        model.ChannelNumber = EvaluationHelper.ObjectToByte(reader["ChannelNumber"]);
                        model.NetConnectMethods = EvaluationHelper.ObjectToByte(reader["NetConnectMethods"]);
                        model.ImgFormat = EvaluationHelper.ObjectToByte(reader["ImgFormat"]);
                        model.TransmitJur = EvaluationHelper.ObjectToByte(reader["TransmitJur"]);
                        model.ReceivePort = EvaluationHelper.ObjectToUshort(reader["ReceivePort"]);
                        model.ForwardingPort = EvaluationHelper.ObjectToUshort(reader["ForwardingPort"]);
                        model.ServerUserId = EvaluationHelper.ObjectToString(reader["ServerUserId"]);
                        model.ServerIp = EvaluationHelper.ObjectToString(reader["ServerIp"]);
                        model.TranServerIp = EvaluationHelper.ObjectToString(reader["TranServerIp"]);
                        model.UserName = EvaluationHelper.ObjectToString(reader["UserName"]);
                        model.UserPwd = EvaluationHelper.ObjectToString(reader["UserPwd"]);
                        model.DeviceType = EvaluationHelper.ObjectToByte(reader["DeviceType"]);
                        model.UserVerify = EvaluationHelper.ObjectToString(reader["UserVerify"]);

                        break;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.MainLog("error in GetVideoLinkageModel!" + ex.Message);
                }
                finally
                {
                    if (reader != null) reader.Close(); //读取完关闭reader对象
                }
            }

            return model;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool DeleteVideoLinkageModel(VideoLinkageModel model)
        {
            if (model != null && model.DeviceId > 0 /*&& model.PanelNumber > 0*/)   //允许话筒删除联动配置 yk 2017-12-21
            {
                string sql = "delete from ipvt_videolinkagetable where DeviceId=?did AND PanelNumber=?pnum;";
                MySqlParameter[] ps = new MySqlParameter[2];
                ps[0] = new MySqlParameter("?did", model.DeviceId);
                ps[1] = new MySqlParameter("?pnum", model.PanelNumber);

                int result = CustomMySqlHelper.ExecuteNonQuery(sql, ps);
                if (result > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SaveVideoLinkageModel(VideoLinkageModel model)
        {
            if (model != null /*&& model.DeviceId > 0*/ && model.PanelNumber >= 0)//一代的设备面板号有0的
            {
                string sql;
                if (model.Id > 0)
                {
                    sql = "update ipvt_videolinkagetable set ChannelNumber=?cnum,NetConnectMethods=?thods," +
                          "ImgFormat=?img,TransmitJur=?jur,ReceivePort=?rport,ForwardingPort=?fport," +
                          "ServerUserId=?suid,ServerIp=?sip,TranServerIp=?tsip,UserName=?uname,UserPwd=?pwd," +
                          "DeviceType=?type,UserVerify=?verify" +
                          " where DeviceId=?did AND PanelNumber=?pnum";
                }
                else
                {
                    sql = "insert into ipvt_videolinkagetable(DeviceId,PanelNumber,ChannelNumber,NetConnectMethods,"
                        + "ImgFormat,TransmitJur,ReceivePort,ForwardingPort,ServerUserId,ServerIp,TranServerIp," 
                        + "UserName,UserPwd,DeviceType,UserVerify) " 
                        + "values(?did,?pnum,?cnum,?thods,?img,?jur,?rport,?fport,?suid,?sip,?tsip,?uname,?pwd,?type,?verify)";
                }

                MySqlParameter[] ps = new MySqlParameter[15];
                ps[0] = new MySqlParameter("?did", model.DeviceId);
                ps[1] = new MySqlParameter("?pnum", model.PanelNumber);
                ps[2] = new MySqlParameter("?cnum", model.ChannelNumber);
                ps[3] = new MySqlParameter("?thods", model.NetConnectMethods);
                ps[4] = new MySqlParameter("?img", model.ImgFormat);
                ps[5] = new MySqlParameter("?jur", model.TransmitJur);
                ps[6] = new MySqlParameter("?rport", model.ReceivePort);
                ps[7] = new MySqlParameter("?fport", model.ForwardingPort);
                ps[8] = new MySqlParameter("?suid", model.ServerUserId);
                ps[9] = new MySqlParameter("?sip", model.ServerIp);
                ps[10] = new MySqlParameter("?tsip", model.TranServerIp);
                ps[11] = new MySqlParameter("?uname", model.UserName);
                ps[12] = new MySqlParameter("?pwd", model.UserPwd);
                ps[13] = new MySqlParameter("?type", model.DeviceType);
                ps[14] = new MySqlParameter("?verify", model.UserVerify);

                int result = CustomMySqlHelper.ExecuteNonQuery(sql, ps);
                if (result > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
