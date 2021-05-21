using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    /// <summary>
    /// Coin is a colidable object that increases the player's score.
    /// </summary>
    public class Coin : Sprite
    {
        public Coin(Animation animation, Vector2 position, int maxHealth, int damage) : base(maxHealth,damage)
        {
            SetAnimation(animation);
            Position = position;
            Rotation = 0;
            RotationVelocity = 2f;
            Scale = 1.5f;

            Animation.TimeBetweenFrames = 0.5f;
            Animation.IsPlaying = true;
        }
        /// <summary>
        /// Update onject.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Position.X + Rectangle.Width < 0)
                IsRemoved = true;

            base.Update(gameTime);
        }
        /// <summary>
        /// Set collision rules.
        /// </summary>
        /// <param name="sprite"></param>
        public override void OnColide(Sprite sprite)
        {
            if (sprite == SpriteManager.Instance.Player)
            {
                IsRemoved = true;
                SpriteManager.Instance.Player.Score += Damage;
            }

            if(IsRemoved) SoundManager.Instance.PlayEffect("coins");
        }
        /// <summary>
        /// Check intersection with other objects.
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public override bool Intersects(Sprite sprite)
        {
            if (sprite == SpriteManager.Instance.Player) return base.Intersects(sprite);

            else return false;
        }
    }
}
