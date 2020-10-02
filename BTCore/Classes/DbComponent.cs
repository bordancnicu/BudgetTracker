using BTCore.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCore.Classes
{
    public abstract class DbComponent
    {
        public int Id
        {
            get; protected set;
        }
        public string Name
        {
            get; protected set;
        }
        protected string DbTable
        {
            get; set;
        }
        internal DbHandler Db;
        internal const string ALLCOLUMNS = "*";


        int GetAllIds(int ownerId)
        {
            return 1;
        }

        internal string PrepareGetAllIdsStatement()
        {
            // string statement;
            return "";
        }



        protected abstract void InitializeProperties();
    }
}
