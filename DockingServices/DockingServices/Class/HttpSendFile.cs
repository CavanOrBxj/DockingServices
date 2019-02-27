using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DockingServices
{
    public class HttpSendFile
    {
        public Thread HttpSendFieTh = null;
        SendFileSon FSon = new SendFileSon();

        public class SendFileSon
        {
            public string AddressUrl { get; set; }
            public string FileName { get; set; }
        }

        /// <summary>   
        /// 将本地文件上传到指定的服务器(HttpWebRequest方法)   
        /// </summary>   
        /// <param name="address">文件上传到的服务器</param>   
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param>   
        /// <param name="saveName">文件上传后的名称</param>    
        /// <returns>成功返回1，失败返回0</returns>   
        public static string UploadFilesByPost(string address, string fileNamePath)
        {
            try
            {
                int u = ServicePointManager.DefaultConnectionLimit;
                ServicePointManager.DefaultConnectionLimit = 200;

                string returnValue = "0";     // 要上传的文件     
                WebResponse webRespon = null;

                FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);     //时间戳

                string sguidSplit = Guid.NewGuid().ToString();
                string filename = fileNamePath.Substring(fileNamePath.LastIndexOf("\\") + 1);

                StringBuilder sb = new StringBuilder(300);

                string strPostHeader = sb.ToString();
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));

                httpReq.ServicePoint.Expect100Continue = false;
                httpReq.Method = "POST";     //对发送的数据不使用缓存
                httpReq.AllowWriteStreamBuffering = false;     //设置获得响应的超时时间（300秒）
                httpReq.Timeout = 60000;
                httpReq.ContentType = "multipart/form-data; boundary=" + sguidSplit;//"text/xml";
                httpReq.Accept = "text/plain, */*";
                httpReq.UserAgent = "WinHttpClient";

                httpReq.Headers["Accept-Language"] = "zh-cn";

                sb.Append("--" + sguidSplit + "\r\n");
                sb.Append("Content-Disposition: form-data; name=\"file\"; filename=\"" + filename + "\"\r\n");
                sb.Append("Content-Type: application/octet-stream;Charset=UTF-8\r\n");
                sb.Append("\r\n");

                byte[] boundaryBytes = Encoding.ASCII.GetBytes(sb.ToString());     //请求头部信息  
                byte[] bEndBytes = Encoding.ASCII.GetBytes("\r\n--" + sguidSplit + "--\r\n");
                long length = fs.Length + boundaryBytes.Length + bEndBytes.Length;
                long fileLength = fs.Length;
                httpReq.ContentLength = length;

                try
                {
                    int bufferLength = 4096;//每次上传4k  
                    byte[] buffer = new byte[bufferLength]; //已上传的字节数   
                    long offset = 0;         //开始上传时间   
                    DateTime startTime = DateTime.Now;

                    int size = r.Read(buffer, 0, bufferLength);
                    Stream postStream = httpReq.GetRequestStream();         //发送请求头部消息   
                    postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    while (size > 0)
                    {
                        postStream.Write(buffer, 0, size);
                        offset += size;
                        TimeSpan span = DateTime.Now - startTime;
                        double second = span.TotalSeconds;
                        Application.DoEvents();
                        size = r.Read(buffer, 0, bufferLength);
                    }
                    //添加尾部的时间戳 
                    postStream.Write(bEndBytes, 0, bEndBytes.Length);
                    postStream.Close();
                    //获取服务器端的响应   
                    webRespon = httpReq.GetResponse();
                    Stream s = webRespon.GetResponseStream();
                    //读取服务器端返回的消息  
                    StreamReader sr = new StreamReader(s);
                    String sReturnString = sr.ReadLine();
                    s.Close();
                    sr.Close();
                    returnValue = "1";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    returnValue = "0";
                }
                finally
                {
                    fs.Close();
                    r.Close();
                    if (httpReq != null)
                    {
                        httpReq.Abort();
                    }
                    if (webRespon != null)
                    {
                        webRespon.Close();
                    }
                    GC.Collect();
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Thread.Sleep(500);
            }
            return "0";
        }

        public static string UploadFilesByPostNoSplit(string address, string fileNamePath)
        {
            string returnValue = "0";     // 要上传的文件   
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);     //时间戳   

            string sguidSplit = Guid.NewGuid().ToString();
            string filename = fileNamePath.Substring(fileNamePath.LastIndexOf("\\") + 1);
            StringBuilder sb = new StringBuilder(300);
            string strPostHeader = sb.ToString();
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
            httpReq.Method = "POST";     //对发送的数据不使用缓存   
            httpReq.AllowWriteStreamBuffering = false;     //设置获得响应的超时时间（300秒）   
            httpReq.Timeout = 30000;
            httpReq.ContentType = "multipart/form-data; boundary=" + sguidSplit;//"text/xml";// 
            httpReq.Accept = "text/plain, */*";
            httpReq.UserAgent = "WinHttpClient";

            httpReq.Headers["Accept-Language"] = "zh-cn";

            sb.Append("--" + sguidSplit + "\r\n");
            sb.Append("Content-Disposition: form-data;filename=\"" + filename + "\"\r\n");
            sb.Append("Content-Type: application/x-tar\r\n");
            sb.Append("\r\n");

            byte[] boundaryBytes = Encoding.ASCII.GetBytes(sb.ToString());     //请求头部信息  
            byte[] bEndBytes = Encoding.ASCII.GetBytes("\r\n--" + sguidSplit + "--\r\n");
            long length = fs.Length + boundaryBytes.Length + bEndBytes.Length;
            long fileLength = fs.Length;
            httpReq.ContentLength = length;
            try
            {
                int bufferLength = 4096;//每次上传4k  
                byte[] buffer = new byte[bufferLength]; //已上传的字节数   
                long offset = 0;         //开始上传时间   
                DateTime startTime = DateTime.Now;

                int size = r.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();         //发送请求头部消息   
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    offset += size;
                    TimeSpan span = DateTime.Now - startTime;
                    double second = span.TotalSeconds;
                    Application.DoEvents();
                    size = r.Read(buffer, 0, bufferLength);
                }
                //添加尾部的时间戳 
                postStream.Write(bEndBytes, 0, bEndBytes.Length);
                postStream.Close();         //获取服务器端的响应   
                WebResponse webRespon = httpReq.GetResponse();
                Stream s = webRespon.GetResponseStream();
                //读取服务器端返回的消息  
                StreamReader sr = new StreamReader(s);
                String sReturnString = sr.ReadLine();
                s.Close();
                sr.Close();
                returnValue = "1";
                Console.WriteLine(sReturnString);

            }
            catch
            {
                returnValue = "0";
            }
            finally
            {
                fs.Close();
                r.Close();
            }
            return returnValue;
        }
    }
}
