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
using System.ComponentModel;
using PewPew_Paradise.GameLogic.SpriteComponents;

namespace PewPew_Paradise.GameLogic
{
    public class SpriteManager
    {
        /// <summary>
        /// All existing sprites
        /// </summary>
        public static Dictionary<int,Sprite> Sprites = new Dictionary<int,Sprite>();
        /// <summary>
        /// Canvas UIElement
        /// </summary>
        public static Grid canvas { get; } = (Grid)MainWindow.Instance.Content;
        /// <summary>
        /// Loaded images
        /// </summary>
        private static Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();

        /// <summary>
        /// Added animation collections
        /// </summary>
        private static Dictionary<string, AnimationCollection> _animations = new Dictionary<string, AnimationCollection>();

        /// <summary>
        /// Load an image and assign a reference name to it
        /// </summary>
        /// <param name="path">Image path (local)</param>
        /// <param name="name">Reference name</param>
        public static void LoadImage(string path, string name)
        {
            if (!_images.ContainsKey(name)) {
                Uri uri = new Uri(path, UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = info.Stream;
                image.CacheOption = BitmapCacheOption.OnDemand;
                image.EndInit();
                _images.Add(name, image);
            }
            else
            {
                Console.WriteLine($"SpriteManager: Image already loaded: {name} ignoring...");
            }
        }

        /// <summary>
        /// Add an AnimationCollection to the usable animation collections and assign a reference
        /// </summary>
        /// <param name="animationCollection">An AnimationCollection (created manually, not loaded)</param>
        /// <param name="name">Reference name</param>
        public static void AddAnimationCollection(AnimationCollection animationCollection, string name)
        {
            if (!_animations.ContainsKey(name)) {
                _animations.Add(name, animationCollection);
            }
            else
            {
                Console.WriteLine($"SpriteManager: Animation already added: {name} ignoring...");
            }
        }


        /// <summary>
        /// Convert Vector2 from game units to screen units
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 VectorToCanvas(Vector2 vector)
        {
            Vector2 canvasV = vector;
            canvasV.x *= canvas.Width / GameManager.GameUnitSize;
            canvasV.y *= canvas.Height / GameManager.GameUnitSize;
            return canvasV;
        }
        /// <summary>
        /// Convert Vector2 from screen units to game units
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public static Vector2 CanvasToVector(Vector2 canvasV)
        {
            Vector2 vector = canvasV;
            vector.x *= GameManager.GameUnitSize / canvas.Width;
            vector.y *= GameManager.GameUnitSize / canvas.Height;
            return vector;
        }

        /// <summary>
        /// Get an already loaded Image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static BitmapImage GetImage(string image)
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
        /// Get an added AnimationCollection
        /// </summary>
        /// <param name="animationCollection"></param>
        /// <returns></returns>
        public static AnimationCollection GetAnimationCollection(string animationCollection)
        {
            if (_animations.ContainsKey(animationCollection))
            {
                return _animations[animationCollection];
            }
            else
            {
                throw new Exception($"Animation collection not found: {animationCollection}");
            }
        }


        /// <summary>
        /// Add an existing sprite to the sprite collection [Do not use unless necessary]
        /// </summary>
        /// <param name="sprite"></param>
        public static void AddSprite(Sprite sprite)
        {
            canvas.Children.Add(sprite.RectangleElement);
            Sprites.Add(sprite.ID,sprite);
            sprite.Start();
        }

        public static void RemoveSprite(Sprite sprite)
        {
            ((Panel)sprite.RectangleElement.Parent).Children.Remove(sprite.RectangleElement);
            Sprites.Remove(sprite.ID);
        }


        /// <summary>
        /// Get a unique sprite ID for a new Sprite [Do not use unless necessary]
        /// </summary>
        /// <param name="sprite"></param>
        public static int CreateSpriteID()
        {
            for (int i = 0;i < int.MaxValue;i++)
            {
                if (!Sprites.ContainsKey(i))
                {
                    return i;
                }
            }
            return 0;
        }


    }
}
