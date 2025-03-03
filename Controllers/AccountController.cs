using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public class AccountController : Controller
{
    public IActionResult UserLogin()
    {
        return View();
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
                var response = await client.PostAsJsonAsync("userlogin", requestBody);
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
                            return View(); // Stay on the same page
                        }
                    }
                }

                ViewBag.ErrorMessage = "Invalid credentials!";
                return View();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                return View();
            }
        }
    }

    public IActionResult ClientLogin()
    {
        return View();
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
                            ViewBag.SuccessMessage = "Login successful!";
                            return View();
                        }
                    }
                }

                ViewBag.ErrorMessage = "Invalid credentials!";
                return View();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                return View();
            }
        }
    }
    public IActionResult UserRegister()
    {
        return View();
    }

    // POST: Account/UserRegister
    [HttpPost]
    public async Task<IActionResult> UserRegister(string name, string email, string password, string mobile)
    {
        var requestBody = new { name, email, password, mobile };

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:3000/"); // Change this to your API URL

            try
            {
                var response = await client.PostAsJsonAsync("userRegister", requestBody);
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using (JsonDocument doc = JsonDocument.Parse(result))
                    {
                        var root = doc.RootElement;
                        if (root.TryGetProperty("message", out var message) && message.GetString() == "User registered successfully")
                        {
                            ViewBag.SuccessMessage = "Registration successful!";
                            return View();
                        }
                    }
                }

                ViewBag.ErrorMessage = "Registration failed!";
                return View();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                return View();
            }
        }
    }
    public IActionResult ClienRegister()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ClientRegister(string name, string email, string password, string mobile)
    {
        var requestBody = new { name, email, password, mobile };

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:3000/"); // Change this to your API URL

            try
            {
                var response = await client.PostAsJsonAsync("userRegister", requestBody);
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using (JsonDocument doc = JsonDocument.Parse(result))
                    {
                        var root = doc.RootElement;
                        if (root.TryGetProperty("message", out var message) && message.GetString() == "Client registered successfully")
                        {
                            ViewBag.SuccessMessage = "Registration successful!";
                            return View();
                        }
                    }
                }

                ViewBag.ErrorMessage = "Registration failed!";
                return View();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = "Server connection error: " + ex.Message;
                return View();
            }
        }
    }
}