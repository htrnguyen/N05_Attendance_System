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
        // Kiểm tra giáo viên đã tạo link điểm danh chưa
        public bool CheckAttendanceLink(int WeekID, int CourseID, int TeacherID)
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
                        WHERE 
                            u.userID = @TeacherID -- ID của giáo viên
                            AND w.weekID = @WeekID -- ID của tuần cần kiểm tra
                            AND c.courseID = @CourseID; -- ID của khóa học cần kiểm tra
                    ";
                    cmd.Parameters.AddWithValue("@WeekID", WeekID);
                    cmd.Parameters.AddWithValue("@CourseID", CourseID);
                    cmd.Parameters.AddWithValue("@TeacherID", TeacherID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Convert.ToBoolean(reader["isAttendanceLinkCreated"]);
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
