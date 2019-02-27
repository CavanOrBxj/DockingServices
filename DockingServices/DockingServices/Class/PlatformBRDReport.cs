using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingServices
{
    [Serializable]
    public class PlatformBRDReport
    {
        public string RPTStartTime;

        public string RPTEndTime;

        public PlatformBRD PlatformBRD;

    }

    public class PlatformBRD
    {
        public string PlatformBRDID;

        public string SourceType;

        public string SourceID;

        public string MsgID;

        public string Sender;

        public string UnitId;

        public string UnitName;

        public string PersonID;

        public string PersonName;

        public string BRDStartTime;

        public string BRDEndTime;

        public string AudioFileURL;

    }
}
