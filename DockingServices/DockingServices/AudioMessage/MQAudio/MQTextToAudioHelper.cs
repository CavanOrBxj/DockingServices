using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DockingServices.AudioMessage;
using System.Data;
using System.Threading;
using System.IO;

namespace DockingServices.AudioMessage.MQAudio
{
    public class MQTextToAudioHelper:AudioHelper
    {
        public MQTextToAudioHelper(AudioModel audio)
        {
            this.AudioModel = audio;

            this.PlayStateInterface = new AudioPlayState();
            
            if (!AudioModel.TextState)
            {
                AudioModel.PlayingTime = DateTime.Now.AddMinutes(1);
                AudioModel.PlayEndTime = DateTime.Now.AddMinutes(5);
            }
        }

        /// <summary>
        /// 播放准备
        /// </summary>
        /// <returns></returns>
        public AudioModel PlayReady()
        {
            try
            {
                string MQInstruction = GetAudioContent(AudioModel.PlayingContent);
                base.PlayReady(2,MQInstruction,"1");//测试值  20181214
            }
            catch (Exception ex)
            {
                return AudioModel;
            }
            return null;
        }




        /// <summary>
        /// 取消播放
        /// </summary>
        public override bool CancelPlay()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return true;
        }

        
        /// <summary>
        /// 组合指令
        /// </summary>
        /// <param name="PlayContent"></param>
        /// <returns></returns>
        public override string CombinationInstruction()
        {

            string str = "";
            for (int i = 0; i < AudioModel.PlayCont; i++)
            {
                str += AudioModel.PlayingContent + "。";
            }

            string strPID = str + "~1";
            return str;
        }
 
        #region 私有

        #region SQL 相关


       
        #endregion
        /// <summary>
        /// 获取文本播放内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetAudioContent(string content)
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            if (AudioModel.PlayCont > 0)
            {
                for (int i = 0; i < AudioModel.PlayCont; i++)
                {
                    builder.Append(content + "。");
                }
            }
            builder.Append("~1");
            return builder.ToString();
        }
        #endregion
    }
}
