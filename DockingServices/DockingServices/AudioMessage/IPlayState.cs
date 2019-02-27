using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingServices.AudioMessage
{
    /// <summary>
    /// 播放状态反馈接口
    /// </summary>
    public interface IPlayState
    {
        /// <summary>
        /// 未处理
        /// </summary>
        /// <returns></returns>
        bool Untreated(string path, string BrdStateCode);

        /// <summary>
        /// 未播放状态反馈
        /// </summary>
        /// <returns>1.反馈成功，2 反馈失败</returns>
        bool NotPlay(string TmcId,string path, string BrdStateCode);
        /// <summary>
        /// 播放中状态反馈
        /// </summary>
        /// <returns>1.反馈成功，2 反馈失败</returns>

        bool Playing(string TmcId, string path, string BrdStateCode,string TimingTerminalState);
        /// <summary>
        /// 播放完状态反馈
        /// </summary>
        /// <returns>1.反馈成功，2 反馈失败</returns>
        bool PlayOver(string TmcId, string path, string BrdStateCode,string TimingTerminalState);
    }
}
