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
    public partial class WRegister : Form
    {
        public WRegister()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string password= textBox2.Text;
            string confirmPassword= textBox3.Text;
            bool passwordIdentity= FormValidation.StringsMatches(password, confirmPassword);
            if(!passwordIdentity)
            {
                MessageBox.Show("Parolele introduse nu se potrivesc!");
                return;
            }
            bool validName = FormValidation.CheckProfileString(name);
            bool validPassword= FormValidation.CheckProfileString(password);
            if(!validName||!validPassword)
            {
                MessageBox.Show("Datele introduse nu sunt corecte!");
                return;
            }
            bool res = Core.CurrentProfile.Add(name, password);
            if(res)
            {
                MessageBox.Show("V-aţi înregistrat cu succes!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Datele introduse nu sunt corect4e!");
            }
        }
    }
}
