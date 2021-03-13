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
        private static Sprite _zoomSprite;
        private static Sprite _zoomSpriteBackground;
        private static Sprite _zoomCursorSprite;

        private static bool _isDrawing;
        private static double _zoomAmount = 16;
        private static Vector2 _mousePos;
        private static Vector2 _mouseNonRoundPos;

        private static SolidColorBrush _colliderBrush;
        private static SolidColorBrush _colliderHoverBrush;
        private static SolidColorBrush _cursorBrush;

        public static void StartDrawing(object sender, MouseEventArgs e)
        {
            _startPoint = GetPoint(e);
            Sprite previewSprite = new Sprite("MrPlaceHolder", _startPoint, Vector2.Zero);
            previewSprite.RectangleElement.Fill = _colliderBrush;
            _previewSprites.Add(previewSprite);
            _endPoint = _startPoint;
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                _endPoint = (_startPoint * 2).Ceil() * 0.5;
                _startPoint = (_startPoint * 2).Floor() * 0.5;
                previewSprite.StretchToBounds(_endPoint,_startPoint);
            }
            _isDrawing = true;
        }
        public static void StopDrawing(object sender, MouseEventArgs e)
        {
            _endPoint = GetPoint(e);
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                _endPoint = (_endPoint * 2).Ceil() * 0.5;
            }
            _previewSprites[_previewSprites.Count - 1].StretchToAbsoluteBounds(_startPoint.RoundToPixels(), _endPoint.RoundToPixels());
            _isDrawing = false;

            UpdateMapHitboxes();   
        }

        private static void UpdateMapHitboxes()
        {
            List<Rect> hitboxes = MainWindow.Instance.load.CurrentMap().hitboxes;
            hitboxes.Clear();
            foreach (Sprite sprite in _previewSprites)
            {
                hitboxes.Add(sprite.GetRect());
            }
        }



        public static void Draw(object sender, MouseEventArgs e)
        {
            _mousePos = GetPoint(e);

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                _mousePos = (_mousePos * 2).Ceil() * 0.5;
            }

            _mouseNonRoundPos = GetPointNonRounded(e);
            if (_isDrawing)
            {
                _endPoint = _mousePos;
                _previewSprites[_previewSprites.Count - 1].StretchToAbsoluteBounds(_startPoint.RoundToPixels(), _endPoint.RoundToPixels());
            }
            PositionZoom();

            bool foundHover = false;
            Rect mouseRect = new Rect(_mousePos, new Size(0.0, 0.0));
            for (int i = _previewSprites.Count - 1; i >= 0; i--)
            {
                _previewSprites[i].RectangleElement.Fill = _colliderBrush;
                if (!foundHover) {
                    if (_previewSprites[i].GetRect().IntersectsWith(mouseRect))
                    {
                        _previewSprites[i].RectangleElement.Fill = _colliderHoverBrush;
                        foundHover = true;
                    }
                }
            }


        }

        private static Vector2 GetPoint(MouseEventArgs e)
        {
            return SpriteManager.CanvasToVector(e.GetPosition(_collisionEditor)).RoundToPixels();
        }
        private static Vector2 GetPointNonRounded(MouseEventArgs e)
        {
            return SpriteManager.CanvasToVector(e.GetPosition(_collisionEditor));
        }

        private static void PositionZoom()
        {
            if (_zoomSprite != null)
            {
                Vector2 displacement = Vector2.One * 3;
                _zoomSprite.Position = _mousePos + displacement;
                Rect rect = _zoomSprite.GetRect();
                if (rect.Right > GameManager.GameUnitSize) {
                    displacement.x *= -1;
                }
                if (rect.Bottom > GameManager.GameUnitSize)
                {
                    displacement.y *= -1;
                }
                _zoomSprite.Position = _mousePos + displacement;
                _zoomSpriteBackground.Position = _zoomSprite.Position;
                _zoomCursorSprite.Position = _zoomSprite.Position;


                ((ImageBrush)_zoomSprite.RectangleElement.Fill).Viewbox = new Rect(_mouseNonRoundPos / GameManager.GameUnitSize - Vector2.One / _zoomAmount * 0.5, (Vector)(Vector2.One / _zoomAmount));
                //((ImageBrush)_zoomSprite.RectangleElement.Fill).Viewport = new Rect(_mousePos, (Vector)(new Vector2(-1,-1) * _zoomAmount));
            }
        }


        private static void BeginZoom(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Z)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    _previewSprites[_previewSprites.Count - 1].Destroy();
                    _previewSprites.RemoveAt(_previewSprites.Count - 1);
                    UpdateMapHitboxes();
                }
            }

            if (e.Key == Key.Delete)
            {
                Rect mouseRect = new Rect(_mousePos,new Size(0.0,0.0));
                for (int i = _previewSprites.Count - 1;i >= 0;i--)
                {
                    if (_previewSprites[i].GetRect().IntersectsWith(mouseRect))
                    {
                        _previewSprites[i].Destroy();
                        _previewSprites.RemoveAt(i);
                        UpdateMapHitboxes();
                        break;
                    }
                }
            }




            if (e.Key == Key.LeftShift)
            {
                if (_zoomSprite == null)
                {
                    _zoomSpriteBackground = new Sprite(MainWindow.Instance.load.CurrentMap().image, Vector2.Zero, Vector2.One * 5);
                    _zoomSpriteBackground.RectangleElement.Fill = MainWindow.Instance.load.CurrentMap().map_color;
                    _zoomSprite = new Sprite(MainWindow.Instance.load.CurrentMap().image, Vector2.Zero, Vector2.One * 5);
                    _zoomCursorSprite = new Sprite(MainWindow.Instance.load.CurrentMap().image, Vector2.Zero, Vector2.One * 0.5);
                    _zoomCursorSprite.RectangleElement.Fill = _cursorBrush;
                    PositionZoom();
                }
            }
        }
        private static void EndZoom(object sender, KeyEventArgs e)
        {
            if (_zoomSprite != null) {
                _zoomSprite.Destroy();
                _zoomSpriteBackground.Destroy();
                _zoomCursorSprite.Destroy();
                _zoomSprite = null;
                _zoomSpriteBackground = null;
                _zoomCursorSprite = null;
            }
        }

        private static void ZoomIn(object state, MouseWheelEventArgs e)
        {
            _zoomAmount += e.Delta * 0.01f;
            _zoomAmount = Math.Min(Math.Max(_zoomAmount,1.0),GameManager.GameResolution * 0.25);
            PositionZoom();
        }

        public static void Init(Grid uIElement)
        {
            MainWindow.Instance.MouseWheel += ZoomIn;
            MainWindow.Instance.KeyDown += BeginZoom;
            MainWindow.Instance.KeyUp += EndZoom;
            MainWindow.Instance.MouseDown += StartDrawing;
            MainWindow.Instance.MouseUp += StopDrawing;
            MainWindow.Instance.MouseMove += Draw;
            _collisionEditor = uIElement;
            _colliderBrush = new SolidColorBrush(Color.FromArgb(180,0,255,0));
            _colliderHoverBrush = new SolidColorBrush(Color.FromArgb(180, 255, 255, 0));
            _cursorBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        }
    }
}
