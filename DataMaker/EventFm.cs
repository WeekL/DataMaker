using Mysql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace DataMaker
{
    public partial class EventFm : Form
    {
        private string _NodeTag = null;
        private string _NodeName = null;
        private MySQLDBHelper _Mysql = null;
        private DataTable _dtDevType = null;
        private DataTable _dtEventType = null;
        private DataTable _dtEventGrade = null;
        private DataTable _dtGroup = null;
        private DataTable _dtTime = null;
        private DataTable _dtLinkage = null;
        private DataTable _dtChannel = null;
        private DataTable _dtRole = null;
        private string _DevCode = null;
        private string _EventLevelID = null;
        private string _CombinationTypeID = null;
        private string _TimeTemplateID = null;//时间模板
        private string _StrategyTemplateID = null;//联动模板
        private string _EventInfoCode = null;//事件类型
        private string _DevHID = null;
        private string _RoleID = null;

        private string _UserToRecv = null;
        public EventFm()
        {
            InitializeComponent();
        }
        public EventFm(string NodeTag, string NodeName,MySQLDBHelper Mysql)
        {
            _NodeTag = NodeTag;
            _NodeName = NodeName;
            _Mysql = Mysql;
            InitializeComponent();
            DataInit();
        }
        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone(); // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.Rows.Add(row); // 将DataRow添加到DataTable中
            return tmp;
        }
        private void DevTypeInit()
        {
            string sql = string.Empty;
            sql = "SELECT * FROM cfg_devtype";
            _dtDevType = _Mysql.GetDataTable(sql);
            DevTypecomboBox.DataSource = _dtDevType;
            DevTypecomboBox.DisplayMember = "Name";
            DevTypecomboBox.ValueMember = "Code";
        }
        private void EventTypeInit()
        {
            string sql = string.Empty;            
            sql = "SELECT * FROM cfg_deveventtype";
            _dtEventType = _Mysql.GetDataTable(sql);
            EventTypecomboBox.DataSource = _dtEventType;
            EventTypecomboBox.DisplayMember = "Name";
            EventTypecomboBox.ValueMember = "id";
        }
        private void EventGradeInit()
        {
            string sql = string.Empty;
            sql = "SELECT * FROM event_levelinfo";
            _dtEventGrade = _Mysql.GetDataTable(sql);
            EventGradecomboBox.DataSource = _dtEventGrade;
            EventGradecomboBox.DisplayMember = "EventLevelName";
            EventGradecomboBox.ValueMember = "id";
        }
        //组合类型
        private void GroupInit()
        {
            string sql = string.Empty;
            sql = "SELECT * FROM event_compositeeventtype";
            _dtGroup = _Mysql.GetDataTable(sql);
            GroupcomboBox.DataSource = _dtGroup;
            GroupcomboBox.DisplayMember = "EventDefineName";
            GroupcomboBox.ValueMember = "id";
        }
        private void TimeInit()
        {
            string sql = string.Empty;
            sql = "SELECT * FROM sys_timetemplate";
            _dtTime = _Mysql.GetDataTable(sql);
            TimecomboBox.DataSource = _dtTime;
            TimecomboBox.DisplayMember = "Name";
            TimecomboBox.ValueMember = "id";
        }
        //联动模板
        private void Linkage()
        {
            string sql = string.Empty;
            sql = "SELECT * FROM event_DetailLinkMode";
            _dtLinkage = _Mysql.GetDataTable(sql);
            LinkagecomboBox.DataSource = _dtLinkage;
            LinkagecomboBox.DisplayMember = "ModeName";
            LinkagecomboBox.ValueMember = "id";
        }
        //关联设备
        private void RelationDev()
        {
            string sql = string.Empty;
            DataTable dtDevice = null;
            DataTable dtChildDevice = null;
            DataTable dtChannel = new DataTable();
            dtChannel.Columns.Add("id", typeof(String));
            dtChannel.Columns.Add("name", typeof(String));

            sql = "SELECT * FROM video_devicebase WHERE DevType = '4' OR DevType = '3'";
            dtDevice = _Mysql.GetDataTable(sql);
            foreach (DataRow drDevice in dtDevice.Rows)
            {
                string fid = drDevice["fid"].ToString();
                sql = "SELECT * FROM sys_devicebase WHERE FatherCode = '" + fid + "'";
                dtChildDevice = _Mysql.GetDataTable(sql);
                DataRow[] dr = dtChildDevice.Select("DeviceType = 131");
                
                foreach (DataRow drVal in dr)
                {
                    //向B中增加行
                    dtChannel.ImportRow(drVal);
                }
            }
            _dtChannel = dtChannel;
            RelationDevcomboBox.DataSource = dtChannel;
            RelationDevcomboBox.DisplayMember = "name";
            RelationDevcomboBox.ValueMember = "id";
        }
        //角色
        private void Auth_Role()
        {
            string sql = string.Empty;
            sql = "SELECT * FROM auth_role";
            _dtRole = _Mysql.GetDataTable(sql);
            RolecomboBox.DataSource = _dtRole;
            RolecomboBox.DisplayMember = "name";
            RolecomboBox.ValueMember = "id";
        }
        private void DataInit()//数据初始化
        {
            DevTypeInit();
            EventTypeInit();
            //GroupInit();
            TimeInit();
            Linkage();
           // RelationDev();
            Auth_Role();
        }
        public static DataTable Paging(DataTable dt, int pageIndex, int pageSize)
        {
            DataTable result = new DataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                result = dt.Clone();
                DataRow[] rows = dt.AsEnumerable().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();
                foreach (DataRow item in rows)
                {
                    result.ImportRow(item);
                }
            }
            return result;
        }

        private void EventData()
        {
            string sql = null;
            DataTable dtDevice = null;

            //查出所有设备
            sql = "SELECT * FROM sys_devicebase WHERE OrgID = '" + _NodeTag + "' AND DeviceType = '" + _DevCode + "'";
            dtDevice = _Mysql.GetDataTable(sql);
            int pageCount = dtDevice.Rows.Count / 20;

            EventprogressBar.Maximum = pageCount + 1;
            EventprogressBar.Value = 0;
            EventprogressBar.Step = 1;
            for(int i = 1;i <= pageCount + 1;i++ )
            {
                DataTable dt = Paging(dtDevice, i, 20);

                Event_DetailLinkPolicy DetailLinkPolicy = new Event_DetailLinkPolicy();
                DetailLinkPolicy.id = Guid.NewGuid().ToString().Replace("-", "");
                DetailLinkPolicy.IsEnable = "1";
                DetailLinkPolicy.PolicyName = _NodeName + "性能测试视频通道报警事件" + i.ToString();
                DetailLinkPolicy.TimeMode = _TimeTemplateID;
                DetailLinkPolicy.UserToRecv = _RoleID;
                sql = string.Format("INSERT INTO event_DetailLinkPolicy (id,IsEnable,PolicyName,TimeMode,UserToRecv) VALUES ('{0}','{1}','{2}','{3}','{4}')", DetailLinkPolicy.id, DetailLinkPolicy.IsEnable, DetailLinkPolicy.PolicyName, DetailLinkPolicy.TimeMode, DetailLinkPolicy.UserToRecv);
                _Mysql.ExcuteNoneQuery(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    Event_SourceDevice SourceDevice = new Event_SourceDevice();
                    SourceDevice.id = Guid.NewGuid().ToString().Replace("-", "");
                    SourceDevice.fid = DetailLinkPolicy.id;
                    SourceDevice.Org_id = _NodeTag;
                    SourceDevice.BelongToModule = "VideoManage";
                    SourceDevice.DeviceId = dr["id"].ToString();
                    SourceDevice.EventType = _EventInfoCode;
                    sql = string.Format("INSERT INTO event_sourcedevice (id,fid,Org_id,BelongToModule,DeviceId,EventType) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')", SourceDevice.id, SourceDevice.fid, SourceDevice.Org_id, SourceDevice.BelongToModule, SourceDevice.DeviceId, SourceDevice.EventType);
                    _Mysql.ExcuteNoneQuery(sql);
                }
                //添加联动模板
                Event_DetailPolicySecLink link = new Event_DetailPolicySecLink()
                {
                    id = Guid.NewGuid().ToString().Replace("-", ""),
                    fid = DetailLinkPolicy.id,
                    HandleType = "6",
                    LinkMode = _StrategyTemplateID
                };
                sql = string.Format("INSERT INTO event_DetailPolicySecLink (id,fid,HandleType,LinkMode) VALUES ('{0}','{1}','{2}','{3}')",link.id,link.fid,link.HandleType,link.LinkMode);
                EventprogressBar.Value += EventprogressBar.Step;//让进度条增加一次
            }
            MessageBox.Show("配置成功");

        }
        private void Eventbutton_Click(object sender, EventArgs e)
        {
            EventData();
        }

        private void DevTypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCurrentIndex = this.DevTypecomboBox.SelectedIndex;
            if (iCurrentIndex < 0)
                return;
            DataRow dr = _dtDevType.Rows[iCurrentIndex];
            _DevCode = dr["Code"].ToString();
        }
        //事件类型
        private void EventTypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCurrentIndex = this.EventTypecomboBox.SelectedIndex;
            if (iCurrentIndex < 0)
                return;
            DataRow dr = _dtEventType.Rows[iCurrentIndex];
            _EventInfoCode = dr["id"].ToString();
        }
        //事件等级
        private void EventGradecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCurrentIndex = this.EventGradecomboBox.SelectedIndex;
            if (iCurrentIndex < 0)
                return;
            DataRow dr = _dtEventGrade.Rows[iCurrentIndex];
            _EventLevelID = dr["id"].ToString();
        }
        //组合类型
        private void GroupcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCurrentIndex = this.GroupcomboBox.SelectedIndex;
            if (iCurrentIndex < 0)
                return;
            DataRow dr = _dtGroup.Rows[iCurrentIndex];
            _CombinationTypeID = dr["id"].ToString();
        }
        //时间模板
        private void TimecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCurrentIndex = this.TimecomboBox.SelectedIndex;
            if (iCurrentIndex < 0)
                return;
            DataRow dr = _dtTime.Rows[iCurrentIndex];
            _TimeTemplateID = dr["id"].ToString();
        }

        private void LinkagecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCurrentIndex = this.LinkagecomboBox.SelectedIndex;
            if (iCurrentIndex < 0)
                return;
            DataRow dr = _dtLinkage.Rows[iCurrentIndex];
            _StrategyTemplateID = dr["id"].ToString();
        }

        private void RelationDevcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCurrentIndex = this.RelationDevcomboBox.SelectedIndex;
            if (iCurrentIndex < 0)
                return;
            DataRow dr = _dtChannel.Rows[iCurrentIndex];
            _DevHID = dr["id"].ToString();
        }

        private void RolecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCurrentIndex = this.RolecomboBox.SelectedIndex;
            if (iCurrentIndex < 0)
                return;
            DataRow dr = _dtRole.Rows[iCurrentIndex];
            _RoleID = dr["id"].ToString();
        }
    }
}
