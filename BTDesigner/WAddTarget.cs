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
    public partial class WAddTarget : Form
    {
        public WAddTarget()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            bool valid = FormValidation.CheckProfileString(name);
            if (!valid)
            {
                MessageBox.Show("Datele introduse nu sunt valide!");
                return;
            }
            new Target().Add(name, Core.CurrentProfile.Id);
            MessageBox.Show("Datale au fost introduse cu succes!");
            this.Close();
        }
    }
}
