using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
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
    public IActionResult AdminLogin()
    {
        return View();
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
                            return View(); // Stay on the same page and show success message
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
    public async Task<IActionResult> UsertRegister(string FullName, string Email, string Password, string Mobile)
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

    public IActionResult ClientRegister()
    {
        return View();
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