using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRPlatForm
{
    public class responseTXT
    {
        public string SourceType = "";
        public string SourceName = "";
        public string SourceID = "";
        
        public string Header(EBD ebdHeader,string strEBDType)
        {
            StringBuilder sbHeader = new StringBuilder();
            sbHeader.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sbHeader.Append("<EBD xmlns=\"http://www.w3.org/2001/XMLSchema\">");
            sbHeader.Append("<Version>1.0</Version>");
            sbHeader.Append("<EBDID>" + ebdHeader.EBDID + "</EBDID>");
            sbHeader.Append("<EBDType>" + strEBDType + "</EBDType>");
            sbHeader.Append("<Source>");
            sbHeader.Append("<SourceType>" + SourceType +"</SourceType>");
            sbHeader.Append("<SourceName>" + SourceName + "</SourceName>");
            sbHeader.Append("<SourceID>" + SourceID + "</SourceID>");
            sbHeader.Append("</Source>");
            sbHeader.Append("<EBDTime>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ "</EBDTime>");
            sbHeader.Append("<RelatedEBD>");
            sbHeader.Append("<EBDID>" + ebdHeader.EBDID + "</EBDID>");
            sbHeader.Append("</RelatedEBD>");
            return sbHeader.ToString();
        }

        public string EBMStateResponse(EBD ebdsr)
        {
            StringBuilder sbEBMState = new StringBuilder();
            Random rd = new Random();
            string responsestr = "";
            responsestr = Header(ebdsr, "EBMStateResponse");
            sbEBMState.Append("<EBMStateResponse>");
            sbEBMState.Append("<MsgID>" + ebdsr.EBM.MsgID +"</MsgID>");
            sbEBMState.Append("<CoveragePercent>" + rd.Next(60,80).ToString() + "</CoveragePercent>");
            #region Coverage
            if (ebdsr.EBM.Coverage != null)
            {
                sbEBMState.Append("<Coverage>");
                for (int i = 0; i < ebdsr.EBM.Coverage.Area.Count; i++)
                {
                    sbEBMState.Append("<Area>");
                    sbEBMState.Append("<AreaName>" + ebdsr.EBM.Coverage.Area[i].AreaName.ToString() + "</AreaName>");
                    sbEBMState.Append("<AreaCode>" + ebdsr.EBM.Coverage.Area[i].AreaCode + "</AreaCode>");
                    sbEBMState.Append("</Area>");
                }
                sbEBMState.Append("</Coverage>");
            }
            #endregion End
            sbEBMState.Append("</EBMStateResponse>");
            sbEBMState.Append("</EBD>");
            responsestr = responsestr + sbEBMState.ToString();
            return responsestr;
        }

        public string SignResponse(string refbdid, string ResultValue)
        {
            StringBuilder sbsign = new StringBuilder();
            sbsign.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sbsign.Append("<Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\">");
            sbsign.Append("<Version>1.0</Version>");
            sbsign.Append("<RefEBDID>" + refbdid + "</RefEBDID>");
            sbsign.Append("<SignatureValue>" + ResultValue + "</SignatureValue>");
            sbsign.Append("</Signature>");
            return sbsign.ToString();
        }

        #region DataRequest
        public string DeviceStateResponse(EBD ebdsr, List<Device> lDevState)
        {
            StringBuilder sbDevState = new StringBuilder();
            string responsestr = "";
            responsestr = Header(ebdsr,"DeviceStateReport");
            sbDevState.Append("<DeviceStateReport>");
            #region Device
            if (lDevState.Count > 0)
            {
                for (int l = 0; l < lDevState.Count; l++)
                {
                    sbDevState.Append("<Device>");
                    sbDevState.Append("<DeviceID>"+lDevState[l].DeviceID+"</DeviceID>");
                    sbDevState.Append("<DeviceCategory>Term</DeviceCategory>");
                    sbDevState.Append("<DeviceType>TN5415E</DeviceType>");
                    sbDevState.Append("<DeviceName>音柱</DeviceName>");
                    sbDevState.Append("<DeviceState>"+ lDevState[l].DeviceState +"</DeviceState>");
                    sbDevState.Append("</Device>");
                }
            }
            #endregion End
            sbDevState.Append("</DeviceStateReport>");
            sbDevState.Append("</EBD>");
            responsestr = responsestr + sbDevState.ToString();
            return responsestr;
        }

        public string DeviceInfoResponse(EBD ebdsr, List<Device> lDev)
        {
            StringBuilder sbDevInfo = new StringBuilder();
            string responsestr = Header(ebdsr, "DeviceInfoReport");
            sbDevInfo.Append("<DeviceInfoReport>");
            sbDevInfo.Append("<RPTStartTime>"+ ebdsr.DataRequest.StartTime +"</RPTStartTime>");
            sbDevInfo.Append("<RPTEndTime>" + ebdsr.DataRequest.EndTime + "</RPTEndTime>");
            #region DeviceInfo
            if (lDev.Count > 0)
            {
                for (int l = 0; l < lDev.Count; l++)
                {
                    sbDevInfo.Append("<Device>");
                    sbDevInfo.Append("<DeviceID>" + lDev[l].DeviceID + "</DeviceID>");
                    sbDevInfo.Append("<DeviceCategory>Term</DeviceCategory>");
                    sbDevInfo.Append("<DeviceType>TN5415E</DeviceType>");
                    sbDevInfo.Append("<DeviceName>音柱</DeviceName>");
                    sbDevInfo.Append("<AreaCode>" + lDev[l].AreaCode + "</AreaCode>");
                    sbDevInfo.Append("<AdminLevel>村级</AdminLevel>");
                    sbDevInfo.Append("</Device>");
                }
            }
            #endregion End
            sbDevInfo.Append("</DeviceInfoReport>");
            sbDevInfo.Append("</EBD>");
            responsestr = responsestr + sbDevInfo.ToString();
            return responsestr;
        }

        public string TermBRDResponse(EBD ebdsr, List<TermBRD> lt)
        {
            StringBuilder sbTerm = new StringBuilder();
            string responsestr = Header(ebdsr, "TermBRDReport");
            sbTerm.Append("<TermBRDReport>");
            sbTerm.Append("<RPTStartTime>" + ebdsr.DataRequest.StartTime + "</RPTStartTime>");
            sbTerm.Append("<RPTEndTime>" + ebdsr.DataRequest.EndTime + "</RPTEndTime>");
            #region Term
            if (lt.Count > 0)
            {
                for (int l = 0; l < lt.Count; l++)
                {
                    sbTerm.Append("<TermBRD>");
                    sbTerm.Append("<TermBRDID>" + lt[l].TermBRDID + "</TermBRDID>");
                    sbTerm.Append("<SourceType>" + lt[l].SourceType + "</SourceType>");
                    sbTerm.Append("<SourceID>" + lt[l].SourceID + "</SourceID>");
                    sbTerm.Append("<MsgID>" + lt[l].MsgID + "</MsgID>");
                    sbTerm.Append("<DeviceID>" + lt[l].DeviceID + "</DeviceID>");
                    sbTerm.Append("<BRDTime>"+ lt[l].BRDTime+ "</BRDTime>");
                    sbTerm.Append("<ResultCode>"+"1"+"</ResultCode>");
                    sbTerm.Append("<ResultDesc>" + "播出正常" + "</ResultDesc>");
                    sbTerm.Append("</TermBRD>");
                }
            }
            #endregion End
            sbTerm.Append("</TermBRDReport>");
            sbTerm.Append("</EBD>");
            responsestr = responsestr + sbTerm.ToString();
            return responsestr;
        }

        public string PlatformBRDResponse(EBD ebdsr, List<PlatformBRD> lP)
        {
            StringBuilder sbPlat = new StringBuilder();
            string responsestr = Header(ebdsr, "PlatformBRDReport");
            sbPlat.Append("<PlatformBRDReport>");
            sbPlat.Append("<RPTStartTime>" + ebdsr.DataRequest.StartTime + "</RPTStartTime>");
            sbPlat.Append("<RPTEndTime>" + ebdsr.DataRequest.EndTime + "</RPTEndTime>");
            #region PlatformBRD
            if (lP.Count > 0)
            {
                for (int i = 0; i < lP.Count; i++)
                {
                    sbPlat.Append("<PlatformBRD>");
                    sbPlat.Append("<PlatformBRDID>111</PlatformBRDID>");
                    sbPlat.Append("<SourceType>111</SourceType>");
                    sbPlat.Append("<SourceID>111</SourceID>");
                    sbPlat.Append("<MsgID>111</MsgID>");
                    sbPlat.Append("<Sender>111</Sender>");
                    sbPlat.Append("<UnitId>111</UnitId>");
                    sbPlat.Append("<UnitName>111</UnitName>");
                    sbPlat.Append("<PersonID>111</PersonID>");
                    sbPlat.Append("<PersonName>111</PersonName>");
                    sbPlat.Append("<BRDStartTime>111</BRDStartTime>");
                    sbPlat.Append("<BRDEndTime>111</BRDEndTime>");
                    sbPlat.Append("<AudioFileURL>"+ "" +"</AudioFileURL>");
                    sbPlat.Append("</PlatformBRD>");
                }
            }
            sbPlat.Append("</PlatformBRDReport>");
            sbPlat.Append("</EBD>");
            responsestr = responsestr + sbPlat.ToString();
            #endregion End
            return responsestr;
        }

        #endregion End
    }
}
