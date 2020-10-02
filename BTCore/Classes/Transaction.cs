using BTCore;
using BTCore.Classes;
using BTCore.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker
{
    public class Transaction : DbComponent
    {
        public double Value
        {
            get; private set;
        }
        public DateTime Date
        {
            get; private set;
        }
        public int Recurent 
        {
            get; private set;
        }
        public int CategoryId
        {
            get; private set;
        }
        public int TargetId
        {
            get; private set;
        }


        public Transaction()
        {
            InitializeProperties();
        }

        public Transaction(int id)
        {
            InitializeProperties();
            Load(id);
        }


        public void Load(int id)
        {
            string whereClause = "id='"+id+"'";
            DataTable data=Db.RunSelectStatement(DbTable, ALLCOLUMNS, whereClause);
            int count = data.Rows.Count;
            if (count < 1)
                return;
            DataRow row = data.Rows[0];
            Id = Convert.ToInt32(row["id"]);
            Name = Convert.ToString(row["name"]);
            Value= Convert.ToDouble(row["value"]);
            Date = DateTime.Parse(Convert.ToString(row["date"]));
            Recurent = Convert.ToInt32(row["recurent"]);
            CategoryId = Convert.ToInt32(row["category_id"]);
            TargetId = Convert.ToInt32(row["target_id"]);
        }

        public bool Add(int accountId, int profileId, string name, double value, int recurent, int categoryId, int targetId)
        {
            bool result = false;
            string date = DateTime.Now.ToString();
            string columns = "(account_id, profile_id, name, value, date, recurent, category_id, target_id)";
            string values = "('"+accountId+"', '"+profileId+"', '"+name+ "', '"+value+ "', '"+date+ "', '"+recurent+ "', '"+categoryId+ "', '"+targetId+"')";
            int afectedRows= Db.RunInsertStatement(DbTable, columns, values);
if(afectedRows>0)
            {
                result = true;
                new Account().UpdateBalance(accountId, value);
                Core.CurrentProfile.RefreshAccounts();
            }
            return result;
        }

        public int[] LoadAllByAccountId(int id)
        {
            string columns = "(id)";
            string whereClause= "account_id='"+id+"'";
            DataTable data = Db.RunSelectStatement(DbTable, columns, whereClause);
            int count = data.Rows.Count;
            int[] result =new int[count];
            for(int i=0; i<count; i++)
            {
                result[i] = Convert.ToInt32(data.Rows[i]["id"]);
            }
            return result;
        }


        protected override void InitializeProperties()
        {
            Db = Core.Db;
            DbTable = "transactions";
        }

    }
}
