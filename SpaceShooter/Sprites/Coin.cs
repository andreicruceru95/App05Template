using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    public class Coin : Sprite
    {
        public int Value { get; private set; }
        public Coin(Animation animation, Vector2 position, int value)
        {
            SetAnimation(animation);
            Position = position;
            Rotation = 0;
            RotationVelocity = 2f;            
            Damage = 0;
            Health = 1000;
            Damage = value;

            Animation.TimeBetweenFrames = 0.2f;
            Animation.IsPlaying = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Position.X + Rectangle.Width < 0)
                IsRemoved = true;

            base.Update(gameTime);
        }

        public override void OnColide(Sprite sprite)
        {
            if (sprite == SpriteManager.Instance.Player)
            {
                IsRemoved = true;
                SpriteManager.Instance.Player.Score += Damage;
            }
            if (sprite is Projectile)
                return;
            if (sprite is Asteroid)
                return;
        }
    }
}
