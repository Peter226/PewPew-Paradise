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
        

        public EnemySprite(string image,  Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            Enemy.enemyList.Add(this);
            AddComponent<PhysicsComponent>().IsActive = false;
            AddComponent<CollideComponent>().IsActive = false;
            AddComponent<Portal>().IsActive = false;
        }
        public void EnemyDeath()
        {
            Random random = new Random();
            int randomfruit = random.Next(FruitSprite.fruitTypes.Count);
            new FruitSprite(FruitSprite.fruitTypes[randomfruit].name, this.Position, Vector2.One).point = FruitSprite.fruitTypes[randomfruit].point;
            Destroy();
        }

        protected override void OnDestroy()
        {
            Enemy.enemyList.Remove(this);
        }

    }
}
