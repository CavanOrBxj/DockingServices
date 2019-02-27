using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingServices.AudioMessage.MQAudio
{
   public class AudioModel
    {
        //开始时间
        public DateTime PlayingTime { get; set; }
        /// <summary>
        /// 播放结束时间
        /// </summary>
        public DateTime PlayEndTime { get; set; }
        //消息类型
        public string ManagerType
        {
            get; set;

        }
        //推流或本地
        public string AuxiliaryType
        {
            get; set;
        }
        /// <summary>
        /// 播放指令
        /// </summary>
        public string PlayingInstruction { get; set; }
        /// <summary>
        /// XML名称
        /// </summary>
        public string XMLfilename { get; set; }
        /// <summary>
        /// xml路径
        /// </summary>
        public string XmlFilaPath { get; set; }
        /// <summary>
        /// 播放内容
        /// </summary>
        public string PlayingContent { get; set; }
        /// <summary>
        /// 播放次数
        /// </summary>
        public int PlayCont { get; set; }
        /// <summary>
        /// 播放区域
        /// </summary>
        public string[] PlayArea { get; set; }
        /// <summary>
        /// 测试状态开关 
        /// true 开 false 关
        /// </summary>
        public bool TextState { get; set; }

        public string AeraCodeReal { get; set; }

        public string MsgTitleNew { get; set; }

        public string EBMID { get; set; }
        public string MsgDesc { get; set; }
    }
}
