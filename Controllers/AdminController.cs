using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace RFID_.NET_MVC_PROJECT.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminLogin()
        {
            return View("~/Views/AdminPanal/AdminLogin.cshtml");
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
                                ViewBag.SuccessMessage = "Login successful!";
                                return View("~/Views/AdminPanal/AdminLogin.cshtml"); // Stay on the same page and show success message
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


    }
}
