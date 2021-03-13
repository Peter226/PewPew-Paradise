using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.GameLogic.SpriteComponents;
using PewPew_Paradise.Maths;

namespace PewPew_Paradise.GameLogic
{
    class ProjectileSprite : Sprite
    {
        public ProjectileSprite(string image, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {

        }
        public override void Update()
        {
           
            Vector2 pos = Position;
            if(Size.x > 0)
                pos.x += 0.01 * GameManager.DeltaTime;
            else
                pos.x -= 0.01 * GameManager.DeltaTime;
            if (Position.x > 15 || Position.x < 1)
                Destroy();
            Position = pos;
        }
    }
}
