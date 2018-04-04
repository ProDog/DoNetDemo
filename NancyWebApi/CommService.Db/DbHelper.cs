using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace CommService.Db
{
    public class DbHelper
    {
        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["OraDataDbConnStr"].ConnectionString;


        public static DataTable ExecuteDbQueryDt(string sql)
        {
            OracleConnection conn = new OracleConnection(ConnString);
            OracleCommand cmd = new OracleCommand(sql, conn);
            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            DataTable table = new DataTable();
            conn.Open();
            adapter.Fill(table);           
            conn.Close();

            return table;
        }

        public static DataSet ExecuteDbQueryDs(List<string> sqlList)
        {
            DataTable table = new DataTable();
            DataSet ds = new DataSet();
            OracleConnection conn = new OracleConnection(ConnString);
            conn.Open();
            for (int i = 0; i < sqlList.Count; i++)
            {
                OracleCommand cmd = new OracleCommand(sqlList[i], conn);
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    ds.Tables.Add(table);
                }
                table = new DataTable();  
            }
            conn.Close();

            return ds;
        }

    }
}
