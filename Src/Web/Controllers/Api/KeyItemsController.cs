using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Kasanova.Common.ApplicationCore.Interfaces;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Kasanova.FaldoneFoto.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Api
{

    [Produces("application/json")]
    [Route("api/KeyItems/{keyName}")]
    public class KeyItemsController : Controller
    {

        private IAppLogger<KeyItemsController> _logger;
        private IKeyItemValueRepository _keyItemValueRepository;



        public KeyItemsController(IAppLogger<KeyItemsController> logger, IKeyItemValueRepository keyItemValueRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _keyItemValueRepository = keyItemValueRepository ?? throw new ArgumentNullException(nameof(keyItemValueRepository));
        }


        //HTTP GET keyName
        public async Task<IEnumerable<KeyItemValue>> Get(string keyName)

        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var keyItems = new List<KeyItemValue>();
            int pageSize = int.MaxValue; // no paging needed
            var parameters = new { CultureId = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Key = keyName };
            var pageInfo = await _keyItemValueRepository.ListAsync(parameters, pageSize, 1);
            keyItems.AddRange(pageInfo.Data);
            for (int i = 2; i <= pageInfo.TotalPages; i++)
            {
                await _keyItemValueRepository.ListAsync(parameters, pageSize, i);
                keyItems.AddRange(pageInfo.Data);
            }
            watch.Stop();
            _logger.LogInformation("{0} method executed in {1} seconds", MethodBase.GetCurrentMethod().Name, watch.Elapsed.TotalSeconds);
            return keyItems;
        }
    }
}