using SocietyManagementApi.Model;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace SocietyManagementApi.Data
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        public List<UserModel> GetAllUsers()
        {
            string connectionString = GetConnectionString();
            List<UserModel> users = new List<UserModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_User_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new UserModel
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Role = reader.GetString(reader.GetOrdinal("Role")),
                                FlatNumber = reader.GetString(reader.GetOrdinal("FlatNumber")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                            });
                        }
                    }
                }
            }
            return users;
        }

        public UserModel GetUserById(int userID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_User_SelectByPk";
                    command.Parameters.AddWithValue("@UserID", userID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserModel
                            {
                               
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Role = reader.GetString(reader.GetOrdinal("Role")),
                                FlatNumber = reader.GetString(reader.GetOrdinal("FlatNumber")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool InsertUser(UserModel user)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_User_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@FlatID", user.FlatNumber);
                    cmd.Parameters.AddWithValue("@Status", user.Status ?? "Active");

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting user: {ex.Message}");
                return false;
            }
        }

        public bool UpdateUser(UserModel user)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_User_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@FlatID", user.FlatNumber);
                    cmd.Parameters.AddWithValue("@Status", user.Status);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        public bool DeleteUser(int userID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_User_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return false;
            }
        }

              public IEnumerable<GetFlatnumber> GetFlatNumber()
        {
            var flatnumber = new List<GetFlatnumber>();
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("PR_GetFlatNumber", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    flatnumber.Add(new GetFlatnumber
                    {
                        FlatID = Convert.ToInt32(reader["FlatID"]),
                        FlatNumber = reader["FlatNumber"].ToString()
                    }
                        );
                }
            }
            return flatnumber;
        }


        [HttpPost]
        public DataTable LoginUser(string userName, string password, string role)
        {
            DataTable userTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("PR_User_Login", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Role", role);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        userTable.Load(reader);
                    }
                }
            }

            return userTable;
        }

        [HttpPost]
        public DataTable RegisterUser(string userName, string password, string email, string contactNo, string role)
        {
            DataTable resultTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("PR_User_Register", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Contact_No", contactNo);
                        cmd.Parameters.AddWithValue("@Role", role);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            resultTable.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Exception: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Exception: " + ex.Message);
                throw;
            }

            return resultTable;
        }



        public List<UserModel> GetTop3Users()
        {
            List<UserModel> users = new List<UserModel>();

            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_User_SelectTop3";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new UserModel
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Role = reader.GetString(reader.GetOrdinal("Role")),
                                FlatNumber = reader.GetString(reader.GetOrdinal("FlatNumber")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                            });
                        }
                    }
                }
            }
            return users;
        }

        public int GetUserCount()
        {
            int count = 0;
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_User_Count";

                    count = (int)command.ExecuteScalar();
                }
            }
            return count;
        }
    }
}
