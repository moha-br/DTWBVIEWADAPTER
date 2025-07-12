//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DTWBVIEWADAPTER
//{
//    internal class Class1
//    {
//    }
//}
using System;
using System.Data;
using System.Data.SQLite;

public class SQLiteHelper
{
    private readonly string _connectionString;

    public SQLiteHelper(string dbPath)
    {
        _connectionString = $"Data Source={dbPath};Version=3;";
    }

    public DataTable ExecuteSelectQuery(string query)
    {
        var dataTable = new DataTable();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(query, connection))
            using (var adapter = new SQLiteDataAdapter(command))
            {
                adapter.Fill(dataTable);
            }
        }

        return dataTable;
    }
}


