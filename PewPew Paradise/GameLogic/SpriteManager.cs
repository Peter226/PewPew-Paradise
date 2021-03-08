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
using System.Windows.Resources;

namespace PewPew_Paradise.GameLogic
{
    public class SpriteManager
    {
        /// <summary>
        /// All existing sprites
        /// </summary>
        public List<Sprite> Sprites = new List<Sprite>();
        /// <summary>
        /// Canvas UIElement
        /// </summary>
        public Grid canvas { get; } = (Grid)MainWindow.Instance.Content;
        /// <summary>
        /// Current active SpriteManager instance
        /// </summary>
        public static SpriteManager Instance;
        /// <summary>
        /// Loaded images
        /// </summary>
        private Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();

        /// <summary>
        /// Load an image and assign a reference name to it
        /// </summary>
        /// <param name="path">Image path (local)</param>
        /// <param name="name">Reference name</param>
        public void LoadImage(string path, string name)
        {
            Uri uri = new Uri(path, UriKind.Relative);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = info.Stream;
            image.CacheOption = BitmapCacheOption.OnDemand;
            image.EndInit();
            _images.Add(name, image);
        }


        /// <summary>
        /// Create a new SpriteManager
        /// </summary>
        public SpriteManager()
        {
            Instance = this;
        }

        /// <summary>
        /// Convert Vector2 from game units to screen units
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
        /// Convert Vector2 from screen units to game units
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public Vector2 CanvasToVector(Vector2 canvas)
        {
            Vector2 vector = canvas;
            vector.x *= GameManager.GameUnitSize / this.canvas.Width;
            vector.y *= GameManager.GameUnitSize / this.canvas.Height;
            return vector;
        }

        /// <summary>
        /// Get an already loaded Image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public BitmapImage GetImage(string image)
        {
            if (_images.ContainsKey(image))
            {
                return _images[image];
            }
            else
            {
                throw new Exception($"Image not found: {image}");
            }
        }

        /// <summary>
        /// Add an existing sprite to the sprite collection [Do not use unless necessary]
        /// </summary>
        /// <param name="sprite"></param>
        public void AddSprite(Sprite sprite)
        {
            canvas.Children.Add(sprite.RectangleElement);
            Sprites.Add(sprite);
            sprite.Start();
        }
    }
}
