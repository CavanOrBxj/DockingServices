using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsumptionQueue;
using DockingServices.model.SendModel;
using System.Windows;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DockingServices.SendTar
{
    public  class SendTarOrder
    {
        public static readonly SendTarOrder  sendHelper=new  SendTarOrder();
        public SendTarOrder()
        {
            Queueinterface<PostModel>.SendQueue.ProcessItemFunction += PostSend;
            Queueinterface<PostModel>.SendQueue.ProcessException += WriteLog;
            Queueinterface<PostModel>.SendQueue.QueryQueueExistsEvent += QueryQueue;
        }
         void WriteLog(object ex, EventArgs<Exception> args)
        {
            MessageBox.Show(ex.ToString());
            Log.Instance.LogWrite("发送队列异常:" + ex.ToString());
        }
        public string AddPostQueue(string address, string FileName)
        {
            if (!string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(FileName))
            {
                PostModel postInfo = new PostModel();
                postInfo.Address = address;
                postInfo.FileName = FileName;
                Queueinterface<PostModel>.SendQueue.Enqueue(postInfo);
                ServerForm.SetManager("队列添加成功:" + postInfo.FileName, Color.Green);
                return "1";
            }
            else
            {
                ServerForm.SetManager("队列添加失败:" + FileName, Color.Red);
                return "2";
            }
        }
        #region IsUrl(是否Url地址)
        /// <summary>
        /// 是否Url地址（统一资源定位）
        /// </summary>
        /// <param name="value">url地址</param>
        /// <returns></returns>
        public static bool IsUrl(string value)
        {
          
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            return
                Regex.IsMatch(value,
                @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$"
                );
        }
        #endregion
        private void PostSend(PostModel postModel)
        {
            try
            {
                string result =  HttpSendFile.UploadFilesByPost(postModel.Address, postModel.FileName);
                if (result != "0")
                {
                    ServerForm.SetManager("发送成功:"+ postModel.FileName, Color.Green);
                }
                else
                {
                    ServerForm.SetManager("发送失败"+ postModel.FileName, Color.Red);

                }
            }
            catch(Exception ex)
            {
                ServerForm.SetManager("发送失败"+ postModel.FileName +"失败原因:"+ ex.Message, Color.Red);
                Log.Instance.LogWrite("发送队列异常:" + ex.ToString());
            }
        }

        public static bool QueryQueue(PostModel info, PostModel infoB)
        {
            if (info.FileName == infoB.FileName)
                return true;
            return false;
            //Console.WriteLine(info.ComId + "====" + DelegateQueue<InfoModel>.queue.Count);
        }
    }
}
