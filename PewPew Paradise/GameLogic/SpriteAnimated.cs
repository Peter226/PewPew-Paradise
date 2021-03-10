using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;
using System.Windows;

namespace PewPew_Paradise.GameLogic
{
    public class SpriteAnimated : Sprite
    {
        private AnimationCollection _animationCollection { get; }

        private int _currentAnimation = 0;
        private int _currentKeyFrame = 0;
        private int _lastAnimation = 0;
        private double _animationTime = 0;

        public delegate void AnimationEndedEvent(Sprite sprite);
        public event AnimationEndedEvent OnAnimationEnded;

        public SpriteAnimated(string image, string animationCollection, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            _brush.Viewport = new Rect(new Vector2(0.0, 0.0),(Point)new Vector2(4.0,4.0));
            _animationCollection = SpriteManager.GetAnimationCollection(animationCollection);
        }
        
        public void PlayAnimation(int animation)
        {
            if (_animationCollection.animations[animation].priority >= _animationCollection.animations[_currentAnimation].priority) {
                _currentAnimation = animation;
                if (_lastAnimation != _currentAnimation) {
                    _animationTime = 0;
                    _currentKeyFrame = 0;
                    _lastAnimation = _currentAnimation;
                    Animate();
                }
            }
        }

        public override void Update()
        {
            Animate();
        }

        private void Animate()
        {
            if (_animationCollection.animations.Count > 0) {
                while (_animationTime > _animationCollection.animations[_currentAnimation].frameTime)
                {
                    _animationTime -= _animationCollection.animations[_currentAnimation].frameTime;
                    _currentKeyFrame++;
                    if (_currentKeyFrame >= _animationCollection.animations[_currentAnimation].keyFrames.Count)
                    {
                        if (!_animationCollection.animations[_currentAnimation].loop) {
                            _currentAnimation = _animationCollection.fallbackAnimation;
                            _animationTime = 0;
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
