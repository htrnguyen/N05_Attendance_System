using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SQLite;
using OfficeOpenXml;
using System.IO;


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
        public bool ValidateExcelFormat(string excelFilePath, Dictionary<string, int> expectedSheetFormats)
        {
            // Thiết lập chế độ License cho EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Sử dụng EPPlus để mở file Excel
            using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                foreach (var sheet in expectedSheetFormats)
                {
                    // Kiểm tra nếu sheet mong đợi có trong file Excel không
                    var worksheet = package.Workbook.Worksheets[sheet.Key];
                    if (worksheet == null)
                    {
                        return false; // Sheet không tồn tại
                    }

                    // Đếm số cột thực sự có dữ liệu, tối đa bằng số cột mong đợi
                    int actualColCount = 0;
                    for (int col = 1; col <= worksheet.Dimension.Columns && col <= sheet.Value; col++)
                    {
                        bool hasData = false;
                        for (int row = 1; row <= worksheet.Dimension.Rows; row++)
                        {
                            if (!string.IsNullOrEmpty(worksheet.Cells[row, col].Text))
                            {
                                hasData = true;
                                break;
                            }
                        }
                        if (hasData)
                        {
                            actualColCount++;
                        }
                    }
                    //Console.WriteLine($"Sheet: {sheet.Key}, Expected Columns: {sheet.Value}, Actual Columns (with data): {actualColCount}");

                    if (actualColCount != sheet.Value)
                    {
                        //Console.WriteLine($"Sheet '{sheet.Key}' có số lượng cột không đúng. Expected: {sheet.Value}, Actual: {actualColCount}");
                        return false; // Số lượng cột không đúng
                    }
                }
            }

            // Tất cả các sheet đều khớp với định dạng mong đợi
            Console.WriteLine("Tất cả các sheet đều đúng định dạng.");
            return true;
        }
        // Đọc dữ liệu từ tất cả các sheet trong file Excel
        public Dictionary<string, List<IList<object>>> GetExcelDataFromAllSheets(string excelFilePath)
        {
            // Thiết lập chế độ License cho EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            Dictionary<string, List<IList<object>>> excelData = new Dictionary<string, List<IList<object>>>();

            // Sử dụng EPPlus để mở file Excel
            using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                // Lặp qua tất cả các sheet
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    List<IList<object>> sheetData = new List<IList<object>>();
                    int rowCount = worksheet.Dimension.Rows; // Tổng số dòng
                    int colCount = worksheet.Dimension.Columns; // Tổng số cột (cả cột có thể rỗng)

                    for (int row = 1; row <= rowCount; row++)
                    {
                        List<object> rowData = new List<object>();

                        // Chỉ thêm cột có dữ liệu
                        for (int col = 1; col <= colCount; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Text;
                            if (!string.IsNullOrEmpty(cellValue)) // Kiểm tra xem cột có dữ liệu hay không
                            {
                                rowData.Add(cellValue);
                            }
                        }

                        // Nếu có dữ liệu trong hàng thì mới thêm vào sheetData
                        if (rowData.Count > 0)
                        {
                            sheetData.Add(rowData);
                        }
                    }

                    // Thêm dữ liệu của sheet này vào dictionary với key là tên sheet nếu có dữ liệu
                    if (sheetData.Count > 0)
                    {
                        excelData[worksheet.Name] = sheetData;
                    }
                }
            }

            return excelData;
        }


    }
}

