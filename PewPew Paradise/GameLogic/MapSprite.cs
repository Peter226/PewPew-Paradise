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

        public MapSprite(string image, SolidColorBrush map_background, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            map_color = map_background;

        }

    }
}
