using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DockingServices
{
    [Serializable]
    public class EBM
    {
        public string EBMVersion; //
        public string EBEID; //

        public string EBMID;

        public RelatedEBM RelatedEBM;//

        public string TestType;  //

        public string StartTime;

        public string EndTime;

        public string ProcessMethod;//

        public MsgBasicInfo MsgBasicInfo;//

        public MsgContent MsgContent;//

        public Dispatch Dispatch;

        public string BrdStateCode;//20181214新增  从数据库读取播放状态
    }

    [Serializable]
    public class RelatedEBM  //
    {
        public string EBMID;
    }

    [Serializable]
    public class MsgBasicInfo
    {
        public string MsgType;
        public string SenderName;
        public string SenderCode;
        public string SendTime;
        public string EventType;
        public string Severity;

        public string StartTime;
        public string EndTime;
    }

    [Serializable]
    public class MsgContent
    {
        public string LanguageCode;

        public string MsgTitle;

        public string MsgDesc;

        public string AreaCode;

        public Auxiliary Auxiliary;
    }

    [Serializable]
    public class Auxiliary
    {
        public string AuxiliaryType;

        public string AuxiliaryDesc;

        public string Size;

        public string Digest;
    }

    [Serializable]
    public class Coverage
    {
       [XmlElement]
       public List<Area> Area;
    }

    [Serializable]
    public class Area
    {
        public string AreaName;
        public string AreaCode;
    }

    [Serializable]
    public class Dispatch
    {
        public string LanguageCode;

        public EBEAS EBEAS;

        public EBEBS EBEBS;
    }

    [Serializable]
    public class EBEAS
    {
        public string EBEID;//应急广播消息适配器ID
    }

    [Serializable]
    public class EBEBS
    {
        public string BrdSysType;

        public string BrdSysInfo;
    }

}
