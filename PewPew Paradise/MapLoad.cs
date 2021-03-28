using PewPew_Paradise.GameLogic;
using PewPew_Paradise.GameLogic.SpriteComponents;
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
        public List<EnemySprite> current = new List<EnemySprite>();
        public Vector2 enemy_pos =  new Vector2(6,7);
        public Random rnd = new Random();
        /// <summary>
        /// Loading all the maps and saving them into a list
        /// </summary>
        public MapLoad ()
        {
           
            
            
            SpriteManager.LoadImage("Images/Sprites/forest_map.png", "forest_map");
            SpriteManager.LoadImage("Images/Sprites/sky2.png", "sky_map");
            SpriteManager.LoadImage("Images/Sprites/underground_map.png", "underground_map");
            MapSprite sky_map = new MapSprite("sky_map", "bee", new SolidColorBrush(Color.FromRgb(106,164,252)), map_position, map_size, false);
            maps.Add(sky_map);
            MapSprite forest_map = new MapSprite("forest_map", "mushroom", new SolidColorBrush(Color.FromRgb(0, 170, 235)), map_position, map_size, false);
            maps.Add(forest_map);
            MapSprite underground_map = new MapSprite("underground_map", "witch", new SolidColorBrush(Color.FromRgb(155, 121, 93)), map_position, map_size, false);
            maps.Add(underground_map);
            //Enemys
            SpriteManager.LoadImage("Images/Sprites/Enemies/bee.png", "bee");
            SpriteManager.LoadImage("Images/Sprites/Enemies/fishbone.png", "fishbone");
            SpriteManager.LoadImage("Images/Sprites/Enemies/mushroom.png", "mushroom");
            SpriteManager.LoadImage("Images/Sprites/Enemies/witch.png", "witch");
            SpriteManager.LoadImage("Images/Sprites/Enemies/zombie.png", "zombie");

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
            MainWindow.playingFieldBrush.Color = maps[level_number].map_color.Color;
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
            enemy_pos.x = rnd.Next(1, 15) + 0.5;
            EnemySprite new_enemy = MainWindow.Instance.enemy.AddEnemy(maps[level_number].enemy, enemy_pos);
            current.Add(new_enemy);
            MainWindow.Instance.enemy.EnemyLoad(new_enemy);


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
            MainWindow.Instance.chars.CharacterLoad(1);
            for (int i = 0; i < Math.Ceiling((double)floor/3); i++)
            {
                
                enemy_pos.x = rnd.Next(1, 15) + 0.5;
                enemy_pos.y = rnd.Next(1, 15) + 0.5;
                EnemySprite new_enemy = MainWindow.Instance.enemy.AddEnemy(maps[level_number].enemy, enemy_pos);
                current.Add(new_enemy);
                MainWindow.Instance.enemy.EnemyLoad(new_enemy);
                
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
        public void ClearAll() 
        {
            for (int i = 0; i < FruitSprite.fruitList.Count; i++)
            {
                FruitSprite.fruitList[0].Destroy();
            }
            FruitSprite.fruitList.Clear();
            for (int i = 0; i < Enemy.enemyList.Count; i++)
            {
                Enemy.enemyList[0].Destroy();
            }
            MainWindow.Instance.chars.SelectedChar(1).GetComponent<CollideComponent>().IsActive = false;
            MainWindow.Instance.chars.SelectedChar(2).GetComponent<CollideComponent>().IsActive = false;
            MainWindow.Instance.chars.SelectedChar(1).GetComponent<PhysicsComponent>().IsActive = false;
            MainWindow.Instance.chars.SelectedChar(2).GetComponent<PhysicsComponent>().IsActive = false;
            MainWindow.Instance.load.CurrentMap().IsActive = false;
            MainWindow.Instance.chars.UnLoadChar(1);
            MainWindow.Instance.chars.UnLoadChar(2);
        }
    }
}
