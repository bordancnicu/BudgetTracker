using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCore.Db
{
    public class DbGenericInteraction
    {
        internal SQLiteConnection Connection;
        internal string DbName;
        internal string DbLocation;

        public DbGenericInteraction()
        {
            InitializeProperties();
            CheckDbFile();
            OpenDbConnection();
            new DbDesigner(this);
        }

        private void CheckDbFile()
        {
            if (!File.Exists(DbLocation))
            {
                SQLiteConnection.CreateFile(DbName);
            }
        }

        internal void OpenDbConnection()
        {
            string statement= "Data Source=" + DbLocation + ";Version=3;UTF8Encoding=True;";
            Connection = new SQLiteConnection(statement);
            Connection.Open();
        }

        internal void CloseDbConnection()
        {
            Connection.Close();
        }


        internal void RunQuery(DataTable result, string statement)
        {
            SQLiteCommand command= new SQLiteCommand(statement, Connection);
            SQLiteDataAdapter dataAdapter= new SQLiteDataAdapter();
            dataAdapter.SelectCommand = command;
            dataAdapter.Fill(result);
        }

        internal int RunNonQuery(string statement)
        {
            int afectedRows= 0;
            SQLiteCommand command= new SQLiteCommand(statement, Connection);
            afectedRows= command.ExecuteNonQuery();
            return afectedRows;
        }

        private void InitializeProperties()
        {
            DbName = "budgettracker.db";
            DbLocation = Directory.GetCurrentDirectory() + @"\" + DbName;
        }

        internal bool ExistsTable(string tableName)
        {
            string stm = @"SELECT * FROM sqlite_master where type='table' AND name='" + tableName + "'";
            SQLiteCommand cmd = new SQLiteCommand(stm, Connection);
            SQLiteDataReader sdr = cmd.ExecuteReader();
            return sdr.HasRows;
        }

        internal int CountRowsInTable(string table)
        {
            string stm = "SELECT count(*) FROM " + table;
            object o=RunScalar(stm);
            return Convert.ToInt32(o);
        }


        internal object RunScalar(string stm)
        {
            SQLiteCommand scmd = new SQLiteCommand(stm, Connection);
            object res = scmd.ExecuteScalar();
            return res;
        }


    }
}
