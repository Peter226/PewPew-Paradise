using PewPew_Paradise.GameLogic;
using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PewPew_Paradise
{
    public class MapLoad
    {
        Vector2 map_position = new Vector2(8, 8);
        Vector2 map_size = new Vector2(16, 16);
        public List<MapSprite> maps = new List<MapSprite>();
        public int floor = 1;
        public int level_number;
        /// <summary>
        /// Loading all the maps and saving them into a list
        /// </summary>
        public MapLoad ()
        {

            SpriteManager.LoadImage("Images/Sprites/forest_map.png", "forest_map");
            SpriteManager.LoadImage("Images/Sprites/sky2.png", "sky_map");
            SpriteManager.LoadImage("Images/Sprites/underground_map.png", "underground_map");
            MapSprite sky_map = new MapSprite("sky_map", new SolidColorBrush(Color.FromRgb(106,164,252)), map_position, map_size, false);
            maps.Add(sky_map);
            MapSprite forest_map = new MapSprite("forest_map", new SolidColorBrush(Color.FromRgb(0, 170, 235)), map_position, map_size, false);
            maps.Add(forest_map);
            MapSprite underground_map = new MapSprite("underground_map",new SolidColorBrush(Color.FromRgb(155, 121, 93)), map_position, map_size, false);
            maps.Add(underground_map);
        }
        /// <summary>
        /// Loading first map with characters
        /// player_number is for single/multiplayer -> same everywhere here
        /// changing floor counter to 1
        /// </summary>
        /// <param name="player_number"></param>
        public void LoadMap(int player_number)
        {
            floor =1;
            level_number = 0;
            maps[level_number].IsActive = true;
            MainWindow.Instance.lb_floor_counter.Content = Floornumbers();
            MainWindow.Instance.PlayingField.Background = maps[level_number].map_color;
            if (player_number == 1)
            {
                MainWindow.Instance.chars.CharacterLoad(1);
            }
            else 
            {
                MainWindow.Instance.chars.CharacterLoad(1);
                MainWindow.Instance.chars.CharacterLoad(2);
            }
        }
        /// <summary>
        /// Loading next map and replace players to start position
        /// </summary>
        /// <param name="player_number"></param>
        public void NextMap(int player_number)
        {
            UnLoadMap();
            MainWindow.Instance.chars.UnLoadCharacter(1);
            MainWindow.Instance.chars.UnLoadCharacter(2);
            level_number++;
            level_number = level_number % maps.Count;
            floor++;
            maps[level_number].IsActive = true;
            MainWindow.Instance.lb_floor_counter.Content = Floornumbers();
            MainWindow.Instance.PlayingField.Background = maps[level_number].map_color;
            MainWindow.Instance.chars.CharacterLoad(1);
            if (player_number != 1)
            {
                MainWindow.Instance.chars.CharacterLoad(2);
            }
        }
        /// <summary>
        /// Unload map
        /// </summary>
        public void UnLoadMap()
        {
            maps[level_number].IsActive = false;
        }
        /// <summary>
        /// Return the number of finished floors
        /// </summary>
        /// <returns></returns>
        public int Floornumbers() { return floor; }
        public MapSprite CurrentMap()
        {
            return maps[level_number];
        }
    }
}
