using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MineStatLib;
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
                ResultModel result = null;
                
                Console.WriteLine(output.domain + " " + output.type);
                
                switch (output.type)
                {
                    case ServiceType.HTTP:
                        result = await FetchHttp(output);
                        break;
                    case ServiceType.MINECRAFT:
                        result = FetchMinecraft(output);
                        break;
                }

                if (result == null)
                {
                    result = new ResultModel()
                    {
                        latency = 0,
                        success = false
                    };
                }
                

                await MySQLQueries.InsertServiceQuery(new QueriesModel()
                {
                    id = RandomUtils.GenRandomString(12),
                    serviceId = output.id,
                    state = result.success,
                    date = DateUtils.GenDatetime(),
                    latency = result.latency
                });
            }
        }

        private static ResultModel FetchMinecraft(ServicesModel serviceInfo)
        {
            bool success = false;
            
            MineStat ms = new MineStat(serviceInfo.ip, 25565);

            if (ms.ServerUp)
            {
                success = true;
            }

            return new ResultModel()
            {
                latency = (int) ms.Latency,
                success = success
            };
        }

        private static async Task<ResultModel> FetchHttp(ServicesModel serviceInfo)
        {
            long latency;
            bool success = false;
            
            HttpClient client = new HttpClient();
                
            Stopwatch stopWatch = Stopwatch.StartNew();

            try
            {
                // TODO: Add classic Http fetching (idk why but fine)
                HttpResponseMessage response = await client.GetAsync("https://" + serviceInfo.domain);
                
                latency = stopWatch.ElapsedMilliseconds;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        success = true;
                        break;
                }

                return new ResultModel()
                {
                    latency = (int) latency,
                    success = success
                };
            }
            catch (InvalidOperationException e)
            {
                return null;
                
            } catch (HttpRequestException e)
            {
                return null;
            }
        }
    }

    class ResultModel
    {
        public int latency { get; set; }
        public bool success { get; set; }
    }
}