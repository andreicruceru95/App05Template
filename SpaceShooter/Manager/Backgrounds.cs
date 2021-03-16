using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Manager
{
    /// <summary>
    /// Background images.
    /// </summary>
    public class Backgrounds
    {
        public Texture2D Texture;
        public Rectangle Rectangle;
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }

    /// <summary>
    /// Moving background images.
    /// </summary>
    public class Scrolling: Backgrounds
    {
        int moveSpeed = 1;
        public Scrolling(Texture2D texture, Rectangle rectangle)
        {
            Texture = texture;
            Rectangle = rectangle;
        }
        /// <summary>
        /// Update background image.
        /// </summary>
        public void Update()
        {
            Rectangle.X -= moveSpeed;
        }
    }
}
