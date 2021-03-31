using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PewPew_Paradise.Highscore
{
    public static class Connection
    {
        public static string Connect(string name) 
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
