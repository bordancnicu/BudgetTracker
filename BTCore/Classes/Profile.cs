using BTCore.Db;
using BudgetTracker;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTCore.Classes
{
    public class Profile : DbComponent
    {
        public DateTime RegistrationDate
        {
            get; private set;
        }
        public string Password
        {
            get; private set;
        }
        public bool IsAuthenticated
        {
            get; private set;
        }
        public Account[] Accounts
        {
            get; private set;
        }
        public bool HasAccounts
        {
            get; private set;
        }
        public Category[] Categories
        {
            get; private set;
        }
        public bool HasCategories
        {
            get; private set;
        }
        public Target[] Targets
        {
            get; private set;
        }
        public bool HasTargets
        {
            get; private set;
        }
        public Currency[] Currencies
        {
            get; private set;
        }


public Profile()
        {
            InitializeProperties();
        }

        public bool Load(string name, string password)
        {
            string whereClause ="name='"+name+"' AND password='"+password+"'";
            DataTable queryResult= Db.RunSelectStatement(DbTable, ALLCOLUMNS, whereClause);
            return LoadClassFromExtractedData(queryResult);
        }

        private bool LoadClassFromExtractedData(DataTable data)
        {
            if (data.Rows.Count > 0)
            {
                foreach (DataRow dr in data.Rows)
                {
                    Id = Convert.ToInt32(dr["id"]);
                    Name = dr["name"].ToString();
                    RegistrationDate = DateTime.Parse(Convert.ToString(dr["registration_date"]));
                    Password = dr["password"].ToString();
                    IsAuthenticated = true;
                }
                GetOwnedAccounts();
                GetOwnedCategories();
                GetOwnedTargets();
                LoadAllCurrencies();
                return true;
            }
            else
            {
                return false;
            }
            }

        public bool Add(string name, string password)
        {
            bool res = false;
            bool nameExist = CheckNameInDb(name);
            if (nameExist == true)
            {
                res = false;
            }
            else
            {
                RegistrationDate = DateTime.Now;
                string date = RegistrationDate.ToString();
                string stm = "INSERT INTO " + DbTable + " (name, registration_date, password) VALUES ('" + name + "', '" + date + "', '" + password + "')";
                Db.RunNonQuery(stm);
                res = true;
            }
            return res;
        }

        public bool CheckNameInDb(string name)
        {
            bool res=false;
            string stm = "SELECT count(name) FROM "+DbTable+" WHERE name='"+name+"'";
            object o=Db.RunScalar(stm);
            int count =Convert.ToInt32(o);
if(count>0)
            {
                res = true;
            }
            return res;
        }

        public bool ChangePassword(string newPassword)
        {
            bool res = false;
if(IsAuthenticated!=true)
            {
                res = false;
            }
            else
            {
                string stm = "UPDATE "+DbTable+" SET password='"+newPassword+"' WHERE id='"+Id+"'";
                Db.RunNonQuery(stm);
                res = true;
            }
            return res;
        }


        protected override void InitializeProperties()
        {
            DbTable = "profiles";
            Db = Core.Db;
            IsAuthenticated = false;
            HasAccounts = false;
            HasCategories = false;
        }


        public void Destroy()
        {
            Core.CurrentProfile = new Profile();
        }

        private void GetOwnedAccounts()
        {
            HasAccounts = false;
            int[] ids= new Account().GetAccountsById(Id);
            int count= ids.Length;
            if (count < 1)
                return;
            Accounts = new Account[count];
for(int i=0; i<count; i++)
            {
                Accounts[i] = new Account(ids[i]);
            }
            HasAccounts = true;
        }

        public void RefreshAccounts()
        {
            GetOwnedAccounts();
        }


        private void GetOwnedCategories()
        {
            HasCategories = false;
            int[] ids = new Category().LoadCategoriesByProfileId(Id);
            int count = ids.Length;
            if (count < 1)
                return;
            Categories = new Category[count];
            for(int i=0; i<count; i++)
            {
                Categories[i] = new Category(ids[i]);
            }
            HasCategories = true;
        }

        public void RefreshCategories()
        {
            GetOwnedCategories();
        }

        private void GetOwnedTargets()
        {
            HasTargets = false;
            int[] ids = new Target().LoadTargetsByProfileId(Id);
            int count = ids.Length;
            if (count < 1)
                return;
            Targets= new Target[count];
            for (int i = 0; i < count; i++)
            {
                Targets[i] = new Target(ids[i]);
            }
            HasTargets = true;
        }

        public void RefreshTargets()
        {
            GetOwnedTargets();
        }

        private void LoadAllCurrencies()
        {
            int[] ids = new Currency().LoadAll();
            int count = ids.Length;
            Currencies = new Currency[count];
            for (int i = 0; i < count; i++)
            {
                Currencies[i] = new Currency(ids[i]);
            }
        }


    }
}
