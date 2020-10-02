using BTCore.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BTCore.Classes
{
    public class Currency : DbComponent
    {
            public string Simbol
        {
            get; private set;
        }


        public Currency()
        {
            InitializeProperties();
        }

        public Currency(int id)
        {
            InitializeProperties();
                Load(id);
        }

        public bool Load(int id)
        {
            bool result= false;
            string whereClause = "id='"+id+"'";
            DataTable data=Db.RunSelectStatement(DbTable, ALLCOLUMNS, whereClause);
            if (data.Rows.Count > 0)
            {
                result= true;
                DataRow row= data.Rows[0];
                Id = Convert.ToInt32(row["id"]);
                Name = Convert.ToString(row["name"]);
                Simbol= Convert.ToString(row["simbol"]);
            }
            return result;
        }

        public int[] LoadAll()
        {
            string columns = "(id)";
            DataTable data= Db.RunSelectStatement(DbTable, columns);
            int count = data.Rows.Count;
            int[] result = new int[count];
            for(int i=0; i<count; i++)
                    {
                result[i]= Convert.ToInt32(data.Rows[i]["id"]);
            }
            return result;
        }

        private void CheckCurrencies()
        {
            int rows = Db.CountRowsInTable(DbTable);
if(rows<1)
            {
                FillCurrencies();
            }
        }

        private void FillCurrencies()
        {
            string columns = "(name, simbol)";
                string values="('Leu', 'RON'), " +
                "('Euro', 'EUR'), " +
                "('Dolar', 'USD'), " +
                "('Forint', 'HUF');";
            Db.RunInsertStatement(DbTable, columns, values);
        }

        protected override void InitializeProperties()
        {
            Db = Core.Db;
            DbTable = "currencies";
            CheckCurrencies();
        }


    }
}
