﻿using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Resources;
using System.IO;
using System.Reflection;

namespace PewPew_Paradise.GameLogic
{
    public class MapSprite : Sprite
    {
        public SolidColorBrush map_color;
        public List<Rect> hitboxes = new List<Rect>();
        public Vector2 mapplace = new Vector2(8, 8);
        public Vector2 map_up = new Vector2(8, -8);
        Vector2 map_position = new Vector2(8, 24);
        public double timer;
        public bool just_loaded;
        public bool just_unloaded;
        public string enemy;

        //JSON
        public static JsonSerializer map_serializer = new JsonSerializer();
        //JSON

        /// <summary>
        /// Creating a MapSprite where you can add the background color with map_background
        /// </summary>
        /// <param name="image"></param>
        /// <param name="enemy"></param>
        /// <param name="map_background"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="active"></param>
        public MapSprite(string image, string enemy, SolidColorBrush map_background, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            map_color = map_background;
            DeserializeMap();
            this.enemy = enemy;
        }
        //MapLoading events
        public delegate void MapLoadDelegate(MapSprite map);
        public static event MapLoadDelegate OnMapLoaded;
        public static event MapLoadDelegate OnMapUnloaded;
        
        //Saving the map collisions into the project
        public void SerializeMap()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            string path = Path.Combine(projectDirectory, "MapCollisions", this.image + ".json");
            StreamWriter sw = new StreamWriter(path);
            JsonWriter jw = new JsonTextWriter(sw);
            map_serializer.Serialize(jw,hitboxes);
            jw.Close();
            sw.Close();
        }


        //Loading the map collisions from the project
        public void DeserializeMap()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"PewPew_Paradise.MapCollisions.{this.image}.json";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                using (JsonReader jreader = new JsonTextReader(reader))
                {
                    hitboxes = (List<Rect>)map_serializer.Deserialize(jreader, typeof(List<Rect>));
                }
            }
        }
        //Invoke OnMapLoaded event when the map is loaded
        public void MapLoaded() 
        {
            Position = map_position;
            timer = 0;
            just_loaded = true;
            just_unloaded = false;
            if (OnMapLoaded != null)
            {
                OnMapLoaded.Invoke(this);
            }
        }
        //Invoke OnMapUnloaded event when the map is unloaded
        public void MapUnloaded()
        {
            timer = 0;
            just_unloaded = true;
            just_loaded = false;
            if (OnMapUnloaded != null)
            {
                OnMapUnloaded.Invoke(this);
            }
        }
        /// <summary>
        /// This checks when a new map has to load OnUpdate
        /// Activate mapchange animations
        /// Changing the background of the playingField to map_color
        /// Change map automatically 4 seconds after all the enemy is dead
        /// Destroying remaining FruitSprites
        /// </summary>
        public override void Update()
        {
            MainWindow.Instance.enemyHitTimer += 0.001 * GameManager.DeltaTime;
            timer += 0.00075 * GameManager.DeltaTime;
            if (just_loaded)
            {
                if (timer > 1)
                {
                    just_loaded = false;
                    timer = 1;
                }
                Position = Vector2.Lerp(map_position, mapplace, timer);
                double mcR = map_color.Color.R * timer + MainWindow.playingFieldBrush.Color.R * (1.0 - timer);
                double mcG = map_color.Color.G * timer + MainWindow.playingFieldBrush.Color.G * (1.0 - timer);
                double mcB = map_color.Color.B * timer + MainWindow.playingFieldBrush.Color.B * (1.0 - timer);

                MainWindow.playingFieldBrush.Color = Color.FromRgb((byte)mcR, (byte)mcG, (byte)mcB);
            }
            if (just_unloaded)
            {
                if (timer > 1)
                {
                    just_unloaded = false;
                    timer = 1;
                    IsActive = false;
                }
                    Position = Vector2.Lerp(mapplace, map_up, timer);
                
            }
            if (Enemy.enemyList.Count == 0 && MainWindow.Instance.enemyHitTimer > 4)
            {
                for(int i=0; i < FruitSprite.fruitList.Count; i++)
                {
                    FruitSprite.fruitList[i].Destroy();
                }
                FruitSprite.fruitList.Clear();
                MainWindow.Instance.load.NextMap(MainWindow.Instance.player_number);
            }
            base.Update();
        }
    }
}
