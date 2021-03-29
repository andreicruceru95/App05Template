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
        public Projectile() 
        {
            Animation = TextureManager.Instance.GetTexture("bullet1");
            Damage = 50;

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

            if (_timer > LifeSpan)
            {
                LinearVelocity = 0;
                Animation = Explosion;
                Animation.IsPlaying = true;

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
            if (sprite == this.Parent)
                return;
            if (sprite is Projectile)
                return;
            if (sprite is Coin)
                return;

            LifeSpan = 0;
        }
    }
}
