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
                var output  = connection.Query<Hscore>("SELECT * from HIGHSCORE ").ToList();
                return output;
            }
        }
        public int ScoreAmount() 
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                int output = connection.Query<Hscore>("SELECT * from HIGHSCORE").Count();
                return output; 
            }
        }
        public List<Hscore> GetOrderedScore()
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                var output = connection.Query<Hscore>("SELECT * from HIGHSCORE ORDER BY score DESC ").ToList();
                
                return output;
            }
        }
        public List<Hscore> GetOrderedFloor()
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                var output = connection.Query<Hscore>("SELECT * from HIGHSCORE ORDER BY floorcount DESC ").ToList();

                return output;
            }
        }
        public void AddScore(Hscore score)
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                connection.Execute("INSERT INTO HIGHSCORE (uname, score, floorcount, characterid) VALUES (@uname, @score, @floorcount, @characterid)",score);
            }
        }
        public void AddChar(Character character)
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                connection.Execute("INSERT INTO CHARACTER (characterid, charactername) VALUES (@characterid, @charactername)", character);
            }
        }
        public void ClearDB()
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                connection.Execute("DELETE FROM HIGHSCORE");
            }
        }


    }
}
