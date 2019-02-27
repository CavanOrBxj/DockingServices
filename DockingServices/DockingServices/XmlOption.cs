using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GRPlatForm
{
    class XmlOption
    {
        XmlDocument xmlDoc;

        /// <summary>
        /// 显示xml全部数据
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="firstNode">根节点</param>
        /// <returns>xml全部数据</returns>
        public List<string> ShowXml(string fileName, string firstNode)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName); //加载xml文件
            XmlNode xn = xmlDoc.SelectSingleNode(firstNode);
            List<string> listxml = new List<string>();

            XmlNodeList xnl = xn.ChildNodes;

            foreach (XmlNode xnf in xnl)
            {
                XmlElement xe = (XmlElement)xnf;

                XmlNodeList xnf1 = xe.ChildNodes;
                foreach (XmlNode xn2 in xnf1)
                {
                    listxml.Add(xn2.InnerText);  //显示子节点点文本  
                }
            }

            return listxml;
        }

        /// <summary>
        /// 显示XML节点信息
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="parentNode">节点(例："EBD//EBDVersion")</param>
        /// <returns></returns>
        public string ShowXmlNode(string fileName, string parentNode)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName); //加载xml文件
            XmlNode xn = xmlDoc.SelectSingleNode(parentNode);
            return xn.InnerText;
        }
    }
}
