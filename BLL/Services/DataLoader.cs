using DAL;
using System;
using System.Data.SQLite;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

public class DataLoader
{
    private readonly GoogleSheetsRepository _googleSheetsRepo;
    private readonly SQLiteRepository _sqliteRepo;
    private readonly string _connectionString;

    public DataLoader()
    {
        this._googleSheetsRepo = new GoogleSheetsRepository();
        this._sqliteRepo = new SQLiteRepository();
        this._connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    }

    public async Task LoadDataAsync()
    {
        await ExecuteWithRetryAsync(async () =>
        {
            // Lấy dữ liệu từ Google Sheets
            var rolesData = await _googleSheetsRepo.GetSheetData("Roles!A1:B");
            var usersData = await _googleSheetsRepo.GetSheetData("Users!A1:F");
            var termsData = await _googleSheetsRepo.GetSheetData("Terms!A1:D");
            var coursesData = await _googleSheetsRepo.GetSheetData("Courses!A1:D");
            var courseAssignmentsData = await _googleSheetsRepo.GetSheetData("CourseAssignments!A1:C");
            var classesData = await _googleSheetsRepo.GetSheetData("Classes!A1:E");
            var groupsData = await _googleSheetsRepo.GetSheetData("Groups!A1:E");
            var enrollmentsData = await _googleSheetsRepo.GetSheetData("Enrollments!A1:C");
            var weeksData = await _googleSheetsRepo.GetSheetData("Weeks!A1:G");
            var announcementsData = await _googleSheetsRepo.GetSheetData("Announcements!A1:C");
            var attendancesData = await _googleSheetsRepo.GetSheetData("Attendances!A1:H");

            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();

                // Tối ưu hóa tốc độ ghi
                await SetPragmaSettingsAsync(command, false);

                // Tạo cơ sở dữ liệu và xóa dữ liệu cũ
                _sqliteRepo.CreateDatabase();
                _sqliteRepo.ClearDatabase();

                // Chèn dữ liệu vào các bảng tương ứng
                await _sqliteRepo.InsertRoleData(rolesData);
                await _sqliteRepo.InsertUserData(usersData);
                await _sqliteRepo.InsertTermData(termsData);
                await _sqliteRepo.InsertCourseData(coursesData);
                await _sqliteRepo.InsertCourseAssignmentData(courseAssignmentsData);
                await _sqliteRepo.InsertClassData(classesData);
                await _sqliteRepo.InsertGroupData(groupsData);
                await _sqliteRepo.InsertEnrollmentData(enrollmentsData);
                await _sqliteRepo.InsertWeekData(weeksData);
                await _sqliteRepo.InsertAnnouncementData(announcementsData);
                await _sqliteRepo.InsertAttendanceData(attendancesData);

                // Khôi phục thiết lập PRAGMA
                await SetPragmaSettingsAsync(command, true);
            }
        });
    }

    // Load bảng Classes và Weeks
    public async Task LoadClassesAndWeeksAsync()
    {
        await ExecuteWithRetryAsync(async () =>
        {
            var classesData = await _googleSheetsRepo.GetSheetData("Classes!A1:E");
            var weeksData = await _googleSheetsRepo.GetSheetData("Weeks!A1:F");

            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();

                // Tối ưu hóa tốc độ ghi
                await SetPragmaSettingsAsync(command, false);

                // Xóa dữ liệu cũ
                _sqliteRepo.ClearClassesAndWeeks();

                // Chèn dữ liệu vào các bảng tương ứng
                await _sqliteRepo.InsertClassData(classesData);
                await _sqliteRepo.InsertWeekData(weeksData);

                // Khôi phục thiết lập PRAGMA
                await SetPragmaSettingsAsync(command, true);
            }
        });
    }

    // Phương thức đặt các thiết lập PRAGMA cho SQLite
    private async Task SetPragmaSettingsAsync(SQLiteCommand command, bool isRestore)
    {
        if (isRestore)
        {
            command.CommandText = @"
            PRAGMA synchronous = NORMAL;
            PRAGMA journal_mode = WAL;
            PRAGMA foreign_keys = ON;";
        }
        else
        {
            command.CommandText = @"
            PRAGMA synchronous = OFF;
            PRAGMA journal_mode = MEMORY;
            PRAGMA foreign_keys = OFF;";
        }
        await command.ExecuteNonQueryAsync();
    }

    // Retry logic để thử lại các thao tác nếu gặp lỗi "database is locked"
    public async Task ExecuteWithRetryAsync(Func<Task> action, int maxRetries = 3)
    {
        int retryCount = 0;
        while (true)
        {
            try
            {
                await action();
                break;
            }
            catch (SQLiteException ex) when (ex.Message.Contains("database is locked") && retryCount < maxRetries)
            {
                retryCount++;
                int delay = (int)Math.Pow(2, retryCount) * 1000;  // Tăng thời gian chờ theo cấp số nhân
                await Task.Delay(delay);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                break;
            }
        }
    }
}
