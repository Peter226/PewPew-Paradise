using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PewPew_Paradise.GameLogic.SpriteComponents;
using PewPew_Paradise.Maths;

namespace PewPew_Paradise.GameLogic
{
    class CollideComponent: SpriteComponent
    {
        public CollideComponent(Sprite parent) : base(parent)
        {
        }
       public override void Update()
        {
            Rect PlayerHitBox = sprite.GetRect();
            foreach (Rect platform in MainWindow.Instance.load.CurrentMap().hitboxes)
            {
                if (PlayerHitBox.IntersectsWith(platform))
                {
                    Vector2 sp = sprite.Position;
                    sp.y -= PlayerHitBox.Bottom - platform.Top;
                    sprite.Position = sp;
                    sprite.GetComponent<PhysicsComponent>().speed.y = 0; 
                    


                }
            }
        }

    }
}
