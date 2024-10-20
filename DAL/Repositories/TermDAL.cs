using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TermDAL
    {
        private string _connectionString;

        public TermDAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        // Lấy tất cả kỳ học dựa vào User ID
        public List<TermDTO> GetAllTermsByUserID(int userID)
        {
            List<TermDTO> terms = new List<TermDTO>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                        SELECT DISTINCT Terms.TermID, Terms.TermName, Terms.StartDate, Terms.EndDate
                        FROM Users
                        LEFT JOIN Enrollments ON Users.UserID = Enrollments.StudentID
                        LEFT JOIN Groups ON Enrollments.GroupID = Groups.GroupID
                        LEFT JOIN Courses AS StudentCourses ON Groups.CourseID = StudentCourses.CourseID
                        LEFT JOIN CourseAssignments ON Users.UserID = CourseAssignments.TeacherID
                        LEFT JOIN Courses AS TeacherCourses ON CourseAssignments.CourseID = TeacherCourses.CourseID
                        LEFT JOIN Terms ON (StudentCourses.TermID = Terms.TermID OR TeacherCourses.TermID = Terms.TermID)
                        WHERE Users.UserID = @userID";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            terms.Add(new TermDTO
                            {
                                TermID = Convert.ToInt32(reader["TermID"]),
                                TermName = reader["TermName"].ToString(),
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = Convert.ToDateTime(reader["EndDate"])
                            });
                        }
                    }
                }
            }
            return terms;
        }
        // Lấy TermID dựa vào TermName
        public int GetTermIDByTermName(string termName)
        {
            int termID = -1;
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT TermID FROM Terms WHERE TermName = @termName";
                    cmd.Parameters.AddWithValue("@termName", termName);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            termID = Convert.ToInt32(reader["TermID"]);
                        }
                    }
                }
            }
            return termID;
        }
    }
}
