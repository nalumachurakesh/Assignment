using assignment.Helper;
using assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<string> GetCountries()
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.first.org/data/v1/countries");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public IActionResult Countries()
        {
            string countires = GetCountries().Result;
            object des = JsonConvert.DeserializeObject<object>(countires);

            ViewBag.Countries = des;
            return View();
        }

        public IActionResult Text()
        {
            string text = System.IO.File.ReadAllText(@"test.txt");
            ViewBag.Countries = text.Split(';');
            return View();
        }

        private List<SelectListItem> GetCities()
        {
            return new()
            {
              new SelectListItem  { Value = "Huntsville", Text = "Huntsville" },
              new SelectListItem  { Value = "Montgomery", Text = "Montgomery" },
              new SelectListItem  { Value = "Birmingham", Text = "Birmingham" },
              new SelectListItem  { Value = "Adak", Text = "Adak" },
              new SelectListItem  { Value = "Akhiok", Text = "Akhiok" },
              new SelectListItem  { Value = "Akiak", Text = "Akiak" },
              new SelectListItem  { Value = "Phoenix", Text = "Phoenix" },
              new SelectListItem  { Value = "Tucson", Text = "Tucson" },
              new SelectListItem  { Value = "Mesa", Text = "Mesa" },
              new SelectListItem  { Value = "Little Rock", Text = "Little Rock" },
              new SelectListItem  { Value = "Fayetteville", Text = "Fayetteville" },
              new SelectListItem  { Value = "Fort Smith", Text = "Fort Smith" },
            };
        }

        private List<SelectListItem> GetStates()
        {
            return new()
            {
                new SelectListItem  { Value = "Alabama", Text = "Alabama" },
                new SelectListItem  { Value =  "Alaska", Text = "Alaska" },
                new SelectListItem  { Value = "Arizona", Text = "Arizona" },
                new SelectListItem  { Value = "Arkansas", Text = "Arkansas" }
            };
        }

        public IActionResult Profile(string id)
        {
            ViewBag.ProfileModel = HttpContext.Session.GetObjectFromJson<List<ProfileModel>>("ProfileModel");

            List<ProfileModel> profileModels;
            if (ViewBag.ProfileModel == null)
            {
                profileModels = new List<ProfileModel>();

                HttpContext.Session.SetObjectAsJson("ProfileModel", profileModels);
                ViewBag.ProfileModel = HttpContext.Session.GetObjectFromJson<List<ProfileModel>>("ProfileModel");
            }

            ProfileModel profileModel = new()
            {
                States = GetStates(),
                Cities = GetCities()
            };

            if (!string.IsNullOrEmpty(id))
            {
                profileModels = (List<ProfileModel>)ViewBag.ProfileModel;

                ProfileModel model = profileModels.Single(p => p.Guid == id);
                profileModel.FirstName = model.FirstName;
                profileModel.LastName = model.LastName;
                profileModel.City = model.City;
                profileModel.State = model.State;
                profileModel.Gender = model.Gender;
                profileModel.Guid = model.Guid;
            }

            return View(profileModel);
        }

        [HttpPost]
        public IActionResult Profile(ProfileModel model)
        {
            List<ProfileModel> profiles = HttpContext.Session.GetObjectFromJson<List<ProfileModel>>("ProfileModel");

            if (string.IsNullOrEmpty(model.Guid))
            {
                model.Guid = Guid.NewGuid().ToString();
                profiles.Add(model);
            }
            else
            {
                int index = profiles.IndexOf(profiles.Single(p => p.Guid == model.Guid));
                profiles[index] = model;
            }

            HttpContext.Session.SetObjectAsJson("ProfileModel", profiles);

            ViewBag.ProfileModel = profiles;

            ProfileModel profileModel = new()
            {
                States = GetStates(),
                Cities = GetCities()
            };

            ModelState.Clear();

            return Redirect("/Home/Profile");
        }

        public IActionResult ProfileDelete(string id)
        {
            List<ProfileModel> profiles = HttpContext.Session.GetObjectFromJson<List<ProfileModel>>("ProfileModel");

            ProfileModel profileModel = profiles.Single(p => p.Guid == id);
            profiles.Remove(profileModel);

            HttpContext.Session.SetObjectAsJson("ProfileModel", profiles);

            ViewBag.ProfileModel = profiles;

            return RedirectToAction("Profile");
        }
    }
}