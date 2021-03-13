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
            GameManager.OnUpdate -= this.CallUpdate;
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
        /// Creates a new SpriteComponent for parent Sprite. [Do not use]
        /// </summary>
        /// <param name="parent"></param>
        public SpriteComponent(Sprite parent)
        {
            sprite = parent;
            IsActive = true;
            GameManager.OnUpdate += CallUpdate;
            parent.OnDetroyed += OnParentDestroyed;
        }

    }
}
