using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingServices.model
{
   public class EDB_EBRBS
    {
        public class EBR
        {
            public string EBRID
            {
                get;
                set;
            }
        }
        public class EBRPS:EBR
        {

        }
        public class EBRST : EBR
        {

        }
        public class EBRAS 
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
            public string StateCode
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
        public class EBD : EBDHEAD
        {
            public EBRPSInfo EBRPSInfo
            {
                get;
                set;
            }

            public EBMStateResponse EBMStateResponse
            {
                get;
                set;
            }
            public EBMBrdLog EBMBrdLog
            {
                get;
                set;
            }
            public EBRASState EBRASState
            {
                get;
                set;
            }
        }
    }
}
