using BTCore.Db;
using BudgetTracker;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;


namespace BTCore.Classes
{
    public class Account : DbComponent
    {
        public double Balance
        {
        get; set;
}
    public Currency ACurrency
    {
    get; set;
        }
        public int Scope
{
get; set;
    }
        public DateTime CreationDate
{
get; set;
    }
        public Transaction[] Transactions
        {
            get; private set;
        }
        public bool HasTransactions
        {
            get; private set;
        }

        public Account()
        {
            InitializeProperties();
            Db = Core.Db;
        }

        public Account(int id)
        {
            InitializeProperties();
            Load(id);
        }

        internal void Load(int id)
        {
            string whereClause ="id='"+id+"'";
            DataTable data = Db.RunSelectStatement(DbTable, ALLCOLUMNS, whereClause);
            DataRow row = data.Rows[0];
                Id = Convert.ToInt32(row["id"]);
                Name =row["name"].ToString();
                Balance = Convert.ToDouble(row["balance"]);
                ACurrency = new Currency(Convert.ToInt32(row["currency_id"]));
                Scope = Convert.ToInt32(row["scope"]);
                CreationDate = DateTime.Parse(Convert.ToString(row["date"]));
            LoadAllTransactions();
            if (Transactions.Length > 0)
                HasTransactions = true;
        }

private void LoadAllTransactions()
        {
            int[] ids=new Transaction().LoadAllByAccountId(Id);
            int count = ids.Length;
            Transactions = new Transaction[count];
for(int i=0; i<count; i++)
            {
                Transactions[i] = new Transaction(ids[i]);
            }
        }

        internal void RefreshTransactions()
        {
            LoadAllTransactions();
        }

        public bool Add(int profileId, string name, double balance, int currencyId, int scope)
        {
            bool result= false;
            string date = DateTime.Now.ToString();
            string columns="(profile_id, name, balance, currency_id, scope, date)";
            string values= "('"+profileId+"', '"+name+"', '"+balance+"', '"+currencyId+"', '"+scope+"', '"+date+"')";
            int afectedRows = Db.RunInsertStatement(DbTable, columns, values);
            if (afectedRows > 0)
            {
                Core.CurrentProfile.RefreshAccounts();
                result= true;
            }
            return result;
        }


public bool Delete(int id)
        {
            bool result= false;
            string whereClause = "id='" + Id + "'";
            int afectedRows = Db.RunDeleteStatement(DbTable, whereClause);
            if (afectedRows> 0)
                result= true;
            return result;
        }

public static DataTable GetScopes()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(String));
            dt.Columns.Add("name", typeof(String));
            DataRow row1 = dt.NewRow();
            DataRow row2 = dt.NewRow();
            row1["id"] = Convert.ToString(1);
            row1["name"]="Cont curent";
            dt.Rows.Add(row1);
            row2["id"] = Convert.ToString(2);
            row2["name"] ="Cont de economii";
            dt.Rows.Add(row2);
            return dt;
        }

        internal DataTable LoadAllAccountsForProfile(int profileId)
        {
            string whereClause = "id='"+profileId+"'";
            return Db.RunSelectStatement(DbTable, ALLCOLUMNS, whereClause);
        }


        protected override void InitializeProperties()
        {
            Db = Core.Db;
            DbTable = "accounts";
            HasTransactions = false;
            }

        internal int[] GetAccountsById(int profileId)
        {
            string whereClause ="profile_id='"+profileId+"'";
            string columns = "(id)";
            DataTable data = Db.RunSelectStatement(DbTable, columns, whereClause);
            int[] result = new int[data.Rows.Count];
            for(int i=0; i<data.Rows.Count; i++)
            {
                result[i] =Convert.ToInt32(data.Rows[i]["id"]);
            }
            return result;
        }

        internal bool UpdateBalance(int id, double transactionValue)
        {
            Load(id);
                Balance += transactionValue;
            string[] columns = { "balance" };
            string[] values = { Balance.ToString() };
            string whereClause = "id='"+Id+"'";
            if (Db.RunUpdateStatement(DbTable, columns, values, whereClause) > 0)
                return true;
            else
                return false;
        }

        public DataTable GetStatisticsView(TransactionsView view)
        {
            DataTable result=GetStatisticsTable();
            FillStatisticsTable(result, view);
            return result;
        }

        private void FillStatisticsTable(DataTable table, TransactionsView view)
        {
            double total = 0;
            double positive = 0;
            double negative = 0;
            foreach (Transaction transaction in Transactions)
            {
                if (FilterTransactionByView(transaction, view))
                {
                    double value = transaction.Value;
                    total += transaction.Value;
                    if (value > 0)
                        positive += value;
                    else
                        negative += value;
                }
            }
            DataRow row = table.NewRow();
            row["total"] = total.ToString();
            row["positive"] = positive.ToString();
            row["negative"] = negative.ToString();
            table.Rows.Add(row);
        }

private DataTable GetStatisticsTable()
        {
            DataTable result = new DataTable();
            result.Columns.Add("total", typeof(string));
            result.Columns.Add("positive", typeof(string));
            result.Columns.Add("negative", typeof(string));
            return result;
        }

        public DataTable GetTransactionsView(TransactionsView view)
        {
            DataTable result = GetTransactionsViewTable();
            FillTransactionsViewTable(result, view);
            return result;
        }

        private DataTable GetTransactionsViewTable()
        {
            DataTable result = new DataTable();
            result.Columns.Add("Name", typeof(string));
            result.Columns.Add("Value", typeof(string));
            result.Columns.Add("Date", typeof(string));
            result.Columns.Add("Category", typeof(string));
            result.Columns.Add("Target", typeof(string));
            return result;
        }

        private void FillTransactionsViewTable(DataTable table, TransactionsView view)
        {
foreach(Transaction transaction in Transactions)
            {
                if (FilterTransactionByView(transaction, view))
                {
                    DataRow row = table.NewRow();
                    row["Name"] = transaction.Name;
                    row["Value"] = transaction.Value;
                    row["Date"] = transaction.Date.ToString();
                    row["Category"] = GetNameForCategoryId(transaction.CategoryId);
                    row["Target"] = GetNameForTargetId(transaction.TargetId);
                    table.Rows.Add(row);
                                        }
            }
        }

                private bool FilterTransactionByView(Transaction transaction, TransactionsView view)
        {
            if(CheckCategoryId(transaction.CategoryId, view.CategoryId)
                &&CheckTargetId(transaction.TargetId, view.TargetId)
                &&CheckStartDate(transaction.Date, view.StartDate)
                &&CheckEndDate(transaction.Date, view.EndDate))
            return true;
            else
            return false;
        }

        private bool CheckEndDate(DateTime transactionDate, DateTime viewDate)
        {
            if (viewDate == default(DateTime))
                return true;
            if (transactionDate.CompareTo(viewDate) < 0)
                return true;
            return false;
        }

        private bool CheckStartDate(DateTime transactionDate, DateTime viewDate)
        {
            if (viewDate == default(DateTime))
                return true;
            if (transactionDate.CompareTo(viewDate) > 0)
                return true;
            return false;
        }

        private bool CheckTargetId(int transactionId, int viewId)
        {
            if (viewId == default(int))
                return true;
            if (transactionId == viewId)
                return true;
            return false;
        }

        private bool CheckCategoryId(int transactionId, int viewId)
        {
            if (viewId==default(int))
                return true;
            if (transactionId == viewId)
                return true;
            return false;
        }

        
    private string GetNameForCategoryId(int id)
    {
        string name="";
        foreach(Category ct in Core.CurrentProfile.Categories)
        {
if(ct.Id==id)
            {
                name = ct.Name;
                break;
            }
        }
            return name;
        }
                    
    private string GetNameForTargetId(int id)
    {
        string name = "";
        foreach (Target tg in Core.CurrentProfile.Targets)
        {
            if (tg.Id == id)
            {
                name = tg.Name;
                break;
            }
        }
            return name;
        }



}
}
