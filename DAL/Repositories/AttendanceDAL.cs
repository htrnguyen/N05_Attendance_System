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
                var range = $"Weeks!G2:G{IsAttendanceLinkCreateds.Count + 1}";
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
        public void UpdateAttendance(int StudentID, int WeekID, int GroupID, int Status, string Latitude, string Longitude, string IpAddress)
        {
            // Lấy thời gian hiện tại và chuyển sang định dạng UTC
            DateTime dateTime = DateTime.UtcNow;

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                        INSERT INTO Attendances (StudentID, WeekID, GroupID, Status, CheckedInAt, Latitude, Longitude, IpAddress)
                        VALUES (@StudentID, @WeekID, @GroupID, @Status, @CheckedInAt, @Latitude, @Longitude, @IpAddress);
                    ";
                    cmd.Parameters.AddWithValue("@StudentID", StudentID);
                    cmd.Parameters.AddWithValue("@WeekID", WeekID);
                    cmd.Parameters.AddWithValue("@GroupID", GroupID);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@CheckedInAt", dateTime);
                    cmd.Parameters.AddWithValue("@Latitude", Latitude); 
                    cmd.Parameters.AddWithValue("@Longitude", Longitude);
                    cmd.Parameters.AddWithValue("@IpAddress", IpAddress);
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Điểm danh thành công");
                }
            }
        }
        public bool UpdateStatusAttendance(int StudentID, int WeekID, int GroupID, int Status, string Latitude, string Longitude, string IpAddress)
        {
            // Lấy thời gian hiện tại và chuyển sang định dạng UTC
            DateTime dateTime = DateTime.UtcNow;

            Console.WriteLine("Time: " + dateTime);

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                        UPDATE Attendances
                        SET 
                            Status = @Status,
                            CheckedInAt = @CheckedInAt,  
                            Latitude = @Latitude,
                            Longitude = @Longitude,
                            IpAddress = @IpAddress
                        WHERE 
                            StudentID = @StudentID
                            AND WeekID = @WeekID
                            AND GroupID = @GroupID;
                    ";

                    // Thêm tham số cho câu lệnh
                    cmd.Parameters.AddWithValue("@StudentID", StudentID);
                    cmd.Parameters.AddWithValue("@WeekID", WeekID);
                    cmd.Parameters.AddWithValue("@GroupID", GroupID);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@CheckedInAt", dateTime);  
                    cmd.Parameters.AddWithValue("@Latitude", Latitude);
                    cmd.Parameters.AddWithValue("@Longitude", Longitude);
                    cmd.Parameters.AddWithValue("@IpAddress", IpAddress);

                    // Thực thi câu lệnh và kiểm tra số hàng bị ảnh hưởng
                    int rowsAffected = cmd.ExecuteNonQuery();

                    Console.WriteLine(rowsAffected > 0 ? "Cập nhật điểm danh thành công" : "Không có hàng nào được cập nhật");

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
                    var groupIDs = new List<int>();
                    var statuses = new List<int>();
                    var checkedInAts = new List<DateTime>();
                    var latitudes = new List<string>();
                    var longitudes = new List<string>();
                    var ipAddresses = new List<string>();

                    while (await reader.ReadAsync())
                    {
                        attendanceID.Add(Convert.ToInt32(reader["AttendanceID"]));
                        studentIDs.Add(Convert.ToInt32(reader["StudentID"]));
                        weekIDs.Add(Convert.ToInt32(reader["WeekID"]));
                        groupIDs.Add(Convert.ToInt32(reader["GroupID"]));
                        statuses.Add(Convert.ToInt32(reader["Status"]));
                        checkedInAts.Add(Convert.ToDateTime(reader["CheckedInAt"]));
                        latitudes.Add(reader["Latitude"].ToString());
                        longitudes.Add(reader["Longitude"].ToString());
                        ipAddresses.Add(reader["IpAddress"].ToString());
                    }

                    // Chuẩn bị danh sách giá trị để cập nhật hàng loạt
                    var values = new List<IList<object>>();
                    for (int i = 0; i < studentIDs.Count; i++)
                    {
                        // Định dạng lại thời gian trước khi thêm vào danh sách
                        var formattedCheckedInAt = checkedInAts[i].ToString("yyyy-MM-dd HH:mm:ss");
                        values.Add(new List<object> { attendanceID[i], studentIDs[i], weekIDs[i], groupIDs[i], statuses[i], formattedCheckedInAt, latitudes[i], longitudes[i], ipAddresses[i] });
                    }

                var range = $"Attendances!A2:I{studentIDs.Count + 1}";
                    await googleSheetsRepo.UpdateToGoogleSheets(range, values);
                }
            }
        // Lấy IpAddress có tồn tại không
        public bool CheckIpAddress(string IPAddress, int WeekID, int CourseID, int TeacherID, int ClassID)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                        SELECT COUNT(*)
                        FROM Attendances A
                        JOIN Weeks W ON A.WeekID = W.WeekID
                        JOIN Groups G ON W.GroupID = G.GroupID
                        JOIN Courses C ON G.CourseID = C.CourseID
                        JOIN CourseAssignments CA ON C.CourseID = CA.CourseID
                        WHERE A.IPAddress = @IPAddress 
                        AND W.WeekID = @WeekID 
                        AND C.CourseID = @CourseID
                        AND CA.TeacherID = @TeacherID
                        AND G.ClassID = @ClassID;
                    ";
                    cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
                    cmd.Parameters.AddWithValue("@WeekID", WeekID);
                    cmd.Parameters.AddWithValue("@CourseID", CourseID);
                    cmd.Parameters.AddWithValue("@TeacherID", TeacherID);
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        // Lấy danh sách điểm danh
        public List<AttendanceDTO> GetAttendances(int teacherID, int courseID, int classID)
        {
            List<AttendanceDTO> attendanceRecords = new List<AttendanceDTO>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                    SELECT 
                        u.FullName AS StudentName,
                        MAX(CASE WHEN w.WeekNumber = 1 THEN a.Status END) AS Week1,
                        MAX(CASE WHEN w.WeekNumber = 2 THEN a.Status END) AS Week2,
                        MAX(CASE WHEN w.WeekNumber = 3 THEN a.Status END) AS Week3,
                        MAX(CASE WHEN w.WeekNumber = 4 THEN a.Status END) AS Week4,
                        MAX(CASE WHEN w.WeekNumber = 5 THEN a.Status END) AS Week5,
                        MAX(CASE WHEN w.WeekNumber = 6 THEN a.Status END) AS Week6,
                        MAX(CASE WHEN w.WeekNumber = 7 THEN a.Status END) AS Week7,
                        MAX(CASE WHEN w.WeekNumber = 8 THEN a.Status END) AS Week8,
                        MAX(CASE WHEN w.WeekNumber = 9 THEN a.Status END) AS Week9,
                        MAX(CASE WHEN w.WeekNumber = 10 THEN a.Status END) AS Week10,
                        MAX(CASE WHEN w.WeekNumber = 11 THEN a.Status END) AS Week11,
                        MAX(CASE WHEN w.WeekNumber = 12 THEN a.Status END) AS Week12,
                        MAX(CASE WHEN w.WeekNumber = 13 THEN a.Status END) AS Week13,
                        MAX(CASE WHEN w.WeekNumber = 14 THEN a.Status END) AS Week14,
                        MAX(CASE WHEN w.WeekNumber = 15 THEN a.Status END) AS Week15
                    FROM 
                        Attendances a
                    JOIN 
                        Users u ON a.StudentID = u.UserID
                    JOIN 
                        Weeks w ON a.WeekID = w.WeekID
                    JOIN 
                        Groups g ON w.GroupID = g.GroupID
                    JOIN 
                        Courses c ON g.CourseID = c.CourseID
                    JOIN 
                        CourseAssignments ca ON c.CourseID = ca.CourseID
                    JOIN 
                        Classes cl ON g.ClassID = cl.ClassID
                    WHERE 
                        ca.TeacherID = @TeacherID
                        AND c.CourseID = @CourseID
                        AND cl.ClassID = @ClassID
                    GROUP BY 
                        u.FullName;
                    ";

                    // Thêm tham số cho truy vấn
                    cmd.Parameters.AddWithValue("@TeacherID", teacherID);
                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                    cmd.Parameters.AddWithValue("@ClassID", classID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var attendanceRecord = new AttendanceDTO
                            {
                                StudentName = reader["StudentName"].ToString(),
                                Week1 = reader["Week1"] != DBNull.Value ? reader["Week1"].ToString() : "",
                                Week2 = reader["Week2"] != DBNull.Value ? reader["Week2"].ToString() : "",
                                Week3 = reader["Week3"] != DBNull.Value ? reader["Week3"].ToString() : "",
                                Week4 = reader["Week4"] != DBNull.Value ? reader["Week4"].ToString() : "",
                                Week5 = reader["Week5"] != DBNull.Value ? reader["Week5"].ToString() : "",
                                Week6 = reader["Week6"] != DBNull.Value ? reader["Week6"].ToString() : "",
                                Week7 = reader["Week7"] != DBNull.Value ? reader["Week7"].ToString() : "",
                                Week8 = reader["Week8"] != DBNull.Value ? reader["Week8"].ToString() : "",
                                Week9 = reader["Week9"] != DBNull.Value ? reader["Week9"].ToString() : "",
                                Week10 = reader["Week10"] != DBNull.Value ? reader["Week10"].ToString() : "",
                                Week11 = reader["Week11"] != DBNull.Value ? reader["Week11"].ToString() : "",
                                Week12 = reader["Week12"] != DBNull.Value ? reader["Week12"].ToString() : "",
                                Week13 = reader["Week13"] != DBNull.Value ? reader["Week13"].ToString() : "",
                                Week14 = reader["Week14"] != DBNull.Value ? reader["Week14"].ToString() : "",
                                Week15 = reader["Week15"] != DBNull.Value ? reader["Week15"].ToString() : ""
                            };
                            attendanceRecords.Add(attendanceRecord);    
                        }
                    }
                }
            }
            return attendanceRecords;
        }
        // Lấy danh sách sinh viên
        public List<ListStudentDTO> GetStudentInClass(int TeacherID, int CourseID, int ClassID) 
        {
            List<ListStudentDTO> students = new List<ListStudentDTO>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                        SELECT	
	                        S.UserID,
	                        G.GroupID
                        FROM 
                            Enrollments E
                        JOIN 
                            Users S ON E.StudentID = S.UserID
                        JOIN 
                            Groups G ON E.GroupID = G.GroupID
                        JOIN 
                            Courses C ON G.CourseID = C.CourseID
                        JOIN 
                            Classes CL ON G.ClassID = CL.ClassID
                        JOIN 
                            CourseAssignments CA ON C.CourseID = CA.CourseID
                        JOIN 
                            Users T ON CA.TeacherID = T.UserID
                        WHERE 
                            T.UserID = @TeacherID
                            AND C.CourseID = @CourseID
	                        AND CL.ClassID = @ClassID
                    ";
                    cmd.Parameters.AddWithValue("@TeacherID", TeacherID);
                    cmd.Parameters.AddWithValue("@CourseID", CourseID);
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var student = new ListStudentDTO
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                GroupID = Convert.ToInt32(reader["GroupID"])
                            };
                            students.Add(student);
                        }
                    }
                }
            }
            return students;
        }
    }
}           
