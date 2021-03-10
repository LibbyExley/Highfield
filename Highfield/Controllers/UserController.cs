using Highfield.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Highfield.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AllUsers()
        {
            var users = await GetApiData();

            return View(users.OrderBy(x => x.LastName));

        }

        public async Task<IActionResult> Colours()
        {
            var colours = new List<string>();
            var users = await GetApiData();
            
            foreach (var user in users)
            {
                colours.Add(user.FavouriteColour);
            }

            var colourCounts = colours.GroupBy(x => x)
                .Select(x => new TopColour { Colour = x.Key, Count = x.Count() });

            var topColours = colourCounts.OrderByDescending(x => x.Count)
                .ThenBy(x => x.Colour);

            return View(topColours);
        }

        public async Task<IActionResult> AgePlusTwenty()
        {
            var users = await GetApiData();
            var ages = new List<Age>();

            foreach (var user in users)
            {
                var currentAge = CalculateAge(user.DOB);
                ages.Add(new Age
                {
                    UserId = user.Id,
                    CurrentAge = currentAge,
                    AgePlusTwenty = currentAge + 20
                });
            }
            return View(ages);
        }

        private int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;

            if (HasNotHadBirthday(birthDate))
            {
                age--;
            }

            return age;

        }

        private static bool HasNotHadBirthday(DateTime birthDate)
        {
            return ((birthDate.Month > DateTime.Now.Month) || (birthDate.Month == DateTime.Now.Month && birthDate.Day > DateTime.Now.Day));
        }

        private async Task<IEnumerable<User>> GetApiData()
        {
            var users = new List<User>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://recruitment.highfieldqualifications.com/api/");

                var response = await client.GetAsync("test");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    users = JsonConvert.DeserializeObject<List<User>>(result);
                }

            }
            return users;
        }
    }
}
