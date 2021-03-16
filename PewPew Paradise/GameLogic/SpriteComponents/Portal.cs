using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PewPew_Paradise.GameLogic.SpriteComponents
{
    public class Portal : SpriteComponent
    {
        public Portal(Sprite parent) : base(parent)
        {
        }
        public override void PreUpdate()
        {
            Vector2 pos = sprite.Position;
            if (sprite.Position.y > 15)
            {
                    pos.y = 2;
            }
            sprite.Position = pos;
            base.PreUpdate();
        }
    }
}
