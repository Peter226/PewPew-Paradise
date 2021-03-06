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
        private List<Sprite> _sprites = new List<Sprite>();
        private Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();
        public Grid canvas { get; } = (Grid)MainWindow.Instance.Content;

        


        public void LoadImage(string path, string name)
        {
            _images.Add(name,new BitmapImage(new Uri(path, UriKind.Relative)));
        }

        public static SpriteManager Instance;

        public SpriteManager()
        {
            Instance = this;
        }


        public Vector2 vectorToCanvas(Vector2 vector)
        {
            Vector2 canvas = vector;
            canvas.x *= this.canvas.Width / GameManager.GameUnitSize;
            canvas.y *= this.canvas.Height / GameManager.GameUnitSize;
            return canvas;
        }
        public Vector2 canvasToVector(Vector2 canvas)
        {
            Vector2 vector = canvas;
            vector.x *= GameManager.GameUnitSize / this.canvas.Width;
            vector.y *= GameManager.GameUnitSize / this.canvas.Height;
            return vector;
        }


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
