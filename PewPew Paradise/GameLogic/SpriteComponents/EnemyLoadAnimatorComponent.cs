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
        
        public EnemyLoadAnimatorComponent(Sprite parent) : base(parent)
        {
        }


        public override void PostUpdate()
        {
            timer += GameManager.DeltaTime;
            if (timer > 4)
            {

            }
        }



    }
}
