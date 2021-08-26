using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using RastaStatus.Models.Queries;
using RastaStatus.Models.Services;

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

                MySqlDataReader rdr = cmd.ExecuteReader();


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

                return servicesModels;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }
    }
}