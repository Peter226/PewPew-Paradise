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

        int currentAnimation = 0;
        int currentKeyFrame = 0;
        double animTime = 0;

        public delegate void AnimationEndedEvent(Sprite sprite);
        public event AnimationEndedEvent OnAnimationEnded;

        public SpriteAnimated(string image, string animationCollection, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            _brush.Viewport = new Rect(new Vector2(0.0, 0.0),(Point)new Vector2(4.0,4.0));
            _animationCollection = SpriteManager.GetAnimationCollection(animationCollection);
        }
        
        public void PlayAnimation(int animation)
        {
            currentAnimation = animation;
            animTime = 0;
            currentKeyFrame = 0;
        }

        public override void Update()
        {
            Animate();
        }

        private void Animate()
        {
            if (_animationCollection.animations.Count > 0) {
                while (animTime > _animationCollection.animations[currentAnimation].frameTime)
                {
                    animTime -= _animationCollection.animations[currentAnimation].frameTime;
                    currentKeyFrame++;
                    if (currentKeyFrame >= _animationCollection.animations[currentAnimation].keyFrames.Count)
                    {
                        currentKeyFrame = 0;
                    }
                }
                
                Vector2 keyframe = _animationCollection.animations[currentAnimation].keyFrames[currentKeyFrame];
                _brush.Viewport = new Rect(-1 * keyframe, (Point)((Vector2.One) * _animationCollection.atlasDimensions - keyframe));
                animTime += GameManager.DeltaTime;
            }
        }

    }
}
