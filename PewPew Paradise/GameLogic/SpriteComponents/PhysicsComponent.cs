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
        Vector2 speed;
        public PhysicsComponent(Sprite parent) : base(parent)
        { 
        }
        public override void Update()
        {
            speed.y += gravityspeed*0.000015*GameManager.DeltaTime;
            sprite.Position += speed;
            
        }
        public override void Disabled()
        {
            speed = Vector2.Zero;
        }

    }
}
