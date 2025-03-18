using SocietyManagementApi.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SocietyManagementApi.Data
{
    public class ContactRepository
    {
        private readonly IConfiguration _configuration;

        public ContactRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        // Get all contact messages
        public List<ContactModel> GetAllContacts()
        {
            string connectionString = GetConnectionString();
            List<ContactModel> contacts = new List<ContactModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Contact_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contacts.Add(new ContactModel
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                                Message = reader.GetString(reader.GetOrdinal("Message")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            });
                        }
                    }
                }
            }
            return contacts;
        }

        // Get a single contact message by ID
        public ContactModel GetContactById(int contactID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Contact_SelectByPk";
                    command.Parameters.AddWithValue("@ID", contactID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ContactModel
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                                Message = reader.GetString(reader.GetOrdinal("Message")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Insert a new contact message
        public bool InsertContact(ContactModel contact)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Contact_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@Name", contact.Name);
                    cmd.Parameters.AddWithValue("@Email", contact.Email);
                    cmd.Parameters.AddWithValue("@Subject", contact.Subject);
                    cmd.Parameters.AddWithValue("@Message", contact.Message);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting contact: {ex.Message}");
                return false;
            }
        }

        // Update an existing contact message
        public bool UpdateContact(ContactModel contact)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Contact_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@ID", contact.ID);
                    cmd.Parameters.AddWithValue("@Name", contact.Name);
                    cmd.Parameters.AddWithValue("@Email", contact.Email);
                    cmd.Parameters.AddWithValue("@Subject", contact.Subject);
                    cmd.Parameters.AddWithValue("@Message", contact.Message);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating contact: {ex.Message}");
                return false;
            }
        }

        // Delete a contact message
        public bool DeleteContact(int contactID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Contact_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@ID", contactID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting contact: {ex.Message}");
                return false;
            }
        }
    }
}
