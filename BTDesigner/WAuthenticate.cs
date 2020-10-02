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
    public partial class WAuthenticate : Form
    {
        public WAuthenticate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string password = textBox2.Text;
            bool nameCheck = FormValidation.CheckProfileString(name);
            bool passwordCheck = FormValidation.CheckProfileString(password);
            if (!nameCheck||!passwordCheck)
            {
                MessageBox.Show("Datele introduse nu sunt corecte!");
                return;
            }
                bool res = Core.CurrentProfile.Load(name, password);
            if (!res)
            {
                MessageBox.Show("Datele introduse nu corespund nici unui cont!");
                return;
            }
            else
            {
                MessageBox.Show("Autentificare cu succes");
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WRegister r = new WRegister();
            r.ShowDialog();
        }

        private void Authenticate_FormClosed(object sender, FormClosedEventArgs e)
        {
if(!Core.CurrentProfile.IsAuthenticated)
            {
                Application.Exit();
            }
        }
    }
}
