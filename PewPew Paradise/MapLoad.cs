using PewPew_Paradise.GameLogic;
using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PewPew_Paradise
{
    class MapLoad
    {
        Vector2 map_position = new Vector2(8, 8);
        Vector2 map_size = new Vector2(16, 16);

        public int floor = 0;
        public MapLoad (string mapname)
        {
            Sprite forest_map = new Sprite(mapname, map_position, map_size, true);
            floor++;
        }
        public int Floornumbers() { return floor; }
    }
}
