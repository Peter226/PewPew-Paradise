using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;

namespace PewPew_Paradise.GameLogic
{
    public class AnimationCollection
    {
        public Vector2 spriteResolution;
        public Vector2 atlasDimensions;
        public string collectionName;
        public int fallbackAnimation;

        public List<SpriteAnimation> animations { get; } = new List<SpriteAnimation>();

        public AnimationCollection(string collectionName, Vector2 spriteResolution, Vector2 atlasDimensions, int fallbackAnimation = 0)
        {
            this.collectionName = collectionName;
            this.spriteResolution = spriteResolution;
            this.atlasDimensions = atlasDimensions;
            this.fallbackAnimation = fallbackAnimation;
        }

    }
}
