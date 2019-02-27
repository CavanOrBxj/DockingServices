using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingServices
{

    public class timestrategies
    {
        /// <summary>
        /// 策略ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 策略开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 策略结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 事件类型
        /// </summary>
        public string EvenType { get; set; }
    }


    public class strategytactics
    {
        public List<timestrategies> TimeList; 
    }


    public class PlayElements
    {

        public EBD EBDITEM { get; set; }
        public string sAnalysisFileName { get; set; }
        public string xmlFilePath { get; set; }


        public string targetPath { get; set; }

        /// <summary>
        /// 数据表EBMInfo的ID
        /// </summary>
        public string EBMInfoID { get; set; }
    }

    public class OrganizationInfo
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        public string ORG_DETAIL { get; set; }

        /// <summary>
        /// 区域码
        /// </summary>
        public string GB_CODE { get; set; }
    }

    /// <summary>
    /// 增量终端状态
    /// </summary>
    public class IncrementalEBRDTState
    {
        /// <summary>
        /// 23位资源码
        /// </summary>
        public string SRV_LOGICAL_CODE_GB { get; set; }
        /// <summary>
        /// 终端物理码
        /// </summary>
       public string  SRV_PHYSICAL_CODE { get; set; }
        /// <summary>
        ///在线/离线状态
        /// </summary>
        public string SRV_RMT_STATUS { get; set; }
        /// <summary>
        /// 终端播放状态   开机、关机、播放中
        /// </summary>
        public string powersupplystatus { get; set; }

        //重写Equals方法

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if ((obj.GetType().Equals(this.GetType())) == false)
            {
                return false;
            }
            IncrementalEBRDTState temp = null;
            temp = (IncrementalEBRDTState)obj;

            return this.SRV_LOGICAL_CODE_GB.Equals(temp.SRV_LOGICAL_CODE_GB) && this.SRV_PHYSICAL_CODE.Equals(temp.SRV_PHYSICAL_CODE)&& this.SRV_RMT_STATUS.Equals(temp.SRV_RMT_STATUS) && this.powersupplystatus.Equals(temp.powersupplystatus);

        }

        //重写GetHashCode方法（重写Equals方法必须重写GetHashCode方法，否则发生警告

        public override int GetHashCode()
        {
            return this.SRV_LOGICAL_CODE_GB.GetHashCode() + this.SRV_PHYSICAL_CODE.GetHashCode() + this.SRV_RMT_STATUS.GetHashCode() + this.powersupplystatus.GetHashCode();
        }

    }
}
