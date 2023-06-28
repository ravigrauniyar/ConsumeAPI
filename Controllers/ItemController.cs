using ConsumeAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConsumeAPI.Controllers
{
    public class ItemController : Controller
    {
        string baseURL = "http://localhost:5000/todos/";

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateItemTodo newItem)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(baseURL, newItem);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Read", "Item");
                }
                else
                {
                    Console.WriteLine("Error calling API.");
                }
            }
            return RedirectToAction("Read", "Item");
        }

        [HttpGet]
        public async Task<IActionResult> Read()
        {
            IList<ItemTodo> todoList = new List<ItemTodo>();

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(baseURL);
                if (response.IsSuccessStatusCode)
                {
                    string dataResult = response.Content.ReadAsStringAsync().Result;
                    todoList = JsonConvert.DeserializeObject<IList<ItemTodo>>(dataResult);
                }
                else
                {
                    Console.WriteLine("Error calling API.");
                }
                ViewData.Model = todoList;
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            ItemTodo todoItem = new ItemTodo();

            using (var client = new HttpClient())
            {
                HttpResponseMessage getData = await client.GetAsync(baseURL+$"get?id={id}");
                if (getData.IsSuccessStatusCode)
                {
                    string dataResult = getData.Content.ReadAsStringAsync().Result;
                    todoItem = JsonConvert.DeserializeObject<ItemTodo>(dataResult);
                }
                else
                {
                    Console.WriteLine("Error calling API.");
                }
                ViewData.Model = todoItem;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update([FromRoute] Guid id)
        {
            UpdateItemTodo updateItem = new();
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(baseURL+$"get?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    string dataResult = response.Content.ReadAsStringAsync().Result;
                    var item = JsonConvert.DeserializeObject<ItemTodo>(dataResult);
                    updateItem = new UpdateItemTodo()
                    {
                        Title = item.Title,
                        Description = item.Description 
                    };
                }
                else
                {
                    Console.WriteLine("Error calling API.");
                }
            }
            return View(updateItem);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateItemTodo updateItem)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(baseURL+$"put?id={updateItem.Id}", updateItem);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Read", "Item");
                }
                else
                {
                    Console.WriteLine("Error calling API.");
                }
            }
            return RedirectToAction("Read", "Item");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage getData = await client.DeleteAsync(baseURL + $"delete?id={id}");
                if (getData.IsSuccessStatusCode)
                {
                    return RedirectToAction("Read", "Item");
                }
                else
                {
                    Console.WriteLine("Error calling API.");
                }
            }
            return RedirectToAction("Read", "Item");
        }
    }
}
