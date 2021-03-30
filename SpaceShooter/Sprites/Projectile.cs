using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    /// <summary>
    /// Projectile is a moving screen object that can be shot by another screen object.
    /// </summary>
    public class Projectile : Sprite
    {
        private float _timer;        
        public Projectile(Animation animation,int maxHealth, int damage) : base(maxHealth, damage) 
        {
            Animation = animation;
            Animation.TimeBetweenFrames = 0.5f;

            base.Initialize();
        }

        /// <summary>
        /// Update projectile.
        /// The projectile is removed when the timer is up.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="sprites"></param>
        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > LifeSpan) IsExploding = true;
            if (IsExploding)
            {
                LinearVelocity = 0;
                Animation = Explosion;
                Animation.IsPlaying = true;
                Scale = 2f;

                if (Animation.CurrentFrame == (Animation.Rows * Animation.Columns) - 1)
                {
                    IsRemoved = true;
                }
            }

            Position += Direction * LinearVelocity;
            Animation.Update(gameTime);
        }
        public override void OnColide(Sprite sprite)
        {
            IsExploding = true;// will trigger explosion.
        }

        public override bool Intersects(Sprite sprite)
        {
            if (Parent == sprite) return false;
            else if (sprite is Asteroid || sprite is Ship) return base.Intersects(sprite);            
            else return false;
        }
    }
}
