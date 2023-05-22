using Microsoft.AspNetCore.Mvc;
using ConsumeAPI.Models;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ConsumeAPI.Controllers
{
    public class LoginTodoController : Controller
    {
        string baseURL = "http://localhost:5000/";

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Read", "Item");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginItemTodo loginModel)
        {
            if (loginModel.Username != null && loginModel.Password != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.GetAsync($"Login?username={loginModel.Username}&password={loginModel.Password}");
                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Read", "Item");
                    }
                    else
                    {
                        Console.WriteLine("Error calling API.");
                    }
                }
            }
            ViewData["ValidateMessage"] = "User Not Found";
            return View();
        }
    }
}
