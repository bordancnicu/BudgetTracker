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
    public class Target : DbComponent
    {
            public int ProfileId
            {
                get; set;
            }

            public Target()
            {
                InitializeProperties();
                Db = Core.Db;
            }

            public Target(int id)
            {
                InitializeProperties();
                Db = Core.Db;
                Load(id);
            }

            public bool Add(string name, int profileId)
            {
                bool result = false;
                string columns = "(name, profile_id)";
                string values = "('" + name + "', '" + profileId + "')";
                int afectedRows = Db.RunInsertStatement(DbTable, columns, values);
                if (afectedRows > 0)
                {
                    result = true;
                Load(name);
                Core.CurrentProfile.RefreshTargets();
            }
                return result;
            }

            public void Load(int id)
            {
                string whereClause = "id='" + id + "'";
                DataTable data = Db.RunSelectStatement(DbTable, ALLCOLUMNS, whereClause);
                int count = data.Rows.Count;
                if (count < 1)
                    return;
                DataRow row = data.Rows[0];
                Id = Convert.ToInt32(row["id"]);
                Name = Convert.ToString(row["name"]);
                ProfileId = Convert.ToInt32(row["profile_id"]);
            }

            public void Load(string name)
            {
                string whereClause = "name='" + name + "'";
                DataTable data = Db.RunSelectStatement(DbTable, ALLCOLUMNS, whereClause);
                int count = data.Rows.Count;
                if (count < 1)
                    return;
                DataRow row = data.Rows[0];
                Id = Convert.ToInt32(row["id"]);
                Name = Convert.ToString(row["name"]);
                ProfileId = Convert.ToInt32(row["profile_id"]);
            }


            public int[] LoadTargetsByProfileId(int profileId)
            {
                string whereClause = "profile_id='" + profileId + "'";
                string columns = "(id)";
                DataTable data = Db.RunSelectStatement(DbTable, columns, whereClause);
                int count = data.Rows.Count;
                int[] result = new int[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = Convert.ToInt32(data.Rows[i]["id"]);
                }
                return result;
            }


            public bool Delete(int id)
            {
                bool result = false;
                string whereClause = "id='" + id + "'";
                int count = Db.RunDeleteStatement(DbTable, whereClause);
                if (count > 0)
                    result = true;
                return result;
            }

            protected override void InitializeProperties()
            {
                DbTable = "targets";
            }

        }

}
