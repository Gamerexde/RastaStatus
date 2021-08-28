using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RastaStatus.Datasource;
using RastaStatus.Models.Queries;
using RastaStatus.Models.Services;
using RastaStatus.Utils;

namespace RastaStatus.Controllers
{
    [ApiController]
    [Route("/test")]
    public class TestController : ControllerBase
    {
        [HttpGet("rand")]
        public string GetRand(int lenght)
        {
            return RandomUtils.GenRandomString(lenght);
        }

        [HttpGet("date")]
        public string GetDate()
        {
            
            return DateUtils.GenDatetime();
        }
    }
}