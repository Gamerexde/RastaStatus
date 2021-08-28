using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using RastaStatus.Models.Queries;
using RastaStatus.Models.Services;
using RastaStatus.Utils;

namespace RastaStatus.Datasource
{
    public interface MySQLQueries
    {
        public static async Task InsertServiceQuery(QueriesModel model)
        {
            try
            {
                MySQLConnection connection = new MySQLConnection(Startup.GetMySQLString());
                MySqlCommand cmd = connection.GetConnection().CreateCommand();

                cmd.CommandText = @"INSERT INTO `queries` (`id`, `serviceId`, `date`, `state`, `latency`) VALUES (@id, @serviceId, @date, @state, @latency)";

                cmd.Parameters.AddWithValue("@id", model.id);
                cmd.Parameters.AddWithValue("@serviceId", model.serviceId);
                cmd.Parameters.AddWithValue("@date", model.date);
                cmd.Parameters.AddWithValue("@state", model.state);
                cmd.Parameters.AddWithValue("@latency", model.latency);


                await cmd.ExecuteNonQueryAsync();
                
                connection.Dispose();
                
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        

        public static async Task<IList<ServicesModel>> GetServicesModel()
        {
            IList<ServicesModel> servicesModels = new List<ServicesModel>();

            try
            {
                MySQLConnection connection = new MySQLConnection(Startup.GetMySQLString());
                
                MySqlCommand cmd = connection.GetConnection().CreateCommand();

                cmd.CommandText = @"SELECT * FROM `services`";

                DbDataReader rdr = await cmd.ExecuteReaderAsync();


                while (await rdr.ReadAsync())
                {
                    string type = (string) rdr[3];
                    int serviceType = 0;
                    
                    serviceType = Int32.Parse(type);

                    servicesModels.Add(new ServicesModel()
                    {
                        id = rdr[0] as string,
                        ip = rdr[1] as string,
                        domain = rdr[2] as string,
                        type = (ServiceType) serviceType
                    });
                }

                connection.Dispose();
                return servicesModels;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        } 
        
        public static async Task<IList<QueriesModel>> GetQueries(bool filter, string serviceID)
        {
            IList<QueriesModel> queriesModels = new List<QueriesModel>();

            try
            {
                MySQLConnection connection = new MySQLConnection(Startup.GetMySQLString());
                
                MySqlCommand cmd = connection.GetConnection().CreateCommand();

                cmd.CommandText = @"SELECT * FROM `queries`";

                DbDataReader rdr = await cmd.ExecuteReaderAsync();


                while (await rdr.ReadAsync())
                {
                    if (filter)
                    {
                        if (serviceID != (string) rdr[1])
                        {
                            continue;
                        }
                    }
                    
                    DateTime time = rdr[2] is DateTime ? (DateTime) rdr[2] : default;
                    
                    queriesModels.Add(new QueriesModel()
                    {
                        id = rdr[0] as string,
                        serviceId = rdr[1] as string,
                        date = time.ToString(DateUtils.MysqlDateFormat),
                        state = (bool)rdr[3],
                        latency = (int)rdr[4]
                    });
                }
                
                connection.Dispose();
                return queriesModels;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }
        
        public static async Task<QueriesModel> GetLatestStatus(string serviceID)
        {
            if (String.IsNullOrEmpty(serviceID))
            {
                return null;
            }
            
            try
            {
                MySQLConnection connection = new MySQLConnection(Startup.GetMySQLString());
                
                MySqlCommand cmd = connection.GetConnection().CreateCommand();

                cmd.CommandText = @"SELECT * FROM `queries` WHERE `serviceId` LIKE @serviceID ORDER BY `queries`.`date` DESC";

                cmd.Parameters.AddWithValue("@serviceID", serviceID);

                DbDataReader rdr = await cmd.ExecuteReaderAsync();

                await rdr.ReadAsync();

                DateTime time = rdr[2] is DateTime ? (DateTime) rdr[2] : default;
                
                
                QueriesModel queriesModel = new QueriesModel()
                {
                    id = rdr[0] as string,
                    serviceId = rdr[1] as string,
                    date = time.ToString(DateUtils.MysqlDateFormat),
                    state = (bool) rdr[3],
                    latency = (int) rdr[4]
                };

                connection.Dispose();
                return queriesModel;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }
    }
}