using DockingServices.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DockingServices
{
    public class responseXML
    {
        private IniFiles serverini = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\Config.ini");
        public string SourceAreaCode = "";
        public string SourceType = "";
        public string SourceName = "";
        public string SourceID = "";
        public string sHBRONO = "0000000000000";//"010332132300000001";//实体编号

        //通用反馈的xml头
        public int xmlHead(XmlDocument xmlDoc, XmlElement xmlElem, EBD ebdsr, string EBDstyle, string strebdid)
        {
            #region 标准头部

            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = strebdid;
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = EBDstyle;
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");//修改于20181210
            xmlSRCAreaCode.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;// 
            xmlSRC.AppendChild(xmlSRCAreaCode);

            XmlElement xmlSRCAreaCodeURL = xmlDoc.CreateElement("URL");//修改于20181210
            xmlSRCAreaCodeURL.InnerText = SingletonInfo.GetInstance().CurrentURL;// 
            xmlSRC.AppendChild(xmlSRCAreaCodeURL);



            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End
            return 0;
        }

        /// <summary>
        /// 通用反馈
        /// </summary>
        /// <returns></returns>
        public XmlDocument EBDResponse(EBD ebdsr, string EBDstyle, string strEBDID,string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            xmlHead(xmlDoc, xmlElem, ebdsr, EBDstyle, strEBDID);

            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");//
            xmlReEBDID.InnerText = ebdsr.EBDID;
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            XmlElement xmlEBDResponse = xmlDoc.CreateElement("EBDResponse");
            xmlElem.AppendChild(xmlEBDResponse);

            string ResultCodetmp = "";
            string ResultDesctmp = "";

            switch (value)
            {
                case "0":
                    ResultCodetmp = "0";
                    ResultDesctmp = "收到数据未处理";
                    break;
                case "1":
                    ResultCodetmp = "1";
                    ResultDesctmp = "接收解析及数据校验成功";
                    break;
                case "2":
                    ResultCodetmp = "2";
                    ResultDesctmp = "接收解析失败";
                    break;
                case "3":
                    ResultCodetmp = "3";
                    ResultDesctmp = "数据内容缺失";
                    break;
                case "4":
                    ResultCodetmp = "4";
                    ResultDesctmp = "签名验证失败";
                    break;
                case "5":
                    ResultCodetmp = "5";
                    ResultDesctmp = "其他错误";
                    break;
            }

            XmlElement xmlResultCode = xmlDoc.CreateElement("ResultCode");
            xmlResultCode.InnerText = ResultCodetmp;
            xmlEBDResponse.AppendChild(xmlResultCode);

            XmlElement xmlResultDesc = xmlDoc.CreateElement("ResultDesc");
            xmlResultDesc.InnerText = ResultDesctmp;
            xmlEBDResponse.AppendChild(xmlResultDesc);

            return xmlDoc;
        }

        /// <summary>
        /// 播发状态反馈  20181213
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <param name="strebdid"></param>
        /// <param name="BrdStateDesc"></param>
        /// <param name="BrdStateCode"></param>
        /// <returns></returns>
        public XmlDocument EBMStateRequestResponse(EBD ebdsr,string ebdidtmp,string BrdStateCode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            #region 标准头部
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);


            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);


            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = ebdidtmp;//
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBMStateResponse";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCEBRID = xmlDoc.CreateElement("EBRID");
            xmlSRCEBRID.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;
            xmlSRC.AppendChild(xmlSRCEBRID);

            XmlElement xmlSRCURL = xmlDoc.CreateElement("URL");
            xmlSRCURL.InnerText = SingletonInfo.GetInstance().CurrentURL;
            xmlSRC.AppendChild(xmlSRCURL);


            //EBDTime
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End


            //RelatedEBD
            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);
            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlReEBDID.InnerText = ebdsr.EBDID.ToString();
            xmlRelatedEBD.AppendChild(xmlReEBDID);



            //EBMStateResponse
            XmlElement xmlEBMStateResponse = xmlDoc.CreateElement("EBMStateResponse");
            xmlElem.AppendChild(xmlEBMStateResponse);


            //RptTime
            XmlElement xmlEBMStateResponseRptTime = xmlDoc.CreateElement("RptTime");
            xmlEBMStateResponseRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
            xmlEBMStateResponse.AppendChild(xmlEBMStateResponseRptTime);


            //EBMID
            XmlElement xmlEBMStateResponseEBM = xmlDoc.CreateElement("EBM");
           // xmlEBMStateResponseRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
            xmlEBMStateResponse.AppendChild(xmlEBMStateResponseEBM);
            XmlElement xmlEBMStateResponseEBMEBMID = xmlDoc.CreateElement("EBMID");
            xmlEBMStateResponseEBMEBMID.InnerText = ebdsr.EBM.EBMID; 
            xmlEBMStateResponseEBM.AppendChild(xmlEBMStateResponseEBMEBMID);

            //BrdStateCode
            XmlElement xmlEBMStateResponseBrdStateCode = xmlDoc.CreateElement("BrdStateCode");
            xmlEBMStateResponseBrdStateCode.InnerText = BrdStateCode;
            xmlEBMStateResponse.AppendChild(xmlEBMStateResponseBrdStateCode);

            //BrdStateDesc

            string BrdStateDesc = "";
            switch (BrdStateCode)
            {
                case "0":
                    BrdStateDesc = "未处理";
                    break;
                case "1":
                    BrdStateDesc = "等待播发";
                    break;
                case "2":
                    BrdStateDesc = "播发中";
                    break;
                case "3":
                    BrdStateDesc = "播发成功";
                    break;
                case "4":
                    BrdStateDesc = "播发失败";
                    break;
                case "5":
                    BrdStateDesc = "播发取消";
                    break;

            }
            XmlElement xmlEBMStateResponseBrdStateDesc = xmlDoc.CreateElement("BrdStateDesc");
            xmlEBMStateResponseBrdStateDesc.InnerText = BrdStateDesc;
            xmlEBMStateResponse.AppendChild(xmlEBMStateResponseBrdStateDesc);

            //ResBrdInfo
            XmlElement xmlEBMStateResponseResBrdInfo = xmlDoc.CreateElement("ResBrdInfo");
            xmlEBMStateResponse.AppendChild(xmlEBMStateResponseResBrdInfo);

            XmlElement xmlEBMStateResponseResBrdInfoResBrdItem = xmlDoc.CreateElement("ResBrdItem");
            xmlEBMStateResponseResBrdInfo.AppendChild(xmlEBMStateResponseResBrdInfoResBrdItem);


            XmlElement xmlEBMStateResponseResBrdInfoResBrdItemEBRPS = xmlDoc.CreateElement("EBRPS");
            xmlEBMStateResponseResBrdInfoResBrdItem.AppendChild(xmlEBMStateResponseResBrdInfoResBrdItemEBRPS);

            XmlElement xmlEBMStateResponseResBrdInfoResBrdItemEBRPSEBRID = xmlDoc.CreateElement("EBRID");
            xmlEBMStateResponseResBrdInfoResBrdItemEBRPSEBRID.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;
            xmlEBMStateResponseResBrdInfoResBrdItemEBRPS.AppendChild(xmlEBMStateResponseResBrdInfoResBrdItemEBRPSEBRID);
            return xmlDoc;
        }


        public string XmlSerialize<T>(T obj)
        {
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    Type t = obj.GetType();
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(sw, obj);
                    sw.Close();
                    string[] array = sw.ToString().Split('\n');
                    string xmlString = "<?xml version='1.0' encoding='utf-8' standalone='yes'?>" + '\n';

                    for (int i = 1; i < array.Length; i++)
                    {
                        xmlString += array[i] + '\n';
                    }

                    return xmlString;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 心跳包 组包
        /// </summary>
        /// <returns></returns>
        public XmlDocument HeartBeatResponse()
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);

            #region 标准头部

            //暂时先注释掉  20181210
            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            //xmlEBDID.InnerText = "01" + sHBRONO + "0000000000000000";

            xmlEBDID.InnerText = "01" + SingletonInfo.GetInstance().CurrentResourcecode + "0000000000000000";
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "ConnectionCheck";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);
            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;
            xmlSRC.AppendChild(xmlSRCAreaCode);


            //暂时先注释掉  20181210
            //XmlElement xmlDEST = xmlDoc.CreateElement("DEST");
            //xmlElem.AppendChild(xmlDEST);

            //XmlElement eebtEE = xmlDoc.CreateElement("EBRID");
            //eebtEE.InnerText = "010232000000000001";// "010334152300000002";// ebdsr.SRC.EBEID;
            //xmlDEST.AppendChild(eebtEE);

            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);

            //暂时先注释掉  20181210
            //XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            //xmlElem.AppendChild(xmlRelatedEBD);
            #endregion End

            XmlElement xmlEBDResponse = xmlDoc.CreateElement("ConnectionCheck");
            xmlElem.AppendChild(xmlEBDResponse);

            XmlElement xmlResultCode = xmlDoc.CreateElement("RptTime");
            xmlResultCode.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlEBDResponse.AppendChild(xmlResultCode);

            return xmlDoc;
        }

        /// <summary>
        /// 实时流
        /// </summary>
        /// <returns></returns>
        public XmlDocument EBMStreamResponse(string strEBMID, string strUrl)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);

            //xmlHead(xmlDoc, xmlElem, ebdsr, EBDstyle);

            #region 标准头部

            //暂时先注释掉  20181210
            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = "01" + sHBRONO + "0000000000000000";
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBMStreamPortRequest";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode.InnerText = sHBRONO;
            xmlSRC.AppendChild(xmlSRCAreaCode);
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End

            XmlElement xmlDevice = xmlDoc.CreateElement("EBMStream");
            xmlElem.AppendChild(xmlDevice);

            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBM");
            xmlDevice.AppendChild(xmlRelatedEBD);
            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBMID");
            xmlReEBDID.InnerText = strEBMID;//与EBDID一致就用这个写
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            XmlElement xmlParams = xmlDoc.CreateElement("Params");
            xmlDevice.AppendChild(xmlParams);
            XmlElement xmlUrl = xmlDoc.CreateElement("Url");
            xmlUrl.InnerText = strUrl;//与EBDID一致就用这个写
            xmlParams.AppendChild(xmlUrl);

            return xmlDoc;
        }

        /// <summary>
        /// 平台播发记录数据数据  20181214
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument PlatformEBMBrdLog(EBD ebdsr, List<EBM> EBMList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            #region 标准头部
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);


            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);


            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = ebdsr.EBDID;//
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBMBrdLog";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCEBRID = xmlDoc.CreateElement("EBRID");
            xmlSRCEBRID.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;
            xmlSRC.AppendChild(xmlSRCEBRID);

            XmlElement xmlSRCURL = xmlDoc.CreateElement("URL");
            xmlSRCURL.InnerText = SingletonInfo.GetInstance().CurrentURL;
            xmlSRC.AppendChild(xmlSRCURL);


            //EBDTime
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End
            //RelatedEBD
            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);
            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlReEBDID.InnerText = ebdsr.EBDID.ToString();
            xmlRelatedEBD.AppendChild(xmlReEBDID);



            //EBMBrdLog
            XmlElement xmlEBMBrdLog = xmlDoc.CreateElement("EBMBrdLog");
            xmlElem.AppendChild(xmlEBMBrdLog);
            foreach (EBM item in EBMList)
            {
                XmlElement xmlEBMBrdItem = xmlDoc.CreateElement("EBMBrdItem");
                xmlEBMBrdLog.AppendChild(xmlEBMBrdItem);

                XmlElement xmlEBMBrdItemEBM = xmlDoc.CreateElement("EBM");
                xmlEBMBrdItem.AppendChild(xmlEBMBrdItemEBM);

                //EBMVersion
                XmlElement xmlEBMBrdItemEBMEBMVersion = xmlDoc.CreateElement("EBMVersion");
                xmlEBMBrdItemEBMEBMVersion.InnerText = item.EBMVersion;
                xmlEBMBrdItemEBM.AppendChild(xmlEBMBrdItemEBMEBMVersion);

                //EBMID
                XmlElement xmlEBMBrdItemEBMEBMID = xmlDoc.CreateElement("EBMID");
                xmlEBMBrdItemEBMEBMID.InnerText = item.EBMID;
                xmlEBMBrdItemEBM.AppendChild(xmlEBMBrdItemEBMEBMID);

                //MsgBasicInfo
                XmlElement xmlEBMBrdItemEBMMsgBasicInfo = xmlDoc.CreateElement("MsgBasicInfo");
                xmlEBMBrdItemEBM.AppendChild(xmlEBMBrdItemEBMMsgBasicInfo);

                //MsgType
                XmlElement xmlEBMBrdItemEBMMsgBasicInfoMsgType = xmlDoc.CreateElement("MsgType");
                xmlEBMBrdItemEBMMsgBasicInfoMsgType.InnerText = item.MsgBasicInfo.MsgType;
                xmlEBMBrdItemEBMMsgBasicInfo.AppendChild(xmlEBMBrdItemEBMMsgBasicInfoMsgType);


                //SenderName
                XmlElement xmlEBMBrdItemEBMMsgBasicInfoSenderName = xmlDoc.CreateElement("SenderName");
                xmlEBMBrdItemEBMMsgBasicInfoSenderName.InnerText = item.MsgBasicInfo.SenderName;
                xmlEBMBrdItemEBMMsgBasicInfo.AppendChild(xmlEBMBrdItemEBMMsgBasicInfoSenderName);

                //SenderCode
                XmlElement xmlEBMBrdItemEBMMsgBasicInfoSenderCode = xmlDoc.CreateElement("SenderCode");
                xmlEBMBrdItemEBMMsgBasicInfoSenderCode.InnerText = item.MsgBasicInfo.SenderCode;
                xmlEBMBrdItemEBMMsgBasicInfo.AppendChild(xmlEBMBrdItemEBMMsgBasicInfoSenderCode);

                //SendTime
                XmlElement xmlEBMBrdItemEBMMsgBasicInfoSendTime = xmlDoc.CreateElement("SendTime");
                xmlEBMBrdItemEBMMsgBasicInfoSendTime.InnerText = item.MsgBasicInfo.SendTime;
                xmlEBMBrdItemEBMMsgBasicInfo.AppendChild(xmlEBMBrdItemEBMMsgBasicInfoSendTime);

                //EventType
                XmlElement xmlEBMBrdItemEBMMsgBasicInfoEventType = xmlDoc.CreateElement("EventType");
                xmlEBMBrdItemEBMMsgBasicInfoEventType.InnerText = item.MsgBasicInfo.EventType;
                xmlEBMBrdItemEBMMsgBasicInfo.AppendChild(xmlEBMBrdItemEBMMsgBasicInfoEventType);

                //Severity
                XmlElement xmlEBMBrdItemEBMMsgBasicInfoSeverity = xmlDoc.CreateElement("Severity");
                xmlEBMBrdItemEBMMsgBasicInfoSeverity.InnerText = item.MsgBasicInfo.Severity;
                xmlEBMBrdItemEBMMsgBasicInfo.AppendChild(xmlEBMBrdItemEBMMsgBasicInfoSeverity);


                //StartTime
                XmlElement xmlEBMBrdItemEBMMsgBasicInfoStartTime = xmlDoc.CreateElement("StartTime");
                xmlEBMBrdItemEBMMsgBasicInfoStartTime.InnerText = item.MsgBasicInfo.StartTime;
                xmlEBMBrdItemEBMMsgBasicInfo.AppendChild(xmlEBMBrdItemEBMMsgBasicInfoStartTime);

                //EndTime
                XmlElement xmlEBMBrdItemEBMMsgBasicInfoEndTime = xmlDoc.CreateElement("EndTime");
                xmlEBMBrdItemEBMMsgBasicInfoEndTime.InnerText = item.MsgBasicInfo.EndTime;
                xmlEBMBrdItemEBMMsgBasicInfo.AppendChild(xmlEBMBrdItemEBMMsgBasicInfoEndTime);

                //MsgContent
                XmlElement xmlEBMBrdItemEBMMsgContent = xmlDoc.CreateElement("MsgContent");
                xmlEBMBrdItemEBM.AppendChild(xmlEBMBrdItemEBMMsgContent);

                //LanguageCode
                XmlElement xmlEBMBrdItemEBMMsgContentLanguageCode = xmlDoc.CreateElement("LanguageCode");
                xmlEBMBrdItemEBMMsgContentLanguageCode.InnerText = item.MsgContent.LanguageCode;
                xmlEBMBrdItemEBMMsgContent.AppendChild(xmlEBMBrdItemEBMMsgContentLanguageCode);

                //MsgTitle
                XmlElement xmlEBMBrdItemEBMMsgContentMsgTitle = xmlDoc.CreateElement("MsgTitle");
                xmlEBMBrdItemEBMMsgContentMsgTitle.InnerText = item.MsgContent.MsgTitle;
                xmlEBMBrdItemEBMMsgContent.AppendChild(xmlEBMBrdItemEBMMsgContentMsgTitle);

                //MsgDesc
                XmlElement xmlEBMBrdItemEBMMsgContentMsgDesc = xmlDoc.CreateElement("MsgDesc");
                xmlEBMBrdItemEBMMsgContentMsgDesc.InnerText = item.MsgContent.MsgDesc;
                xmlEBMBrdItemEBMMsgContent.AppendChild(xmlEBMBrdItemEBMMsgContentMsgDesc);

                //AreaCode
                XmlElement xmlEBMBrdItemEBMMsgContentAreaCode = xmlDoc.CreateElement("AreaCode");
                xmlEBMBrdItemEBMMsgContentAreaCode.InnerText = item.MsgContent.AreaCode;
                xmlEBMBrdItemEBMMsgContent.AppendChild(xmlEBMBrdItemEBMMsgContentAreaCode);

                if (item.MsgContent.Auxiliary!=null)
                {
                    //Auxiliary
                    XmlElement xmlEBMBrdItemEBMMsgContentAuxiliary = xmlDoc.CreateElement("Auxiliary");
                    xmlEBMBrdItemEBMMsgContent.AppendChild(xmlEBMBrdItemEBMMsgContentAuxiliary);

                    //AuxiliaryType
                    XmlElement xmlEBMBrdItemEBMMsgContentAuxiliaryAuxiliaryType = xmlDoc.CreateElement("AuxiliaryType");
                    xmlEBMBrdItemEBMMsgContentAuxiliaryAuxiliaryType.InnerText = item.MsgContent.Auxiliary.AuxiliaryType;
                    xmlEBMBrdItemEBMMsgContentAuxiliary.AppendChild(xmlEBMBrdItemEBMMsgContentAuxiliaryAuxiliaryType);

                    //AuxiliaryDesc
                    XmlElement xmlEBMBrdItemEBMMsgContentAuxiliaryAuxiliaryDesc = xmlDoc.CreateElement("AuxiliaryDesc");
                    xmlEBMBrdItemEBMMsgContentAuxiliaryAuxiliaryDesc.InnerText = item.MsgContent.Auxiliary.AuxiliaryDesc;
                    xmlEBMBrdItemEBMMsgContentAuxiliary.AppendChild(xmlEBMBrdItemEBMMsgContentAuxiliaryAuxiliaryDesc);


                    //Size
                    XmlElement xmlEBMBrdItemEBMMsgContentAuxiliarySize = xmlDoc.CreateElement("Size");
                    xmlEBMBrdItemEBMMsgContentAuxiliarySize.InnerText = item.MsgContent.Auxiliary.Size;
                    xmlEBMBrdItemEBMMsgContentAuxiliary.AppendChild(xmlEBMBrdItemEBMMsgContentAuxiliarySize);

                }
                XmlElement xmlEBMBrdItemBrdStateCode = xmlDoc.CreateElement("BrdStateCode");
                xmlEBMBrdItemBrdStateCode.InnerText = item.BrdStateCode;
                xmlEBMBrdItem.AppendChild(xmlEBMBrdItemBrdStateCode);

                string BrdStateDesc = "";
                switch (item.BrdStateCode)
                {

                    case "0":
                        BrdStateDesc = "未处理";
                        break;
                    case "1":
                        BrdStateDesc = "等待播发";
                        break;
                    case "2":
                        BrdStateDesc = "播发中";
                        break;
                    case "3":
                        BrdStateDesc = "播发成功";
                        break;
                    case "4":
                        BrdStateDesc = "播发失败";
                        break;
                    case "5":
                        BrdStateDesc = "播发取消";
                        break;
                }

                XmlElement xmlEBMBrdItemBrdStateDesc = xmlDoc.CreateElement("BrdStateDesc");
                xmlEBMBrdItemBrdStateDesc.InnerText = BrdStateDesc;
                xmlEBMBrdItem.AppendChild(xmlEBMBrdItemBrdStateDesc);

                
            }
            return xmlDoc;
        }

        /// <summary>
        /// 终端播发记录数据
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument TermBRDResponse(EBD ebdsr, List<TermBRD> lt)
        {
            XmlDocument xmlDoc = new XmlDocument();
            #region 标准头部
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);

            //暂时先注释掉  20181210
            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = ebdsr.EBDID;//
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBDResponse";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBEID");
            xmlSRCAreaCode.InnerText = ebdsr.SRC.EBRID;
            xmlSRC.AppendChild(xmlSRCAreaCode);

            XmlElement xmlDEST = xmlDoc.CreateElement("DEST");
            xmlElem.AppendChild(xmlDEST);
            XmlElement xmlDESTEBEID = xmlDoc.CreateElement("EBEID");
            //try
            //{
            //    xmlDESTEBEID.InnerText = ebdsr.DEST.EBEID;
            //}
            //catch
            //{
            //}
            xmlSRC.AppendChild(xmlDESTEBEID);
            //XmlElement xmlSourceID = xmlDoc.CreateElement("EBEID");
            //xmlSourceID.InnerText = SourceID;//
            //xmlSRC.AppendChild(xmlSourceID);

            //EBDTime
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End
            //RelatedEBD
            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);
            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlReEBDID.InnerText = ebdsr.EBDID.ToString();//与EBDID一致就用这个写
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            #region TermBRD
            XmlElement xmlTermBRDReport = xmlDoc.CreateElement("TermBRDReport");
            xmlElem.AppendChild(xmlTermBRDReport);

            XmlElement xmlRPTStartTime = xmlDoc.CreateElement("RPTStartTime");//RPTStartTime
            xmlRPTStartTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// ebdsr.DataRequest.StartTime;//
            xmlTermBRDReport.AppendChild(xmlRPTStartTime);
            XmlElement xmlRPTEndTime = xmlDoc.CreateElement("RPTEndTime");//RPTEndTime
            xmlRPTEndTime.InnerText = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss"); //ebdsr.DataRequest.EndTime;//
            xmlTermBRDReport.AppendChild(xmlRPTEndTime);
            #region Term
            if (lt.Count > 0)
            {
                for (int l = 0; l < lt.Count; l++)
                {
                    XmlElement xmlTerm = xmlDoc.CreateElement("TermBRD");//TermBRD
                    xmlTermBRDReport.AppendChild(xmlTerm);

                    XmlElement xmlTermBRDID = xmlDoc.CreateElement("TermBRDID");
                    xmlTermBRDID.InnerText = lt[l].TermBRDID;//
                    xmlTerm.AppendChild(xmlTermBRDID);
                    XmlElement xmlTSourceType = xmlDoc.CreateElement("SourceType");
                    xmlTSourceType.InnerText = lt[l].SourceType;//
                    xmlTerm.AppendChild(xmlTSourceType);
                    XmlElement xmlTSourceID = xmlDoc.CreateElement("SourceID");
                    xmlTSourceID.InnerText = lt[l].SourceID;//
                    xmlTerm.AppendChild(xmlTSourceID);

                    XmlElement xmlMsgID = xmlDoc.CreateElement("MsgID");//
                    xmlMsgID.InnerText = lt[l].MsgID;//广播ID
                    xmlTerm.AppendChild(xmlMsgID);
                    XmlElement xmlDeviceID = xmlDoc.CreateElement("DeviceID");//
                    xmlDeviceID.InnerText = lt[l].DeviceID;
                    xmlTerm.AppendChild(xmlDeviceID);
                    XmlElement xmlBRDTime = xmlDoc.CreateElement("BRDTime");//
                    xmlBRDTime.InnerText = lt[l].BRDTime;//
                    xmlTerm.AppendChild(xmlBRDTime);
                    XmlElement xmlResultCode = xmlDoc.CreateElement("ResultCode");//
                    xmlResultCode.InnerText = "1";//播发结果
                    xmlTerm.AppendChild(xmlResultCode);
                    XmlElement xmlResultDesc = xmlDoc.CreateElement("ResultDesc");//
                    xmlResultDesc.InnerText = "播出正常";//播发结果描述
                    xmlTerm.AppendChild(xmlResultDesc);
                }
            }
            #endregion End
            #endregion End
            return xmlDoc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lDev"></param>
        /// <param name="strebdid"></param>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument DeviceInfoResponse(List<Device> lDev, string strebdid, EBD ebdsr = null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            #region 标准头部
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);

            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1";
            xmlElem.AppendChild(xmlProtocolVer);

            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = strebdid;//
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBRDTInfo";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            //EBRID
            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;
            xmlSRC.AppendChild(xmlSRCAreaCode);

            //URL
            XmlElement xmlSRCAreaCodeURL = xmlDoc.CreateElement("URL");
            xmlSRCAreaCodeURL.InnerText = SingletonInfo.GetInstance().CurrentURL;
            xmlSRC.AppendChild(xmlSRCAreaCodeURL);

            //EBDTime
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");

            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End

            if (ebdsr!=null)
            {
                XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
               // xmlRelatedEBD.InnerText = ebdsr.EBDID;
                xmlElem.AppendChild(xmlRelatedEBD);

                XmlElement xmlRelatedEBDEBDID = xmlDoc.CreateElement("EBDID");
                xmlRelatedEBDEBDID.InnerText = ebdsr.EBDID;
                xmlRelatedEBD.AppendChild(xmlRelatedEBDEBDID);
            }

            #region DeviceInfoReport
            XmlElement xmlDeviceInfoReport = xmlDoc.CreateElement("EBRDTInfo");
            xmlElem.AppendChild(xmlDeviceInfoReport);

            string DeviEBRID = sHBRONO.Substring(4, sHBRONO.Length - 6);

            #region Device
            if (lDev.Count > 0)
            {
                for (int l = 0; l < lDev.Count; l++)
                {
                    XmlElement xmlDevice = xmlDoc.CreateElement("EBRDT");//Term
                    xmlDeviceInfoReport.AppendChild(xmlDevice);

                    XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
                    xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    xmlDevice.AppendChild(xmlRptTime);

                    XmlElement xmlRptType2 = xmlDoc.CreateElement("RptType");
                    xmlRptType2.InnerText = "Sync";
                    xmlDevice.AppendChild(xmlRptType2);

                    XmlElement xmlRelatedEBRPS = xmlDoc.CreateElement("RelatedEBRPS");
                    xmlDevice.AppendChild(xmlRelatedEBRPS);

                    XmlElement xmlEBRID = xmlDoc.CreateElement("EBRID");
                    xmlEBRID.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;
                    xmlRelatedEBRPS.AppendChild(xmlEBRID);

                    XmlElement xmlDeviceID = xmlDoc.CreateElement("EBRID");
                    xmlDeviceID.InnerText = lDev[l].EBRID;
                    xmlDevice.AppendChild(xmlDeviceID);

                    XmlElement xmlDeviceName = xmlDoc.CreateElement("EBRName");
                    xmlDeviceName.InnerText = lDev[l].DeviceName;
                    xmlDevice.AppendChild(xmlDeviceName);

                    XmlElement xmlLongitude = xmlDoc.CreateElement("Longitude");
                    xmlLongitude.InnerText = lDev[l].Longitude;
                    xmlDevice.AppendChild(xmlLongitude);

                    XmlElement xmlLatitude = xmlDoc.CreateElement("Latitude");
                    xmlLatitude.InnerText = lDev[l].Latitude;
                    xmlDevice.AppendChild(xmlLatitude);
                }
            }
            #endregion End
            #endregion End
            return xmlDoc;
        }

        /// <summary>
        /// 平台信息 增量 全量
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument platformInfoResponse(string strebdid,EBD ebdsr=null)
        {
            XmlDocument xmlDoc = new XmlDocument();

            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);

            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);


            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = strebdid;//strebdid;//自己的ID前面一长串
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBRPSInfo";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;// sHBRONO;单独的ID
            xmlSRC.AppendChild(xmlSRCAreaCode);


            //暂时先注释  20181210
            //XmlElement xmlDEST = xmlDoc.CreateElement("DEST");
            //xmlElem.AppendChild(xmlDEST);

            //XmlElement xmlDESTEBRID = xmlDoc.CreateElement("EBRID");
            //xmlDESTEBRID.InnerText = "010232000000000001";// sHBRONO;单独的ID
            //xmlDEST.AppendChild(xmlDESTEBRID);

            //EBDTime
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");

            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);

            if (ebdsr!=null)
            {
                XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
                xmlElem.AppendChild(xmlRelatedEBD);

                XmlElement xmlRelatedEBDEBDID = xmlDoc.CreateElement("EBDID");
                xmlRelatedEBDEBDID.InnerText = ebdsr.EBDID;
                xmlRelatedEBD.AppendChild(xmlRelatedEBDEBDID);
            }


            XmlElement xmlDeviceInfoReport = xmlDoc.CreateElement("EBRPSInfo");
            xmlElem.AppendChild(xmlDeviceInfoReport);

            if (ebdsr != null)
            {
                XmlElement xmlParams = xmlDoc.CreateElement("Params");
                xmlDeviceInfoReport.AppendChild(xmlParams);

                XmlElement xmlRPTStartTime = xmlDoc.CreateElement("RPTStartTime");//RPTStartTime
                xmlRPTStartTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// ebdsr.DataRequest.StartTime;
                xmlParams.AppendChild(xmlRPTStartTime);

                XmlElement xmlRPTEndTime = xmlDoc.CreateElement("RPTEndTime");//RPTEndTime
                xmlRPTEndTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //ebdsr.DataRequest.EndTime;
                xmlParams.AppendChild(xmlRPTEndTime);
                XmlElement xmlRptType = xmlDoc.CreateElement("RptType");//RPTEndTime
                if (ebdsr.OMDRequest.Params.RptType == "Full")
                {
                    xmlRptType.InnerText = "Full";
                }
                else
                {
                    xmlRptType.InnerText = "Incremental";
                }
                xmlParams.AppendChild(xmlRptType);
            }

            if (ebdsr==null || (ebdsr!=null && ebdsr.OMDRequest.Params.RptType=="Full"))
            {
                XmlElement xmlDevice = xmlDoc.CreateElement("EBRPS");//Term
                xmlDeviceInfoReport.AppendChild(xmlDevice);

                XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
                xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                xmlDevice.AppendChild(xmlRptTime);

                XmlElement xmlRptType2 = xmlDoc.CreateElement("RptType");
                xmlRptType2.InnerText = "Sync";
                xmlDevice.AppendChild(xmlRptType2);

                //XmlElement xmlRelatedEBRPS = xmlDoc.CreateElement("RelatedEBRPS");
                //xmlDevice.AppendChild(xmlRelatedEBRPS);

                //XmlElement xmlEBRID = xmlDoc.CreateElement("EBRID");
                //xmlEBRID.InnerText = sHBRONO;
                //xmlRelatedEBRPS.AppendChild(xmlEBRID);

                XmlElement xmlDeviceID = xmlDoc.CreateElement("EBRID");
                xmlDeviceID.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;
                xmlDevice.AppendChild(xmlDeviceID);

                XmlElement xmlDeviceName = xmlDoc.CreateElement("EBRName");
                xmlDeviceName.InnerText = SingletonInfo.GetInstance().PlatformEBRName;//"丹阳县应急广播平台";
                xmlDevice.AppendChild(xmlDeviceName);

                XmlElement Address = xmlDoc.CreateElement("Address");
                Address.InnerText =SingletonInfo.GetInstance().PlatformAddress;//"丹阳县广电";
                xmlDevice.AppendChild(Address);

                XmlElement Contact = xmlDoc.CreateElement("Contact");
                Contact.InnerText = SingletonInfo.GetInstance().PlatformContact;//"老铁";
                xmlDevice.AppendChild(Contact);

                XmlElement PhoneNumber = xmlDoc.CreateElement("PhoneNumber");
                PhoneNumber.InnerText = SingletonInfo.GetInstance().PlatformPhoneNumber;//"12345678901";
                xmlDevice.AppendChild(PhoneNumber);

                XmlElement Longitude = xmlDoc.CreateElement("Longitude");
                Longitude.InnerText = SingletonInfo.GetInstance().Longitude; //"120.55"; // "113.7747551274";
                xmlDevice.AppendChild(Longitude);

                XmlElement Latitude = xmlDoc.CreateElement("Latitude");
                Latitude.InnerText = SingletonInfo.GetInstance().Latitude; //"31.87"; //  "34.6328783614";
                xmlDevice.AppendChild(Latitude);

                XmlElement URL = xmlDoc.CreateElement("URL");
                URL.InnerText = SingletonInfo.GetInstance().CurrentURL;
                xmlDevice.AppendChild(URL);
            }
            return xmlDoc;
        }

        /// <summary>
        /// 平台状态 增量 全量
        /// </summary>
        /// <param name="strebdid"></param>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument platformstateInfoResponse(string strebdid,string code, EBD ebdsr = null)
        {
            XmlDocument xmlDoc = new XmlDocument();

            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);

            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);


            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = strebdid;//strebdid;//自己的ID前面一长串
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBRPSState";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;// sHBRONO;单独的ID
            xmlSRC.AppendChild(xmlSRCAreaCode);


            //暂时先注释  20181210
            //XmlElement xmlDEST = xmlDoc.CreateElement("DEST");
            //xmlElem.AppendChild(xmlDEST);

            //XmlElement xmlDESTEBRID = xmlDoc.CreateElement("EBRID");
            //xmlDESTEBRID.InnerText = "010232000000000001";// sHBRONO;单独的ID
            //xmlDEST.AppendChild(xmlDESTEBRID);

            //EBDTime
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");

            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);

            if (ebdsr != null)
            {
                XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
                xmlElem.AppendChild(xmlRelatedEBD);

                XmlElement xmlRelatedEBDEBDID = xmlDoc.CreateElement("EBDID");
                xmlRelatedEBDEBDID.InnerText = ebdsr.EBDID;
                xmlRelatedEBD.AppendChild(xmlRelatedEBDEBDID);
            }
            XmlElement xmlDeviceInfoReport = xmlDoc.CreateElement("EBRPSState");
            xmlElem.AppendChild(xmlDeviceInfoReport);

            if (ebdsr == null || (ebdsr != null && ebdsr.OMDRequest.Params.RptType == "Full"))
            {
                XmlElement xmlDevice = xmlDoc.CreateElement("EBRPS");//Term
                xmlDeviceInfoReport.AppendChild(xmlDevice);

                XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
                xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                xmlDevice.AppendChild(xmlRptTime);

                XmlElement xmlDeviceID = xmlDoc.CreateElement("EBRID");
                xmlDeviceID.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;
                xmlDevice.AppendChild(xmlDeviceID);


                string StateCodetmp = "";
                string StateDesctmp = "";
                switch (code)
                {
                    case "1":
                        StateCodetmp = "1";
                        StateDesctmp = "开机/运行正常";
                        break;
                    case "2":
                        StateCodetmp = "2";
                        StateDesctmp = "关机/停止运行";
                        break;
                    case "3":
                        StateCodetmp = "3";
                        StateDesctmp = "故障";
                        break;
                    case "4":
                        StateCodetmp = "4";
                        StateDesctmp = "故障恢复";
                        break;
                    case "5":
                        StateCodetmp = "5";
                        StateDesctmp = "播发中";
                        break;
                }


                XmlElement xmlStateCode = xmlDoc.CreateElement("StateCode");
                xmlStateCode.InnerText = StateCodetmp;
                xmlDevice.AppendChild(xmlStateCode);

                XmlElement xmlStateDesc = xmlDoc.CreateElement("StateDesc");
                xmlStateDesc.InnerText = StateDesctmp;
                xmlDevice.AppendChild(xmlStateDesc);
            }
            return xmlDoc;
        }

  
        public XmlDocument DeviceStateResponse(List<Device> lDevState, string strebdid, EBD ebdsr = null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            #region 标准头部
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);

            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = strebdid;//
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBRDTState";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;
            xmlSRC.AppendChild(xmlSRCAreaCode);

            XmlElement xmlSRCAreaCodeURL = xmlDoc.CreateElement("URL");
            xmlSRCAreaCodeURL.InnerText = SingletonInfo.GetInstance().CurrentURL;
            xmlSRC.AppendChild(xmlSRCAreaCodeURL);

            //EBDTime
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End

            if (ebdsr!=null)
            {
                //RelatedEBD
                XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
                xmlElem.AppendChild(xmlRelatedEBD);

                XmlElement xmlRelatedEBDEBDID = xmlDoc.CreateElement("EBDID");
                xmlRelatedEBDEBDID.InnerText = ebdsr.EBDID;
                xmlRelatedEBD.AppendChild(xmlRelatedEBDEBDID);
            }

            #region DeviceInfoReport
            XmlElement xmlDeviceStateReport = xmlDoc.CreateElement("EBRDTState");
            xmlElem.AppendChild(xmlDeviceStateReport);

           
            #region Device
            if (lDevState.Count > 0)
            {
                for (int l = 0; l < lDevState.Count; l++)
                {
                    XmlElement xmlDevice = xmlDoc.CreateElement("EBRDT");
                    xmlDeviceStateReport.AppendChild(xmlDevice);

                    XmlElement xmlDeviceCategory = xmlDoc.CreateElement("RptTime");
                    xmlDeviceCategory.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    xmlDevice.AppendChild(xmlDeviceCategory);

                    XmlElement xmlDeviceID = xmlDoc.CreateElement("EBRID");
                    xmlDeviceID.InnerText =lDevState[l].EBRID;
                    xmlDevice.AppendChild(xmlDeviceID);



                    string StateDesctmp = "";
                    switch (lDevState[l].StateCode)
                    {
                        case "1":
                            StateDesctmp = "开机/运行正常";
                            break;

                        case "2":
                            StateDesctmp = "关机/停止运行";
                            break;

                        case "3":
                            StateDesctmp = "故障";
                            break;

                        case "4":
                            StateDesctmp = "故障恢复";
                            break;

                        case "5":
                            StateDesctmp = "播发中";
                            break;

                    }
                    XmlElement xmlDeviceType = xmlDoc.CreateElement("StateCode");
                    xmlDeviceType.InnerText = lDevState[l].StateCode;
                    xmlDevice.AppendChild(xmlDeviceType);
                    XmlElement xmlDeviceName = xmlDoc.CreateElement("StateDesc");
                    xmlDeviceName.InnerText = StateDesctmp;
                    xmlDevice.AppendChild(xmlDeviceName);
                }
            }
            #endregion End
            #endregion End
            return xmlDoc;
        }

        public XmlDocument SignResponse(string refbdid, string strIssuerID, string strCertSN, string strSignatureValue)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));

            XmlElement xmlElem = xmlDoc.CreateElement("Signature");
            xmlDoc.AppendChild(xmlElem);

            //Version
            XmlElement xmlVersion = xmlDoc.CreateElement("Version");
            xmlVersion.InnerText = "1.0";
            xmlElem.AppendChild(xmlVersion);

            //RelatedEBD
            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = refbdid;
            xmlRelatedEBD.AppendChild(xmlEBDID);

            // SignatureCert
            XmlElement xmlSignatureCert = xmlDoc.CreateElement("SignatureCert");
            xmlElem.AppendChild(xmlSignatureCert);

            XmlElement xmlCertType = xmlDoc.CreateElement("CertType");
            xmlCertType.InnerText = "01";
            xmlSignatureCert.AppendChild(xmlCertType);

            XmlElement xmlIssuerID = xmlDoc.CreateElement("IssuerID");
            xmlIssuerID.InnerText = strIssuerID;
            xmlSignatureCert.AppendChild(xmlIssuerID);

            //CertSN
            XmlElement xmlCertSN = xmlDoc.CreateElement("CertSN");
            xmlCertSN.InnerText = strCertSN;
            xmlSignatureCert.AppendChild(xmlCertSN);


            //SignatureTime
            XmlElement xmlSignatureTime = xmlDoc.CreateElement("SignatureTime");

            double D = DateTime.Now.ToOADate();
            Byte[] Bytes = BitConverter.GetBytes(D);
            String S = BitConverter.ToString(Bytes);

            xmlSignatureTime.InnerText = S;
            xmlElem.AppendChild(xmlSignatureTime);
            //DigestAlgorithm
            XmlElement xmlDigestAlgorithm = xmlDoc.CreateElement("DigestAlgorithm");
            xmlDigestAlgorithm.InnerText = "SM3";
            xmlElem.AppendChild(xmlDigestAlgorithm);
            //SignatureAlgorithm
            XmlElement xmlSignatureAlgorithm = xmlDoc.CreateElement("SignatureAlgorithm");
            xmlSignatureAlgorithm.InnerText = "SM2";
            xmlElem.AppendChild(xmlSignatureAlgorithm);

            XmlElement xmlSignatureValue = xmlDoc.CreateElement("SignatureValue");
            xmlSignatureValue.InnerText = strSignatureValue;
            xmlElem.AppendChild(xmlSignatureValue);



            return xmlDoc;
        }

        /// <summary>
        /// 回应主动请求的播发状态  20181214
        /// </summary>
        /// <param name="EBMID"></param>
        /// <param name="strebdid"></param>
        /// <returns></returns>
        public XmlDocument ResponeEBMStateRequrest(EBD ebdsr, string strebdid,string BrdStateCode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            #region 标准头部
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);

            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);


            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = strebdid;
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBMStateResponse";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode.InnerText = SingletonInfo.GetInstance().CurrentResourcecode;//ebdsr.SRC.EBEID;
            xmlSRC.AppendChild(xmlSRCAreaCode);


            XmlElement xmlSRCAreaCodeURL = xmlDoc.CreateElement("URL");
            xmlSRCAreaCodeURL.InnerText = SingletonInfo.GetInstance().CurrentURL;
            xmlSRC.AppendChild(xmlSRCAreaCodeURL);


            //EBDTime
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End


            //RelatedEBD
            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlRelatedEBDEBDID = xmlDoc.CreateElement("EBDID");
            xmlRelatedEBDEBDID.InnerText = ebdsr.EBDID;
            xmlRelatedEBD.AppendChild(xmlRelatedEBDEBDID);


            XmlElement xmlEBMStateResponse = xmlDoc.CreateElement("EBMStateResponse");
            xmlElem.AppendChild(xmlEBMStateResponse);

            XmlElement RptTime = xmlDoc.CreateElement("RptTime");
            RptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//从100000000000开始编号
            xmlEBMStateResponse.AppendChild(RptTime);

            XmlElement xmlEBM = xmlDoc.CreateElement("EBM");
            xmlEBMStateResponse.AppendChild(xmlEBM);

            XmlElement xmlEBMID = xmlDoc.CreateElement("EBMID");
            xmlEBMID.InnerText = ebdsr.EBM.EBEID;//从100000000000开始编号
            xmlEBM.AppendChild(xmlEBMID);

           

            XmlElement xmlBRDState = xmlDoc.CreateElement("BrdStateCode");
            xmlBRDState.InnerText = BrdStateCode;
            xmlEBMStateResponse.AppendChild(xmlBRDState);


            string BrdStateDesc = "";
            switch (BrdStateCode)
            {
                case "0":
                    BrdStateDesc = "未处理";
                    break;
                case "1":
                    BrdStateDesc = "等待播发";
                    break;
                case "2":
                    BrdStateDesc = "播发中";
                    break;
                case "3":
                    BrdStateDesc = "播发成功";
                    break;
                case "4":
                    BrdStateDesc = "播发失败";
                    break;
                case "5":
                    BrdStateDesc = "播发取消";
                    break;
            }

            XmlElement xmlBrdStateDesc = xmlDoc.CreateElement("BrdStateDesc");
            xmlBrdStateDesc.InnerText = BrdStateDesc;
            xmlEBMStateResponse.AppendChild(xmlBrdStateDesc);
            return xmlDoc;
        }

      

    }
}
