using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace DockingServices
{
    public class CommonFunc
    {
        /// <summary>
        /// 以UTF-8无BOM编码保存xml至文件。
        /// </summary>
        /// <param name="savePath">保存至路径</param>
        /// <param name="xml"></param>
        public void SaveXmlWithUTF8NotBOM(XmlDocument xml, string savePath)
        {
            using (StreamWriter sw = new StreamWriter(savePath, false, new UTF8Encoding(false)))
            {
                xml.Save(sw);
                sw.WriteLine();
                sw.Close();
            }
        }
    }
}
