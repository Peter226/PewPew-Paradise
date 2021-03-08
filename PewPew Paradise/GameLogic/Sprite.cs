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
    /// <summary>
    /// Basic Sprite with an image
    /// </summary>
    public class Sprite
    {
        private Image _image;
        private Vector2 _position;
        private Vector2 _size;
        private bool _active;

        /// <summary>
        /// Enable or disable the sprite and it's visibility
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                _image.IsEnabled = _active;
                _image.Visibility = (Visibility)_active.CompareTo(true);
            }
        }




        /// <summary>
        /// Called when sprite is created
        /// </summary>
        public virtual void Start()
        {

        }

        /// <summary>
        /// Called every frame
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Create a new sprite and add it to the screen
        /// </summary>
        /// <param name="image">Name reference defined with SpriteManager.Instance.LoadImage</param>
        /// <param name="position">Position in game units</param>
        /// <param name="size">Size in game units</param>
        public Sprite(string image, Vector2 position, Vector2 size, bool active = true)
        {
            _image = new Image();
            _image.Stretch = Stretch.Fill;
            _image.HorizontalAlignment = HorizontalAlignment.Left;
            _image.VerticalAlignment = VerticalAlignment.Top;
            _image.Source = SpriteManager.Instance.GetImage(image);
            _image.RenderTransformOrigin = new Point(0.5, 0.5);
            Position = position;
            Size = size;
            IsActive = active;
            SpriteManager.Instance.AddSprite(this);
            GameManager.Instance.OnUpdate += Update;
        }


        /// <summary>
        /// The UIElement of the Sprite
        /// </summary>
        public Image ImageElement
        {
            get { return _image; }
        }

        /// <summary>
        /// Get or set the position of the sprite (in game units)
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                Thickness margin = _image.Margin;
                Vector2 canvasPosition = SpriteManager.Instance.VectorToCanvas(_position - _size * 0.5);
                margin.Left = canvasPosition.x;
                margin.Top = canvasPosition.y;
                _image.Margin = margin;
            }
        }

        /// <summary>
        /// Get or set the size of the sprite (in game units)
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return _size;
            }
            set
            {
                Vector2 scale = Vector2.One;
                _size = value;
                if (_size.x < 0)
                {
                    _size.x = -_size.x;
                    scale.x = -1;
                }
                if (_size.y < 0) {
                    _size.y = -_size.y;
                    scale.y = -1;
                }
                ScaleTransform scaleTransform = new ScaleTransform(scale.x,scale.y);
                _image.RenderTransform = scaleTransform;
                Vector2 canvasSize = SpriteManager.Instance.VectorToCanvas(_size);
                _image.Width = canvasSize.x;
                _image.Height = canvasSize.y;
                Position = _position;
            }
        }


    }
}
