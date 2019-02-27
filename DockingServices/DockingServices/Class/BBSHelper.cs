using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingServices.Class
{
    class BBSHelper
    {


        public static string GetSequenceCodes()
        {
            SingletonInfo.GetInstance().SequenceCodes += 1;
            return SingletonInfo.GetInstance().SequenceCodes.ToString().PadLeft(16, '0');
        }


        /// <summary>
        /// 保留小数点后指定位数 
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="savelength">小数点后保留位数</param>
        /// <returns></returns>
        public static string StrDeal(string str, int savelength)
        {
            string[] strs = str.Split('.');
            string part1 = strs[0];
            string part2 = strs[1];
            if (part2.Length> savelength)
            {
                part2= part2.Substring(0, savelength);
            }
            return part1 + "." + part2;
        }
    }
}
