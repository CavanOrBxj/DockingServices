using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace DockingServices
{
    /// <summary>
    /// comboBox 数据源的绑定
    /// </summary>
    public class ComboBind
    {
        /// <summary>
        /// DataGridViewComboBoxColumn 数据源的绑定
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="dict"></param>
        public static void Binding(DataGridViewComboBoxColumn cmb, Dictionary<object, object> dict)
        {
            cmb.DisplayMember = "Key";
            cmb.ValueMember = "Value";
            cmb.DataSource = BuildDataTable(cmb.DisplayMember, cmb.ValueMember, dict);
        }

        //public static void Binding(ComboBoxColumn cmb, Dictionary<object, object> dict)
        //{
        //    cmb.DisplayMember = "Key";
        //    cmb.ValueMember = "Value";
        //    cmb.DataSource = BuildDataTable(cmb.DisplayMember, cmb.ValueMember, dict);
        //}

        //public static void Binding(DataGridViewComboBoxExColumn cmb, Dictionary<object, object> dict)
        //{
        //    cmb.DisplayMember = "Key";
        //    cmb.ValueMember = "Value";
        //    cmb.DataSource = BuildDataTable(cmb.DisplayMember, cmb.ValueMember, dict);
        //}
        
        /// <summary>
        /// DataGridViewComboBoxColumn 数据源的绑定
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="dict"></param>
        public static void Binding(DataGridViewComboBoxCell cmb, Dictionary<object, object> dict)
        {
            cmb.DisplayMember = "Key";
            cmb.ValueMember = "Value";
            cmb.DataSource = BuildDataTable(cmb.DisplayMember, cmb.ValueMember, dict);
        }

        /// <summary>
        /// DataGridViewComboBoxColumn 数据源的绑定
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="dict"></param>
        public static void Binding1(DataGridViewComboBoxColumn cmb, Dictionary<object, object> dict)
        {
            cmb.ValueMember = "Value";
            cmb.DisplayMember = "Key";
            cmb.DataSource = BuildDataTable(cmb.DisplayMember, cmb.ValueMember, dict, true);
        }

        //public static void Binding(BaseDataGridViewComboBoxColumn cmb, Dictionary<object, object> dict)
        //{
        //    cmb.DisplayMember = "Key";
        //    cmb.ValueMember = "Value";
        //    cmb.DataSource = BuildDataTable(cmb.DisplayMember, cmb.ValueMember, dict);
        //}
        /// <summary>
        /// ComboBox 数据源的绑定
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="dict"></param>
        public static void Binding(ComboBox cmb, Dictionary<object, object> dict)
        {
            cmb.DisplayMember = "Key";
            cmb.ValueMember = "Value";
            cmb.DataSource = BuildDataTable(cmb.DisplayMember, cmb.ValueMember, dict);
        }



        /// <summary>
        /// ComboBox 数据源的绑定
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="dict"></param>
        public static void Binding1(ComboBox cmb, Dictionary<object, object> dict)
        {
            cmb.DisplayMember = "Key";
            cmb.ValueMember = "Value";
            cmb.DataSource = BuildDataTable(cmb.DisplayMember, cmb.ValueMember, dict, true);
        }
        /// <summary>
        /// 将源字典转换为表格，字典的key为一列，value为一列。
        /// </summary>
        /// <param name="key">指定Key列名称。</param>
        /// <param name="value">指定值列名称。</param>
        /// <param name="dict">数据源字典。</param>
        /// <param name="rotate">行和列显示倒换。</param>
        /// <returns></returns>
        private static DataTable BuildDataTable(string key, string value, Dictionary<object, object> dict, bool rotate = false)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(key, typeof(object));
            dt.Columns.Add(value, typeof(object));

            foreach (var item in dict)
            {
                if (rotate)
                {
                    dt.Rows.Add(item.Value, item.Key);
                }
                else
                {
                    dt.Rows.Add(item.Key, item.Value);
                }
            }

            return dt;
        }

    }
}