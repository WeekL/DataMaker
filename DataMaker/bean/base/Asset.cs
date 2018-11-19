using System;
using Mysql;
using DataMaker.util;

namespace DataMaker.bean
{
    class Asset : Table
    {
        private static string head;

        //所属机构
        private Organization org;
        //公有附属表
        public AssetState state;

        public Asset()
            : base("zichangongyoubiao")
        {
            org = QuickLoadUtil.curOrg;
            state = new AssetState(getId());
            //sHead = "zichandalei,zichanxiaolei,shuliang,suoshujigoumingchen,suoshujigoubianhao";
            //sValue = "'cc3fe646d94e47a18889493165a237da','4ace977665014e3a9242b559b0848982','1','" + org.id + "','" + org.num + "'";
            setParam("zichandalei", "cc3fe646d94e47a18889493165a237da");
            setParam("zichanxiaolei", "4ace977665014e3a9242b559b0848982");
            setParam("shuliang", "1");
            setParam("suoshujigoumingchen", org.id);
            setParam("suoshujigoubianhao", org.num);
        }

        public override string initHead()
        {
            if (head == null)
            {
                head = base.buildSQLHead();
            }
            return head;
        }

        public void setName(string name)
        {
            setParam("zichanmingcheng", name);
        }

        public void setNum(string num)
        {
            setParam("zichanbianhao", num);
            state.setCode(num);
        }

        public new void excute(MySQLDBHelper db)
        {
            db.ExcuteNoneQuery(buildSqlStr() + ";" + state.buildSqlStr());
            Console.WriteLine("====================创建资产及公有附属表数据：资产id = " + getId());
        }
    }

    class AssetState : Table
    {
        private static string head;
        public AssetState(string fid) : base("gongyoufushubiao")
        {
            //sHead = "zichanzhiliangzhuangtai,zichankucunzhuangtai,fid";
            //sValue = "'35010','0','" + fid + "'";
            setParam("fid", fid);
            setParam("zichanzhiliangzhuangtai", "35010");
            setParam("zichankucunzhuangtai", "0");
        }

        public override string initHead()
        {
            if (head == null)
            {
                head = base.buildSQLHead();
            }
            return head;
        }

        public void setCode(string code)
        {
            setParam("shebeiweiyibiaoshima", code);
        }
    }
}
