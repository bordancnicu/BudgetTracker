using BTCore.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTDesigner
{
    public class DisplayTools
    {

        public static DataTable getComboBoxPattern()
        {
            DataTable items = new DataTable();
            items.Columns.Add("value", typeof(Int32));
            items.Columns.Add("display", typeof(string));
            return items;
        }

        public static void setComboBoxSource(ComboBox comboBox, DataTable data)
        {
            comboBox.DataSource = data;
            comboBox.DisplayMember = "display";
            comboBox.ValueMember = "value";
        }



    }
}
