using Microsoft.AspNetCore.Mvc;
using RFID_.NET_MVC_PROJECT.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using RFID_.NET_MVC_PROJECT.Models;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace RFID_.NET_MVC_PROJECT.Controllers
{
    public class ClientController : Controller
    {

        public string GetClientDataFromCookie(string propertyName)
        {
            var clientDataJson = Request.Cookies["Client_data"];

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

        public bool SetClientDataInCookie(string cookieName, string jsonData)
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



        public IActionResult ClientLogin()
        {
            return View("~/Views/ClientPanal/ClientLogin.cshtml");
        }

        public IActionResult ClientRegister()
        {
            return View("~/Views/ClientPanal/ClientRegister.cshtml");
        }
        public IActionResult ClientDashboard()
        {
            string clientId = GetClientDataFromCookie("client_id");
            if (clientId == null)
            {
                return RedirectToAction("ClientLogin");
            }
            return View("~/Views/ClientPanal/ClientDashboard.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ClientLogin(string email, string password)
        {
            var requestBody = new { email, password };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");

                try
                {
                    var response = await client.PostAsJsonAsync("clientlogin", requestBody);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        using (JsonDocument doc = JsonDocument.Parse(result))
                        {
                            var root = doc.RootElement;
                            if (root.TryGetProperty("success", out var success) && success.GetBoolean())
                            {
                                var clientData = new
                                {
                                    success = success.GetBoolean(),

                                    client_id = root.GetProperty("client_id").GetInt32(),
                                    client_name = root.GetProperty("client_name").GetString(),
                                    client_email = root.GetProperty("client_email").GetString(),
                                    client_mobile = root.GetProperty("client_mobile").GetString()
                                };

                                string clientDataJson = JsonSerializer.Serialize(clientData);

                                SetClientDataInCookie("Client_data", clientDataJson);
                                ViewBag.SuccessMessage = "Login successful!";
                                return RedirectToAction("ClientDashboard", "Client");
                            }
                        }
                    }

                    ViewBag.ErrorMessage = "Invalid credentials!";
                    return View("~/Views/ClientPanal/ClientLogin.cshtml");
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return View("~/Views/ClientPanal/ClientLogin.cshtml");
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> ClientRegister(string FullName, string Email, string Password, string Mobile)
        {
            var requestBody = new { name = FullName, email = Email, password = Password, mobile = Mobile };


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("clientRegister", content);
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
                                return View("~/Views/ClientPanal/ClientRegister.cshtml");
                            }
                        }
                    }

                    ViewBag.ErrorMessage = "Registration failed!";
                    return View("~/Views/ClientPanal/ClientRegister.cshtml");
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return View("~/Views/ClientPanal/ClientRegister.cshtml");
                }
            }

        }


        public async Task<IActionResult> GetSubject()

        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                string clientId = GetClientDataFromCookie("client_id");
                if (clientId == null)
                {
                    return RedirectToAction("ClientLogin");
                }
                try
                {
                    var response = await client.GetAsync($"getClientSubjects?client_id={clientId}");

                    if (response.IsSuccessStatusCode)
                    {

                        var result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<getSUbjectApiResponse>(result);

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
        public async Task<IActionResult> ClientSubject(getCtIdAndSubjectName request)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                try
                {
                    var response = await client.GetAsync($"getClientSubjectDetails?ct_id={request.ct_id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<getSubjectDetailApiResponse>(result);

                        if (apiResponse?.success == true)
                        {
                            // Prepare the view model
                            var viewModel = new ClientSubjectViewModel
                            {
                                Info = request,  // Pass the ct_id and subject_name
                                SubjectDetails = apiResponse.data // Pass the list of SubjectDetails
                            };

                            return PartialView("~/Views/ClientPanal/ClientSubject.cshtml", viewModel);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Failed to fetch ClientSubject Details. Please try again later.";
                            return PartialView("~/Views/ClientPanal/ClientSubject.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to fetch ClientSubject Details. Please try again later.";
                        return PartialView("~/Views/ClientPanal/ClientSubject.cshtml");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return PartialView("~/Views/ClientPanal/ClientSubject.cshtml");
                }

                return View("~/Views/ClientPanal/ClientSubject.cshtml");
            }
        }


        public IActionResult EditPassword()
        {

            return PartialView("~/Views/ClientPanal/EditPassword.cshtml"); // Return a partial view
        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(string currentPassword, string newPassword)
        {
            string clientId = GetClientDataFromCookie("client_id");
            if (clientId == null)
            {
                return RedirectToAction("ClientLogin");
            }

            var requestBody = new
            {
                client_id = clientId,
                current_password = currentPassword,
                new_password = newPassword
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");

                try
                {
                    var response = await client.PutAsJsonAsync("clientChangePassword", requestBody);
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

            return View("~/Views/ClientPanal/EditPassword.cshtml");
        }
        public IActionResult EditProfile()
        {
            string client_name = GetClientDataFromCookie("client_name");
            string client_email = GetClientDataFromCookie("client_email");
            string client_mobile = GetClientDataFromCookie("client_mobile");
            ViewBag.clientName = client_name;
            ViewBag.clientEmail = client_email;
            ViewBag.clientMobile = client_mobile;

            return PartialView("~/Views/ClientPanal/EditProfile.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateClient(string name, string email, string mobile)
        {

            string clientId = GetClientDataFromCookie("client_id");
            if (clientId == null)
            {

                return RedirectToAction("ClientLogin");
            }
            // Prepare the request data for the API call
            var requestBody = new
            {
                client_id = clientId,
                name = name,
                email = email,
                mobile = mobile
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");  // Your API base URL

                try
                {
                    var response = await client.PutAsJsonAsync("clientUpdate", requestBody);
                    var result = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

                    if (apiResponse?.success == true)
                    {
                        string jsonData = $"{{\"client_id\":\"{clientId}\",\"client_name\":\"{name}\",\"client_email\":\"{email}\",\"client_mobile\":\"{mobile}\"}}";
                        SetClientDataInCookie("Client_data", jsonData);
                        TempData["SuccessMessage"] = "Client status updated successfully!";

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
        public async Task<IActionResult> AddNewSubject()
        {



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                try
                {
                    var response = await client.GetAsync("/getTokensForClient");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<getTokensForClientApiResponse>(result);

                        if (apiResponse?.success == true)
                        {
                            var users = apiResponse.data;
                            if (users != null && users.Any())
                            {
                                // log or debug here
                                Console.WriteLine("Tokens fetched successfully: " + users.Count);
                            }
                            else
                            {
                                Console.WriteLine("No Tokens found in response.");
                            }
                            return PartialView("~/Views/ClientPanal/AddNewSubject.cshtml", users);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Failed to fetch Tokens. Please try again later.";
                            return PartialView("~/ Views / ClientPanal / AddNewSubject.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to fetch Tokens. Please try again later.";
                        return PartialView("~/Views/ClientPanal/AddNewSubject.cshtml");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return PartialView("~/Views/ClientPanal/AddNewSubject.cshtml");
                }
            }



        }
        public async Task<IActionResult> ProcideToAdd(int token_id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                try
                {
                    var response = await client.GetAsync($"/getTokenById?token_id={token_id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(result); // Log the raw response to verify the structure

                        var apiResponse = JsonSerializer.Deserialize<getTokenDetailsApiResponse>(result);

                        if (apiResponse?.success == true && apiResponse.data != null)
                        {
                            var token = apiResponse.data;
                            Console.WriteLine("Token fetched successfully: " + token.name);
                            return PartialView("~/Views/ClientPanal/ProcideToAdd.cshtml", token);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Failed to fetch token. Please try again later.";
                            return PartialView("~/Views/ClientPanal/ProcideToAdd.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to fetch token. Please try again later.";
                        return PartialView("~/Views/ClientPanal/ProcideToAdd.cshtml");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return PartialView("~/Views/ClientPanal/ProcideToAdd.cshtml");
                }
            }
        }
        public async Task<IActionResult> BuySubject(int token_id, string subject_name, string pass_key, string purchase_date, string expire_date)
        {
            string clientId = GetClientDataFromCookie("client_id");
            if (clientId == null)
            {
                return RedirectToAction("ClientLogin");
            }

            // Prepare the request data for the API call
            var requestBody = new
            {
                token_id = token_id,
                pass_key = pass_key,
                subject_name = subject_name,
                client_id = clientId,
                purchase_date = purchase_date,
                expire_date = expire_date,
                status = "0" // assuming status is 0 initially
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");  // Your Express.js API URL

                try
                {
                    // Send the POST request with the request body serialized as JSON
                    var response = await client.PostAsJsonAsync("addNewSubject", requestBody);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

                        if (apiResponse?.success == true)
                        {
                            TempData["SuccessMessage"] = "New subject added successfully!";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = $"Failed to add new subject";
                        }
                    }
                    else
                    {
                        // Handle unsuccessful HTTP response (non-2xx)
                        TempData["ErrorMessage"] = "Failed to connect to the API or unexpected API response.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error occurred while updating user status: " + ex.Message;
                }
            }

            return RedirectToAction("ClientDashboard");
        }

        public async Task<IActionResult> getUsersWhichInSubject(getCtIdAndSubjectName request)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                try
                {
                    var response = await client.GetAsync($"getUserWhichInSubject?ct_id={request.ct_id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<getUsersWhichInSubjectApiResponse>(result);

                        if (apiResponse?.success == true)
                        {
                            var viewModel = new UsersWhichInSubjectModel
                            {
                                Info = request,  
                                UsersWhichInSubject = apiResponse.data                             };
                            return PartialView("~/Views/ClientPanal/UsersOfSubject.cshtml", viewModel);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Failed to fetch Users. Please try again later.";
                            return PartialView("~/Views/ClientPanal/UsersOfSubject.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to fetch Users. Please try again later.";
                        return PartialView("~/Views/ClientPanal/UsersOfSubject.cshtml");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                    return PartialView("~/Views/ClientPanal/UsersOfSubject.cshtml");
                }
            }
        }


        [HttpPut]
        public async Task<IActionResult> EditUserInSubject([FromBody] EditUserRequest request)
        {
            var requestBody = new
            {
                user_id = request.user_id,
                ct_id = request.ct_id,
                designation = request.Designation,
                rfid = request.Rfid
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/"); // Your API base URL

                try
                {
                    var response = await client.PutAsJsonAsync("editUserInsubject", requestBody);
                    var result = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

                    if (apiResponse?.success == true)
                    {
                        return Json(new { success = true, message = "User status updated successfully!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to update user status. Please try again." });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error occurred while updating user status: " + ex.Message });
                }
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUserInSubject([FromBody] DeleteUserRequest request)
        {
            var requestBody = new
            {
                user_id = request.user_id,
                ct_id = request.ct_id
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/"); 

                try
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "deleteUserFromSubject")
                    {
                        Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                    };

                    var response = await client.SendAsync(requestMessage);

                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

                    if (apiResponse?.success == true)
                    {
                        return Json(new { success = true, message = "User deleted successfully!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to delete user. Please try again." });
                    }
                }
                catch (HttpRequestException ex)
                {
                    return Json(new { success = false, message = "HTTP Error: " + ex.Message });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error occurred while deleting user: " + ex.Message });
                }
            }
        }



    }
    public class EditUserRequest
    {
        public string user_id { get; set; }
        public string ct_id { get; set; }
        public string Designation { get; set; }
        public string Rfid { get; set; }
    }
    public class DeleteUserRequest
    {
        public string user_id { get; set; }
        public string ct_id { get; set; }
       
    }
}