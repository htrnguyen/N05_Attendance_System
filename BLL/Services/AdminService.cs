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
        // Kiểm tra định dạng file Excel trước khi đồng bộ
        public bool ValidateExcelFormat(string excelFilePath)
        {
            // Định nghĩa định dạng mong đợi: tên sheet và số lượng cột
            Dictionary<string, int> expectedSheetFormats = new Dictionary<string, int>
            {
                { "Roles", 2 },
                { "Users", 6 },
                { "Terms", 4 },
                { "Courses", 4 },
                { "CourseAssignments", 3 },
                { "Classes", 5 },
                { "Groups", 5 },
                { "Enrollments", 3 },
                { "Weeks", 7 },
                { "Announcements", 3 },
                { "Attendances", 9 }
            };
            // Gọi phương thức kiểm tra định dạng
            return _adminDAL.ValidateExcelFormat(excelFilePath, expectedSheetFormats);
        }
        // Đồng bộ dữ liệu từ Excel lên Google Sheets
        public async Task SyncAllSheetsFromExcelToGoogleSheets(string excelFilePath, Dictionary<string, string> sheetMappings)
        {
            // Kiểm tra định dạng file Excel
            if (!ValidateExcelFormat(excelFilePath))
            {
                throw new Exception("File Excel không đúng định dạng");
            }

            try
            {
                // Lấy dữ liệu từ tất cả các sheet trong file Excel
                var excelData = _adminDAL.GetExcelDataFromAllSheets(excelFilePath);

                // Lặp qua từng sheet để đồng bộ lên Google Sheets
                foreach (var sheet in excelData)
                {
                    string sheetName = sheet.Key;
                    List<IList<object>> sheetData = sheet.Value;

                    // Kiểm tra xem sheet này có trong mapping để đồng bộ lên Google Sheets không
                    if (sheetMappings.ContainsKey(sheetName))
                    {
                        string googleSheetRange = sheetMappings[sheetName];

                        // Xoá dữ liệu hiện có trên Google Sheets (nếu cần)
                        await _googleSheetsRepo.ClearSheetData(googleSheetRange);

                        // Cập nhật dữ liệu từ Excel lên Google Sheets
                        await _googleSheetsRepo.UpdateToGoogleSheets(googleSheetRange, sheetData);
                    }
                }
            }
            catch (FormatException ex)
            {
                throw new Exception("Lỗi định dạng dữ liệu trong quá trình đồng bộ lên Google Sheets", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi trong quá trình đồng bộ lên Google Sheets", ex);
            }
        }

    }
}
