using SocietyMangementApi.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SocietyMangementApi.Data
{
    public class VisitorRepository
    {
        private readonly IConfiguration _configuration;

        public VisitorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        public List<VisitorModel> GetAllVisitors()
        {
            string connectionString = GetConnectionString();
            List<VisitorModel> visitors = new List<VisitorModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Visitor_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            visitors.Add(new VisitorModel
                            {
                                VisitorID = reader.GetInt32(reader.GetOrdinal("VisitorID")),
                                VisitorName = reader.GetString(reader.GetOrdinal("VisitorName")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                WhomToMeet = reader.GetString(reader.GetOrdinal("WhomToMeet")),
                                FlatType = reader.GetString(reader.GetOrdinal("FlatType")),
                                FlatNumber = reader.GetString(reader.GetOrdinal("FlatNumber")),
                                VisitPurpose = reader.GetString(reader.GetOrdinal("VisitPurpose")),
                                EntryTime = reader.GetDateTime(reader.GetOrdinal("EntryTime")),
                                ExitTime = reader.IsDBNull(reader.GetOrdinal("ExitTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExitTime")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            });
                        }
                    }
                }
            }
            return visitors;
        }



        public VisitorModel GetVisitorById(int visitorID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Visitor_SelectById";
                    command.Parameters.AddWithValue("@VisitorID", visitorID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new VisitorModel
                            {
                                VisitorID = reader.GetInt32(reader.GetOrdinal("VisitorID")),
                                VisitorName = reader.GetString(reader.GetOrdinal("VisitorName")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                WhomToMeet = reader.GetString(reader.GetOrdinal("WhomToMeet")),
                                FlatType = reader.GetString(reader.GetOrdinal("FlatType")),
                                FlatNumber = reader.GetString(reader.GetOrdinal("FlatNumber")),
                                VisitPurpose = reader.IsDBNull(reader.GetOrdinal("VisitPurpose")) ? null : reader.GetString(reader.GetOrdinal("VisitPurpose")),
                                EntryTime = reader.GetDateTime(reader.GetOrdinal("EntryTime")),
                                ExitTime = reader.IsDBNull(reader.GetOrdinal("ExitTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExitTime")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool InsertVisitor(VisitorModel visitor)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Visitor_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    visitor.Status = "In";
                    cmd.Parameters.AddWithValue("@VisitorName", visitor.VisitorName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", visitor.PhoneNumber);
                    cmd.Parameters.AddWithValue("@WhomToMeet", visitor.WhomToMeet);
                    cmd.Parameters.AddWithValue("@FlatType", visitor.FlatType);
                    cmd.Parameters.AddWithValue("@FlatNumber", visitor.FlatNumber);
                    cmd.Parameters.AddWithValue("@VisitPurpose", visitor.VisitPurpose ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EntryTime", visitor.EntryTime);
                    cmd.Parameters.AddWithValue("@ExitTime", visitor.ExitTime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", visitor.Status);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting visitor: {ex.Message}");
                return false;
            }
        }


        public bool UpdateVisitor(VisitorModel visitor)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Visitor_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };visitor.Status = "In";
                    
                    cmd.Parameters.AddWithValue("@VisitorID", visitor.VisitorID);
                    cmd.Parameters.AddWithValue("@VisitorName", visitor.VisitorName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", visitor.PhoneNumber);
                    cmd.Parameters.AddWithValue("@WhomToMeet", visitor.WhomToMeet);
                    cmd.Parameters.AddWithValue("@FlatType", visitor.FlatType);
                    cmd.Parameters.AddWithValue("@FlatNumber", visitor.FlatNumber);
                    cmd.Parameters.AddWithValue("@VisitPurpose", visitor.VisitPurpose ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EntryTime", visitor.EntryTime);
                    cmd.Parameters.AddWithValue("@ExitTime", visitor.ExitTime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", visitor.Status);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating visitor: {ex.Message}");
                return false;
            }
        }


        public bool DeleteVisitor(int visitorID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Visitor_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@VisitorID", visitorID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting visitor: {ex.Message}");
                return false;
            }
        }


        public List<FlatModel> GetFlatsByFlatType(string flatTypeName)
        {
            string connectionString = GetConnectionString();
            List<FlatModel> flats = new List<FlatModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Flat_SelectByFlatType", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@FlatTypeName", flatTypeName);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flats.Add(new FlatModel
                        {
                            FlatID = reader.GetInt32(reader.GetOrdinal("FlatID")),
                            FlatNumber = reader.GetString(reader.GetOrdinal("FlatNumber"))
                        });
                    }
                }
            }
            return flats;
        }



        public List<VisitorModel> GetVisitorsByEntryTime(DateTime? startDate)
        {
            string connectionString = GetConnectionString();
            List<VisitorModel> visitors = new List<VisitorModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Visitor_SelectByEntryTime", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // If startDate is null, pass DBNull to fetch all records
                cmd.Parameters.AddWithValue("@StartDate", startDate ?? (object)DBNull.Value);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        visitors.Add(new VisitorModel
                        {
                            VisitorID = reader.GetInt32(reader.GetOrdinal("VisitorID")),
                            VisitorName = reader.GetString(reader.GetOrdinal("VisitorName")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            WhomToMeet = reader.GetString(reader.GetOrdinal("WhomToMeet")),
                            FlatType = reader.GetString(reader.GetOrdinal("FlatType")),
                            FlatNumber = reader.GetString(reader.GetOrdinal("FlatNumber")),
                            VisitPurpose = reader.IsDBNull(reader.GetOrdinal("VisitPurpose")) ? null : reader.GetString(reader.GetOrdinal("VisitPurpose")),
                            EntryTime = reader.GetDateTime(reader.GetOrdinal("EntryTime")),
                            ExitTime = reader.IsDBNull(reader.GetOrdinal("ExitTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExitTime")),
                            Status = reader.GetString(reader.GetOrdinal("Status"))
                        });
                    }
                }
            }
            return visitors;
        }



        public List<VisitorModel> GetTop3Visitors()
        {
            List<VisitorModel> visitors = new List<VisitorModel>();

            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Visitor_SelectTop3";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            visitors.Add(new VisitorModel
                            {
                                VisitorID = reader.GetInt32(reader.GetOrdinal("VisitorID")),
                                VisitorName = reader.GetString(reader.GetOrdinal("VisitorName")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                WhomToMeet = reader.GetString(reader.GetOrdinal("WhomToMeet")),
                                FlatType = reader.GetString(reader.GetOrdinal("FlatType")),
                                FlatNumber = reader.GetString(reader.GetOrdinal("FlatNumber")),
                                VisitPurpose = reader.GetString(reader.GetOrdinal("VisitPurpose")),
                                EntryTime = reader.GetDateTime(reader.GetOrdinal("EntryTime")),
                                ExitTime = reader.IsDBNull(reader.GetOrdinal("ExitTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExitTime")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            });
                        }
                    }
                }
            }
            return visitors;
        }

        public int GetVisitorCount()
        {
            int count = 0;

            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Visitor_Count";

                    count = (int)command.ExecuteScalar();
                }
            }
            return count;
        }


        public VisitorStatsModel GetVisitorStatistics()
        {
            VisitorStatsModel stats = new VisitorStatsModel();

            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Visitor_Statistics";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.TodayVisitors = reader.GetInt32(reader.GetOrdinal("TodayVisitors"));
                            stats.LastWeekVisitors = reader.GetInt32(reader.GetOrdinal("LastWeekVisitors"));
                            stats.LastTwoWeeksVisitors = reader.GetInt32(reader.GetOrdinal("LastTwoWeeksVisitors"));
                        }
                    }
                }
            }

            return stats;
        }
    }
}

