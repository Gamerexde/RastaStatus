using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RastaStatus.Datasource;
using RastaStatus.Models.Queries;

namespace RastaStatus.Controllers
{
    [ApiController]
    [Route("/api/status")]
    public class StatusController : ControllerBase
    {
        [HttpGet("querieslist")]
        public async Task<ActionResult<IList<QueriesModel>>> GetQueriesList(bool filter, string serviceId)
        {
            if (filter)
            {
                if (String.IsNullOrEmpty(serviceId))
                {
                    return BadRequest();
                } 
            }
            
            
            IList<QueriesModel> servicesModels = await MySQLQueries.GetQueries(filter, serviceId);
            return Ok(servicesModels);
        }
        
        [HttpGet("lateststatus")]
        public async Task<ActionResult<QueriesModel>> GetLatestStatus(string serviceId)
        {
            if (String.IsNullOrEmpty(serviceId))
            {
                return BadRequest();
            } 
            
            
            QueriesModel servicesModels = await MySQLQueries.GetLatestStatus(serviceId);
            
            return Ok(servicesModels);
        }
        
        [HttpGet("uptime")]
        public async Task<ActionResult<float>> GetUptimePercent(string serviceId)
        {
            if (String.IsNullOrEmpty(serviceId))
            {
                return BadRequest();
            }
            
            
            IList<QueriesModel> servicesModels = await MySQLQueries.GetQueries(true, serviceId);

            IList<QueriesModel> successQueries = servicesModels
                .Where(query => query.state)
                .Select(query => query)
                .ToList();
            
            IList<QueriesModel> failedQueries = servicesModels
                .Where(query => query.state == false)
                .Select(query => query)
                .ToList();

            float ratio = (float)(successQueries.Count - failedQueries.Count) / successQueries.Count;

            float percent = ratio * 100;


            return Ok(percent);
        }
    }
    
    
}