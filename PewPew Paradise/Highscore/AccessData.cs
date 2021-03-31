using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace PewPew_Paradise.Highscore
{
    public class AccessData
    {
        public List<Hscore> GetScore() 
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Connection.Connect("Highscore")))
            {
                var output  = connection.Query<Hscore>("select * from dbo.Highscores ").ToList();
                return output;
            }
        }

    }
}
