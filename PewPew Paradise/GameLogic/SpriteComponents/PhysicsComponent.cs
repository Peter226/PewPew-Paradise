using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;


namespace PewPew_Paradise.GameLogic.SpriteComponents
{
    class PhysicsComponent : SpriteComponent
    {
        const double gravityspeed = 9.81;
        public PhysicsComponent(Sprite parent) : base(parent)
        { 
        }
        public override void Update()
        {
            sprite.Position = new Vector2(sprite.Position.x, sprite.Position.y + gravityspeed*0.01);
        }

    }
}
