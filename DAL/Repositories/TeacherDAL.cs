using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DTO;
using System.Data.SQLite;

namespace DAL.Repositories
{
    public class TeacherDAL
    {
        private string _connectionString;

        public TeacherDAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        // Lấy tất cả các khóa học của giáo viên
        public List<CourseDTO> GetCourses(int userID, int termID)
        {
            List<CourseDTO> courses = new List<CourseDTO>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                    SELECT DISTINCT 
                        c.courseID, 
                        c.courseName, 
                        c.courseCode,
                        g.groupID,
                        g.groupName,
                        g.sessionTime,
                        cl.classID,
                        cl.className
                    FROM 
                        CourseAssignments ca
                    JOIN 
                        Courses c ON ca.courseID = c.courseID
                    JOIN 
                        Groups g ON c.courseID = g.courseID
                    JOIN 
                        Terms t ON c.termID = t.termID
                    JOIN 
                        Users u ON ca.teacherID = u.userID
                    JOIN 
                        Classes cl ON g.classID = cl.classID  -- Kết nối với bảng Classes để lấy tên lớp
                    WHERE 
                        u.userID = @UserID
                        AND u.roleID = 2  -- Chỉ lọc giáo viên
                        AND t.termID = @TermID;
                     ";
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@TermID", termID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var course = new CourseDTO
                            {
                                CourseID = Convert.ToInt32(reader["courseID"]),
                                CourseName = reader["courseName"].ToString(),
                                CourseCode = reader["courseCode"].ToString(),
                                GroupID = Convert.ToInt32(reader["groupID"]),
                                GroupName = reader["groupName"].ToString(),
                                SessionTime = reader["sessionTime"].ToString(),
                                TeacherID = userID,
                                ClassID = Convert.ToInt32(reader["classID"]),
                                ClassName = reader["className"].ToString(),
                            };
                            courses.Add(course);
                        }
                    }
                }
            }
            return courses;
        }
        // Lấy toàn bộ tuần học của môn học
        public List<WeekDTO> GetWeeks(int CourseID, int GroupID)
        {
            List<WeekDTO> weeks = new List<WeekDTO>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                    SELECT 
                        w.weekID,
                        w.weekNumber,
                        w.startDate,
                        w.endDate
                    FROM 
                        Courses c
                    JOIN 
                        Terms t ON c.termID = t.termID
                    JOIN 
                        Weeks w ON c.courseID = w.courseID
                    WHERE 
                        c.courseID = @CourseID
                        AND w.groupID = @GroupID  -- Thêm GroupID vào điều kiện
                    ";
                            cmd.Parameters.AddWithValue("@CourseID", CourseID);
                    cmd.Parameters.AddWithValue("@GroupID", GroupID);  
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var week = new WeekDTO
                            {
                                WeekID = Convert.ToInt32(reader["weekID"]),
                                WeekNumber = Convert.ToInt32(reader["weekNumber"]),
                                StartDate = reader["startDate"].ToString(),
                                EndDate = reader["endDate"].ToString()
                            };
                            weeks.Add(week);
                        }
                    }
                }
            }
            return weeks;
        }
        // Lấy toàn bộ thông báo của giáo viên
        public List<AnnouncementsDTO> GetAnnouncements(int WeekID)
        {
            List<AnnouncementsDTO> announcements = new List<AnnouncementsDTO>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                    SELECT 
                        a.announcementID,
                        a.content
                    FROM 
                        Announcements a
                    JOIN 
                        Weeks w ON a.weekID = w.weekID  -- Kết nối bảng Announcements với Weeks
                    WHERE 
                        w.weekID = @WeekID;  -- Sử dụng weekID trong điều kiện
                    ";
                    cmd.Parameters.AddWithValue("@WeekID", WeekID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var announcement = new AnnouncementsDTO
                            {
                                AnnouncementID = Convert.ToInt32(reader["announcementID"]),
                                Content = reader["content"].ToString()
                            };
                            announcements.Add(announcement);
                        }
                    }
                }
            }
            return announcements;
        }
        // Kiểm tra giáo viên đã tạo link điểm danh chưa
        public bool CheckAttendanceLink(int WeekID, int CourseID, int TeacherID, int ClassID)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                        SELECT 
                            w.weekID,
                            w.weekNumber,
                            w.startDate,
                            w.endDate,
                            w.isAttendanceLinkCreated
                        FROM 
                            Weeks w
                        JOIN 
                            Courses c ON w.courseID = c.courseID
                        JOIN 
                            CourseAssignments ca ON c.courseID = ca.courseID
                        JOIN 
                            Users u ON ca.teacherID = u.userID
                        JOIN 
                            Groups g ON w.GroupID = g.GroupID
                        JOIN 
                            Classes cl ON g.classID = cl.classID
                        WHERE 
                            u.userID = @TeacherID
                            AND w.weekID = @WeekID
                            AND c.courseID = @CourseID
                            AND cl.classID = @ClassID
                    ";
                    cmd.Parameters.AddWithValue("@WeekID", WeekID);
                    cmd.Parameters.AddWithValue("@CourseID", CourseID);
                    cmd.Parameters.AddWithValue("@TeacherID", TeacherID);
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //Console.WriteLine(reader["weekID"] + " " + reader["weekNumber"] + " " + reader["startDate"] + " " + reader["endDate"] + " " + reader["isAttendanceLinkCreated"]);
                            return Convert.ToBoolean(reader["isAttendanceLinkCreated"]);
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }
        // Thêm thông báo
        public bool AddAnnouncement(int weekID, string content)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                    INSERT INTO Announcements (weekID, content)
                    VALUES (@WeekID, @Content);
                    ";
                    cmd.Parameters.AddWithValue("@WeekID", weekID);
                    cmd.Parameters.AddWithValue("@Content", content);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        // Cập nhật thông báo
        public bool UpdateAnnouncement(int announcementID, string newContent)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                    UPDATE Announcements
                    SET content = @NewContent
                    WHERE announcementID = @AnnouncementID;
                    ";
                    cmd.Parameters.AddWithValue("@NewContent", newContent);
                    cmd.Parameters.AddWithValue("@AnnouncementID", announcementID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        // Xóa thông báo
        public bool DeleteAnnouncement(int announcementID)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                    DELETE FROM Announcements
                    WHERE announcementID = @AnnouncementID;
                    ";
                    cmd.Parameters.AddWithValue("@AnnouncementID", announcementID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public async Task SyncAnnouncementsDataToGoogleSheet(GoogleSheetsRepository googleSheetsRepo)
        {
            await googleSheetsRepo.ClearSheetData("Announcements!A2:C");
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT * FROM Announcements;
                ";

                var reader = await command.ExecuteReaderAsync();
                var announcementID = new List<int>();
                var weekIDs = new List<int>();
                var contents = new List<string>();

                while (reader.Read())
                {
                    announcementID.Add(Convert.ToInt32(reader["AnnouncementID"]));
                    weekIDs.Add(Convert.ToInt32(reader["WeekID"]));
                    contents.Add(reader["Content"].ToString());
                }

                // Chuẩn bị danh sách giá trị để cập nhật hàng loạt
                var values = new List<IList<object>>();
                for (int i = 0; i < weekIDs.Count; i++)
                {
                    values.Add(new List<object> { announcementID[i], weekIDs[i], contents[i] });
                }

                var range = $"Announcements!A2:C{weekIDs.Count + 1}";
                await googleSheetsRepo.UpdateToGoogleSheets(range, values);
            }
        }
    }
}
