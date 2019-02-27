using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DockingServices.AudioMessage;
using System.Data;
using System.IO;
using System.Xml;
using System.Drawing;
using DockingServices.Class;

namespace DockingServices.AudioMessage.MQAudio
{
    //播放状态反馈类
    public class AudioPlayState : IPlayState
    {
        private delegate bool HandlingDelegate(string TmcId, string path, string BrdStateCode);
        private event HandlingDelegate HandlingEvent;
        //未播放

        //播放中

        /// <summary>
        /// 播放完成
        /// </summary>
        /// <returns></returns>
        public bool NotPlay(string TmcId, string path, string BrdStateCode)
        {
            try
            {
                bool Radio= EmergencyBroadcast(TmcId, path, BrdStateCode,null);
                return Radio;
            }
            catch (Exception ex)
            {
                throw new Exception("未播放:" +ex.Message);
            }
            return false;

        }
        public bool FeedbackFunction(EBD ebdsr,string BrdStateCode, string TimingTerminalState)
        {
            bool flag = false;
            try
            {
                if (string.IsNullOrEmpty(TimingTerminalState))
                {
                    bool eb = sendEBMStateResponse(ebdsr, BrdStateCode);
                    if (eb)
                    {
                        flag = true;
                    }
                }
                else
                {
                    bool eb= sendEBMStateResponse(ebdsr, BrdStateCode);
                    bool Up = UpdateState();
                    if (eb && Up)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return flag;
        }
        private bool EmergencyBroadcast(string TmcId, string path, string BrdStateCode,string TimingTerminalState)
        {
            EBD ebd;
            DataTable dt;
            bool flag = false;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
                    String xmlInfo = sr.ReadToEnd();
                    xmlInfo = xmlInfo.Replace("xmlns:xs", "xmlns");
                    sr.Close();
                    xmlInfo = XmlSerialize.ReplaceLowOrderASCIICharacters(xmlInfo);
                    xmlInfo = XmlSerialize.GetLowOrderASCIICharacters(xmlInfo);
                    ebd = XmlSerialize.DeserializeXML<EBD>(xmlInfo);
                }
                if (Convert.ToInt32(BrdStateCode) != 0)
                {
                    if (Convert.ToInt32(TmcId) < 0)
                    {
                        return flag;
                    }
                    else
                    {
                        dt = ViewDataTsCmdStore(TmcId);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            flag = FeedbackFunction(ebd, BrdStateCode, TimingTerminalState);
                            return flag;
                        }
                    }
                }
                else
                {
                    flag = FeedbackFunction(ebd, BrdStateCode, TimingTerminalState);
                    return flag;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("应急消息回馈:" + ex.Message);
            }
        }

        public string GetSequenceCodes()
        {
            SingletonInfo.GetInstance().SequenceCodes += 1;
            return SingletonInfo.GetInstance().SequenceCodes.ToString().PadLeft(16, '0');
        }

        /// <summary>
        /// 播发状态反馈  20181213
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <param name="BrdStateDesc"></param>
        /// <param name="BrdStateCode"></param>
        /// <returns></returns>
        private bool sendEBMStateResponse(EBD ebdsr, string BrdStateCode)
        {
            //*反馈
            #region 先删除解压缩包中的文件
            bool flag = false;
            foreach (string xmlfiledel in Directory.GetFileSystemEntries(ServerForm.sEBMStateResponsePath))
            {
                if (File.Exists(xmlfiledel))
                {
                    FileInfo fi = new FileInfo(xmlfiledel);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(xmlfiledel);//直接删除其中的文件  
                }
            }
            #endregion End
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            string frdStateName = "10" + SingletonInfo.GetInstance().CurrentResourcecode + GetSequenceCodes();
            string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";
            xmlHeartDoc = rHeart.EBMStateRequestResponse(ebdsr, frdStateName,BrdStateCode);
            TarXml.AudioResponseXml.CreateXML(xmlHeartDoc, ServerForm.sEBMStateResponsePath + xmlEBMStateFileName);
            ServerForm.mainFrm.GenerateSignatureFile(ServerForm.sEBMStateResponsePath, frdStateName);
            ServerForm.tar.CreatTar(ServerForm.sEBMStateResponsePath, ServerForm.sSendTarPath, frdStateName);
            string sHeartBeatTarName = ServerForm.sSendTarPath + "\\EBDT_" + frdStateName + ".tar";
            try
            {
               string result=  HttpSendFile.UploadFilesByPost(SingletonInfo.GetInstance().SendTarAddress, sHeartBeatTarName);
                if (result != "0")
                {
                    return true;
                }
            }
            catch (Exception w)
            {
                Log.Instance.LogWrite("应急消息播发状态反馈发送平台错误：" + w.Message);
            }
            return flag;
        }


        /// <summary>
        /// 比较两个列表的内容是否相等  
        /// </summary>
        /// <param name="List1"></param>
        /// <param name="List2"></param>
        /// <returns></returns>
        private bool CheckList(List<IncrementalEBRDTState> List1, List<IncrementalEBRDTState> List2)
        {
            bool flag = true;
            if (List1.Count == List2.Count)
            {
                foreach (IncrementalEBRDTState item in List1)
                {
                    foreach (var item1 in List2)
                    {
                        if (item1.SRV_LOGICAL_CODE_GB == item.SRV_LOGICAL_CODE_GB)
                        {
                            //被实例化的对象，是必须分属性一一对比的，没办法一次性对比。
                            if (item1.powersupplystatus != item.powersupplystatus)
                            {
                                flag = false;
                                break;
                            }

                            if (item1.SRV_RMT_STATUS != item.SRV_RMT_STATUS)
                            {
                                flag = false;
                                break;
                            }

                            if (item1.SRV_PHYSICAL_CODE != item.SRV_PHYSICAL_CODE)
                            {
                                flag = false;
                                break;
                            }

                        }
                    }
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 从数据库获取终端状态信息
        /// </summary>
        /// <param name="datatype"></param>
        /// <returns></returns>
        private List<Device> GetEBRDTStateFromDataBase(string datatype)
        {
            List<Device> ListDevicetmp = new List<Device>();
            if (datatype == "Incremental")
            {
                #region 增量
                string MediaSql = "select a.SRV_PHYSICAL_CODE,a.SRV_LOGICAL_CODE_GB,a.SRV_RMT_STATUS,b.powersupplystatus from SRV a inner join Srv_Status b on a.SRV_PHYSICAL_CODE = b.srv_physical_code";
                DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);
                if (dtMedia.Rows.Count > 0)
                {
                    List<IncrementalEBRDTState> Listtmp = new List<IncrementalEBRDTState>();
                    foreach (DataRow item in dtMedia.Rows)
                    {
                        IncrementalEBRDTState pp = new IncrementalEBRDTState();
                        pp.powersupplystatus = item["powersupplystatus"].ToString();
                        pp.SRV_LOGICAL_CODE_GB = item["SRV_LOGICAL_CODE_GB"].ToString();
                        pp.SRV_PHYSICAL_CODE = item["SRV_PHYSICAL_CODE"].ToString();
                        pp.SRV_RMT_STATUS = item["SRV_RMT_STATUS"].ToString();
                        Listtmp.Add(pp);
                    }
                    if (!CheckList(Listtmp, SingletonInfo.GetInstance().ListIncrementalEBRDTState))
                    {
                        foreach (IncrementalEBRDTState item in Listtmp)
                        {
                            IncrementalEBRDTState selectone = SingletonInfo.GetInstance().ListIncrementalEBRDTState.Find(c => c.SRV_PHYSICAL_CODE.Equals(item.SRV_PHYSICAL_CODE));
                            if (selectone != null)
                            {
                                if (!selectone.Equals(item))
                                {
                                    selectone = item;
                                    Device pp = new Device();
                                    pp.EBRID = item.SRV_LOGICAL_CODE_GB;

                                    if (item.SRV_RMT_STATUS == "离线")
                                    {
                                        pp.StateCode = "3";
                                    }
                                    else
                                    {
                                        string statustmp = item.powersupplystatus;
                                        if (statustmp.Contains("广播"))
                                        {
                                            pp.StateCode = "5";
                                        }
                                        if (statustmp.Contains("关机"))
                                        {
                                            pp.StateCode = "2";
                                        }
                                        if (statustmp.Contains("开机"))
                                        {
                                            pp.StateCode = "1";
                                        }
                                    }
                                    ListDevicetmp.Add(pp);

                                }
                            }
                            else
                            {
                                //说明是新增的终端
                                Device pp = new Device();
                                pp.EBRID = item.SRV_LOGICAL_CODE_GB;
                                string statustmp = item.powersupplystatus;
                                if (statustmp.Contains("广播"))
                                {
                                    pp.StateCode = "5";
                                }
                                if (statustmp.Contains("关机"))
                                {
                                    pp.StateCode = "2";
                                }
                                if (statustmp.Contains("开机"))
                                {
                                    pp.StateCode = "1";
                                }
                                ListDevicetmp.Add(pp);
                                SingletonInfo.GetInstance().ListIncrementalEBRDTState.Add(item);
                            }
                        }
                    }
                    else
                    {
                       // MessageBox.Show("终端状态没有发生变化");
                    }
                }
                #endregion
            }
            else
            {
                #region 全量
                //全量终端信息  包括没有回传功能的设备
                string MediaSql1 = "select SRV_PHYSICAL_CODE,SRV_LOGICAL_CODE_GB,SRV_RMT_STATUS,SRV_RMT_SWITCH from SRV";
                string MediaSql2 = "select srv_physical_code,powersupplystatus from Srv_Status";

                DataTable dtMedia1 = mainForm.dba.getQueryInfoBySQL(MediaSql1);
                DataTable dtMedia2 = mainForm.dba.getQueryInfoBySQL(MediaSql2);
                foreach (DataRow item1 in dtMedia1.Rows)
                {
                    if (item1["SRV_RMT_SWITCH"].ToString() == "启用")
                    {
                        //带回传功能的终端
                        DataRow[] drsingle = dtMedia2.Select(string.Format("srv_physical_code={0}", item1["SRV_PHYSICAL_CODE"]));
                        if (drsingle.Length > 0)
                        {
                            foreach (DataRow item2 in dtMedia2.Rows)
                            {
                                if (item2["srv_physical_code"].ToString() == item1["SRV_PHYSICAL_CODE"].ToString())
                                {
                                    Device pp = new Device();
                                    pp.EBRID = item1["SRV_LOGICAL_CODE_GB"].ToString();

                                    if (item1["SRV_RMT_STATUS"].ToString() == "离线")
                                    {
                                        pp.StateCode = "3";
                                    }
                                    else
                                    {
                                        string statustmp = item2["powersupplystatus"].ToString();
                                        if (statustmp.Contains("广播"))
                                        {
                                            pp.StateCode = "5";
                                        }
                                        if (statustmp.Contains("关机"))
                                        {
                                            pp.StateCode = "2";
                                        }
                                        if (statustmp.Contains("开机"))
                                        {
                                            pp.StateCode = "1";
                                        }
                                    }
                                    ListDevicetmp.Add(pp);
                                }
                            }
                        }
                        else
                        {
                            //有回传功能但数据没回传回来
                            Device pp = new Device();
                            pp.EBRID = item1["SRV_LOGICAL_CODE_GB"].ToString();
                            pp.StateCode = "3";
                            ListDevicetmp.Add(pp);
                        }
                    }
                    else
                    {
                        //不带回传功能的终端
                        Device pp = new Device();
                        pp.EBRID = item1["SRV_LOGICAL_CODE_GB"].ToString();
                        pp.StateCode = "1";//没有回传，不知道具体状态，先强制赋值  20190111
                        ListDevicetmp.Add(pp);
                    }
                }
                #endregion
            }
            return ListDevicetmp;
        }

        #region 应急广播平台终端状态上报函数
        private bool UpdateState()
        {
            bool flag = false;
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            string MediaSql = "";
            ServerForm.DeleteFolder(ServerForm.sHeartSourceFilePath);//删除原有XML发送文件的文件夹下的XML
            string frdStateName = "";
            List<Device> lDev = new List<Device>();
            try
            {
                lDev = GetEBRDTStateFromDataBase("Full");
                frdStateName = "10" + SingletonInfo.GetInstance().CurrentResourcecode + BBSHelper.GetSequenceCodes();
                string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";
                xmlHeartDoc = rHeart.DeviceStateResponse(lDev, frdStateName);
                TarXml.AudioResponseXml.CreateXML(xmlHeartDoc, ServerForm.sHeartSourceFilePath + xmlEBMStateFileName);
                ServerForm.mainFrm.GenerateSignatureFile(ServerForm.sHeartSourceFilePath, frdStateName);
                ServerForm.tar.CreatTar(ServerForm.sHeartSourceFilePath, ServerForm.sSendTarPath, frdStateName);//使用新TAR
                string sHeartBeatTarName = ServerForm.sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                string result = SendTar.SendTarOrder.sendHelper.AddPostQueue(SingletonInfo.GetInstance().SendTarAddress, sHeartBeatTarName);
                if (result == "1")
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("终端状态变更:" + ex.Message);
            }
            return flag;
        }
        #endregion


        public DataTable ViewDataTsCmdStore(string TsCmd_ID)
        {
            string MediaSql;
            try
            {
                MediaSql = "select TsCmd_ID,TsCmd_ExCute from  TsCmdStore where TsCmd_ID='" + TsCmd_ID + "'";
                //  MediaSql = "select top(1)TsCmd_ID,TsCmd_XmlFile from  TsCmdStore where TsCmd_ValueID = '" + ebd.EBMStateRequest.EBM.EBMID + "' order by TsCmd_Date desc";
                DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);

                return dtMedia != null && dtMedia.Rows.Count > 0 ? dtMedia : null;
            }
            catch (Exception ex)
            {
               // throw new Exception("查询TsCmdStore出现异常:" + ex.Message);
                return null;
            }
          
        }

        public bool Playing(string TmcId, string path, string BrdStateCode,string TimingTerminalState)
        {
            try
            {
                bool Radio = EmergencyBroadcast(TmcId, path, BrdStateCode, TimingTerminalState);
                return Radio;
            }
            catch (Exception ex)
            {
               //throw new Exception("未播放:" + ex.Message);
                return false;
            }
           
        }

        public bool PlayOver(string TmcId, string path, string BrdStateCode, string TimingTerminalState)
        {
            try
            {
                bool Radio = EmergencyBroadcast(TmcId, path, BrdStateCode, TimingTerminalState);
                return Radio;
            }
            catch (Exception ex)
            {
               // throw new Exception("未播放:" + ex.Message);
                return false;
            }
          
        }

        public bool Untreated(string path, string BrdStateCode)
        {
            try
            {
                bool Radio = EmergencyBroadcast("-1", path, BrdStateCode,null);
                return Radio;
            }
            catch (Exception ex)
            {
               // throw new Exception("未播放:" + ex.Message);
                return false;
            }
          
        }
    }
}
