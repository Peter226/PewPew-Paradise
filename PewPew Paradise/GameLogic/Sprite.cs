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
    class Sprite
    {
        private Image _image;
        private Vector2 _position;



        public Sprite(BitmapImage image)
        {
            _image = new Image();
            _image.Source = image;
        }
        public Image Image
        {
            get { return Image; }
            set { Image = value; }
        }




    }
}
