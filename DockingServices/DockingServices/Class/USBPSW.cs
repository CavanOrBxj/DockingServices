using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DockingServices
{
    public class USBE
    {
        [DllImport("libTassYJGBCmd_SJJ1313.dll", EntryPoint = "OpenDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int OpenDevice(ref IntPtr phDeviceHandle);

        [DllImport("libTassYJGBCmd_SJJ1313.dll", EntryPoint = "CloseDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int CloseDevice(ref int phDeviceHandle);

        //导入证书
        [DllImport("libTassYJGBCmd_SJJ1313.dll", EntryPoint = "ImportTrustedCert", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int ImportTrustedCert(ref int phDeviceHandle, StringBuilder strcertPath);

        //使用设备私钥计算数据签名
        [DllImport("libTassYJGBCmd_SJJ1313.dll", EntryPoint = "GenerateSignatureWithDevicePrivateKey", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int GenerateSignatureWithDevicePrivateKey(ref int phDeviceHandle, int nDataType, byte[] inputData, int nDataLength, byte[] pucCounter, byte[] pucSignCerSn, byte[] pucSignature);

        //使用设备私钥计算数据签名(字符串模式)
        [DllImport("libTassYJGBCmd_SJJ1313.dll", EntryPoint = "GenerateSignatureWithDevicePrivateKey_String", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int GenerateSignatureWithDevicePrivateKey_string(ref int phDeviceHandle, int nDataType, string pcData, byte[] pcResult);

        //使用证书验证数据签名
        [DllImport("libTassYJGBCmd_SJJ1313.dll", EntryPoint = "VerifySignatureWithTrustedCert", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int VerifySignatureWithTrustedCert(ref int phDeviceHandle, int nDataType, byte[] pucData, int nDataLength, byte[] pucCounter, byte[] pucSignCerSn, byte[] pucSignature);

        //使用证书验证数据签名（字符串）
        [DllImport("libTassYJGBCmd_SJJ1313.dll", EntryPoint = "VerifySignatureWithTrustedCert_String", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int VerifySignatureWithTrustedCert_String(ref int phDeviceHandle, int nDataType, byte[] pucData);

        //计算数据摘要
        [DllImport("libTassYJGBCmd_SJJ1313.dll", EntryPoint = "CalcHash", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int CalcHash(ref int phDeviceHandle, int nHashAlg, byte[] pucData, int nDataLength, byte[] pucHash, ref int pnHashLength);

        //2017-8-2密码器接口
        //平台签名
        //int Platform_CalculateSignature(void *hDeviceHandle,int nDataType,unsigned char *pucData,int nDataLength,char *pcSignature,int *pnSignatureLength);
        [DllImport("libTassYJGBCmd_SJJ1313.dll", EntryPoint = "Platform_CalculateSignature", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int Platform_CalculateSignature(ref int phDeviceHandle, int nDataType, byte[] inputData, int nDataLength, byte[] pcSignature, ref int pnSignatureLength);

        //2017-8-2密码器接口
        //平台签名验证
        [DllImport("libTassYJGBCmd_SJJ1313.dll", EntryPoint = "Platform_VerifySignature", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        //int Platform_VerifySignature(void *hDeviceHandle,int nDataType,unsigned char *pucData,int nDataLength,char *szSignature);
        public static extern int Platform_VerifySignature(ref int hDeviceHandle, int nDataType, byte[] pucData, int nDataLength, byte[] szSignature);

        public int USB_OpenDevice(ref IntPtr phDeviceHandle)
        {
            int nReturn = OpenDevice(ref phDeviceHandle);
            return nReturn;
        }

        public int ImportTrustedCert(ref int phDeviceHandle)
        {
            StringBuilder strSrc = new StringBuilder("C:\\Windows\\windows_x32\\data");
            int nReturn = ImportTrustedCert(ref phDeviceHandle, strSrc);
            return nReturn;
        }

        public int USB_CloseDevice(ref int phDeviceHandle)
        {
            int nReturn = CloseDevice(ref phDeviceHandle);
            return nReturn;
        }

        public string GenerateSignatureWithDevicePrivateKey(ref int phDeviceHandle, byte[] strpcData, int size, ref string strSignture, ref string strpucCounter, ref string strpucSignCerSn)
        {
            //签名
            byte[] Signture = new byte[64];
            byte[] pucCounter = new byte[4];
            byte[] pucSignCerSn = new byte[6];

            int nResult = GenerateSignatureWithDevicePrivateKey(ref phDeviceHandle, 1, strpcData, size, pucCounter, pucSignCerSn, Signture);
            Console.WriteLine(nResult);
            strSignture = Convert.ToBase64String(Signture);
            strpucCounter = Convert.ToBase64String(pucCounter);
            strpucSignCerSn = Convert.ToBase64String(pucSignCerSn);

            byte[] a = new byte[Signture.Length + pucCounter.Length + pucSignCerSn.Length];
            Array.Copy(pucCounter, a, pucCounter.Length);
            Array.Copy(pucSignCerSn, 0, a, pucCounter.Length, pucSignCerSn.Length);
            Array.Copy(Signture, 0, a, pucCounter.Length + pucSignCerSn.Length, Signture.Length);
            Console.WriteLine(a.Length);
            string Fp = Convert.ToBase64String(a);
            return Fp;
        }


        //---------------------------2017-8-21接口变更Platform_CalculateSignature-----------------------------------------
        public string Platform_CalculateSingature_String(int PhDeviceHandle, int DataType, byte[] strpcData, int size, ref string strSingBody)
        {
            string CalculateSignaturestr = string.Empty;
            //int Platform_CalculateSignature(void *hDeviceHandle,int nDataType,unsigned char *pucData,int nDataLength,char *pcSignature,int *pnSignatureLength);
            try
            {
                //StringBuilder pucCounter = new StringBuilder(128);
                byte[] pucCounter = new byte[100];
                int pucSignCerSn = 100;
                int nResule = Platform_CalculateSignature(ref PhDeviceHandle, 1, strpcData, size, pucCounter, ref pucSignCerSn);
                string str = Convert.ToBase64String(pucCounter);
                CalculateSignaturestr = Encoding.Default.GetString(pucCounter);
                //Console.WriteLine(str);
                //Console.WriteLine(pucCounter);
                //Console.WriteLine(CalculateSignaturestr);
                //Console.WriteLine(nResule.ToString("x8"));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return CalculateSignaturestr;
        }

        //---------------------------2017-8-21接口变更Platform_VerifySignature-----------------------------------------
        public int PlatformVerifySignature(int phDeviceHandle, int dataType, byte[] xmlData, int xmlDataLength, byte[] strSingBody)
        {
            int Result = -1;
            try
            {
                Result = Platform_VerifySignature(ref phDeviceHandle, 1, xmlData, xmlDataLength, strSingBody);
                Console.WriteLine(Result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Result;
        }

        public int GenerateSignatureWithDevicePrivateKey_String(ref int phDeviceHandle, string strpcData, ref string strSignture)
        {
            //签名
            byte[] Signture = new byte[200];
            int nResult = GenerateSignatureWithDevicePrivateKey_string(ref phDeviceHandle, 1, strpcData, Signture);
            strSignture = Encoding.Default.GetString(Signture);
            return nResult;
        }

        //验签ref
        public int VerifySignatureWithTrustedCert(ref int phDeviceHandle, byte[] strpcData, int nDataLength, byte[] pucCounter, byte[] pucSignCerSn, byte[] pucSignature)
        {
            int nResult = VerifySignatureWithTrustedCert(ref phDeviceHandle, 1, strpcData, nDataLength, pucCounter, pucSignCerSn, pucSignature);
            return nResult;
        }
    }
}
