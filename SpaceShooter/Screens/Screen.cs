using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Screens
{
    /// <summary>
    /// Screen class sets the rules for creating a screen.
    /// </summary>
    public abstract class Screen
    {
        protected Game1 _game;
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;

        public Screen(Game1 game,GraphicsDevice graphicsDevice, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void PostUpdate(GameTime gameTime);
        public abstract void Draw(GameTime gametime, SpriteBatch spritebatch);
    }
}
