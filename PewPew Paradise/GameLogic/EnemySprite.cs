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
        double timer = 0;
        public bool dead;
        public EnemySprite(string image,  Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            Enemy.enemyList.Add(this);
            AddComponent<PhysicsComponent>().IsActive = false;
            AddComponent<CollideComponent>().IsActive = false;
            AddComponent<Portal>().IsActive = false;
            AddComponent<AnimatorComponent>().SetAnimation("Enemy");
        }
        public void EnemyDeath()
        {
            if (!dead) {
                dead = true;
                GetComponent<PhysicsComponent>().IsActive = false;
                GetComponent<CollideComponent>().IsActive = false;
                GetComponent<AnimatorComponent>().PlayAnimation(3);
                GetComponent<AnimatorComponent>().OnAnimationEnded += FinishDeath;
            }
        }

        private void FinishDeath(AnimatorComponent animator, int animationID)
        {
            GetComponent<AnimatorComponent>().OnAnimationEnded -= FinishDeath;
            Random random = new Random();
            int randomfruit = random.Next(FruitSprite.fruitTypes.Count);
            new FruitSprite(FruitSprite.fruitTypes[randomfruit].name, this.Position, Vector2.One).point = FruitSprite.fruitTypes[randomfruit].point;
            Destroy();
        }
        protected override void OnDestroy()
        {
            Enemy.enemyList.Remove(this);
        }

        public override void Update()
        {
            if (!dead) {
                Vector2 pos = this.Position;
                Vector2 size = this.Size;
                timer += GameManager.DeltaTime * 0.001;
                if (!MainWindow.Instance.load.CurrentMap().just_loaded)
                {
                    if (timer < 7)
                    {
                        size.x = 1;
                        pos.x += 0.002 * GameManager.DeltaTime;
                        this.Position = pos;
                        this.Size = size;
                    }
                    else if (timer > 7 && timer < 14)
                    {
                        pos.x += -0.002 * GameManager.DeltaTime;
                        size.x = -1;
                        this.Size = size;
                    }
                    else
                    {
                        timer = 0;
                    }
                    if (timer % 2 < 0.25 && GetComponent<CollideComponent>().isOnGround)
                    {
                        GetComponent<AnimatorComponent>().PlayAnimation(1);
                        GetComponent<PhysicsComponent>().speed.y = -6.375;
                    }
                    this.Position = pos;
                }

                if (!GetComponent<CollideComponent>().isOnGround && GetComponent<PhysicsComponent>().speed.y > 0.0)
                {
                    GetComponent<AnimatorComponent>().PlayAnimation(2);
                }
            }

        }

    }
}
