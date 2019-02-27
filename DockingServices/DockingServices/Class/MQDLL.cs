using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DockingServices
{
    public class MQDLL
    {
        //ACTIVEMQWRAP_API int   StartActiveMQ(const char * szURI,const char * szProducer,const char *szConsumer);
        //ACTIVEMQWRAP_API bool  StopActiveMQ();
        //ACTIVEMQWRAP_API int   GetMessageData(char * szMessage,int nSize);
        //ACTIVEMQWRAP_API int   SendMessageMQ(const char *szMessage);
        //ACTIVEMQWRAP_API int   SetMessageCallback(CallBack pcallback,void *pUserData);
        [DllImport(@"AtiveMQwrap.dll", EntryPoint = "StartActiveMQ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int StartActiveMQ(string szURI, string szProducer, string szConsumer);

        [DllImport(@"AtiveMQwrap.dll", EntryPoint = "StopActiveMQ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int StopActiveMQ();

        [DllImport(@"AtiveMQwrap.dll", EntryPoint = "SendMessageMQ", CallingConvention = CallingConvention.Cdecl,CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        public static extern int SendMessageMQ([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)]string szMessage);//CharSet=CharSet.Ansi(szMessage)

        [DllImport(@"AtiveMQwrap.dll", EntryPoint = "SetMessageCallback", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int SetMessageCallback(CallBackMessageDelegete callback, object o);

        public static unsafe bool CallBackMessage(void* lpdata, int nsize, object o)
        {
            try
            {
                string sMessage = "";
                string sFileName = "";
                string sDuration = "";

                byte[] bMessage = new byte[nsize];
                
                using (UnmanagedMemoryStream ms = new UnmanagedMemoryStream((byte*)lpdata, nsize))
                {
                    ms.Read(bMessage, 0, bMessage.Length);
                }

                for (int i = 0; i < bMessage.Length; i++)
                {
                    sMessage += Convert.ToString(bMessage[i]);
                }
                sFileName = sMessage.Split('|')[0];
                sDuration = sMessage.Split('|')[1];
                Console.WriteLine(sFileName + "   >>" + sDuration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;

        }

        public unsafe delegate bool CallBackMessageDelegete(void* lpdata, int nsize, object o);

        public CallBackMessageDelegete callback;

        public unsafe void SetCallback(object o)
        {
            callback = new CallBackMessageDelegete(CallBackMessage);
            SetMessageCallback(callback, o);
        }
    }
}
