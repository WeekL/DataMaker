using Mysql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataMaker
{
    public partial class LoginMysqlFm : Form
    {
        private static string ip, user, pwd,basedb;

        public LoginMysqlFm()
        {
            InitializeComponent();
            ipAddressControl1.Text = "192.168.15.49";
        }

        private void LoginMysqlbutton_Click(object sender, EventArgs e)
        {
            ip = ipAddressControl1.Text;
            user = UserNametextBox.Text;
            pwd = PwdtextBox.Text;
            basedb = BaseDatatextBox.Text;
            try
            {
                string ConnectMysql = null;
                MySQLDBHelper _Mysql = new MySQLDBHelper();
                ConnectMysql = MySQLDBHelper.GetConnectString(ipAddressControl1.Text, UserNametextBox.Text, PwdtextBox.Text, BaseDatatextBox.Text);
                if (0 == _Mysql.Open(ConnectMysql, null))
                {
                    this.Visible = false;
                    DataMakerFm fm = new DataMakerFm(_Mysql);
                    fm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Mysql连接失败!!!");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(ex.Message);
            }
        }

        public static MySQLDBHelper getNewDbHelper()
        {
            try
            {
                string ConnectMysql = null;
                MySQLDBHelper _Mysql = new MySQLDBHelper();
                ConnectMysql = MySQLDBHelper.GetConnectString(ip, user, pwd, basedb);
                if (0 == _Mysql.Open(ConnectMysql, null))
                {
                    return _Mysql;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
    }
}
