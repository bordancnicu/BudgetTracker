using BTCore;
using BTCore.Classes;
using BTCore.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTDesigner
{
    public partial class WMain : Form
    {
        public WMain()
        {
            InitializeComponent();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Core.CurrentProfile.Destroy();
        }


        private void button2_Click(object sender, EventArgs e)
        {
                new WChangePassword().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new WAddAccount().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new WAddTransaction().ShowDialog();
        }

        private void Main_Activated(object sender, EventArgs e)
        {
            FillAccountsComboBox();
        }

        private void FillAccountsComboBox()
        {
            DataTable items = DisplayTools.getComboBoxPattern();
            if(Core.CurrentProfile.HasAccounts)
            {
            for(int i=0; i<Core.CurrentProfile.Accounts.Length; i++)
            {
                Account account = Core.CurrentProfile.Accounts[i];
                DataRow row = items.NewRow();
                row["value"] = account.Id;
                row["display"] = account.Name + " " + account.Balance + " " + account.ACurrency.Simbol;
                items.Rows.Add(row);
            }
            }
            else
            {
                DataRow row = items.NewRow();
                row["value"] = 0;
                row["display"] = "Nu aveti inca nici un cont.";
                items.Rows.Add(row);
            }
            DisplayTools.setComboBoxSource(comboBox1, items);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new WAddCategory().ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new WAddTarget().ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(Core.CurrentProfile.HasAccounts)
            {
                new WShowTransactions().ShowDialog();
            }
            else
            {
                MessageBox.Show("Nu aveţi nici un cont!");
            }
        }
    }
}