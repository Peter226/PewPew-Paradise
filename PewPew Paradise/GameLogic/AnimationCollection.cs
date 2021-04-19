using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;

namespace PewPew_Paradise.GameLogic
{
    /// <summary>
    /// Collection of animations for sprite components
    /// </summary>
    public class AnimationCollection
    {
        /// <summary>
        /// Dimensions of the atlas (tiling of the animations, eg. 4x4)
        /// </summary>
        public Vector2 atlasDimensions;
        /// <summary>
        /// Reference name of the collection
        /// </summary>
        public string collectionName;
        /// <summary>
        /// Animation played when no other animations are playing
        /// </summary>
        public int fallbackAnimation;

        public List<SpriteAnimation> animations { get; } = new List<SpriteAnimation>();
        /// <summary>
        /// Create a new animation collection and add it to a reference dictionary
        /// </summary>
        /// <param name="collectionName">name of the collection used for reference</param>
        /// <param name="atlasDimensions">tiling of sprites in the atlas</param>
        /// <param name="fallbackAnimation">animation that plays if no other animation is active</param>
        public AnimationCollection(string collectionName, Vector2 atlasDimensions, int fallbackAnimation = 0)
        {
            this.collectionName = collectionName;
            this.atlasDimensions = atlasDimensions;
            this.fallbackAnimation = fallbackAnimation;
            SpriteManager.AddAnimationCollection(this, collectionName);
        }

        /// <summary>
        /// We load all of our animations here used in the game
        /// </summary>
        public static void LoadAll()
        {
            //PLAYER ANIMATIONS
            AnimationCollection playerAnimations = new AnimationCollection("Player", Vector2.One * 4, 0);
            SpriteAnimation idleAnimation = new SpriteAnimation(150, true);
            playerAnimations.animations.Add(idleAnimation);
            idleAnimation.keyFrames.Add(new Vector2(0, 0));
            idleAnimation.keyFrames.Add(new Vector2(1, 0));
            idleAnimation.keyFrames.Add(new Vector2(2, 0));
            idleAnimation.keyFrames.Add(new Vector2(3, 0));

            SpriteAnimation walkAnimation = new SpriteAnimation(100, false, 2);
            playerAnimations.animations.Add(walkAnimation);
            walkAnimation.keyFrames.Add(new Vector2(0, 1));
            walkAnimation.keyFrames.Add(new Vector2(1, 1));

            SpriteAnimation jumpAnimation = new SpriteAnimation(300, false, 3);
            playerAnimations.animations.Add(jumpAnimation);
            jumpAnimation.keyFrames.Add(new Vector2(2, 1));
            jumpAnimation.keyFrames.Add(new Vector2(3, 1));

            SpriteAnimation fallAnimation = new SpriteAnimation(100, false, 3);
            playerAnimations.animations.Add(fallAnimation);
            fallAnimation.keyFrames.Add(new Vector2(0, 2));

            SpriteAnimation shootingAnimation = new SpriteAnimation(300, false, 4);
            playerAnimations.animations.Add(shootingAnimation);
            shootingAnimation.keyFrames.Add(new Vector2(1, 2));

            SpriteAnimation hurtAnimation = new SpriteAnimation(500, false, 5);
            playerAnimations.animations.Add(hurtAnimation);
            hurtAnimation.keyFrames.Add(new Vector2(3, 2));

            SpriteAnimation deathAnimation = new SpriteAnimation(2000, false, 6);
            playerAnimations.animations.Add(deathAnimation);
            deathAnimation.keyFrames.Add(new Vector2(2, 2));

            SpriteManager.AddAnimationCollection(playerAnimations, "Player");

            //ENEMY ANIMATIONS
            AnimationCollection enemyAnimations = new AnimationCollection("Enemy", new Vector2(4,3), 0);
            SpriteAnimation e_walkAnimation = new SpriteAnimation(150, true);
            enemyAnimations.animations.Add(e_walkAnimation);
            e_walkAnimation.keyFrames.Add(new Vector2(0, 0));
            e_walkAnimation.keyFrames.Add(new Vector2(1, 0));
            e_walkAnimation.keyFrames.Add(new Vector2(2, 0));
            e_walkAnimation.keyFrames.Add(new Vector2(3, 0));

            SpriteAnimation e_jumpAnimation = new SpriteAnimation(300, false, 1);
            enemyAnimations.animations.Add(e_jumpAnimation);
            e_jumpAnimation.keyFrames.Add(new Vector2(2, 1));
            e_jumpAnimation.keyFrames.Add(new Vector2(3, 1));

            SpriteAnimation e_fallAnimation = new SpriteAnimation(200, false, 2);
            enemyAnimations.animations.Add(e_fallAnimation);
            e_fallAnimation.keyFrames.Add(new Vector2(0, 1));
            e_fallAnimation.keyFrames.Add(new Vector2(1, 1));

            SpriteAnimation e_deathAnimation = new SpriteAnimation(100, false, 3);
            enemyAnimations.animations.Add(e_deathAnimation);
            e_deathAnimation.keyFrames.Add(new Vector2(0, 2));
            e_deathAnimation.keyFrames.Add(new Vector2(1, 2));
            e_deathAnimation.keyFrames.Add(new Vector2(2, 2));
            e_deathAnimation.keyFrames.Add(new Vector2(3, 2));

            SpriteManager.AddAnimationCollection(enemyAnimations, "Enemy");
        }
    }
}
