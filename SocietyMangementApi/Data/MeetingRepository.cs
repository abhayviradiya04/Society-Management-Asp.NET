using SocietyManagementApi.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SocietyManagementApi.Data
{
    public class MeetingRepository
    {
        private readonly IConfiguration _configuration;

        public MeetingRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        // Get all meetings
        public List<MeetingModel> GetAllMeetings()
        {
            string connectionString = GetConnectionString();
            List<MeetingModel> meetings = new List<MeetingModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Meetings_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            meetings.Add(new MeetingModel
                            {
                                MeetingID = reader.GetInt32(reader.GetOrdinal("MeetingID")),
                                MeetingTitle = reader.GetString(reader.GetOrdinal("MeetingTitle")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                OrganizerID = reader.GetInt32(reader.GetOrdinal("OrganizerID")),
                                StartDateTime = reader.GetDateTime(reader.GetOrdinal("StartDateTime")),
                                EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime")),
                                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            });
                        }
                    }
                }
            }
            return meetings;
        }

        // Get a meeting by its ID
        public MeetingModel GetMeetingById(int meetingID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Meetings_SelectByPk";
                    command.Parameters.AddWithValue("@MeetingID", meetingID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new MeetingModel
                            {
                                MeetingID = reader.GetInt32(reader.GetOrdinal("MeetingID")),
                                MeetingTitle = reader.GetString(reader.GetOrdinal("MeetingTitle")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                OrganizerID = reader.GetInt32(reader.GetOrdinal("OrganizerID")),
                                StartDateTime = reader.GetDateTime(reader.GetOrdinal("StartDateTime")),
                                EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime")),
                                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Insert a new meeting
        public bool InsertMeeting(MeetingModel meeting)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Meetings_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@MeetingTitle", meeting.MeetingTitle);
                    cmd.Parameters.AddWithValue("@Description", meeting.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@OrganizerID", meeting.OrganizerID);
                    cmd.Parameters.AddWithValue("@StartDateTime", meeting.StartDateTime);
                    cmd.Parameters.AddWithValue("@EndDateTime", meeting.EndDateTime);
                    cmd.Parameters.AddWithValue("@Location", meeting.Location ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", meeting.Status);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting meeting: {ex.Message}");
                return false;
            }
        }


        // Update an existing meeting
        public bool UpdateMeeting(MeetingModel meeting)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Meetings_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@MeetingID", meeting.MeetingID);
                    cmd.Parameters.AddWithValue("@MeetingTitle", meeting.MeetingTitle);
                    cmd.Parameters.AddWithValue("@Description", meeting.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@OrganizerID", meeting.OrganizerID);
                    cmd.Parameters.AddWithValue("@StartDateTime", meeting.StartDateTime);
                    cmd.Parameters.AddWithValue("@EndDateTime", meeting.EndDateTime);
                    cmd.Parameters.AddWithValue("@Location", meeting.Location ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", meeting.Status);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating meeting: {ex.Message}");
                return false;
            }
        }

        // Delete a meeting by its ID
        public bool DeleteMeeting(int meetingID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Meetings_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@MeetingID", meetingID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting meeting: {ex.Message}");
                return false;
            }
        }

        public List<MeetingModel> GetTop3Meetings()
        {
            List<MeetingModel> meetings = new List<MeetingModel>();
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand("PR_Meetings_SelectTop3", sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            meetings.Add(MapMeeting(reader));
                        }
                    }
                }
            }
            return meetings;
        }
        private MeetingModel MapMeeting(SqlDataReader reader)
        {
            return new MeetingModel
            {
                MeetingID = reader.GetInt32(reader.GetOrdinal("MeetingID")),
                MeetingTitle = reader.GetString(reader.GetOrdinal("MeetingTitle")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                OrganizerID = reader.GetInt32(reader.GetOrdinal("OrganizerID")),
                StartDateTime = reader.GetDateTime(reader.GetOrdinal("StartDateTime")),
                EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime")),
                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                Status = reader.GetString(reader.GetOrdinal("Status"))
            };
        }

        // Get total meeting count
        public int GetMeetingCount()
        {
            int count = 0;
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand("PR_Meetings_Count", sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    count = (int)command.ExecuteScalar();
                }
            }
            return count;
        }
    }
}
