using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using PewPew_Paradise.Maths;
namespace PewPew_Paradise.GameLogic
{
    public class SpriteManager
    {
        private List<Sprite> _sprites = new List<Sprite>();
        private Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();

        public void LoadImage(string path, string name)
        {
            _images.Add(name,new BitmapImage(new Uri(path)));
        }

        public static SpriteManager Instance;

        public SpriteManager()
        {
            Instance = this;
        }

        public void CreateSprite(string image, Vector2 position, Vector2 size)
        {
            if (_images.ContainsKey(image))
            {
                Sprite sprite = new Sprite(_images[image]);
                _sprites.Add(sprite);
            }
            else
            {
                throw new Exception($"SpriteManager could not create sprite\nImage not found: {image}");
            }
        }
    }
}
