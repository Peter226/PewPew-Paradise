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


        public AnimatorComponent(Sprite parent) : base(parent)
        {
            _brush = (ImageBrush)sprite.RectangleElement.Fill;
            _brush.Viewport = new Rect(new Vector2(0.0, 0.0), (Point)new Vector2(4.0, 4.0));
           
        }
        public virtual void SetAnimation(string animationCollection)
        {
            _animationCollection = SpriteManager.GetAnimationCollection(animationCollection);
            _currentAnimation = _animationCollection.fallbackAnimation;
            _lastAnimation = _currentAnimation;
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

        public override void Update()
        {
            base.Update();
            Animate();
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
                            _currentAnimation = _animationCollection.fallbackAnimation;
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
                _brush.Viewport = new Rect(-1 * keyframe, (Point)((Vector2.One) * _animationCollection.atlasDimensions - keyframe));
                _animationTime += GameManager.DeltaTime;
            }
        }


    }
}
