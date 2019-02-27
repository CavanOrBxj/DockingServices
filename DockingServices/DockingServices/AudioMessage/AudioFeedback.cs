using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingServices.AudioMessage
{
  public   class AudioFeedback<T>
    {
        /// <summary>
        /// 播放前
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public delegate bool NotPlayFeedbackDelegate(T t);

        /// <summary>
        /// 播放中
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public delegate bool PlayingFeedbackDelegate(T t);


        //播放完成
        public delegate bool PlayOverFeedbackDelegate(T t);
   
    }
}
