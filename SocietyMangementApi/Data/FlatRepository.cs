using SocietyManagementApi.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SocietyManagementApi.Data
{
    public class FlatRepository
    {
        private readonly IConfiguration _configuration;

        public FlatRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        public List<FlatModel> GetAllFlats()
        {
            string connectionString = GetConnectionString();
            List<FlatModel> flats = new List<FlatModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Flats_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            flats.Add(new FlatModel
                            {
                                FlatID = reader.GetInt32(reader.GetOrdinal("FlatID")),
                                FlatNumber = reader.GetString(reader.GetOrdinal("FlatNumber")),
                                FlatTypeID = reader.GetInt32(reader.GetOrdinal("FlatTypeID")),
                                FloorNumber = reader.GetInt32(reader.GetOrdinal("FloorNumber")),
                                Block = reader.IsDBNull(reader.GetOrdinal("Block")) ? null : reader.GetString(reader.GetOrdinal("Block"))
                            });
                        }
                    }
                }
            }
            return flats;
        }

        public FlatModel GetFlatById(int flatID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Flats_SelectByPk";
                    command.Parameters.AddWithValue("@FlatID", flatID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new FlatModel
                            {
                                FlatID = reader.GetInt32(reader.GetOrdinal("FlatID")),
                                FlatNumber = reader.GetString(reader.GetOrdinal("FlatNumber")),
                                FlatTypeID = reader.GetInt32(reader.GetOrdinal("FlatTypeID")),
                                FloorNumber = reader.GetInt32(reader.GetOrdinal("FloorNumber")),
                                Block = reader.IsDBNull(reader.GetOrdinal("Block")) ? null : reader.GetString(reader.GetOrdinal("Block"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool InsertFlat(FlatModel flat)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Flats_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@FlatNumber", flat.FlatNumber);
                    cmd.Parameters.AddWithValue("@FlatTypeID", flat.FlatTypeID);
                    cmd.Parameters.AddWithValue("@FloorNumber", flat.FloorNumber);
                    cmd.Parameters.AddWithValue("@Block", flat.Block ?? (object)DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting flat: {ex.Message}");
                return false;
            }
        }

        public bool UpdateFlat(FlatModel flat)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Flats_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@FlatID", flat.FlatID);
                    cmd.Parameters.AddWithValue("@FlatNumber", flat.FlatNumber);
                    cmd.Parameters.AddWithValue("@FlatTypeID", flat.FlatTypeID);
                    cmd.Parameters.AddWithValue("@FloorNumber", flat.FloorNumber);
                    cmd.Parameters.AddWithValue("@Block", flat.Block ?? (object)DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating flat: {ex.Message}");
                return false;
            }
        }

        public bool DeleteFlat(int flatID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Flats_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@FlatID", flatID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting flat: {ex.Message}");
                return false;
            }
        }
    }
}
