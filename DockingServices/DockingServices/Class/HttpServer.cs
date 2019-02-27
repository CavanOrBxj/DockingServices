using DockingServices.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace DockingServices
{
    /// <summary>
    /// Http GET，POST处理过程类
    /// </summary>
    public class HttpProcessor
    {
        public TcpClient socket;
        public HttpServerBase srv;
        private Stream inputStream;
        public Stream outputStream;
        public string http_method;
        public string http_url;
        public string http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();
        private string sSeparateString = string.Empty;
        private string sEndLine = "\r\n";

        private static int MAX_POST_SIZE = 100 * 1024 * 1024; // 100MB

        public HttpProcessor(TcpClient s, HttpServerBase srv)
        {
            this.socket = s;
            this.srv = srv;
        }

        private string streamReadLine(Stream inputStream)
        {
            string data = "";
            int next_char;
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }

        private string streamDataReadLine(Stream inputStream, ref List<byte> lLData)
        {
            List<byte> lListValue = new List<byte>();
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                lListValue.Add((byte)next_char);
                if (next_char == '\n')
                {
                    break;
                }
                if (next_char == '\r')
                {
                    continue;
                }
                if (next_char == -1)
                {
                    Thread.Sleep(1); continue;
                }
                data += Convert.ToChar(next_char);
            }
            if (lLData.Count > 0)
                lLData.Clear();
            lLData.AddRange(lListValue);
            return data;
        }

        public void process()
        {
            // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
            // "processed" view of the world, and we want the data raw after the headers
            inputStream = new BufferedStream(socket.GetStream());
            // we probably shouldn't be using a streamwriter for all output from handlers either
            //outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            outputStream = new BufferedStream(socket.GetStream());
            try
            {
                if (parseRequest() == false)
                {
                    writeFailure();//返回失败标志
                    outputStream.Flush();
                    inputStream = null; outputStream = null;
                    socket.Close();
                    return;
                }
                readHeaders();
                if (http_method.Equals("GET"))
                {
                    // handleGETRequest();
                }
                else if (http_method.Equals("POST"))
                {
                    handlePOSTRequest();
                }
                else if (http_method.Equals("PUT"))//E:\工作\93\RevTarTmp
                {
                    ServerForm.lRevFiles.Add("D:\\work\\93\\RevTarTmp\\EBDT_100102320000000000010000000000005306.tar");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("处理请求错误: " + ex.Message);
                writeFailure();
            }
            try
            {
                outputStream.Flush();
                inputStream = null; outputStream = null;
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 验证处理请求
        /// </summary>
        /// <returns>处理成功标志</returns>
        public bool parseRequest()
        {
            string request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                Log.Instance.LogWrite("头部验证错误，无法解析，丢弃处理！");
                Console.WriteLine("头部验证错误，无法解析，丢弃处理！");
                return false;
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];
            Console.WriteLine("头部验证字符串：" + request);
            return true;
        }

        public void readHeaders()
        {
            Console.WriteLine("readHeaders()");
            string line;
            sSeparateString = string.Empty;//初始化接收
            while ((line = streamReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    Console.WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    if (line != "platformtype" && (sSeparateString != "" && !line.Contains(sSeparateString)))
                    {
                        if (line == "" || line == string.Empty)
                        {
                            return;//结束头部
                        }
                        else
                        {
                            Console.WriteLine("头部验证出错!");
                            return;
                        }
                    }
                    else
                    {
                        //Console.WriteLine(line);
                        continue;
                    }
                }
                String name = line.Substring(0, separator);

                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }
                string value = line.Substring(pos, line.Length - pos);
                if (name == "Content-Type" && sSeparateString == "")
                {
                    string[] sSeparateVaule = value.Split('=');
                    if (sSeparateVaule.Length > 1)
                    {
                        sSeparateString = sSeparateVaule[1];
                    }
                }
                Console.WriteLine("头部: {0}:{1}", name, value);
                httpHeaders[name] = value;
            }
        }

        public void handleGETRequest()
        {
            srv.handleGETRequest(this);
        }

        private const int BUF_SIZE = 10 * 1024 * 1024;

        public void handlePOSTRequest()
        {
            Console.WriteLine("get post data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    Console.WriteLine(string.Format("POST Content-Length({0}) too big for this simple server", content_len));
                    return;
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)
                {
                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    if (numread == 0)
                    {
                        if (to_read == 0)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("client disconnected during post");
                        }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            Console.WriteLine("get post data end");

            PostRequestDeal(new StreamReader(ms), ServerForm.sRevTarPath);
            return;
        }

      

        private void PostRequestDeal(StreamReader sr, string sFileForldPath)
        {
            //直接使用StreamReader为导致接收文件数据缺失，直接用Stream可接收所有数据，但需自行处理分行和结尾，
            //有其他更好方法请自行修改
            Console.WriteLine("361:" + sFileForldPath);
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                int content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    Console.WriteLine(String.Format("POST Content-Length({0}) too big for this simple server", content_len));
                }
                try
                {
                    Stream stream = sr.BaseStream;
                    string sFilePath = string.Empty;
                    int charData = 0;
                    List<byte> data = new List<byte>();
                    List<byte[]> dataArray = new List<byte[]>();
                    while (stream.Position != stream.Length && charData != -1)
                    {
                        charData = stream.ReadByte();
                        data.Add((byte)charData);
                    }
                    if (data.Count < 200) return;
                    int index = data.IndexOf((byte)'\n');
                    while (index >= 0)
                    {
                        dataArray.Add(data.Take(index + 1).ToArray());
                        data.RemoveRange(0, index + 1);
                        index = data.IndexOf((byte)'\n');
                    }
                    int startIndex = 0;
                    int endIndex = 0;
                    int length = 0;
                    bool flag = false;//是否需要特殊处理  20180108
                    for (int j = 0; j < dataArray.Count; j++)
                    {
                        string str = Encoding.UTF8.GetString(dataArray[j], 0, dataArray[j].Length - 2);

                        #region 解析Content-Disposition
                        if (str.Contains("Content-Disposition"))
                        {
                            string[] sSeparateVaule = str.Split('=');
                            if (sSeparateVaule.Length > 1)
                            {
                                string revfilename = sSeparateVaule[sSeparateVaule.Length - 1];//文件名
                                if (revfilename != "")
                                {
                                    revfilename = revfilename.Replace("\"", "");
                                    sFilePath = sFileForldPath + "\\" + revfilename;
                                }
                                else
                                {
                                    sFilePath = sFileForldPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".tar";
                                }
                                revfilename = string.Empty;
                            }
                        }
                        #endregion
                        //判断是数据开头则退出循环
                        if (dataArray[j][0] == 69 && dataArray[j][1] == 66 && dataArray[j][2] == 68 && (dataArray[j][3] == 66 || dataArray[j][3] == 82 || dataArray[j][3] == 84 || dataArray[j][3] == 83))
                        {
                            if (dataArray[j][3] == 82 || dataArray[j][3] == 84 || dataArray[j][3] == 83)
                            {
                                flag = true;
                            }
                            startIndex = j;
                            break;
                        }
                        length += dataArray[j].Length;
                    }
                    for (int j = dataArray.Count - 1; j > 1; j--)
                    {
                        length += dataArray[j].Length;
                        string str = Encoding.UTF8.GetString(dataArray[j]);
                        //判断是http结尾则退出循环
                        if (str.Contains("--" + sSeparateString + "--") && sSeparateString != "")
                        {
                            if (dataArray[j - 1].Length == 2 && dataArray[j - 1][0] == '\r' && dataArray[j - 1][1] == '\n')
                            {
                                if (dataArray[j][3] == 83)
                                {
                                    endIndex = j;
                                    length -= dataArray[j].Length;  //ceshi
                                }
                                else
                                {
                                    endIndex = j - 1;
                                    length += 2;
                                }
                            }
                            else
                            {
                                endIndex = j;
                                if (flag)
                                {
                                    length -= dataArray[j].Length;  //ceshi
                                }
                            }
                            break;
                        }
                    }
                    if (startIndex < 2) return;
                    var bodyData = new byte[stream.Length - length]; //文件数据
                    int dstLength = 0;
                    if (flag)
                    {
                        for (int j = startIndex; j < endIndex + 1; j++)
                        {
                            Array.Copy(dataArray[j], 0, bodyData, dstLength, dataArray[j].Length);
                            dstLength += dataArray[j].Length;
                        }
                    }
                    else
                    {
                        for (int j = startIndex; j < endIndex; j++)
                        {
                            Array.Copy(dataArray[j], 0, bodyData, dstLength, dataArray[j].Length);
                            dstLength += dataArray[j].Length;
                        }
                    }
                    //存储文件
                    File.WriteAllBytes(sFilePath, bodyData);
                    //处理接收的文件
                    bool verifySuccess = false;
                    DealTarBack(sFilePath, out verifySuccess);
                    if (SingletonInfo.GetInstance().m_UsbPwsSupport == "2")
                    {
                        //不支持签名
                        ServerForm.lRevFiles.Add(sFilePath);//完成接收文件后把文件增加到处理列表上去
                    }
                    else
                    {
                        //支持签名
                        if (verifySuccess)
                        {
                            ServerForm.lRevFiles.Add(sFilePath);//完成接收文件后把文件增加到处理列表上去
                        }
                    }
                }
                catch (Exception em)
                {
                    Console.WriteLine("HS422：" + em.Message);
                }
                Console.WriteLine("接收Tar文件成功！");
            }
        }

        /// <summary>
        /// 验签函数 及通用反馈
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="PlatformVerifySignatureresule"></param>
        public void DealTarBack(string filepath, out bool PlatformVerifySignatureresule)
        {
            PlatformVerifySignatureresule = false;//验签是否通过
            EBD ebdb = null;
            if (File.Exists(filepath))
            {
                try
                {
                    #region 先删除预处理解压缩包中的文件
                    foreach (string xmlfiledel in Directory.GetFileSystemEntries(ServerForm.strBeUnTarFolder))
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
                    ServerForm.tar.UnpackTarFiles(filepath, ServerForm.strBeUnTarFolder);//把压缩包解压到专门存放接收到的XML文件的文件夹下
                    string[] xmlfilenames = Directory.GetFiles(ServerForm.strBeUnTarFolder, "*.xml");//从解压XML文件夹下获取解压的XML文件名
                    string sTmpFile = string.Empty;
                    string sAnalysisFileName = "";
                    string sSignFileName = "";
                    if (SingletonInfo.GetInstance().m_UsbPwsSupport == "1")
                    {
                        if (xmlfilenames.Length < 2)//没有签名文件
                            PlatformVerifySignatureresule = false;
                    }

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
                    if (!string.IsNullOrWhiteSpace(sAnalysisFileName))
                    {
                        using (FileStream fsr = new FileStream(sAnalysisFileName, FileMode.Open))
                        {
                            StreamReader sr = new StreamReader(fsr, Encoding.UTF8);
                            string xmlInfo = sr.ReadToEnd();
                            xmlInfo = xmlInfo.Replace("xmlns:xs", "xmlns");
                            sr.Close();
                            xmlInfo = XmlSerialize.ReplaceLowOrderASCIICharacters(xmlInfo);
                            xmlInfo = XmlSerialize.GetLowOrderASCIICharacters(xmlInfo);
                            ebdb = XmlSerialize.DeserializeXML<EBD>(xmlInfo);
                        }
                    }
                    string myEBDType = string.Empty;
                    if (ebdb != null)
                    {
                        myEBDType = ebdb.EBDType;
                    }
                    if (!string.IsNullOrWhiteSpace(sSignFileName) && myEBDType != "ConnectionCheck" && SingletonInfo.GetInstance().m_UsbPwsSupport=="1")
                    {
                        //读取xml中的文件,转换为byte字节
                        byte[] xmlArray = File.ReadAllBytes(sAnalysisFileName);

                        #region 签名处理
                        Console.WriteLine("开始验证签名文件!");
                        using (FileStream SignFs = new FileStream(sSignFileName, FileMode.Open))
                        {
                            StreamReader signsr = new StreamReader(SignFs, Encoding.UTF8);
                            string xmlsign = signsr.ReadToEnd();
                            signsr.Close();
                            responseXML signrp = new responseXML();//签名回复
                            XmlDocument xmlSignDoc = new XmlDocument();
                            try
                            {
                                int nDeviceHandle = (int)ServerForm.mainFrm.phDeviceHandle;
                                xmlsign = XmlSerialize.ReplaceLowOrderASCIICharacters(xmlsign);
                                xmlsign = XmlSerialize.GetLowOrderASCIICharacters(xmlsign);
                                Signature sign = XmlSerialize.DeserializeXML<Signature>(xmlsign);
                                xmlsign = XmlSerialize.ReplaceLowOrderASCIICharacters(xmlsign);
                                xmlsign = XmlSerialize.GetLowOrderASCIICharacters(xmlsign);
                                string PucStr = sign.SignatureValue;
                                byte[] pucsingVi = Encoding.UTF8.GetBytes(sign.SignatureValue);

                                //0是签名通过
                                var result = ServerForm.mainFrm.usb.PlatformVerifySignature(nDeviceHandle, 1, xmlArray, xmlArray.Length, pucsingVi);
                                PlatformVerifySignatureresule = result == 0;
                                Log.Instance.LogWrite(PlatformVerifySignatureresule ? "签名验证成功" : "签名验证失败-" + result);
                            }
                            catch (Exception ex)
                            {
                                Log.Instance.LogWrite("签名文件错误：" + ex.Message);
                            }
                        }
                        Console.WriteLine("结束验证签名文件！");
                        #endregion End
                    }

                    ServerForm.DeleteFolder(ServerForm.strBeSendFileMakeFolder);//删除原有XML发送文件的文件夹下的XML
                    if (!PlatformVerifySignatureresule && myEBDType != "ConnectionCheck" &&SingletonInfo.GetInstance().m_UsbPwsSupport=="1")
                    {
                        //验签失败
                        CurrencyTarBack(ebdb, "4");
                        return;
                    }
                    Console.WriteLine("要解析文件：" + sAnalysisFileName);

                    #region 通用反馈  20181210
                    CurrencyTarBack(ebdb, "1");
                    #endregion

                    if (ebdb != null)
                    {
                        switch (ebdb.EBDType)
                        {
                            case "ConnectionCheck":
                                //要提示系统在线
                                ServerForm.mainFrm.serverFrm.OnlineCheck(true);
                                break;
                        }
                    }
                }
                catch (Exception ep)
                {
                    Log.Instance.LogWrite("验签过程异常："+ ep.Message);
                }
            }
        }


        /// <summary>
        /// 通用反馈  收到tar就回
        /// </summary>
        private void CurrencyTarBack(EBD ebdb,string value)
        {
            try
            {
                ServerForm.DeleteFolder(ServerForm.strBeSendFileMakeFolder);//删除原有XML发送文件的文件夹下的XML
                XmlDocument xmlDoc = new XmlDocument();
                responseXML rp = new responseXML();
                string fName = "10" + SingletonInfo.GetInstance().CurrentResourcecode + BBSHelper.GetSequenceCodes();
                xmlDoc = rp.EBDResponse(ebdb, "EBDResponse", fName, value);
                string xmlSignFileName = "\\EBDB_" + fName + ".xml";

                CreateXML(xmlDoc, ServerForm.strBeSendFileMakeFolder + xmlSignFileName);

                //进行签名
                ServerForm.mainFrm.GenerateSignatureFile(ServerForm.strBeSendFileMakeFolder, fName);
                ServerForm.tar.CreatTar(ServerForm.strBeSendFileMakeFolder, ServerForm.sSendTarPath, fName);//使用新TAR
                string sSendTarName = ServerForm.sSendTarPath + "\\EBDT_" + fName + ".tar";
                FileStream fsSnd = new FileStream(sSendTarName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fsSnd);     //时间戳 
                int datalen = (int)fsSnd.Length + 2;
                int bufferLength = 4096;
                long offset = 0; //开始上传时间 
                writeHeader(datalen.ToString(), "EBDT_" + fName + ".tar");

                byte[] buffer = new byte[4096]; //已上传的字节数 
                int size = br.Read(buffer, 0, bufferLength);
                while (size > 0)
                {
                    outputStream.Write(buffer, 0, size);
                    offset += size;
                    size = br.Read(buffer, 0, bufferLength);
                }
                outputStream.Write(Encoding.UTF8.GetBytes(sEndLine), 0, 2);
                outputStream.Flush();//提交写入的数据                                        
                fsSnd.Close();
            }
            catch (Exception esb)
            {
                Console.WriteLine("401:" + esb.Message);
            }

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

        public void sendResponse(string frdStateName)
        {
            string sStateBeatTarName = ServerForm.sSendTarPath + "\\EBDT_" + frdStateName + ".tar";
            FileStream fsStateSnd = new FileStream(sStateBeatTarName, FileMode.Open, FileAccess.Read);
            BinaryReader brState = new BinaryReader(fsStateSnd);
            int Statedatalen = (int)fsStateSnd.Length + 2;
            int bufferStateLength = 4096;
            long StateOffset = 0; //
            writeHeader(Statedatalen.ToString(), "\\EBDT_" + frdStateName + ".tar");
            byte[] Statebuffer = new byte[4096]; //已上传的字节数 
            int Satesize = brState.Read(Statebuffer, 0, bufferStateLength);
            while (Satesize > 0)
            {
                outputStream.Write(Statebuffer, 0, Satesize);
                StateOffset += Satesize;
                Satesize = brState.Read(Statebuffer, 0, bufferStateLength);
            }
            outputStream.Write(Encoding.UTF8.GetBytes(sEndLine), 0, 2);
            outputStream.Flush();//提交写入的数据                                        
            fsStateSnd.Close();
        }

        public void writeHeader(string strDataLen, string strTarName)//,ref FileStream fsave
        {
            StringBuilder sbHeader = new StringBuilder(200);

            sbHeader.Append("HTTP/1.1 200 OK" + sEndLine);//HTTP/1.1 200 OK
            sbHeader.Append("Content-Disposition:attachment;filename=" + "\"" + strTarName + "\"" + sEndLine);
            sbHeader.Append("Content-Type:application/x-tar" + sEndLine);
            sbHeader.Append("Server:WinHttpClient" + sEndLine);
            sbHeader.Append("Content-Length:" + strDataLen + sEndLine);
            sbHeader.Append("Date:" + DateTime.Now.ToString("r") + sEndLine);
            sbHeader.Append(sEndLine);
            byte[] bTmp = Encoding.UTF8.GetBytes(sbHeader.ToString());
            outputStream.Write(bTmp, 0, bTmp.Length);
        }

        public void writeSuccess(string content_type = "text/html")
        {
            StringBuilder sbSuccess = new StringBuilder(200);
            sbSuccess.Append("HTTP/1.0 200" + sEndLine);
            sbSuccess.Append("Connection: close" + sEndLine);
            sbSuccess.Append(sEndLine);
            sbSuccess.Append("True");
            byte[] bTmp = Encoding.UTF8.GetBytes(sbSuccess.ToString());
            outputStream.Write(bTmp, 0, bTmp.Length);
        }

        public void writeFailure()
        {
            StringBuilder sbHeader = new StringBuilder(200);
            sbHeader.Append("HTTP/1.0 404 File not found" + sEndLine);
            sbHeader.Append("Connection: close" + sEndLine);
            sbHeader.Append(sEndLine);
            byte[] bTmp = Encoding.UTF8.GetBytes(sbHeader.ToString());
            outputStream.Write(bTmp, 0, bTmp.Length);
        }
    }
    /// <summary>
    /// Http服务基类
    /// </summary>
    public abstract class HttpServerBase
    {
        protected int port;
        protected IPAddress ipServer;
        TcpListener listener;
        bool is_active = true;

        public HttpServerBase(IPAddress ipserver, int port)
        {
            this.port = port;
            this.ipServer = ipserver;
        }

        public HttpServerBase(int port)
        {
            this.port = port;
        }

        public void listen()
        {
            try
            {
                if (ipServer == null)
                {
                    listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);//没有具体绑定IP
                }
                else
                {
                    listener = new TcpListener(ipServer, port);//绑定具体IP
                }
                listener.Start();
                #region 
                
                #endregion
                while (is_active)
                {
                    if (listener.Pending())
                    {
                        TcpClient s = listener.AcceptTcpClient();
                        HttpProcessor processor = new HttpProcessor(s, this);
                        Thread thread = new Thread(new ThreadStart(processor.process));
                        thread.Name = "监听线程:" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        thread.IsBackground = true;
                        thread.Start();
                        Thread.Sleep(1);
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.LogWrite(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        public bool StopListen()
        {
            try
            {
                listener.Stop();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public abstract void handleGETRequest(HttpProcessor p);

        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }
    /// <summary>
    /// Http服务的接口实现
    /// </summary>
    public class HttpServer : HttpServerBase
    {
        public HttpServer(int port)
            : base(port)
        {
        }

        public HttpServer(IPAddress ipaddr, int port)
            : base(ipaddr, port)
        {
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            Console.WriteLine("request: {0}", p.http_url);
            p.writeSuccess();
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            Console.WriteLine("POST request: {0}", p.http_url);
            p.writeSuccess();
        }
    }


}
