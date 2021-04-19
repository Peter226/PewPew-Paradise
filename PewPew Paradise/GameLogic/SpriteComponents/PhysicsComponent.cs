using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;


namespace PewPew_Paradise.GameLogic.SpriteComponents
{
    /// <summary>
    /// Used to apply physics like gravity to sprites
    /// </summary>
    public class PhysicsComponent : SpriteComponent
    {

        //gravity constant
        const double gravityspeed = 9.81;
        /// <summary>
        /// current speed/velocity of the sprite
        /// </summary>
        public Vector2 speed;
        public PhysicsComponent(Sprite parent) : base(parent)
        { 
        }
        /// <summary>
        /// Defines the gravity/falling speed of sprites
        /// </summary>
        /// <param name="gravityspeed"></param>
        /// <param name="speed"></param>
        public override void Update()
        {
            speed.y += gravityspeed * 0.002 * GameManager.DeltaTime;
            sprite.Position += speed * 0.002 * GameManager.DeltaTime;
            CollideComponent collider = sprite.GetComponent<CollideComponent>();
            if (collider != null)
            {
                if (collider.isOnGround)
                {
                    speed.x /= 1 + (GameManager.DeltaTime * 0.01);
                }
            }
        }
        /// <summary>
        /// Disables the gravity/falling speed of sprites
        /// </summary>
        public override void Disabled()
        {
            speed = Vector2.Zero;
        }
    }
}
