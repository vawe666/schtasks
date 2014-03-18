using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ConsoleApplication.SqlClient;

namespace ConsoleApplication.Data
{
    public class Dealer
    {
        private static DatabaseProperty connupCollect = DBSettings.GetDatabaseProperty(ConnString.upCollect);

        public static DataTable Select()
        {
            SqlQuery q = new SqlQuery();
            q.CommandText = string.Format(@" select 1");
            q.CommandType = CommandType.Text;

            return SqlDataAccess.ExecuteDataset(q, connupCollect).Tables[0];
        }
    }
}
