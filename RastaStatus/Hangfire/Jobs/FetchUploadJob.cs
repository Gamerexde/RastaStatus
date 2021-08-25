using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RastaStatus.Datasource;
using RastaStatus.Models.Queries;
using RastaStatus.Models.Services;
using RastaStatus.Utils;

namespace RastaStatus.Hangfire.Jobs
{
    public class FetchUploadJob
    {
        public static async Task Start()
        {
            IList<ServicesModel> servicesModels = await MySQLQueries.GetServicesModel();

            foreach (ServicesModel output in servicesModels)
            {
                long latency;
                bool success = false;
                
                
                HttpClient client = new HttpClient();
                
                Stopwatch stopWatch = Stopwatch.StartNew();
                HttpResponseMessage response = await client.GetAsync("https://" + output.domain);

                latency = stopWatch.ElapsedMilliseconds;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        success = true;
                        break;
                    default:
                        success = false;
                        break;
                }
                
                MySQLQueries.InsertServiceQuery(new QueriesModel()
                {
                    id = RandomUtils.GenRandomString(12),
                    serviceId = output.id,
                    state = success,
                    date = DateUtils.GenDatetime(),
                    latency = (int) latency
                });
            }

        }
    }
}