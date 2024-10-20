using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
//using DTO;
using System.Diagnostics;

namespace DAL
{
    public class SQLiteRepository
    {
        private readonly string _connectionString;

        public SQLiteRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        // Kiểm tra xem cơ sở dữ liệu đã tồn tại chưa
        public bool DatabaseExists()
        {
            return File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "attendance.db"));
        }

        // Tạo bảng nếu chưa tồn tại trong SQLite
        public void CreateDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                // Tạo bảng Roles
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Roles (
                    RoleID INTEGER PRIMARY KEY,
                    RoleName TEXT NOT NULL
                )";
                command.ExecuteNonQuery();

                // Tạo bảng Users
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE CHECK (Email LIKE '%_@__%.__%'),
                    FullName TEXT NOT NULL,
                    RoleID INTEGER,
                    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
                )";
                command.ExecuteNonQuery();

                // Tạo bảng Terms
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Terms (
                    TermID INTEGER PRIMARY KEY AUTOINCREMENT,
                    TermName TEXT NOT NULL,
                    StartDate DATE NOT NULL,
                    EndDate DATE NOT NULL
                )";
                command.ExecuteNonQuery();

                // Tạo bảng Courses
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Courses (
                    CourseID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CourseName TEXT NOT NULL,
                    CourseCode TEXT NOT NULL,
                    TermID INTEGER,
                    FOREIGN KEY (TermID) REFERENCES Terms(TermID) ON DELETE CASCADE
                )";
                command.ExecuteNonQuery();

                // Tạo bảng CourseAssignments
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS CourseAssignments (
                    AssignmentID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CourseID INTEGER,
                    TeacherID INTEGER,
                    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID) ON DELETE CASCADE,
                    FOREIGN KEY (TeacherID) REFERENCES Users(UserID) ON DELETE CASCADE
                )";
                command.ExecuteNonQuery();

                // Tạo bảng Classes
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Classes (
                    ClassID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ClassCode TEXT NOT NULL,
                    ClassName TEXT NOT NULL,
                    Latitude TEXT,
                    Longitude TEXT
                )";
                command.ExecuteNonQuery();

                // Tạo bảng Groups
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Groups (
                    GroupID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CourseID INTEGER,
                    ClassID INTEGER,
                    GroupName TEXT NOT NULL,
                    SessionTime TEXT NOT NULL,
                    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID) ON DELETE CASCADE
                    FOREIGN KEY (ClassID) REFERENCES Classes(ClassID) ON DELETE CASCADE
                )";
                command.ExecuteNonQuery();

                // Tạo bảng Enrollments
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Enrollments (
                    EnrollmentID INTEGER PRIMARY KEY AUTOINCREMENT,
                    StudentID INTEGER,
                    GroupID INTEGER,
                    FOREIGN KEY (StudentID) REFERENCES Users(UserID) ON DELETE CASCADE,
                    FOREIGN KEY (GroupID) REFERENCES Groups(GroupID) ON DELETE CASCADE
                )";
                command.ExecuteNonQuery();

                // Tạo bảng Weeks
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Weeks (
                    WeekID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CourseID INTEGER,
                    GroupID INTEGER,
                    WeekNumber INTEGER NOT NULL,
                    StartDate DATE NOT NULL,
                    EndDate DATE NOT NULL,
                    IsAttendanceLinkCreated BOOLEAN DEFAULT 0,
                    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID) ON DELETE CASCADE
                )";
                command.ExecuteNonQuery();

                // Tạo bảng Announcements
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Announcements (
                    AnnouncementID INTEGER PRIMARY KEY AUTOINCREMENT,
                    WeekID INTEGER,
                    Content TEXT NOT NULL,
                    FOREIGN KEY (weekID) REFERENCES Weeks(weekID) ON DELETE CASCADE
                )";
                command.ExecuteNonQuery();

                // Tạo bảng Attendances
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Attendances (
                    AttendanceID INTEGER PRIMARY KEY AUTOINCREMENT,
                    StudentID INTEGER,
                    WeekID INTEGER,
                    Status TEXT NOT NULL CHECK (Status IN ('Có mặt', 'Vắng mặt')),
                    CheckedInAt DATETIME,
                    Latitude TEXT,
                    Longitude TEXT,
                    IPAddress TEXT,
                    FOREIGN KEY (StudentID) REFERENCES Users(UserID) ON DELETE CASCADE,
                    FOREIGN KEY (WeekID) REFERENCES Weeks(WeekID) ON DELETE CASCADE
                )";
                command.ExecuteNonQuery();
            }
        }
        // Xoá dữ liệu cũ trong toàn bộ bảng
        public void ClearDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                // Xóa dữ liệu từ các bảng theo thứ tự để tránh lỗi khóa ngoại
                command.CommandText = "DELETE FROM Attendances";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Announcements";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Weeks";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Enrollments";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Groups";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Classes";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM CourseAssignments";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Courses";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Terms";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Users";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Roles";
                command.ExecuteNonQuery();

                // Đặt lại autoincrement về 1
                command.CommandText = "DELETE FROM sqlite_sequence";
                command.ExecuteNonQuery();
            }
        }

        // Xoá dữ liệu bảng Classes và Weeks
        public void ClearClassesAndWeeks()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = "DELETE FROM Classes";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Weeks";
                command.ExecuteNonQuery();

                // Đặt lại autoincrement Classes và Weeks về 1
                command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Classes'";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Weeks'";
                command.ExecuteNonQuery();
            }
        }

        // Chèn dữ liệu vào bảng Roles
        public async Task InsertRoleData(IList<IList<object>> roleData)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    for (int i = 1; i < roleData.Count; i++)
                    {
                        var row = roleData[i];
                        command.CommandText = @"
                            INSERT INTO Roles (RoleID, RoleName) 
                            VALUES (@RoleID, @RoleName);
                        ";
                        command.Parameters.AddWithValue("@RoleID", row[0]);
                        command.Parameters.AddWithValue("@RoleName", row[1]);

                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                    }
                    transaction.Commit();
                }
            }
        }
        // Chèn dữ liệu vào bảng Users
        public async Task InsertUserData(IList<IList<object>> userData)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    for (int i = 1; i < userData.Count; i++)
                    {
                        var row = userData[i];
                        command.CommandText = @"
                            INSERT INTO Users (Username, Password, Email, FullName, RoleID) 
                            VALUES (@Username, @Password, @Email, @FullName, @RoleID);
                        ";
                        command.Parameters.AddWithValue("@Username", row[1]);
                        command.Parameters.AddWithValue("@Password", row[2]);
                        command.Parameters.AddWithValue("@Email", row[3]);
                        command.Parameters.AddWithValue("@FullName", row[4]);
                        command.Parameters.AddWithValue("@RoleID", row[5]);

                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
            }
        }
        // Chèn dữ liệu vào bảng Terms
        public async Task InsertTermData(IList<IList<object>> termData)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    for (int i = 1; i < termData.Count; i++)
                    {
                        var row = termData[i];
                        command.CommandText = @"
                            INSERT INTO Terms (TermName, StartDate, EndDate) 
                            VALUES (@TermName, @StartDate, @EndDate);
                        ";
                        command.Parameters.AddWithValue("@TermName", row[1]);
                        command.Parameters.AddWithValue("@StartDate", row[2]);
                        command.Parameters.AddWithValue("@EndDate", row[3]);

                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
            }
        }
        // Chèn dữ liệu vào bảng Courses
        public async Task InsertCourseData(IList<IList<object>> courseData)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    for (int i = 1; i < courseData.Count; i++)
                    {
                        var row = courseData[i];
                        command.CommandText = @"
                            INSERT INTO Courses (CourseName, CourseCode, TermID) 
                            VALUES (@CourseName, @CourseCode, @TermID);
                        ";
                        command.Parameters.AddWithValue("@CourseName", row[1]);
                        command.Parameters.AddWithValue("@CourseCode", row[2]);
                        command.Parameters.AddWithValue("@TermID", row[3]);

                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
            }
        }
        // Chèn dữ liệu vào bảng CourseAssignments
        public async Task InsertCourseAssignmentData(IList<IList<object>> courseAssignmentData)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    for (int i = 1; i < courseAssignmentData.Count; i++)
                    {
                        var row = courseAssignmentData[i];
                        command.CommandText = @"
                            INSERT INTO CourseAssignments (CourseID, TeacherID) 
                            VALUES (@CourseID, @TeacherID);
                        ";
                        command.Parameters.AddWithValue("@CourseID", row[1]);
                        command.Parameters.AddWithValue("@TeacherID", row[2]);

                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
            }
        }
        // Chèn dữ liệu vào bảng Classes
        public async Task InsertClassData(IList<IList<object>> classData)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    for (int i = 1; i < classData.Count; i++)
                    {
                        var row = classData[i];
                        command.CommandText = @"
                            INSERT INTO Classes (ClassCode, ClassName, Latitude, Longitude)
                            VALUES (@ClassCode, @ClassName, @Latitude, @Longitude);
                        ";
                        command.Parameters.AddWithValue("@ClassCode", row[1]);
                        command.Parameters.AddWithValue("@ClassName", row[2]);
                        command.Parameters.AddWithValue("@Latitude", row[3]);
                        command.Parameters.AddWithValue("@Longitude", row[4]);

                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
            }
        }
        // Chèn dữ liệu vào bảng Groups
        public async Task InsertGroupData(IList<IList<object>> groupData)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    for (int i = 1; i < groupData.Count; i++)
                    {
                        var row = groupData[i];
                        command.CommandText = @"
                            INSERT INTO Groups (CourseID, ClassID, GroupName, SessionTime) 
                            VALUES (@CourseID, @ClassID, @GroupName, @SessionTime);
                        ";
                        command.Parameters.AddWithValue("@CourseID", row[1]);
                        command.Parameters.AddWithValue("@ClassID", row[2]);
                        command.Parameters.AddWithValue("@GroupName", row[3]);
                        command.Parameters.AddWithValue("@SessionTime", row[4]);

                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
            }
        }
        // Chèn dữ liệu vào bảng Enrollments
        public async Task InsertEnrollmentData(IList<IList<object>> enrollmentData)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    for (int i = 1; i < enrollmentData.Count; i++)
                    {
                        var row = enrollmentData[i];
                        command.CommandText = @"
                            INSERT INTO Enrollments (StudentID, GroupID) 
                            VALUES (@StudentID, @GroupID);
                        ";
                        command.Parameters.AddWithValue("@StudentID", row[1]);
                        command.Parameters.AddWithValue("@GroupID", row[2]);

                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
            }
        }
        // Chèn dữ liệu vào bảng Weeks
        public async Task InsertWeekData(IList<IList<object>> weekData)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    for (int i = 1; i < weekData.Count; i++)
                    {
                        var row = weekData[i];
                        command.CommandText = @"
                            INSERT INTO Weeks (CourseID, GroupID, WeekNumber, StartDate, EndDate, IsAttendanceLinkCreated)
                            VALUES (@CourseID, @GroupID, @WeekNumber, @StartDate, @EndDate, @IsAttendanceLinkCreated);
                        ";
                        command.Parameters.AddWithValue("@CourseID", row[1]);
                        command.Parameters.AddWithValue("@GroupID", row[2]);
                        command.Parameters.AddWithValue("@WeekNumber", row[3]);
                        command.Parameters.AddWithValue("@StartDate", row[4]);
                        command.Parameters.AddWithValue("@EndDate", row[5]);
                        command.Parameters.AddWithValue("@IsAttendanceLinkCreated", row[6]);

                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
            }
        }
        // Chèn dữ liệu vào bảng Announcements
        public async Task InsertAnnouncementData(IList<IList<object>> announcementData)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    for (int i = 1; i < announcementData.Count; i++)
                    {
                        var row = announcementData[i];
                        command.CommandText = @"
                            INSERT INTO Announcements (WeekID, Content) 
                            VALUES (@WeekID, @Content);
                        ";
                        command.Parameters.AddWithValue("@WeekID", row[1]);
                        command.Parameters.AddWithValue("@Content", row[2]);

                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
            }
        }
        // Chèn dữ liệu vào bảng Attendances
        public async Task InsertAttendanceData(IList<IList<object>> attendanceData)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    for (int i = 1; i < attendanceData.Count; i++)
                    {
                        var row = attendanceData[i];
                        command.CommandText = @"
                            INSERT INTO Attendances (StudentID, WeekID, Status, CheckedInAt, Latitude, Longitude, IPAddress) 
                            VALUES (@StudentID, @WeekID, @Status, @CheckedInAt, @Latitude, @Longitude, @IPAddress);
                        ";
                        command.Parameters.AddWithValue("@StudentID", row[1]);
                        command.Parameters.AddWithValue("@WeekID", row[2]);
                        command.Parameters.AddWithValue("@Status", row[3]);
                        command.Parameters.AddWithValue("@CheckedInAt", row[4]);
                        command.Parameters.AddWithValue("@Latitude", row[5]);
                        command.Parameters.AddWithValue("@Longitude", row[6]);
                        command.Parameters.AddWithValue("@IPAddress", row[7]);

                        await command.ExecuteNonQueryAsync();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
            }
        }
    }
}
