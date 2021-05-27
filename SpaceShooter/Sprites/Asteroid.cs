using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Manager;
using System;

namespace SpaceShooter.Sprites
{
    /// <summary>
    /// Asteroid is an colidable object that can take damage and damage other objects.
    /// </summary>
    public class Asteroid : Sprite
    {
        private float timer = 0;
        private Rectangle healthRectangle;
        private Texture2D healthTexture;

        public Asteroid(Animation animation, int maxHealth, int damage) : base(maxHealth, damage)
        {
            var rand = new Random();

            Animation = animation;
            Position = new Vector2(Camera.SCREEN_WIDTH, rand.Next(0, Camera.SCREEN_HEIGHT));
            Direction = new Vector2(-1, 0);
            LinearVelocity = 1;
            Rotation = MathHelper.ToRadians(3);
            RotationVelocity = rand.Next(1, 6);
            LifeSpan = 20f;
            CurrentHealth = 100;
            Damage = CurrentHealth;

            healthTexture = TextureManager.Instance.GetTexture("healthTexture").Texture;
            //healthRectangle = new Rectangle((int)Position.X -(CurrentHealth/2), (int)Position.Y, CurrentHealth, 5);

            base.Initialize();
        }
        /// <summary>
        /// Update object.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            healthRectangle = new Rectangle((int)Position.X - (CurrentHealth / 2), (int)Position.Y, CurrentHealth, 5);

            Rotation += MathHelper.ToRadians(RotationVelocity);
            Position += (Direction * LinearVelocity);// * timer;
            timer = 0;

            if (CurrentHealth <= 50)
                Animation = TextureManager.Instance.GetTexture("stone50");
            if(CurrentHealth < 30)
                Animation = TextureManager.Instance.GetTexture("stone30");

            if (Position.X + Rectangle.Width <= 0)
            {
                IsRemoved = true;
                SpriteManager.Instance.SetGameSpeed((float)0.1);
            }

            base.Update(gameTime);
        }
        /// <summary>
        /// Draw object.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);
        }
        /// <summary>
        /// Set collision rules.
        /// </summary>
        /// <param name="sprite"></param>
        public override void OnColide(Sprite sprite) 
        {
            base.OnColide(sprite);

            if (sprite is Ship)
            {
                CurrentHealth = 0;
                IsRemoved = true;
            }

            if (CurrentHealth <= 0 && (sprite.Parent == SpriteManager.Instance.Player || sprite == SpriteManager.Instance.Player))
            {
                SpriteManager.Instance.Player.Score += 10;                
            }

            if(IsRemoved || CurrentHealth <= 0) SoundManager.Instance.PlayEffect("flame");
        }
        /// <summary>
        /// Check intersection.
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public override bool Intersects(Sprite sprite)
        {
            if (sprite.Parent == SpriteManager.Instance.Player || sprite == SpriteManager.Instance.Player) 
                return base.Intersects(sprite);
            else return false;
        }
    }
}
