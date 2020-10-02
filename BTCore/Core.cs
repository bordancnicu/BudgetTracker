using BTCore.Classes;
using BTCore.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCore
{
    public class Core
    {
        internal static DbHandler Db
        {
            get; private set;
        }
        public static string Test
        {
            get; set;
        }
        public static Profile CurrentProfile
        {
            get; internal set;
        }

        public Core()
        {
            Db = new DbHandler();
            CurrentProfile = new Profile();
            //Test = CurrentProfile.GetTableName();
        }



    }
}
