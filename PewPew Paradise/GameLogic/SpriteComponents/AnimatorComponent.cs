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
    public class AnimatorComponent : SpriteComponent
    {


        private AnimationCollection _animationCollection;

        private int _currentAnimation = 0;
        private int _currentKeyFrame = 0;
        private int _lastAnimation = 0;
        private double _animationTime = 0;
        private ImageBrush _brush;
        public delegate void AnimationEndedEvent(AnimatorComponent component, int animationID);
        public event AnimationEndedEvent OnAnimationEnded;
        bool fallBack;

        public AnimatorComponent(Sprite parent) : base(parent)
        {
            _brush = (ImageBrush)sprite.RectangleElement.Fill;
            _brush.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            _brush.Stretch = Stretch.Fill;
        }

        public virtual void SetAnimation(string animationCollection)
        {
            _animationCollection = SpriteManager.GetAnimationCollection(animationCollection);
            _brush.Viewport = new Rect(new Vector2(0.0, 0.0), (Point)Vector2.One);
            _brush.Viewbox = new Rect(Vector2.Zero, (Size)((Vector2.One / _animationCollection.atlasDimensions)));
            _currentAnimation = _animationCollection.fallbackAnimation;
            _lastAnimation = _currentAnimation;
        }

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
            if (fallBack) {
                fallBack = false;
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
                            fallBack = true;
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
