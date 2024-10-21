using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using DTO;
using System.Configuration;

namespace BLL.Services
{
    public class StudentService
    {
        private StudentDAL _studentDAL;
        public StudentService()
        {
            this._studentDAL = new StudentDAL();
        }
        // Lấy tất cả các khoá học 
        public List<CourseDTO> GetCourses(int userID, int termID)
        {
            return _studentDAL.GetCourses(userID, termID);
        }
        // Lấy toàn bộ tuần học của môn học
        public List<WeekDTO> GetWeeks(int CourseID, int GroupID)
        {
            return _studentDAL.GetWeeks(CourseID, GroupID);
        }
        // Lấy toàn bộ thông báo của tuần học
        public List<AnnouncementsDTO> GetAnnouncements(int WeekID)
        {
            return _studentDAL.GetAnnouncements(WeekID);
        }
        // Kiểm tra xem Giáo viên đã tạo link chưa
        public bool CheckAttendanceLink(int WeekID, int CourseID, int TeacherID, int ClassID)
        {
            return _studentDAL.CheckAttendanceLink(WeekID, CourseID, TeacherID, ClassID);
        }
        // Kiểm tra inh viên đã điểm danh chưa
        public bool CheckAttendance(int UserID, int WeekID, int CourseID)
        {
            return _studentDAL.CheckAttendance(UserID, WeekID, CourseID);
        }
        // Lấy danh sách điểm danh
        public List<ListAttendancesDTO> GetListAttendances(int UserID, int WeekID)
        {
            return _studentDAL.GetListAttendances(UserID, WeekID);
        }
    }
}
