using PewPew_Paradise.GameLogic;
using PewPew_Paradise.GameLogic.SpriteComponents;
using PewPew_Paradise.Highscore;
using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public Rect dangerzone1 = new Rect();
        public Rect dangerzone2 = new Rect();
        public Rect dangerzone3 = new Rect();
        public Rect dangerzone4 = new Rect();
        
        /// <summary>
        /// Loading all the maps and saving them into a list
        /// </summary>
        public MapLoad ()
        {
           
            
            
            SpriteManager.LoadImage("Images/Sprites/forest_map.png", "forest_map");
            SpriteManager.LoadImage("Images/Sprites/sky2.png", "sky_map");
            SpriteManager.LoadImage("Images/Sprites/underground_map.png", "underground_map");
            SpriteManager.LoadImage("Images/Sprites/water_map.png", "water_map");
            SpriteManager.LoadImage("Images/Sprites/lava_map.png", "lava_map");
            SpriteManager.LoadImage("Images/Sprites/spooky_map.png", "spooky_map");
            MapSprite sky_map = new MapSprite("sky_map", "bee", new SolidColorBrush(Color.FromRgb(106,164,252)), map_position, map_size, false);
            maps.Add(sky_map);
            MapSprite forest_map = new MapSprite("forest_map", "mushroom", new SolidColorBrush(Color.FromRgb(0, 170, 235)), map_position, map_size, false);
            maps.Add(forest_map);
            MapSprite underground_map = new MapSprite("underground_map", "zombie", new SolidColorBrush(Color.FromRgb(155, 121, 93)), map_position, map_size, false);
            maps.Add(underground_map);
            MapSprite water_map = new MapSprite("water_map", "fishbone", new SolidColorBrush(Color.FromRgb(38, 196, 255)), map_position, map_size, false);
            maps.Add(water_map);
            MapSprite lava_map = new MapSprite("lava_map", "dragon", new SolidColorBrush(Color.FromRgb(168, 69, 25)), map_position, map_size, false);
            maps.Add(lava_map);
            MapSprite spooky_map = new MapSprite("spooky_map", "witch", new SolidColorBrush(Color.FromRgb(20, 30, 53)), map_position, map_size, false);
            maps.Add(spooky_map);
            //Enemys
            SpriteManager.LoadImage("Images/Sprites/Enemies/bee.png", "bee");
            SpriteManager.LoadImage("Images/Sprites/Enemies/fishbone.png", "fishbone");
            SpriteManager.LoadImage("Images/Sprites/Enemies/mushroom.png", "mushroom");
            SpriteManager.LoadImage("Images/Sprites/Enemies/witch.png", "witch");
            SpriteManager.LoadImage("Images/Sprites/Enemies/zombie.png", "zombie");
            SpriteManager.LoadImage("Images/Sprites/Enemies/dragon.png", "dragon");
            
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
                dangerzone2 = MainWindow.Instance.chars.SelectedChar(2).GetRect();
                dangerzone2 = Dangerzone(dangerzone2);
            }
            maps[level_number].MapLoaded();
            dangerzone1 = MainWindow.Instance.chars.SelectedChar(1).GetRect();
            dangerzone1 = Dangerzone(dangerzone1);
            dangerzone3 = new Rect(4.5, 15.5, 0, 1);
            dangerzone3 = PortalDanger(dangerzone3);
            dangerzone4 = new Rect(11.5, 15.5, 0, 1);
            dangerzone4 = PortalDanger(dangerzone4);
            do
            {
                enemy_pos.x = rnd.Next(1, 14) + 0.5;
                enemy_pos.y = rnd.Next(3, 14) + 0.5;
            }
            while (!(((enemy_pos.x < dangerzone1.TopLeft.X || enemy_pos.x > dangerzone1.TopRight.X) || enemy_pos.y > dangerzone1.BottomLeft.Y) &&
                       ((enemy_pos.x < dangerzone2.TopLeft.X || enemy_pos.x > dangerzone2.TopRight.X) || enemy_pos.y > dangerzone2.BottomLeft.Y) &&
                       ((enemy_pos.x < dangerzone3.TopLeft.X || enemy_pos.x > dangerzone3.TopRight.X) || enemy_pos.y < dangerzone3.TopLeft.Y) &&
                       ((enemy_pos.x < dangerzone4.TopLeft.X || enemy_pos.x > dangerzone4.TopRight.X) || enemy_pos.y < dangerzone4.TopLeft.Y)));

            EnemySprite new_enemy = MainWindow.Instance.enemy.AddEnemy(maps[level_number].enemy, enemy_pos);
            
            current.Add(new_enemy);
            new_enemy.IsActive = true;
            MainWindow.Instance.enemy.EnemyLoad(new_enemy);

        }
        /// <summary>
        /// Loading next map and replace players to start position
        /// </summary>
        /// <param name="player_number"></param>
        public void NextMap(int player_number)
        {
            UnLoadMap();
            int smthing = current.Count;
            for (int i = 0; i < smthing; i++)
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
            dangerzone1 = MainWindow.Instance.chars.SelectedChar(1).GetRect();
            dangerzone1=Dangerzone(dangerzone1);
            dangerzone3 = new Rect(4.5,15.5,0,1);
            dangerzone3 = PortalDanger(dangerzone3);
            dangerzone4 = new Rect(11.5, 15.5, 0, 1);
            dangerzone4 = PortalDanger(dangerzone4);
            if (player_number != 1)
            {
                MainWindow.Instance.chars.CharacterLoad(2);
                dangerzone2 = MainWindow.Instance.chars.SelectedChar(2).GetRect();
                dangerzone2 = Dangerzone(dangerzone2);
            }
            int enemyCount = (int)Math.Ceiling((double)floor / maps.Count);
            for (int i = 0; i < enemyCount; i++)
            {
                if(i==6)
                { break; }
                do
                {
                    enemy_pos.x = rnd.Next(1, 14) + 0.5;
                    enemy_pos.y = rnd.Next(3, 14) + 0.5;
                }
                while (!(((enemy_pos.x < dangerzone1.TopLeft.X || enemy_pos.x > dangerzone1.TopRight.X) || enemy_pos.y > dangerzone1.BottomLeft.Y) &&
                       ((enemy_pos.x < dangerzone2.TopLeft.X || enemy_pos.x > dangerzone2.TopRight.X) || enemy_pos.y > dangerzone2.BottomLeft.Y) &&
                       ((enemy_pos.x < dangerzone3.TopLeft.X || enemy_pos.x > dangerzone3.TopRight.X) || enemy_pos.y < dangerzone3.TopLeft.Y) &&
                       ((enemy_pos.x < dangerzone4.TopLeft.X || enemy_pos.x > dangerzone4.TopRight.X) || enemy_pos.y < dangerzone4.TopLeft.Y)));

                EnemySprite new_enemy = MainWindow.Instance.enemy.AddEnemy(maps[level_number].enemy, enemy_pos);
                current.Add(new_enemy);
                new_enemy.IsActive = true;
                MainWindow.Instance.enemy.EnemyLoad(new_enemy);
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
        public Rect Dangerzone(Rect rect) 
        {
            
            rect.Width -= 0.2;
            rect.X += 0.1;
            bool intersected = false;
            while (rect.Height < 20 && !intersected)
            {

                foreach (Rect hitbox in maps[level_number].hitboxes)
                {
                    if (rect.IntersectsWith(hitbox))
                    {
                        rect.Height = hitbox.Top - rect.Y;
                        intersected = true;
                        break;
                    }
                }
                if (!intersected)
                {
                    rect.Height++;
                }
            }
            rect.Height += 0.5;
            rect.X -= 1.5;
            rect.Width += 3;
            //SpriteManager.DebugRect(rect, 5);
            return rect;
        }
        public Rect PortalDanger(Rect rect)
        {

            
            bool intersected = false;
            while (rect.Height < 20 && !intersected)
            {

                foreach (Rect hitbox in maps[level_number].hitboxes)
                {
                    if (rect.IntersectsWith(hitbox))
                    {
                        rect.Y = hitbox.Y;
                        rect.Height = 17 - rect.Y;
                        intersected = true;
                        break;
                    }
                }
                if (!intersected)
                {
                    rect.Y--;
                }
            }
            
            
            rect.X -= 0.5;
            rect.Width += 1;
            //SpriteManager.DebugRect(rect, 5);
            return rect;
        }
        public void ClearAll() 
        {
            
            int fruitcount = FruitSprite.fruitList.Count;
            for (int i = 0; i < fruitcount; i++)
            {
                FruitSprite.fruitList[0].Destroy();
            }
            FruitSprite.fruitList.Clear();
            int enemycount = Enemy.enemyList.Count;
            for (int i = 0; i < enemycount; i++)
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
            MainWindow.Instance.score1 = 0;
            MainWindow.Instance.score2 = 0;
            MainWindow.Instance.lb_player2_name.Visibility = Visibility.Collapsed;
            MainWindow.Instance.lb_player2_score.Visibility = Visibility.Collapsed;
            MainWindow.Instance.chars.SelectedChar(1).Life = PlayerSprite.maxLife;
            MainWindow.Instance.chars.SelectedChar(2).Life = PlayerSprite.maxLife;

        }
    }
}
