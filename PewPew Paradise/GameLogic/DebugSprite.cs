using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PewPew_Paradise.GameLogic
{
    public class DebugSprite : Sprite
    {
        double timer;
        private static SolidColorBrush _colliderBrush = new SolidColorBrush(Color.FromArgb(180, 0, 255, 0));
        public DebugSprite(string image, double time, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            timer = time;
            RectangleElement.Fill = _colliderBrush;
        }


        public override void Start()
        {
            base.Start();
        }

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
