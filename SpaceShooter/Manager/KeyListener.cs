using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Manager
{
    /// <summary>
    /// A way to get player input from keyboard or mouse.
    /// Replaces the traditional keyboard check with event listeners.
    /// </summary>
    public class KeyListener : Component
    {
        #region Fields
        private KeyboardState currentKey;
        private KeyboardState previousKey;
        private Keys key;
        private bool canRepeat;
        #endregion

        public event EventHandler Pressed;

        public KeyListener(Keys key, bool canRepeat)
        {
            this.key = key;
            this.canRepeat = canRepeat;
        }
        #region Methods
        /// <summary>
        /// Update keys
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            previousKey = currentKey;
            currentKey = Keyboard.GetState();

            if(!canRepeat)
            {
                if(currentKey.IsKeyDown(key) && previousKey.IsKeyUp(key))
                {
                    Pressed?.Invoke(this, new EventArgs());
                }
            }
            else if(currentKey.IsKeyDown(key))
            {                
                Pressed?.Invoke(this, new EventArgs());                
            }
        }
        /// <summary>
        /// Draw keys
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
