using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataMaker
{
    public class ModelConvertHelper<T> where T : new()
    {    
        public static List<T> FillModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> modelList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T model = (T)Activator.CreateInstance(typeof(T));  
                //T model = new T();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                    if (propertyInfo != null && dr[i] != DBNull.Value)
                        propertyInfo.SetValue(model, dr[i], null);
                }

                modelList.Add(model);
            }
            return modelList;
        }
         public static IList<T> ConvertToModel(DataTable dt)    
         {    
            // 定义集合    
             IList<T> ts = new List<T>(); 
     
            // 获得此模型的类型   
             Type type = typeof(T);      
            string tempName = "";      
      
            foreach (DataRow dr in dt.Rows)      
             {    
                 T t = new T();     
                // 获得此模型的公共属性      
                 PropertyInfo[] propertys = t.GetType().GetProperties(); 
                foreach (PropertyInfo pi in propertys)      
                 {      
                     tempName = pi.Name;  // 检查DataTable是否包含此列    
   
                    if (dt.Columns.Contains(tempName))      
                     {      
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;         
   
                        object value = dr[tempName];      
                        if (value != DBNull.Value)      
                             pi.SetValue(t, value, null);  
                     }     
                 }      
                 ts.Add(t);      
             }     
            return ts;     
         }     
     }    
}
