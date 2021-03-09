using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise;
using PewPew_Paradise.Maths;
using PewPew_Paradise.GameLogic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PewPew_Paradise.Editor
{
    public class CollisionEditor
    {
        private static Grid _collisionEditor;

        private static Vector2 _startPoint;
        private static Vector2 _endPoint;
        private static List<Sprite> _previewSprites = new List<Sprite>();
        private static bool _isDrawing;

        public static void StartDrawing(object sender, MouseEventArgs e)
        {
            _startPoint = GetPoint(e);
            Sprite previewSprite = new Sprite("CollisionPreview", _startPoint, Vector2.Zero);
            _previewSprites.Add(previewSprite);
            _endPoint = _startPoint;
            _isDrawing = true;
        }
        public static void StopDrawing(object sender, MouseEventArgs e)
        {
            _endPoint = GetPoint(e);
            _previewSprites[_previewSprites.Count - 1].StretchToAbsoluteBounds(_startPoint, _endPoint);
            _isDrawing = false;
        }
        public static void Draw(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                _endPoint = GetPoint(e);
                _previewSprites[_previewSprites.Count - 1].StretchToAbsoluteBounds(_startPoint,_endPoint);
            }
        }

        private static Vector2 GetPoint(MouseEventArgs e)
        {
            return SpriteManager.CanvasToVector(e.GetPosition(_collisionEditor)).RoundToPixels();
        }

        public static void Init(Grid uIElement)
        {
            _collisionEditor = uIElement;
            SpriteManager.LoadImage("Images/Sprites/Utility/collision_preview.png","CollisionPreview");
        }
    }
}
