using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DockingServices.AudioMessage.SendMQ
{
   public class MqSendOrder
    {
        //MQ指令集合
        private List<Property> m_lstProperty = new List<Property>();
        public MQ m_mq;
        public readonly static MqSendOrder sendOrder = new MqSendOrder();
        private IniFiles serverini;
        public MqSendOrder()
        {
            m_mq = new MQ();
            serverini = new IniFiles(@Application.StartupPath + "\\Config.ini");
            m_mq = new MQ();
            m_mq.uri = serverini.ReadValue("MQActiveOrder", "ServerUrl"); 
            m_mq.username = serverini.ReadValue("MQActiveOrder", "User"); 
            m_mq.password = serverini.ReadValue("MQActiveOrder", "Password");
            m_mq.Start();
            Thread.Sleep(500);
            m_mq.CreateProducer(true, "fee.bar");
        }
        public bool SendMq(EBD ebd,int Type, string ParamValue, string TsCmd_ID, string TsCmd_ValueID)
        {
            if (ebd != null)
            {
                string InfoValueStr = "insert into InfoVlaue values('" + ebd.EBDID + "')";
                mainForm.dba.UpdateDbBySQL(InfoValueStr);
            }
            m_lstProperty = Install(Type, ParamValue, TsCmd_ID, TsCmd_ValueID);
            return m_mq.SendMQMessage(true, "Send", m_lstProperty);
        }
        private List<Property> Install(int Type, string value, string TsCmd_ID, string TsCmd_ValueID)
        {
            
            List<Property> InstallList = new List<Property>();
            Property item = new Property();
            item.name = "TsCmd_Mode";
            item.value = "区域";
            InstallList.Add(item);

            Property itemTime = new Property(); ;
            itemTime.name = "TsCmd_Date";
            itemTime.value = DateTime.Now.AddSeconds(2).ToString("yyyy-MM-dd HH:mm:ss");
            InstallList.Add(itemTime);

            Property itemStatus = new Property();
            itemStatus.name = "TsCmd_Status";
            itemStatus.value = "0";
            InstallList.Add(itemStatus);

            Property itemVoice = new Property();
            itemVoice.name = "VOICE";
            itemVoice.value = "3";
            InstallList.Add(itemVoice);

            Property itemTsCmd_ID = new Property();
            itemTsCmd_ID.name = "TsCmd_ID";
            itemTsCmd_ID.value = TsCmd_ID;
            InstallList.Add(itemTsCmd_ID);

            Property itemTsCmd_ValueID = new Property();
            itemTsCmd_ValueID.name = "TsCmd_ValueID";
            itemTsCmd_ValueID.value = TsCmd_ValueID;
            InstallList.Add(itemTsCmd_ValueID);
            // TsCmd_ValueID = "1"
            Type t = ServerForm.MQUserInfo.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (var PropertyInfo in PropertyList)
            {
                Property userinfo = new Property();
                userinfo.name = PropertyInfo.Name;
                object valueobj = PropertyInfo.GetValue(ServerForm.MQUserInfo, null);
                userinfo.value = valueobj == null ? "" : valueobj.ToString();
                InstallList.Add(userinfo);
            }
            string strOrder = "";


            if (Type == 1)//音频文件播发
            {
                Property itemType = new Property();
                itemType.name = "TsCmd_Type";
                itemType.value = "播放视频GB";
                InstallList.Add(itemType);

                Property itemTsCmd_PlayCount = new Property();    //2018-05-23
                itemTsCmd_PlayCount.name = "TsCmd_PlayCount";
                itemTsCmd_PlayCount.value = "2";
                InstallList.Add(itemTsCmd_PlayCount);
            }
            else
            {
                Property itemType = new Property();
                itemType.name = "TsCmd_Type";
                itemType.value = "TTS播放";
                InstallList.Add(itemType);
                Property itemTsCmd_PlayCount = new Property();    //2018-05-23
                itemTsCmd_PlayCount.name = "TsCmd_PlayCount";
                itemTsCmd_PlayCount.value = "10";
                InstallList.Add(itemTsCmd_PlayCount);
            }

            Property itemTsCmd_Params = new Property();
            itemTsCmd_Params.name = "TsCmd_Params";
            itemTsCmd_Params.value = value;
            InstallList.Add(itemTsCmd_Params);

            //打印MQ指令
            foreach (var Property in InstallList)
            {
                strOrder += Property.name + "  " + Property.value + Environment.NewLine;

            }
            Console.WriteLine(strOrder);
            return InstallList;
        }
        //指令MQ初始化
        private void MQActivStart()
        {
            m_mq = new MQ();
            m_mq.uri = serverini.ReadValue("MQActiveOrder", "ServerUrl"); ;
            m_mq.username = serverini.ReadValue("MQActiveOrder", "User"); ;
            m_mq.password = serverini.ReadValue("MQActiveOrder", "Password");
            m_mq.Start();
            Thread.Sleep(500);
            m_mq.CreateProducer(true, "fee.bar");
            //Property ite = new Property();
            //ite.name = "你是猪吗?";
            //ite.value = "这个都不会";
            //m_lstProperty.Add(ite);
            //m_mq.SendMQMessage(true, "Send", m_lstProperty);
        }
    }
}
