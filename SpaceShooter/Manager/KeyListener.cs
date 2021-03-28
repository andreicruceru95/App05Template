using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Manager
{
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
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
