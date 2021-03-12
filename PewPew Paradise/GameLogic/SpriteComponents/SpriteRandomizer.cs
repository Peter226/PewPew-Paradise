using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PewPew_Paradise.GameLogic.SpriteComponents
{
    public class SpriteRandomizer : SpriteComponent
    {
        Random random = new Random();
        public SpriteRandomizer(Sprite parent) : base(parent)
        {
        }
        
        public override void Update()
        {
            sprite.Position = new Maths.Vector2(random.NextDouble() * 16.0, random.NextDouble() * 16.0);
        }

    }
}
