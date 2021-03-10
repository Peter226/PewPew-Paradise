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
        public int priority;

        public SpriteAnimation(double frameTime, bool looping, int priority = 1)
        {
            this.frameTime = frameTime;
            loop = looping;
            this.priority = priority;
        }

    }
}
