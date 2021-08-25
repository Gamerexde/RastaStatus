using System;
using MySql.Data.MySqlClient;

namespace RastaStatus.Datasource
{
    public class MySQLConnection : IDisposable
    {
        private MySqlConnection _connection; 
        
        public MySQLConnection(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public MySqlConnection GetConnection()
        {
            _connection.Open();
            return _connection;
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}