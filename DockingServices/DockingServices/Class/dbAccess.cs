using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DockingServices
{
    public class dbAccess
    {
        public SqlConnection conn;
        private Boolean isSet;
        public Object oSqlLock;
        private int iTimeOut;

        public int ITimeOut
        {
            get { return iTimeOut; }
            set { iTimeOut = value; }
        }
        public Boolean IsSet
        {
            get { return isSet; }
        }

        public ConnectionState ConnState
        {
            get
            {
                if (this.conn == null)
                    return ConnectionState.Closed;
                else
                    return this.conn.State;
            }
        }

        public dbAccess()
        {
            conn = new SqlConnection();
            oSqlLock = new Object();
        }

        //打开Conn
        public Boolean OpenConn()
        {
            if (this.conn.State == ConnectionState.Open)
                return true;
            else
            {
                try
                {
                    conn.Open();
                    this.isSet = true;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                    throw;
                }
            }
        }

        //关闭conn
        public Boolean CloseConn()
        {
            conn.Close();
            return conn.State == ConnectionState.Closed;
        }

        //根据SQL语句获取查询结果表
        public DataTable getQueryInfoBySQL(string strQry)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = GetCmdByProperty();
                cmd.Connection = conn;
                cmd.CommandText = strQry;

                lock (oSqlLock)
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(strQry, conn))
                    {
                        da.Fill(dt);
                    }
                    return dt;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("getQueryInfoBySQL:" + e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        //根据SQL语句获取查询结果表
        public DataTable getQueryInfoBySQL(string strQry, SqlTransaction sqlTran)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = GetCmdByProperty();
                cmd.Connection = conn;
                cmd.Transaction = sqlTran;
                cmd.CommandText = strQry;

                lock (oSqlLock)
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(strQry, conn))
                    {
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("getQueryInfoBySQL:" + e.Message);
                return null;
            }
        }

        public int getResultIDBySQL(string strQry, string tablename)
        {
            try
            {
                SqlCommand cmd = GetCmdByProperty();
                cmd.Connection = conn;
                cmd.CommandText = strQry;
                lock (oSqlLock)
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteScalar();

                    cmd.CommandText = "SELECT  IDENT_CURRENT('" + tablename + "')";
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("getResultIDBySQL:" + e.Message);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        //根据SQL语句获取查询结果
        public Object getQueryResultBySQL(string strQry, SqlTransaction sqlTran)
        {
            try
            {
                SqlCommand cmd = GetCmdByProperty();
                cmd.Connection = conn;
                cmd.Transaction = sqlTran;
                cmd.CommandText = strQry;
                lock (oSqlLock)
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("getQueryResultBySQL:" + e.Message);
                return null;
            }
        }

        //根据SQL语句获取查询结果
        public Object getQueryResultBySQL(string strQry)
        {
            try
            {
                SqlCommand cmd = GetCmdByProperty();
                cmd.Connection = conn;
                cmd.CommandText = strQry;
                lock (oSqlLock)
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("getQueryResultBySQL:" + e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        //根据存储过程更新数据库，带参数
        public int UpdateDbByProc(string sProcedure, SqlParameter[] procParm)
        {
            try
            {
                SqlCommand cmd = GetCmdByProperty();
                cmd.Connection = conn;
                cmd.CommandText = sProcedure;
                cmd.Parameters.AddRange(procParm);
                cmd.CommandType = CommandType.StoredProcedure;
                lock (oSqlLock)
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateDbByProc:" + e.Message);
                return -1;
            }
            finally
            {
                conn.Close();

            }
        }

        //根据存储过程更新数据库，无参数
        public int UpdateDbByProc(string sProcedure)
        {
            try
            {
                SqlCommand cmd = GetCmdByProperty();
                cmd.Connection = conn;
                cmd.CommandText = sProcedure;
                cmd.CommandType = CommandType.StoredProcedure;

                lock (oSqlLock)
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateDbByProc:" + e.Message);
                return -1;
            }
            finally
            {
                conn.Close();
            }
        }

        //根据SQL更新数据库
        public int UpdateDbBySQL(string sSQL)
        {
            SqlCommand cmd = GetCmdByProperty();
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = sSQL;

                lock (oSqlLock)
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateDbBySQL:" + e.Message);
                return -1;
            }
            finally
            {
                if (cmd != null)
                    cmd = null;
                conn.Close();
            }
        }

        public void UpdateOrInsertBySQL(string strQry)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(strQry, conn);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateDbBySQL:" + ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public void DelDataBySQL(string strQry)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(strQry, conn);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DelDataBySQL:" + ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// 插入数据并返回ID
        /// </summary>
        /// <param name="sSQL"></param>
        /// <returns></returns>
        public int InsertDbBySQLRetID(string sSQL)
        {
            try
            {
                int iRet = 0;
                SqlCommand cmd = new SqlCommand(sSQL, conn);
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = sSQL;
                cmd.CommandType = CommandType.Text;
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                iRet = (int)cmd.ExecuteScalar();
                return iRet;
            }
            catch (Exception e)
            {
                Console.WriteLine("InsertDbBySQL:" + e.Message);
                return -1;
            }
            finally
            {
                conn.Close();
            }

        }

        public int UpdateDbBySQLRetID(string sSQL)
        {
            try
            {
                int iRet = 0;
                SqlCommand cmd = new SqlCommand(sSQL, conn);
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = sSQL + ";Select @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj == null)
                    iRet = 0;
                else
                {
                    if (!int.TryParse(obj.ToString(), out iRet))
                        iRet = 0;
                }
                return iRet;
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateDbBySQL:" + e.Message);
                return -1;
            }
            finally
            {
                conn.Close();
            }

        }

        //根据SQL更新数据库，运行于事物中，无需关闭窗体
        public int UpdateDbBySQL(string sSQL, SqlTransaction sqlTran)
        {
            SqlCommand cmd = GetCmdByProperty();
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = sSQL;
                cmd.Transaction = sqlTran;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateDbBySQL:" + e.Message);
                return -1;
            }
            finally
            {
                if (cmd != null)
                    cmd = null;
                conn.Close();
            }
        }

        /// <summary>
        /// 更新或新增记录  特殊用法
        /// </summary>
        /// <param name="sTable"></param>      表名
        /// <param name="sColValTye"></param>  字符串数组，组成方式为 “字段名，值,字段类型”
        /// <param name="sWhere"></param>      更新条件
        /// <param name="dba"></param>         数据库处理对象
        /// <returns></returns>                返回值，成功1、失败-1
        public static int UpdateOrInsert(string sTable, int iMode, List<string> sColValTye, string sWhere, dbAccess dba)
        {
            try
            {
                string sSql = "", sSql2 = "", sTmp = "";
                string[] strTmp;
                if (iMode == 1) //新增
                {
                    for (int i = 0; i < sColValTye.Count; i++)
                    {
                        if (sColValTye[i] == null || sColValTye[i].Length == 0) continue;
                        strTmp = sColValTye[i].Split(',');
                        if (strTmp.Length == 3)
                        {
                            if (strTmp[2].ToLower() == "string")
                            {
                                sSql += strTmp[0] + ",";
                                sSql2 += "'" + strTmp[1] + "',";
                            }
                            else if (strTmp[2].ToLower() == "datetime")
                            {
                                sSql += strTmp[0] + ",";
                                sSql2 += "cast('" + strTmp[1] + "' as datetime),";
                            }
                            else if (strTmp[2].ToLower() == "int")
                            {
                                sSql += strTmp[0] + ",";
                                sSql2 += strTmp[1] + ",";
                            }
                        }
                        else if (strTmp.Length > 3)
                        {
                            sTmp = "";
                            for (int j = 1; j < strTmp.Length - 1; j++)
                            {
                                sTmp = sTmp + strTmp[j] + ",";
                            }
                            sTmp = sTmp.TrimEnd(',');
                            if (strTmp[strTmp.Length - 1].ToLower() == "string")
                            {
                                sSql += strTmp[0] + ",";
                                sSql2 += "'" + sTmp + "',";
                            }
                            else if (strTmp[strTmp.Length - 1].ToLower() == "datetime")
                            {
                                sSql += strTmp[0] + ",";
                                sSql2 += "cast('" + strTmp[1] + "' as datetime),";
                            }
                            else if (strTmp[strTmp.Length - 1].ToLower() == "int")
                            {
                                sSql += strTmp[0] + ",";
                                sSql2 += sTmp + ",";
                            }
                        }
                    }
                    if (sSql.Length > 0)
                    {
                        sSql = sSql.TrimEnd(',');
                        sSql2 = sSql2.TrimEnd(',');
                        sSql = "Insert into " + sTable + "(" + sSql + ") values (" + sSql2 + ")";
                    }
                }
                else if (iMode == 2) //修改
                {
                    for (int i = 0; i < sColValTye.Count; i++)
                    {
                        if (sColValTye[i] == null || sColValTye[i].Length == 0) continue;
                        strTmp = sColValTye[i].Split(',');

                        if (strTmp.Length == 3)
                        {
                            if (strTmp[2].ToLower() == "string")
                            {
                                sSql += strTmp[0] + " = '" + strTmp[1] + "',";
                            }
                            else if (strTmp[2].ToLower() == "datetime")
                            {
                                sSql += strTmp[0] + " = cast('" + strTmp[1] + "' as datetime),";
                            }
                            else if (strTmp[2].ToLower() == "int")
                            {
                                sSql += strTmp[0] + " = " + strTmp[1] + ",";
                            }
                        }
                        else if (strTmp.Length > 3)
                        {
                            sTmp = "";
                            for (int j = 1; j < strTmp.Length - 1; j++)
                            {
                                sTmp = sTmp + strTmp[j] + ",";
                            }
                            sTmp = sTmp.TrimEnd(',');
                            if (strTmp[strTmp.Length - 1].ToLower() == "string")
                            {
                                sSql += strTmp[0] + " = '" + sTmp + "',";
                            }
                            else if (strTmp[strTmp.Length - 1].ToLower() == "datetime")
                            {
                                sSql += strTmp[0] + " = cast('" + strTmp[1] + "' as datetime),";
                            }
                            else if (strTmp[strTmp.Length - 1].ToLower() == "int")
                            {
                                sSql += strTmp[0] + " = " + sTmp + ",";
                            }
                        }
                    }
                    if (sSql.Length > 0)
                    {
                        sSql = sSql.TrimEnd(',');
                        sSql = "Update " + sTable + " Set " + sSql + (sWhere.Length > 0 ? sWhere : "");
                    }

                }
                if (sSql.Length > 0 && dba.UpdateDbBySQL(sSql) != 1)
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
            return 1;
        }

        //insert data
        public bool InsertData(string strSql, SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strSql;
            cmd.Parameters.AddRange(parameters);

            if (conn.State != ConnectionState.Open)
                conn.Open();
            //finally, execute the command.
            int retval = cmd.ExecuteNonQuery();
            // detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear();
            if (retval < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public bool InsertData(string strSql)
        {
            try
            {
                bool iRet =false;
                SqlCommand cmd = new SqlCommand(strSql, conn);
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
               // cmd.CommandText = sSQL;
                cmd.CommandType = CommandType.Text;
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                object obj = cmd.ExecuteNonQuery();
                if (obj != null)
                    iRet = true;
                return iRet;
            }
            catch (Exception e)
            {
                Console.WriteLine("InsertDataError:" + e.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        //获取服务器时间
        public DateTime GetServerTime()
        {
            try
            {
                DateTime dt;
                string sDatetime = "select Getdate() from Users";
                dt = Convert.ToDateTime(getQueryResultBySQL(sDatetime));
                return dt;
            }
            catch (Exception e)
            {
                Console.WriteLine("GetServerTime:" + e.Message);
                return DateTime.Now;
            }
        }

        //使用统一限制属性创建SQLCommand
        public SqlCommand GetCmdByProperty()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = iTimeOut;
            return cmd;
        }
        public void BulkToDB(DataTable dt)
        {
            if (dt == null || dt.Rows.Count < 1) return;
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
            bulkCopy.DestinationTableName = "RdsBackUp";
            bulkCopy.BulkCopyTimeout = iTimeOut;
            bulkCopy.BatchSize = dt.Rows.Count;
            try
            {

                lock (oSqlLock)
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    bulkCopy.WriteToServer(dt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("BulkToDB:" + ex.Message);
            }
            finally
            {
                conn.Close();
                if (bulkCopy != null)
                    bulkCopy.Close();
            }
        }

        /// <summary>
        /// 判断用户登录
        /// </summary>
        /// <param name="sCode"></param>  编号
        /// <param name="sUser"></param>  用户名
        /// <param name="sPass"></param>  密码
        /// <param name="dba"></param>    数据库操作
        /// <returns></returns>           返回查询结果值
        public int loginJudge(string sCode, string sUser, string sPass, dbAccess dba)
        {
            String sLog = "Select count(1) as usercount from users where user_code ='"
                        + sCode + "' and user_detail = '" + sUser
                        + "' and user_pwd = '" + sPass + "'";
            return Convert.ToInt32(getQueryResultBySQL(sLog));
        }
    }
}
