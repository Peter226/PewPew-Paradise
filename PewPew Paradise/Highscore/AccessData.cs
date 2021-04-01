using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                var output  = connection.Query<Hscore>("select * from HIGHSCORES ").ToList();
                return output;
            }
        }
        public void AddScore(Hscore score)
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                connection.Execute("insert into HIGHSCORES (uname, score, floor) VALUES (@uname, @score, @floor)",score);
            }
        }
    }
}
