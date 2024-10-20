using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Configuration; // Thêm namespace này để dùng ConfigurationManager
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public class GoogleSheetsRepository
    {
        private static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private static string _spreadsheetId;
        private readonly string jsonPath;
        private readonly SheetsService _sheetsService;
        private const int MaxRetryAttempts = 3;  // Số lần retry tối đa khi có lỗi

        public GoogleSheetsRepository()
        {
            // Khởi tạo
            string googleApiJson = ConfigurationManager.AppSettings["GoogleApiJson"];
            _spreadsheetId = ConfigurationManager.AppSettings["GoogleSheetId"];


            // Lưu JSON vào file tạm
            jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "temp_google_credentials.json");
            File.WriteAllText(jsonPath, googleApiJson);

            // Khởi tạo credential từ file JSON
            GoogleCredential credential;
            using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(SheetsService.Scope.Spreadsheets);
            }

            // Khởi tạo SheetsService
            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ConfigurationManager.AppSettings["ApplicationName"]
            });
        }
        // Phương thức lấy dữ liệu từ một phạm vi của bảng tính
        public async Task<IList<IList<object>>> GetSheetData(string sheetRange)
        {
            return await RetryOnExceptionAsync(MaxRetryAttempts, async () =>
            {
                var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, sheetRange);
                var response = await request.ExecuteAsync();
                return response.Values;
            });
        }
        // Phương thức cập nhật lên Google Sheets
        public async Task UpdateToGoogleSheets(string sheetRange, List<IList<object>> values)
        {
            var valueRange = new ValueRange
            {
                Values = values
            };

            var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, _spreadsheetId, sheetRange);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

            await RetryOnExceptionAsync(MaxRetryAttempts, async () =>
            {
                await updateRequest.ExecuteAsync();
                return Task.CompletedTask;
            });
        }
        // Hàm hỗ trợ để retry nếu gặp lỗi
        private async Task<T> RetryOnExceptionAsync<T>(int maxAttempts, Func<Task<T>> operation)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex)
                {
                    attempts++;
                    if (attempts >= maxAttempts)
                        throw new Exception($"Thất bại sau {attempts} lần thử", ex);
                }
            }
        }
    }
}
