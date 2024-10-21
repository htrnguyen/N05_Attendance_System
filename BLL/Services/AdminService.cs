using DAL.Repositories;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BLL.Services
{
    public class AdminService
    {
        private readonly AdminDAL _adminDAL;
        private readonly GoogleSheetsRepository _googleSheetsRepo;

        public AdminService()
        {
            this._adminDAL = new AdminDAL();
            this._googleSheetsRepo = new GoogleSheetsRepository();
        }
        // Lấy danh sách tên bảng
        public List<string> GetTableNames()
        {
            return _adminDAL.GetTableNames();
        }
        // Lấy dữ liệu từ bảng
        public DataTable GetTableData(string tableName)
        {
            return _adminDAL.GetTableData(tableName);
        }
    }
}
