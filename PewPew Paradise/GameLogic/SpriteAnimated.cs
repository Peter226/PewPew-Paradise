using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;


namespace PewPew_Paradise.GameLogic
{
    public class SpriteAnimated : Sprite
    {
        private AnimationCollection _animationCollection { get; }
        public SpriteAnimated(string image, string animationCollection, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            _brush.Viewport = new System.Windows.Rect(new System.Windows.Point(0.0, 0.0), new System.Windows.Point(4.0,4.0));
        }
   
        


    }
}
