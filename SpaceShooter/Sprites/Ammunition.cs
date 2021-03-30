using Microsoft.Xna.Framework;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    public class Ammunition : Projectile
    {
        public Ammunition(Animation animation,int maxHealth, int damage) : base(animation, maxHealth, damage)
        {}
        public override void Update(GameTime gameTime)
        {
            Rotation += MathHelper.ToRadians(RotationVelocity);
            Position += (Direction * LinearVelocity);

            if(Position.X + Rectangle.Width < 0)
                IsRemoved = true;
        }
        public override bool Intersects(Sprite sprite)
        {
            if (sprite != SpriteManager.Instance.Player) return false;

            else return base.Intersects(sprite);
        }

        public override void OnColide(Sprite sprite)
        {
            //if(sprite == SpriteManager.Instance.Player) IsRemoved = true;            
        }
    }
}
