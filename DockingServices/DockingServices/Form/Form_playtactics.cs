using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DockingServices
{
    public partial class Form_playtactics : Form
    {
        public Form_playtactics()
        {
            InitializeComponent();
        }

        SqlConnection sConn;
        string file;
        string serverIP;
        string db;
        string user;
        string password;
        public int typenum;

        private void btn_AddEvent_Click(object sender, EventArgs e)
        {
            AddPoliceItem addPoliceItem = new AddPoliceItem();
            if (addPoliceItem.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string strevent_type = addPoliceItem.strevent_type;
            string StartTime = addPoliceItem.StartTime;
            string EndTime = addPoliceItem.EndTime;
            string strplay_time = addPoliceItem.strplay_time;
            string strplay_count = addPoliceItem.strplay_count;
            string strplay_rank = addPoliceItem.strplay_rank;
            string strcbb_Type = addPoliceItem.strcbb_Type;

            if (strcbb_Type == "红色预警")
            {
                strcbb_Type = "01";
            }
            else if (strcbb_Type == "橙色预警")
            {
                strcbb_Type = "02"; 
            }
            else if (strcbb_Type == "黄色预警")
            {
                strcbb_Type = "03";
            }
            else
            {
                strcbb_Type = "04";
            }

            string sql = string.Format("insert into Custom_policy values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", strevent_type, StartTime, EndTime, strcbb_Type, strplay_time, strplay_count, strplay_rank);
            try
            {
                sConn.Open();
                SqlCommand cmd = new SqlCommand(sql, sConn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            sConn.Close();
            FlashData();
        }
        
        private void FlashData()
        {
            sConn.Open();
            this.listView1.Items.Clear();
            string sql = "select * from Custom_policy";
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sql, sConn);
            sda.Fill(dt);
            sConn.Close();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string num = dt.Rows[i][0].ToString();
                string strevent_type = dt.Rows[i][1].ToString();
                string StartTime = dt.Rows[i][2].ToString();
                string EndTime = dt.Rows[i][3].ToString();

                string EvenType = dt.Rows[i][4].ToString();
                if (EvenType == "01")
                {
                    EvenType = "红色预警";
                }
                else if (EvenType == "02")
                {
                    EvenType = "橙色预警";
                }
                else if (EvenType == "03")
                {
                    EvenType = "黄色预警";
                }
                else
                {
                    EvenType = "蓝色预警";
                }

                string strplay_time = dt.Rows[i][5].ToString();
                string strplay_count = dt.Rows[i][6].ToString();
                string strplay_rank = dt.Rows[i][7].ToString();

                ListViewItem item = new ListViewItem();
                item.Text = num;
                item.SubItems.Add(strevent_type);
                item.SubItems.Add(StartTime);
                item.SubItems.Add(EndTime);
                item.SubItems.Add(EvenType);

                item.SubItems.Add(strplay_time);
                item.SubItems.Add(strplay_count);
                item.SubItems.Add(strplay_rank);

                listView1.Items.Add(item);
            }
        }

        private void Form_playtactics_Load(object sender, EventArgs e)
        {
            file = System.Windows.Forms.Application.ExecutablePath + ".config";
            serverIP = "";// ConfigHelper.GetAppConfig("ServerIP");
            db = "";// ConfigHelper.GetAppConfig("DataBase");
            user = "";// ConfigHelper.GetAppConfig("user");
            password = "";// ConfigHelper.GetAppConfig("password");
            try
            {
                string strConnectString = "";

                strConnectString = "server =";
                strConnectString += serverIP;
                strConnectString += ";database=";
                strConnectString += db;
                strConnectString += ";uid=";
                strConnectString += user;
                strConnectString += ";pwd=";
                strConnectString += password;

                sConn = new SqlConnection(strConnectString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          //  FlashData();
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                string id = this.listView1.FocusedItem.SubItems[0].Text;
                string sql = string.Format("delete from Custom_policy where NO='{0}'", id);
                DialogResult dr = MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        sConn.Open();
                        SqlCommand cmd = new SqlCommand(sql, sConn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    sConn.Close();
                    FlashData();
                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
