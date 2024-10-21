using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SQLite;


namespace DAL.Repositories
{
    public class AdminDAL
    {
        private string _connectionString;

        public AdminDAL()
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        // Lấy danh sách tên bảng
        public List<string> GetTableNames()
        {
            List<string> tableNames = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                DataTable dt = conn.GetSchema("Tables");
                foreach (DataRow row in dt.Rows)
                {
                    tableNames.Add(row["TABLE_NAME"].ToString());
                }   
                return tableNames;
            }
        }
        // Lấy dữ liệu từ bảng 
        public DataTable GetTableData(string tableName)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = $"SELECT * FROM {tableName}";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}

