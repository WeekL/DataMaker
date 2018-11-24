using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mysql;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LoadRunner.DotNet;
using DataMaker.util;
namespace DataMaker
{
    public partial class DataMakerFm : Form
    {
        private MySQLDBHelper _Mysql = null;
        private string _NodeName = null;
        private string _NodeTag = null;
        private string _NodeText = string.Empty;
        public DataMakerFm()
        {
            InitializeComponent();
        }
        public DataMakerFm(MySQLDBHelper Mysql)
        {
            try
            {
                //string a = GetClientLocalIPv4Address();
                _Mysql = Mysql;
                InitializeComponent();
                OrgtreeView.Nodes.Clear();
                OrgbindTreeView1();
            }
            catch(Exception)
            {

            }
        }
        private string GetClientLocalIPv4Address()
        {
            string strLocalIP = string.Empty;
            try
            {
                IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHost.AddressList[0];
                strLocalIP = ipAddress.ToString();
                return strLocalIP;
            }
            catch
            {
                return "unknown";
            }
        } 
        /// <summary>  
        /// 获取本机的物理地址  
        /// </summary>
        private string GetMacAddr()
        {
            string madAddr = null;
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc2 = mc.GetInstances();
                foreach (ManagementObject mo in moc2)
                {
                    if (Convert.ToBoolean(mo["IPEnabled"]) == true)
                    {
                        madAddr = mo["MacAddress"].ToString();
                        madAddr = madAddr.Replace(':', '-');
                    }
                    mo.Dispose();
                }
                if (madAddr == null)
                {
                    return "unknown";
                }
                else
                {
                    return madAddr;
                }
            }
            catch (Exception)
            {
                return "unknown";
            }
        }  
        //设置树单选,就是只能有一个树节点被选中
        private void SetNodeCheckStatus(TreeNode tn, TreeNode node)
        {
            if (tn == null)
                return;
            if (tn != node)
            {
                tn.Checked = false;
            }
            // Check children nodes
            foreach (TreeNode tnChild in tn.Nodes)
            {
                if (tnChild != node)
                {
                    tnChild.Checked = false;
                }
                SetNodeCheckStatus(tnChild, node);
            }
        }
        //在树节点被选中后触发
        private void treeView1_AfterCheacked(object sender, TreeViewEventArgs e)
        {
            //过滤不是鼠标选中的其它事件，防止死循环
            if (e.Action != TreeViewAction.Unknown)
            {
                //Event call by mouse or key-press
                foreach (TreeNode tnChild in OrgtreeView.Nodes)
                    SetNodeCheckStatus(tnChild, e.Node);
                string sName = e.Node.Text;
            }
        }
        //获得选择节点
        private void GetSelectNode(TreeNode tn)
        {
            if (tn == null)
                return;
            if (tn.Checked == true)
            {
                _NodeName = tn.Text;
                return;
            }
            // Check children nodes
            foreach (TreeNode tnChild in tn.Nodes)
            {
                GetSelectNode(tnChild);
            }
        }
        //选择树的节点并点击右键，触发事件
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//判断你点的是不是右键
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = OrgtreeView.GetNodeAt(ClickPoint);
                if (CurrentNode != null)//判断你点的是不是一个节点
                {
                    switch (CurrentNode.Name)//根据不同节点显示不同的右键菜单，当然你可以让它显示一样的菜单
                    {
                        case "":
                            _NodeTag = CurrentNode.Tag.ToString();CurrentNode.ContextMenuStrip = contextMenuStrip1;
                            _NodeText = CurrentNode.Text;
                            _NodeName = _NodeText;
                            break;
                        default:
                            break;
                    }
                    OrgtreeView.SelectedNode = CurrentNode;//选中这个节点
                }
            }
        }
        public void GetAllNodeText(TreeNodeCollection tnc, DataTable dt)
        {
            foreach (TreeNode node in tnc)
            {
                DataRow[] drr = dt.Select("FatherCode is null");
                if (drr.Length > 0)
                {
                    for (int i = 0; i < drr.Length; i++)
                    {
                        TreeNode tnn = new TreeNode();
                        tnn.Text = drr[i]["name"].ToString();
                        tnn.Tag = drr[i]["id"].ToString();
                        if (drr[i]["OrgID"].ToString() == node.Tag.ToString())
                        {
                            node.Nodes.Add(tnn);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (node.Nodes.Count != 0)
                {
                    GetAllNodeText(node.Nodes, dt);
                }
            }
        }
        private void DevbindTreeView1()
        {
            string mysql = "select * from sys_devicebase";
            DataTable dt = _Mysql.GetDataTable(mysql);
            DataRow[] dr = dt.Select("FatherCode is null");
            for (int i = 0; i < dr.Length; i++)
            {
                TreeNode tn = new TreeNode();
                tn.Text = dr[i]["name"].ToString();
                tn.Tag = dr[i]["id"].ToString();

                OrgFillTree(tn, dt);

                OrgtreeView.Nodes.Add(tn);
            }
        
        }
        private void DevFillTree(TreeNode node, DataTable dt)
        {

        }


        //列出一级机构
        private void OrgbindTreeView1()
        {
            string sql = "select * from org_dept";
            DataTable dt = _Mysql.GetDataTable(sql);
            DataRow[] dr = dt.Select("pid is null");
            for (int i = 0; i < dr.Length; i++)
            {
                TreeNode tn = new TreeNode();
                tn.Text = dr[i]["name"].ToString();
                tn.Tag = dr[i]["id"].ToString();
                OrgFillTree(tn, dt);
                OrgtreeView.Nodes.Add(tn);
            }
        }
        //列出所有下级机构
        private void OrgFillTree(TreeNode node, DataTable dt)
        {
            DataRow[] drr = dt.Select("pid='" + node.Tag.ToString() + "'");
            if (drr.Length > 0)
            {
                for (int i = 0; i < drr.Length; i++)
                {
                    TreeNode tnn = new TreeNode();
                    tnn.Text = drr[i]["name"].ToString();
                    tnn.Tag = drr[i]["id"].ToString();
                    if (drr[i]["pid"].ToString() == node.Tag.ToString())
                    {
                        OrgFillTree(tnn, dt);
                    }
                    node.Nodes.Add(tnn);
                }
            }
        }


        //
        //按钮事件
        //

        //刷新机构树
        private void Connectbutton_Click(object sender, EventArgs e)
        {
            OrgtreeView.Nodes.Clear();
            OrgbindTreeView1();
           // DevbindTreeView1();
        }

        private void 添加组织机构ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddOrgFm fm = new AddOrgFm(_NodeTag, _Mysql);
            fm.ShowDialog();
        }
        private void 删除组织机构ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count = 0;
            string mysql = "SELECT COUNT(*) FROM org_dept WHERE pid='" + _NodeTag + "'";
            count = _Mysql.ExcuteNoneQuery(mysql);
            if(0 == count)
            {
                mysql = "DELETE FROM org_dept WHERE id='" + _NodeTag + "'";
                count = _Mysql.ExcuteNoneQuery(mysql);
                TreeNode node = OrgtreeView.SelectedNode;
                node.Remove();
            }
            else
            {
                MessageBox.Show("该节点下有子节点无法删除");
            }
        }

        private void 添加视频主机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddVideoDevFm fm = new AddVideoDevFm(_NodeTag,_Mysql);
            QuickLoadUtil.attach(_Mysql, _NodeTag);//预加载机构相关信息
            fm.Show();
            //fm.ShowDialog();
        }

        private void 添加报警主机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddAlmDevFm fm = new AddAlmDevFm(_NodeTag, _Mysql);
            QuickLoadUtil.attach(_Mysql, _NodeTag);//预加载机构相关信息
            fm.Show();
            //fm.ShowDialog();
        }

        private void 添加人员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPersonnel fm = new AddPersonnel(_NodeTag, _Mysql);
            QuickLoadUtil.attach(_Mysql, _NodeTag);//预加载机构相关信息
            fm.Show();
            //fm.ShowDialog();
        }

        private void 添加门禁控制器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddACSDevFm fm = new AddACSDevFm(_NodeTag, _Mysql);
            QuickLoadAcs.attach(_Mysql, _NodeTag);//预加载机构相关信息
            fm.Show();
            //fm.ShowDialog();
        }

        private void 删除人员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mysql = "SELECT * FROM hr_member WHERE OrgId='" + _NodeTag + "'";
            DataTable dt = _Mysql.GetDataTable(mysql);
            List<string> lstId = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                lstId.Add(dr["id"].ToString());
            } 
        }

        private void OrgtoolStripButton_Click(object sender, EventArgs e)
        {
            AddOrgFm fm = new AddOrgFm(_NodeTag, _Mysql);
            fm.ShowDialog();
        }
        public List<string> CheckedNodes(TreeNode parent, List<string> checkednodes)
        {

            TreeNode node = parent;
            if (node != null)
            {
                if (node.Checked == true && node.FirstNode == null)
                    checkednodes.Add(node.Text);

                if (node.FirstNode != null)////如果node节点还有子节点则进入遍历
                {
                    CheckedNodes(node.FirstNode, checkednodes);
                }
                if (node.NextNode != null)////如果node节点后面有同级节点则进入遍历
                {
                    CheckedNodes(node.NextNode, checkednodes);
                }
            }

            return checkednodes;
        }
        private void PrintTreeViewNode(TreeNodeCollection node)
        {
            foreach (TreeNode n in node)
            {
                string mysql = "SELECT * FROM sys_devicebase WHERE OrgId='" + _NodeTag + "' AND FatherCode is NULL AND DeviceType='111'";
                DataTable dt = _Mysql.GetDataTable(mysql);
                if(dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        n.Nodes.Add(dr["name"].ToString());
                    } 
                }
                PrintTreeViewNode(n.Nodes);
            }
        }
        private void VideoDevtoolStripButton_Click(object sender, EventArgs e)
        {
            //PrintTreeViewNode(OrgtreeView.Nodes);
            //MessageBox.Show("遍历完成");
        }

        private void 添加ATM预警主机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddIASDevFm fm = new AddIASDevFm(_NodeTag, _Mysql);
            QuickLoadUtil.attach(_Mysql, _NodeTag);//预加载机构相关信息
            fm.Show();
            //fm.ShowDialog();
        }

        private void 添加IP对讲主机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddIpDev fm = new AddIpDev(_NodeTag, _Mysql);
            QuickLoadUtil.attach(_Mysql, _NodeTag);//预加载机构相关信息
            fm.Show();
            //fm.ShowDialog();
        }

        private void 添加IP对讲寻呼话筒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddIpmic fm = new AddIpmic(_NodeTag, _Mysql);
            QuickLoadUtil.attach(_Mysql, _NodeTag);//预加载机构相关信息
            fm.ShowDialog();
        }

        private void 删除报警主机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string mysql = "DELETE FROM sys_devicebase WHERE OrgID = '" + _NodeTag + "' and DeviceType = '511'";
            //_Mysql.ExcuteNoneQuery(mysql);
        }

        private void 删除视频主机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string mysql = "DELETE FROM sys_devicebase WHERE OrgID = '" + _NodeTag + "' and DeviceType = '131'";
            //_Mysql.ExcuteNoneQuery(mysql);
            //mysql = "DELETE FROM sys_devicebase WHERE OrgID = '" + _NodeTag + "' and DeviceType = '111'";
            //_Mysql.ExcuteNoneQuery(mysql);
        }

        private void 删除ATM预警主机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string mysql = "DELETE FROM sys_devicebase WHERE OrgID = '" + _NodeTag + "' and DeviceType = '531'";
            //_Mysql.ExcuteNoneQuery(mysql);
        }

        private void 删除门禁控制器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string mysql = "DELETE FROM sys_devicebase WHERE OrgID = '" + _NodeTag + "' and DeviceType = '581'";
            //_Mysql.ExcuteNoneQuery(mysql);
        }

        private void EventConfigtoolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void 事件策略配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventFm fm = new EventFm(_NodeTag, _NodeName, _Mysql);
            QuickLoadUtil.attach(_Mysql, _NodeTag);//预加载机构相关信息
            fm.Show();
        }

        private void 添加级联信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CascadeFm fm = new CascadeFm(_Mysql);
            fm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CascadeFm fm = new CascadeFm(_Mysql);
            fm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            iot_storageFm fm = new iot_storageFm(_Mysql);
            fm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void DataMakerFm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _Mysql.Close();
            QuickLoadUtil.detach();
        }
    }
}
