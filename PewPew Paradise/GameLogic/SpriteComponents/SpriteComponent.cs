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

        public virtual void Enabled()
        {

        }
        public virtual void Disabled()
        {

        }


        public virtual void Start()
        {

        }
        public virtual void Update()
        {

        }

        

        /// <summary>
        /// Creates a new SpriteComponent for parent Sprite. [Do not use]
        /// </summary>
        /// <param name="parent"></param>
        public SpriteComponent(Sprite parent)
        {
            sprite = parent;
        }

    }
}
