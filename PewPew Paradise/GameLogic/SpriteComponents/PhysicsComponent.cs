﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;


namespace PewPew_Paradise.GameLogic.SpriteComponents
{
    public class PhysicsComponent : SpriteComponent
    {
        const double gravityspeed = 9.81;
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
