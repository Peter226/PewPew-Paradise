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
    public class CollideComponent: SpriteComponent
    {
        public bool isOnGround;
        Vector2 splast;
        public override void Start()
        {
            splast = sprite.Position;
        }
        public CollideComponent(Sprite parent) : base(parent)
        {
        }
        public override void Enabled()
        {
            splast = sprite.Position;
            isOnGround = false;
        }
        public override void PostUpdate()
        {
            isOnGround = false;
            
           
            double distance = splast.DistanceTo(sprite.Position);
            int iterations = (int)Math.Ceiling(3 * distance);
            Vector2 phx = sprite.Position;
            for (int i=0; i < iterations; i++ )
            {
                Vector2 spbetween = splast + (phx - splast) * ((i + 1) / iterations);
                sprite.Position = spbetween;
                

                bool didhit = false;

                foreach (Rect platform in MainWindow.Instance.load.CurrentMap().hitboxes)
                {
                    Rect PlayerHitBox = sprite.GetRect();
                    if (PlayerHitBox.IntersectsWith(platform))
                    {
                        didhit = true;
                        Vector2 sp = sprite.Position;
                        double Bottom = Math.Abs(PlayerHitBox.Bottom - platform.Top);
                        double Left = Math.Abs(PlayerHitBox.Right - platform.Left);
                        double Right = Math.Abs(PlayerHitBox.Left - platform.Right);
                        double Top = Math.Abs(PlayerHitBox.Top - platform.Bottom);
                        double min = Math.Min(Math.Min(Bottom, Left), Math.Min(Right, Top));

                        PhysicsComponent physicsComponent = sprite.GetComponent<PhysicsComponent>();
                        if (Bottom == min)
                        {
                            sp.y -= PlayerHitBox.Bottom - platform.Top;
                            physicsComponent.speed.y = Math.Min(physicsComponent.speed.y, 0);
                            isOnGround = true;
                        }
                        else if (Top == min)
                        {
                            sp.y -= PlayerHitBox.Top - platform.Bottom;
                            physicsComponent.speed.y = Math.Max(physicsComponent.speed.y, 0);
                        }
                        else if (Right == min)
                        {
                            sp.x -= PlayerHitBox.Left - platform.Right;
                            physicsComponent.speed.x = Math.Max(physicsComponent.speed.x, 0);
                        }
                        else if (Left == min)
                        {
                            sp.x -= PlayerHitBox.Right - platform.Left;
                            physicsComponent.speed.x = Math.Min(physicsComponent.speed.x, 0);
                        }
                        sprite.Position = sp;





                    }
                }
                if (didhit) break;
               
            }
            splast = sprite.Position;
        }


    }
}
