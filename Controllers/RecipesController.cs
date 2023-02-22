using recipes.Models;
using Microsoft.AspNetCore.Mvc;
using recipes.Data;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace recipes.Controllers
{
    public class RecipesController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        public readonly Data.ApplicationDbContext _db;

        public RecipesController(ApplicationDbContext db)
        {
            _db = db;
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
        }
        Ingredient rp = new Ingredient();
        List<Recipe> list = new List<Recipe>();


        public IActionResult Index()
        {
            var list = _db.Recipes.ToList();
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Recipe b)
        {
            if (b.Name.Equals(b.Category))
            {
                ModelState.AddModelError("Name", "The name and category of the Book cannot be same");
            }
            if (ModelState.IsValid)
            {
                _db.Add(b);
                _db.ChangeTracker.DetectChanges();
                System.Console.WriteLine(_db.ChangeTracker.DebugView.LongView);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(b);
        }
        public IActionResult PizzaRecipe()
        {
            return View();
        }
        public IActionResult PizzaIngredients()
        {
            return View();
        }
        public IActionResult BurgerRecipe()
        {
            return View();
        }
        public IActionResult BurgerIngredients()
        {
            return View();
        }
        public IActionResult RecipePage()
        {
            var list = _db.Recipes.ToList();
            return View(list);
        }
        HttpClient client = new HttpClient();
       // [HttpGet("{id}")]
        public ActionResult IngredientPage()
        {
            List<Ingredient> list = new List<Ingredient>();
            client.BaseAddress = new Uri("http://localhost:5296/api/Ingredient");
            var response = client.GetAsync("Ingredient");
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<Ingredient>>();
                display.Wait();
                list = display.Result;
            }

            return View(list);
        }
        // [HttpGet]
        // public async Task<ActionResult<Ingredient>> IngredientPage(int? id)
        // {
        //     rp = new Ingredient();
        //     using (var httpClient = new HttpClient(_clientHandler))
        //     {
        //         using (var response = await httpClient.GetAsync("http://localhost:5296/api/Ingredient/" + id))
        //         {
        //             string apiResponse = await response.Content.ReadAsStringAsync();
        //             rp = JsonConvert.DeserializeObject<Ingredient>(apiResponse);
        //         }
        //     }
        //     return View();
        //     // if (id == null || id == 0)
        //     // {
        //     //     return NotFound();
        //     // }
        //     // var selectedBook = _db.Ingredients.FirstOrDefault(x => x.RecipeId == id);
        //     // if (selectedBook == null)
        //     // {
        //     //     return NotFound();
        //     // }

        //     // return View(selectedBook);
        //     // // var list = _db.Recipes.ToList();
        //     // // return View(list);
        // }

        //Get
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var selectedBook = _db.Recipes.Find(id);
            if (selectedBook == null)
            {
                return NotFound();
            }

            return View(selectedBook);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Recipe b)
        {
            if (b.Name.Equals(b.Category))
            {
                ModelState.AddModelError("CustomError", "The name and category of the Book cannot be same");
            }
            if (ModelState.IsValid)
            {
                _db.Update(b);
                _db.SaveChanges();
                // var obj = list.FirstOrDefault(x => x.Id == b.Id);
                // if (obj != null) 
                //     obj.Name = b.Name;
                //foreach(var i in list)
                //    System.Console.WriteLine(i.Name);
                TempData["success"] = "Recipe Updated SuccessFully!";
                return RedirectToAction("Index");
            }
            return View(b);
        }
        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var selectedBook = _db.Recipes.Find(id);
            if (selectedBook == null)
            {
                return NotFound();
            }

            return View(selectedBook);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Recipe b)
        {

            if (ModelState.IsValid)
            {
                _db.Remove(b);
                _db.SaveChanges();
                // var obj = list.FirstOrDefault(x => x.Id == b.Id);
                // if (obj != null) 
                //     obj.Name = b.Name;
                //foreach(var i in list)
                //    System.Console.WriteLine(i.Name);
                TempData["success"] = "Book Deleted SuccessFully!";
                return RedirectToAction("Index");
            }
            return View(b);
        }



    }
}