using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;

namespace SpaceShooter.Sprites
{
    /// <summary>
    /// Ship is an representation of a game player.
    /// It allows the user to move and shot projectiles.
    /// </summary>
    public class Ship : Sprite
    {
        public Projectile Projectile;
        public int Score { get; set; }

        private KeyboardState currentKey;
        private KeyboardState previousKey;

        private List<KeyListener> keys;

        public Texture2D Texture
        {
            get
            {
                return Animation.Texture;
            }
        }

        public Ship() : base()
        {
            Animation = TextureManager.Instance.GetTexture("green ship");

            Score = 0;

            LoadKeys();
            base.Initialize();
        }
        private void LoadKeys()
        {
            var left = new KeyListener(Keys.A, true);
            left.Pressed += Left_Pressed;
            var right = new KeyListener(Keys.D, true);
            right.Pressed += Right_Pressed;
            var forward = new KeyListener(Keys.W, true);
            forward.Pressed += Forward_Pressed;
            var shoot = new KeyListener(Keys.Space, false);
            shoot.Pressed += Shoot_Pressed;

            keys = new List<KeyListener>()
            {
                left,
                right,
                forward,
                shoot
            };
        }

        private void Shoot_Pressed(object sender, EventArgs e)
        {
            AddProjectile();
            SoundManager.Instance.PlayEffect("laser");

            Score++;
        }

        private void Forward_Pressed(object sender, EventArgs e)
        {
            Position += Direction * LinearVelocity;
        }

        private void Right_Pressed(object sender, EventArgs e)
        {
            Rotation += MathHelper.ToRadians(RotationVelocity);
        }

        private void Left_Pressed(object sender, EventArgs e)
        {
            Rotation -= MathHelper.ToRadians(RotationVelocity);
        }

        /// <summary>
        /// Update Ship instance by listening and responding to user's input.
        /// </summary>
        /// <param name="gameTime">Time of game.</param>
        public override void Update(GameTime gameTime)            
        {
            foreach (var key in keys)
                key.Update(gameTime);

            Direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            //previousKey = currentKey;
            //currentKey = Keyboard.GetState();

            //Move();
            //Shoot();

            base.Update(gameTime);
        }
        ///// <summary>
        ///// Respond to user movement input.
        ///// </summary>
        //private void Move()
        //{            
        //    if (currentKey.IsKeyDown(Keys.A))
                
        //    else if (currentKey.IsKeyDown(Keys.D))
                

        //    if (currentKey.IsKeyDown(Keys.W))
        //    {
        //        LinearVelocity++;
        //        if (LinearVelocity >= 4)
        //            LinearVelocity = 4;

                
        //    }

        //    if (LinearVelocity == 0)
        //        Animation.IsPlaying = false;

        //}
        /// <summary>
        /// Shoot projectiles when user presses space key.
        /// </summary>
        private void Shoot()
        {
            // this makes sure that each press of space key will shoot only once.
            if (currentKey.IsKeyDown(Keys.Space) && previousKey.IsKeyUp(Keys.Space))
            {
                AddProjectile();
                SoundManager.Instance.PlayEffect("laser");

                Score++;
            }
        }

        public override void OnColide(Sprite sprite)
        {
            if(sprite is Coin)
            {
                Score += sprite.Damage;
                sprite.IsRemoved = true;
            }
            LinearVelocity = 0;

            if (Position.X >= sprite.Position.X)
                Position.X++;
            else
                Position.X--;
            if (Position.Y >= sprite.Position.Y)
                Position.Y++;
            else
                Position.Y--;

            LinearVelocity = 4;            

            base.OnColide(sprite);

            if (sprite is Asteroid)
                sprite.IsRemoved = true;
        }

        /// <summary>
        /// Add projectile to list.
        /// Each projectile will have the same direction and origin as the shooting parent.
        /// Each projectile will have twice the speed of it's parent.
        /// </summary>
        private void AddProjectile()
        {
            var projectile = Projectile.Clone() as Projectile;
            projectile.Direction = this.Direction;
            projectile.Position = this.Position;
            projectile.LinearVelocity = this.LinearVelocity * 2;
            projectile.LifeSpan = 1.5f;
            projectile.Parent = this;

            Children.Add(projectile);
        }
    }
}
