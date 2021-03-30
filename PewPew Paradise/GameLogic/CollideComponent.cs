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
        public delegate void OnCollideDelegate();

        public event OnCollideDelegate OnCollide;
        
        public override void PostUpdate()
        {
            isOnGround = false;
            
           
            double distance = splast.DistanceTo(sprite.Position);
            int iterations = (int)Math.Ceiling(3 * distance);
            Vector2 phx = sprite.Position;
            for (int i=0; i < iterations; i++ )
            {
                Vector2 spbetween = splast + (phx - splast) * (((double)i + 1.0) / (double)iterations);
                sprite.Position = spbetween;
                

                bool didhit = false;
                foreach (Rect platform in MainWindow.Instance.load.CurrentMap().hitboxes)
                {
                    //Rect modplat =  new Rect(platform.X + MainWindow.Instance.load.CurrentMap().Position.x - 8, platform.Y + MainWindow.Instance.load.CurrentMap().Position.y - 8, platform.Width,platform.Height);
                    Rect modplat = platform;
                    Rect PlayerHitBox = sprite.GetRect();
                    PlayerHitBox.X += 0.1;
                    PlayerHitBox.Width -= 0.2;
                    PlayerHitBox.Y += 0.0001;
                    PlayerHitBox.Height -= 0.0001;
                    if (PlayerHitBox.IntersectsWith(modplat))
                    {
                        didhit = true;
                        Vector2 sp = sprite.Position;
                        double Bottom = Math.Abs(PlayerHitBox.Bottom - modplat.Top);
                        double Left = Math.Abs(PlayerHitBox.Right - modplat.Left);
                        double Right = Math.Abs(PlayerHitBox.Left - modplat.Right);
                        double Top = Math.Abs(PlayerHitBox.Top - modplat.Bottom);
                        double min = Math.Min(Math.Min(Bottom, Left), Math.Min(Right, Top));
                        PhysicsComponent physicsComponent = sprite.GetComponent<PhysicsComponent>();
                        if (physicsComponent != null)
                        {
                            if (Bottom == min)
                            {
                                sp.y -= PlayerHitBox.Bottom - modplat.Top;
                                physicsComponent.speed.y = Math.Min(physicsComponent.speed.y, 0);
                                isOnGround = true;
                            }
                            else if (Top == min)
                            {
                                sp.y -= PlayerHitBox.Top - modplat.Bottom;
                                physicsComponent.speed.y = Math.Max(physicsComponent.speed.y, 0);
                            }
                            else if (Right == min)
                            {
                                sp.x -= PlayerHitBox.Left - modplat.Right;
                                physicsComponent.speed.x = Math.Max(physicsComponent.speed.x, 0);
                            }
                            else if (Left == min)
                            {
                                sp.x -= PlayerHitBox.Right - modplat.Left;
                                physicsComponent.speed.x = Math.Min(physicsComponent.speed.x, 0);
                            }
                        }
                        else
                        {
                            if (Bottom == min)
                            {
                                sp.y -= PlayerHitBox.Bottom - modplat.Top;
                                isOnGround = true;
                            }
                            else if (Top == min)
                            {
                                sp.y -= PlayerHitBox.Top - modplat.Bottom;
                            }
                            else if (Right == min)
                            {
                                sp.x -= PlayerHitBox.Left - modplat.Right;
                            }
                            else if (Left == min)
                            {
                                sp.x -= PlayerHitBox.Right - modplat.Left;
                            }
                        }

                        sprite.Position = sp;
                    }
                }
                if (didhit) 
                {
                    if(OnCollide != null)
                    {
                        OnCollide.Invoke();
                    }
                    break;
                }
               
            }
            splast = sprite.Position;
        }
    }
}
