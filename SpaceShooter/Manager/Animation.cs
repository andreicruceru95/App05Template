using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Manager
{
    /// <summary>
    /// Animation class will split an atlas type image into separate 
    /// frames and iterate display them one at a time.
    /// </summary>
    public class Animation
    {
        public Texture2D Texture;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public bool IsActive { get; set; } = false;
        public Rectangle SourceRectangle { get; private set; }

        private int currentFrame;
        private int totalFrames;

        public int CurrentFrame
        {
            get
            {
                return currentFrame;
            }
        }
        /// <summary>
        /// Initialize Animation
        /// </summary>
        /// <param name="texture">2D image</param>
        /// <param name="rows">number of frames in the image's height</param>
        /// <param name="columns">number of frames in the image's width</param>
        public Animation(Texture2D texture, int rows, int columns)
        {
            this.Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            SourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        /// <summary>
        /// Update frames
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (IsActive)
                currentFrame++;

            if (currentFrame == totalFrames)
                currentFrame = 0;
        }
        /// <summary>
        /// Draw image.
        /// Create a frame as a rectangle and draw it instead of the full image.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="location"></param>
        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;


            SourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(Texture, destinationRectangle, SourceRectangle, Color.White);
        }
    }
}
