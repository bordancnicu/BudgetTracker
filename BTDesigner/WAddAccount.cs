using BTCore;
using BTCore.Classes;
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
    public partial class WAddAccount : Form
    {
        public Account account=new Account();
        public WAddAccount()
        {
            InitializeComponent();
            SetupCurrencies();
            SetupScopes();
        }


        private void SetupCurrencies()
        {
            comboBox1.DataSource = Core.CurrentProfile.Currencies;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
        }

        private void SetupScopes()
        {
            DataTable dt = Account.GetScopes();
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            double balance;
            bool convert = Double.TryParse(textBox2.Text, out balance);
            if (!convert)
            {
                MessageBox.Show("Valoarea introdusă pentru balanţa contului nu este corectă. Te rugăm să introduci doar numere reale!");
            return;
            }
            int currencyId =Convert.ToInt32(comboBox1.SelectedValue);
            int scope = Convert.ToInt32(comboBox2.SelectedValue);
            int profileId = Core.CurrentProfile.Id;
            bool res=account.Add(profileId, name, balance, currencyId, scope);
            if(!res)
            {
                MessageBox.Show("Nu s-a reuşit adăugarea contului!");
            }
            else
            {
                Core.CurrentProfile.RefreshAccounts();
                MessageBox.Show("Contul a fost adăugat cu succes!");
                this.Close();
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
