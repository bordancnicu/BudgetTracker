using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BTCore.Db
{
    public class DbHandler : DbGenericInteraction
    {

        internal DataTable RunSelectStatement(string tableName, string selectedColumns)
        {
            DataTable result = new DataTable();
            string statement = "SELECT " + selectedColumns + " FROM " + tableName;
            RunQuery(result, statement);
            return result;
        }

        internal DataTable RunSelectStatement(String tableName, string columns, string whereClause)
        {
            string statement = "SELECT "+columns+" FROM "+ tableName+" WHERE "+whereClause;
            DataTable result = new DataTable();
            RunQuery(result, statement);
            return result;
        }

        internal int RunInsertStatement(string tableName, string[] columns, string[] values)
        {
            string statement = "INSERT INTO "+tableName+" ";
            string formatedColumns="(";
            string formatedValues = "(";
            int count = columns.Length-1;
            for(int i=0; i<count; i++)
            {
                formatedColumns+= columns[i]+", ";
                formatedValues+= "'"+values[i] + "', ";
            }
            formatedColumns+= columns[count] + ") ";
            formatedValues+= "'"+values[count] + "') ";
            statement += formatedColumns + " VALUES " + formatedValues;
            return RunNonQuery(statement);
        }

        internal int RunInsertStatement(string tableName, string columns, string values)
        {
            string statement = "INSERT INTO "+tableName+columns+" VALUES "+values;
            return RunNonQuery(statement);
        }

        internal int RunUpdateStatement(string tableName, string[] columns, string[] values, string whereClause)
        {
            string statement = "UPDATE " + tableName + " SET ";
            int count = columns.Length-1;
            for (int i = 0; i < count; i++)
            {
                statement += columns[i] + "='" + values[i] + "', ";
            }
            statement += columns[count] + "='" + values[count] + "'";
            statement += " WHERE " + whereClause;
            return RunNonQuery(statement);
        }

        internal int RunDeleteStatement(string tableName, string whereClause)
        {
            string statement = "DELETE FROM " + tableName + " WHERE " + whereClause;
            return RunNonQuery(statement);
        }


    }
}
