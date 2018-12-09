using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;

namespace RecogniseCode
{
    public class XMLSourceHelp
    {
        /// <summary>
        /// XML数据文件数据列表
        /// </summary>
        private DataTable m_XmlDataSource = null;

        public XMLSourceHelp(string strXmlPath)
        {
            LoadSource(strXmlPath);
        }
        
        /// <summary>
        /// 功能描述:加载数据
        /// 作　　者:huangzh
        /// 创建日期:2015-10-10 17:48:32
        /// 任务编号:
        /// </summary>
        private void LoadSource(string strXmlPath)
        {
            m_XmlDataSource = LoadXmlInfo(strXmlPath);
        }

        /// <summary>
        /// 转换ID为中文名
        /// </summary>
        /// <param name="objId">Id数字码</param>
        /// <param name="file">文件</param>
        /// <param name="strDefualtValue">默认值</param>
        /// <returns>返回转换后的名字</returns>
        public string ConvertIdToName(object objId,  string strDefualtValue = "")
        {
            if (objId == null || string.IsNullOrWhiteSpace(objId.ToString()))
            {
                return strDefualtValue;
            }
             
            if (m_XmlDataSource == null)
            {
                return strDefualtValue;
            }
            var names = from item in m_XmlDataSource.AsEnumerable()
                        where item.Field<string>("id") == objId.ToString()
                        select item.Field<string>("value");
            if (names == null || names.Count() <= 0)
                return strDefualtValue;
            return names.First();
        }

      

        /// <summary>
        /// 加载XML信息
        /// </summary>
        /// <param name="strPath">文件路径</param>
        /// <returns>返回Xml信息数据表</returns>
        private DataTable LoadXmlInfo(string strPath)
        {
            DataTable dt = CreateDt();
            GetSource(strPath, ref dt);
            return dt;
        }

        /// <summary>
        /// 功能描述:读取数据
        /// 作　　者:huangzh
        /// 创建日期:2015-09-25 09:28:43
        /// 任务编号:
        /// </summary>
        /// <param name="strFile">strFile</param>
        /// <param name="dt">dt</param>
        private void GetSource(string strFile, ref DataTable dt)
        {
            XmlDocument document = new XmlDocument();
            document.Load(strFile);
            XmlNodeList nodelist = document.SelectSingleNode("/source").ChildNodes;
            foreach (XmlNode xn in nodelist)
            {
                if (xn.NodeType == XmlNodeType.Element)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = xn.Attributes["id"].Value;
                    dr[1] = xn.Attributes["value"].Value;
                    dt.Rows.Add(dr);
                }
            }
        }

        /// <summary>
        /// 功能描述:创建一个DataTable
        /// 作　　者:huangzh
        /// 创建日期:2015-09-25 09:28:08
        /// 任务编号:
        /// </summary>
        /// <returns>返回值</returns>
        private DataTable CreateDt()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("value", typeof(string));
            return dt;
        }  
    }
}
