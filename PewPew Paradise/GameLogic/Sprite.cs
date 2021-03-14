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
using PewPew_Paradise.GameLogic.SpriteComponents;
namespace PewPew_Paradise.GameLogic
{
    /// <summary>
    /// Basic Sprite with an image
    /// </summary>
    public class Sprite
    {

        public string image { get; }
        protected Rectangle _image { get; }
        protected ImageBrush _brush { get; }
        private Vector2 _position;
        private Vector2 _size;
        private bool _active;
        private int _id;

        //List of components that have been added to the spirte
        private List<SpriteComponent> _components = new List<SpriteComponent>();

        public delegate void OnDestroyDelegate(object state);
        public event OnDestroyDelegate OnDetroyed;

        /// <summary>
        /// Get the current components added
        /// </summary>
        /// <returns></returns>
        public List<SpriteComponent> GetComponents()
        {
            return _components;
        }


        /// <summary>
        /// Add a new SpriteComponent to the Sprite (Only one of each type can be added per Sprite)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddComponent<T>() where T : SpriteComponent
        {

            foreach (SpriteComponent existingComponent in _components)
            {
                if (existingComponent.GetType() == typeof(T))
                {
                    return null;
                }
            }
            T spriteComponent = (T)Activator.CreateInstance(typeof(T), new object[] { this });
            spriteComponent.Start();
            _components.Add(spriteComponent);
            return spriteComponent;
        }

        /// <summary>
        /// Get a SpriteComponent of a Sprite that has already been added
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : SpriteComponent
        {
            foreach (SpriteComponent existingComponent in _components)
            {
                if (existingComponent.GetType() == typeof(T))
                {
                    return (T)existingComponent;
                }
            }
            return null;
        }

        /// <summary>
        /// Remove a certain type of component from the sprite
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveComponent<T>() where T : SpriteComponent
        {
            for (int i = 0;i < _components.Count;i++)
            {
                if (_components[i].GetType() == typeof(T))
                {
                    SpriteComponent spriteComponent = _components[i];
                    _components.RemoveAt(i);
                    return;
                }
            }
        }



        public int ID
        {
            get { return _id; }
        }

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

        private void CallUpdate()
        {
            if (IsActive)
            {
                Update();
            }
        }



        /// <summary>
        /// Destroy sprite
        /// </summary>
        public virtual void Destroy()
        {
            if (OnDetroyed != null) {
                OnDetroyed.Invoke(this);
            }
            GameManager.OnUpdate -= CallUpdate;
            SpriteManager.RemoveSprite(this);
        }

        /// <summary>
        /// Create a new sprite and add it to the screen
        /// </summary>
        /// <param name="image">Name reference defined with SpriteManager.Instance.LoadImage</param>
        /// <param name="position">Position in game units</param>
        /// <param name="size">Size in game units</param>
        public Sprite(string image, Vector2 position, Vector2 size, bool active = true)
        {
            this.image = image;   
            _id = SpriteManager.CreateSpriteID();
            _image = new Rectangle();
            _image.Stretch = Stretch.Fill;
            _image.HorizontalAlignment = HorizontalAlignment.Left;
            _image.VerticalAlignment = VerticalAlignment.Top;

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = SpriteManager.GetImage(image);
            _image.Fill = brush;
            _brush = brush;
            Position = position;
            Size = size;
            IsActive = active;
            SpriteManager.AddSprite(this);
            GameManager.OnUpdate += CallUpdate;
        }

        /// <summary>
        /// The UIElement of the Sprite
        /// </summary>
        public Rectangle RectangleElement
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
                Vector2 canvasPosition = SpriteManager.VectorToCanvas(_position - _size * 0.5);
                margin.Left = canvasPosition.x;
                margin.Top = canvasPosition.y;
                _image.Margin = margin;
            }
        }

        /// <summary>
        /// Get the Rect of the Sprite in game units
        /// </summary>
        /// <returns></returns>
        public Rect GetRect()
        {
            Rect rect = new Rect();
            Vector2 topLeft = Position - Size.Abs() / 2.0;
            rect.X = topLeft.x;
            rect.Y = topLeft.y;
            rect.Width = Size.Abs().x;
            rect.Height = Size.Abs().y;
            return rect;
        }


        /// <summary>
        /// Stretch Sprite to a Rect
        /// </summary>
        /// <param name="rect"></param>
        public void StretchToBounds(Rect rect)
        {
            StretchToBounds(new Vector2(rect.Left,rect.Top),new Vector2(rect.Right,rect.Bottom));
        }
        /// <summary>
        /// Stretch Sprite to a Rect (does not allow negative values)
        /// </summary>
        /// <param name="rect"></param>
        public void StretchToAbsoluteBounds(Rect rect)
        {
            StretchToAbsoluteBounds(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Bottom));
        }

        /// <summary>
        /// Stretch Sprite to a Rect defined by two Vector2s
        /// </summary>
        /// <param name="rect"></param>
        public void StretchToBounds(Vector2 a, Vector2 b)
        {
            Position = ((a + b) / 2.0);
            Size = (b - a);
        }
        /// <summary>
        /// Stretch Sprite to a Rect defined by two Vector2s (does not allow negative values)
        /// </summary>
        /// <param name="rect"></param>
        public void StretchToAbsoluteBounds(Vector2 a, Vector2 b)
        {
            Position = ((a + b) / 2.0);
            Size = (b - a).Abs();
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
                    scale.x = -1;
                }
                if (_size.y < 0) {
                    scale.y = -1;
                }
                ScaleTransform scaleTransform = new ScaleTransform(scale.x,scale.y);
                _image.RenderTransform = scaleTransform;
                Vector2 canvasSize = SpriteManager.VectorToCanvas(_size.Abs());
                _image.Width = canvasSize.x;
                _image.Height = canvasSize.y;
                Position = _position;
            }
        }
    }
}
