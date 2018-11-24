using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mysql;

namespace DataMaker.bean
{
    class IPCallDevice : Device
    {
        private static string head;

        private string resId;

        public IPCallDevice()
            : base("call_devicebase_mc")
        {
            //sHead = "CallDevBrand,AccessProtocol";
            //sValue = "'Brand_CALL_MC_HYG3','0'";
            setParam("CallDevBrand", "Brand_CALL_MC_HYG3");
            setParam("AccessProtocol", "0");
            deviceBase.setDeviceMainType("550");
            deviceBase.setDeviceType("554");
        }

        public override string initHead()
        {
            if (head == null)
            {
                head = base.buildSQLHead();
            }
            return head;
        }

        public void setPanelCount(int count)
        {
            List<Device> childs = new List<Device>();
            for (int i = 0; i < count; i++)
            {
                CallPanel panel = new CallPanel();
                panel.setFatherCode(getId());
                panel.setDeviceCode("1" + this.deviceBase.getDeviceCode() + i.ToString());
                panel.setDeviceName("对讲面板" + i.ToString());
                panel.setParam("ResourceID", resId);
                childs.Add(panel);
            }
            childList.Add(childs);
        }

        public void setResourceId(string id)
        {
            resId = id;
            setParam("ResourceID", resId);
        }
    }

    class CallMic : Device
    {
        private static string head;

        public CallMic()
            : base("call_devicebase_mic")
        {
            //sHead = "CallDevBrand,AccessProtocol";
            //sValue = "'Brand_CALL_MC_HYG3','0'";
            setPort("5060");
            setParam("CallDevBrand", "Brand_CALL_MC_HYG3");
            setParam("AccessProtocol", "0");
            setParam("showOrder", "1");
            deviceBase.setDeviceMainType("550");
            deviceBase.setDeviceType("553");
        }

        public override string initHead()
        {
            if (head == null)
            {
                head = base.buildSQLHead();
            }
            return head;
        }
    }

    class CallPanel : Device
    {
        private static string head;

        public CallPanel()
            : base("call_devicebase_panel")
        {
            //sHead = "CallDevBrand,AccessProtocol,PanelType";
            //sValue = "'Brand_CALL_MC_HYG3','0','0'";
            setParam("CallDevBrand", "Brand_CALL_MC_HYG3");
            setParam("AccessProtocol", "0");
            setParam("PanelType", "0");
            deviceBase.setDeviceMainType("550");
            deviceBase.setDeviceType("552");
        }

        public override string initHead()
        {
            if (head == null)
            {
                head = base.buildSQLHead();
            }
            return head;
        }
    }
}
