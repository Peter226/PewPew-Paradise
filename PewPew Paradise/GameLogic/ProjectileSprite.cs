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
    class ProjectileSprite : Sprite
    {
        /// <summary>
        /// Setting the projectile's components
        /// Enabling collision and disabling physics
        /// </summary>
        /// <param name="image"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="active"></param>
        public ProjectileSprite(string image, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            AddComponent<CollideComponent>().OnCollide += Collision;
            AddComponent<PhysicsComponent>().IsActive = false;
        }
        /// <summary>
        /// Setting the speed of the projectile
        /// Checking if projectile intersects with the enemy sprite and if it does what to do
        /// </summary>
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
            EnemySprite hitEnemy = null;
            foreach (EnemySprite enemy in Enemy.enemyList)
            {
                Rect enemyHitBox = enemy.GetRect();
                if (enemyHitBox.IntersectsWith(this.GetRect()))
                {
                    hitEnemy = enemy;
                    break;
                    
                }
            }
            if(hitEnemy!=null)
            {
                    hitEnemy.EnemyDeath();
                    MainWindow.Instance.enemyHitTimer = 1;
                    Destroy();
            }

        }
        /// <summary>
        /// Destroying the projectile upon colliding with enemy sprite
        /// </summary>
        public void Collision()
        {
            Destroy();
        }
    }
}
