using Microsoft.AspNetCore.Mvc;
using MyApi5.UI.Resources;
using System.Text.Json;

namespace MyApi5.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly HttpClient _client;
        public AccountController(HttpClient client)
        {
            _client = client;
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var content = new StringContent(JsonSerializer.Serialize(loginRequest,options), System.Text.Encoding.UTF8, "application/json");
            using (var response = await _client.PostAsync("https://localhost:7007/api/Users/Login", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    LoginResponse loginResponse = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync(), options);
                    Response.Cookies.Append("token", "Bearer " + loginResponse.Token);
                    return RedirectToAction("Index", "Category");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", "Username or Password is incorrect.");
                    return View();
                }
                else
                {
                    TempData["Error"] = "Something went wrong";
                }
            }
            return View();
        }

    }
}
