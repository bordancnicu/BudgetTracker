using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCore.Db
{
    internal class DbDesigner
    {
        protected DbGenericInteraction Db;

        internal DbDesigner(DbGenericInteraction db)
        {
            Db = db;
            CheckTables();
        }

        private void CheckTables()
        {
            if (Db.ExistsTable("profiles"))
            {
                return;
            }
            string[] tablesStatement = GetTablesStatement();
            foreach(string stm in tablesStatement)
            {
                Db.RunNonQuery(stm);
            }
        }


        private string[] GetTablesStatement()
        {
            string[] statements = new string[6];
            statements[0] = "CREATE TABLE profiles(" +
    "id INTEGER PRIMARY KEY, " +
    "name TEXT NOT NULL UNIQUE, " +
    "registration_date TEXT NOT NULL, " +
    "password TEXT NOT NULL)";
            statements[1] = "CREATE TABLE currencies(" +
    "id INTEGER PRIMARY KEY, " +
    "name TEXT UNIQUE NOT NULL, " +
    "simbol TEXT UNIQUE NOT NULL)"; ;
            statements[2] = "CREATE TABLE categories(" +
                "id INTEGER PRIMARY KEY, " +
                "name TEXT NOT NULL, " +
                "profile_id INTEGER REFERENCES profiles (id) ON DELETE CASCADE)";
            statements[3] = "CREATE TABLE accounts (" +
"id INTEGER PRIMARY KEY, " +
"profile_id INTEGER REFERENCES profiles (id) ON DELETE CASCADE, " +
"name TEXT NOT NULL, " +
"balance REAL NOT NULL, " +
"currency_id INTEGER REFERENCES currencies(id), " +
"scope INTEGER NOT NULL, " +
"date TEXT NOT NULL) ";
            statements[4] = "CREATE TABLE targets (" +
                "id INTEGER PRIMARY KEY, " +
                "name TEXT NOT NULL UNIQUE, " +
                "profile_id INTEGER REFERENCES profiles(id) ON DELETE CASCADE) ";
            statements[5] = "CREATE TABLE transactions (" +
                "id INTEGER PRIMARY KEY, " +
                "account_id INTEGER REFERENCES accounts(id), " +
                "profile_id INTEGER REFERENCES profiles(id) ON DELETE CASCADE, " +
                "name TEXT NOT NULL, " +
                "value REAL NOT NULL, " +
                "date TEXT NOT NULL, " +
"recurent INTEGER NOT NULL, " +
                                "target_id INTEGER REFERENCES targets(id), " +
                                "category_id INTEGER REFERENCES categories(id))";
            return statements;
        }


    }
}
