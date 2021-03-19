using PewPew_Paradise.GameLogic.SpriteComponents;
using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PewPew_Paradise.GameLogic
{
    public class Enemy
    {
        
        public void EnemyLoad(EnemySprite enemy)
        {

            enemy.IsActive = true;
            enemy.GetComponent<PhysicsComponent>().IsActive = true;
            enemy.GetComponent<CollideComponent>().IsActive = true;

        }
        public EnemySprite AddEnemy(string spritename, Vector2 pos)
        { 

            
            EnemySprite enemy= new EnemySprite(spritename, pos, new Vector2(1, 1), false);
            return enemy;
        }
        public void UnloadEnemy()
        {
            

        }
    }
}
