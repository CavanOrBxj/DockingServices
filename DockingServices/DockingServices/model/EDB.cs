using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingServices.model
{
    public class EBDHEAD
    {

        public string EBDVersion
        {
            get;
            set;
        }
        public string EBDID
        {
            get;
            set;
        }
        public string EBDType
        {
            get;
            set;
        }

        public SRC SRC
        {
            get;
            set;
        }
        public DEST DEST
        {
            get;
            set;
        }
        public string EBDTime
        {
            get;
            set;
        }
        public RelatedEBD RelatedEBD
        {
            get;
            set;
        }

    }
    [Serializable]
    public class EBDResponse
    {
        public int ResultCode
        {
            get;
            set;
        }
        public string ResultDesc
        {
            get;
            set;
        }
    }
    //心跳EBD
    public class EBD:EBDHEAD
    {

        public ConnectionCheck ConnectionCheck
        {
            get;
            set;
        }
        public List<EBRDT> EBRDTState
        {
            get;
            set;
        }

        public List<EBRPS> EBRPSState
        {
            get;
            set;
        }

        public EBRPSInfo EBRPSInfo
        {
            get;
            set;
        }
        public EBRPSInfo EBRDTInfo
        {
            get;
            set; 
        }
        public EBDResponse EBDResponse
        {
            get;set;
        }

        public EBRASInfo EBRASInfo
        {
            get; set;
        }


    }
    public class EBMStateResponse
    {
       public string RptTime
        {
            get;
            set;
        }
        public EBM EBM
        {
            get;
            set;
        }
        public string BrdStateCode
        {
            get;
            set;
        }
        public string BrdStateDesc
        {
            get;
            set;
        }
        public Coverage Coverage
        {
            get;
            set;
        }
        public ResBrdInfo ResBrdInfo
        {
            get;
            set;

        }
    }
    public class ResBrdInfo
    {
       public List<ResBrdItems> ResBrdItem
        {
            get;
            set;
        }
    }
    public class ResBrdItems
    {
        public EDB_EBRBS.EBRPS EBRPS
        {
            get;
            set;
        }
        public EDB_EBRBS.EBRST EBRST
        {
            get;
            set;
        }
        public EDB_EBRBS.EBRAS EBRAS
            {
            get;
            set;
            }
        public EBRBS EBRBS
        {
            get;
            set;
        }
     
    }
    public class EBRASState
    {
        public model.EDB_EBRBS.EBRAS EBRAS
        {
            get;
            set;
        }
        
    }

    public class EBRBS
    {
        public string RptTime
        {
            get;
            set;
        }
        public string BrdSysInfo
        {
            get;
            set;
        }
        public string StartTime
        {
            get;
            set;
        }
        public string EndTime
        {
            get;
            set;
        }
        public string FileURL
        {
            get;
            set;
        }
        public string BrdStateCode
        {
            get;
            set;
        }
        public string BrdStateDesc
        {
            get;
            set;
        }

    } 
    
    //播发记录
    public class EBMBrdLog
    {
        public Params Params
        {
            get;
            set;
        }
        public List<EBMBrdItems> EBMBrdItem
        {
            get;
            set;
        }

    }
    public class EBMBrdItems
    {
        public EBM EBM
        {
            get;
            set;
        }

        public TunersPlayRecording TunersPlayRecording
        {
            get;
            set;
        }


        public string UnitInfo
        {
            get;
            set;
        }

        public string BrdStateCode
        {
            get;
            set;
        }
        public string BrdStateDesc
        {
            get;
            set;
        }
        public Coverage Coverage
        {
            get;
            set;
        }
        public ResBrdInfo ResBrdInfo
        {
            get;
            set;
        }



    }

    /// <summary>
    /// 图南播放记录信息
    /// </summary>
    public class TunersPlayRecording
    {
        /// <summary>
        /// 应急广播消息ID
        /// </summary>
        public string EmergencyBroadcastID { get; set; }

        /// <summary>
        /// 播发类型  文件/实时音频/电插播/话筒插播  等等
        /// </summary>
        public string BroadcastType { get; set; }

        /// <summary>
        /// 播发开始时间
        /// </summary>
        public string BroadcastStartTime { get; set; }

        /// <summary>
        /// 播发结束时间
        /// </summary>
        public string BroadcastEndTime { get; set; }

        /// <summary>
        /// 覆盖范围
        /// </summary>
        public string OverlayArea { get; set; }
    }


    public class Coverage
    {
       public string CoverageRate
        {
            get;
            set;
        }
        public string AreaCode
        {
            get;
            set;
        }
        public string ResBrdStat
        {
            get;
            set;
        }
    }
    public class EBRPSInfo
    {
        public Params Params
        {
            get;
            set;
        }
        public List<EBRPSS> EBRPS
        {
            get;
            set;
        }
        public List<Information> EBRDT
        {
            get;
            set;
        }
    }


    public class EBRASInfo
    {
        public Params Params
        {
            get;
            set;
        }
        public List<EBRAS> EBRAS
        {
            get;
            set;
        }
    }
    public class EBRAS
    {
        /// <summary>
        /// 数据操作生成时间
        /// </summary>
        public string RptTime
        {
            get;
            set;
        }
        /// <summary>
        /// 数据操作类型
        /// </summary>
        public string RptType
        {
            get;
            set;
        }
        /// <summary>
        /// 关联应急广播平台信息
        /// </summary>
        public RelatedEBRPS RelatedEBRPS
        {
            get;
            set;
        }

        /// <summary>
        /// 应急平台编码
        /// </summary>
        public string EBRID
        {
            get;
            set;
        }
        /// <summary>
        /// 应急平台名称
        /// </summary>
        public string EBRName
        {
            get;
            set;
        }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude
        {
            get;
            set;
        }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude
        {
            get;
            set;
        }
        //网络地址
        public string URL
        {
            get;
            set;
        }

    }


    public class Params
    {
        public string RptStartTime
        {
            get;
            set;
        }
        public string RptEndTime
        {
            get;
            set;
        }
        public string RptType
        {
            get;
            set;
        }
  
    }
    public class EBRPSS
    {
        /// <summary>
        /// 数据操作生成时间
        /// </summary>
        public string RptTime
        {
            get;
            set;
        }
        /// <summary>
        /// 数据操作类型
        /// </summary>
        public string RptType
        {
            get;
            set;
        }
        /// <summary>
        /// 关联应急广播平台信息
        /// </summary>
        public RelatedEBRPS RelatedEBRPS
        {
            get;
            set;
        }

        /// <summary>
        /// 应急平台编码
        /// </summary>
        public string EBRID
        {
            get;
            set;
        }
        /// <summary>
        /// 应急平台名称
        /// </summary>
        public string EBRName
        {
            get;
            set;
        }
        /// <summary>
        /// 应急平台地址
        /// </summary>
        public string Address
        {
            get;
            set;
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact
        {
            get;
            set;

        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string PhoneNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude
        {
            get;
            set;
        }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude
        {
            get;
            set;
        }
        //网络地址
        public string URL
        {
            get;
            set;
        }

    }

    //关联应急平台信息
    public class RelatedEBRPS
    {
        /// <summary>
        /// 关联应急广播信息编码
        /// </summary>
        public string EBRID
        {
            get;
            set;
        }
    }
    public class EBRDTState
    {
        public EBRDT EBRDT
        {
            get;
            set;
        }
    }
    public class EBRDT
    {
        public string RptTime
        {
            get;
           set;
        }
        public string EBRID
        {
            get;
            set;
        }
        public int StateCode
        {
            get;
            set;
        }
        public string StateDesc
        {
            get;
            set;
        }

        public string SRV_RMT_SWITCH
        {
            get;
            set;
        }
    }

    public class EBRPS
    {
        public string RptTime
        {
            get;
            set;
        }
        public string EBRID
        {
            get;
            set;
        }
        public int StateCode
        {
            get;
            set;
        }
        public string StateDesc
        {
            get;
            set;
        }
    }


    public class terminal
    {
        public string RptTime
        {
            get;
            set;
        }
        public string RptType
        {
            get;
            set;
        }

        public RelatedEBRPS RelatedEBRPS
        {
            get;
            set;
        }
        public string EBRID
        {
            get;
            set;
        }
        public string EBRName
        {
            get;
            set;
        }


    }
    //平台设备及终端信息
    public class Information : terminal
    {
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude
        {
            get;
            set;
        }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude
        {
            get;
            set;
        }

    }
    public class ConnectionCheck
    {
        public string RptTime
        {
            get;
            set;
        }
    }

    public class EBMStateRequest
    {
        public EBM EBM
        {
            get;
            set;
        }
    }
    public class EBM
    {
        public string EBMID
        {
            get;
            set;
        }
        public MsgContent MsgContent
        {
            get;
            set;
        }
    }

   
    public class MsgContent
    {
         public string LanguageCode
         {
            get;
            set;
         }
         public string  MsgTitle
        {
            get;
            set;

        }
         public string MsgDesc
        {
            get;
            set;
        }
         public string AreaCode
        {
            get;
            set;
        }
    }
    public class RelatedEBD
    {
        public string EBDID
        {
            get;
            set;
        }
    }
    public class SRC
    {
        public string  EBRID
        {
            get;
            set;
        }
        public string URL
        {
            get;
            set;
        }

    }
    public class DEST
    {
        public string EBRID
        {
            get;
            set;
        }
    }
}
