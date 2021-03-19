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
        Vector2 map_position = new Vector2(8, 24);
        Vector2 map_size = new Vector2(16, 16);
        public List<MapSprite> maps = new List<MapSprite>();
        public int floor = 1;
        public int level_number;
        public List<EnemySprite> enemys = new List<EnemySprite>();
        public List<EnemySprite> current = new List<EnemySprite>();
        public Vector2 enemy_pos =  new Vector2(6,7);
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
            //Enemys
            SpriteManager.LoadImage("Images/Sprites/Enemies/bee.png", "bee");
            SpriteManager.LoadImage("Images/Sprites/Enemies/fishbone.png", "fishbone");
            SpriteManager.LoadImage("Images/Sprites/Enemies/mushroom.png", "mushroom");
            SpriteManager.LoadImage("Images/Sprites/Enemies/witch.png", "witch");
            SpriteManager.LoadImage("Images/Sprites/Enemies/zombie.png", "zombie");
            EnemySprite bee1 = new EnemySprite("bee",  enemy_pos, new Vector2(1,1), false);
            enemys.Add(bee1);
            //EnemySprite bee2 = new EnemySprite("bee", "sky_map", enemy_pos[1], new Vector2(1, 1), false);
            //enemys.Add(bee2);
            EnemySprite mushroom1 = new EnemySprite("mushroom", enemy_pos, new Vector2(1, 1), false);
            enemys.Add(mushroom1);
            //EnemySprite mushroom2 = new EnemySprite("mushroom", "forest_map", enemy_pos[1], new Vector2(1, 1), false);
            //enemys.Add(mushroom2);
            EnemySprite witch1 = new EnemySprite("witch", enemy_pos, new Vector2(1, 1), false);
            enemys.Add(witch1);
            //EnemySprite witch2 = new EnemySprite("witch", "underground", enemy_pos[1], new Vector2(1, 1), false);
            //enemys.Add(witch2);

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
            maps[level_number].MapLoaded();
            EnemySprite first_enemy = enemys[level_number];
            MainWindow.Instance.enemy.EnemyLoad(enemys[level_number]);
            current.Add(first_enemy);


        }
        /// <summary>
        /// Loading next map and replace players to start position
        /// </summary>
        /// <param name="player_number"></param>
        public void NextMap(int player_number)
        {

            UnLoadMap();
            for (int i = 0; i < current.Count; i++)
            {
                current[i].Destroy();
            }
            current.Clear();
            MainWindow.Instance.chars.UnLoadCharacter(1);
            MainWindow.Instance.chars.UnLoadCharacter(2);
            level_number++;
            level_number = level_number % maps.Count;
            floor++;
            maps[level_number].IsActive = true;
            MainWindow.Instance.lb_floor_counter.Content = Floornumbers();
            MainWindow.Instance.PlayingField.Background = maps[level_number].map_color;
            MainWindow.Instance.chars.CharacterLoad(1);
            for (int i = 0; i < Math.Ceiling((double)floor/3); i++)
            {
                EnemySprite new_enemy = MainWindow.Instance.enemy.AddEnemy(enemys[level_number].image, enemy_pos);
                current.Add(new_enemy);
                MainWindow.Instance.enemy.EnemyLoad(new_enemy);
                if (enemy_pos.x < 15)
                    enemy_pos.x += 1;
                else
                    enemy_pos.x = 1;
            }
            if (player_number != 1)
            {
                MainWindow.Instance.chars.CharacterLoad(2);
            }
            maps[level_number].MapLoaded();

            
        }
        /// <summary>
        /// Unload map
        /// </summary>
        public void UnLoadMap()
        {
            //maps[level_number].IsActive = false;
            maps[level_number].MapUnloaded();

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
