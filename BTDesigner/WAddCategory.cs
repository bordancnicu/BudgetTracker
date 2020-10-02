using BTCore;
using BudgetTracker;
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
    public partial class WAddCategory : Form
    {
        public WAddCategory()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            bool validString=FormValidation.CheckProfileString(name);
            if (!validString)
            {
                MessageBox.Show("Numele introdus nu este valid!");
                return;
            }
            new Category().Add(name, Core.CurrentProfile.Id);
            Core.CurrentProfile.RefreshCategories();
            MessageBox.Show("Categoria a fost adăugată cu succes!");
            this.Close();
        }
    }
}
