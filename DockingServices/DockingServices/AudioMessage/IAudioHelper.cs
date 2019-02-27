using DockingServices.AudioMessage.MQAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingServices.AudioMessage
{
   public interface IAudioHelper
    {
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="auio"></param>
        /// <returns></returns>
        bool AudioPlay(int type,string ParamValue, string TsCmd_ID, string TsCmd_ValueID,string EBMInfoID);
        /// <summary>
        /// 命令组合
        /// </summary>
        /// <param name="PlayContent"></param>
        /// <returns></returns>
        string CombinationInstruction();


        bool CancelPlay();
    }
}
