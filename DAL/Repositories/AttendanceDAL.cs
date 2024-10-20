using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data.SQLite;
using System.Configuration;

namespace DAL.Repositories
{
    public class AttendanceDAL
    {
        private string _connectionString;

        public AttendanceDAL()
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        // Cập nhật toạ độ lên Database
        public bool UpdateCoordinates(int ClassID, int CourseID, string Latitude, string Longitude)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                    UPDATE Classes
                    SET 
                        latitude = @Latitude, 
                        longitude = @Longitude
                    WHERE 
                        classID = @ClassID
                        AND classID IN (
                            SELECT classID 
                            FROM Groups g
                            JOIN Courses c ON g.courseID = c.courseID
                            WHERE c.courseID = @CourseID
                        );
                    ";
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                    cmd.Parameters.AddWithValue("@CourseID", CourseID);
                    cmd.Parameters.AddWithValue("@Latitude", Latitude);
                    cmd.Parameters.AddWithValue("@Longitude", Longitude);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        // Cập nhật trạng thái link điểm danh
        public bool UpdateAttendanceLinkStatus(int WeekID, int CourseID, int TeacherID)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                        UPDATE Weeks
                        SET isAttendanceLinkCreated = 0
                        WHERE weekID = @WeekID
                          AND courseID = @CourseID
                          AND courseID IN (
                              SELECT ca.courseID
                              FROM CourseAssignments ca
                              WHERE ca.teacherID = @TeacherID
                          );
                    ";
                    cmd.Parameters.AddWithValue("@WeekID", WeekID);
                    cmd.Parameters.AddWithValue("@CourseID", CourseID);
                    cmd.Parameters.AddWithValue("@TeacherID", TeacherID);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        // Đồng bộ bảng Classes lên GoogleSheet
        public async Task SyncClassessDataToGoogleSheet(GoogleSheetsRepository googleSheetsRepo)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT Latitude, Longitude FROM Classes;
                ";

                var reader = await command.ExecuteReaderAsync();
                var latitudes = new List<string>();
                var longitudes = new List<string>();

                while (await reader.ReadAsync())
                {
                    latitudes.Add(reader["Latitude"].ToString());
                    longitudes.Add(reader["Longitude"].ToString());
                }

                // Chuẩn bị danh sách giá trị để cập nhật hàng loạt
                var values_lat = new List<IList<object>>();
                var values_lon = new List<IList<object>>();

                foreach (var latitude in latitudes)
                {
                    values_lat.Add(new List<object> { latitude });
                }

                foreach (var longitude in longitudes)
                {
                    values_lon.Add(new List<object> { longitude });
                }

                var range_lat = $"Classes!D2:D{latitudes.Count + 1}";
                var range_lon = $"Classes!E2:E{longitudes.Count + 1}";

                await googleSheetsRepo.UpdateToGoogleSheets(range_lat, values_lat);
                await googleSheetsRepo.UpdateToGoogleSheets(range_lon, values_lon);

            }
        }
        // Đồng bộ bảng Weeks lên GoogleSheet
        public async Task SyncWeeksDataToGoogleSheet(GoogleSheetsRepository googleSheetsRepo)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT IsAttendanceLinkCreated FROM Weeks;
                ";

                var reader = await command.ExecuteReaderAsync();
                var IsAttendanceLinkCreateds = new List<int>();
                while (await reader.ReadAsync())
                {
                    IsAttendanceLinkCreateds.Add(Convert.ToInt32(reader["IsAttendanceLinkCreated"]));
                }

                // Chuẩn bị danh sách giá trị để cập nhật hàng loạt
                var values = new List<IList<object>>();
                foreach (var isAttendanceLinkCreated in IsAttendanceLinkCreateds)
                {
                    values.Add(new List<object> { isAttendanceLinkCreated });
                }

                // Cập nhật mật khẩu lên Google Sheets với tất cả hàng trong một lần
                var range = $"Weeks!F2:F{IsAttendanceLinkCreateds.Count + 1}";
                await googleSheetsRepo.UpdateToGoogleSheets(range, values);
            }
        }
        // Lấy toạ độ của lớp
        public List<string> GetClassCoordinates(int ClassID)
        {
            List<string> coordinates = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                    SELECT 
                        latitude,
                        longitude
                    FROM 
                        Classes
                    WHERE 
                        classID = @ClassID
                    ";
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            coordinates.Add(reader["latitude"].ToString());
                            coordinates.Add(reader["longitude"].ToString());
                        }
                    }
                }
            }
            return coordinates;
        }
        // Cập nhật trạng thái điểm danh của sinh viên
        public bool UpdateAttendance(int StudentID, int WeekID, string Status, string Latiude, string Longitude, string IpAddress)
        {
            DateTime dateTime = DateTime.Now;
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                        INSERT INTO Attendances (StudentID, WeekID, Status, CheckedInAt, Latitude, Longitude, IpAddress)
                        VALUES (@StudentID, @WeekID, @Status, datetime('now'), @Latitude, @Longitude, @IpAddress);
                    ";
                    cmd.Parameters.AddWithValue("@StudentID", StudentID);
                    cmd.Parameters.AddWithValue("@WeekID", WeekID);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@Latitude", Latiude);
                    cmd.Parameters.AddWithValue("@Longitude", Longitude);
                    cmd.Parameters.AddWithValue("@IpAddress", IpAddress);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        // Cập nhật lên Google Sheets
        public async Task SyncAttendancesDataToGoogleSheet(GoogleSheetsRepository googleSheetsRepo)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT * FROM Attendances;
                ";

                var reader = await command.ExecuteReaderAsync();
                var attendanceID = new List<int>();
                var studentIDs = new List<int>();
                var weekIDs = new List<int>();
                var statuses = new List<string>();
                var checkedInAts = new List<DateTime>();
                var latitudes = new List<string>();
                var longitudes = new List<string>();
                var ipAddresses = new List<string>();

                while (await reader.ReadAsync())
                {
                    attendanceID.Add(Convert.ToInt32(reader["AttendanceID"]));
                    studentIDs.Add(Convert.ToInt32(reader["StudentID"]));
                    weekIDs.Add(Convert.ToInt32(reader["WeekID"]));
                    statuses.Add(reader["Status"].ToString());
                    checkedInAts.Add(Convert.ToDateTime(reader["CheckedInAt"]));
                    latitudes.Add(reader["Latitude"].ToString());
                    longitudes.Add(reader["Longitude"].ToString());
                    ipAddresses.Add(reader["IpAddress"].ToString());
                }

                // Chuẩn bị danh sách giá trị để cập nhật hàng loạt
                var values = new List<IList<object>>();
                for (int i = 0; i < studentIDs.Count; i++)
                {
                    values.Add(new List<object> { attendanceID[i], studentIDs[i], weekIDs[i], statuses[i], checkedInAts[i], latitudes[i], longitudes[i], ipAddresses[i] });
                }

                var range = $"Attendances!A2:H{studentIDs.Count + 1}";
                await googleSheetsRepo.UpdateToGoogleSheets(range, values);
            }
           
        }
        // Lấy IpAddress có tồn tại không
        public bool CheckIpAddress(string IpAddress)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                            SELECT COUNT(*) FROM Attendances WHERE IpAddress = @IpAddress;
                        ";
                    cmd.Parameters.AddWithValue("@IpAddress", IpAddress);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}           
