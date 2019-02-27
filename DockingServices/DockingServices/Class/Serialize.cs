using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace DockingServices
{
    public class Serialize
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="path"></param>
        /// <param name="objectType"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static object ConvertFileToObject(string path, Type objectType, Encoding encoding)
        {
            object convertedObject = null;
            if (!string.IsNullOrEmpty(path))
            {
                XmlSerializer ser = new XmlSerializer(objectType);
                using (StreamReader reader = new StreamReader(path, encoding))
                {
                    convertedObject = ser.Deserialize(reader);
                    reader.Close();
                }
            }
            return convertedObject;
        }

        public static string ObjectToXmlSerializer(Object Obj)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            //去除xml声明
            settings.OmitXmlDeclaration = true;
            settings.Encoding = Encoding.Default;
            settings.Indent = true;//换行缩进
            System.IO.MemoryStream mem = new MemoryStream();
            using (XmlWriter writer = XmlWriter.Create(mem, settings))
            {
                //去除默认命名空间xmlns:xsd和xmlns:xsi
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer formatter = new XmlSerializer(Obj.GetType());
                formatter.Serialize(writer, Obj, ns);
            }
            return Encoding.Default.GetString(mem.ToArray());
        }

        //反序列化
        public static T ObjectToXmlDESerializer<T>(string str) where T : class
        {
            object obj;
            using (System.IO.MemoryStream mem = new MemoryStream(Encoding.Default.GetBytes(str)))
            {
                using (XmlReader reader = XmlReader.Create(mem))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(T));
                    obj = formatter.Deserialize(reader);
                }
            }
            return obj as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static string ReplaceLowOrderASCIICharacters(string tmp)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.AppendFormat("&#x{0:X};", ss);
                else info.Append(cc);
            }
            return info.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetLowOrderASCIICharacters(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            int pos, startIndex = 0, len = input.Length;
            if (len <= 4) return input;
            StringBuilder result = new StringBuilder();
            while ((pos = input.IndexOf("&#x", startIndex)) >= 0)
            {
                bool needReplace = false;
                string rOldV = string.Empty, rNewV = string.Empty;
                int le = (len - pos < 6) ? len - pos : 6;
                int p = input.IndexOf(";", pos, le);
                if (p >= 0)
                {
                    rOldV = input.Substring(pos, p - pos + 1);
                    // 计算 对应的低位字符
                    short ss;
                    if (short.TryParse(rOldV.Substring(3, p - pos - 3), NumberStyles.AllowHexSpecifier, null, out ss))
                    {
                        if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                        {
                            needReplace = true;
                            rNewV = Convert.ToChar(ss).ToString();
                        }
                    }
                    pos = p + 1;
                }
                else pos += le;
                string part = input.Substring(startIndex, pos - startIndex);
                if (needReplace) result.Append(part.Replace(rOldV, rNewV));
                else result.Append(part);
                startIndex = pos;
            }
            result.Append(input.Substring(startIndex));
            return result.ToString();
        }

    }
}

