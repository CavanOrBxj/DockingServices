using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DockingServices.AudioMessage.MQAudio;
using System.Data;
using System.Threading;
using System.IO;
using System.Drawing;
using DockingServices.AudioMessage.SendMQ;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace DockingServices.AudioMessage
{
    public enum AudioType
    {
        Text = 0,
        speech = 1,
    }
    
    public enum AudioPlayState
    {
        NotPlay = 0,
        Playing = 1,
        PlayingOver = 2,
        error = 3
    }
    //播放类
    public class AudioHelper : IAudioHelper
    {
        public readonly Dictionary<string,Thread> th = new  Dictionary<string,Thread>();
        /// <summary>
        /// 播放状态
        /// </summary>
        public AudioPlayState AudioPlayState
        {
            get; set;
        }

        public EBD EBD
        {
            get; set;
        }

        public AudioModel AudioModel
        {
            get;
            set;
        }


        /// <summary>
        /// 播放状态接口
        /// </summary>
        public IPlayState PlayStateInterface { get; set; }

        public virtual AudioModel PlayReady(int type, string MQIns,string EBMInfoID)
        {
            try
            {
                if (MoreTime())
                {
                    //已经过了播放时间 不处理
                    PlayStateInterface.Untreated(AudioModel.XmlFilaPath, "0");
                    ServerForm.SetManager("已经过了消息结束时间，消息不再播放：结束时间："+ AudioModel.PlayEndTime, Color.Red);
                }
                else
                {
                    bool res = false;
                    string MQInstruction = MQIns;

                    string AreaString = CombinationArea();
                    ///获取TsCmd_ValueID
                    string TsCmd_ValueID = "";
                    //if (type == 2)//好坑  现场又坑我一把  20181219
                    //{
                    //    TsCmd_ValueID = GetTmcValue(AreaString);
                    //}
                    //else
                    //{
                    //    TsCmd_ValueID = AreaString;//注意此处的TsCmd_ValueID为12位的区域码  20181212与刘工一起核查
                    //}

                    TsCmd_ValueID = GetTmcValue(AreaString);
                    if (!string.IsNullOrEmpty(TsCmd_ValueID))
                    {
                        string result = InsertTsCmdStore(TsCmd_ValueID, AreaString, MQInstruction, AudioModel.PlayingTime.ToString(), AudioModel.PlayEndTime.ToString());

                        if (SingletonInfo.GetInstance().DicTsCmd_ID.ContainsKey(AreaString))
                        {
                            SingletonInfo.GetInstance().DicTsCmd_ID.Remove(AreaString);
                        }
                        SingletonInfo.GetInstance().DicTsCmd_ID.Add(AreaString, result);
                        if (!string.IsNullOrEmpty(result))
                        {
                            Thread thread;
                            string uuid = Guid.NewGuid().ToString("N");
                            thread = new Thread(delegate () {
                            AudioPlay(type, MQInstruction, result, TsCmd_ValueID, EBMInfoID);
                            }
                            );

                            SingletonInfo.GetInstance().DicPlayingThread[AudioModel.AeraCodeReal].Add(thread);
                            thread.IsBackground = true;
                            thread.Start();
                            while (true)
                            {
                                Thread.Sleep(200);
                                if (thread.ThreadState == ThreadState.Stopped)
                                {
                                    thread.Abort();
                                    GC.Collect();
                                    if (SingletonInfo.GetInstance().DicPlayingThread.ContainsKey(AudioModel.AeraCodeReal))
                                    {
                                        SingletonInfo.GetInstance().DicPlayingThread.Remove(AudioModel.AeraCodeReal);
                                        ServerForm.SetManager("播放过程线程stopped，DicPlayingThread中的字典被清理", Color.Green);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
             //  MessageBox.Show("DicPlayingThread.count:"+SingletonInfo.GetInstance().DicPlayingThread.Count.ToString() + ","+ ex.Message+ex.StackTrace);//  测试注释
               // return AudioModel;
            }
            return null;
        }

    
        /// <summary>
        ///已经过了播放时间   当前时间大于播放时间的情况为true
        /// </summary>
        /// <returns></returns>
        public virtual bool MoreTime()
        {
            if (Convert.ToDateTime(AudioModel.PlayEndTime) < DateTime.Now)
                return true;
            return false;
        }

        public EBD GetEBD(string path)
        {
            try
            {
                EBD ebd;
                using (FileStream fs = new FileStream(AudioModel.XmlFilaPath, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
                    String xmlInfo = sr.ReadToEnd();
                    xmlInfo = xmlInfo.Replace("xmlns:xs", "xmlns");
                    sr.Close();
                    xmlInfo = XmlSerialize.ReplaceLowOrderASCIICharacters(xmlInfo);
                    xmlInfo = XmlSerialize.GetLowOrderASCIICharacters(xmlInfo);
                    ebd = XmlSerialize.DeserializeXML<EBD>(xmlInfo);
                }
                return ebd;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("文转语读取文件{0},XML失败，错误原因:" + ex.Message));
            }
            return null;
        }
        /// <summary>
        /// 播放
        /// </summary>
        /// <returns></returns>
        public  bool AudioPlay(int type, string ParamValue, string TsCmd_ID, string TsCmd_ValueID,string EBMInfoID)
        {
            try
            {
               // ServerForm.SetManager("EBM开始时间: " + AudioModel.PlayingTime + "===>EBM结束时间: " + AudioModel.PlayEndTime, Color.Green);
                ServerForm.SetManager("播放开始时间: " + AudioModel.PlayingTime + "===>播放结束时间: " + AudioModel.PlayEndTime, Color.Green);
                ServerForm.SetManager("等待播放"+AudioModel.PlayingContent, Color.Green);
               
                EBD ebd = GetEBD(AudioModel.XmlFilaPath);
                string AreaString = CombinationArea();


                #region 未播放
                if (DateTime.Compare(AudioModel.PlayingTime, DateTime.Now)>0)
                {
                    AudioPlayState = AudioMessage.AudioPlayState.NotPlay;
                    lock (ServerForm.PlayBackObject)
                    {
                        ServerForm.PlayBack = ServerForm.PlaybackStateType.NotBroadcast;
                    }

                    string strSql = string.Format("update EBMInfo set EBMState = '{0}' where id='{1}'", "1", EBMInfoID);
                    mainForm.dba.UpdateDbBySQL(strSql);


                    Task.Factory.StartNew(() =>
                    {
                        PlayStateInterface.NotPlay(TsCmd_ID, AudioModel.XmlFilaPath, "1");
                        ServerForm.SetManager("反馈未播放状态", Color.Green);
                    });
                }
                #endregion 未播放代码
                //播放中
                #region 播放中

                while (true)
                {
                    DateTime current = DateTime.Now;
                    Thread.Sleep(1000);
                    if (DateTime.Compare(current, AudioModel.PlayingTime) > 0)//当前时间大于播放开始时间
                    {
                        ServerForm.SetManager("播放开始："+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Color.Green);
                        lock (ServerForm.PlayBackObject)
                        {
                            ServerForm.PlayBack = ServerForm.PlaybackStateType.Playback;

                        }
                        AudioPlayState = AudioMessage.AudioPlayState.Playing;
                       // MqSendOrder sendOrder = new MqSendOrder();
                        bool result = SendMQ.MqSendOrder.sendOrder.SendMq(ebd, type, ParamValue, TsCmd_ID, TsCmd_ValueID);
                        string strSql = string.Format("update EBMInfo set EBMState = '{0}' where id='{1}'", "2", EBMInfoID);
                        mainForm.dba.UpdateDbBySQL(strSql);

                        Task.Factory.StartNew(() =>
                        {
                            PlayStateInterface.Playing(TsCmd_ID, AudioModel.XmlFilaPath,"2", "播发中");
                            ServerForm.SetManager("反馈播发中状态", Color.Green);
                        });
                        
                        ServerForm.SetManager("播放中的反馈已发送", Color.Green);
                        break;
                    }
                }
                #endregion 播放中代码

                //播放完成
                #region 播放完
                ServerForm.SetManager("进入等待播放完成流程", Color.Green);
                while (true)
                {
                    Thread.Sleep(1000);
                   // ServerForm.SetManager("未到结束时间在播放过程中："+tmp.ToString(), Color.Green);
                    if (DateTime.Compare(DateTime.Now, AudioModel.PlayEndTime)< 0)//结束时间大于当前时间  
                    {
                        string MediaSql = "select TsCmd_ID,TsCmd_ExCute from  TsCmdStore where TsCmd_ID='" + TsCmd_ID + "'";
                        //  MediaSql = "select top(1)TsCmd_ID,TsCmd_XmlFile from  TsCmdStore where TsCmd_ValueID = '" + ebd.EBMStateRequest.EBM.EBMID + "' order by TsCmd_Date desc";
                        DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);

                        if (dtMedia!=null && dtMedia.Rows.Count>0)
                        {
                            if (dtMedia.Rows[0]["TsCmd_ExCute"].ToString().Contains("播放完毕"))
                            {
                                string strSql = string.Format("update EBMInfo set EBMState = '{0}' where id='{1}'", "3", EBMInfoID);
                                mainForm.dba.UpdateDbBySQL(strSql);

                                ServerForm.SetManager("播放结束", Color.Green);
                                lock (ServerForm.PlayBackObject)
                                {
                                    ServerForm.PlayBack = ServerForm.PlaybackStateType.PlayOut;

                                }
                                AudioPlayState = AudioMessage.AudioPlayState.PlayingOver;

                                Task.Factory.StartNew(() =>
                                {
                                    PlayStateInterface.PlayOver(TsCmd_ID, AudioModel.XmlFilaPath, "3", "开机/运行中");
                                    ServerForm.SetManager("反馈播放完成状态", Color.Green);
                                });
                                
                                if (SingletonInfo.GetInstance().DicTsCmd_ID.ContainsKey(AreaString))
                                {

                                    SingletonInfo.GetInstance().DicTsCmd_ID.Remove(AreaString);
                                }

                                if (SingletonInfo.GetInstance().DicPlayingThread.ContainsKey(AudioModel.AeraCodeReal))
                                {
                                    SingletonInfo.GetInstance().DicPlayingThread.Remove(AudioModel.AeraCodeReal);
                                  //  MessageBox.Show("文件播放完了->删除" + AudioModel.AeraCodeReal + "的字典值");
                                }

                                break;
                            }
                        }
                    }
                    else
                    {
                        lock (ServerForm.PlayBackObject)
                        {
                            ServerForm.PlayBack = ServerForm.PlaybackStateType.PlayOut;
                        }
                        ServerForm.SetManager("播放结束", Color.Green);

                        Task.Factory.StartNew(() =>
                        {
                            PlayStateInterface.PlayOver(TsCmd_ID, AudioModel.XmlFilaPath, "3", "开机/运行中");
                            ServerForm.SetManager("反馈播放完成状态", Color.Green);
                        });

                        string strSqlupdateEBMInfo = string.Format("update EBMInfo set EBMState = '{0}' where id='{1}'", "3", EBMInfoID);
                        mainForm.dba.UpdateDbBySQL(strSqlupdateEBMInfo);
                        //没播放完 但是文件时间到了
                        string strSql = "";
                        if (type == 1)
                        {
                            strSql = string.Format("update PLAYRECORD set PR_REC_STATUS = '{0}' where PR_SourceID='{1}'", "删除", TsCmd_ID);
                            mainForm.dba.UpdateDbBySQL(strSql);
                            string strSqlTsCmdStore = string.Format("update TsCmdStore set TsCmd_ExCute = '{0}' where TsCmd_ID='{1}'", "播放完毕", TsCmd_ID);
                            mainForm.dba.UpdateDbBySQL(strSqlTsCmdStore);
                        }
                        else if (type == 2)
                        {
                            strSql = string.Format("update PLAYRECORD set PR_REC_STATUS = '{0}' ", "删除");
                            mainForm.dba.UpdateDbBySQL(strSql);
                        }
                        
                        if (SingletonInfo.GetInstance().DicTsCmd_ID.ContainsKey(AreaString))
                        {
                            SingletonInfo.GetInstance().DicTsCmd_ID.Remove(AreaString);
                        }

                        if (SingletonInfo.GetInstance().DicPlayingThread.ContainsKey(AudioModel.AeraCodeReal))
                        {
                            SingletonInfo.GetInstance().DicPlayingThread.Remove(AudioModel.AeraCodeReal);
                           // MessageBox.Show("播放时间到了->删除"+ AudioModel.AeraCodeReal+"的字典值");
                        }
                        break;
                    }
                }
                #endregion 播放完代码
                GC.Collect();
             
                return true;
            }
            catch (Exception ex)
            {
                AudioPlayState = AudioMessage.AudioPlayState.error;
             //   MessageBox.Show(ex.Message + ex.StackTrace);
            }
            return false;
        }
        /// <summary>
        /// 组合
        /// </summary>
        /// <param name="PlayContent"></param>
        /// <returns></returns>
        public virtual string CombinationInstruction()
        {
            return null;
        }
        /// <summary>
        /// 停止播放
        /// </summary>
        /// <returns></returns>
        public virtual bool CancelPlay()
        {
            throw new NotImplementedException();
        }

        protected string CombinationArea()
        {

            string AreaCodeValue = "";
            if (AudioModel.PlayArea.Length > 1)
            {
                for (int i = 0; i < AudioModel.PlayArea.Length; i++)
                {
                    if (i == AudioModel.PlayArea.Length - 1)
                    {
                        AreaCodeValue += "'" + AudioModel.PlayArea[0] + "'";
                    }
                    else
                    {
                        AreaCodeValue += "'" + AudioModel.PlayArea[i] + "',";
                    }
                }
            }
            else
            {
                AreaCodeValue += "'" + AudioModel.PlayArea[0] + "'";
            }
            AreaCodeValue = AreaCodeValue.Substring(1);
            AreaCodeValue = AreaCodeValue.Substring(0, AreaCodeValue.Length - 1);
            return AreaCodeValue;
        }
        /// <summary>
        /// 获取TMC区域代码
        /// </summary>
        /// <param name="AreaCode"></param>
        /// <returns></returns>
       protected string GetTmcValue(string AreaCode)
        {
            string sqlQueryTsCmd_ValueID = "select ORG_ID from Organization where GB_CODE in (" + AreaCode + ")";
            DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(sqlQueryTsCmd_ValueID);
            string TsCmd_ValueID = "";
            if (dtMedia!=null&& dtMedia.Rows.Count > 0)
            {
                foreach (DataRow item in dtMedia.Rows)
                {
                    TsCmd_ValueID += item["ORG_ID"].ToString() + ",";
                }
            }
            if (string.IsNullOrEmpty(TsCmd_ValueID))
            {
                return null;
            }
            TsCmd_ValueID = TsCmd_ValueID.Substring(0, TsCmd_ValueID.Length - 1);
            return TsCmd_ValueID;
        }
        protected virtual string InsertTsCmdStore(string TsCmd_ValueID, string m_AreaCode, string Content, string sDateTime, string sEndDateTime)
        {
            try
            {
                string sqlstr = "insert into TsCmdStore(TsCmd_Type, TsCmd_Mode, TsCmd_UserID, TsCmd_ValueID, TsCmd_Params, TsCmd_Date, TsCmd_Status,TsCmd_EndTime,TsCmd_Note,TsCmd_PlayCount)" +
                                                                                 "values('音源播放', '区域', 1,'" + TsCmd_ValueID + "', '" + Content + "', '" + sDateTime + "', 0,'" + sEndDateTime + "'," + "'-1'" + ",'20'" +")";

                string TsCmdStoreID = mainForm.dba.UpdateDbBySQLRetID(sqlstr).ToString();
                if (Convert.ToInt32(TsCmdStoreID) > 0)
                {
                    return TsCmdStoreID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("插入TsCmdStore失败:" + ex.Message);
            }
            return null;
        }


    }
}
