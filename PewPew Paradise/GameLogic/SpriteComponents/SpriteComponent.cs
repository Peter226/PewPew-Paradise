using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PewPew_Paradise.GameLogic.SpriteComponents
{
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

        public void Destroy()
        {
            GameManager.OnUpdate -= CallUpdate;
            GameManager.OnPostUpdate -= CallPostUpdate;
            GameManager.OnPreUpdate -= CallPreUpdate;
            
        }

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

        private void CallUpdate()
        {
            if (IsActive && sprite.IsActive)
            {
                Update();
            }
        }
        private void CallPostUpdate()
        {
            if (IsActive && sprite.IsActive)
            {
                PostUpdate();
            }
        }
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
