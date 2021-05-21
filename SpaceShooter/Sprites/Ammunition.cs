using Microsoft.Xna.Framework;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    /// <summary>
    /// Ammunition is a projectile type object, spawned on screen.
    /// The player changes the projectile type by coliding with an amunition object.
    /// </summary>
    public class Ammunition : Projectile
    {
        public Ammunition(Animation animation,int maxHealth, int damage) : base(animation, maxHealth, damage)
        {}
        /// <summary>
        /// Update ammunition
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Rotation += MathHelper.ToRadians(RotationVelocity);
            Position += (Direction * LinearVelocity);

            if(Position.X + Rectangle.Width < 0)
                IsRemoved = true;
        }
        /// <summary>
        /// Check intersections of rectangles
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public override bool Intersects(Sprite sprite)
        {
            if (sprite != SpriteManager.Instance.Player) return false;

            else return base.Intersects(sprite);
        }
        /// <summary>
        /// Check colision of objects.
        /// </summary>
        /// <param name="sprite"></param>
        public override void OnColide(Sprite sprite)
        {
            //if(sprite == SpriteManager.Instance.Player) IsRemoved = true;            
        }
    }
}
