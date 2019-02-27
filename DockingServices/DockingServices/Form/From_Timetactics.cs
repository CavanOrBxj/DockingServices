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
    public partial class From_Timetactics : Form
    {
        public From_Timetactics()
        {
            InitializeComponent();
        }

        private void From_Timetactics_Load(object sender, EventArgs e)
        {
            InitCombox();
            //显示数据
            FlashData();
        }


        void InitCombox()
        {
            try
            {
                Dictionary<object, object> dic = new Dictionary<object, object>();
                dic.Add("未知级别","0");
                dic.Add("特别重大/红色预警", "1");
                dic.Add("重大/橙色预警", "2");
                dic.Add("较大/黄色预警", "3");
                dic.Add("一般/蓝色预警", "4");
                dic.Add("全级别", "100");
                ComboBind.Binding(cbb_Type, dic);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        private void btn_AddEvent_Click(object sender, EventArgs e)
        {
            string StartTime = dtp_StartTime.Text;
            string EndTime = dtp_EndTime.Text;
            string EvenType = cbb_Type.SelectedValue.ToString();
            string sql = string.Format("insert into EBTime_Strategy values('{0}','{1}','{2}') SELECT CAST(scope_identity() AS int)", StartTime, EndTime, EvenType);

            int ID = mainForm.dba.InsertDbBySQLRetID(sql);
            timestrategies pp = new timestrategies();
            pp.ID = ID.ToString();
            pp.StartTime = StartTime;
            pp.EndTime = EndTime;
            pp.EvenType = EvenType;
            SingletonInfo.GetInstance().audit.TimeList.Add(pp);
            FlashData();

        }

        private void FlashData()
        {
            this.listView1.Items.Clear();
            string sqlstr = "select * from EBTime_Strategy";
            DataTable dt = mainForm.dba.getQueryInfoBySQL(sqlstr);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string num = dt.Rows[i][0].ToString();
                string StartTime = dt.Rows[i][1].ToString();
                string EndTime = dt.Rows[i][2].ToString();
                string EvenType = "";

                switch (dt.Rows[i][3].ToString())
                {
                    case "0":
                        EvenType = "未知级别";
                        break;
                    case "1":
                        EvenType = "特别重大/红色预警";
                        break;
                    case "2":
                        EvenType = "重大/橙色预警";
                        break;
                    case "3":
                        EvenType = "较大/黄色预警";
                        break;
                    case "4":
                        EvenType = "一般/蓝色预警";
                        break;
                    case "100":
                        EvenType = "全级别";
                        break;
                }
                ListViewItem item = new ListViewItem();
                item.Text = num;
                item.SubItems.Add(StartTime);
                item.SubItems.Add(EndTime);
                item.SubItems.Add(EvenType);
                listView1.Items.Add(item);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int pp = 0;
                if (this.listView1.SelectedItems.Count > 0)
                {
                    string id = this.listView1.FocusedItem.SubItems[0].Text;

                    DialogResult dr = MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {
                        string sql = string.Format("delete from EBTime_Strategy where id='{0}'", id);
                        mainForm.dba.DelDataBySQL(sql);
                        timestrategies SELECTED = SingletonInfo.GetInstance().audit.TimeList.Find(C => C.ID.Equals(id));
                        SingletonInfo.GetInstance().audit.TimeList.Remove(SELECTED);
                        pp = SingletonInfo.GetInstance().audit.TimeList.Count;
                       // MessageBox.Show("策略数量："+ pp.ToString());
                        FlashData();
                    }
                }
                else
                {
                    MessageBox.Show("未选中任何行");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
    }
}
