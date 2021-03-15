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

       public override void Update()
        {
            isOnGround = false;
            
           
            double distance = splast.DistanceTo(sprite.Position);
            int iterations = (int)Math.Ceiling(3 * distance);
            Vector2 phx = sprite.Position;
            for (int i=0; i<iterations; i++ )
            {
                Vector2 spbetween = splast + (phx - splast) * ((i + 1) / iterations);
                sprite.Position = spbetween;
                Rect PlayerHitBox = sprite.GetRect();

                bool didhit = false;

                foreach (Rect platform in MainWindow.Instance.load.CurrentMap().hitboxes)
                {
                    if (PlayerHitBox.IntersectsWith(platform))
                    {
                        didhit = true;
                        Vector2 sp = spbetween;
                        double Bottom = Math.Abs(PlayerHitBox.Bottom - platform.Top);
                        double Left = Math.Abs(PlayerHitBox.Right - platform.Left);
                        double Right = Math.Abs(PlayerHitBox.Left - platform.Right);
                        double Top = Math.Abs(PlayerHitBox.Top - platform.Bottom);
                        double min = Math.Min(Math.Min(Bottom, Left), Math.Min(Right, Top));

                        if (Bottom == min)
                        {
                            sp.y -= PlayerHitBox.Bottom - platform.Top;
                            sprite.Position = sp;
                            sprite.GetComponent<PhysicsComponent>().speed.y = 0;
                            isOnGround = true;
                        }
                        else if (Top == min)
                        {
                            sp.y -= PlayerHitBox.Top - platform.Bottom;
                            sprite.Position = sp;
                            sprite.GetComponent<PhysicsComponent>().speed.y = 0;
                        }
                        else if (Right == min)
                        {
                            sp.x -= PlayerHitBox.Left - platform.Right;
                            sprite.Position = sp;
                            sprite.GetComponent<PhysicsComponent>().speed.x = 0;
                        }
                        else if (Left == min)
                        {
                            sp.x -= PlayerHitBox.Right - platform.Left;
                            sprite.Position = sp;
                            sprite.GetComponent<PhysicsComponent>().speed.x = 0;
                        }






                    }
                }
                if (didhit) break;
               
            }
            splast = sprite.Position;
        }


    }
}
