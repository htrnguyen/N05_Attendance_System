CREATE DATABASE N05
GO

USE N05
GO

-- Tạo bảng Roles
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(255) NOT NULL
);
GO

-- Tạo bảng Users
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    RoleID INT FOREIGN KEY REFERENCES Roles(RoleID)
);
GO

-- Tạo bảng Terms
CREATE TABLE Terms (
    TermID INT PRIMARY KEY IDENTITY(1,1),
    TermName NVARCHAR(255) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL
);

-- Tạo bảng Courses
CREATE TABLE Courses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    CourseName NVARCHAR(255) NOT NULL,
    CourseCode NVARCHAR(100) NOT NULL,
    TermID INT FOREIGN KEY REFERENCES Terms(TermID)
);

-- Tạo bảng CourseAssignments
CREATE TABLE CourseAssignments (
    AssignmentID INT PRIMARY KEY IDENTITY(1,1),
    CourseID INT FOREIGN KEY REFERENCES Courses(CourseID),
    TeacherID INT FOREIGN KEY REFERENCES Users(UserID)
);

-- Tạo bảng Classes
CREATE TABLE Classes (
    ClassID INT PRIMARY KEY IDENTITY(1,1),
    ClassCode NVARCHAR(100) NOT NULL,
    ClassName NVARCHAR(255) NOT NULL,
    Latitude NVARCHAR(100),
    Longitude NVARCHAR(100)
);

-- Tạo bảng Groups
CREATE TABLE Groups (
    GroupID INT PRIMARY KEY IDENTITY(1,1),
    CourseID INT FOREIGN KEY REFERENCES Courses(CourseID),
    ClassID INT FOREIGN KEY REFERENCES Classes(ClassID),
    GroupName NVARCHAR(255) NOT NULL,
    SessionTime NVARCHAR(255) NOT NULL
);

-- Tạo bảng Enrollments
CREATE TABLE Enrollments (
    EnrollmentID INT PRIMARY KEY IDENTITY(1,1),
    StudentID INT FOREIGN KEY REFERENCES Users(UserID),
    GroupID INT FOREIGN KEY REFERENCES Groups(GroupID)
);

-- Tạo bảng Weeks
CREATE TABLE Weeks (
    WeekID INT PRIMARY KEY IDENTITY(1,1),
    CourseID INT FOREIGN KEY REFERENCES Courses(CourseID),
	GroupID INT FOREIGN KEY REFERENCES Groups(GroupID), 
    WeekNumber INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    IsAttendanceLinkCreated BIT DEFAULT 1
);

-- Tạo bảng Announcements
CREATE TABLE Announcements (
    AnnouncementID INT PRIMARY KEY IDENTITY(1,1),
    WeekID INT FOREIGN KEY REFERENCES Weeks(WeekID),
    Content NVARCHAR(MAX) NOT NULL
);

-- Tạo bảng Attendances
CREATE TABLE Attendances (
    AttendanceID INT PRIMARY KEY IDENTITY(1,1),
    StudentID INT FOREIGN KEY REFERENCES Users(UserID),
    WeekID INT FOREIGN KEY REFERENCES Weeks(WeekID),
    Status NVARCHAR(50) NOT NULL,
    CheckedInAt DATETIME,
    Latitude NVARCHAR(100),
    Longitude NVARCHAR(100),
    IPAddress NVARCHAR(100)
);

-- Thêm dữ liệu mẫu vào Roles
INSERT INTO Roles (RoleName) 
VALUES 
('Student'),
('Teacher'), 
('Admin');

-- Thêm dữ liệu mẫu vào bảng Terms
INSERT INTO Terms (TermName, StartDate, EndDate) VALUES
(N'Học kỳ Xuân 2023', '2023-01-15', '2023-05-15'),
(N'Học kỳ Hè 2023', '2023-06-01', '2023-08-15'),
(N'Học kỳ Thu 2023', '2023-09-01', '2023-12-20'),
(N'Học kỳ Đông 2024', '2024-01-10', '2024-04-30');
GO

-- Thêm dữ liệu mẫu vào bảng Users
INSERT INTO Users (Username, Password, Email, FullName, RoleID) VALUES
(N'sinhvien1', 'password1', 'sinhvien1@edu.vn', N'Nguyễn Văn An', 1),
(N'sinhvien2', 'password2', 'sinhvien2@edu.vn', N'Trần Thị Bình', 1),
(N'sinhvien3', 'password3', 'sinhvien3@edu.vn', N'Lê Văn Công', 1),
(N'sinhvien4', 'password4', 'sinhvien4@edu.vn', N'Phạm Thị Dung', 1),
(N'sinhvien5', 'password5', 'sinhvien5@edu.vn', N'Hoàng Văn Em', 1),
(N'sinhvien6', 'password6', 'sinhvien6@edu.vn', N'Đặng Thị Hương', 1),
(N'giangvien1', 'password7', 'giangvien1@edu.vn', N'Vũ Thị Phương', 2),
(N'giangvien2', 'password8', 'giangvien2@edu.vn', N'Đỗ Văn Quang', 2),
(N'quantri', 'adminpass', 'admin@edu.vn', N'Quản Trị Viên', 3);
GO

-- Thêm dữ liệu mẫu vào bảng Courses
INSERT INTO Courses (CourseName, CourseCode, TermID) VALUES
(N'Lập trình Cơ bản', N'LP101', 1),
(N'Cấu trúc Dữ liệu', N'CTDL201', 1),
(N'Thuật toán Nâng cao', N'TTN301', 2),
(N'Hệ quản trị CSDL', N'CSDL401', 3),
(N'Hệ điều hành', N'HDH501', 3),
(N'An ninh Mạng', N'ANM601', 4),
(N'Phát triển Web', N'PTW701', 4),
(N'Khoa học Dữ liệu', N'KHDL801', 4);
GO

-- Thêm dữ liệu mẫu vào bảng Classes
INSERT INTO Classes (ClassCode, ClassName, Latitude, Longitude) VALUES
(N'Lop01', N'Phòng 101', '21.028511', '105.804817'),
(N'Lop02', N'Phòng 102', '21.028512', '105.804818'),
(N'Lop03', N'Phòng 103', '21.028513', '105.804819'),
(N'Lop04', N'Phòng 104', '21.028514', '105.804820'),
(N'Lop05', N'Phòng 105', '21.028515', '105.804821');
GO

-- Thêm dữ liệu mẫu vào bảng CourseAssignments
INSERT INTO CourseAssignments (CourseID, TeacherID) VALUES
(1, 7),  -- Lập trình Cơ bản do giảng viên 1 phụ trách
(2, 7),
(3, 8),  -- Thuật toán Nâng cao do giảng viên 2 phụ trách
(4, 8),
(5, 7),
(6, 8),
(7, 7),
(8, 8);
GO

-- Thêm dữ liệu mẫu vào bảng Groups
INSERT INTO Groups (CourseID, ClassID, GroupName, SessionTime) VALUES
(1, 1, N'Nhóm A', N'Thứ Hai 8:00-10:00'),
(1, 2, N'Nhóm B', N'Thứ Tư 10:00-12:00'),
(2, 1, N'Nhóm A', N'Thứ Ba 13:00-15:00'),
(3, 3, N'Nhóm C', N'Thứ Năm 14:00-16:00'),
(4, 2, N'Nhóm B', N'Thứ Sáu 9:00-11:00'),
(5, 4, N'Nhóm D', N'Thứ Bảy 15:00-17:00'),
(6, 3, N'Nhóm C', N'Chủ Nhật 10:00-12:00'),
(7, 4, N'Nhóm D', N'Thứ Hai 14:00-16:00'),
(8, 5, N'Nhóm E', N'Thứ Tư 16:00-18:00');
GO

-- Thêm dữ liệu mẫu vào bảng Enrollments
INSERT INTO Enrollments (StudentID, GroupID) VALUES
(1, 1),
(1, 3),
(1, 5),
(2, 1),
(2, 2),
(2, 6),
(3, 4),
(3, 7),
(4, 5),
(4, 8),
(5, 2),
(5, 6),
(6, 9),
(6, 3);
GO

-- Thêm dữ liệu mẫu vào bảng Weeks
INSERT INTO Weeks (CourseID, GroupID, WeekNumber, StartDate, EndDate, IsAttendanceLinkCreated) VALUES
(1, 1, 1, '2023-01-16', '2023-01-22', 1),
(1, 1, 2, '2023-01-23', '2023-01-29', 1),
(1, 2, 1, '2023-01-18', '2023-01-24', 1),
(2, 3, 1, '2023-01-17', '2023-01-23', 1),
(2, 3, 2, '2023-01-24', '2023-01-30', 1),
(3, 4, 1, '2023-06-02', '2023-06-08', 1),
(4, 5, 1, '2023-09-05', '2023-09-11', 1),
(5, 6, 1, '2023-09-12', '2023-09-18', 1),
(6, 7, 1, '2024-01-12', '2024-01-18', 1),
(7, 8, 1, '2024-01-13', '2024-01-19', 1),
(8, 9, 1, '2024-01-14', '2024-01-20', 1);
GO

-- Thêm dữ liệu mẫu vào bảng Announcements
INSERT INTO Announcements (WeekID, Content) VALUES
(1, N'Chào mừng các bạn đến với Lập trình Cơ bản.'),
(2, N'Tuần này chúng ta sẽ học về biến và kiểu dữ liệu.'),
(4, N'Bài tập tuần này đã được đăng, các bạn nhớ hoàn thành trước hạn.'),
(7, N'Chuẩn bị cho bài kiểm tra giữa kỳ vào tuần sau.'),
(9, N'Tuần này chúng ta sẽ có buổi thực hành tại phòng lab.'),
(11, N'Lớp Khoa học Dữ liệu sẽ có buổi hội thảo với chuyên gia.');
GO

