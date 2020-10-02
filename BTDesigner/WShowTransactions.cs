using BTCore;
using BTCore.Classes;
using BTDesigner.WindowProcessors;
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
    public partial class WShowTransactions : Form
    {

        public WShowTransactions()
        {
            InitializeComponent();
            InitializeAccountsComboBox();
            InitializeCategoriesComboBox();
            InitializeTargetsComboBox();
            InitializeDateTexts();
        }

        private void InitializeAccountsComboBox()
        {
            SetSourceToComboBox(WPShowTransaction.GetAccountsComboBox(), comboBox1);
        }

        private void InitializeCategoriesComboBox()
        {
            SetSourceToComboBox(WPShowTransaction.GetCategoriesComboBox(), comboBox2);
        }

        private void InitializeTargetsComboBox()
        {
            SetSourceToComboBox(WPShowTransaction.GetTargetsComboBox(), comboBox3);
        }

private void InitializeDateTexts()
        {
            textBox1.Text = WPShowTransaction.GetStartDateText();
            textBox2.Text = WPShowTransaction.GetEndDateText();
        }

        private void SetSourceToComboBox(DataTable source, ComboBox comboBox)
        {
            comboBox.DataSource = source;
            comboBox.ValueMember = "value";
            comboBox.DisplayMember = "display";
        }


        private void ValidateRequest()
        {
            int accountId= (int)comboBox1.SelectedValue;
            int categoryId= (int)comboBox2.SelectedValue;
            int targetId= (int)comboBox3.SelectedValue;
            DateTime startDate;
            DateTime endDate;
            if(!DateTime.TryParse(textBox1.Text, out startDate)|| !DateTime.TryParse(textBox2.Text, out endDate))
            {
                MessageBox.Show("Formatul de data nu este corect! Va rugam respectaţi formatul de dată, specificat în câmpul corespunzător.");
                return;
            }
            if(startDate.CompareTo(endDate)>0)
            {
                MessageBox.Show("Data de inceput trebuie sa fie inaintea datei de sfârşit!");
                return;
            }
            TransactionsView tView = new TransactionsView();
            if(categoryId!=0) tView.CategoryId = categoryId;
            if (targetId!= 0) tView.TargetId= targetId;
            tView.StartDate = startDate;
            tView.EndDate = endDate;
            dataGridView1.DataSource = Core.CurrentProfile.Accounts[accountId].GetTransactionsView(tView);
            OutputStatistics(Core.CurrentProfile.Accounts[accountId].GetStatisticsView(tView));
        }

        private void OutputStatistics(DataTable statistics)
        {
            string display = "Bilanţ total: " + statistics.Rows[0]["total"]+ System.Environment.NewLine;
            display += "Total încasat: " + statistics.Rows[0]["positive"]+ System.Environment.NewLine;
            display += "Total cheltuit: " + statistics.Rows[0]["negative"] + System.Environment.NewLine;
            textBox3.Text = display;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                ValidateRequest();
        }

    }
}
