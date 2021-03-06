using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.GameLogic.SpriteComponents;

namespace PewPew_Paradise.GameLogic
{
    /// <summary>
    /// Sprite type for fruits
    /// </summary>
    public class FruitSprite : Sprite
    {
        static public List<FruitType> fruitTypes = new List<FruitType>();
        static public List<FruitSprite> fruitList = new List<FruitSprite>();
        public int point;
        /// <summary>
        /// Adding physics, collision and portal components to collectibles
        /// Storing each of them in a fruitList list
        /// </summary>
        /// <param name="image"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="active"></param>
        public FruitSprite(string image, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            AddComponent<PhysicsComponent>();
            AddComponent<CollideComponent>();
            AddComponent<PortalComponent>();
            fruitList.Add(this);
        }
        /// <summary>
        /// Adding each collectibles to fruitTypes list with their name and point
        /// Loading their sprite with LoadImage
        /// </summary>
        public static void LoadImages()
        {
            fruitTypes.Add(new FruitType("C_Banana", 1000));
            SpriteManager.LoadImage("Images/Sprites/Collectibles/Banana.png","C_Banana");
            fruitTypes.Add(new FruitType("C_Cherry", 400));
            SpriteManager.LoadImage("Images/Sprites/Collectibles/Cherry.png", "C_Cherry");
            fruitTypes.Add(new FruitType("C_Grapes", 700));
            SpriteManager.LoadImage("Images/Sprites/Collectibles/Grapes.png", "C_Grapes");
            fruitTypes.Add(new FruitType("C_Kiwi", 900));
            SpriteManager.LoadImage("Images/Sprites/Collectibles/Kiwi.png", "C_Kiwi");
            fruitTypes.Add(new FruitType("C_Lemon", 500));
            SpriteManager.LoadImage("Images/Sprites/Collectibles/Lemon.png", "C_Lemon");
            fruitTypes.Add(new FruitType("C_Plum", 600));
            SpriteManager.LoadImage("Images/Sprites/Collectibles/Plum.png", "C_Plum");
            fruitTypes.Add(new FruitType("C_Strawberry", 800));
            SpriteManager.LoadImage("Images/Sprites/Collectibles/Strawberry.png", "C_Strawberry");
            fruitTypes.Add(new FruitType("C_Orange", 100));
            SpriteManager.LoadImage("Images/Sprites/Collectibles/Orange.png", "C_Orange");
            fruitTypes.Add(new FruitType("C_Apple", 200));
            SpriteManager.LoadImage("Images/Sprites/Collectibles/Apple.png", "C_Apple");
            fruitTypes.Add(new FruitType("C_Pineapple", 300));
            SpriteManager.LoadImage("Images/Sprites/Collectibles/Pineapple.png", "C_Pineapple");

        }
        /// <summary>
        /// Removeing the collectible from the list when collected and destroying it
        /// </summary>
        public void FruitCollect()
        {
            fruitList.Remove(this);
            Destroy();
        }
    }
}
