using JsonExtensions;
using Kasanova.Common.ApplicationCore.Interfaces;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        private IAppLogger<HomeController> _logger;
        private IConfiguration _configuration;
        private IMemoryCache _cache;

        public HomeController(IAppLogger<HomeController> logger, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }


        // GET: Home
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = _configuration.GetValue<string>("WebApiBaseUrl");
                ViewBag.IsCardView = true;

                Stopwatch watch = new Stopwatch();
                watch.Start();

                var uri = new UriBuilder(apiUrl);
                client.BaseAddress = uri.Uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HomeViewModel model = new HomeViewModel
                {
                    Client = client
                };
                await model.Load();

                watch.Stop();
                _logger.LogInformation("{0} method executed in {1} seconds", "Home.Index", watch.Elapsed.TotalSeconds);
                return View(model);
            }

        }

        // POST: Home
        [HttpPost]
        public async Task<IActionResult> Post(HomeViewModel model)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        #region Private methods

        private async Task<IEnumerable<Category>> GetCategories()
        {
            string apiUrl = _configuration.GetValue<string>("WebApiBaseUrl");
            using (HttpClient client = new HttpClient())
            {
                var uri = new UriBuilder(apiUrl);
                client.BaseAddress = uri.Uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Categories");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new PrivateSetterContractResolver()
                };
                var results = JsonConvert.DeserializeObject<IEnumerable<Category>>(data, settings);
                return results;
            }
        }

        private async Task<IEnumerable<KeyItemValue>> GetItemValues(string keyId)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var apiUrl = _configuration.GetValue<string>("WebApiBaseUrl");
            var uri = new UriBuilder(apiUrl);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri.Uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("KeyItems/" + keyId);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new PrivateSetterContractResolver()
                };
                var results = JsonConvert.DeserializeObject<IEnumerable<KeyItemValue>>(data, settings);
                watch.Stop();
                _logger.LogInformation("{0} method executed in {1} seconds", MethodBase.GetCurrentMethod().Name, watch.Elapsed.TotalSeconds);
                return results;
            }

            #endregion


        }
    }
}