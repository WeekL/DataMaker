using Mysql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataMaker
{
    public partial class iot_storageFm : Form
    {
        private MySQLDBHelper _Mysql = null;
        public iot_storageFm()
        {
            InitializeComponent();
        }
        public iot_storageFm(MySQLDBHelper Mysql)
        {
            _Mysql = Mysql;
            InitializeComponent();
            // 加入这行多线程操作控件
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            long start = 10000000;
            iot_storageEventprogressBar.Maximum = Int32.Parse(iot_storagetextBox.Text);
            iot_storageEventprogressBar.Value = 0;
            iot_storageEventprogressBar.Step = 1;
            string mysql = null;
            Task t = new Task(() => 
            {
                
                for (count = 1; count <= Int32.Parse(iot_storagetextBox.Text); count++)
                {
                    //写act_hi_actinst表
                    mysql = "INSERT into act_hi_actinst (ID_,PROC_DEF_ID_,PROC_INST_ID_,EXECUTION_ID_,ACT_ID_,TASK_ID_,CALL_PROC_INST_ID_,ACT_NAME_,ACT_TYPE_,ASSIGNEE_,START_TIME_,END_TIME_,DURATION_,TENANT_ID_) VALUES ('" + start.ToString() + "','P59E3EDAF6C451425AFC393C859099821:30:92565','100001','100001','sid-DD4F029D-F4FB-4FEA-B949-13B7FE181B0E','','','','startEvent','','2018-07-23 21:38:11.472','2018-07-23 21:38:11.484','12','');";
                    _Mysql.ExcuteNoneQuery(mysql);
                    //写act_hi_taskinst表
                    mysql = "INSERT INTO act_hi_taskinst (ID_,PROC_DEF_ID_,TASK_DEF_KEY_,PROC_INST_ID_,EXECUTION_ID_,NAME_,PARENT_TASK_ID_,DESCRIPTION_,OWNER_,ASSIGNEE_,START_TIME_,CLAIM_TIME_,END_TIME_,DURATION_,DELETE_REASON_,PRIORITY_,DUE_DATE_,FORM_KEY_,CATEGORY_,TENANT_ID_) VALUES ('" + start.ToString() + "','P59E3EDAF6C451425AFC393C859099821:30:92565','T373550831F3B2E005FC6DA904A14C11D','100001','100001','新增机构','','','','admin','2018-07-23 21:38:11.487',NULL,'2018-07-23 21:38:18.292','6805','completed','50',NULL,'机构主表','','');";
                    _Mysql.ExcuteNoneQuery(mysql);
                    //写act_hi_procinst表
                    mysql = "INSERT INTO act_hi_procinst (ID_,PROC_INST_ID_,BUSINESS_KEY_,PROC_DEF_ID_,START_TIME_,END_TIME_,DURATION_,START_USER_ID_,START_ACT_ID_,END_ACT_ID_,SUPER_PROCESS_INSTANCE_ID_,DELETE_REASON_,TENANT_ID_,NAME_) VALUES ('" + start.ToString() + "','" + start.ToString() + "',NULL,'P59E3EDAF6C451425AFC393C859099821:30:92565','2018-07-23 21:38:11.472','2018-07-23 21:38:35.686','24214','admin','sid-DD4F029D-F4FB-4FEA-B949-13B7FE181B0E','sid-9B616503-2B4D-4B9F-985F-A20503EDBFA6',NULL,NULL,NULL,'新增机构(2018-07-23 21:38:11)');";
                    _Mysql.ExcuteNoneQuery(mysql);
                    start = start + count;
                    iot_storageEventprogressBar.Value += iot_storageEventprogressBar.Step;//让进度条增加一次
                }
                MessageBox.Show("添加成功");
            });
            t.Start();
        }
    }
}
