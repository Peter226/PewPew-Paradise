using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PewPew_Paradise.GameLogic.SpriteComponents
{
    /// <summary>
    /// Component for animating sprites (only animates their textures)
    /// </summary>
    public class AnimatorComponent : SpriteComponent
    {
        //Collection of animations used to animate the sprite
        private AnimationCollection _animationCollection;
        private int _currentAnimation = 0;
        private int _currentKeyFrame = 0;
        private int _lastAnimation = 0;
        private double _animationTime = 0;
        private ImageBrush _brush;
        public delegate void AnimationEndedEvent(AnimatorComponent component, int animationID);
        /// <summary>
        /// Event that is fired when the currently playing animation ends.
        /// </summary>
        public event AnimationEndedEvent OnAnimationEnded;
        private bool _fallBack;

        public AnimatorComponent(Sprite parent) : base(parent)
        {
            _brush = (ImageBrush)sprite.RectangleElement.Fill;
            _brush.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            _brush.Stretch = Stretch.Fill;
        }
        /// <summary>
        /// Set the animation collection of the component
        /// </summary>
        /// <param name="animationCollection">reference string of the collection</param>
        public virtual void SetAnimation(string animationCollection)
        {
            _animationCollection = SpriteManager.GetAnimationCollection(animationCollection);
            _brush.Viewport = new Rect(new Vector2(0.0, 0.0), (Point)Vector2.One);
            _brush.Viewbox = new Rect(Vector2.Zero, (Size)((Vector2.One / _animationCollection.atlasDimensions)));
            _currentAnimation = _animationCollection.fallbackAnimation;
            _lastAnimation = _currentAnimation;
        }
        /// <summary>
        /// Play animation ignoring priority
        /// </summary>
        /// <param name="animation">animation id/index</param>
        public void ForcePlayAnimation(int animation)
        {
            _currentAnimation = animation;
            if (_lastAnimation != _currentAnimation)
            {
                _animationTime = 0;
                _currentKeyFrame = 0;
                _lastAnimation = _currentAnimation;
                Animate();
            }
        }
        /// <summary>
        /// Play animation
        /// </summary>
        /// <param name="animation">animation id/index</param>
        public void PlayAnimation(int animation)
        {
            if (_animationCollection.animations[animation].priority >= _animationCollection.animations[_currentAnimation].priority)
            {
                _currentAnimation = animation;
                if (_lastAnimation != _currentAnimation)
                {
                    _animationTime = 0;
                    _currentKeyFrame = 0;
                    _lastAnimation = _currentAnimation;
                    Animate();
                }
            }
        }
        public override void PreUpdate()
        {
            base.PreUpdate();
            if (_fallBack) {
                _fallBack = false;
                _currentAnimation = _animationCollection.fallbackAnimation;
                _lastAnimation = _currentAnimation;
            }
        }
        public override void Update()
        {
            base.Update();
        }
        public override void PostUpdate()
        {
            base.Update();
            Animate();
        }
        private Vector2 GetAtlasDisplacement()
        {
            Vector2 src = SpriteManager.CanvasToVector(Vector2.One);
            Vector2 scl = src / sprite.Size.Abs();
            Vector2 dsp = scl / _animationCollection.atlasDimensions;
            return dsp;
        }
        /// <summary>
        /// Animate the sprite
        /// </summary>
        private void Animate()
        {
            if (_animationCollection.animations.Count > 0)
            {
                while (_animationTime > _animationCollection.animations[_currentAnimation].frameTime)
                {
                    _animationTime -= _animationCollection.animations[_currentAnimation].frameTime;
                    _currentKeyFrame++;
                    if (_currentKeyFrame >= _animationCollection.animations[_currentAnimation].keyFrames.Count)
                    {
                        if (!_animationCollection.animations[_currentAnimation].loop)
                        {
                            _fallBack = true;
                            _lastAnimation = _currentAnimation;
                            _animationTime = 0;
                            if (OnAnimationEnded != null)
                            {
                                OnAnimationEnded.Invoke(this, _currentAnimation);
                            }
                        }
                        _currentKeyFrame = 0;
                    }
                }
                Vector2 keyframe = _animationCollection.animations[_currentAnimation].keyFrames[_currentKeyFrame];
                Vector2 disp = GetAtlasDisplacement() * 2;
                _brush.Viewbox = new Rect((keyframe + disp) / _animationCollection.atlasDimensions, (Size)(((Vector2.One - disp * 2) / _animationCollection.atlasDimensions)));
                _animationTime += GameManager.DeltaTime;
            }
        }
    }
}
