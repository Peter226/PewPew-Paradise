using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.GameLogic.SpriteComponents;
using PewPew_Paradise.Maths;

namespace PewPew_Paradise.GameLogic
{
    public class EnemySprite : Sprite
    {
        

        public EnemySprite(string image,  Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            
            AddComponent<PhysicsComponent>().IsActive = false;
            AddComponent<CollideComponent>().IsActive = false;
        }

    }
}
