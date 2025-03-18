using SocietyManagementApi.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SocietyManagementApi.Data
{
    public class NoticeBoardRepository
    {
        private readonly IConfiguration _configuration;

        public NoticeBoardRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        public List<NoticeBoardModel> GetAllNotices()
        {
            string connectionString = GetConnectionString();
            List<NoticeBoardModel> notices = new List<NoticeBoardModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_NoticeBoard_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notices.Add(new NoticeBoardModel
                            {
                                NoticeID = reader.GetInt32(reader.GetOrdinal("NoticeID")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                PostedBy = reader.GetInt32(reader.GetOrdinal("PostedBy")),
                                PostingDate = reader.GetDateTime(reader.GetOrdinal("PostingDate")),
                                ExpirationDate = reader.IsDBNull(reader.GetOrdinal("ExpirationDate")) ? null : reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                                Visibility = reader.IsDBNull(reader.GetOrdinal("Visibility")) ? null : reader.GetString(reader.GetOrdinal("Visibility")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            });
                        }
                    }
                }
            }
            return notices;
        }

        public NoticeBoardModel GetNoticeById(int noticeID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_NoticeBoard_SelectByPk";
                    command.Parameters.AddWithValue("@NoticeID", noticeID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new NoticeBoardModel
                            {
                                NoticeID = reader.GetInt32(reader.GetOrdinal("NoticeID")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                PostedBy = reader.GetInt32(reader.GetOrdinal("PostedBy")),
                                PostingDate = reader.GetDateTime(reader.GetOrdinal("PostingDate")),
                                ExpirationDate = reader.IsDBNull(reader.GetOrdinal("ExpirationDate")) ? null : reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                                Visibility = reader.IsDBNull(reader.GetOrdinal("Visibility")) ? null : reader.GetString(reader.GetOrdinal("Visibility")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool InsertNotice(NoticeBoardModel notice)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_NoticeBoard_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@Title", notice.Title);
                    cmd.Parameters.AddWithValue("@Description", notice.Description);
                    cmd.Parameters.AddWithValue("@PostedBy", notice.PostedBy);
                    cmd.Parameters.AddWithValue("@PostingDate", notice.PostingDate);
                    cmd.Parameters.AddWithValue("@ExpirationDate", notice.ExpirationDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Visibility", notice.Visibility ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", notice.Status);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting notice: {ex.Message}");
                return false;
            }
        }

        public bool UpdateNotice(NoticeBoardModel notice)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_NoticeBoard_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@NoticeID", notice.NoticeID);
                    cmd.Parameters.AddWithValue("@Title", notice.Title);
                    cmd.Parameters.AddWithValue("@Description", notice.Description);
                    cmd.Parameters.AddWithValue("@PostedBy", notice.PostedBy);
                    cmd.Parameters.AddWithValue("@PostingDate", notice.PostingDate);
                    cmd.Parameters.AddWithValue("@ExpirationDate", notice.ExpirationDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Visibility", notice.Visibility ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", notice.Status);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating notice: {ex.Message}");
                return false;
            }
        }

        public bool DeleteNotice(int noticeID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_NoticeBoard_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@NoticeID", noticeID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting notice: {ex.Message}");
                return false;
            }
        }
    }
}
