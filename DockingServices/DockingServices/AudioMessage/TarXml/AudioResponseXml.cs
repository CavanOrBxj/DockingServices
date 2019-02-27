using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DockingServices.AudioMessage.TarXml
{
    public class AudioResponseXml
    {
        public static void CreateXML(XmlDocument XD, string Path)
        {
            CommonFunc ComX = new CommonFunc();
            ComX.SaveXmlWithUTF8NotBOM(XD, Path);
            if (ComX != null)
            {
                ComX = null;
            }
        }
    }
}
