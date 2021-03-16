using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
namespace PewPew_Paradise.GameLogic
{
    public class MapSprite : Sprite
    {
        /// <summary>
        /// MapSprite for changing background colors
        /// </summary>
        public SolidColorBrush map_color;

        public List<Rect> hitboxes = new List<Rect>();
        public Vector2 mapplace = new Vector2(8, 8);
        public Vector2 map_up = new Vector2(8, -16);
        Vector2 map_position = new Vector2(8, 24);
        public double timer;
        public bool just_loaded;
        public bool just_unloaded;
        public MapSprite(string image, SolidColorBrush map_background, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            map_color = map_background;
            

        }
        public delegate void MapLoadDelegate(MapSprite map);
        public static event MapLoadDelegate OnMapLoaded;
        public static event MapLoadDelegate OnMapUnloaded;
        
        public void MapLoaded() 
        {
            timer = 0;
            just_loaded = true;
            if (OnMapLoaded != null)
            {
                OnMapLoaded.Invoke(this);
            }
        }
        public void MapUnloaded()
        {
            timer = 0;
            just_unloaded = true;

            if (OnMapUnloaded != null)
            {
                OnMapUnloaded.Invoke(this);
            }
        }
        
        public override void Update()
        {
            
            timer += 0.001 * GameManager.DeltaTime;
            if (just_loaded)
            {
                if (timer > 1)
                {
                    just_loaded = false;
                    timer = 1;
                }
                Position = Vector2.Lerp(map_position, mapplace, timer);
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
            base.Update();
        }
    }
}
