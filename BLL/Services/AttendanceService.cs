using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL.Repositories;
using DAL;

namespace BLL.Services
{
    public class AttendanceService
    {
        private readonly AttendanceDAL _attendanceDAL;
        private readonly GoogleSheetsRepository _googleSheetsRepo;
        public AttendanceService()
        {
            this._attendanceDAL = new AttendanceDAL();
            this._googleSheetsRepo = new GoogleSheetsRepository();
        }
        // Cập nhật toạ độ lên Database
        public bool UpdateCoordinates(int ClassID, int CourseID, string Latitude, string Longitude)
        {
            return _attendanceDAL.UpdateCoordinates(ClassID, CourseID, Latitude, Longitude);
        }
        // Cập nhật trạng thái link điểm danh
        public bool UpdateAttendanceLinkStatus(int WeekID, int CourseID, int TeacherID)
        {
            return _attendanceDAL.UpdateAttendanceLinkStatus(WeekID, CourseID, TeacherID);
        }
        // Cập nhật Classes lên GoogleSheet
        public async Task UpdateClassesToGoogleSheet()
        {
            await _attendanceDAL.SyncClassessDataToGoogleSheet(_googleSheetsRepo);
        }
        // Cập nhật Weeks lên GoogleSheet
        public async Task UpdateWeeksToGoogleSheet()
        {
            await _attendanceDAL.SyncWeeksDataToGoogleSheet(_googleSheetsRepo);
        }
        // Lấy toạ độ lớp học
        public List<String> GetClassCoordinates(int ClassID)
        {
            return _attendanceDAL.GetClassCoordinates(ClassID);
        }
        // Cập nhật trạng thái điểm danh của sinh viên
        public bool UpdateAttendance(int StudentID, int WeekID, string Status, string Latiude, string Longtiude, string IpAddress)
        {
            return _attendanceDAL.UpdateAttendance(StudentID, WeekID, Status, Latiude, Longtiude, IpAddress);
        }
        // Cập nhật lên Google Sheets
        public async Task SyncAttendancesDataToGoogleSheet()
        {
            await _attendanceDAL.SyncAttendancesDataToGoogleSheet(_googleSheetsRepo);
        }
        // Kiểm tra IpAddress có tồn tại trong bảng điểm danh không
        public bool CheckIpAddress(string IpAddress)
        {
            return _attendanceDAL.CheckIpAddress(IpAddress);
        }
    }
}
