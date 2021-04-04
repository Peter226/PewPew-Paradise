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
        static public List<EnemySprite> enemyList = new List<EnemySprite>();
        
        
        /// <summary>
        /// Loading an enemy, make components and activity true
        /// </summary>
        /// <param name="enemy"></param>
        public void EnemyLoad(EnemySprite enemy)
        {

            enemy.IsActive = true;
            enemy.GetComponent<PhysicsComponent>().IsActive = true;
            enemy.GetComponent<CollideComponent>().IsActive = true;
            enemy.GetComponent<Portal>().IsActive = true;

        }
        /// <summary>
        /// Creates an enemy in a position
        /// spritename is the image of the enemysprite
        /// </summary>
        /// <param name="spritename"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public EnemySprite AddEnemy(string spritename, Vector2 pos)
        { 
            EnemySprite enemy = new EnemySprite(spritename, pos, new Vector2(1, 1), false);
            
            return enemy;
        }
    }
}
