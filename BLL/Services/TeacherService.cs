using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using DTO;

namespace BLL.Services
{
    public class TeacherService
    {
        private readonly TeacherDAL _teacherDAL;

        public TeacherService()
        {
            _teacherDAL = new TeacherDAL();
        }
        // Lấy tất cả các khóa học của giáo viên
        public List<CourseDTO> GetCourses(int userID, int termID)
        {
            return _teacherDAL.GetCourses(userID, termID);
        }
        // Lấy toàn bộ tuần học của môn học
        public List<WeekDTO> GetWeeks(int CourseID)
        {
            return _teacherDAL.GetWeeks(CourseID);
        }
        // Lấy toàn bộ thông báo của giáo viên
        public List<AnnouncementsDTO> GetAnnouncements(int WeekID)
        {
            return _teacherDAL.GetAnnouncements(WeekID);
        }
        // Kiểm tra xem giáo viên đã điểm danh cho tuần học đó chưa
        public bool CheckAttendanceLink(int WeekID, int CourseID, int TeacherID)
        {
            return _teacherDAL.CheckAttendanceLink(WeekID, CourseID, TeacherID);
        }
    }
}
