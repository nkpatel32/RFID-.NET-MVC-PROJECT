using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RFID_.NET_MVC_PROJECT.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult ClientLogin()
        {
            return View("~/Views/ClientPanal/ClientLogin.cshtml");
        }

        public IActionResult ClientRegister()
        {
            return View("~/Views/ClientPanal/ClientRegister.cshtml");
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
                        // Parse JSON response
                        using (JsonDocument doc = JsonDocument.Parse(result))
                        {
                            var root = doc.RootElement;
                            if (root.TryGetProperty("success", out var success) && success.GetBoolean())
                            {
                                ViewBag.SuccessMessage = "Login successful!";
                                return View("~/Views/ClientPanal/ClientLogin.cshtml"); // Stay on the same page
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

                    Console.WriteLine("API Response: " + result); // Debugging output

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
    }
}
