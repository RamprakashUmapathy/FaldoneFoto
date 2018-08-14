using AutoMapper;
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
using System.Net.Http;
using System.Threading.Tasks;
using Web.Extensions;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        private IAppLogger<HomeController> _logger;
        private IConfiguration _configuration;
        private IMemoryCache _cache;
        private HttpClient Client { get; }
        private IMapper Mapper { get; }
        public HomeController(IAppLogger<HomeController> logger, IConfiguration configuration, IMemoryCache memoryCache, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Client = new HttpClient();
            string apiUrl = _configuration.GetValue<string>("WebApiBaseUrl");
            var uri = new UriBuilder(apiUrl);
            Client.BaseAddress = uri.Uri;
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET: Home
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Client.RegisterForDispose(this.ControllerContext.HttpContext);
            ViewBag.IsCardView = true;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            HomeViewModel model = new HomeViewModel
            {
                Client = Client,
                PhotoBaseUrl = _configuration.GetValue<string>("PhotoBaseUrl"),
                Mapper = Mapper
            };

            await model.LoadAsync(false);
            watch.Stop();

            _logger.LogInformation("{0} method executed in {1} seconds", "Home.Index(GET)", watch.Elapsed.TotalSeconds);
            return View(model);
        }

        // POST: Home
        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel model)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Client.RegisterForDispose(this.ControllerContext.HttpContext);

            model.Client = Client;
            model.PhotoBaseUrl = _configuration.GetValue<string>("PhotoBaseUrl");
            model.Mapper = Mapper;
            await model.LoadAsync(true);

            //Required for the partial call back DevExpress
            string uri = $"articles/{model.ShopSignId}";
            if (!String.IsNullOrEmpty(model.CategoryId))
                uri = uri + $"/{model.CategoryId}";
            if (!String.IsNullOrEmpty(model.FamilyId))
                uri = uri + $"/{model.FamilyId}";
            if (!String.IsNullOrEmpty(model.SeriesId))
                uri = uri + $"/{model.SeriesId}";
            if (!String.IsNullOrEmpty(model.Level1Id))
                uri = uri + $"/{model.Level1Id}";
            if (!String.IsNullOrEmpty(model.Level2Id))
                uri = uri + $"/{model.Level2Id}";
            if (!String.IsNullOrEmpty(model.StyleId))
                uri = uri + $"/{model.StyleId}";

            var t = _cache.Set<string>("uri", uri);

            watch.Stop();
            _logger.LogInformation("{0} method executed in {1} seconds", "Home.Index(POST)", watch.Elapsed.TotalSeconds);
            return View(model);
        }

        //Required for DevExpress Partial Databinding
        public ActionResult PartialBinding()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var results = new List<ArticleCardViewModel>();
            string uri = _cache.Get<string>("uri");
            using (HttpResponseMessage response =  Client.GetAsync(uri).Result)
            {
                response.EnsureSuccessStatusCode();
                var data = response.Content.ReadAsStringAsync().Result;
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new PrivateSetterContractResolver()
                };
                var articles = JsonConvert.DeserializeObject<List<ChalcoArticle>>(data, settings);
                articles.ForEach(a =>
                {
                    var vm = Mapper.Map<ArticleCardViewModel>(a);
                    results.Add(vm);
                });
            }
            watch.Stop();
            _logger.LogInformation("{0} method executed in {1} seconds", "Home.Index(POST)", watch.Elapsed.TotalSeconds);
            return PartialView("~/Views/Home/_ArticleCardLayout.cshtml", results);
        }

    }
}