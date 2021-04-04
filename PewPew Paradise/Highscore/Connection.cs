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
        /// <summary>
        /// Return connection
        /// name is the name of the connection to HighscoresDB which was given in Appconfig
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Connect(string name) 
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
