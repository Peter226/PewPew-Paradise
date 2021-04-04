using System;
using System.Collections.Generic;
using System.Collections;
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
        /// <summary>
        /// Getting all the data from HIGHSCORE table in a Hscore list
        /// </summary>
        /// <returns></returns>
        public List<Hscore> GetScore() 
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                var output  = connection.Query<Hscore>("SELECT * from HIGHSCORE ").ToList();
                return output;
            }
        }
        /// <summary>
        /// Returns the number of rows in HIGHSCORE table
        /// </summary>
        /// <returns></returns>
        public int ScoreAmount() 
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                int output = connection.Query<Hscore>("SELECT * from HIGHSCORE").Count();
                return output; 
            }
        }
        /// <summary>
        /// Returns a Hscore list ordered by score
        /// This is used for showing top scores
        /// </summary>
        /// <returns></returns>
        public List<Hscore> GetOrderedScore()
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                var output = connection.Query<Hscore>("SELECT * from HIGHSCORE ORDER BY score DESC ").ToList();
                
                return output;
            }
        }
        /// <summary>
        /// Returns a Hscore list ordered by floorcount
        /// This is used for showing top floors
        /// </summary>
        /// <returns></returns>
        public List<Hscore> GetOrderedFloor()
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                var output = connection.Query<Hscore>("SELECT * from HIGHSCORE ORDER BY floorcount DESC ").ToList();

                return output;
            }
        }
        /// <summary>
        /// Returns a characterid from HIGHSCORE ordered by the count of characterids
        /// This only returns the 1st element of the list
        /// If the database is empty this returns 0
        /// </summary>
        /// <returns></returns>
        public int GetMostPlayedChar()
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {

                var output = connection.Query<int>("Select characterid from Highscore Group By characterid Order By count(*) DESC").ToList();
                if (output.Count != 0)
                {
                    return output[0];
                }
                else
                    return 0;

            }
        }
        /// <summary>
        /// Insert a full row into HIGHSCORE Table
        /// </summary>
        /// <param name="score"></param>
        public void AddScore(Hscore score)
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                connection.Execute("INSERT INTO HIGHSCORE (uname, score, floorcount, characterid) VALUES (@uname, @score, @floorcount, @characterid)",score);
            }
        }
        /// <summary>
        /// Insert a full row into CHARACTER Table
        /// </summary>
        /// <param name="character"></param>
        public void AddChar(Character character)
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                connection.Execute("INSERT INTO CHARACTER (characterid, charactername) VALUES (@characterid, @charactername)", character);
            }
        }
        /// <summary>
        /// Returns the name of the character from the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCharName(int id)
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                var charname = connection.Query<string>($"SELECT charactername FROM CHARACTER WHERE characterid={id}").ToList();
                return charname[0];
            }
        }
        /// <summary>
        /// Deletes all the data from HIGHSCORE table
        /// </summary>
        public void ClearDB()
        {
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                connection.Execute("DELETE FROM HIGHSCORE");
            }
        }
        /// <summary>
        /// Filling CHARACTER table if its empty
        /// This is called when the program starts to run to avoid missing data issues
        /// </summary>
        public void InitDB()
        {
            List<Character> chars;
            using (IDbConnection connection = new SQLiteConnection(Connection.Connect("Highscore")))
            {
                chars = connection.Query<Character>("SELECT * from CHARACTER").ToList();
            }
            if (chars == null || chars.Count < 1)
            {
                AddChar(new Character() { characterid = 0, charactername = "Moni The Unicorn" });
                AddChar(new Character() { characterid = 1, charactername = "Oli The Orc" });
                AddChar(new Character() { characterid = 2, charactername = "Peti the Rat" });
                AddChar(new Character() { characterid = 3, charactername = "Mr. The PlaceHolder" });
                AddChar(new Character() { characterid = 4, charactername = "Hugo The Cactus" });
                AddChar(new Character() { characterid = 5, charactername = "Michael The Monkey" });
                AddChar(new Character() { characterid = 6, charactername = "Chochie The Muffin" });
                AddChar(new Character() { characterid = 7, charactername = "Philip The Penguin" });
                AddChar(new Character() { characterid = 8, charactername = "Ms.Bread The Toast" });
                AddChar(new Character() { characterid = 9, charactername = "Frigyes The Turtle" });
                AddChar(new Character() { characterid = 10, charactername = "Jello The Slime" });
                AddChar(new Character() { characterid = 11, charactername = "Fred The Dinosaur" });
                AddChar(new Character() { characterid = 12, charactername = "Cola The Koala" });
            }
        }

    }
}
