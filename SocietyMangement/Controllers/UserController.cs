using Microsoft.AspNetCore.Mvc;
using SocietyMangement.Models;
using System.Text;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Data;
using Azure.Identity;
using System.Net.Http.Headers;

namespace SocietyMangement.Controllers
{
    public class UserController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7057/api/User");
        private readonly HttpClient _client;
        private IConfiguration configuration;

        public UserController(IConfiguration _configuration)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            configuration = _configuration;
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
        public async Task<IActionResult> UserList()
        {
            SetAuthorizationHeader();
            List<UserModel> users = new List<UserModel>();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<List<UserModel>>(data);
                }
                else
                {
                    ModelState.AddModelError("", "Error fetching data.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Exception: {ex.Message}");
            }
            return View(users);
        }

        public async Task<IActionResult> UserAddEdit(int? UserID)
        {
            SetAuthorizationHeader();
            await GetFlatNumber();
            if (UserID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/{UserID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserModel>(data);

                    return View(user);
                }
            }
            return View(new UserModel());
        }

        [HttpPost]
        public async Task<IActionResult> UserSave(UserModel user)
        {
            SetAuthorizationHeader();
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;
                Console.WriteLine(content);

                if (user.UserID == 0)
                {
                    response = await _client.PostAsync(_client.BaseAddress, content);
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/{user.UserID}", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("UserList");
                }
            }

            return View("UserAddEdit", user);
        }

        public async Task<IActionResult> Delete(int UserID)
        {
            SetAuthorizationHeader();
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/{UserID}");
            return RedirectToAction("UserList");
        }

        //public IActionResult Login()
        //{
        //    return View();
        //}

        //public IActionResult UserLogin(UserLoginModel userLoginModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            string connectionString = this.configuration.GetConnectionString("ConnectionString");
        //            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
        //                {
        //                    sqlCommand.CommandType = CommandType.StoredProcedure;
        //                    sqlCommand.CommandText = "PR_User_Login";
        //                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
        //                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
        //                    sqlCommand.Parameters.Add("@Role", SqlDbType.VarChar).Value = userLoginModel.Role;

        //                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
        //                    {
        //                        DataTable dataTable = new DataTable();
        //                        dataTable.Load(sqlDataReader);

        //                        if (dataTable.Rows.Count > 0)
        //                        {
        //                            foreach (DataRow dr in dataTable.Rows)
        //                            {
        //                                HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
        //                                HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
        //                                HttpContext.Session.SetString("Role", dr["Role"].ToString());
        //                            }

        //                            if (HttpContext.Session.GetString("Role") == "Admin")
        //                            {
        //                                return RedirectToAction("VisitorList", "Visitor");
        //                            }
        //                            else
        //                            {
        //                                return RedirectToAction("GallaryList", "Gallary");
        //                            }


        //                        }
        //                        else
        //                        {
        //                            TempData["ErrorMessage"] = "Invalid credentials or role!";
        //                            return View("Login");
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        TempData["ErrorMessage"] = e.Message;
        //    }

        //    return View("Login");
        //}



        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Clear();
        //    return RedirectToAction("Login", "User");
        //}

        //public IActionResult Register()
        //{
        //    return View();
        //}

        //public IActionResult UserRegister(UserRegisterModel userRegisterModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            string connectionString = this.configuration.GetConnectionString("ConnectionString");
        //            SqlConnection sqlConnection = new SqlConnection(connectionString);
        //            sqlConnection.Open();
        //            SqlCommand sqlCommand = sqlConnection.CreateCommand();
        //            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
        //            sqlCommand.CommandText = "PR_User_Register";
        //            sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userRegisterModel.UserName;
        //            sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userRegisterModel.Password;
        //            sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = userRegisterModel.Email;
        //            sqlCommand.Parameters.Add("@PhoneNumber", SqlDbType.VarChar).Value = userRegisterModel.PhoneNumber;
        //            sqlCommand.Parameters.Add("@Role", SqlDbType.VarChar).Value = userRegisterModel.Role;

        //            sqlCommand.ExecuteNonQuery();
        //            return RedirectToAction("Login", "User");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        TempData["ErrorMessage"] = e.Message;
        //        return RedirectToAction("Register");
        //    }
        //    return RedirectToAction("Register");
        //}





        // Login Action
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(userLoginModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Call the Auth API
                    var response = await _client.PostAsync("https://localhost:7057/api/Auth/auth", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<LoginResponse>(data);

                        if (result != null && !string.IsNullOrEmpty(result.Token))
                        {
                            // Store Token and User Data in Session
                            HttpContext.Session.SetString("JWTToken", result.Token);
                            HttpContext.Session.SetString("UserID", result.User.UserID.ToString());
                            HttpContext.Session.SetString("UserName", result.User.UserName);
                            HttpContext.Session.SetString("Role", result.User.Role);

                            // Role-based redirection
                            if (result.User.Role == "Admin")
                            {
                                return RedirectToAction("VisitorList", "Visitor");
                            }
                            else if (result.User.Role == "Resident")
                            {
                                return RedirectToAction("GallaryList", "Gallary");
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Invalid credentials!";
                            return View("Login");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error connecting to the API!";
                        return View("Login");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return View("Login");
        }



        // Logout Action
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            Response.Cookies.Delete("AuthToken"); // Delete JWT token if stored in a cookie

            return RedirectToAction("Login", "User");
        }


        // Register Action
        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> UserRegister(UserRegisterModel userRegisterModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(userRegisterModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Make API call to register user
                    var response = await _client.PostAsync($"{_client.BaseAddress}/register", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<dynamic>(data);

                        if (result != null && result.Message == "User registered successfully")
                        {
                            TempData["SuccessMessage"] = "Registration successful. Please login to continue.";
                            return RedirectToAction("Login", "User");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = result.Message ?? "An error occurred during registration.";
                            return View("Register");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error connecting to the API!";
                        return View("Register");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Register");
            }

            return RedirectToAction("Register");
        }



        private async Task GetFlatNumber()
        {
            SetAuthorizationHeader();
            try
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/flatnumber");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var flatnumber = JsonConvert.DeserializeObject<List<GetFlatnumber>>(data);
                    ViewBag.FlatNumber = flatnumber;
                }
                else
                {
                    ViewBag.FlatNumber = new List<GetFlatnumber>(); // Empty list if API fails
                }
            }
            catch
            {
                ViewBag.FlatNumber = new List<GetFlatnumber>(); // Empty list in case of exception
            }
        }
    }
}
