using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Repositories;
using DTO;

namespace BLL.Services
{
    public class TeacherService
    {
        private readonly TeacherDAL _teacherDAL;
        private readonly GoogleSheetsRepository _googleSheetsRepo;

        public TeacherService()
        {
            _teacherDAL = new TeacherDAL();
            this._googleSheetsRepo = new GoogleSheetsRepository();
        }
        // Lấy tất cả các khóa học của giáo viên
        public List<CourseDTO> GetCourses(int userID, int termID)
        {
            return _teacherDAL.GetCourses(userID, termID);
        }
        // Lấy toàn bộ tuần học của môn học
        public List<WeekDTO> GetWeeks(int CourseID, int GroupID)
        {
            return _teacherDAL.GetWeeks(CourseID, GroupID);
        }
        // Lấy toàn bộ thông báo của giáo viên
        public List<AnnouncementsDTO> GetAnnouncements(int WeekID)
        {
            return _teacherDAL.GetAnnouncements(WeekID);
        }
        // Kiểm tra xem giáo viên đã điểm danh cho tuần học đó chưa
        public bool CheckAttendanceLink(int WeekID, int CourseID, int TeacherID, int ClassID)
        {
            return _teacherDAL.CheckAttendanceLink(WeekID, CourseID, TeacherID, ClassID);
        }
        // Thêm thông báo
        public bool AddAnnouncement(int weekID, string content)
        {
            return _teacherDAL.AddAnnouncement(weekID, content);
        }
        // Cập nhật thông báo
        public bool UpdateAnnouncement(int announcementID, string newContent)
        {
            return _teacherDAL.UpdateAnnouncement(announcementID, newContent);
        }
        // Xóa thông báo
        public bool DeleteAnnouncement(int announcementID)
        {
            return _teacherDAL.DeleteAnnouncement(announcementID);
        }
        // Đồng bộ thông báo lên Gooogle Sheets
        public async Task SyncAnnouncementsDataToGoogleSheet()
        {
            await _teacherDAL.SyncAnnouncementsDataToGoogleSheet(_googleSheetsRepo);
        }
    }
}
