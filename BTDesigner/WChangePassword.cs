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
    public partial class WChangePassword : Form
    {
        public WChangePassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pass1 = textBox1.Text;
            string pass2 = textBox2.Text;
            string pass3 = textBox3.Text;
            bool validPass1 = FormValidation.CheckProfileString(pass1);
            bool validPass2 = FormValidation.CheckProfileString(pass2);
            bool validPass3 = FormValidation.CheckProfileString(pass3);
            if(!validPass1||!validPass2||!validPass3)
            {
                MessageBox.Show("Datele introduse nu sunt corecte!");
                return;
            }
if(!FormValidation.StringsMatches(pass2, pass3))
                {
                    MessageBox.Show("Parolele nu coincid");
                    return;
                }
if(!FormValidation.StringsMatches(pass1, Core.CurrentProfile.Password))
            {
                MessageBox.Show("Parola veche este greşită!");
                return;
            }
            bool changed=Core.CurrentProfile.ChangePassword(pass2);
            if(changed)
            {
                MessageBox.Show("Parola a fost schimbată cu succes!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Eroare la modificarea parolei!");
            }
        }

    }
}
