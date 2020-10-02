using BTCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTDesigner.WindowProcessors
{
    public class WPShowTransaction
    {

        public static DataTable GetAccountsComboBox()
        {
            DataTable result = DisplayTools.getComboBoxPattern();
            for(int i=0; i<Core.CurrentProfile.Accounts.Length; i++)
            {
                DataRow row = result.NewRow();
                row["value"] = i;
                row["display"] = Core.CurrentProfile.Accounts[i].Name+" - "+ Core.CurrentProfile.Accounts[i].Balance.ToString()+" - "+Core.CurrentProfile.Accounts[i].ACurrency.Simbol;
                result.Rows.Add(row);
            }
            return result;
        }

        public static DataTable GetCategoriesComboBox()
        {
            DataTable result = DisplayTools.getComboBoxPattern();
            DataRow emptyRow= result.NewRow();
            emptyRow["value"] =0;
            emptyRow["display"] ="Toate";
            result.Rows.Add(emptyRow);
            if (Core.CurrentProfile.HasCategories)
            {
                for (int i = 0; i < Core.CurrentProfile.Categories.Length; i++)
                {
                    DataRow row = result.NewRow();
                    row["value"] = Core.CurrentProfile.Categories[i].Id;
                    row["display"] = Core.CurrentProfile.Categories[i].Name;
                    result.Rows.Add(row);
                }
            }
    return result;
        }

        public static DataTable GetTargetsComboBox()
        {
            DataTable result = DisplayTools.getComboBoxPattern();
            DataRow emptyRow = result.NewRow();
            emptyRow["value"] = 0;
            emptyRow["display"] = "Toate";
            result.Rows.Add(emptyRow);
            if (Core.CurrentProfile.HasCategories)
            {
                for (int i = 0; i < Core.CurrentProfile.Targets.Length; i++)
                {
                    DataRow row = result.NewRow();
                    row["value"] = Core.CurrentProfile.Targets[i].Id;
                    row["display"] = Core.CurrentProfile.Targets[i].Name;
                    result.Rows.Add(row);
                }
            }
            return result;
        }

        public static string GetStartDateText()
        {
            return FormatDateToOutput(Core.CurrentProfile.RegistrationDate);
        }

        public static string GetEndDateText()
        {
            return FormatDateToOutput(DateTime.Now);
        }
            

        private static string FormatDateToOutput(DateTime date)
        {
            return date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString();
        }



    }
}
