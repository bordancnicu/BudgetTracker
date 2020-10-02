using BTCore;
using BTCore.Classes;
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
    public partial class WAddTransaction : Form
    {
        string ValidationMessage
        {
            get; set;
        }

        public WAddTransaction()
        {
            InitializeComponent();
            ValidationMessage = "";
        }

        private void WAddTransaction_Activated(object sender, EventArgs e)
        {
            if (!Core.CurrentProfile.HasAccounts)
            {
                MessageBox.Show("Nu aveţi încă nici un cont pentru a putea adăuga tranzacţii!");
                this.Close();
            }
            else
            {
                loadInOutComboBox();
                loadCategoriesComboBox();
                loadTargetsComboBox();
                loadAccountsComboBox();
                ShowOtherTextBox(comboBox2, textBox3);
                ShowOtherTextBox(comboBox3, textBox4);
            }
        }

        private void loadInOutComboBox()
        {
            DataTable items = DisplayTools.getComboBoxPattern();
            DataRow row1 = items.NewRow();
            DataRow row2 = items.NewRow();
            row1["value"] = 0;
            row1["display"] = "Plătit";
            row2["value"] = 1;
            row2["display"] = "Încasat";
            items.Rows.Add(row1);
            items.Rows.Add(row2);
            DisplayTools.setComboBoxSource(comboBox1, items);
        }

        private void loadCategoriesComboBox()
        {
            DataTable items = DisplayTools.getComboBoxPattern();
            if (Core.CurrentProfile.HasCategories)
            {
                for (int i = 0; i < Core.CurrentProfile.Categories.Length; i++)
                {
                    DataRow row = items.NewRow();
                    row["value"] = Core.CurrentProfile.Categories[i].Id;
                    row["display"] = Core.CurrentProfile.Categories[i].Name;
                    items.Rows.Add(row);
                }
            }
            DataRow other = items.NewRow();
            other["value"] = -1;
            other["display"] = "Alta:";
            items.Rows.Add(other);
            DisplayTools.setComboBoxSource(comboBox2, items);
        }

        private void loadTargetsComboBox()
        {
            DataTable items = DisplayTools.getComboBoxPattern();
            if (Core.CurrentProfile.HasTargets)
            {
                for (int i = 0; i < Core.CurrentProfile.Targets.Length; i++)
                {
                    DataRow row = items.NewRow();
                    row["value"] = Core.CurrentProfile.Targets[i].Id;
                    row["display"] = Core.CurrentProfile.Targets[i].Name;
                    items.Rows.Add(row);
                }
            }
            DataRow other = items.NewRow();
            other["value"] = -1;
            other["display"] = "Altul:";
            items.Rows.Add(other);
            DisplayTools.setComboBoxSource(comboBox3, items);
        }

        private void loadAccountsComboBox()
        {
            DataTable items = DisplayTools.getComboBoxPattern();
            if(Core.CurrentProfile.HasAccounts)
            {
            for (int i = 0; i < Core.CurrentProfile.Accounts.Length; i++)
            {
                DataRow row = items.NewRow();
                Account account = Core.CurrentProfile.Accounts[i];
                row["value"] = account.Id;
                row["display"] = account.Name + " " + account.Balance + " " + account.ACurrency.Simbol;
                items.Rows.Add(row);
            }
            }
            DisplayTools.setComboBoxSource(comboBox4, items);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowOtherTextBox(comboBox2, textBox3);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowOtherTextBox(comboBox3, textBox4);
        }

    private void ShowOtherTextBox(ComboBox combo, TextBox text)
        {
            text.Visible = false;
            int selected;
            bool parseOK = Int32.TryParse(combo.SelectedValue.ToString(), out selected);
            if (selected == -1)
                text.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = CheckName();
            double value= CheckValue();
            int recurent = Convert.ToInt32(checkBox1.Checked);
            int categoryId = CheckCategories();
            int targetId = CheckTargets();
            int accountId = (int)comboBox4.SelectedValue;
            int profileId = Core.CurrentProfile.Id;

            if (!String.IsNullOrEmpty(ValidationMessage))
            {
                MessageBox.Show(ValidationMessage);
                ValidationMessage = "";
            }
            else
            {
                bool success=new Transaction().Add(accountId, profileId, name, value, recurent, categoryId, targetId);
                if (success)
                {
                    MessageBox.Show("Tranzacţia a fost salvată");

                    this.Close();
                }
                else
                    MessageBox.Show("Tranzacţia nu a putut fi salvată.");
            }

        }

        private string CheckName()
        {
            string name = textBox1.Text;
            if (!FormValidation.CheckProfileString(name))
                ValidationMessage += "Numele introdus nu este valid! ";
            return name;
        }

        private double CheckValue()
        {
            double value= -1;
            if (!Double.TryParse(textBox2.Text, out value))
                ValidationMessage += "Suma introdusă nu este corectă! ";
            else
            {
if((int)comboBox1.SelectedValue==0)
                {
                    value = MakeNegative(value);
                }
            }
            return value;
        }

        private int CheckCategories()
        {
            int category = (int)comboBox2.SelectedValue;
            if(category==-1)
            {
                category = CheckNewCategory();
            }
            return category;
        }

        private int CheckNewCategory()
        {
            int result = 0;
            string newCategory = textBox3.Text;
            bool valid= FormValidation.CheckProfileString(newCategory);
            if (!valid)
                {
                    ValidationMessage += "Numele categoriei nu este valid! "; 
                }
            else
            {
                Category addedCategory = new Category();
                if(addedCategory.Add(newCategory, Core.CurrentProfile.Id))
                {
                    result = addedCategory.Id;
                }
            }
            return result;
        }

        private int CheckTargets()
        {
            int target= (int)comboBox3.SelectedValue;
            if (target== -1)
            {
                target= CheckNewTarget();
            }
            return target;
        }

        private int CheckNewTarget()
        {
            int result = 0;
            string newTarget= textBox4.Text;
            bool valid = FormValidation.CheckProfileString(newTarget);
            if (!valid)
            {
                ValidationMessage += "Numele către/dinspre nu este valid! ";
            }
            else
            {
                Target addedTarget= new Target();
                if (addedTarget.Add(newTarget, Core.CurrentProfile.Id))
                {
                    result = addedTarget.Id;
                }
            }
            return result;
        }

        private double MakeNegative(double number)
        {
            return number-(number * 2);
        }

    } // end class
}
