#pragma once

#define CMD_DATA     0
#define MSG_DATA     1

#define HASHALG_SM3	         0x00000001	// SM3杂凑算法 SM3-256
#define HASHALG_SHA1         0x00000002	// SHA_1杂凑算法
#define HASHALG_SHA256       0x00000004	// SHA_256杂凑算法
#define HASHALG_MD5	         0x00000008	// MD5杂凑算法
#define HASHALG_SHA224	     0x00000010	// SHA_224杂凑算法
#define HASHALG_SHA384	     0x00000020	// SHA_384杂凑算法
#define HASHALG_SHA512	     0x00000040	// SHA_512杂凑算法

// 函数接口
#ifdef __cplusplus
extern "C" {
#endif
    /***************************************************************************
    * Subroutine: SetCfgPathName
    * Function:   设置SDF的配置文件路径和文件名
    * Input:
    *   @cfgPathFileName 文件路径和路径名(例如：c:\tacipher.ini)
    *
    * Return:       0 for success, other is error
    * Description:  1.在加载完毕动态库后就调用，否则不生效。
    *
    * Date:         2016.12.15
    *
    * ModifyRecord:
    * *************************************************************************/
    int SetCfgPathName(char *cfgPathFileName);

    /**
    * @brief   打开密码设备
    *
    * @param   phDeviceHandle  [out] 返回的设备句柄
    *
    * @return  0,成功;其他表示失败
    */
    int  OpenDevice(void **phDeviceHandle);
    /**
    * @brief   关闭密码设备，释放相关资源
    *
    * @param   hDeviceHandle   [in] 待关闭的密码设备句柄
    *
    * @return  0,成功;其他表示失败
    */
    int CloseDevice(void *hDeviceHandle);
    /**
    * @brief   导入根证书
    *
    * @param   hDeviceHandle    [in]    已打开的密码设备句柄
    * @param   szCertPath       [in]    证书在本机的存储路径
    *
    * @return  0,成功;其他表示失败
    */
    int ImportRootCert(
        void *hDeviceHandle,
        char *szCertPath);
    /**
    * @brief   导入自身证书
    *
    * @param   hDeviceHandle    [in]    已打开的密码设备句柄
    * @param   szCertPath       [in]    证书在本机的存储路径
    *
    * @return  0,成功;其他表示失败
    */
    int ImportSelfCert(
        void *hDeviceHandle,
        char *szCertPath);
    /**
    * @brief   导入可信证书
    *
    * @param   hDeviceHandle    [in]    已打开的密码设备句柄
    * @param   szCertPath       [in]    证书在本机的存储路径
    *
    * @return  0,成功;其他表示失败
    */
    int ImportTrustedCert(
        void *hDeviceHandle,
        char *szCertPath);
    /**
    * @brief   删除可信证书
    *
    * @param   hDeviceHandle    [in]    已打开的密码设备句柄
    * @param   pucCertSn        [in]    证书序列号
    *
    * @return  0,成功;其他表示失败
    */
    int DeleteTrustedCertWithSn(
        void *hDeviceHandle,
        unsigned char *pucCertSn);
    int GetAllCert(
        void *hDeviceHandle,
        unsigned char *pucCertSet,
        int *nCertCount,
        unsigned char *pucCounter);
    /**
    * @brief    使用设备私钥计算数据签名
    *           使用设备密钥对指令或消息进行签名
    * @param    hDeviceHandle       [in]    已打开的密码设备句柄
    * @param    nDataType           [in]    待签名的数据类型
    *                                       0(CMD_DATA):指令数据; 1(CMD_DATA):消息数据
    * @param    pucData             [in]    待计算签名数据
    * @param    nDataLength         [in]    待计算签名数据长度
    * @param    pucCounter          [out]   签名计数器,4字节
    * @param    pucSignCerSn        [out]   签名证书编号,6字节
    * @param    pucSignature         [out]   签名结果,64字节
    *
    * @return  0,成功;其他表示失败
    */
    int GenerateSignatureWithDevicePrivateKey(
        void *hDeviceHandle,
        int nDataType,
        unsigned char *pucData,
        int nDataLength,
        unsigned char *pucCounter,
        unsigned char *pucSignCerSn,
        unsigned char *pucSignature);
    /**
    * @brief    使用证书验证数据签名
    *           使用设备中的可信证书验证签名
    * @param    hDeviceHandle       [in]    已打开的密码设备句柄
    * @param    nDataType           [in]    待签名的数据类型
    *                                       0(CMD_DATA):指令数据; 1(CMD_DATA):消息数据
    * @param    pucData             [in]    待计算签名数据
    * @param    nDataLength         [in]    待计算签名数据长度
    * @param    pucCounter          [in]    签名计数器,4字节
    * @param    pucSignCerSn        [in]    签名证书编号,6字节
    * @param    pucSignature         [in]    签名结果,64字节
    *
    * @return  0,成功;其他表示失败
    */
    int VerifySignatureWithTrustedCert(
        void *hDeviceHandle,
        int nDataType,
        unsigned char *pucData,
        int nDataLength,
        unsigned char *pucCounter,
        unsigned char *pucSignCerSn,
        unsigned char *pucSignature);
    /**
    * @brief    使用设备私钥计算数据签名(字符串模式)
    *           使用设备密钥对指令或消息进行签名(输入数据和输出结果均为字符串)
    * @param    hDeviceHandle       [in]    已打开的密码设备句柄
    * @param    nDataType           [in]    待签名的数据类型
    *                                       0(CMD_DATA):指令数据; 1(MSG_DATA):消息数据
    * @param    pcData              [in]    待计算签名数据
    * @param    pcResult            [out]   签名结果,格式:[N字节签名数据原文 || 4字节记数器 || 6字节签名证书号 || 64字节签名值]
    *
    * @return  0,成功;其他表示失败
    */
    int GenerateSignatureWithDevicePrivateKey_String(
        void *hDeviceHandle,
        int nDataType,
        char *pcData,
        char *pcResult);
    /**
    * @brief    使用证书验证数据签名(字符串模式)
    *           使用设备中的可信证书验证签名(输入数据为字符串)
    * @param    hDeviceHandle       [in]    已打开的密码设备句柄
    * @param    nDataType           [in]    待签名的数据类型
    *                                       0(CMD_DATA):指令数据; 1(MSG_DATA):消息数据
    * @param    pcData              [in]    待验证签名数据,格式:[N字节签名数据原文 || 4字节记数器 || 6字节签名证书号 || 64字节签名值]
    *
    * @return  0,成功;其他表示失败
    */
    int VerifySignatureWithTrustedCert_String(
        void *hDeviceHandle,
        int nDataType,
        char *pcData);
    /**         
    * @brief    平台计算数据签名
    *           输出签名结果格式为 Base64( 4B Counter || 6B CertID || 64B Signature )
    * @param    hDeviceHandle       [in]    已打开的密码设备句柄
    * @param    nDataType           [in]    待签名的数据类型
    *                                       0(CMD_DATA):指令数据; 1(MSG_DATA):消息数据
    * @param    pcData              [in]    待签名数据
    * @param    nDataLength         [in]    待签名数据长度
    * @param    pcSignature         [out]   签名
    * @param    pnSignatureLength   [out]   签名长度
    *
    * @return  0,成功;其他表示失败
    */
    int Platform_CalculateSignature(
        void *hDeviceHandle,
        int nDataType,
        unsigned char *pucData,
        int nDataLength,
        char *pcSignature,
        int *pnSignatureLength);
    /**
    * @brief    平台验证数据签名
    *           签名值格式为 Base64( 4B Counter || 6B CertID || 64B Signature )
    * @param    hDeviceHandle       [in]    已打开的密码设备句柄
    * @param    nDataType           [in]    待签名的数据类型
    *                                       0(CMD_DATA):指令数据; 1(MSG_DATA):消息数据
    * @param    pcData              [in]    待签名数据
    * @param    nDataLength         [in]    待签名数据长度
    * @param    szSignature         [out]   待验证签名值
    *
    * @return  0,成功;其他表示失败
    */
    int Platform_VerifySignature(
        void *hDeviceHandle,
        int nDataType,
        unsigned char *pucData,
        int nDataLength,
        char *szSignature);
    /**
    * @brief   计算数据摘要
    *
    * @param   hDeviceHandle   [in]    已打开的密码设备句柄
    * @param   nHashAlg        [in]    SM3之外的HASH算法标识
    * @param   pucInData       [in]    待计算数据摘要的原文数据
    * @param   nDataLength     [in]    输入数据的长度
    * @param   pucHash         [out]   输出HASH结果
    * @param   pnHashLength    [out]   HASH结果长度
    *
    * @return  0,成功;其他表示失败
    */
    int CalcHash(
        void *hDeviceHandle,
        int nHashAlg,
        unsigned char *pucData, int nDataLength,
        unsigned char *pucHash, int * pnHashLength);
    /**
    * @brief   16进制数转字符串
    *
    * @param   pucHex       [in]    输入16进制数据
    * @param   nHexLen      [in]    输入16进制数据长度
    * @param   pcAscii      [out]   输出16进制字符串
    *
    * @return  0,成功;其他表示失败
    */
    int Hex2Ascii(
        unsigned char *pucHex,
        int nHexLen,
        char *pcAscii);
    /**
    * @brief   字符串转16进制数
    *
    * @param   pcAscii      [in]   输入16进制字符串
    * @param   nAaciiLen    [in]   输入16进制字符串长度
    * @param   pucHex       [out]    输出16进制数据
    * @param   pnHexLen     [out]    输出16进制数据长度
    *
    * @return  0,成功;其他表示失败
    */
    int Ascii2Hex(
        char *pcAscii,
        int nAaciiLen,
        unsigned char *pucHex,
        int *pnHexLen);

    void Test(void);

#ifdef __cplusplus
}
#endif


#define YJERR_OK                                0x00000000	    // 操作成功

#define YJERR_OPENDEVICE                        0x00000100      // 打开会话失败
#define YJERR_CLOSEDEVICE                       0x00000200      // 关闭会话失败
#define YJERR_OPENSESSION                       0x00000300      // 打开会话失败
#define YJERR_CLOSESESSION                      0x00000400      // 关闭会话失败
#define YJERR_CREATEFILE                        0x00000500      // 创建文件失败
#define YJERR_DELETEFILE                        0x00000600      // 删除文件失败
#define YJERR_READFILE                          0x00000700      // 读取文件失败
#define YJERR_WRITEFILE                         0x00000800      // 写入文件失败
#define YJERR_HASHINIT                          0x00000900      // HASH初始化失败
#define YJERR_HASHUPDATE                        0x00000A00      // HASH操作失败
#define YJERR_HASHFINAL                         0x00000B00      // HASH结束失败
#define YJERR_EXPPUBKEY                         0x00000C00      // 导出公钥失败
#define YJERR_ACCESSPRIKEY                      0x00000D00      // 获取私钥权限失败
#define YJERR_RELEASESPRIKEY                    0x00000E00      // 释放私钥权限失败
#define YJERR_SIGNATURE                         0x00000F00      // 签名失败
#define YJERR_VERIFY                            0x00001000      // 验签失败

#define YJERR_FILEOPEN                          0x0000E100      // 打开本机文件失败
#define YJERR_FILEREAD                          0x0000E200      // 读取本机文件失败
#define YJERR_COUNTERMUTEXFAILED                0x0000E300      // 签名计数器互斥锁操作失败
#define YJERR_COUNTER                           0x0000E400      // 签名计数器操作失败
#define YJERR_INDEXMUTEXFAILED                  0x0000E500      // 证书索引文件互斥锁操作失败
#define YJERR_INDEX                             0x0000E600      // 证书索引文件操作失败
#define YJERR_MEMSMALL                          0x0000E700      // 提供的缓冲区指针过小
#define YJERR_CERTINDEX                         0x0000E800      // 证书索引超限
#define YJERR_NOCERT                            0x0000E900      // 无证书
#define YJERR_CERTCOUNTMAX                      0x0000EA00      // 证书数量已经最大
#define YJERR_STRLEN                            0x0000EB00      // 字符串长度必须为偶数
#define YJERR_STRVALUE                          0x0000EC00      // 字符串值必须为0-9或A-F

