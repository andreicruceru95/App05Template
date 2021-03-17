using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceShooter.Manager;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;

namespace SpaceShooter
{
    /// <summary>
    /// Space Shooting 60fps game that allows the user to control 
    /// the movement of a sprite and shoot projectiles.
    /// </summary>
    public class Game1 : Game
    {
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont font;
        /// <summary>
        /// Initialize the graphics and content pipeline.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;            
        }
        /// <summary>
        /// Initialize Game;
        /// </summary>
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Camera.SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = Camera.SCREEN_HEIGHT;
            _graphics.ApplyChanges();

            ContentLoader.Instance.Initialize(Content);
            SoundManager.Instance.Initialize();
            TextureManager.Instance.Initialize();
            SpriteManager.Instance.Initialize();

            SoundManager.Instance.PlayMusic("background1");

            base.Initialize();            
        }
        /// <summary>
        /// Load required game content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Ariel");
        }

        /// <summary>
        /// Update Game and game objects.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            SpriteManager.Instance.Update(gameTime);
            Camera.Instance.Update(SpriteManager.Instance.Player);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw on screen.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // To activate/deactivate the camera that follows the player, replace the following statements: 
            //_spriteBatch.Begin(transformMatrix: Camera.Instance.Transform);
            _spriteBatch.Begin();

            SpriteManager.Instance.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
