using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PewPew_Paradise.GameLogic.SpriteComponents
{
    /// <summary>
    /// Base class for attachable sprite components with overrideable methods
    /// </summary>
    public class SpriteComponent
    {
        public Sprite sprite { get; }
        private bool _isActive;

        /// <summary>
        /// Get or set the active state of the component
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set {
                if (_isActive != value) {
                    if (value)
                    {
                        Enabled();
                    }
                    else
                    {
                        Disabled();
                    }
                    _isActive = value;
                }
            }
        }

        /// <summary>
        /// Called when component is enabled 
        /// </summary>
        public virtual void Enabled()
        {

        }
        /// <summary>
        /// Called when component is disabled
        /// </summary>
        public virtual void Disabled()
        {

        }
        /// <summary>
        /// Destoy the component and remove from the sprite
        /// </summary>
        public void Destroy()
        {
            if (sprite.GetComponent(GetType()) != null)
            {
                sprite.RemoveComponent(GetType());
            }
            else
            {
                GameManager.OnUpdate -= CallUpdate;
                GameManager.OnPostUpdate -= CallPostUpdate;
                GameManager.OnPreUpdate -= CallPreUpdate;
            }
        }
        /// <summary>
        /// Called when the sprite the component is on is destroyed
        /// </summary>
        /// <param name="parent"></param>
        protected virtual void OnParentDestroyed(object parent)
        {
            Destroy();
        }

        /// <summary>
        /// Called when component is added
        /// </summary>
        public virtual void Start()
        {

        }


        /// <summary>
        /// Called every frame before Update
        /// </summary>
        public virtual void PreUpdate()
        {

        }

        /// <summary>
        /// Called every frame
        /// </summary>
        public virtual void Update()
        {

        }
        /// <summary>
        /// Called every frame after Update
        /// </summary>
        public virtual void PostUpdate()
        {

        }
        /// <summary>
        /// Used for calling the update function
        /// </summary>
        private void CallUpdate()
        {
            if (IsActive && sprite.IsActive)
            {
                Update();
            }
        }
        /// <summary>
        /// Used for calling the post update function
        /// </summary>
        private void CallPostUpdate()
        {
            if (IsActive && sprite.IsActive)
            {
                PostUpdate();
            }
        }
        /// <summary>
        /// Used for calling the pre update function
        /// </summary>
        private void CallPreUpdate()
        {
            if (IsActive && sprite.IsActive)
            {
                PreUpdate();
            }
        }

        /// <summary>
        /// Creates a new SpriteComponent for parent Sprite. [Do not use]
        /// </summary>
        /// <param name="parent"></param>
        public SpriteComponent(Sprite parent)
        {
            sprite = parent;
            IsActive = true;
            GameManager.OnUpdate += CallUpdate;
            GameManager.OnPostUpdate += CallPostUpdate;
            GameManager.OnPreUpdate += CallPreUpdate;
            parent.OnDestroyed += OnParentDestroyed;
        }

    }
}
