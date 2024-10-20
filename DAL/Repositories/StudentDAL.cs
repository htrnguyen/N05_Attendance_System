using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class StudentDAL
    {
        private string _connectionString;

        public StudentDAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        // Lấy tất cả các khoá học
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
                        g.groupName,
                        g.sessionTime,
                        u_teacher.userID,
                        cl.classID,
                        cl.className 
                    FROM 
                        Enrollments e
                    JOIN 
                        Groups g ON e.groupID = g.groupID
                    JOIN 
                        Courses c ON g.courseID = c.courseID
                    JOIN 
                        Terms t ON c.termID = t.termID
                    JOIN 
                        Users u ON e.studentID = u.userID
                    JOIN 
                        CourseAssignments ca ON c.courseID = ca.courseID
                    JOIN 
                        Users u_teacher ON ca.teacherID = u_teacher.userID
                    JOIN 
                        Classes cl ON g.classID = cl.classID  -- Kết nối với bảng Classes để lấy tên lớp
                    WHERE 
                        u.userID = @UserID
                        AND u.roleID = 1  -- Chỉ lọc sinh viên
                        AND t.termID = @TermID;  -- Điều kiện cho termID
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
                                GroupName = reader["groupName"].ToString(),
                                SessionTime = reader["sessionTime"].ToString(),
                                TeacherID = Convert.ToInt32(reader["userID"]),
                                ClassID = Convert.ToInt32(reader["classID"]),
                                ClassName = reader["className"].ToString()
                            };
                            courses.Add(course);
                        }
                    }
                }
            }
            return courses;
        }
        // Lấy toàn bộ tuần học của môn học
        public List<WeekDTO> GetWeeks(int CourseID)
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
                    ";
                    cmd.Parameters.AddWithValue("@CourseID", CourseID);
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
        // Kiểm tra Giáo viên đã tạo link điểm danh chưa
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
                            AND cl.classID = @ClassID;
                    ";
                    cmd.Parameters.AddWithValue("@WeekID", WeekID);
                    cmd.Parameters.AddWithValue("@CourseID", CourseID);
                    cmd.Parameters.AddWithValue("@TeacherID", TeacherID);
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
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
        // Kiểm tra sinh viên đã điểm danh chưa
        public bool CheckAttendance(int UserID, int WeekID, int CourseID)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                        SELECT a.attendanceID, a.studentID, a.status, a.checkedInAt, a.IpAddress
                        FROM Attendances a
                        JOIN Users u ON a.studentID = u.userID
                        JOIN Weeks w ON a.weekID = w.weekID
                        JOIN Courses c ON w.courseID = c.courseID
                        WHERE a.studentID = @UserID
                        AND w.weekID = @WeekID
                        AND c.courseID = @CourseID;
                    ";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@WeekID", WeekID);
                    cmd.Parameters.AddWithValue("@CourseID", CourseID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Nếu status = "Có mặt " thì trả về true
                        if (reader.Read())
                        {
                            return reader["status"].ToString() == "Có mặt";
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
    }
}
