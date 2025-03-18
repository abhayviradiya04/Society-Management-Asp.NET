using SocietyManagementApi.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SocietyManagementApi.Data
{
    public class EventRepository
    {
        private readonly IConfiguration _configuration;

        public EventRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        public List<EventModel> GetAllEvents()
        {
            string connectionString = GetConnectionString();
            List<EventModel> events = new List<EventModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Events_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new EventModel
                            {
                                EventID = reader.GetInt32(reader.GetOrdinal("EventID")),
                                EventTitle = reader.GetString(reader.GetOrdinal("EventTitle")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                OrganizerID = reader.GetInt32(reader.GetOrdinal("OrganizerID")),
                                StartDateTime = reader.GetDateTime(reader.GetOrdinal("StartDateTime")),
                                EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime")),
                                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                EventImage = reader.IsDBNull(reader.GetOrdinal("EventImage")) ? null : reader.GetString(reader.GetOrdinal("EventImage"))
                            
                        });
                        }
                    }
                }
            }
            return events;
        }

        public EventModel GetEventById(int eventID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Events_SelectByPk";
                    command.Parameters.AddWithValue("@EventID", eventID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new EventModel
                            {
                                EventID = reader.GetInt32(reader.GetOrdinal("EventID")),
                                EventTitle = reader.GetString(reader.GetOrdinal("EventTitle")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                OrganizerID = reader.GetInt32(reader.GetOrdinal("OrganizerID")),
                                StartDateTime = reader.GetDateTime(reader.GetOrdinal("StartDateTime")),
                                EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime")),
                                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                EventImage = reader.IsDBNull(reader.GetOrdinal("EventImage")) ? null : reader.GetString(reader.GetOrdinal("EventImage"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool InsertEvent(EventModel eventModel)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Events_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@EventTitle", eventModel.EventTitle);
                    cmd.Parameters.AddWithValue("@Description", eventModel.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@OrganizerID", eventModel.OrganizerID);
                    cmd.Parameters.AddWithValue("@StartDateTime", eventModel.StartDateTime);
                    cmd.Parameters.AddWithValue("@EndDateTime", eventModel.EndDateTime);
                    cmd.Parameters.AddWithValue("@Location", eventModel.Location ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", eventModel.Status);
                    cmd.Parameters.AddWithValue("@EventImage", eventModel.EventImage ?? (object)DBNull.Value);


                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting event: {ex.Message}");
                return false;
            }
        }

        public bool UpdateEvent(EventModel eventModel)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Events_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@EventID", eventModel.EventID);
                    cmd.Parameters.AddWithValue("@EventTitle", eventModel.EventTitle);
                    cmd.Parameters.AddWithValue("@Description", eventModel.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@OrganizerID", eventModel.OrganizerID);
                    cmd.Parameters.AddWithValue("@StartDateTime", eventModel.StartDateTime);
                    cmd.Parameters.AddWithValue("@EndDateTime", eventModel.EndDateTime);
                    cmd.Parameters.AddWithValue("@Location", eventModel.Location ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", eventModel.Status);
                    cmd.Parameters.AddWithValue("@EventImage", eventModel.EventImage ?? (object)DBNull.Value);


                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating event: {ex.Message}");
                return false;
            }
        }

        public bool DeleteEvent(int eventID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Events_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@EventID", eventID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting event: {ex.Message}");
                return false;
            }
        }

        public List<EventModel> GetTop3Events()
        {
            string connectionString = GetConnectionString();
            List<EventModel> events = new List<EventModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Events_SelectTop3";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new EventModel
                            {
                                EventID = reader.GetInt32(reader.GetOrdinal("EventID")),
                                EventTitle = reader.GetString(reader.GetOrdinal("EventTitle")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                OrganizerID = reader.GetInt32(reader.GetOrdinal("OrganizerID")),
                                StartDateTime = reader.GetDateTime(reader.GetOrdinal("StartDateTime")),
                                EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime")),
                                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                EventImage = reader.IsDBNull(reader.GetOrdinal("EventImage")) ? null : reader.GetString(reader.GetOrdinal("EventImage"))
                            });
                        }
                    }
                }
            }
            return events;
        }

        public int GetEventCount()
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Events_Count";

                    return (int)command.ExecuteScalar();
                }
            }
        }

    }
}
