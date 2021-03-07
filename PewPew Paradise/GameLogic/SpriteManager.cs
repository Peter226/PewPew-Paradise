using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using PewPew_Paradise.GameLogic;
using PewPew_Paradise.Maths;
namespace PewPew_Paradise.GameLogic
{
    public class SpriteManager
    {
        /// <summary>
        /// Current active SpriteManager
        /// </summary>
        public static SpriteManager instance;

        /// <summary>
        /// Panel that is used as the canvas for sprites
        /// </summary>
        public Grid canvas { get; } = (Grid)MainWindow.Instance.Content;

        private List<Sprite> _sprites = new List<Sprite>();
        private Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();

        public SpriteManager()
        {
            instance = this;
        }


        /// <summary>
        /// Load an image to be used for a sprite
        /// </summary>
        /// <param name="path">local path of the image</param>
        /// <param name="name">a name for later reference to the image</param>
        public void LoadImage(string path, string name)
        {
            _images.Add(name,new BitmapImage(new Uri(path, UriKind.Relative)));
        }



        /// <summary>
        /// Translates vector in game units to vector in screen units
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Vector2 VectorToCanvas(Vector2 vector)
        {
            Vector2 canvas = vector;
            canvas.x *= this.canvas.Width / GameManager.GameUnitSize;
            canvas.y *= this.canvas.Height / GameManager.GameUnitSize;
            return canvas;
        }

        /// <summary>
        /// Translates vector in game units to vector in screen units
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Vector2 CanvasToVector(Vector2 canvas)
        {
            Vector2 vector = canvas;
            vector.x *= GameManager.GameUnitSize / this.canvas.Width;
            vector.y *= GameManager.GameUnitSize / this.canvas.Height;
            return vector;
        }


        /// <summary>
        /// Create a sprite on the canvas
        /// </summary>
        /// <param name="image">Image reference name (defined in LoadImage)</param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Sprite CreateSprite(string image, Vector2 position, Vector2 size)
        {
            if (_images.ContainsKey(image))
            {
                Sprite sprite = new Sprite(_images[image]);
                sprite.Position = position;
                sprite.Size = size;
                _sprites.Add(sprite);
                return sprite;
            }
            else
            {
                throw new Exception($"SpriteManager could not create sprite\nImage not found: {image}");
            }
        }
    }
}
