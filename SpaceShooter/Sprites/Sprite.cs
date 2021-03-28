using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;

namespace SpaceShooter.Sprites
{
    /// <summary>
    /// Sprite class represents a dinamic Game object drawn on the screen 
    /// as a two dimensional texture.
    /// </summary>
    public class Sprite : Component, ICloneable
    {
        public Animation Animation { get; set; }
        public Animation Explosion { get; set; }
        public float Rotation { get; set; }

        public List<Sprite> Children { get; set; }
        public Vector2 Position;
        public Vector2 Origin;
        public Vector2 Direction;
        public bool IsExploding;
        public float LifeSpan = 0f;
        public int Health { get; set; }
        public int Damage { get; set; }

        public float RotationVelocity = 3f;
        public float LinearVelocity = 4f;
        
        public Sprite Parent;

        public bool IsRemoved = false;
        public Color[] TextureData;

        public Random rand = new Random();
        public Matrix Transform
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateTranslation(new Vector3(Position, 0));
            }
        }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, Animation.Texture.Width, Animation.Texture.Height);
            }
        }

        /// <summary>
        /// Initialize sprite with a given texture.
        /// </summary>
        /// <param name="texture">2D image.</param>
        public Sprite()
        {
            Children = new List<Sprite>();            
        }

        /// <summary>
        /// Here you can initialize any values for a freshley created sprite object.
        /// </summary>
        public virtual void Initialize()
        {
            //middle of the image
            Origin = new Vector2(Animation.Texture.Width / 2, Animation.Texture.Height / 2);
            Explosion = TextureManager.Instance.GetTexture("explosion2");
            if (Animation != null)
            {
                TextureData = new Color[Animation.Texture.Width * Animation.Texture.Height];
                Animation.Texture.GetData(TextureData);
            }

        }
        public override void Update(GameTime gameTime)
        {
            if (Health <= 0)
                IsRemoved = true;


            Animation.Update(gameTime);
        }

        /// <summary>
        /// Draw sprite on screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Animation.Texture, Position, Animation.SourceRectangle, Color.White,
                Rotation, Origin, 1, SpriteEffects.None, 0);            
        }
        public virtual void OnColide(Sprite sprite)
        {
            Health -= sprite.Damage;
            if(Health < 0)
                Health = 0;
        }

        public virtual void SetAnimation(Animation animation)
        {
            Animation = animation;
            TextureData = new Color[Animation.Texture.Width * Animation.Texture.Height];
            Animation.Texture.GetData(TextureData);
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
        public bool Intersects(Sprite sprite)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            var transformAToB = this.Transform * Matrix.Invert(sprite.Transform);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            var stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            var stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            var yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            for (int yA = 0; yA < this.Rectangle.Height; yA++)
            {
                // Start at the beginning of the row
                var posInB = yPosInB;

                for (int xA = 0; xA < this.Rectangle.Width; xA++)
                {
                    // Round to the nearest pixel
                    var xB = (int)Math.Round(posInB.X);
                    var yB = (int)Math.Round(posInB.Y);

                    if (0 <= xB && xB < sprite.Rectangle.Width &&
                        0 <= yB && yB < sprite.Rectangle.Height)
                    {
                          // Get the colors of the overlapping pixels
                            var colourA = this.TextureData[xA + yA * this.Rectangle.Width];
                            var colourB = sprite.TextureData[xB + yB * sprite.Rectangle.Width];

                            // If both pixel are not completely transparent
                            if (colourA.A != 0 && colourB.A != 0)
                            {
                                return true;
                            }
                        
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }
    }
}
