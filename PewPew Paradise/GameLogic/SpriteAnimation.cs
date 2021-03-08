using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;
namespace PewPew_Paradise.GameLogic
{
    class SpriteAnimation
    {
        public double frameTime { get; }
        List<Vector2> keyFrames { get; } = new List<Vector2>();
        public SpriteAnimation(double frameTime)
        {
            this.frameTime = frameTime;
        }

    }
}
