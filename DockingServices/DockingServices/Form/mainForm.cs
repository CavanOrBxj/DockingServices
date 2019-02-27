using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DockingServices
{
    public partial class mainForm : Form
    {
        //子窗体定义
        private ServerIPSetForm setipFrm;
        private ServerSetForm setServerFrm;
        private Form_playtactics playtacticsFrm;
        private From_Timetactics TimetacticsFrm;
        private TmpFolderSetForm tmpforldFrm;
        private InfoSetForm infoFrm;
        public ServerForm serverFrm;
        //
        public static dbAccess dba;
        public List<string> lTarPathName = new List<string>();//接收到的Tar包列表
        public static string sSendTarName = "";//发送Tar包名字

        //public bool MQStartFlag = false;
        public string strSourceType = "";
        public string strSourceName = "";
        public string strSourceID = "";
       
        public static SerialPort comm = new SerialPort();
        public static SerialPort sndComm = new SerialPort();//临时发送语音用

        public static bool bWaitOrNo = true;//等待 2016-04-01
        public static bool bMsgStatusFree = false;//

        private List<byte> lCommData = new List<byte>();
        private object oComm = new object();
        private Thread thComm;

        public USBE usb = new USBE();
        public IntPtr phDeviceHandle = (IntPtr)1;

        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            try
            {
                SingletonInfo.GetInstance().m_UsbPwsSupport = SingletonInfo.GetInstance().serverini.ReadValue("USBPSW", "USBPSWSUPPART");
                SingletonInfo.GetInstance().IsCompatible= SingletonInfo.GetInstance().serverini.ReadValue("CompatibleMode", "IsCompatible")=="1"?true:false;
                //打开密码器
                if (SingletonInfo.GetInstance().m_UsbPwsSupport == "1"&& !SingletonInfo.GetInstance().IsCompatible)
                {
                    try
                    {
                        int nReturn = usb.USB_OpenDevice(ref phDeviceHandle);
                        if (nReturn != 0)
                        {
                            MessageBox.Show("密码器打开失败！");
                        }
                    }
                    catch (Exception em)
                    {
                        MessageBox.Show("密码器打开失败：" + em.Message);
                    }
                }

                //初始化写日志线程
                string sLogPath = Application.StartupPath + "\\Log";
                if (!Directory.Exists(sLogPath))
                    Directory.CreateDirectory(sLogPath);
                Log.Instance.LogDirectory = sLogPath + "\\";
                Log.Instance.FileNamePrefix = "EBD_";
                Log.Instance.CurrentMsgType = MsgLevel.Debug;
                Log.Instance.logFileSplit = LogFileSplit.Daily;
                Log.Instance.MaxFileSize = 2;
                Log.Instance.InitParam();

                GetAuditData();
                GetPlatformInfo();

                this.Invoke(new Action(() =>
                {
                    this.Text = "应急广播消息服务V" + Application.ProductVersion;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region  获取审核策略数据
        private void GetAuditData()
        {
            string sqlstr = "select * from EBTime_Strategy";
            DataTable dt = mainForm.dba.getQueryInfoBySQL(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                timestrategies tt = new timestrategies();
                tt.ID= dt.Rows[i][0].ToString();
                tt.StartTime= dt.Rows[i][1].ToString();
                tt.EndTime= dt.Rows[i][2].ToString();
                tt.EvenType= dt.Rows[i][3].ToString();
                SingletonInfo.GetInstance().audit.TimeList.Add(tt);
            }
        }
        #endregion

        #region  从数据库获取平台基本信息
        private void GetPlatformInfo()
        {
            if (SingletonInfo.GetInstance().IsCompatible)
            {
                //兼容版本  平台基本数据从配置文件读取
                SingletonInfo.GetInstance().CurrentURL = SingletonInfo.GetInstance().serverini.ReadValue("CompatibleMode", "URL");
                SingletonInfo.GetInstance().CurrentResourcecode = SingletonInfo.GetInstance().serverini.ReadValue("CompatibleMode", "HBRONO");
                SingletonInfo.GetInstance().PlatformEBRName = SingletonInfo.GetInstance().serverini.ReadValue("CompatibleMode", "EBRName");
                SingletonInfo.GetInstance().PlatformContact = SingletonInfo.GetInstance().serverini.ReadValue("CompatibleMode", "Contact");
                SingletonInfo.GetInstance().PlatformPhoneNumber = SingletonInfo.GetInstance().serverini.ReadValue("CompatibleMode", "PhoneNumber");
                SingletonInfo.GetInstance().Longitude = SingletonInfo.GetInstance().serverini.ReadValue("CompatibleMode", "Longitude");
                SingletonInfo.GetInstance().Latitude = SingletonInfo.GetInstance().serverini.ReadValue("CompatibleMode", "Latitude");
                SingletonInfo.GetInstance().PlatformAddress = SingletonInfo.GetInstance().serverini.ReadValue("CompatibleMode", "Address");
                SingletonInfo.GetInstance().SendTarAddress = SingletonInfo.GetInstance().serverini.ReadValue("CompatibleMode", "BJURL");
            }
            else
            {
                //不是兼容版本 平台基本数据需要从数据库获取
                string sqlstr = "select * from PlatformResource";
                DataTable dt = mainForm.dba.getQueryInfoBySQL(sqlstr);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (dt.Rows[i]["platformType"].ToString() == "1")
                        {
                            SingletonInfo.GetInstance().CurrentURL = dt.Rows[i]["ipAddress"].ToString();
                            SingletonInfo.GetInstance().CurrentResourcecode = dt.Rows[i]["sourceCode"].ToString();
                            SingletonInfo.GetInstance().PlatformEBRName = dt.Rows[i]["platformName"].ToString();
                            SingletonInfo.GetInstance().PlatformContact = dt.Rows[i]["contact"].ToString();
                            SingletonInfo.GetInstance().PlatformPhoneNumber = dt.Rows[i]["phone"].ToString();
                            SingletonInfo.GetInstance().Longitude = dt.Rows[i]["longitude"].ToString();
                            SingletonInfo.GetInstance().Latitude = dt.Rows[i]["latitude"].ToString();
                            SingletonInfo.GetInstance().PlatformAddress = dt.Rows[i]["address"].ToString();
                        }
                        else
                            if (dt.Rows[i]["platformType"].ToString() == "-1")
                        {
                            SingletonInfo.GetInstance().SendTarAddress = dt.Rows[i]["ipAddress"].ToString();
                        }

                    }
                }
            }
        }
        #endregion

        #region 菜单响应

        private void mnuServerAddrSet_Click(object sender, EventArgs e)
        {
            if (setServerFrm == null || setServerFrm.IsDisposed)
            {
                setServerFrm = new ServerSetForm();
                setServerFrm.MdiParent = this;
                setServerFrm.Show();
            }
            else
            {
                if (setServerFrm.WindowState == FormWindowState.Minimized)
                {
                    setServerFrm.WindowState = FormWindowState.Normal;
                }
                else
                    setServerFrm.Activate();
            }
        }

        private void ServerIPSet_Click(object sender, EventArgs e)
        {
            if (setipFrm == null || setipFrm.IsDisposed)
            {
                setipFrm = new ServerIPSetForm();
                setipFrm.MdiParent = this;
                setipFrm.Show();
            }
            else
            {
                if (setipFrm.WindowState == FormWindowState.Minimized)
                {
                    setipFrm.WindowState = FormWindowState.Normal;
                }
                else
                    setipFrm.Activate();
            }
        }

        private void mnuFolderSet_Click(object sender, EventArgs e)
        {
            if (tmpforldFrm == null || tmpforldFrm.IsDisposed)
            {
                tmpforldFrm = new TmpFolderSetForm();
                tmpforldFrm.MdiParent = this;
                tmpforldFrm.Show();
            }
            else
            {
                if (tmpforldFrm.WindowState == FormWindowState.Minimized)
                {
                    tmpforldFrm.WindowState = FormWindowState.Normal;
                }
                else
                    tmpforldFrm.Activate();
            }
        }

        private void mnuSysInfoSet_Click(object sender, EventArgs e)
        {
            if (infoFrm == null || infoFrm.IsDisposed)
            {
                infoFrm = new InfoSetForm();
                infoFrm.MdiParent = this;
                infoFrm.Show();
            }
            else
            {
                if (infoFrm.WindowState == FormWindowState.Minimized)
                {
                    infoFrm.WindowState = FormWindowState.Normal;
                }
                else
                    infoFrm.Activate();
            }
        }

        private void mnuServerStart_Click(object sender, EventArgs e)
        {
            if (serverFrm == null || serverFrm.IsDisposed)
            {
                try
                {
                    serverFrm = new ServerForm();
                    serverFrm.MdiParent = this;
                    serverFrm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "mainFormb");
                }
            }
            else
            {
                if (serverFrm.WindowState == FormWindowState.Minimized)
                {
                    serverFrm.WindowState = FormWindowState.Normal;
                }
                else
                    serverFrm.Activate();
            }
        }

        #endregion End

        private void mnuExit_Click(object sender, EventArgs e)
        {
            if (serverFrm != null)
            {
                serverFrm.Close();
                serverFrm.Dispose();
            }
            if (comm != null)
            {
                comm.Close();
                comm.Dispose();
            }
            if (sndComm != null)
            {
                sndComm.Close();
                sndComm.Dispose();
            }
            this.Dispose(true);//释放资源
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
            if (serverFrm != null)
                serverFrm.Hide();
            this.Visible = false;
            this.Hide();
            this.ShowInTaskbar = false;
            this.nIcon.Visible = true;
            //关闭密码器
            if (SingletonInfo.GetInstance().m_UsbPwsSupport == "1" && !SingletonInfo.GetInstance().IsCompatible)
            {
                try
                {
                    int nDeviceHandle = (int)phDeviceHandle;
                    int nReturn = usb.USB_CloseDevice(ref nDeviceHandle);
                }
                catch (Exception em)
                {
                    MessageBox.Show("密码器关闭失败：" + em.Message);
                }
            }
            if (serverFrm != null)
            {
                serverFrm.Close();
                serverFrm.Dispose();
            }
            if (comm != null)
            {
                comm.Close();
                comm.Dispose();
            }
            if (sndComm != null)
            {
                sndComm.Close();
                sndComm.Dispose();
            }
            this.Dispose();                //释放资源
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void mnuShow_Click(object sender, EventArgs e)
        {
            if (!this.ShowInTaskbar)
            {
                this.Visible = true;
                this.Show();
                this.ShowInTaskbar = true;
                nIcon.Visible = false;
                foreach (Form frm in this.MdiChildren)
                {
                    if (!frm.IsDisposed & frm != null)
                        frm.Show();
                }
            }
        }

        private void mnuQuit_Click(object sender, EventArgs e)
        {
            if (serverFrm != null)
            {
                serverFrm.Close();
                serverFrm.Dispose();
            }
            if (comm != null)
            {
                comm.Close();
                comm.Dispose();
            }
            if (sndComm != null)
            {
                sndComm.Close();
                sndComm.Dispose();
            }
            if (thComm != null)
            {
                thComm.Abort();
                thComm = null;
            }
            this.Dispose(true);
            Application.ExitThread();
        }

        /// <summary>
        /// 文件签名
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strEBDID"></param>
        public void GenerateSignatureFile(string strPath, string strEBDID)
        {
            if (SingletonInfo.GetInstance().m_UsbPwsSupport != "1")
            {
                return;
            }

            string sSignFileName = "\\EBDB_" + strEBDID + ".xml";

            using (FileStream SignFs = new FileStream(strPath + sSignFileName, FileMode.Open))
            {
                StreamReader signsr = new StreamReader(SignFs, Encoding.UTF8);
                string xmlsign = signsr.ReadToEnd();
                signsr.Close();
                responseXML signrp = new responseXML();
                XmlDocument xmlSignDoc = new XmlDocument();
                try
                {
                    //对文件进行签名
                    int nDeviceHandle = (int)phDeviceHandle;
                    byte[] pucSignature = Encoding.UTF8.GetBytes(xmlsign);

                    string strSignture = "";
                    string strpucCounter = "";
                    string strpucSignCerSn = "";
                    string nReturn = usb.Platform_CalculateSingature_String(nDeviceHandle, 1, pucSignature, pucSignature.Length, ref strSignture);
                    //生成签名文件
                    string xmlSIGNFileName = "\\EBDS_EBDB_" + strEBDID + ".xml";
                    xmlSignDoc = signrp.SignResponse(strEBDID, strpucCounter, strpucSignCerSn, nReturn);
                    CommonFunc cm = new CommonFunc();
                    cm.SaveXmlWithUTF8NotBOM(xmlSignDoc, strPath + xmlSIGNFileName);
                    if (cm != null)
                    {
                        cm = null;
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.LogWrite("签名文件错误：" + ex.Message);
                }
            }
        }

        private void mnuauditpolicy_Click(object sender, EventArgs e)
        {
            try
            {
                if (TimetacticsFrm == null || TimetacticsFrm.IsDisposed)
                {
                    TimetacticsFrm = new From_Timetactics();
                    TimetacticsFrm.MdiParent = this;
                    TimetacticsFrm.Show();
                }
                else
                {
                    if (TimetacticsFrm.WindowState == FormWindowState.Minimized)
                    {
                        TimetacticsFrm.WindowState = FormWindowState.Normal;
                    }
                    else
                        TimetacticsFrm.Activate();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
