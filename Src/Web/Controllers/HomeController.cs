using AutoMapper;
using Kasanova.Common.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
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

            await model.Load(false);
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
            await model.Load(true);

            watch.Stop();
            _logger.LogInformation("{0} method executed in {1} seconds", "Home.Index(POST)", watch.Elapsed.TotalSeconds);
            return View(model);
        }

    }
}