using System;
using MySqlConnector;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoAddress.Models
{
    public class AppDb : IDisposable
    {
        public MySqlConnection Connection { get; }

        public AppDb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }
        public void Dispose() => Connection.Dispose();
    }
}