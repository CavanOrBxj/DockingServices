using Apache.NMS;
using DockingServices.AudioMessage.MQAudio;
using DockingServices.Class;
using DockingServices.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;


namespace DockingServices
{
    public partial class ServerForm : Form
    {
        private bool RealAudioFlag = false;
        private IMessageConsumer m_consumer; //消费者
        private bool isConn = false; //是否已与MQ服务器正常连接
        Random rdMQFileName = new Random();
        object OMDRequestLock = new object();//OMDRequest业务锁
        public SendInfo send = new SendInfo();
        private string SEBDIDStatusFlag = "";
        private SendFileHttpPost postfile = new SendFileHttpPost();

        public static string sTarPathName = "";//全局变量
        public static string sTmptarFileName = "";//定义处理Tar包临时文件名

        Thread thTar = null;//解压回复线程
        Thread httpthread = null;//HTTP服务

        Thread dealwithtar = null;//处理tar包线程

        Thread thFeedBack = null;//回复状态线程

        Thread ccplayerthread = null;//播放CCPLAY线程

        Thread thBackup = null;//周期反馈线程

        private HttpServer httpServer = null;//HttpServer端//
        public static TarHelper tar = null;
        public static Object oLockFile = null;//文件操作锁
        private Object oLockTarFile = null;
        private Object oLockFeedBack = null;

        private Object oLockPlay = null;

        private List<string> xmlfilename = new List<string>();//获取Tar包里面的XML文件名列表（一个签名包，一个请求包）
        public static List<string> lRevFiles;
        private string sUrlAddress = string.Empty;//回复地址
        private bool bDeal = true;//线程处理是否停止处理
      //  private IniFiles SingletonInfo.GetInstance().serverini;
        //临时文件夹变量
        public string sSendTarName = "";//发送Tar包名字
        public static string sRevTarPath = "";//接收Tar包存放路径
        public static string sSendTarPath = "";//发送Tar包存放路径
        public static string sSourcePath = "";//需压缩文件路径
        public static string sUnTarPath = "";//Tar包解压缩路径
        public static string sAudioFilesFolder = "";//音频文件存放位置

        public string sServerIP = "";
        public string sServerPort = "";
        private IPAddress iServerIP;
        private int iServerPort = 0;

      
        public static mainForm mainFrm;
        //定时反馈执行结果
        List<string> lFeedBack;//反馈列表

        public static string strSourceAreaCode = "";
        public static string strSourceType = "";
        public static string strSourceName = "";
        public static string strSourceID = "";
        public static string strHBAREACODE = "";  //2016-04-03 电科院区域码对应

        //同步返回处理临时文件夹路径
        public static string strBeUnTarFolder = "";//预处理解压缩
        public static string strBeSendFileMakeFolder = "";//生成XML文件路径
        //心跳包变量
        public static string sHeartSourceFilePath = string.Empty;

        //SRV状态包变量
        public static string SRVSSourceFilePath = string.Empty;
        //SRV信息包变量
        public static string SRVISourceFilePath = string.Empty;

        //平台状态包变量
        public static string TerraceSSourceFilePath = string.Empty;
        //平台信息包变量
        public static string TerradcISourceFilePath = string.Empty;
        //定时心跳
        public static string TimesHeartSourceFilePath = string.Empty;

        public static string sEBMStateResponsePath = string.Empty;
        private DateTime dtLinkTime = new DateTime();//用于判断平台连接状态
        private const int OnOffLineInterval = 300;//离线在线间隔
        /*2016-03-31*/
        private List<string> listAreaCode;  //2016-04-01
        // private string AreaCode;            //2016-04-01
        private string EMBCloseAreaCode = "";//关闭区域逻辑代码
        //private string strAreaFlag = "";     //区域标志, 1代表命令发送到本区域，2代表上一级，3代表上上一级

        private int iAudioDelayTime = 0;//文转语延迟时间
        private int iMediaDelayTime = 0;//音频延迟时间
        private string bCharToAudio = "";  //1文转语，2 音频播放 
        public static EBD ebd;

        delegate void SetTextCallback(string text, Color color); //在界面上显示信息委托
        private string PlayType = "";

        //MQInfo
        private string MQUrl = "";
        private string CloudConsumer = "";
        private string CloudProducer = "";
        private string AudioCloudIP = "";

        //平台使用的PID序号
        private string FileAudioPIDID = "";
        private string ActulAudioCloudIP = "";
        private string TsCmdStoreID = "";//对应的TsCmdStore表中的ID

        public string m_strIP;
        public string m_Port;
        public string m_nAudioPID;
        public string m_nVedioPID;
        public string m_nVedioRat;
        public string m_nAuioRat;
        public string m_EBDID;
        public string m_EBMID;
        public string m_EBRID;
        public static string m_StreamPortURL;
        public static string m_UsbPwsSupport;
        public static string m_nAudioPIDID;
        public ccplayer ccplay;
        public static string m_AreaCode;
        public static string m_ccplayURL;
        //EBM是否人工审核
        private bool EBMVerifyState = false;

        //直播流播放启用ccplayer倒计时
        DateTime ccplayerStopTime = DateTime.Now.AddSeconds(-50);

        // System.Timers.Timer t = new System.Timers.Timer(30000);//心跳
        System.Timers.Timer t;
        System.Timers.Timer tSrvState;// = new System.Timers.Timer(600000); //终端状态
        System.Timers.Timer tTerraceState;//= new System.Timers.Timer(30000); //平台状态
        System.Timers.Timer tSrvInfo;// = new System.Timers.Timer(180000);  //终端信息
        System.Timers.Timer tTerraceInfrom; //= new System.Timers.Timer(180000);  //平台信息
        //System.Timers.Timer InfromActiveTime = new System.Timers.Timer(30000); //暂不使用
        System.Timers.Timer Tccplayer = new System.Timers.Timer(1000);
        private int NUMInfrom = 0;
        //MQ指令集合
        private List<Property> m_lstProperty = new List<Property>();
        public static UserInfo MQUserInfo = new UserInfo();//MQ指令用户信息

        private static FTPHelper ftphelper;

        [DllImport("TTSDLL.dll", EntryPoint = "TTSConvertOut", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void TTSConvertOut([In()] [MarshalAs(UnmanagedType.LPStr)]string szPath, [In()][MarshalAs(UnmanagedType.LPStr)] string szContent);

        public static PlaybackStateType PlayBack;
        public static object PlayBackObject = new object();
        public static FullDelegate.SetTextDelete SetManager;

        public static Dictionary<string, model.EBRPSS> EbrpssInfo = new Dictionary<string, model.EBRPSS>();
        public Dictionary<string, string> DicSrvType;//数据表SrvType中序号与名称的对应  

        public enum PlaybackStateType
        {
            NotBroadcast,
            Playback,
            PlayOut
        }

        public ServerForm()
        {
            try
            {
                InitializeComponent();
                SetManager = new FullDelegate.SetTextDelete(SetText);
                dtLinkTime = DateTime.Now.AddSeconds(-1 - OnOffLineInterval);
            }
            catch (Exception ex)
            {

            }
        }

        public void OnlineCheck(bool state)
        {
            this.Invoke((EventHandler)delegate
            {
                this.Text = "在线";
                dtLinkTime = DateTime.Now;//刷新时间
            });

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "服务-未开启")
            {
                btnStart.Text = "服务-已开启";
                txtServerPort.Enabled = false;
                    FindUserInfo("admin");
                if (SingletonInfo.GetInstance().IsFirstLoad)
                {
                    #region 上报平台、终端的全量信息  20190111   注意各个上报的时间间隔
                    //起线程
                    Thread t1 = new Thread(new ThreadStart(FirstTimeReportData));
                    t1.IsBackground = true;
                    t1.Start();
                    #endregion
                    SingletonInfo.GetInstance().IsFirstLoad = false;
                    SingletonInfo.GetInstance().serverini.WriteValue("PLATFORMINFO", "IsFirstLoad", "1");
                }
            }
            else
            {
                #region 停止服务
                try
                {
                    if (thTar != null)
                    {
                        thTar.Abort();
                    }
                    if (thFeedBack != null)
                    {
                        thFeedBack.Abort();
                    }
                    if (httpthread != null)
                    {
                        httpthread.Abort();
                        httpthread = null;
                    }
                    if (thBackup != null)
                    {
                        thBackup.Abort();
                        thBackup = null;
                    }
                    httpServer.StopListen();

                    //文转语Stop()
                    MQDLL.StopActiveMQ();
                }
                catch (Exception em)
                {
                    Log.Instance.LogWrite("停止线程错误：" + em.Message);
                }
                btnStart.Text = "服务-未开启";
                txtServerPort.Enabled = true;

                tTerraceInfrom.Enabled = false;
                tSrvInfo.Enabled = false;
                tTerraceState.Enabled = false;
                tSrvState.Enabled = false;
                t.Enabled = false;
                SingletonInfo.GetInstance().serverini.WriteValue("PLATFORMINFO", "SequenceCodes", SingletonInfo.GetInstance().SequenceCodes.ToString());
                return;
                #endregion End
            }
            if (txtServerPort.Text.Trim() != "")
            {
                if (int.TryParse(txtServerPort.Text, out iServerPort))
                {
                    if (iServerPort < 1 || iServerPort > 65535)
                    {
                        MessageBox.Show("无效的端口号，请重新输入！");
                        txtServerPort.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("非端口号，请重新输入！");
                    txtServerPort.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("服务端口号不能为空！");
                txtServerPort.Focus();
                return;
            }
            bDeal = true;//解析开关
            try
            {
                IPAddress[] ipArr;
                ipArr = Dns.GetHostAddresses(Dns.GetHostName());
                if (!ipArr.Contains(iServerIP))
                {
                    MessageBox.Show("IP设置错误，请重新设置后运行服务！");
                    return;
                }
                httpServer = new HttpServer(iServerIP, iServerPort);
            }
            catch (Exception es)
            {
                MessageBox.Show("可能端口已经使用中，请重新分配端口：" + es.Message);
                return;
            }

            httpthread = new Thread(new ThreadStart(httpServer.listen));
            httpthread.IsBackground = true;
            httpthread.Name = "HttpServer服务";
            httpthread.Start();
            //=================
            dealwithtar = new Thread(new ThreadStart(httpServer.listen));
            dealwithtar.IsBackground = true;
            dealwithtar.Name = "处理tar包线程";
            dealwithtar.Start();
            //=================
            thTar = new Thread(DealTar);
            thTar.IsBackground = true;
            thTar.Name = "解压回复线程";
            thTar.Start();
            //=================
            //thFeedBack = new Thread(FeedBackDeal);
            //thFeedBack.IsBackground = true;
            //thFeedBack.Name = "处理反馈线程";
            //thFeedBack.Start();
            //=================
            //thBackup = new Thread(AnswerBackUP);
            //thBackup.IsBackground = true;
            //thBackup.Name = "周期状态信息反馈";
            //thBackup.Start();

            ccplayerthread = new Thread(CPPPlayerThread);
            ccplayerthread.Start();
        }

        private void FirstTimeReportData()
        {
            PlatformInfoReported("Full");//主动上报全量平台信息
            Thread.Sleep(5000);
            PlatformstateInfoReported("Full");//主动上报全量平台状态
            Thread.Sleep(5000);
            PlatformEBRDTInfoReported("Full");//主动上报全量终端信息
            Thread.Sleep(10000);
            PlatformEBRDTStateReported("Full");//主动上报全量终端状态
          
        }

        private void ServerForm_Load(object sender, EventArgs e)  //页面参数初始化
        {
            bDeal = true;//解析开关
            oLockFile = new Object();//文件操作锁
            oLockTarFile = new object();
            oLockFeedBack = new object();//回复处理锁
            oLockPlay = new object();
            tar = new TarHelper();
            strSourceAreaCode = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "SourceAreaCode");
            strSourceID = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "SourceID");
            strSourceName = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "SourceName");
            strSourceType = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "SourceType");
           
            strHBAREACODE = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "HBAREACODE");
          //  AudioFlag = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "AudioFlag"); //********音频文件是否立即播放标志：1：立即播放 2：根据下发时间播放
          //  TEST = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "TEST");//********音频文件是否处于测试状态：test:测试状态，即收到的TAR包内xml的开始、结束时间无论是否过期，开始时间+1，结束时间+30
         //   TextFirst = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "TextFirst");//********文转语是否处于优先级1：文转语优先 2：语音优先
            PlayType = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "PlayType");//********1:推流播放 2:平台播放
            ccplay = new ccplayer();

            m_strIP = SingletonInfo.GetInstance().serverini.ReadValue("CCPLAY", "ccplay_strIP");
            m_Port = SingletonInfo.GetInstance().serverini.ReadValue("CCPLAY", "ccplay_Port");
            m_nAudioPID = SingletonInfo.GetInstance().serverini.ReadValue("CCPLAY", "ccplay_AudioPID");
            m_nVedioPID = SingletonInfo.GetInstance().serverini.ReadValue("CCPLAY", "ccplay_VedioPID");
            m_nVedioRat = SingletonInfo.GetInstance().serverini.ReadValue("CCPLAY", "ccplay_VedioRat");
            m_nAuioRat = SingletonInfo.GetInstance().serverini.ReadValue("CCPLAY", "ccplay_AuioRat");

            m_nAudioPIDID = SingletonInfo.GetInstance().serverini.ReadValue("CCPLAY", "ccplay_AudioPIDID");

            m_StreamPortURL = SingletonInfo.GetInstance().serverini.ReadValue("StreamPortURL", "URL");


            m_AreaCode = SingletonInfo.GetInstance().serverini.ReadValue("AREA", "AreaCode"); // 注释于20181010
            MQUrl = SingletonInfo.GetInstance().serverini.ReadValue("MQInfo", "ServerUrl");
            CloudConsumer = SingletonInfo.GetInstance().serverini.ReadValue("MQInfo", "CloudConsumer");
            CloudProducer = SingletonInfo.GetInstance().serverini.ReadValue("MQInfo", "CloudProducer");
            AudioCloudIP = SingletonInfo.GetInstance().serverini.ReadValue("MQInfo", "AudioCloudIP");

            FileAudioPIDID = SingletonInfo.GetInstance().serverini.ReadValue("CCPLAY", "ccplay_FileAuioRat");
            ActulAudioCloudIP = SingletonInfo.GetInstance().serverini.ReadValue("CCPLAY", "ccplay_AudioPIDID");
            EBMVerifyState = SingletonInfo.GetInstance().serverini.ReadValue("EBD", "EBMState").ToString() == "False" ? false : true;//true表示自动审核  false表示人工审核

            SingletonInfo.GetInstance().HeartbeatInterval = SingletonInfo.GetInstance().serverini.ReadValue("PLATFORMINFO", "HeartbeatInterval");
            SingletonInfo.GetInstance().PlatformInfoInterval = SingletonInfo.GetInstance().serverini.ReadValue("PLATFORMINFO", "PlatformInfoInterval");
            SingletonInfo.GetInstance().PlatformStateInterval= SingletonInfo.GetInstance().serverini.ReadValue("PLATFORMINFO", "PlatformStateInterval");
            SingletonInfo.GetInstance().EBRDTStateInterval= SingletonInfo.GetInstance().serverini.ReadValue("PLATFORMINFO", "EBRDTStateInterval");
            SingletonInfo.GetInstance().EBRDTInfoInterval = SingletonInfo.GetInstance().serverini.ReadValue("PLATFORMINFO", "EBRDTInfoInterval");

            SingletonInfo.GetInstance().SequenceCodes = Convert.ToInt32(SingletonInfo.GetInstance().serverini.ReadValue("PLATFORMINFO", "SequenceCodes"));
            SingletonInfo.GetInstance().IsFirstLoad = SingletonInfo.GetInstance().serverini.ReadValue("PLATFORMINFO", "IsFirstLoad") == "0" ? true : false;
            SingletonInfo.GetInstance().EBRPSStateCode = Convert.ToInt32(SingletonInfo.GetInstance().serverini.ReadValue("PLATFORMINFO", "StateCode"));
            SingletonInfo.GetInstance().RemoteFTPpath = SingletonInfo.GetInstance().serverini.ReadValue("PLATFORMINFO", "RemoteFTPpath");
            DicSrvType = new Dictionary<string, string>();
            GetSrvType();

            if (EBMVerifyState)
            {
                btn_Verify.Text = "人工审核-未开启";
                SingletonInfo.GetInstance().CheckEBMStatusFlag = "1";
            }
            else
            {
                btn_Verify.Text = "人工审核-已开启";
                SingletonInfo.GetInstance().CheckEBMStatusFlag = "0";
            }

            listAreaCode = new List<string>();  //2016-04-12
            try
            {
                iAudioDelayTime = int.Parse(SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "AudioDelayTime"));
            }
            catch
            {
                iAudioDelayTime = 1000;
            }
            try
            {
                iMediaDelayTime = int.Parse(SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "MediaDelayTime"));
            }
            catch
            {
                iMediaDelayTime = 1000;
            }
            /* 2016-04-03 */

            mainFrm = (this.ParentForm as mainForm);
            lRevFiles = new List<string>();
            lFeedBack = new List<string>();//反馈列表
            #region 设置处理文件夹路径Tar包存放文件夹路径
            try
            {
                //接收TAR包存放路径
                sRevTarPath = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "RevTarFolder");
                if (!Directory.Exists(sRevTarPath))
                {
                    Directory.CreateDirectory(sRevTarPath);//不存在该路径就创建
                }
                sTarPathName = sRevTarPath + "\\revebm.tar";//存放接收Tar包的路径及文件名。

                //接收到的Tar包解压存放路径
                sUnTarPath = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "UnTarFolder");
                if (!Directory.Exists(sUnTarPath))
                {
                    Directory.CreateDirectory(sUnTarPath);//不存在该路径就创建
                }
                //生成的需发送的XML文件路径
                sSourcePath = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "XmlBuildFolder");
                if (!Directory.Exists(sSourcePath))
                {
                    Directory.CreateDirectory(sSourcePath);//
                }
                //生成的TAR包，将要被发送的位置
                sSendTarPath = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "SndTarFolder");
                if (!Directory.Exists(sSendTarPath))
                {
                    Directory.CreateDirectory(sSendTarPath);
                }
                sAudioFilesFolder = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "AudioFileFolder");
                if (!Directory.Exists(sAudioFilesFolder))
                {
                    Directory.CreateDirectory(sAudioFilesFolder);
                }
                sHeartSourceFilePath = @Application.StartupPath + "\\HeartBeat";
                if (!Directory.Exists(sHeartSourceFilePath))
                {
                    Directory.CreateDirectory(sHeartSourceFilePath);
                }
                SRVSSourceFilePath = @Application.StartupPath + "\\SrvStateBeat";
                if (!Directory.Exists(SRVSSourceFilePath))
                {
                    Directory.CreateDirectory(SRVSSourceFilePath);
                }
                SRVISourceFilePath = @Application.StartupPath + "\\SrvInfromBeat";
                if (!Directory.Exists(SRVISourceFilePath))
                {
                    Directory.CreateDirectory(SRVISourceFilePath);
                }
                TerraceSSourceFilePath = @Application.StartupPath + "\\TerraceStateBeat";
                if (!Directory.Exists(TerraceSSourceFilePath))
                {
                    Directory.CreateDirectory(TerraceSSourceFilePath);
                }
                TerradcISourceFilePath = @Application.StartupPath + "\\TerracdInfromBeat";
                if (!Directory.Exists(TerradcISourceFilePath))
                {
                    Directory.CreateDirectory(TerradcISourceFilePath);
                }
                TimesHeartSourceFilePath = @Application.StartupPath + "\\TerracdInfromBeat";
                if (!Directory.Exists(TimesHeartSourceFilePath))
                {
                    Directory.CreateDirectory(TimesHeartSourceFilePath);
                }
                //反馈应急消息播发状态
                sEBMStateResponsePath = @Application.StartupPath + "\\EBMStateResponse";
                if (!Directory.Exists(sEBMStateResponsePath))
                {
                    Directory.CreateDirectory(sEBMStateResponsePath);
                }
                //预处理文件夹
                strBeUnTarFolder = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "BeUnTarFolder");
                if (!Directory.Exists(strBeUnTarFolder))
                {
                    Directory.CreateDirectory(strBeUnTarFolder);
                }
                strBeSendFileMakeFolder = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "BeXmlFileMakeFolder");
                if (!Directory.Exists(strBeSendFileMakeFolder))
                {
                    Directory.CreateDirectory(strBeSendFileMakeFolder);
                }
                //预处理文件夹
                if (strBeUnTarFolder == "" || strBeSendFileMakeFolder == "")
                {
                    MessageBox.Show("预处理文件夹路径不能为空，请设置好路径！");
                    this.Close();
                }

                if (sRevTarPath == "" || sSendTarPath == "" || sSourcePath == "" || sUnTarPath == "")
                {
                    MessageBox.Show("文件夹路径不能为空，请设置好路径！");
                    this.Close();
                }
            }
            catch (Exception em)
            {
                MessageBox.Show("文件夹设置错误，请重新：" + em.Message);
                this.Close();
            }
            #endregion 文件夹路径设置END

            sServerIP = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "ServerIP");
            txtServerPort.Text = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "ServerPort");
            if (sServerIP != "")
            {
                if (!IPAddress.TryParse(sServerIP, out iServerIP))
                {
                    MessageBox.Show("非有效的IP地址，关闭服务重新配置IP后启动！");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("服务IP不能为空，关闭服务重新配置IP后启动！");
                this.Close();
            }

            this.Invoke(new Action(() =>
            {
                this.Text = "离线";
            }));

           
            if (tim_ClearMemory.Enabled == false)
            {
                tim_ClearMemory.Enabled = true;
            }
            int HeartbeatInterval = Convert.ToInt32(SingletonInfo.GetInstance().HeartbeatInterval);
            t = new System.Timers.Timer(HeartbeatInterval * 1000);

            int PlatformInfoInterval = Convert.ToInt32(SingletonInfo.GetInstance().PlatformInfoInterval);
            tTerraceInfrom = new System.Timers.Timer(PlatformInfoInterval * 3600 * 1000);

            int PlatformStateInterval = Convert.ToInt32(SingletonInfo.GetInstance().PlatformStateInterval);
            tTerraceState = new System.Timers.Timer(PlatformStateInterval*60*1000);

            int EBRDTInfoInterval = Convert.ToInt32(SingletonInfo.GetInstance().EBRDTInfoInterval);
            tSrvInfo = new System.Timers.Timer(EBRDTInfoInterval*3600*1000);

            int EBRDTStateInterval= Convert.ToInt32(SingletonInfo.GetInstance().EBRDTStateInterval);
            tSrvState = new System.Timers.Timer(60*1000* EBRDTStateInterval);


            t.Elapsed += new System.Timers.ElapsedEventHandler(HeartUP);
            t.AutoReset = true;

            tSrvState.Elapsed += new System.Timers.ElapsedEventHandler(SrvStateUP);
            tSrvState.AutoReset = true;

            tTerraceState.Elapsed += new System.Timers.ElapsedEventHandler(TerraceStateUP);
            tTerraceState.AutoReset = true;

            tSrvInfo.Elapsed += new System.Timers.ElapsedEventHandler(SrvInfromUP);
            tSrvInfo.AutoReset = true;

            tTerraceInfrom.Elapsed += new System.Timers.ElapsedEventHandler(TerraceInfrom);
            tTerraceInfrom.AutoReset = true;

            Tccplayer.Elapsed += new System.Timers.ElapsedEventHandler(TimerCcplayer);
            Tccplayer.AutoReset = true;

            //信息主动上报
            //InfromActiveTime.Elapsed += new System.Timers.ElapsedEventHandler(InfromActive);
            //InfromActiveTime.AutoReset = true;

            //   InitAeracodeDic();

            ConnectMQServer();
            InitFTPServer();
            InitListIncrementalEBRDTState();
        }


        /// <summary>
        /// 获取终端名称
        /// </summary>
        private void GetSrvType()
        {
            string  MediaSql = "select *  from SrvType";
            DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);
            if (dtMedia.Rows.Count>0)
            {
                for (int i = 0; i < dtMedia.Rows.Count; i++)
                {
                    DicSrvType.Add(dtMedia.Rows[i]["SRV_ID"].ToString(), dtMedia.Rows[i]["SRV_DETAIL"].ToString());
                }
            }
        }

        /// <summary>
        /// 初始化终端状态增量信息列表
        /// </summary>
        public void InitListIncrementalEBRDTState()
        {
            string MediaSql = "select a.SRV_PHYSICAL_CODE,a.SRV_LOGICAL_CODE_GB,a.SRV_RMT_STATUS,b.powersupplystatus from SRV a inner join Srv_Status b on a.SRV_PHYSICAL_CODE = b.srv_physical_code";
            DataTable dtMediaSRV = mainForm.dba.getQueryInfoBySQL(MediaSql);
            for (int i = 0; i < dtMediaSRV.Rows.Count; i++)
            {
                IncrementalEBRDTState tmpone = new IncrementalEBRDTState();
                tmpone.powersupplystatus = dtMediaSRV.Rows[i]["powersupplystatus"].ToString();
                tmpone.SRV_LOGICAL_CODE_GB = dtMediaSRV.Rows[i]["SRV_LOGICAL_CODE_GB"].ToString();
                tmpone.SRV_PHYSICAL_CODE = dtMediaSRV.Rows[i]["SRV_PHYSICAL_CODE"].ToString();
                tmpone.SRV_RMT_STATUS = dtMediaSRV.Rows[i]["SRV_RMT_STATUS"].ToString();
                SingletonInfo.GetInstance().ListIncrementalEBRDTState.Add(tmpone);
            }
        }

        public void CPPPlayerThread()
        {
            try
            {
                while (true)
                {
                    if (ccplay.m_bPlayFlag)
                    {
                        ccplay.init("", m_ccplayURL, m_strIP, m_Port, "pipe", "EVENT", m_nAudioPID, m_nVedioPID, m_nVedioRat, m_nAuioRat);
                        ccplay.CreatePipeandEvent("pipename", "eventname");
                        ccplay.CreateCPPPlayer();
                        Thread.Sleep(2000);
                        ccplay.StopCPPPlayer();
                        //string strSql = "delete  from PLAYRECORD";
                        //mainForm.dba.UpdateDbBySQL(strSql);
                        ccplay.m_bPlayFlag = false;
                    }
                    Thread.Sleep(500);
                }

            }
            catch (Exception es)
            {
                Log.Instance.LogWrite(es.Message);
            }
        }

        /// <summary>
        /// 从HttpServer得到的Tar包获取数据并解析；以后tar包弄成List处理
        /// </summary>
        private void DealTar()
        {
            List<string> lDealTarFiles = new List<string>();
            List<string> AudioFileListTmp = new List<string>();//收集的音频文件列表
            List<string> AudioFileList = new List<string>();//收集的音频文件列表
            while (bDeal)
            {
                //没有Tar包不处理
                if (lRevFiles.Count == 0)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                else
                {
                    lock (oLockTarFile)
                    {
                        if (lRevFiles.Count > 0)
                        {
                            lDealTarFiles.AddRange(lRevFiles);
                            lRevFiles.Clear();
                        }
                    }
                }
                this.Invoke((EventHandler)delegate
                {
                    this.Text = "在线";
                    dtLinkTime = DateTime.Now;//刷新时间
                });
                #region 处理Tar包
                if (lDealTarFiles.Count == 0)
                {
                    continue;//没有处理文件包不处理
                }
                try
                {
                    while (lDealTarFiles.Count > 0)
                    {
                        SetText("解压文件：" + lDealTarFiles[0].ToString(), Color.Green);
                        try
                        {
                            #region 解压
                            if (File.Exists(lDealTarFiles[0]))
                            {
                                try
                                {
                                    DeleteFolder(sUnTarPath);
                                    tar.UnpackTarFiles(lDealTarFiles[0], sUnTarPath);
                                    //把压缩包解压到专门存放接收到的XML文件的文件夹下
                                    SetText("解压文件：" + lDealTarFiles[0].ToString() + "成功", Color.Green);
                                }
                                catch (Exception exa)
                                {
                                    SetText("删除解压文件夹：" + sUnTarPath + "文件失败!错误信息：" + exa.Message, Color.Red);
                                }
                            }
                            #endregion 解压
                        }
                        catch (Exception ex)
                        {
                            Log.Instance.LogWrite("解压出错：" + ex.Message);
                        }
                        //处理XML文件
                        try
                        {
                            string[] xmlfilenames = Directory.GetFiles(sUnTarPath, "*.xml");//从解压XML文件夹下获取解压的XML文件名
                            string sTmpFile = string.Empty;
                            string sAnalysisFileName = "";
                            string sSignFileName = "";

                            for (int i = 0; i < xmlfilenames.Length; i++)
                            {
                                sTmpFile = Path.GetFileName(xmlfilenames[i]);
                                if (sTmpFile.ToUpper().IndexOf("EBDB") > -1 && sTmpFile.ToUpper().IndexOf("EBDS_EBDB") < 0)
                                {
                                    sAnalysisFileName = xmlfilenames[i];
                                }
                            }
                            DeleteFolder(sSourcePath);//删除原有XML发送文件的文件夹下的XML

                            if (sAnalysisFileName != "")
                            {
                                using (FileStream fs = new FileStream(sAnalysisFileName, FileMode.Open))
                                {
                                    StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
                                    String xmlInfo = sr.ReadToEnd();
                                    xmlInfo = xmlInfo.Replace("xmlns:xs", "xmlns");
                                    sr.Close();
                                    xmlInfo = XmlSerialize.ReplaceLowOrderASCIICharacters(xmlInfo);
                                    xmlInfo = XmlSerialize.GetLowOrderASCIICharacters(xmlInfo);
                                    ebd = XmlSerialize.DeserializeXML<EBD>(xmlInfo);
                                    sUrlAddress = SingletonInfo.GetInstance().SendTarAddress;  //异步反馈的地址
                                    #region 根据EBD类型处理XML文件
                                    switch (ebd.EBDType)
                                    {
                                        case "EBM":
                                            #region 业务处理
                                            string sqlstr = "";
                                            string strMsgType = ebd.EBM.MsgBasicInfo.MsgType; //播发类型
                                            string strAuxiliaryType = "";
                                            if (strMsgType == "1")
                                            {
                                                //播放
                                                #region 获取播放类型  1推流播放  2 文件播放
                                                if (ebd.EBM.MsgContent != null)
                                                {
                                                    if (ebd.EBM.MsgContent.Auxiliary != null)
                                                    {
                                                        //辅助数据不为空 文件播放或者是直播流
                                                        strAuxiliaryType = ebd.EBM.MsgContent.Auxiliary.AuxiliaryType;
                                                        if (strAuxiliaryType == "61")
                                                        {
                                                            //直播流
                                                            PlayType = "1";
                                                        }
                                                        else
                                                        {
                                                            //文件播放
                                                            PlayType = "2";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //辅助数据为空 为文转语
                                                        ebd.EBM.MsgContent.Auxiliary = new Auxiliary();
                                                        ebd.EBM.MsgContent.Auxiliary.AuxiliaryType = "3";
                                                        strAuxiliaryType = "3";
                                                        ebd.EBM.MsgContent.Auxiliary.AuxiliaryDesc = "文本转语";
                                                        PlayType = "1";
                                                    }
                                                }
                                                #endregion

                                                #region 根据区域码播放 
                                                if (!string.IsNullOrEmpty(ebd.EBM.MsgContent.AreaCode))
                                                {
                                                    #region 处理消息
                                                    if (true)
                                                    {
                                                        #region 处理应急内容
                                                        AudioFileListTmp.Clear();
                                                        AudioFileList.Clear();
                                                        string[] mp3files = Directory.GetFiles(sUnTarPath, "*.mp3");
                                                        AudioFileListTmp.AddRange(mp3files);
                                                        if (AudioFileListTmp.Count > 0)
                                                        {
                                                            #region 根据策略判断SingletonInfo.GetInstance().CheckEBMStatusFlag的值  "0"表示需要融合平台审核 "1"表示不需要融合平台审核
                                                            if (!EBMVerifyState)
                                                            {
                                                                SingletonInfo.GetInstance().CheckEBMStatusFlag = StrategyChenck(ebd);
                                                            }
                                                            else
                                                            {
                                                                SingletonInfo.GetInstance().CheckEBMStatusFlag = "1";
                                                            }
                                                            #endregion

                                                            #region  发送给融合平台关于审核信息
                                                            //待审核数据插入数据库
                                                            string lab_EBMType = "音频文件播发";

                                                            sqlstr = "insert into CheckEBMData(EBDID, EBDDID, CodeA, NameA, EBMID, SentTime,EBMStartTime,EBMEndTime, EBMTitle,EBMType,EBMDesc,EBMCode,Severity,EBMUrl,CheckStatus)" +
                        "values('" + ebd.SRC.EBRID + "','" + ebd.EBDID + "', '" + ebd.EBM.MsgBasicInfo.SenderCode + "', '" + ebd.EBM.MsgBasicInfo.SenderName + "','" + ebd.EBM.EBMID + "', '" + ebd.EBM.MsgBasicInfo.SendTime + "','" + ebd.EBM.MsgBasicInfo.StartTime + "','" + ebd.EBM.MsgBasicInfo.EndTime + "','" + ebd.EBM.MsgContent.MsgTitle + "','" + lab_EBMType + "','" + ebd.EBM.MsgContent.MsgDesc + "','" + ebd.EBM.MsgContent.AreaCode + "','" + ebd.EBM.MsgBasicInfo.Severity + "','" + ebd.EBM.MsgContent.Auxiliary.AuxiliaryDesc + "','" + SingletonInfo.GetInstance().CheckEBMStatusFlag + "') SELECT CAST(scope_identity() AS int)";
                                                            ebd.CheckEBMDataID = mainForm.dba.InsertDbBySQLRetID(sqlstr).ToString();

                                                            //MQ发送消息审核  
                                                            CheckEBMDataMQSend(ebd);
                                                            #endregion

                                                        }
                                                        else //文转语
                                                        {
                                                            #region 根据策略判断SingletonInfo.GetInstance().CheckEBMStatusFlag的值  "0"表示需要融合平台审核 "1"表示不需要融合平台审核
                                                            if (!EBMVerifyState)
                                                            {
                                                                SingletonInfo.GetInstance().CheckEBMStatusFlag = StrategyChenck(ebd);
                                                            }
                                                            else
                                                            {
                                                                SingletonInfo.GetInstance().CheckEBMStatusFlag = "1";
                                                            }
                                                            #endregion

                                                            #region 发送给融合平台关于审核的信息
                                                            //待审核数据插入数据库
                                                            string lab_EBMType = "文本转语音播发";
                                                            sqlstr = "insert into CheckEBMData(EBDID, EBDDID, CodeA, NameA, EBMID, SentTime,EBMStartTime,EBMEndTime, EBMTitle,EBMType,EBMDesc,EBMCode,Severity,EBMUrl,CheckStatus)" +
                        "values('" + ebd.SRC.EBRID + "','" + ebd.EBDID + "', '" + ebd.EBM.MsgBasicInfo.SenderCode + "', '" + ebd.EBM.MsgBasicInfo.SenderName + "','" + ebd.EBM.EBMID + "', '" + ebd.EBM.MsgBasicInfo.SendTime + "','" + ebd.EBM.MsgBasicInfo.StartTime + "','" + ebd.EBM.MsgBasicInfo.EndTime + "','" + ebd.EBM.MsgContent.MsgTitle + "','" + lab_EBMType + "','" + ebd.EBM.MsgContent.MsgDesc + "','" + ebd.EBM.MsgContent.AreaCode + "','" + ebd.EBM.MsgBasicInfo.Severity + "','" + ebd.EBM.MsgContent.Auxiliary.AuxiliaryDesc + "','" + SingletonInfo.GetInstance().CheckEBMStatusFlag + "') SELECT CAST(scope_identity() AS int)";
                                                            ebd.CheckEBMDataID = mainForm.dba.InsertDbBySQLRetID(sqlstr).ToString();

                                                            //MQ发送消息审核  
                                                            CheckEBMDataMQSend(ebd);
                                                            #endregion


                                                            if (!EBMVerifyState && SingletonInfo.GetInstance().CheckEBMStatusFlag == "0")//
                                                            {
                                                                ListViewItem listItem = new ListViewItem();
                                                                listItem.Text = (list_PendingTask.Items.Count + 1).ToString();
                                                                listItem.SubItems.Add(lDealTarFiles[0]);
                                                                this.Invoke(new Action(() => { list_PendingTask.Items.Add(listItem); }));
                                                                lDealTarFiles.RemoveAt(0);//无论是否成功，都移除
                                                                continue;
                                                            }


                                                            string xmlFile = Path.GetFileName(sAnalysisFileName);
                                                            string xmlFilePath = sAudioFilesFolder + "\\" + xmlFile;

                                                            #region  保存EBMInfo 新增于20181214
                                                            string ebminfoid = "";
                                                            ebminfoid = SaveEBD(ebd);
                                                            #endregion

                                                            //先考虑文转语
                                                            PlayElements pe = new PlayElements();
                                                            pe.EBDITEM = ebd;
                                                            pe.sAnalysisFileName = sAnalysisFileName;
                                                            pe.targetPath = "";
                                                            pe.xmlFilePath = xmlFilePath;
                                                            pe.EBMInfoID = ebminfoid;
                                                            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(PlaybackProcess);

                                                            Thread myThread = new Thread(ParStart);
                                                            myThread.IsBackground = true;

                                                            if (SingletonInfo.GetInstance().DicPlayingThread.ContainsKey(ebd.EBM.MsgContent.AreaCode))
                                                            {
                                                                SingletonInfo.GetInstance().DicPlayingThread.Remove(ebd.EBM.MsgContent.AreaCode);
                                                            }

                                                            List<Thread> ThreadList = new List<Thread>();
                                                            ThreadList.Add(myThread);
                                                            SingletonInfo.GetInstance().DicPlayingThread.Add(ebd.EBM.MsgContent.AreaCode, ThreadList);
                                                            myThread.Start(pe);
                                                        }
                                                        #endregion
                                                    }
                                                    #endregion

                                                    #region 移动音频文件到文件库上
                                                    try
                                                    {
                                                        AudioFileListTmp.Clear();
                                                        AudioFileList.Clear();
                                                        string[] mp3files = Directory.GetFiles(sUnTarPath, "*.mp3");
                                                        AudioFileListTmp.AddRange(mp3files);
                                                        string[] wavfiles = Directory.GetFiles(sUnTarPath, "*.wav");
                                                        AudioFileListTmp.AddRange(wavfiles);

                                                        #region  把音频文件上传到ftp服务器
                                                        if (AudioFileListTmp.Count > 0)
                                                        {
                                                            string ftppath = ebd.EBM.MsgContent.Auxiliary.AuxiliaryDesc;
                                                            string path = AudioFileListTmp[0];
                                                            ftphelper.UploadFile(path, ftppath);
                                                        }
                                                        #endregion

                                                        if (!EBMVerifyState && AudioFileListTmp.Count > 0 && SingletonInfo.GetInstance().CheckEBMStatusFlag == "0")//
                                                        {
                                                            ListViewItem listItem = new ListViewItem();
                                                            listItem.Text = (list_PendingTask.Items.Count + 1).ToString();
                                                            listItem.SubItems.Add(lDealTarFiles[0]);
                                                            this.Invoke(new Action(() => { list_PendingTask.Items.Add(listItem); }));
                                                            lDealTarFiles.RemoveAt(0);//无论是否成功，都移除
                                                            continue;
                                                        }
                                                        string sTmpDealFile = string.Empty;
                                                        string targetPath = string.Empty;

                                                        string sStartTime = ebd.EBM.MsgBasicInfo.StartTime;
                                                        string sEndDateTime = ebd.EBM.MsgBasicInfo.EndTime;

                                                        string xmlFilePath = "";

                                                        string xmlFile = Path.GetFileName(sAnalysisFileName);
                                                        xmlFilePath = sAudioFilesFolder + "\\" + xmlFile;
                                                        System.IO.File.Copy(sAnalysisFileName, xmlFilePath, true);

                                                        lDealTarFiles.RemoveAt(0);
                                                        for (int ai = 0; ai < AudioFileListTmp.Count; ai++)
                                                        {
                                                            sTmpDealFile = Path.GetFileName(AudioFileListTmp[ai]);
                                                            targetPath = sAudioFilesFolder + "\\" + sTmpDealFile;
                                                            System.IO.File.Copy(AudioFileListTmp[ai], targetPath, true);
                                                            AudioFileList.Add(targetPath);


                                                            #region  保存EBMInfo 新增于20181214
                                                            string ebminfoid = "";
                                                            ebminfoid = SaveEBD(ebd);
                                                            #endregion

                                                            #region 音频播放 20181212
                                                            if (SingletonInfo.GetInstance().DicPlayingThread.ContainsKey(ebd.EBM.MsgContent.AreaCode))
                                                            {
                                                                foreach (var item in SingletonInfo.GetInstance().DicPlayingThread[ebd.EBM.MsgContent.AreaCode])
                                                                {
                                                                    item.Abort();
                                                                }
                                                                SingletonInfo.GetInstance().DicPlayingThread.Remove(ebd.EBM.MsgContent.AreaCode);

                                                                SetText("开播前删除字典值：" + ebd.EBM.MsgContent.AreaCode + "-->" + SingletonInfo.GetInstance().DicPlayingThread.Count.ToString(), Color.Purple);
                                                            }

                                                            PlayElements pe = new PlayElements();
                                                            pe.EBDITEM = ebd;
                                                            pe.sAnalysisFileName = sAnalysisFileName;
                                                            pe.targetPath = SingletonInfo.GetInstance().RemoteFTPpath + "\\" + ebd.EBM.MsgContent.Auxiliary.AuxiliaryDesc;
                                                            pe.xmlFilePath = xmlFilePath;
                                                            pe.EBMInfoID = ebminfoid;
                                                            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(PlaybackProcess);
                                                            Thread myThread = new Thread(ParStart);
                                                            myThread.IsBackground = true;
                                                            List<Thread> ThreadList = new List<Thread>();
                                                            ThreadList.Add(myThread);

                                                            SingletonInfo.GetInstance().DicPlayingThread.Add(ebd.EBM.MsgContent.AreaCode, ThreadList);
                                                            myThread.Start(pe);
                                                            #endregion----------------------------------
                                                        }
                                                    }
                                                    catch (Exception fex)
                                                    {
                                                        Log.Instance.LogWrite(fex.Message);
                                                    }
                                                    #endregion End

                                                    AudioFileList.Clear();
                                                }
                                                #endregion
                                            }
                                            else if (strMsgType == "2")
                                            {
                                                //停播

                                                string EBMIDtmp = ebd.EBM.EBMID;
                                                string sqlQueryTsCmd_ValueID = "select AreaCode from EBMInfo where EBMID = " + EBMIDtmp + "";
                                                DataTable dtMediaAreaCode = mainForm.dba.getQueryInfoBySQL(sqlQueryTsCmd_ValueID);
                                                string AreaString = "";

                                                if (dtMediaAreaCode != null)
                                                {
                                                    if (dtMediaAreaCode.Rows.Count > 0)
                                                    {
                                                        AreaString = dtMediaAreaCode.Rows[0]["AreaCode"].ToString();
                                                        if (SingletonInfo.GetInstance().DicTsCmd_ID.ContainsKey(AreaString))
                                                        {
                                                            SetText("停止播发：" + DateTime.Now.ToString(), Color.Red);
                                                            string PR_SourceID = SingletonInfo.GetInstance().DicTsCmd_ID[AreaString];
                                                            string strSql = string.Format("update PLAYRECORD set PR_REC_STATUS = '{0}' where PR_SourceID='{1}'", "删除", PR_SourceID);
                                                            strSql += " update EBMInfo set EBMState=5 where EBMID='" + EBMIDtmp + "' ";//更新EBMInfo的数据   20190114
                                                            strSql += "delete from InfoVlaue";  //待确认   20190114  是否需要加这一句
                                                            mainForm.dba.UpdateDbBySQL(strSql);
                                                            Tccplayer.Enabled = false;
                                                            RealAudioFlag = false;//标记为已经执行
                                                            if (SingletonInfo.GetInstance().DicTsCmd_ID.ContainsKey(AreaString))
                                                            {
                                                                if (SingletonInfo.GetInstance().DicTsCmd_ID.Remove(AreaString))
                                                                {
                                                                    //SetText("去除DicTsCmd_ID的键：" + AreaString, Color.Black);
                                                                }
                                                            }
                                                            if (SingletonInfo.GetInstance().DicPlayingThread.ContainsKey(AreaString))
                                                            {
                                                                foreach (var item in SingletonInfo.GetInstance().DicPlayingThread[AreaString])
                                                                {
                                                                    Application.DoEvents();
                                                                    item.Abort();
                                                                    GC.Collect();
                                                                }
                                                                SingletonInfo.GetInstance().DicPlayingThread.Remove(AreaString);
                                                            }
                                                            lDealTarFiles.RemoveAt(0);//无论是否成功，都移除  先注释 20180820

                                                            #region  要回包告诉上级平台 播发取消了   20190122

                                                            #endregion
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("未找到播放记录！");
                                                        break;
                                                    }
                                                    
                                                }
                                                else
                                                {
                                                    MessageBox.Show("同一消息播发多次，且数据未变化！");
                                                    break;
                                                }
                                            }
                                            #endregion End
                                            break;
                                        case "EBMStreamPortRequest":
                                            #region EBM实时流
                                            try
                                            {
                                                XmlDocument xmlDoc = new XmlDocument();
                                                responseXML rp = new responseXML();
                                                rp.SourceAreaCode = ServerForm.strSourceAreaCode;
                                                rp.SourceType = ServerForm.strSourceType;
                                                rp.SourceName = ServerForm.strSourceName;
                                                rp.SourceID = ServerForm.strSourceID;
                                                rp.sHBRONO = SingletonInfo.GetInstance().CurrentResourcecode;


                                                string fName = "10" + rp.sHBRONO + BBSHelper.GetSequenceCodes();
                                                xmlDoc = rp.EBMStreamResponse(fName, ServerForm.m_StreamPortURL);
                                                UnifyCreateTar(xmlDoc, fName);
                                                string sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + fName + ".tar";
                                                string xmlSignFileName = "\\EBDB_" + fName + ".xml";
                                                CreateXML(xmlDoc, ServerForm.strBeSendFileMakeFolder + xmlSignFileName);
                                                send.address = SingletonInfo.GetInstance().SendTarAddress;
                                                send.fileNamePath = sHeartBeatTarName;
                                                postfile.UploadFilesByPostThread(send);

                                                ////进行签名
                                                //ServerForm.mainFrm.GenerateSignatureFile(ServerForm.strBeSendFileMakeFolder, fName);
                                                //ServerForm.tar.CreatTar(ServerForm.strBeSendFileMakeFolder, ServerForm.sSendTarPath, fName);//使用新TAR
                                                //string sSendTarName = ServerForm.sSendTarPath + "\\EBDT_" + fName + ".tar";
                                            }
                                            catch (Exception esb)
                                            {
                                                Console.WriteLine("401:" + esb.Message);
                                            }
                                            #endregion End

                                            ListViewItem OMDRequestItemPort = new ListViewItem();
                                            OMDRequestItemPort.Text = "实时流端口请求";
                                            this.Invoke(new Action(() => { list_OMDRequest.Items.Add(OMDRequestItemPort); }));
                                            lDealTarFiles.RemoveAt(0);//无论是否成功，都移除  先注释 20180820
                                            break;
                                        case "EBMStateRequest":
                                            lock (OMDRequestLock)
                                            {
                                                lDealTarFiles.RemoveAt(0);//无论是否成功，都移除  先注释 20180820
                                                EBMStateRequest(ebd);
                                                Console.WriteLine(">>>>>>>>>>>>>>>>>>>EBMStateRequest");
                                            }
                                            ListViewItem OMDRequestItemEBMStateRequest = new ListViewItem();
                                            OMDRequestItemEBMStateRequest.Text = "播发状态请求";
                                            this.Invoke(new Action(() => { list_OMDRequest.Items.Add(OMDRequestItemEBMStateRequest); }));
                                        
                                            break;
                                        case "OMDRequest":
                                            #region 运维请求反馈
                                            string strOMDType = ebd.OMDRequest.OMDType;
                                            try
                                            {
                                                XmlDocument xmlStateDoc = new XmlDocument();
                                                responseXML rState = new responseXML();
                                                string frdStateName = "10" + SingletonInfo.GetInstance().CurrentResourcecode + BBSHelper.GetSequenceCodes();
                                                string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";
                                                lock (OMDRequestLock)
                                                {
                                                    lDealTarFiles.RemoveAt(0);//无论是否成功，都移除  先注释 20180820
                                                    TarOMRequest(xmlStateDoc, rState, strOMDType, frdStateName, xmlEBMStateFileName, ebd);
                                                }
                                            }
                                            catch (Exception h)
                                            {
                                                Log.Instance.LogWrite("运维信息上报异常:" + h.Message);
                                            }
                                            #endregion End
                                            break;
                                        default:
                                            this.Invoke((EventHandler)delegate
                                            {
                                                this.Text = "在线";
                                                dtLinkTime = DateTime.Now;//刷新时间
                                            });
                                            lDealTarFiles.RemoveAt(0);//无论是否成功，都移除  先注释 20180820
                                            break;
                                    }
                                    #endregion 根据EBD类型处理XML文件
                                }
                            }
                           // lDealTarFiles.RemoveAt(0);//无论是否成功，都移除
                        }
                        catch (Exception dxml)
                        {
                            Log.Instance.LogWrite("处理XML:" + dxml.Message);
                        }
                    }//for循环处理接收到的Tar包
                }
                catch (Exception em)
                {
                    Log.Instance.LogWrite(em.Message);
                }
                #endregion 处理Tar包

            }//while循环处理解压缩文件
        }

        protected string CombinationArea(string[] PlayArea)
        {
            string AreaCodeValue = "";
            if (PlayArea.Length > 1)
            {
                for (int i = 0; i < PlayArea.Length; i++)
                {
                    if (i == PlayArea.Length - 1)
                    {
                        AreaCodeValue += "'" + PlayArea[0] + "'";
                    }
                    else
                    {
                        AreaCodeValue += "'" + PlayArea[i] + "',";
                    }
                }
            }
            else
            {
                AreaCodeValue += "'" + PlayArea[0] + "'";
            }
            return AreaCodeValue;
        }

        public void PlaybackProcess(object o)
        {
            try
            {
                PlayElements pe = (PlayElements)o;
                EBD ebd = pe.EBDITEM;
                string sAnalysisFileName = pe.sAnalysisFileName;
                string xmlFilePath = pe.xmlFilePath;
                string targetPath = pe.targetPath;

                AudioModel audio = new AudioModel();
                audio.PlayingTime = Convert.ToDateTime(ebd.EBM.MsgBasicInfo.StartTime);
                audio.PlayEndTime = Convert.ToDateTime(ebd.EBM.MsgBasicInfo.EndTime);//测试注释20181219
                string xmlFile = Path.GetFileName(sAnalysisFileName);
                audio.XmlFilaPath = xmlFilePath;

                audio.PlayingContent = targetPath;
                audio.AeraCodeReal = ebd.EBM.MsgContent.AreaCode;
                audio.MsgTitleNew = ebd.EBM.MsgContent.MsgTitle;
                audio.EBMID = ebd.EBM.EBMID;
                audio.PlayArea = ebd.EBM.MsgContent.AreaCode.Split(',');
                audio.MsgDesc = ebd.EBM.MsgContent.MsgDesc.Trim();
                MQAudioHelper mqaudio = new MQAudioHelper(audio);
                mqaudio.PlayReady(pe.EBMInfoID);
            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message + ex.StackTrace);
            }

        }

        /// <summary>
        /// 根据策略决定当前消息是否需要审核  返回"0"表示需要审核 返回"1"表示不需要审核
        /// </summary>
        /// <param name="EbdInfo"></param>
        /// <returns></returns>
        public string StrategyChenck(EBD EbdInfo)
        {
            string EBMStatusFlag = "1";
            string severity = EbdInfo.EBM.MsgBasicInfo.Severity;
            if (SingletonInfo.GetInstance().audit.TimeList.Count > 0)
            {
                //有一个条件（消息时间，消息等级）不满足就不审核
                foreach (timestrategies item in SingletonInfo.GetInstance().audit.TimeList)
                {
                    string dt = DateTime.Now.ToLongTimeString();
                    if (DateTime.Parse(item.EndTime) > DateTime.Parse(dt) && DateTime.Parse(dt) > DateTime.Parse(item.StartTime))
                    {
                        // MessageBox.Show("在时间段内");
                        //在时间段内
                        switch (item.EvenType)
                        {
                            case "0":
                                if (severity == "0")
                                {
                                    EBMStatusFlag = "0";
                                }
                                break;
                            case "1":
                                if (severity == "1")
                                {
                                    EBMStatusFlag = "0";
                                }
                                break;
                            case "2":
                                if (severity == "2")
                                {
                                    EBMStatusFlag = "0";
                                }
                                break;
                            case "3":
                                if (severity == "3")
                                {
                                    EBMStatusFlag = "0";
                                }
                                break;
                            case "4":
                                if (severity == "4")
                                {
                                    EBMStatusFlag = "0";
                                }
                                break;
                            case "100":
                                EBMStatusFlag = "0";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        // MessageBox.Show("不在时间段内");
                        EBMStatusFlag = "1";
                    }
                    if (EBMStatusFlag == "1")
                    {
                        break;
                    }
                }
            }
            else
            {
                //没有添加审核策略 同时人工审核开关已经打开 则必须要审核
                EBMStatusFlag = "0";
            }

            return EBMStatusFlag;
        }

        private void CheckEBMDataMQSend(EBD EbdInfo)
        {
            List<Property> m_lstProperty = new List<Property>();
            Property property;
            property.name = "ID";
            property.value = EbdInfo.CheckEBMDataID;
            m_lstProperty.Add(property);

            property.name = "EBDID";
            property.value = EbdInfo.SRC.EBRID;
            m_lstProperty.Add(property);

            property.name = "EBDDID";
            property.value = EbdInfo.EBDID;
            m_lstProperty.Add(property);

            property.name = "CodeA";
            property.value = EbdInfo.EBM.MsgBasicInfo.SenderCode;
            m_lstProperty.Add(property);

            property.name = "NameA";
            property.value = EbdInfo.EBM.MsgBasicInfo.SenderName;
            m_lstProperty.Add(property);

            property.name = "EBMID";
            property.value = EbdInfo.EBM.EBMID;
            m_lstProperty.Add(property);

            property.name = "SentTime";
            property.value = EbdInfo.EBM.MsgBasicInfo.SendTime;
            m_lstProperty.Add(property);

            property.name = "EBMStartTime";
            property.value = EbdInfo.EBM.MsgBasicInfo.StartTime;
            m_lstProperty.Add(property);

            property.name = "EBMEndTime";
            property.value = EbdInfo.EBM.MsgBasicInfo.EndTime;
            m_lstProperty.Add(property);

            property.name = "EBMTitle";
            property.value = EbdInfo.EBM.MsgContent.MsgTitle;
            m_lstProperty.Add(property);


            string lab_EBMType = "";
            if (EbdInfo.EBM.MsgContent.Auxiliary != null)
            {
                lab_EBMType = "音频文件播发";
            }
            else
            {
                lab_EBMType = "文本转语音播发";
            }

            property.name = "EBMType";
            property.value = lab_EBMType;
            m_lstProperty.Add(property);

            property.name = "EBMDesc";
            property.value = EbdInfo.EBM.MsgContent.MsgDesc;
            m_lstProperty.Add(property);

            property.name = "EBMCode";
            property.value = EbdInfo.EBM.MsgContent.AreaCode;
            m_lstProperty.Add(property);

            property.name = "EBMUrl";
            property.value = EbdInfo.EBM.MsgContent.Auxiliary.AuxiliaryDesc;
            m_lstProperty.Add(property);


            property.name = "Severity";
            property.value = EbdInfo.EBM.MsgBasicInfo.Severity;
            m_lstProperty.Add(property);

            property.name = "CheckStatus";
            property.value = SingletonInfo.GetInstance().CheckEBMStatusFlag;
            m_lstProperty.Add(property);

            SingletonInfo.GetInstance().m_mq_checkEBM.SendMQMessage(true, "", m_lstProperty);
        }

        public string GetORG_ID(string code)
        {
            string org = "";
            string sqlstr = "select ORG_ID from Organization where GB_CODE ='" + code + "'";
            DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(sqlstr);
            if (dtMedia != null && dtMedia.Rows.Count > 0)
            {

                if (dtMedia.Rows.Count == 1)
                {
                    org = dtMedia.Rows[0][0].ToString();
                }
            }
            return org;
        }

        private void TarOMRequest(XmlDocument xmlStateDoc, responseXML rState, string strOMDType, string frdStateName, string xmlEBMStateFileName, EBD ebd)
        {
            string sHeartBeatTarName = "";
            DataTable dtMedia = null;
            List<Device> lDev = new List<Device>();
            switch (strOMDType)
            {
                case "EBRDTInfo":
                    SetText("EBRDTInfo    NO:6", Color.Orange);
                    ListViewItem OMDRequestEBRDTInfo = new ListViewItem();
                    OMDRequestEBRDTInfo.Text = "设备信息请求";
                    this.Invoke(new Action(() => { list_OMDRequest.Items.Add(OMDRequestEBRDTInfo); }));

                    string MediaSql = "";
                    if (ebd.OMDRequest.Params.RptType == "Incremental")
                    {
                         MediaSql = "select *  from SRV where SRV_FLAG3 <> '2' or SRV_FLAG3 Is Null";

                    }
                    else
                    {
                        MediaSql = "select *  from SRV";
                    }
                    DataTable dtMediaSRV = mainForm.dba.getQueryInfoBySQL(MediaSql);

                    for (int idtM = 0; idtM < dtMediaSRV.Rows.Count; idtM++)
                    {
                        Device DV = new Device();
                        string strSRV_ID = "";
                        string strSRV_CODE = "";
                        string strSRV_GOOGLE = "";

                        strSRV_ID = dtMediaSRV.Rows[idtM]["SRV_ID"].ToString();
                        strSRV_CODE = dtMediaSRV.Rows[idtM]["SRV_CODE"].ToString();
                        strSRV_GOOGLE = dtMediaSRV.Rows[idtM]["SRV_GOOGLE"].ToString();
                        string Longitudetmp = strSRV_GOOGLE.Split(',')[0];
                        string Latitudetmp = strSRV_GOOGLE.Split(',')[1];
                        string deviceTypeId = dtMediaSRV.Rows[idtM]["deviceTypeId"].ToString();
                        DV.DeviceID = strSRV_ID;
                        DV.DeviceName = DicSrvType[deviceTypeId];
                        DV.DeviceType = strSRV_CODE;
                        DV.Longitude = BBSHelper.StrDeal(Longitudetmp, 6);
                        DV.Latitude = BBSHelper.StrDeal(Latitudetmp, 6);
                        DV.EBRID = dtMediaSRV.Rows[idtM]["SRV_LOGICAL_CODE_GB"].ToString(); ;
                        lDev.Add(DV);
                    }

                    if (dtMediaSRV.Rows.Count > 0)
                    {
                        string strSql = string.Format("update SRV set SRV_FLAG3 = '{0}' where SRV_FLAG3 <>'2' or SRV_FLAG3 Is Null", "2");
                        mainForm.dba.UpdateDbBySQL(strSql);
                    }
                    xmlStateDoc = rState.DeviceInfoResponse(lDev, frdStateName, ebd);
                    UnifyCreateTar(xmlStateDoc, frdStateName);
                    sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                    send.address = SingletonInfo.GetInstance().SendTarAddress;
                    send.fileNamePath = sHeartBeatTarName;
                    postfile.UploadFilesByPostThread(send);
                    break;
                case "EBRDTState":
                    SetText("EBRDTState     NO:9", Color.Orange);
                    ListViewItem OMDRequestEBRDTState = new ListViewItem();
                    OMDRequestEBRDTState.Text = "设备状态请求";
                    this.Invoke(new Action(() => { list_OMDRequest.Items.Add(OMDRequestEBRDTState); }));
                    lDev = GetEBRDTStateFromDataBase(ebd.OMDRequest.Params.RptType);
                     xmlStateDoc = rState.DeviceStateResponse(lDev, frdStateName, ebd);
                    UnifyCreateTar(xmlStateDoc, frdStateName);
                    sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                    send.address = SingletonInfo.GetInstance().SendTarAddress;
                    send.fileNamePath = sHeartBeatTarName;
                    postfile.UploadFilesByPostThread(send);
                    break;
                case "EBRPSInfo"://平台信息
                    SetText("EBRPSInfo     NO:2", Color.Orange);
                    try
                    {
                        xmlStateDoc = rState.platformInfoResponse(frdStateName, ebd);
                        UnifyCreateTar(xmlStateDoc, frdStateName);
                        sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                        send.address = SingletonInfo.GetInstance().SendTarAddress;
                        send.fileNamePath = sHeartBeatTarName;
                        postfile.UploadFilesByPostThread(send);
                        ListViewItem OMDRequestEBRPSInfo = new ListViewItem();
                        OMDRequestEBRPSInfo.Text = "平台信息请求";
                        this.Invoke(new Action(() => { list_OMDRequest.Items.Add(OMDRequestEBRPSInfo); }));
                    }
                    catch
                    {
                    }
                    break;
                case "EBRPSState"://平台状态
                    SetText("EBRPSState    NO:7", Color.Orange);
                    try
                    {
                        xmlStateDoc = rState.platformstateInfoResponse(frdStateName,SingletonInfo.GetInstance().EBRPSStateCode.ToString(),ebd);
                        UnifyCreateTar(xmlStateDoc, frdStateName);
                        sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                        send.address = SingletonInfo.GetInstance().SendTarAddress;
                        send.fileNamePath = sHeartBeatTarName;
                        postfile.UploadFilesByPostThread(send);
                        ListViewItem OMDRequestEBRPSState = new ListViewItem();
                        OMDRequestEBRPSState.Text = "平台状态请求";
                        this.Invoke(new Action(() => { list_OMDRequest.Items.Add(OMDRequestEBRPSState); }));
                    }
                    catch
                    {
                    }
                    break;
                case "EBMBrdLog"://播发记录
                    SetText("EBMBrdLog    NO:8", Color.Orange);
                    MediaSql = "select *  from EBMInfo where StartTime between '" + ebd.OMDRequest.Params.RptStartTime + "' and '" + ebd.OMDRequest.Params.RptEndTime + "'";
                    dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);
                    List<EBM> EBMList = new List<EBM>();
                    if (dtMedia != null && dtMedia.Rows.Count > 0)
                    {
                        for (int idtM = 0; idtM < dtMedia.Rows.Count; idtM++)
                        {
                            EBM ebmtmp = new EBM();
                            ebmtmp.EBMVersion = dtMedia.Rows[idtM]["EBDVersion"].ToString();
                            ebmtmp.EBMID= dtMedia.Rows[idtM]["EBMID"].ToString();
                            ebmtmp.MsgBasicInfo = new MsgBasicInfo();
                            ebmtmp.MsgBasicInfo.MsgType= dtMedia.Rows[idtM]["MsaType"].ToString();
                            ebmtmp.MsgBasicInfo.SenderName = dtMedia.Rows[idtM]["SenderName"].ToString();
                            ebmtmp.MsgBasicInfo.SenderCode = dtMedia.Rows[idtM]["SenderCode"].ToString();
                            ebmtmp.MsgBasicInfo.SendTime = dtMedia.Rows[idtM]["SendTime"].ToString();
                            ebmtmp.MsgBasicInfo.EventType = dtMedia.Rows[idtM]["EventType"].ToString();
                            ebmtmp.MsgBasicInfo.Severity = dtMedia.Rows[idtM]["Severity"].ToString();
                            ebmtmp.MsgBasicInfo.StartTime = dtMedia.Rows[idtM]["StartTime"].ToString();
                            ebmtmp.MsgBasicInfo.EndTime = dtMedia.Rows[idtM]["EndTime"].ToString();
                            ebmtmp.MsgContent = new MsgContent();
                            ebmtmp.MsgContent.LanguageCode = dtMedia.Rows[idtM]["LanguageCode"].ToString();
                            ebmtmp.MsgContent.MsgTitle = dtMedia.Rows[idtM]["MsgTitle"].ToString();
                            ebmtmp.MsgContent.MsgDesc = dtMedia.Rows[idtM]["msgDesc"].ToString();
                            ebmtmp.MsgContent.AreaCode = dtMedia.Rows[idtM]["AreaCode"].ToString();

                            if (dtMedia.Rows[idtM]["AuxiliaryDesc"].ToString()!="文本转语")
                            {
                                ebmtmp.MsgContent.Auxiliary = new Auxiliary();
                                ebmtmp.MsgContent.Auxiliary.AuxiliaryType = dtMedia.Rows[idtM]["AuxiliaryType"].ToString();
                                ebmtmp.MsgContent.Auxiliary.AuxiliaryDesc = dtMedia.Rows[idtM]["AuxiliaryDesc"].ToString();
                                ebmtmp.MsgContent.Auxiliary.Size = dtMedia.Rows[idtM]["Size"].ToString();
                            }
                            ebmtmp.BrdStateCode= dtMedia.Rows[idtM]["EBMState"].ToString();
                            EBMList.Add(ebmtmp);
                        }
                        xmlStateDoc = rState.PlatformEBMBrdLog(ebd, EBMList);
                        UnifyCreateTar(xmlStateDoc, frdStateName);
                        sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                        send.address = SingletonInfo.GetInstance().SendTarAddress;
                        send.fileNamePath = sHeartBeatTarName;
                        postfile.UploadFilesByPostThread(send);
                    }
                    ListViewItem OMDRequestEBMBrdLog = new ListViewItem();
                    OMDRequestEBMBrdLog.Text = "播发记录请求";
                    this.Invoke(new Action(() => { list_OMDRequest.Items.Add(OMDRequestEBMBrdLog); }));
                    break;
            }
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>" + strOMDType);
        }

        private void UnifyCreateTar(XmlDocument xmlStateDoc, string frdStateName)
        {
            string XMLSavePath = CreateCMLSavePath(frdStateName);
            string xmlSignFileName = "\\EBDB_" + frdStateName + ".xml";
            CreateXML(xmlStateDoc, XMLSavePath + xmlSignFileName);
            ServerForm.mainFrm.GenerateSignatureFile(XMLSavePath, frdStateName);
            ServerForm.tar.CreatTar(XMLSavePath, ServerForm.sSendTarPath, frdStateName);//使用新TAR
        }

        private string CreateCMLSavePath(string FileName)
        {
            string SaveXMLofName = strBeSendFileMakeFolder + "\\" + FileName;// "D:\\work\\93\\BeXmlFiles\\" + FileName;
            if (!Directory.Exists(SaveXMLofName))
            {
                Directory.CreateDirectory(SaveXMLofName);
            }
            else
            {
                ServerForm.DeleteFolder(SaveXMLofName);
            }
            return SaveXMLofName;
        }

        //做成委托
        private string SaveEBD(EBD ebm)
        {
            string EBDVersion = "";//,  --协议版本号
            string SEBDID = "";////--应急广播数据包ID
            string SEDBType = "";//,--事件类型( EBM EBMStateResponse EBMStateRequest OMDRequest EBRSTInfo EBRASInfo EBRBSInfo EBRDTInfo EBMBrdLog EBRASState EBRBSState EBRDTState ConnectionCheck EBDResponse -)
            string SEBRID = "";// ,--数据包来源对象ID
            string EBRID = "";//,--数据包目标对象ID
            string SEBBuidTime = "";//,---数据包生成时间
            string EBMID = "";//,--应急广播消息ID
            string MsaType = "";// ,---- 消息类型 1：请求播发 2：取消播发
            string SenderName = "";//,--发布机构名称
            string SenderCode = "";// ,--发布机构编码
            string SendTime = "";// ,--发布时间
            string EventType = "";//,--事件类型编码
            string Severity = "";// ,--事件级别
            string StartTime = "";// ,--播发起始时间
            string EndTime = "";// ,--播发结束时间
            string LanguageCode = "";// ,--语种代码(中文为:zho)
            string MsgTitle = "";// ,--消息标题文本-
            string MsgDesc = "";//,--消息内容文本
            string AreaCode = "";// ,--覆盖区域编码 eg:110000000000,120000000000,130000000000
            string AuxiliaryType = "";//,--辅助数据类型 61：实时流 2文件
            string AuxiliaryDesc = "";// , --文件名称
            string Size = "";// , --文件大小
            string EBMState = "0";// --执行状态 0：未处理1：等待播发，指未到消息播发时间 2：播发中  3：播发成功 4：播发失败，包括播发全部失败、播发部分失败、未按要求播发等情况  5：播发取消

            //EBM处理
            if (ebm != null)
            {
                EBDVersion = ebm.EBDVersion;
                SEBDID = ebd.EBDID;
                SEDBType = ebd.EBDType;
                SEBRID = ebm.SRC.EBRID;
                //   EBRID = ebm.DEST.EBRID;
                    EBRID = "";
                   SEBBuidTime = ebm.EBDTime;
                SEBDIDStatusFlag = SEBDID;
                if (ebd.EBDType == "EBM")
                {
                    EBMID = ebm.EBM.EBMID;
                    MsaType = ebm.EBM.MsgBasicInfo.MsgType;
                    SenderName = ebm.EBM.MsgBasicInfo.SenderName;
                    SenderCode = ebm.EBM.MsgBasicInfo.SenderCode;
                    SendTime = ebm.EBM.MsgBasicInfo.SendTime;
                    EventType = ebm.EBM.MsgBasicInfo.EventType;
                    Severity = ebm.EBM.MsgBasicInfo.Severity;
                    StartTime = ebm.EBM.MsgBasicInfo.StartTime;
                    EndTime = ebm.EBM.MsgBasicInfo.EndTime;
                    LanguageCode = ebm.EBM.MsgContent.LanguageCode;
                    MsgTitle = ebm.EBM.MsgContent.MsgTitle;
                    MsgDesc = ebm.EBM.MsgContent.MsgDesc;
                    AreaCode = ebm.EBM.MsgContent.AreaCode;
                    if (ebm.EBM.MsgContent.Auxiliary != null)
                    {
                        AuxiliaryType = ebm.EBM.MsgContent.Auxiliary.AuxiliaryType;
                        AuxiliaryDesc = ebm.EBM.MsgContent.Auxiliary.AuxiliaryDesc;
                        Size = ebm.EBM.MsgContent.Auxiliary.Size;
                    }
                }
            }

            StringBuilder sbSql = new StringBuilder(100);
            sbSql.Append("insert into EBMInfo Values(");
            sbSql.Append("'" + EBDVersion + "',");
            sbSql.Append("'" + SEBDID + "',");
            sbSql.Append("'" + SEDBType + "',");
            sbSql.Append("'" + SEBRID + "',");
            sbSql.Append("'" + EBRID + "',");
            sbSql.Append("'" + SEBBuidTime + "',");              
            sbSql.Append("'" + EBMID + "',");              
            sbSql.Append("'" + MsaType + "',");         
            sbSql.Append("'" + SenderName + "',");         
            sbSql.Append("'" + SenderCode + "',");          
            sbSql.Append("'" + SendTime + "',");
            sbSql.Append("'" + EventType + "',");
            sbSql.Append("'" + Severity + "',");
            sbSql.Append("'" + StartTime + "',");
            sbSql.Append("'" + EndTime + "',");
            sbSql.Append("'" + LanguageCode + "',");
            sbSql.Append("'" + MsgTitle + "',");
            sbSql.Append("'" + MsgDesc + "',");
            sbSql.Append("'" + AreaCode + "',");
            sbSql.Append("'" + AuxiliaryType + "',");
            sbSql.Append("'" + AuxiliaryDesc + "',");
            sbSql.Append("'" + Size + "',");
            sbSql.Append("'" + EBMState + "',");
            sbSql.Append("'" + TsCmdStoreID + "'");
            sbSql.Append(")");
            sbSql.Append(" SELECT CAST(scope_identity() AS int)");
            return  mainForm.dba.InsertDbBySQLRetID(sbSql.ToString()).ToString();
        }

        #region 数据计算校验和
        private string DataSum(string sCmdStr)
        {
            //, char cSplit, ref List<byte> list

            try
            {
                int iSum = 0;
                List<byte> listCmd = new List<byte>();
                string sSum = "";
                if (sCmdStr.Trim() == "")
                    return "";
                string[] sTmp = sCmdStr.Split(' ');
                byte[] cmdByte = new byte[sTmp.Length];
                for (int i = 0; i < sTmp.Length; i++)
                {
                    cmdByte[i] = byte.Parse(sTmp[i], System.Globalization.NumberStyles.HexNumber);
                    listCmd.Add(cmdByte[i]);
                    iSum = iSum + int.Parse(sTmp[i], System.Globalization.NumberStyles.HexNumber);
                }
                sSum = Convert.ToString(iSum, 16).ToUpper().PadLeft(4, '0');
                sSum = sSum.Substring(sSum.Length - 2, 2);
                return sSum;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
                return "";
            }
        }
        #endregion 数据计算校验和

        
        #region ToList
        private void PlatToList(DataTable dt, ref List<PlatformBRD> lPlat)
        {//PlatformID,BRDSourceType,BRDSourceID,BRDMsgID,BRDSender,BRDStartTime,BRDEndTime,MediaFileURL
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PlatformBRD pm = new PlatformBRD();
                    pm.PlatformBRDID = dt.Rows[i][0].ToString();
                    pm.SourceType = dt.Rows[i][1].ToString();
                    pm.SourceID = dt.Rows[i][2].ToString();
                    pm.MsgID = dt.Rows[i][3].ToString();
                    pm.Sender = dt.Rows[i][4].ToString();
                    pm.BRDStartTime = dt.Rows[i][5].ToString();
                    Console.WriteLine(pm.BRDStartTime);
                    pm.BRDEndTime = dt.Rows[i][6].ToString();
                    pm.AudioFileURL = dt.Rows[i][7].ToString();
                    pm.UnitId = "3424";//播发部门ID
                    pm.UnitName = "公安局";//播发部门名称
                    pm.PersonID = "74";
                    pm.PersonName = "吴局";

                    lPlat.Add(pm);
                }
            }
        }

        private void TermToList(DataTable dt, ref List<TermBRD> lTerm)
        {
            //PlatformID,BRDSourceType,BRDSourceID,BRDMsgID,BRDTime
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TermBRD tm = new TermBRD();
                    tm.BRDTime = dt.Rows[i][4].ToString();
                    tm.DeviceID = dt.Rows[i][0].ToString();
                    tm.SourceType = dt.Rows[i][1].ToString();
                    tm.SourceID = dt.Rows[i][2].ToString();
                    tm.MsgID = dt.Rows[i][3].ToString();
                    tm.TermBRDID = dt.Rows[i][0].ToString();
                    tm.ResultCode = "1";
                    tm.ResultDesc = "正常";

                    lTerm.Add(tm);
                }
            }
        }

        private void DeviceDataToList(DataTable dt, ref List<Device> listD)
        {
            string sBaidu = string.Empty;
            int iPos = -1;
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Device dv = new Device();
                    dv.DeviceID = dt.Rows[i][0].ToString();
                    dv.DeviceCategory = "Term";
                    dv.DeviceType = "TN5415E";
                    dv.DeviceState = "正常";
                    dv.AreaCode = dt.Rows[i][1].ToString();
                    dv.DeviceName = "音柱";
                    dv.AdminLevel = "村级";
                    sBaidu = dt.Rows[i][2].ToString();
                    if (sBaidu.Length > 0)
                    {
                        iPos = sBaidu.IndexOf(",");
                        if (iPos > 0)
                        {
                            dv.Longitude = sBaidu.Substring(0, iPos);
                            dv.Latitude = sBaidu.Substring(iPos + 1);
                        }

                    }
                    listD.Add(dv);
                }
            }
        }

        private void DeviceStateToList(DataTable dt, ref List<Device> listD)
        {
            string devState = string.Empty;
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Device dv = new Device();
                    dv.DeviceID = dt.Rows[i][0].ToString();
                    dv.DeviceCategory = "Term";
                    dv.DeviceType = "TN5415E";
                    devState = dt.Rows[i][1].ToString();
                    switch (devState)
                    {
                        case "正常":
                        default:
                            dv.DeviceState = "1";
                            break;
                        case "故障":
                            dv.DeviceState = "2";
                            break;
                        case "故障恢复":
                            dv.DeviceState = "3";
                            break;
                    }

                    dv.AreaCode = dt.Rows[i][2].ToString();
                    dv.DeviceName = "音柱";
                    dv.AdminLevel = "村级";
                    listD.Add(dv);
                }
            }
        }
        #endregion End

        private int DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            int dateDiff = 0;

            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = (int)(ts.TotalSeconds);
            //Console.WriteLine(DateTime1.ToString() + "-" + DateTime2.ToString() + "=" +dateDiff.ToString());
            return dateDiff;
        }

        /// <summary>
        /// 清空指定的文件夹，但不删除文件夹
        /// </summary>
        /// <param name="folderpath">文件夹路径</param>
        public static void DeleteFolder(string folderpath)
        {
            try
            {
                foreach (string delFile in Directory.GetFileSystemEntries(folderpath))
                {
                    if (File.Exists(delFile))
                    {
                        FileInfo fi = new FileInfo(delFile);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                        File.Delete(delFile);//直接删除其中的文件
                        // SetText("删除文件：" + delFile);
                    }
                    else
                    {
                        DirectoryInfo dInfo = new DirectoryInfo(delFile);
                        if (dInfo.GetFiles().Length != 0)
                        {
                            DeleteFolder(dInfo.FullName);//递归删除子文件夹
                        }
                        Directory.Delete(delFile);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("G1475：" + ex.Message);
                Log.Instance.LogWrite("G1475：" + ex.Message);
            }
        }


        #region 替换后面的“00”为“AA”
        private string ReplaceToAA(string dataStr)
        {
            string lh_Str = "";
            string AA_Str = "";
            if (dataStr != "" && dataStr != " ")
            {
                for (int i = 0; i < dataStr.Length; i = i + 2)
                {
                    AA_Str = dataStr.Substring(i, 2);
                    if (AA_Str == "00")
                    {
                        AA_Str = "AA";
                    }
                    lh_Str = lh_Str + AA_Str;
                }
                lh_Str = lh_Str.TrimEnd(' ');
            }
            else
            {
                lh_Str = "";
            }
            return lh_Str;
        }
        #endregion

        private string L_H(string dataStr)
        {
            string lh_Str = "";
            if (dataStr != "" && dataStr != " ")
            {
                for (int i = 0; i < dataStr.Length; i = i + 2)
                {
                    lh_Str = dataStr.Substring(i, 2) + " " + lh_Str;
                }
                lh_Str = lh_Str.TrimEnd(' ');
            }
            else
            {
                lh_Str = "";
            }
            return lh_Str;
        }

        private void timHold_Tick(object sender, EventArgs e)
        {
            switch (bCharToAudio)
            {
                case "1":
                    {
                        //文转
                        #region 文转语
                        if (mainForm.bMsgStatusFree)
                        {
                            //if (mainForm.bMsgStatusFree)
                            //{
                            //    iHoldTimesCnt = iHoldTimes;
                            //}
                            //string cmdSStr = "54 01 03 01 00";
                            //cmdSStr = cmdSStr + " " + CRCBack(cmdSStr);
                            //SendCRCCmd(mainForm.sndComm, cmdSStr, 1);//

                            //if (iHoldTimesCnt < iHoldTimes)
                            //{
                            //    for (int i = 0; i < listAreaCode.Count; i++)
                            //    {
                            //        string cmdOpen = "4C " + listAreaCode[i] + " C0 02 01 04";
                            //        SendCmd(mainForm.comm, cmdOpen, 1);
                            //    }
                            //    iHoldTimesCnt++;//累加
                            //}
                            //else
                            {
                                timHold.Stop();
                                //string cmdStr = "4C " + EMBCloseAreaCode + " C0 02 00 01";//停止时发关机指令
                                //SendCmd(mainForm.comm, cmdStr, 8);//发送指令
                                Thread.Sleep(2000);
                                for (int i = 0; i < listAreaCode.Count; i++)
                                {
                                    string cmdOpen = "4C " + listAreaCode[i] + " C0 02 00 04";
                                    //  string cmdOpen = "4C AA AA AA AA AA C0 02 01 04";
                                    //  string cmdOpen = "FE FE FE 4C AA AA AA AA AA C0 02 01 04 65 16";
                                    //  string cmdOpen = "FE FE FE 4C AA AA AA AA AA C0 02 00 04 64 16";
                                    //SendCmd(mainForm.comm, cmdOpen, 6);
                                    Log.Instance.LogWrite("文转语结束应急关机：" + cmdOpen);
                                    //2016-04-01  改写数据池
                                    string strsum = DataSum(cmdOpen);
                                    cmdOpen = "FE FE FE " + cmdOpen + " " + strsum + " 16";
                                    //   cmdOpen = "FE FE FE 4C AA AA AA 01 05 B0 02 01 00 03 16";
                                    string strsql = "";
                                    strsql = "insert into CommandPool(CMD_TIME,CMD_BODY,CMD_FLAG)" +
                                    " VALUES('" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','" + cmdOpen + "','" + '0' + "')";
                                    mainForm.dba.UpdateOrInsertBySQL(strsql);
                                    mainForm.dba.UpdateOrInsertBySQL(strsql);
                                }

                                Log.Instance.LogWrite("文转语播放结束：" + DateTime.Now.ToString());//+ cmdStr);
                                SetText("文转语播放结束" + DateTime.Now.ToString(), Color.Blue);
                                Thread.Sleep(1000);
                                listAreaCode.Clear();//清除应急区域列表
                                //     this.txtMsgShow.Text = "";
                                bCharToAudio = "";
                                //  sendEBMStateResponse(ebd);
                            }
                        }
                        #endregion End
                    }
                    break;
                case "2":
                    {
                        //if (MediaPlayer.playState == WMPLib.WMPPlayState.wmppsStopped || MediaPlayer.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
                        //{
                        //}
                        /*
                        #region 音频播放
                        if (MediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying && MediaPlayer.playState != WMPLib.WMPPlayState.wmppsBuffering && MediaPlayer.playState != WMPLib.WMPPlayState.wmppsTransitioning)
                        {
                            iHoldTimesCnt = iHoldTimes;
                            Log.Instance.LogWrite("播放器状态："+MediaPlayer.playState.ToString());
                        }
                        if (iHoldTimesCnt < iHoldTimes)
                        {
                            for (int i = 0; i < listAreaCode.Count; i++)
                            {
                                string cmdOpen = "4C " + listAreaCode[i] + " C0 02 01 04";
                                SendCmd(mainForm.comm, cmdOpen, 1);
                            }
                        }
                        else
                        {
                            timHold.Stop();
                            //string cmdStr = "4C " + EMBCloseAreaCode + " C0 02 00 01";//停止时发关机指令
                            //SendCmd(mainForm.comm, cmdStr, 8);//发送指令 发送8次
                            for (int i = 0; i < listAreaCode.Count; i++)
                            {
                                string cmdOpen = "4C " + listAreaCode[i] + " C0 02 00 01";
                                //  string cmdOpen = "4C AA AA AA AA AA C0 02 01 04";
                                //  string cmdOpen = "FE FE FE 4C AA AA AA AA AA C0 02 01 04 65 16";
                                //  string cmdOpen = "FE FE FE 4C AA AA AA AA AA C0 02 00 04 64 16";
                                //SendCmd(mainForm.comm, cmdOpen, 6);
                                Log.Instance.LogWrite("应急关机：" + cmdOpen);
                                //2016-04-01  改写数据池
                                string strsum = DataSum(cmdOpen);
                                cmdOpen = "FE FE FE " + cmdOpen + " " + strsum + " 16";
                                //   cmdOpen = "FE FE FE 4C AA AA AA 01 05 B0 02 01 00 03 16";
                                string strsql = "";
                                strsql = "insert into CommandPool(CMD_TIME,CMD_BODY,CMD_FLAG)" +
                                " VALUES('" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','" + cmdOpen + "','" + '0' + "')";
                                mainForm.dba.UpdateOrInsertBySQL(strsql);
                                mainForm.dba.UpdateOrInsertBySQL(strsql);
                            }
                            Log.Instance.LogWrite("语音播放结束：" + DateTime.Now.ToString());// + cmdStr);
                            Thread.Sleep(1000);
                            listAreaCode.Clear();//清除应急区域列表
                            MediaPlayer.Ctlcontrols.stop();
                            MediaPlayer.close();
                            iHoldTimesCnt = 0;
                            //   this.txtMsgShow.Text = "";
                            SetText("播放音频文件结束" + DateTime.Now.ToString());
                            sendEBMStateResponse(ebd);
                            bCharToAudio = "";
                        }
                        #endregion End
                         */
                    }
                    break;
                default:
                    bCharToAudio = "";
                    break;
            }
        }

        private void timHeart_Tick(object sender, EventArgs e)
        {

            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            rHeart.SourceAreaCode = strSourceAreaCode;
            rHeart.SourceType = strSourceType;
            rHeart.SourceName = strSourceName;
            rHeart.SourceID = strSourceID;
            rHeart.sHBRONO = SingletonInfo.GetInstance().CurrentResourcecode;
            try
            {
                xmlHeartDoc = rHeart.HeartBeatResponse();  // rState.EBMStateResponse(ebd);
                string xmlStateFileName = "\\EBDB_000000000009.xml";
                CreateXML(xmlHeartDoc, sHeartSourceFilePath + xmlStateFileName);
                tar.CreatTar(sHeartSourceFilePath, sSendTarPath, "000000000009");//使用新TAR
            }
            catch (Exception ec)
            {
                Log.Instance.LogWrite("心跳处错误：" + ec.Message);
            }
            string sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_000000000009" + ".tar";
            HttpSendFile.UploadFilesByPost(SingletonInfo.GetInstance().SendTarAddress, sHeartBeatTarName);

            #region 心跳判断
            if (dtLinkTime != null && dtLinkTime.ToString() != "")
            {
                int timetick = DateDiff(DateTime.Now, dtLinkTime);
                //大于600秒（10分钟）
                if (timetick > OnOffLineInterval)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.Text = "离线";
                    }));
                   
                }
                else
                {
                    this.Invoke(new Action(() =>
                    {
                        this.Text = "在线";
                    }));
                   
                }
                if (timetick > OnOffLineInterval * 3)
                {
                    dtLinkTime = DateTime.Now.AddSeconds(-2 * OnOffLineInterval);
                }
            }
            else
            {
                dtLinkTime = DateTime.Now;
            }
            #endregion End
        }  

        //线程间同步
        public void SetText(string text, Color colo)
        {
            if (this.txtMsgShow.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text, colo });
            }
            else
            {
                string strs = this.txtMsgShow.Text;
                string[] strR = strs.Split("\r\n".ToCharArray());     //\r\n   为回车符号   
                int i = strR.Length - 1;     //得到   strR数组   的长度   
                if (i > 200)
                {
                    this.txtMsgShow.Clear();
                    this.txtMsgShow.Refresh();
                }
                this.txtMsgShow.ForeColor = colo;
                this.txtMsgShow.AppendText(text);
                this.txtMsgShow.AppendText(Environment.NewLine);
            }
        }

        private void tim_MediaPlay_Tick(object sender, EventArgs e)
        {

        }

        //定时释放内存
        private void tim_ClearMemory_Tick(object sender, EventArgs e)
        {
            ClearMemory();
        }

        #region 内存回收 //2016-04-25 add
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        #endregion

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (tim_MediaPlay.Enabled)    //定时查询媒体播放定时器
                {
                    tim_MediaPlay.Enabled = false;
                }
                if (tim_ClearMemory.Enabled)  //清除内存垃圾定时器
                {
                    tim_ClearMemory.Enabled = false;
                }

                if (thTar != null)
                {
                    thTar.Abort();
                    //thTar = null;
                }
                if (thFeedBack != null)
                {
                    thFeedBack.Abort();
                }
                if (httpthread != null)
                {
                    httpthread.Abort();
                    httpthread = null;
                }
                httpServer.StopListen();
                MQDLL.StopActiveMQ();

                SingletonInfo.GetInstance().serverini.WriteValue("PLATFORMINFO", "SequenceCodes", SingletonInfo.GetInstance().SequenceCodes.ToString());
            }
            catch (Exception em)
            {
                Log.Instance.LogWrite("ServerFormCloseing停止线程错误：" + em.Message);
            }
        }

        #region 应急广播平台信息上报函数  
        private void PlatformInfoReported(string datatype = "Incremental")
        {
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            ServerForm.DeleteFolder(sHeartSourceFilePath);//删除原有XML发送文件的文件夹下的XML
            string frdStateName = "";
            try
            {
                frdStateName = "10" + SingletonInfo.GetInstance().CurrentResourcecode + BBSHelper.GetSequenceCodes();
                string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";
                xmlHeartDoc = rHeart.platformInfoResponse(frdStateName);
                CreateXML(xmlHeartDoc, sHeartSourceFilePath + xmlEBMStateFileName);
                ServerForm.mainFrm.GenerateSignatureFile(sHeartSourceFilePath, frdStateName);
                ServerForm.tar.CreatTar(sHeartSourceFilePath, ServerForm.sSendTarPath, frdStateName);//使用新TAR
                string sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                HttpSendFile.UploadFilesByPost(SingletonInfo.GetInstance().SendTarAddress, sHeartBeatTarName);
            }
            catch (Exception ex)
            {
                Log.Instance.LogWrite("应急广播平台信息上报异常：" + ex.Message);
            }
        }
        #endregion

        #region 应急广播平台状态上报函数
        private void PlatformstateInfoReported(string datatype = "Incremental")
        {
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            ServerForm.DeleteFolder(sHeartSourceFilePath);//删除原有XML发送文件的文件夹下的XML
            string frdStateName = "";
            try
            {
                frdStateName = "10" + SingletonInfo.GetInstance().CurrentResourcecode + BBSHelper.GetSequenceCodes();
                string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";
                xmlHeartDoc = rHeart.platformstateInfoResponse(frdStateName,SingletonInfo.GetInstance().EBRPSStateCode.ToString());
                CreateXML(xmlHeartDoc, sHeartSourceFilePath + xmlEBMStateFileName);
                ServerForm.mainFrm.GenerateSignatureFile(sHeartSourceFilePath, frdStateName);
                ServerForm.tar.CreatTar(sHeartSourceFilePath, ServerForm.sSendTarPath, frdStateName);//使用新TAR
                string sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                HttpSendFile.UploadFilesByPost(SingletonInfo.GetInstance().SendTarAddress, sHeartBeatTarName);
            }
            catch
            {
            }
        }
        #endregion

        #region 应急广播平台终端信息上报函数 
        private void PlatformEBRDTInfoReported(string datatype= "Incremental")
        {
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            string MediaSql = "";
            string strSRV_ID = "";
            string strSRV_CODE = "";
            string strSRV_GOOGLE = "";
            ServerForm.DeleteFolder(sHeartSourceFilePath);//删除原有XML发送文件的文件夹下的XML
            string frdStateName = "";
            List<Device> lDev = new List<Device>();
            try
            {
                DataTable dtMedia;
                if (datatype == "Incremental")
                {
                    MediaSql = "select *  from SRV where SRV_FLAG3 <> '2' or SRV_FLAG3 Is Null";
                }
                else
                {
                    MediaSql = "select *  from SRV";
                }
                dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);
                for (int idtM = 0; idtM < dtMedia.Rows.Count; idtM++)
                {
                    Device DV = new Device();
                    strSRV_ID = dtMedia.Rows[idtM]["SRV_ID"].ToString();
                    strSRV_CODE = dtMedia.Rows[idtM]["SRV_CODE"].ToString();
                    strSRV_GOOGLE= dtMedia.Rows[idtM]["SRV_GOOGLE"].ToString();
                    string Longitudetmp = strSRV_GOOGLE.Split(',')[0];
                    string Latitudetmp = strSRV_GOOGLE.Split(',')[1];
                    string deviceTypeId= dtMedia.Rows[idtM]["deviceTypeId"].ToString();
                    DV.DeviceID = strSRV_ID;
                    DV.DeviceName = DicSrvType[deviceTypeId];
                    DV.DeviceType = strSRV_CODE;
                    DV.Longitude = BBSHelper.StrDeal(Longitudetmp, 6);
                    DV.Latitude = BBSHelper.StrDeal(Latitudetmp,6); 
                    DV.EBRID = dtMedia.Rows[idtM]["SRV_LOGICAL_CODE_GB"].ToString(); ;
                    lDev.Add(DV);
                }
                frdStateName = "10" + SingletonInfo.GetInstance().CurrentResourcecode + BBSHelper.GetSequenceCodes();
                string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";

                xmlHeartDoc = rHeart.DeviceInfoResponse(lDev, frdStateName);
                CreateXML(xmlHeartDoc, sHeartSourceFilePath + xmlEBMStateFileName);
                ServerForm.mainFrm.GenerateSignatureFile(sHeartSourceFilePath, frdStateName);
                ServerForm.tar.CreatTar(sHeartSourceFilePath, ServerForm.sSendTarPath, frdStateName);//使用新TAR
                string sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                HttpSendFile.UploadFilesByPost(SingletonInfo.GetInstance().SendTarAddress, sHeartBeatTarName);

                if (dtMedia.Rows.Count>0)
                {
                    string strSql = string.Format("update SRV set SRV_FLAG3 = '{0}' where SRV_FLAG3 <>'2' or SRV_FLAG3 Is Null", "2");
                    mainForm.dba.UpdateDbBySQL(strSql);
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 应急广播平台终端状态上报函数
        private void PlatformEBRDTStateReported(string datatype = "Incremental")
        {
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            string MediaSql = "";
            ServerForm.DeleteFolder(sHeartSourceFilePath);//删除原有XML发送文件的文件夹下的XML
            string frdStateName = "";
            List<Device> lDev = new List<Device>();
            try
            {
                lDev = GetEBRDTStateFromDataBase(datatype);
                frdStateName = "10" + SingletonInfo.GetInstance().CurrentResourcecode + BBSHelper.GetSequenceCodes();
                string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";
                xmlHeartDoc = rHeart.DeviceStateResponse(lDev, frdStateName);
                CreateXML(xmlHeartDoc, sHeartSourceFilePath + xmlEBMStateFileName);
                ServerForm.mainFrm.GenerateSignatureFile(sHeartSourceFilePath, frdStateName);
                ServerForm.tar.CreatTar(sHeartSourceFilePath, ServerForm.sSendTarPath, frdStateName);//使用新TAR
                string sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                HttpSendFile.UploadFilesByPost(SingletonInfo.GetInstance().SendTarAddress, sHeartBeatTarName);

            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        //平台信息上报
        private void button1_Click(object sender, EventArgs e)
        {
            PlatformInfoReported();
        }

        //终端状态上报
        private void button2_Click(object sender, EventArgs e)
        {
            PlatformEBRDTStateReported();
        }

        //平台状态上报
        private void button3_Click(object sender, EventArgs e)
        {
            PlatformstateInfoReported();
        }

        //终端信息上报
        private void button4_Click(object sender, EventArgs e)
        {
            PlatformEBRDTInfoReported();
        }
        
        /// <summary>
        /// 定时的心跳反馈包
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void HeartUP(object source, System.Timers.ElapsedEventArgs e)
        {
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            rHeart.SourceAreaCode = strSourceAreaCode;
            rHeart.SourceType = strSourceType;
            rHeart.SourceName = strSourceName;
            rHeart.SourceID = strSourceID;
            rHeart.sHBRONO = SingletonInfo.GetInstance().CurrentResourcecode;
            DeleteFolder(TimesHeartSourceFilePath);//删除原有XML发送文件的文件夹下的XML
            try
            {
                xmlHeartDoc = rHeart.HeartBeatResponse();
                string HreartName = "01" + rHeart.sHBRONO + "0000000000000000";
                string xmlStateFileName = "EBDB_" + "01" + rHeart.sHBRONO + "0000000000000000.xml";
                CreateXML(xmlHeartDoc, TimesHeartSourceFilePath + "\\" + xmlStateFileName);
               // ServerForm.mainFrm.GenerateSignatureFile(TimesHeartSourceFilePath, "01" + rHeart.sHBRONO + "0000000000000000");
                tar.CreatTar(TimesHeartSourceFilePath, sSendTarPath, "01" + rHeart.sHBRONO + "0000000000000000");
            }
            catch (Exception ec)
            {
                Log.Instance.LogWrite("心跳处错误：" + ec.Message);
            }
            string sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + "01" + rHeart.sHBRONO + "0000000000000000" + ".tar";
            string pp= HttpSendFile.UploadFilesByPost(SingletonInfo.GetInstance().SendTarAddress, sHeartBeatTarName);      
            #region 心跳判断
            if (pp=="1")
            {
                //发送成功
                if (dtLinkTime != null && dtLinkTime.ToString() != "")
                {
                    int timetick = DateDiff(DateTime.Now, dtLinkTime);
                    //大于600秒（10分钟）
                    if (timetick > OnOffLineInterval)
                    {

                        this.Invoke(new Action(() =>
                        {
                            this.Text = "离线";
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.Text = "在线";
                        }));
                      
                    }
                    if (timetick > OnOffLineInterval * 3)
                    {
                        dtLinkTime = DateTime.Now.AddSeconds(-2 * OnOffLineInterval);
                    }
                }
                else
                {
                    dtLinkTime = DateTime.Now;
                }
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    this.Text = "离线";
                }));
            }
            #endregion End
            Thread.Sleep(1000);
        }

        /// <summary>
        /// 终端状态上报
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void SrvStateUP(object source, System.Timers.ElapsedEventArgs e)
        {
            PlatformEBRDTStateReported();
        }

        /// <summary>
        /// 终端信息上报
        /// </summary>
        private void SrvInfromUP(object source, System.Timers.ElapsedEventArgs e)
        {
            PlatformEBRDTInfoReported();
        }

        /// <summary>
        /// 平台状态上报
        /// </summary>
        private void TerraceStateUP(object source, System.Timers.ElapsedEventArgs e)
        {
            PlatformstateInfoReported();
        }

        /// <summary>
        /// 平台信息上报
        /// </summary>
        private void TerraceInfrom(object source, System.Timers.ElapsedEventArgs e)
        {
            PlatformInfoReported();
        }

        private void CreateXML(XmlDocument XD, string Path)
        {
            CommonFunc ComX = new CommonFunc();
            ComX.SaveXmlWithUTF8NotBOM(XD, Path);
            if (ComX != null)
            {
                ComX = null;
            }
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
                if (dtMedia.Rows.Count>0)
                {
                    List<IncrementalEBRDTState> Listtmp = new List<IncrementalEBRDTState>();
                    foreach (DataRow item in dtMedia.Rows)
                    {
                        IncrementalEBRDTState pp = new IncrementalEBRDTState();
                        pp.powersupplystatus = item["powersupplystatus"].ToString();
                        pp.SRV_LOGICAL_CODE_GB= item["SRV_LOGICAL_CODE_GB"].ToString();
                        pp.SRV_PHYSICAL_CODE = item["SRV_PHYSICAL_CODE"].ToString();
                        pp.SRV_RMT_STATUS = item["SRV_RMT_STATUS"].ToString();
                        Listtmp.Add(pp);
                    }
                  if(!CheckList(Listtmp, SingletonInfo.GetInstance().ListIncrementalEBRDTState))  
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
                        MessageBox.Show("终端状态没有发生变化");
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


        /// <summary>
        /// 比较两个列表的内容是否相等  
        /// </summary>
        /// <param name="List1"></param>
        /// <param name="List2"></param>
        /// <returns></returns>
        private bool CheckList(List<IncrementalEBRDTState>List1,List<IncrementalEBRDTState>List2)
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
        /// ccplayer推流播放停止计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerCcplayer(object source, System.Timers.ElapsedEventArgs e)
        {
            if (ccplayerStopTime < DateTime.Now)
            {
                try
                {
                    SetText("停止播发：" + DateTime.Now.ToString() + "EBM文件日期: " + ccplayerStopTime, Color.Red);
                    ccplay.StopCPPPlayer2();
                    string strSql = string.Format("update PLAYRECORD set PR_REC_STATUS = '{0}'", "删除");
                    strSql += " update EBMInfo set EBMState=1 where SEBDID='" + SEBDIDStatusFlag + "' ";
                    mainForm.dba.UpdateDbBySQL(strSql);
                    Tccplayer.Enabled = false;
                }
                catch (Exception ex)
                {
                    Log.Instance.LogWrite("直播停止ccplayer推流：" + ex.Message);
                }
            }
            Thread.Sleep(20);
        }

        private void btn_InfroState_Click(object sender, EventArgs e)
        {
            string StateFaleText = btn_InfroState.Text;
            if (StateFaleText == "信息状态上报-未开启")
            {
                tSrvState.Enabled = true;
                tSrvInfo.Enabled = true;
                tTerraceInfrom.Enabled = true;
                tTerraceState.Enabled = true;
                //InfromActiveTime.Enabled = true;
                btn_InfroState.Text = "信息状态上报-已开启";
            }
            else
            {
                tSrvState.Enabled = false;
                tSrvInfo.Enabled = false;
                tTerraceInfrom.Enabled = false;
                tTerraceState.Enabled = false;
                //InfromActiveTime.Enabled = false;
                btn_InfroState.Text = "信息状态上报-未开启";
            }
        }

        //心跳定时上报
        private void btn_HreartState_Click(object sender, EventArgs e)
        {
            string StateFaleText = btn_HreartState.Text;
            if (StateFaleText == "心跳状态上报-未开启")
            {
                t.Enabled = true;
                btn_HreartState.Text = "心跳状态上报-已开启";
            }
            else
            {
                t.Enabled = false;
                btn_HreartState.Text = "心跳状态上报-未开启";
            }
        }

        private void btn_Verify_Click(object sender, EventArgs e)
        {
            //EBMVerifyState
            string StateFaleText = btn_Verify.Text;
            if (StateFaleText == "人工审核-未开启")
            {
                SingletonInfo.GetInstance().serverini.WriteValue("EBD", "EBMState", "False");
                EBMVerifyState = false;//人工审核状态  true  表示已开启
                btn_Verify.Text = "人工审核-已开启";
            }
            else
            {
                SingletonInfo.GetInstance().serverini.WriteValue("EBD", "EBMState", "True");
                EBMVerifyState = false;
                btn_Verify.Text = "人工审核-未开启";
            }
        }

        //手动审核任务列表中审核事件
        private void list_PendingTask_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (this.list_PendingTask.SelectedItems.Count > 0)
            //{
            //    string EBMPath = this.list_PendingTask.FocusedItem.SubItems[1].Text; 
            //    AnalysisEBM(EBMPath);
            //}
        }

        /// <summary>
        /// 审核完成后下发指令
        /// </summary>
        /// <param name="EBMPath"></param>
        private void AnalysisEBMCheckOver(string EBMPath)
        {
            List<string> lDealTarFiles = new List<string>();
            List<string> AudioFileListTmp = new List<string>();//收集的音频文件列表
            List<string> AudioFileList = new List<string>();//收集的音频文件列表

            SetText("解压文件：" + EBMPath.ToString(), Color.Green);
            try
            {
                #region 解压
                if (File.Exists(EBMPath))
                {
                    try
                    {
                        DeleteFolder(sUnTarPath);
                        tar.UnpackTarFiles(EBMPath, sUnTarPath);
                        //把压缩包解压到专门存放接收到的XML文件的文件夹下
                        SetText("解压文件：" + EBMPath + "成功", Color.Green);
                    }
                    catch (Exception exa)
                    {
                        SetText("删除解压文件夹：" + sUnTarPath + "文件失败!错误信息：" + exa.Message, Color.Red);
                    }
                }
                #endregion 解压
            }
            catch (Exception ex)
            {
                Log.Instance.LogWrite("解压出错：" + ex.Message);
            }
            try
            {
                string[] xmlfilenames = Directory.GetFiles(sUnTarPath, "*.xml");//从解压XML文件夹下获取解压的XML文件名
                string sTmpFile = string.Empty;
                string sAnalysisFileName = "";
                string sSignFileName = "";

                for (int i = 0; i < xmlfilenames.Length; i++)
                {
                    sTmpFile = Path.GetFileName(xmlfilenames[i]);
                    if (sTmpFile.ToUpper().IndexOf("EBDB") > -1 && sTmpFile.ToUpper().IndexOf("EBDS_EBDB") < 0)
                    {
                        sAnalysisFileName = xmlfilenames[i];
                    }
                    else if (sTmpFile.ToUpper().IndexOf("EBDS_EBDB") > -1)//签名文件
                    {
                        sSignFileName = xmlfilenames[i];//签名文件
                    }
                }
                DeleteFolder(sSourcePath);//删除原有XML发送文件的文件夹下的XML

                if (sSignFileName == "")
                {
                    //验证签名功能
                }
                else
                {
                    #region 签名处理
                    //Console.WriteLine("开始验证签名文件!");
                    //using (FileStream SignFs = new FileStream(sSignFileName, FileMode.Open))
                    //{
                    //    StreamReader signsr = new StreamReader(SignFs, System.Text.Encoding.UTF8);
                    //    string xmlsign = signsr.ReadToEnd();
                    //    signsr.Close();
                    //    responseXML signrp = new responseXML();//签名回复
                    //    XmlDocument xmlSignDoc = new XmlDocument();
                    //    try
                    //    {
                    //        xmlsign = XmlSerialize.ReplaceLowOrderASCIICharacters(xmlsign);
                    //        xmlsign = XmlSerialize.GetLowOrderASCIICharacters(xmlsign);
                    //        Signature sign = XmlSerialize.DeserializeXML<Signature>(xmlsign);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Log.Instance.LogWrite("签名文件错误：" + ex.Message);
                    //    }
                    //}
                    //Console.WriteLine("结束验证签名文件！");
                    #endregion End
                }

                if (sAnalysisFileName != "")
                {
                    using (FileStream fs = new FileStream(sAnalysisFileName, FileMode.Open))
                    {
                        StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                        String xmlInfo = sr.ReadToEnd();
                        xmlInfo = xmlInfo.Replace("xmlns:xs", "xmlns");
                        sr.Close();
                        xmlInfo = XmlSerialize.ReplaceLowOrderASCIICharacters(xmlInfo);
                        xmlInfo = XmlSerialize.GetLowOrderASCIICharacters(xmlInfo);
                        ebd = XmlSerialize.DeserializeXML<EBD>(xmlInfo);
                     
                        AudioFileListTmp.Clear();
                        AudioFileList.Clear();
                        string[] mp3files = Directory.GetFiles(sUnTarPath, "*.mp3");
                        AudioFileListTmp.AddRange(mp3files);

                        EBMInfo EBMInfo = new EBMInfo();
                        EBMInfo.ebd = ebd;
                        if (AudioFileListTmp.Count > 0)
                        {
                            EBMInfo.AudioUrl = AudioFileListTmp[0];
                        }
                        if (AudioFileListTmp.Count > 0)//文件播放
                        {
                            string sTmpDealFile = string.Empty;
                            string targetPath = string.Empty;
                            
                            for (int ai = 0; ai < AudioFileListTmp.Count; ai++)
                            {

                                string xmlFilePath = "";

                                string xmlFile = Path.GetFileName(sAnalysisFileName);
                                xmlFilePath = sAudioFilesFolder + "\\" + xmlFile;
                                System.IO.File.Copy(sAnalysisFileName, xmlFilePath, true);


                                sTmpDealFile = Path.GetFileName(AudioFileListTmp[ai]);
                                targetPath = sAudioFilesFolder + "\\" + sTmpDealFile;
                                System.IO.File.Copy(AudioFileListTmp[ai], targetPath, true);
                                AudioFileList.Add(targetPath);


                                #region  保存EBMInfo 新增于20181214
                                string ebminfoid = "";
                                ebminfoid = SaveEBD(ebd);
                                #endregion

                                #region 音频播放 20181212
                                if (SingletonInfo.GetInstance().DicPlayingThread.ContainsKey(ebd.EBM.MsgContent.AreaCode))
                                {
                                    foreach (var item in SingletonInfo.GetInstance().DicPlayingThread[ebd.EBM.MsgContent.AreaCode])
                                    {
                                        item.Abort();
                                    }
                                    SingletonInfo.GetInstance().DicPlayingThread.Remove(ebd.EBM.MsgContent.AreaCode);

                                    SetText("开播前删除字典值：" + ebd.EBM.MsgContent.AreaCode + "-->" + SingletonInfo.GetInstance().DicPlayingThread.Count.ToString(), Color.Purple);
                                }

                                PlayElements pe = new PlayElements();
                                pe.EBDITEM = ebd;
                                pe.sAnalysisFileName = sAnalysisFileName;
                                pe.targetPath = SingletonInfo.GetInstance().RemoteFTPpath + "\\" + ebd.EBM.MsgContent.Auxiliary.AuxiliaryDesc;
                                pe.xmlFilePath = xmlFilePath;
                                pe.EBMInfoID = ebminfoid;
                                ParameterizedThreadStart ParStart = new ParameterizedThreadStart(PlaybackProcess);
                                Thread myThread = new Thread(ParStart);
                                myThread.IsBackground = true;
                                List<Thread> ThreadList = new List<Thread>();
                                ThreadList.Add(myThread);

                                SingletonInfo.GetInstance().DicPlayingThread.Add(ebd.EBM.MsgContent.AreaCode, ThreadList);
                                myThread.Start(pe);
                                #endregion----------------------------------
                            }
                        }
                        else//文本转语音
                        {

                            #region  保存EBMInfo 新增于20181214
                            string ebminfoid = "";
                            ebminfoid = SaveEBD(ebd);
                            #endregion


                            string xmlFile = Path.GetFileName(sAnalysisFileName);
                            string xmlFilePath = sAudioFilesFolder + "\\" + xmlFile;


                            PlayElements pe = new PlayElements();
                            pe.EBDITEM = ebd;
                            pe.sAnalysisFileName = sAnalysisFileName;
                            pe.targetPath = "";
                            pe.xmlFilePath = xmlFilePath;
                            pe.EBMInfoID = ebminfoid;
                            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(PlaybackProcess);

                            Thread myThread = new Thread(ParStart);
                            myThread.IsBackground = true;

                            if (SingletonInfo.GetInstance().DicPlayingThread.ContainsKey(ebd.EBM.MsgContent.AreaCode))
                            {
                                SingletonInfo.GetInstance().DicPlayingThread.Remove(ebd.EBM.MsgContent.AreaCode);
                            }

                            List<Thread> ThreadList = new List<Thread>();
                            ThreadList.Add(myThread);
                            SingletonInfo.GetInstance().DicPlayingThread.Add(ebd.EBM.MsgContent.AreaCode, ThreadList);
                            myThread.Start(pe);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 播发状态请求反馈  20181214
        /// </summary>
        private void EBMStateRequest(EBD ebd)
        {
            SetText("EBMStateRequest    NO:1", Color.Orange);
            try
            {
                XmlDocument xmlStateDoc = new XmlDocument();
                responseXML rState = new responseXML();
                //rState.SourceAreaCode = ServerForm.strSourceAreaCode;
                //rState.SourceType = ServerForm.strSourceType;
                //rState.SourceName = ServerForm.strSourceName;
                //rState.SourceID = ServerForm.strSourceID;
                //rState.sHBRONO = SingletonInfo.GetInstance().CurrentResourcecode;
                string frdStateName = "10" + SingletonInfo.GetInstance().CurrentResourcecode + BBSHelper.GetSequenceCodes();
                string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";
                if (ebd == null)
                    return;
                string EBMID = ebd.EBMStateRequest.EBM.EBMID;
                try
                {
                    string MediaSql = "select * from  EBMInfo where EBMID='" + EBMID + "'";
                    DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);
                    string BrdStateCode = "";
                    if (dtMedia.Rows.Count>0)
                    {
                        BrdStateCode = dtMedia.Rows[0]["EBMState"].ToString();
                    }
                    xmlStateDoc = rState.ResponeEBMStateRequrest(ebd, frdStateName, BrdStateCode);
                    UnifyCreateTar(xmlStateDoc, frdStateName);
                    string sHeartBeatTarName = sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                    send.address = SingletonInfo.GetInstance().SendTarAddress;
                    send.fileNamePath = sHeartBeatTarName;
                    postfile.UploadFilesByPostThread(send);
                }
                catch
                {
                }
            }
            catch (Exception h)
            {
                Log.Instance.LogWrite("错误510行:" + h.Message);
            }
        }

        //指令MQ初始化
        private void MQActivStart()
        {
           SingletonInfo.GetInstance().m_mq = new MQ();
            SingletonInfo.GetInstance().m_mq.uri = SingletonInfo.GetInstance().serverini.ReadValue("MQActiveOrder", "ServerUrl");
            SingletonInfo.GetInstance().m_mq.username = SingletonInfo.GetInstance().serverini.ReadValue("MQActiveOrder", "User");
            SingletonInfo.GetInstance().m_mq.password = SingletonInfo.GetInstance().serverini.ReadValue("MQActiveOrder", "Password");
            SingletonInfo.GetInstance().m_mq.Start();
            Thread.Sleep(500);
            SingletonInfo.GetInstance().m_mq.CreateProducer(true,"fee.bar");
        }

        private void ConnectMQServer()
        {
            try
            {
                SingletonInfo.GetInstance().m_mq_checkEBM = new MQ();
                SingletonInfo.GetInstance().m_mq_checkEBM.uri = SingletonInfo.GetInstance().serverini.ReadValue("MQCheckEbmInfo", "ServerUrl");
                SingletonInfo.GetInstance().m_mq_checkEBM.username = SingletonInfo.GetInstance().serverini.ReadValue("MQCheckEbmInfo", "User");
                SingletonInfo.GetInstance().m_mq_checkEBM.password = SingletonInfo.GetInstance().serverini.ReadValue("MQCheckEbmInfo", "Password");
                SingletonInfo.GetInstance().m_mq_checkEBM.Start();
                isConn = true;
                m_consumer = SingletonInfo.GetInstance().m_mq_checkEBM.CreateConsumer(true, SingletonInfo.GetInstance().serverini.ReadValue("MQCheckEbmInfo", "ReceiveTopicName"));
                m_consumer.Listener += new MessageListener(consumer_listener_ChenckData);
                SingletonInfo.GetInstance().m_mq_checkEBM.CreateProducer(true, SingletonInfo.GetInstance().serverini.ReadValue("MQCheckEbmInfo", "SendTopicName"));//创建消息生产者   //Queue
            }
            catch (Exception ex)
            {
                Log.Instance.LogWrite(ex.Message);
            }

        }

        private void InitFTPServer()
        {
           string ftpserver= SingletonInfo.GetInstance().serverini.ReadValue("FTPServer", "ftpserver");
            string ftpusername= SingletonInfo.GetInstance().serverini.ReadValue("FTPServer", "ftpusername");
            string ftppwd= SingletonInfo.GetInstance().serverini.ReadValue("FTPServer", "ftppwd");
            ftphelper = new FTPHelper(ftpserver, ftpusername, ftppwd);
        }



        /// <summary>
        ///  MQ消息接收   键值对
        /// </summary>
        /// <param name="message"></param>
        private void consumer_listener_ChenckData(IMessage message)
        {
            try
            {
               Serialize(message.Properties);
            }
            catch (Exception ex)
            {
                this.m_consumer.Close();
            }
        }

        public void Serialize(IPrimitiveMap MsgMap)
        {

            string ID = MsgMap["ID"].ToString();
            string EBDDID = MsgMap["EBDDID"].ToString();
            string EBMPath = sRevTarPath + "\\EBDT_" + EBDDID + ".tar";
            AnalysisEBMCheckOver(EBMPath);

        }


        private bool SendMQOrderNew(int Type, string ParamValue, string TsCmd_ID, string TsCmd_ValueID, bool flag = true)
        {
            try
            {
                if (ebd != null && flag)
                {
                    string InfoValueStr = "insert into InfoVlaue values('" + ebd.EBDID + "')";
                    mainForm.dba.UpdateDbBySQL(InfoValueStr);
                }
                if (ParamValue.Length > 0)
                {
                    if (SingletonInfo.GetInstance().m_mq == null)
                    {
                        MQActivStart();
                    }
                }
                m_lstProperty = InstallNew(Type, ParamValue, TsCmd_ID, TsCmd_ValueID);//~0~1200~192~0~1~1应急

                return SingletonInfo.GetInstance().m_mq.SendMQMessage(true, "Send", m_lstProperty);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 组装MQ指令
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="value"></param>
        /// <param name="TsCmd_ID"></param>
        /// <param name="TsCmd_ValueIDTmp"></param>
        /// <returns></returns>
        private List<Property> InstallNew(int Type, string value, string TsCmd_ID, string TsCmd_ValueIDTmp)
        {
            List<Property> InstallList = new List<Property>();
            Property item = new Property();
            item.name = "TsCmd_Mode";
            item.value = "区域";
            InstallList.Add(item);


            Property itemPlayCount = new Property();
            itemPlayCount.name = "TsCmd_PlayCount";
            itemPlayCount.value = "1";
            InstallList.Add(itemPlayCount);



            Property itemEndTime = new Property(); ;
            itemEndTime.name = "TsCmd_EndTime";
            itemEndTime.value = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd HH:mm:ss");
            InstallList.Add(itemEndTime);


            Property itemTime = new Property(); ;
            itemTime.name = "TsCmd_Date";
            // itemTime.value = DateTime.Now.AddSeconds(2).ToString("yyyy-MM-dd HH:mm:ss");
            itemTime.value = DateTime.Now.AddSeconds(-30).ToString("yyyy-MM-dd HH:mm:ss");
            InstallList.Add(itemTime);

            Property itemStatus = new Property();
            itemStatus.name = "TsCmd_Status";
            itemStatus.value = "0";
            InstallList.Add(itemStatus);

            Property itemVoice = new Property();
            itemVoice.name = "VOICE";
            itemVoice.value = "4";
            InstallList.Add(itemVoice);

            Property itemTsCmd_ID = new Property();
            itemTsCmd_ID.name = "TsCmd_ID";
            itemTsCmd_ID.value = TsCmd_ID;
            InstallList.Add(itemTsCmd_ID);

            Type t = MQUserInfo.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (var PropertyInfo in PropertyList)
            {
                Property userinfo = new Property();
                userinfo.name = PropertyInfo.Name;

                if (userinfo.name == "TsCmd_ValueID")
                {
                    userinfo.value = TsCmd_ValueIDTmp;
                }
                else
                {
                    object valueobj = PropertyInfo.GetValue(MQUserInfo, null);
                    userinfo.value = valueobj == null ? "" : valueobj.ToString();
                }
                InstallList.Add(userinfo);

            }
            string strOrder = "";


            if (Type == 1)//音频文件播发
            {
                Property itemType = new Property();
                itemType.name = "TsCmd_Type";
                itemType.value = "播放视频GB";
                InstallList.Add(itemType);
            }
            else
            {
                Property itemType = new Property();
                itemType.name = "TsCmd_Type";
                itemType.value = "音源播放";
                InstallList.Add(itemType);
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

        private bool SendMQOrder(int Type, string ParamValue, string TsCmd_ID)
        {
            try
            {
                if (ebd != null)
                {
                    string InfoValueStr = "insert into InfoVlaue values('" + ebd.EBDID + "')";
                    mainForm.dba.UpdateDbBySQL(InfoValueStr);
                }
                if (ParamValue.Length > 0)
                {
                    if (SingletonInfo.GetInstance().m_mq == null)
                    {
                        MQActivStart();
                    }
                }
                m_lstProperty = Install(Type, ParamValue, TsCmd_ID);//~0~1200~192~0~1~1应急
                return SingletonInfo.GetInstance().m_mq.SendMQMessage(true, "Send", m_lstProperty);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 组装MQ指令
        /// </summary>
        /// <param name="Type">指令Type 1(音频文件播发) 2(网络URL播发)</param>
        /// <param name="value"></param>
        private List<Property> Install(int Type, string value, string TsCmd_ID)
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

            Type t = MQUserInfo.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (var PropertyInfo in PropertyList)
            {
                Property userinfo = new Property();
                userinfo.name = PropertyInfo.Name;
                object valueobj = PropertyInfo.GetValue(MQUserInfo, null);
                userinfo.value = valueobj == null ? "" : valueobj.ToString();
                InstallList.Add(userinfo);

            }
            string strOrder = "";


            if (Type == 1)//音频文件播发
            {
                Property itemType = new Property();
                itemType.name = "TsCmd_Type";
                itemType.value = "播放视频";
                InstallList.Add(itemType);
            }
            else
            {
                Property itemType = new Property();
                itemType.name = "TsCmd_Type";
                itemType.value = "音源播放";
                InstallList.Add(itemType);
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


        private void FindUserInfo(string Name)
        {
            string sql = "select * from Users U inner join Organization O on U.USER_ORG_CODE=O.ORG_CODEA where U.USER_DETAIL='" + Name + "'";
            DataTable dtUser = mainForm.dba.getQueryInfoBySQL(sql);
            if (dtUser.Rows.Count > 0)
            {
                MQUserInfo.USER_PRIORITY = dtUser.Rows[0]["USER_PRIORITY"].ToString();
                MQUserInfo.TsCmd_UserID = dtUser.Rows[0]["USER_ID"].ToString();
                MQUserInfo.USER_ORG_CODE = dtUser.Rows[0]["USER_ORG_CODE"].ToString();
                // MQUserInfo.TsCmd_ValueID = dtUser.Rows[0]["ORG_ID"].ToString();
                SingletonInfo.GetInstance().USER_PRIORITY = MQUserInfo.USER_PRIORITY;
                SingletonInfo.GetInstance().TsCmd_UserID = MQUserInfo.TsCmd_UserID;
                SingletonInfo.GetInstance().USER_ORG_CODE = MQUserInfo.USER_ORG_CODE;
            }
        }


        /// <summary>
        /// 复制大文件
        /// </summary>
        /// <param name="fromPath">源文件的路径</param>
        /// <param name="toPath">文件保存的路径</param>
        /// <param name="eachReadLength">每次读取的长度</param>
        /// <returns>是否复制成功</returns>
        public bool CopyFile(string fromPath, string toPath, int eachReadLength)
        {
            //将源文件 读取成文件流
            FileStream fromFile = new FileStream(fromPath, FileMode.Open, FileAccess.Read);
            //已追加的方式 写入文件流
            FileStream toFile = new FileStream(toPath, FileMode.Append, FileAccess.Write);
            //实际读取的文件长度
            int toCopyLength = 0;
            //如果每次读取的长度小于 源文件的长度 分段读取
            if (eachReadLength < fromFile.Length)
            {
                byte[] buffer = new byte[eachReadLength];
                long copied = 0;
                while (copied <= fromFile.Length - eachReadLength)
                {
                    toCopyLength = fromFile.Read(buffer, 0, eachReadLength);
                    fromFile.Flush();
                    toFile.Write(buffer, 0, eachReadLength);
                    toFile.Flush();
                    //流的当前位置
                    toFile.Position = fromFile.Position;
                    copied += toCopyLength;
                }
                int left = (int)(fromFile.Length - copied);
                toCopyLength = fromFile.Read(buffer, 0, left);
                fromFile.Flush();
                toFile.Write(buffer, 0, left);
                toFile.Flush();
            }
            else
            {
                //如果每次拷贝的文件长度大于源文件的长度 则将实际文件长度直接拷贝
                byte[] buffer = new byte[fromFile.Length];
                fromFile.Read(buffer, 0, buffer.Length);
                fromFile.Flush();
                toFile.Write(buffer, 0, buffer.Length);
                toFile.Flush();
            }
            fromFile.Close();
            toFile.Close();
            return true;
        }

    }
}
