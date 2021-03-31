using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PewPew_Paradise.Highscore
{
    public class Hscore
    {
        public int id { get; set; }
        public string uname { get; set; }
        public int score { get; set; }
        public int floor { get; set; }
        public string FullInfo
        {
            get 
            {
                return $"{ uname }\t{ score }\t{ floor }";
            }
        }
    }
}
