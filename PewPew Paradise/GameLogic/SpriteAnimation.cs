using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;
namespace PewPew_Paradise.GameLogic
{
    /// <summary>
    /// Animation class for sprites
    /// </summary>
    public class SpriteAnimation
    {
        public double frameTime { get; }
        /// <summary>
        /// List of keyframes (indexers in the texture atlas)
        /// </summary>
        public List<Vector2> keyFrames { get; } = new List<Vector2>();
        /// <summary>
        /// is it a looping animation?
        /// </summary>
        public bool loop;
        /// <summary>
        /// overwrite priority
        /// </summary>
        public int priority;
        /// <summary>
        /// Create a new sprite animation
        /// </summary>
        /// <param name="frameTime">time in milliseconds</param>
        /// <param name="looping">does the animation loop?</param>
        /// <param name="priority">priority of the animation</param>
        public SpriteAnimation(double frameTime, bool looping, int priority = 1)
        {
            this.frameTime = frameTime;
            loop = looping;
            this.priority = priority;
        }

    }
}
