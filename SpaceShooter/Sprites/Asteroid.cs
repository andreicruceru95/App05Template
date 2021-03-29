using Microsoft.Xna.Framework;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    public class Asteroid : Sprite
    {
        float timer = 0;        

        public Asteroid(Animation animation)
        {
            var rand = new Random();

            Animation = animation;
            Position = new Vector2(Camera.SCREEN_WIDTH, rand.Next(0, Camera.SCREEN_HEIGHT));
            Direction = new Vector2(-1, 0);
            LinearVelocity = 1;
            Rotation = MathHelper.ToRadians(3);
            RotationVelocity = rand.Next(1, 6);
            LifeSpan = 20f;
            Health = 100;
            Damage = Health;
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            Rotation += MathHelper.ToRadians(RotationVelocity);
            Position += (Direction * LinearVelocity) * timer;

            if (Health <= 50)
                Animation = TextureManager.Instance.GetTexture("stone50");
            if(Health < 30)
                Animation = TextureManager.Instance.GetTexture("stone30");           

            if (Position.X + Rectangle.Width <= 0)
                IsRemoved = true;

            base.Update(gameTime);
        }

        public override void OnColide(Sprite sprite)
        {
            if (sprite == SpriteManager.Instance.Player)
                IsRemoved = true;
            if (sprite is Coin)
                return;

              base.OnColide(sprite);
        }
    }
}
