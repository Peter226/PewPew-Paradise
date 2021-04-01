﻿using System;
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
        public void AddScore(Hscore score)
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                connection.Execute("INSERT INTO HIGHSCORE (uname, score, floorcount) VALUES (@uname, @score, @floorcount)",score);
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
