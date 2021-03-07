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
    /// Basic sprite object that has it's transform defined in game units. Has a static image
    /// </summary>
    public class Sprite
    {
        private Image _image;
        private Vector2 _position;
        private Vector2 _size;


        /// <summary>
        /// Used by the SpriteManager, do not use this if not necessary
        /// </summary>
        /// <param name="image"></param>
        public Sprite(BitmapImage image)
        {
            _image = new Image();
            _image.Stretch = Stretch.Fill;
            _image.HorizontalAlignment = HorizontalAlignment.Left;
            _image.VerticalAlignment = VerticalAlignment.Top;
            _image.Source = image;
            _image.RenderTransformOrigin = new Point(0.5,0.5);
            SpriteManager.instance.canvas.Children.Add(_image);
        }

        /// <summary>
        /// Get the current image of the sprite
        /// </summary>
        public Image Image
        {
            get { return _image; }
        }


        /// <summary>
        /// Get or set the position of the sprite in game units
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                //We need to update the UIelement position in screen units aswell
                _position = value;
                Thickness margin = _image.Margin;
                Vector2 canvasPosition = SpriteManager.instance.VectorToCanvas(_position - _size * 0.5);
                margin.Left = canvasPosition.x;
                margin.Top = canvasPosition.y;
                _image.Margin = margin;
            }
        }

        /// <summary>
        /// Get or set the size of the sprite in game units
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return _size;
            }
            set
            {
                //We need to update the UIelement size in screen units aswell
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
                Vector2 canvasSize = SpriteManager.instance.VectorToCanvas(_size);
                _image.Width = canvasSize.x;
                _image.Height = canvasSize.y;
                Position = _position;
            }
        }


    }
}
