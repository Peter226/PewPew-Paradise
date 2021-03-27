using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.GameLogic.SpriteComponents;
using PewPew_Paradise.Maths;
namespace PewPew_Paradise.GameLogic.SpriteComponents
{
    public class EnemyLoadAnimatorComponent : SpriteComponent
    {
        double timer = 0;
        Vector2 destination;
        public EnemyLoadAnimatorComponent(Sprite parent) : base(parent)
        {
            destination = sprite.Position;
        }


        public override void PostUpdate()
        {
            PhysicsComponent pc = sprite.GetComponent<PhysicsComponent>();
            if (pc != null)
            {
                pc.IsActive = false;
            }
            timer += GameManager.DeltaTime;
            if (timer > 4)
            {
                if (pc != null)
                {
                    pc.IsActive = true;
                }
                Destroy();
            }
        }



    }
}
