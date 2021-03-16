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
        public float LifeSpan = 0f;

        private float _timer;
        public Projectile() 
        {
            _animation = TextureManager.Instance.GetTexture("bullet1");
            

            base.Initialize();
        }

        /// <summary>
        /// Update projectile.
        /// The projectile is removed when the timer is up.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="sprites"></param>
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;            

            if(_timer > LifeSpan)
            {
                LinearVelocity = 0;
                _animation = _explosion;
                _animation.IsActive = true;

                if(_animation.CurrentFrame == (_animation.Rows * _animation.Columns) -1)
                    IsRemoved = true;
            }

            Position += Direction * LinearVelocity;
            _animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _animation.Draw(spriteBatch, Position);
        }
    }
}
