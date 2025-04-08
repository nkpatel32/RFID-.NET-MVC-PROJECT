using Microsoft.AspNetCore.Mvc;
using RFID_.NET_MVC_PROJECT.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RFID_.NET_MVC_PROJECT.Controllers
{
    public class UserController : Controller
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
        }

        public string GetUserDataFromCookie(string propertyName)
        {
            var clientDataJson = Request.Cookies["User_data"];

            if (string.IsNullOrEmpty(clientDataJson))
            {
                return null;
            }

            try
            {

                using (JsonDocument doc = JsonDocument.Parse(clientDataJson))
                {
                    var root = doc.RootElement;


                    if (root.TryGetProperty(propertyName, out var propertyValue))
                    {

                        if (propertyValue.ValueKind == JsonValueKind.String)
                        {
                            return propertyValue.GetString();
                        }
                        else if (propertyValue.ValueKind == JsonValueKind.Number)
                        {
                            return propertyValue.GetInt32().ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (JsonException)
            {
                return null;
            }
        }

        public bool SetUserDataInCookie(string cookieName, string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData))
            {
                return false; // Invalid JSON data
            }

            try
            {
                // Validate if the JSON is correctly formatted
                JsonDocument.Parse(jsonData);

                // Determine if the request is over HTTPS
                bool isSecure = Request.IsHttps;

                // Set the cookie with the provided JSON data
                Response.Cookies.Append(cookieName, jsonData, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(1), // Adjust expiration as needed
                    HttpOnly = true,
                    Secure = isSecure // Only set Secure flag to true if the request is HTTPS
                });

                return true;
            }
            catch (JsonException)
            {
                return false; // Invalid JSON format, return false
            }
        }



        public IActionResult UserLogin()
        {
            return View("~/Views/UserPanal/UserLogin.cshtml");
        }

        public IActionResult UserRegister()
        {
            return View("~/Views/UserPanal/UserRegister.cshtml");
        }
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]

        public IActionResult UserDashboard()
        {
            string clientId = GetUserDataFromCookie("user_id");
            if (clientId == null)
            {
                return RedirectToAction("UserLogin");
            }
            return View("~/Views/UserPanal/UserDashboard.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(string email, string password)
        {
            var requestBody = new { email, password };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");

                try
                {
                    var response = await client.PostAsJsonAsync("userLogin", requestBody);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        using (JsonDocument doc = JsonDocument.Parse(result))
                        {
                            var root = doc.RootElement;
                            if (root.TryGetProperty("success", out var success) && success.GetBoolean())
                            {
                                var userData = new
                                {
                                    success = success.GetBoolean(),

                                    user_id = root.GetProperty("user_id").GetInt32(),
                                    user_name = root.GetProperty("user_name").GetString(),
                                    user_email = root.GetProperty("user_email").GetString(),
                                    user_mobile = root.GetProperty("user_mobile").GetString()
                                };

                                string userDataJson = JsonSerializer.Serialize(userData);

                                SetUserDataInCookie("User_data", userDataJson);
                                ViewBag.SuccessMessage = "Login successful!";
                                return RedirectToAction("UserDashboard", "User");
                            }
                        }
                    }

                    ViewBag.ErrorMessage = "Invalid credentials!";
                    return View("~/Views/UserPanal/UserLogin.cshtml");
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return View("~/Views/UserPanal/UserLogin.cshtml");
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> UserRegister(string FullName, string Email, string Password, string Mobile)
        {
            var requestBody = new { name = FullName, email = Email, password = Password, mobile = Mobile };


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("userRegister", content);
                    var result = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("API Response: " + result);

                    if (response.IsSuccessStatusCode)
                    {
                        using (JsonDocument doc = JsonDocument.Parse(result))
                        {
                            var root = doc.RootElement;
                            if ((root.TryGetProperty("success", out var success) || root.TryGetProperty("Success", out success))
                                && success.GetBoolean())
                            {
                                ViewBag.SuccessMessage = "Registration successful!";
                                return View("~/Views/UserPanal/UserRegister.cshtml");
                            }
                        }
                    }

                    ViewBag.ErrorMessage = "Registration failed!";
                    return View("~/Views/UserPanal/UserRegister.cshtml");
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return View("~/Views/UserPanal/UserRegister.cshtml");
                }
            }

        }


        public async Task<IActionResult> GetSubject()

        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                string userId = GetUserDataFromCookie("user_id");
                if (userId == null)
                {
                    return RedirectToAction("UserLogin");
                }
                try
                {
                    var response = await client.GetAsync($"getUserSubjects?user_id={userId}");

                    if (response.IsSuccessStatusCode)
                    {

                        var result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<getUserSubjectApiResponse>(result);

                        if (apiResponse?.success == true)
                        {
                            return Json(apiResponse.data);
                        }
                        else
                        {

                            return Json(new { message = "Failed to fetch subjects. Please try again later." });
                        }
                    }
                    else
                    {

                        return Json(new { message = "Failed to fetch subjects. Please try again later." });
                    }
                }
                catch (HttpRequestException ex)
                {

                    return Json(new { message = $"Server connection error: {ex.Message}" });
                }
            }
        }
        public IActionResult EditPassword()
        {

            return PartialView("~/Views/UserPanal/EditPassword.cshtml"); // Return a partial view
        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(string currentPassword, string newPassword)
        {
            string userId = GetUserDataFromCookie("user_id");
            if (userId == null)
            {
                return RedirectToAction("UserLogin");
            }

            var requestBody = new
            {
                user_id = userId,
                current_password = currentPassword,
                new_password = newPassword
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");

                try
                {
                    var response = await client.PutAsJsonAsync("userChangePassword", requestBody);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        using (JsonDocument doc = JsonDocument.Parse(result))
                        {
                            var root = doc.RootElement;
                            if (root.TryGetProperty("success", out var success) && success.GetBoolean())
                            {
                                ViewBag.SuccessMessage = "Password changed successfully!";
                            }
                            else
                            {
                                ViewBag.ErrorMessage = root.GetProperty("message").GetString() ?? "Password change failed. Please try again.";
                            }
                        }
                    }
                    else
                    {
                        using (JsonDocument doc = JsonDocument.Parse(result))
                        {
                            var root = doc.RootElement;
                            if (root.TryGetProperty("message", out var message))
                            {
                                ViewBag.ErrorMessage = message.GetString();
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Password change failed. Please try again.";
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                }
            }

            return View("~/Views/UserPanal/EditPassword.cshtml");
        }
        public IActionResult EditProfile()
        {
            string user_name = GetUserDataFromCookie("user_name");
            string user_email = GetUserDataFromCookie("user_email");
            string user_mobile = GetUserDataFromCookie("user_mobile");
            ViewBag.userName = user_name;
            ViewBag.userEmail = user_email;
            ViewBag.userMobile = user_mobile;

            return PartialView("~/Views/UserPanal/EditProfile.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(string name, string email, string mobile)
        {

            string userId = GetUserDataFromCookie("user_id");
            if (userId == null)
            {

                return RedirectToAction("UserLogin");
            }
            // Prepare the request data for the API call
            var requestBody = new
            {
                user_id = userId,
                name = name,
                email = email,
                mobile = mobile
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");  // Your API base URL

                try
                {
                    var response = await client.PutAsJsonAsync("userUpdate", requestBody);
                    var result = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

                    if (apiResponse?.success == true)
                    {
                        string jsonData = $"{{\"user_id\":\"{userId}\",\"user_name\":\"{name}\",\"user_email\":\"{email}\",\"user_mobile\":\"{mobile}\"}}";
                        SetUserDataInCookie("User_data", jsonData);
                        TempData["SuccessMessage"] = "User status updated successfully!";

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update user status. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error occurred while updating user status: " + ex.Message;
                }
            }

            // Redirect back to the ManageUsers view
            return RedirectToAction("EditProfile");
        }
        [HttpGet]
        public async Task<IActionResult> UserSubject(getCtIdAndSubjectName request)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                string userId = GetUserDataFromCookie("user_id");

                if (userId == null)
                {
                    return RedirectToAction("UserLogin");
                }

                try
                {
                    var response = await client.GetAsync($"getUserSubjectDetalis?user_id={userId}&ct_id={request.ct_id}");

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<getSubjectDetailForUserApiResponse>(result);

                        if (apiResponse?.success == true)
                        {
                            var viewModel = new UserSubjectViewModel
                            {
                                Info = request,  // Pass the ct_id and subject_name
                                UserSubjectDetailForUser = apiResponse.data // Pass the list of SubjectDetails
                            };

                            return PartialView("~/Views/UserPanal/UserSubject.cshtml", viewModel);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Failed to fetch ClientSubject Details. Please try again later.";
                            return PartialView("~/Views/UserPanal/UserSubject.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to fetch subject details. Please try again later.";
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                }

                // Return view with error message if fetching fails
                return PartialView("~/Views/UserPanal/UserSubject.cshtml");
            }
        }

        public IActionResult viewAttendanceofUser(int ct_id, string subject_name)
        {
            var model = new getCtIdAndSubjectName
            {
                ct_id = ct_id,
                subject_name = subject_name
            };
            return View("~/Views/UserPanal/viewAttendanceofUser.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendanceforUser(int ct_id, DateTime from_date, DateTime to_date)
        {
            string userId = GetUserDataFromCookie("user_id");

            if (userId == null)
            {
                return RedirectToAction("UserLogin");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/"); // Your API base URL

                try
                {
                    // Constructing URL with query parameters
                    string url = $"getPunchRecordByUser?user_id={userId}&ct_id={ct_id}&from_date={from_date:yyyy-MM-dd}&to_date={to_date:yyyy-MM-dd}";

                    // Sending GET request to the API
                    var response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<AttendanceRecordforUserapiresponse>(result);

                    if (apiResponse?.success == true)
                    {
                        return Json(new { success = true, data = apiResponse.data });
                    }   
                    else
                    {
                        return Json(new { success = false, message = "Failed to retrieve attendance records." });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error occurred while fetching attendance: " + ex.Message });
                }
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
            // Remove "User_data" cookie
            if (Request.Cookies["User_data"] != null)
            {
                Response.Cookies.Delete("User_data");
            }

            // Optionally clear session
            HttpContext.Session.Clear();

            // Redirect to login or home
            return RedirectToAction("UserLogin", "User");
        }



    }
}