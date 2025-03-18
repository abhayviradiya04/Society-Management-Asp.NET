using SocietyManagementApi.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SocietyManagementApi.Data
{
    public class MaintenanceRepository
    {
        private readonly IConfiguration _configuration;

        public MaintenanceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        public List<MaintenanceModel> GetAllMaintenanceRecords()
        {
            string connectionString = GetConnectionString();
            List<MaintenanceModel> maintenanceRecords = new List<MaintenanceModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Maintenance_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            maintenanceRecords.Add(new MaintenanceModel
                            {
                                MaintenanceID = reader.GetInt32(reader.GetOrdinal("MaintenanceID")),
                                FlatID = reader.GetInt32(reader.GetOrdinal("FlatID")), // FlatID should be integer
                                FlatNumber = reader.IsDBNull(reader.GetOrdinal("FlatNumber")) ? null : reader.GetString(reader.GetOrdinal("FlatNumber")), // FlatNumber is a string
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? null : reader.GetString(reader.GetOrdinal("UserName")), // UserName is a string
                                Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                                DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                                PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus")),
                                PaidDate = reader.IsDBNull(reader.GetOrdinal("PaidDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("PaidDate")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                            });
                        }
                    }
                }
            }
            return maintenanceRecords;
        }




        public MaintenanceModel GetMaintenanceById(int maintenanceID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Maintenance_SelectByPk";
                    command.Parameters.AddWithValue("@MaintenanceID", maintenanceID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new MaintenanceModel
                            {
                                MaintenanceID = reader.GetInt32(reader.GetOrdinal("MaintenanceID")),
                                FlatID = reader.GetInt32(reader.GetOrdinal("FlatID")),
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                                DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                                PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus")),
                                PaidDate = reader.IsDBNull(reader.GetOrdinal("PaidDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("PaidDate")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool InsertMaintenance(MaintenanceModel maintenance)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Maintenance_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@FlatID", maintenance.FlatID);
                    cmd.Parameters.AddWithValue("@UserID", (object)maintenance.UserID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Amount", maintenance.Amount);
                    cmd.Parameters.AddWithValue("@DueDate", maintenance.DueDate);
                    cmd.Parameters.AddWithValue("@PaymentStatus", maintenance.PaymentStatus);
                    cmd.Parameters.AddWithValue("@PaidDate", (object)maintenance.PaidDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Notes", (object)maintenance.Notes ?? DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting maintenance record: {ex.Message}");
                return false;
            }
        }

        public bool UpdateMaintenance(MaintenanceModel maintenance)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Maintenance_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@MaintenanceID", maintenance.MaintenanceID);
                    cmd.Parameters.AddWithValue("@FlatID", maintenance.FlatID);
                    cmd.Parameters.AddWithValue("@UserID", (object)maintenance.UserID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Amount", maintenance.Amount);
                    cmd.Parameters.AddWithValue("@DueDate", maintenance.DueDate);
                    cmd.Parameters.AddWithValue("@PaymentStatus", maintenance.PaymentStatus);
                    cmd.Parameters.AddWithValue("@PaidDate", (object)maintenance.PaidDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Notes", (object)maintenance.Notes ?? DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating maintenance record: {ex.Message}");
                return false;
            }
        }

        public bool DeleteMaintenance(int maintenanceID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Maintenance_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@MaintenanceID", maintenanceID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting maintenance record: {ex.Message}");
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

        public IEnumerable<GetUserNameByFlatID> GetUserNameByFlatID(int flatID)
        {
            var username = new List<GetUserNameByFlatID>();
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("PR_GetUserNameByFlatID", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@FlatID", flatID);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    username.Add(new GetUserNameByFlatID
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader.GetString(reader.GetOrdinal("UserName"))
                    });
                }
            }
            return username;

        }
    }
}
