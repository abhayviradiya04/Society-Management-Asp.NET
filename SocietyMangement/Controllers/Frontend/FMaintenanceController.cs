using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocietyMangement.Models;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;
using System.Net.Http.Headers;

namespace SocietyMangement.Controllers.Frontend
{
    [CheckAccess]
    public class FMaintenanceController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri _baseAddress = new Uri("https://localhost:7057/api/Maintenance");
        private readonly IConfiguration _configuration;

        public FMaintenanceController(IConfiguration configuration)
        {
            _client = new HttpClient { BaseAddress = _baseAddress };
            _configuration = configuration;
        }
        private void SetAuthorizationHeader()
        {
            var token = HttpContext.Session.GetString("JWTToken");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                Console.WriteLine("⚠️ No JWT Token found in session.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> FMaintenanceList()
        {
            SetAuthorizationHeader();
            List<MaintenanceModel> maintenanceList = new List<MaintenanceModel>();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(string.Empty);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    maintenanceList = JsonConvert.DeserializeObject<List<MaintenanceModel>>(data);
                }
                else
                {
                    ModelState.AddModelError("", "Unable to fetch maintenance data from the API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }

            return View(maintenanceList);
        }

        [HttpGet]
        public IActionResult ExportToExcel(int maintenanceId)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string connectionString = _configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Maintenance_SelectByPk", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MaintenanceID", maintenanceId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        if (table.Rows.Count == 0)
                        {
                            return NotFound("No data found for the selected maintenance.");
                        }

                        using (ExcelPackage package = new ExcelPackage())
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Maintenance");
                            worksheet.Cells["A1"].LoadFromDataTable(table, true);

                            // Format Header Row
                            using (ExcelRange range = worksheet.Cells["A1:F1"])
                            {
                                range.Style.Font.Bold = true;
                                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            }

                            var stream = new MemoryStream();
                            package.SaveAs(stream);
                            stream.Position = 0;

                            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Maintenance_{maintenanceId}.xlsx");
                        }
                    }
                }
            }
        }


        [HttpGet]
        public IActionResult MakePayment(int maintenanceId)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            MaintenanceModel maintenance = new MaintenanceModel();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Maintenance WHERE MaintenanceID = @MaintenanceID", conn);
                cmd.Parameters.AddWithValue("@MaintenanceID", maintenanceId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    maintenance.MaintenanceID = Convert.ToInt32(reader["MaintenanceID"]);
                   
                    maintenance.Amount = Convert.ToDecimal(reader["Amount"]);
                    maintenance.DueDate = Convert.ToDateTime(reader["DueDate"]);
                    maintenance.PaymentStatus = reader["PaymentStatus"].ToString();
                }
                reader.Close();
            }

            return View(maintenance);
        }

        [HttpPost]
        public IActionResult ProcessPayment(int maintenanceId, string paymentMethod, string paymentDetails)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Fetch required maintenance record details
                    SqlCommand fetchCmd = new SqlCommand("SELECT FlatID, UserID, Amount, DueDate FROM Maintenance WHERE MaintenanceID = @MaintenanceID", conn);
                    fetchCmd.Parameters.AddWithValue("@MaintenanceID", maintenanceId);

                    SqlDataReader reader = fetchCmd.ExecuteReader();
                    if (!reader.Read())
                    {
                        TempData["ErrorMessage"] = "Maintenance record not found.";
                        return RedirectToAction("FMaintenanceList");
                    }

                    int flatID = Convert.ToInt32(reader["FlatID"]);
                    object userID = reader["UserID"] == DBNull.Value ? (object)DBNull.Value : Convert.ToInt32(reader["UserID"]);
                    decimal amount = Convert.ToDecimal(reader["Amount"]);
                    DateTime dueDate = Convert.ToDateTime(reader["DueDate"]);
                    reader.Close();

                    // Update Payment Status
                    SqlCommand cmd = new SqlCommand("PR_Maintenance_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@MaintenanceID", maintenanceId);
                    cmd.Parameters.AddWithValue("@FlatID", flatID);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@DueDate", dueDate);
                    cmd.Parameters.AddWithValue("@PaymentStatus", "Paid");
                    cmd.Parameters.AddWithValue("@PaidDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Notes", paymentDetails ?? DBNull.Value.ToString());

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        TempData["SuccessMessage"] = "Payment successful!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Payment failed.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error processing payment: {ex.Message}";
            }

            return RedirectToAction("FMaintenanceList");
        }



    }
}
