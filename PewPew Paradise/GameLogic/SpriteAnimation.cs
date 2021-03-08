using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;
namespace PewPew_Paradise.GameLogic
{
    public class SpriteAnimation
    {
        public double frameTime { get; }
        public List<Vector2> keyFrames { get; } = new List<Vector2>();
        public bool loop;

        public SpriteAnimation(double frameTime, bool looping)
        {
            this.frameTime = frameTime;
            loop = looping;
        }

    }
}
