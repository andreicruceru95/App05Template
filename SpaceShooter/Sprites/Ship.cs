using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    /// <summary>
    /// Ship is an representation of a game player.
    /// It allows the user to move and shot projectiles.
    /// </summary>
    public class Ship : Sprite
    {
        public Projectile Projectile;

        private KeyboardState currentKey;
        private KeyboardState previousKey;

        public Texture2D Texture
        {
            get
            {
                return _animation.Texture;
            }
        }
        private SoundEffect laserEffect; 

        public Ship()
        {
            laserEffect = SoundManager.Instance.GetSoundEffect("laser");
            _animation = TextureManager.Instance.GetTexture("green ship");

            base.Initialize();            
        }

        /// <summary>
        /// Update Ship instance by listening and responding to user's input.
        /// </summary>
        /// <param name="gameTime">Time of game.</param>
        /// <param name="sprites">List of sprites.</param>
        public override void Update(GameTime gameTime, List<Sprite> sprites)            
        {
            Direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));
            previousKey = currentKey;
            currentKey = Keyboard.GetState();

            Move();
            Shoot(sprites);

            base.Update(gameTime,sprites);
        }
        /// <summary>
        /// Respond to user movement input.
        /// </summary>
        private void Move()
        {
            
            if (currentKey.IsKeyDown(Keys.A))
                _rotation -= MathHelper.ToRadians(RotationVelocity);

            else if (currentKey.IsKeyDown(Keys.D))
                _rotation += MathHelper.ToRadians(RotationVelocity);           

            if (currentKey.IsKeyDown(Keys.W))
                Position += Direction * LinearVelocity;

            if (LinearVelocity == 0)
                _animation.IsActive = false;

        }
        /// <summary>
        /// Shoot projectiles when user presses space key.
        /// </summary>
        /// <param name="sprites"></param>
        private void Shoot(List<Sprite> sprites)
        {
            // this makes sure that each press of space key will shoot only once.
            if (currentKey.IsKeyDown(Keys.Space) && previousKey.IsKeyUp(Keys.Space))
            {
                AddProjectile(sprites);
                laserEffect.Play(0.5f, 0.5f, 0);
            }
        }

        /// <summary>
        /// Add projectile to list.
        /// Each projectile will have the same direction and origin as the shooting parent.
        /// Each projectile will have twice the speed of it's parent.
        /// </summary>
        /// <param name="sprites">List of sprites</param>
        private void AddProjectile(List<Sprite> sprites)
        {
            var projectile = Projectile.Clone() as Projectile;
            projectile.Direction = this.Direction;
            projectile.Position = this.Position;
            projectile.LinearVelocity = this.LinearVelocity * 2;
            projectile.LifeSpan = 1.5f;
            projectile.Parent = this;

            sprites.Add(projectile);
        }
    }
}
