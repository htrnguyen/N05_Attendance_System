CREATE DATABASE N05
GO

USE N05
GO

-- Tạo bảng Roles
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(255) NOT NULL
);

-- Tạo bảng Users
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    RoleID INT FOREIGN KEY REFERENCES Roles(RoleID)
);

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

-- Thêm dữ liệu vào Users (Admin, Teachers, Students)
INSERT INTO Users (Username, Password, Email, FullName, RoleID)
VALUES 
('admin01', 'hashedpassword1', 'admin@example.com', 'Administrator', 3),
('teacher01', 'hashedpassword2', 'teacher1@example.com', 'John Smith', 2),
('teacher02', 'hashedpassword3', 'teacher2@example.com', 'Jane Doe', 2),
('teacher03', 'hashedpassword4', 'teacher3@example.com', 'Mark Brown', 2),
('teacher04', 'hashedpassword5', 'teacher4@example.com', 'Emma White', 2),
('teacher05', 'hashedpassword6', 'teacher5@example.com', 'Chris Black', 2),
('student01', 'hashedpassword7', 'student1@example.com', 'Alice Johnson', 1),
('student02', 'hashedpassword8', 'student2@example.com', 'Bob Lee', 1),
('student03', 'hashedpassword9', 'student3@example.com', 'Charlie Kim', 1),
('student04', 'hashedpassword10', 'student4@example.com', 'David Park', 1),
('student05', 'hashedpassword11', 'student5@example.com', 'Eva Davis', 1),
('student06', 'hashedpassword12', 'student6@example.com', 'Frank Miller', 1),
('student07', 'hashedpassword13', 'student7@example.com', 'Grace Wilson', 1),
('student08', 'hashedpassword14', 'student8@example.com', 'Henry Adams', 1),
('student09', 'hashedpassword15', 'student9@example.com', 'Ivy Baker', 1),
('student10', 'hashedpassword16', 'student10@example.com', 'Jack Nguyen', 1);

-- Thêm dữ liệu vào bảng Terms
INSERT INTO Terms (TermName, StartDate, EndDate)
VALUES 
('Fall 2024', '2024-09-01', '2024-12-15'),
('Spring 2025', '2025-01-15', '2025-05-30'),
('Fall 2025', '2025-09-01', '2025-12-15'),
('Spring 2026', '2026-01-15', '2026-05-30');

-- Thêm dữ liệu vào bảng Courses
INSERT INTO Courses (CourseName, CourseCode, TermID)
VALUES 
('Database Design', 'CS101', 1),
('Software Engineering', 'CS102', 1),
('Data Structures', 'CS103', 2),
('Algorithms', 'CS104', 2),
('Operating Systems', 'CS105', 3),
('Computer Networks', 'CS106', 3),
('Artificial Intelligence', 'CS107', 4),
('Machine Learning', 'CS108', 4),
('Web Development', 'CS109', 1),
('Mobile App Development', 'CS110', 2);

-- Gán giáo viên cho các môn học trong CourseAssignments
INSERT INTO CourseAssignments (CourseID, TeacherID)
VALUES 
(1, 2), (1, 3),
(2, 2), (2, 4),
(3, 3), (3, 5),
(4, 4), (4, 2),
(5, 5), (5, 3),
(6, 2), (6, 4),
(7, 3), (7, 5),
(8, 4), (8, 2),
(9, 5), (9, 4),
(10, 2), (10, 3);

-- Thêm dữ liệu vào bảng Classes
INSERT INTO Classes (ClassCode, ClassName, Latitude, Longitude)
VALUES 
('DB101', 'Database Design Class A', '21.028511', '105.804817'),
('DB102', 'Database Design Class B', '21.028511', '105.804817');

-- Thêm dữ liệu vào bảng Groups
INSERT INTO Groups (CourseID, ClassID, GroupName, SessionTime)
VALUES 
(1, 1, 'Group 1', '09:00 AM - 11:00 AM'),
(1, 2, 'Group 2', '01:00 PM - 03:00 PM'),
(2, 1, 'Group 1', '09:00 AM - 11:00 AM'),
(2, 2, 'Group 2', '01:00 PM - 03:00 PM');

-- Thêm dữ liệu vào bảng Enrollments (Sinh viên đăng ký các nhóm học)
INSERT INTO Enrollments (StudentID, GroupID)
VALUES 
(3, 1), (3, 2), (3, 3), (3, 4),  
(4, 1), (4, 2), (4, 3), (4, 4),
(5, 1), (5, 2), (5, 3), (5, 4),
(6, 1), (6, 2), (6, 3), (6, 4),
(7, 1), (7, 2), (7, 3), (7, 4),
(8, 1), (8, 2), (8, 3), (8, 4),
(9, 1), (9, 2), (9, 3), (9, 4),
(10, 1), (10, 2), (10, 3), (10, 4);

-- Tạo tuần học cho các môn trong bảng Weeks (10-15 tuần mỗi môn, liên kết với GroupID)
INSERT INTO Weeks (CourseID, GroupID, WeekNumber, StartDate, EndDate, IsAttendanceLinkCreated)
VALUES 
-- Khóa học 1 (CS101 - Database Design)
(1, 1, 1, '2024-09-01', '2024-09-07', 0),
(1, 1, 2, '2024-09-08', '2024-09-14', 1),
(1, 1, 3, '2024-09-15', '2024-09-21', 1),
(1, 1, 4, '2024-09-22', '2024-09-28', 1),
(1, 1, 5, '2024-09-29', '2024-10-05', 1),
(1, 1, 6, '2024-10-06', '2024-10-12', 1),
(1, 1, 7, '2024-10-13', '2024-10-19', 1),
(1, 1, 8, '2024-10-20', '2024-10-26', 1),
(1, 1, 9, '2024-10-27', '2024-11-02', 1),
(1, 1, 10, '2024-11-03', '2024-11-09', 1),
(1, 2, 1, '2024-09-01', '2024-09-07', 1),
(1, 2, 2, '2024-09-08', '2024-09-14', 1),
(1, 2, 3, '2024-09-15', '2024-09-21', 1),
(1, 2, 4, '2024-09-22', '2024-09-28', 1),
(1, 2, 5, '2024-09-29', '2024-10-05', 1),
(1, 2, 6, '2024-10-06', '2024-10-12', 1),
(1, 2, 7, '2024-10-13', '2024-10-19', 1),
(1, 2, 8, '2024-10-20', '2024-10-26', 1),
(1, 2, 9, '2024-10-27', '2024-11-02', 1),
(1, 2, 10, '2024-11-03', '2024-11-09', 1),

-- Khóa học 2 (CS102 - Software Engineering)
(2, 3, 1, '2025-01-15', '2025-01-21', 0),
(2, 3, 2, '2025-01-22', '2025-01-28', 0),
(2, 3, 3, '2025-01-29', '2025-02-04', 0),
(2, 3, 4, '2025-02-05', '2025-02-11', 0),
(2, 3, 5, '2025-02-12', '2025-02-18', 0),
(2, 3, 6, '2025-02-19', '2025-02-25', 0),
(2, 3, 7, '2025-02-26', '2025-03-04', 0),
(2, 3, 8, '2025-03-05', '2025-03-11', 0),
(2, 3, 9, '2025-03-12', '2025-03-18', 0),
(2, 3, 10, '2025-03-19', '2025-03-25', 0),
(2, 4, 1, '2025-01-15', '2025-01-21', 0),
(2, 4, 2, '2025-01-22', '2025-01-28', 0),
(2, 4, 3, '2025-01-29', '2025-02-04', 0),
(2, 4, 4, '2025-02-05', '2025-02-11', 0),
(2, 4, 5, '2025-02-12', '2025-02-18', 0),
(2, 4, 6, '2025-02-19', '2025-02-25', 0),
(2, 4, 7, '2025-02-26', '2025-03-04', 0),
(2, 4, 8, '2025-03-05', '2025-03-11', 0),
(2, 4, 9, '2025-03-12', '2025-03-18', 0),
(2, 4, 10, '2025-03-19', '2025-03-25', 0);

INSERT INTO Attendances (StudentID, WeekID, Status, CheckedInAt, Latitude, Longitude)
VALUES
-- Sinh viên 1 (Alice Johnson)
(3, 1, 'Có mặt', '2024-09-01 09:05:00', '21.028511', '105.804817'),
(3, 2, 'Vắng mặt', NULL, NULL, NULL),
(3, 3, 'Có mặt', '2024-09-15 09:05:00', '21.028511', '105.804817'),
(3, 4, 'Có mặt', '2024-09-22 09:10:00', '21.028511', '105.804817'),
(3, 5, 'Vắng mặt', NULL, NULL, NULL),

-- Sinh viên 2 (Bob Lee)
(4, 1, 'Có mặt', '2024-09-01 09:05:00', '21.028511', '105.804817'),
(4, 2, 'Có mặt', '2024-09-08 09:03:00', '21.028511', '105.804817'),
(4, 3, 'Vắng mặt', NULL, NULL, NULL),
(4, 4, 'Có mặt', '2024-09-22 09:05:00', '21.028511', '105.804817'),
(4, 5, 'Có mặt', '2024-09-29 09:08:00', '21.028511', '105.804817'),

-- Sinh viên 3 (Charlie Kim)
(5, 1, 'Có mặt', '2024-09-01 09:07:00', '21.028511', '105.804817'),
(5, 2, 'Có mặt', '2024-09-08 09:06:00', '21.028511', '105.804817'),
(5, 3, 'Có mặt', '2024-09-15 09:02:00', '21.028511', '105.804817'),
(5, 4, 'Có mặt', '2024-09-22 09:04:00', '21.028511', '105.804817'),
(5, 5, 'Vắng mặt', NULL, NULL, NULL),

-- Sinh viên 4 (David Park)
(6, 1, 'Vắng mặt', NULL, NULL, NULL),
(6, 2, 'Có mặt', '2024-09-08 09:03:00', '21.028511', '105.804817'),
(6, 3, 'Có mặt', '2024-09-15 09:05:00', '21.028511', '105.804817'),
(6, 4, 'Có mặt', '2024-09-22 09:01:00', '21.028511', '105.804817'),
(6, 5, 'Vắng mặt', NULL, NULL, NULL);

SELECT 
    studentName,
    courseName,
    [1] AS 'Week 1',
    [2] AS 'Week 2',
    [3] AS 'Week 3',
    [4] AS 'Week 4',
    [5] AS 'Week 5',
	[6] AS 'Wekk 6',
FROM 
    (SELECT 
         u.fullname AS studentName,
         c.courseName,
         w.weekNumber,
         a.status
     FROM 
         Attendances a
     JOIN 
         Users u ON a.studentID = u.userID
     JOIN 
         Enrollments e ON a.studentID = e.studentID
     JOIN 
         Groups g ON e.groupID = g.groupID
     JOIN 
         Weeks w ON a.weekID = w.weekID
     JOIN 
         Courses c ON g.courseID = c.courseID
    ) AS SourceTable
PIVOT 
    (MAX(status) 
     FOR weekNumber IN ([1], [2], [3], [4], [5]) -- Liệt kê các tuần bạn cần pivot
    ) AS PivotTable
ORDER BY 
    studentName;
