using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Text.Json;
using RFID_.NET_MVC_PROJECT.Models;
namespace RFID_.NET_MVC_PROJECT.Controllers
{
    public class AdminController : Controller
    {
       
        public IActionResult AdminLogin()
        {
            return View("~/Views/AdminPanal/AdminLogin.cshtml");
        }
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]

        public IActionResult AdminDashboard()
        {
            return View("~/Views/AdminPanal/AdminDashboard.cshtml");
        }
        public async Task<IActionResult> ManageClients()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                try
                {
                    var response = await client.GetAsync("getAllClients");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<getClientsApiResponse>(result);

                        if (apiResponse?.success == true)
                        {
                            var users = apiResponse.data;
                            if (users != null && users.Any())
                            {
                                // log or debug here
                                Console.WriteLine("Clients fetched successfully: " + users.Count);
                            }
                            else
                            {
                                Console.WriteLine("No Clients found in response.");
                            }
                            return PartialView("~/Views/AdminPanal/ManageClients.cshtml", users);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Failed to fetch Clients. Please try again later.";
                            return PartialView("~/Views/AdminPanal/ManageClients.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to fetch Clients. Please try again later.";
                        return PartialView("~/Views/AdminPanal/ManageClients.cshtml");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return PartialView("~/Views/AdminPanal/ManageClients.cshtml");
                }
            }
        }
        public IActionResult EditPassword()
        {

            return PartialView("~/Views/AdminPanal/EditPassword.cshtml"); // Return a partial view
        }
        public async Task<IActionResult> ManageUsers()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                try
                {
                    var response = await client.GetAsync("getAllUsers");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<getUsersApiResponse>(result);

                        if (apiResponse?.success == true)
                        {
                            var users = apiResponse.data;
                            if (users != null && users.Any())
                            {
                                // log or debug here
                                Console.WriteLine("Users fetched successfully: " + users.Count);
                            }
                            else
                            {
                                Console.WriteLine("No users found in response.");
                            }
                            return PartialView("~/Views/AdminPanal/ManageUsers.cshtml", users);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Failed to fetch users. Please try again later.";
                            return PartialView("~/Views/AdminPanal/ManageUsers.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to fetch users. Please try again later.";
                        return PartialView("~/Views/AdminPanal/ManageUsers.cshtml");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return PartialView("~/Views/AdminPanal/ManageUsers.cshtml");
                }
            }
        }


        public async Task<IActionResult> AdminTokensDetails()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                try
                {
                    var response = await client.GetAsync("getTokensForAdmin");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<getTokensDetailsApiResponse>(result);

                        if (apiResponse?.success == true)
                        {
                            var users = apiResponse.data;
                            if (users != null && users.Any())
                            {
                                // log or debug here
                                Console.WriteLine("TokensDetails fetched successfully: " + users.Count);
                            }
                            else
                            {
                                Console.WriteLine("No TokensDetails found in response.");
                            }
                            return PartialView("~/Views/AdminPanal/TokensDetails.cshtml", users);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Failed to fetch TokensDetails. Please try again later.";
                            return PartialView("~/Views/AdminPanal/TokensDetails.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to fetch TokensDetails. Please try again later.";
                        return PartialView("~/Views/AdminPanal/TokensDetails.cshtml");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return PartialView("~/Views/AdminPanal/TokensDetails.cshtml");
                }
            }
        }
        public async Task<IActionResult> PurchasedTokens()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                try
                {
                    var response = await client.GetAsync("getPurchasedTokens");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<getPurchasedTokensApiResponse>(result);

                        if (apiResponse?.success == true)
                        {
                            var users = apiResponse.data;
                            if (users != null && users.Any())
                            {
                                // log or debug here
                                Console.WriteLine("PurchasedTokens fetched successfully: " + users.Count);
                            }
                            else
                            {
                                Console.WriteLine("No PurchasedTokens found in response.");
                            }
                            return PartialView("~/Views/AdminPanal/PurchasedTokens.cshtml", users);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Failed to fetch PurchasedTokens. Please try again later.";
                            return PartialView("~/Views/AdminPanal/PurchasedTokens.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to fetch PurchasedTokens. Please try again later.";
                        return PartialView("~/Views/AdminPanal/PurchasedTokens.cshtml");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return PartialView("~/Views/AdminPanal/PurchasedTokens.cshtml");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(string username, string password)
        {
            var requestBody = new { username, password };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");

                try
                {
                    var response = await client.PostAsJsonAsync("adminlogin", requestBody);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        using (JsonDocument doc = JsonDocument.Parse(result))
                        {
                            var root = doc.RootElement;
                            if (root.TryGetProperty("success", out var success) && success.GetBoolean())
                            {
                                var cookieOptions = new CookieOptions
                                {
                                    HttpOnly = true, // makes the cookie inaccessible to JavaScript
                                    SameSite = SameSiteMode.Strict, // prevent cross-site request forgery
                                    Expires = DateTimeOffset.UtcNow.AddHours(12) // expires after 1 hour
                                };

                                // Set the Secure flag based on the current protocol (HTTP or HTTPS)
                                if (Request.IsHttps)
                                {
                                    cookieOptions.Secure = true; // Only send cookie over HTTPS
                                }
                                else
                                {
                                    cookieOptions.Secure = false; // Allow cookie over HTTP
                                }

                                HttpContext.Response.Cookies.Append("Username", username, cookieOptions);
                                return RedirectToAction("AdminDashboard", "Admin");

                            }
                        }
                    }

                    ViewBag.ErrorMessage = "Invalid credentials!";
                    return View("~/Views/AdminPanal/AdminLogin.cshtml");
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return View("~/Views/AdminPanal/AdminLogin.cshtml");
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditPassword(string username, string currentPassword, string newPassword)
        {
            var usernamecookes = Request.Cookies["username"];

            var requestBody = new
            {
                username= usernamecookes,
                current_password = currentPassword, 
                new_password = newPassword
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");  

                try
                {
                    var response = await client.PutAsJsonAsync("adminChangePassword", requestBody);
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

            return View("~/Views/AdminPanal/EditPassword.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(int user_id, int current_status)
        {
            // Toggle the status (0 -> 1 or 1 -> 0)
            var newStatus = current_status == 0 ? 1 : 0;

            // Prepare the request data for the API call
            var requestBody = new
            {
                user_id = user_id,
                status = newStatus
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");  // Your API base URL

                try
                {
                    var response = await client.PutAsJsonAsync("updateUserStatus", requestBody);
                    var result = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

                    if (apiResponse?.success == true)
                    {
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
            return RedirectToAction("ManageUsers");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateClientStatus(int client_id, int current_status)
        {
            // Toggle the status (0 -> 1 or 1 -> 0)
            var newStatus = current_status == 0 ? 1 : 0;

            // Prepare the request data for the API call
            var requestBody = new
            {
                client_id = client_id,
                status = newStatus
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");  // Your API base URL

                try
                {
                    var response = await client.PutAsJsonAsync("updateClientStatus", requestBody);
                    var result = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

                    if (apiResponse?.success == true)
                    {
                        TempData["SuccessMessage"] = "client status updated successfully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update client status. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error occurred while updating client status: " + ex.Message;
                }
            }

            // Redirect back to the ManageUsers view
            return RedirectToAction("ManageClients");
        }
        [HttpPost]
        public async Task<IActionResult> EditToken(int tokenId, string name, decimal price, int duration_day, string description,int  status)
        {
           

            
            var requestBody = new
            {
                token_id = tokenId,
                name = name,
                price= price,
                duration_day= duration_day,
                description= description,
                status= status
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");  // Your API base URL

                try
                {
                    var response = await client.PutAsJsonAsync("editTokenDetails", requestBody);
                    var result = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

                    if (apiResponse?.success == true)
                    {
                        TempData["SuccessMessage"] = "TokenDetails updated successfully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update TokenDetails. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error occurred while updating TokenDetails: " + ex.Message;
                }
            }

            // Redirect back to the ManageUsers view
            return RedirectToAction("AdminTokensDetails");
        }

        [HttpPost]
        public async Task<IActionResult> AddToken( string name, decimal price, int duration_day, string description, int status)
        {



            var requestBody = new
            {
               
                name = name,
                price = price,
                duration_day = duration_day,
                description = description,
                status = status
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");  // Your API base URL

                try
                {
                    var response = await client.PostAsJsonAsync("addNewToken", requestBody);
                    var result = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

                    if (apiResponse?.success == true)
                    {
                        TempData["SuccessMessage"] = "TokenDetails Add successfully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to Add TokenDetails. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error occurred while Add TokenDetails: " + ex.Message;
                }
            }

            // Redirect back to the ManageUsers view
            return RedirectToAction("AdminTokensDetails");
        }

        [HttpGet]

        public IActionResult Logout()
        {
            // Remove "Username" cookie
            if (Request.Cookies["Username"] != null)
            {
                Response.Cookies.Delete("Username");
            }

            // Optionally clear session
            HttpContext.Session.Clear();

            // Redirect to admin login page
            return RedirectToAction("AdminLogin", "Admin");
        }

    }
}
