using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserDAL
    {
        private string _connectionString;
        private GoogleSheetsRepository _googleSheetsRepo;
        public UserDAL()
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            this._googleSheetsRepo = new GoogleSheetsRepository();
        }
        // Xử lý đăng nhập
        public bool isLogin(string username, string password)
        {
            // Kiểm tra username và password có tồn tại không
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM Users WHERE Username = @username AND Password = @password";
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }
        // Kiểm tra email có  tồn tại không
        public bool isEmailExist(string email)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM Users WHERE Email = @email";
                    cmd.Parameters.AddWithValue("@email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }
        // Thay đổi mật khẩu
        public void ChangePassword(string email, string newPassword)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE Users SET Password = @newPassword WHERE Email = @email";
                    cmd.Parameters.AddWithValue("@newPassword", newPassword);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Thay đổi mật khẩu lên Google Sheet
        public async Task ChangePasswordGoogleSheet()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                Console.WriteLine("Connecting to SQLite...");
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT Password FROM Users;
                ";

                var reader = await command.ExecuteReaderAsync();
                var passwords = new List<string>();
                while (await reader.ReadAsync())
                {
                    passwords.Add(reader.GetString(0));
                }

                // Chuẩn bị danh sách giá trị để cập nhật hàng loạt
                var values = new List<IList<object>>();
                foreach (var password in passwords)
                {
                    values.Add(new List<object> { password });
                }

                // Cập nhật mật khẩu lên Google Sheets với tất cả hàng trong một lần
                var range = $"Users!C2:C{passwords.Count + 1}";
                await _googleSheetsRepo.UpdateToGoogleSheets(range, values);
            }
        }
        // Lấy thông tin User
        public UserDTO GetUser(string username)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                        SELECT Users.UserID, Users.Email, Users.FullName, Roles.RoleName
                        FROM Users
                        JOIN Roles ON Users.RoleID = Roles.RoleID
                        WHERE Users.username = @username
                    ";
                    cmd.Parameters.AddWithValue("@username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserDTO
                            {
                                UserID = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                FullName = reader.GetString(2),
                                Role = reader.GetString(3)
                            };
                        }
                        return null;
                    }
                }
            }
        }
    }
}
