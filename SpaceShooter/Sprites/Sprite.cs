using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    /// <summary>
    /// Sprite class represents a dinamic Game object drawn on the screen 
    /// as a two dimensional texture.
    /// </summary>
    public class Sprite : ICloneable
    {
        protected Animation _animation;

        protected Animation _explosion;

        protected float _rotation;       

        public Vector2 Position;
        public Vector2 Origin;
        public Vector2 Direction;

        public float RotationVelocity = 3f;
        public float LinearVelocity = 4f;

        // Parent is necesary to implement methods that
        // forbids a shooting projectile to damage its shooter.
        public Sprite Parent;

        public bool IsRemoved = false;

        /// <summary>
        /// Initialize sprite with a given texture.
        /// </summary>
        /// <param name="texture">2D image.</param>
        public Sprite()
        { }

        /// <summary>
        /// Here you can initialize any values for a freshley created sprite object.
        /// </summary>
        public virtual void Initialize()
        {
            //middle of the image
            Origin = new Vector2(_animation.Texture.Width / 2, _animation.Texture.Height / 2);
            _explosion = TextureManager.Instance.GetTexture("explosion2");
        }
        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _animation.Update(gameTime);
        }

        /// <summary>
        /// Draw sprite on screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_animation.Texture, Position, _animation.SourceRectangle, Color.White,
            //    _rotation, Origin, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(_animation.Texture, Position, null, Color.White, _rotation,
                Origin, 1, SpriteEffects.None, 0); ;
        }

        /// <summary>
        /// Clone a sprite.
        /// A Clone is a copy of an object that doesn't require any extra memory to be created.
        /// </summary>
        /// <returns>A Clone of this object.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
