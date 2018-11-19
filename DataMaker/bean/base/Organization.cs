using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMaker.bean
{
    class Organization
    {
        //机构id
        public string id;
        //机构名称
        public string name;
        //机构编号
        public string num;

        public List<string> areaList = new List<string>();
        public List<List<string>> pointList = new List<List<string>>();

        public Organization(bool autoCreate = true)
        {
            if (autoCreate)
            {
                id = StringUtil.createRandomGUID();
            }
        }

        public Organization(string id,bool autoQuery = false)
        {
            this.id = id;
            if (autoQuery)
            {
                //此处查找数据库获取name和num
            }
        }

        public Organization(string id, string name, string num)
        {
            this.id = id;
            this.name = name;
            this.num = num;
        }
    }
}
