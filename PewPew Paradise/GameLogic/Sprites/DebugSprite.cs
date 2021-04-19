using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PewPew_Paradise.GameLogic
{
    /// <summary>
    /// Debug sprite used for visualizing rects
    /// </summary>
    public class DebugSprite : Sprite
    {
        private double timer;
        private static SolidColorBrush _colliderBrush = new SolidColorBrush(Color.FromArgb(180, 0, 255, 0));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="time">lifetime</param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="active"></param>
        public DebugSprite(string image, double time, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            timer = time;
            RectangleElement.Fill = _colliderBrush;
        }
        /// <summary>
        /// Remove from the lifetime of the debug element, and destroy it when it has expired
        /// </summary>
        public override void Update()
        {
            base.Update();
            timer -= GameManager.DeltaTime * 0.001;
            if (timer < 0)
            {
                Destroy();
            }
        }

    }
}
