using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Manager;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;

namespace SpaceShooter.Screens
{
    /// <summary>
    /// Menu screen is a map to other screens.
    /// </summary>
    public class MenuScreen : Screen
    {
        private List<Button> buttons;
        private Texture2D background;
        private Vector2 possition = new Vector2(Camera.SCREEN_WIDTH/2 + 350, Camera.SCREEN_HEIGHT/2 - 200);
        
        public MenuScreen(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) 
            : base(game,graphicsDevice,content)
        {
            background = TextureManager.Instance.GetTexture("background-menu").Texture;
            var newGameTexture = TextureManager.Instance.GetTexture("new game").Texture;

            var newGameButton = new Button(newGameTexture,
                Color.Red);
            newGameButton.Click += NewGameButton_Click;
            newGameButton.Position = possition ;                       

            var credits = new Button(TextureManager.Instance.GetTexture("score").Texture,
                Color.Red);
            credits.Click += CreditsButton_Click;
            credits.Position = possition + new Vector2(0, 100);

            var quitButton = new Button(TextureManager.Instance.GetTexture("quit").Texture,
                Color.Red);
            quitButton.Click += QuitButton_Click;
            quitButton.Position = possition + new Vector2(0, 200);

            buttons = new List<Button>()
            {
                newGameButton,               
                credits,
                quitButton
            };
            foreach (Button btn in buttons)
                btn.Width = 200;
        }

#region Buttons
        private void CreditsButton_Click(object sender, EventArgs e)
        {
            _game.ChangeScreen(new CreditsScreen(_game, _graphicsDevice, _content));
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            _game.Exit();            
        }
               
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeScreen(new GameIntro(_game, _graphicsDevice, _content));
            //_game.ChangeScreen(new GameScreen(_game, _graphicsDevice, _content));
        }
        #endregion

        public override void LoadContent()
        { }       
        /// <summary>
        /// Update screen components.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            foreach(var button in buttons)
            {
                button.Update(gameTime);
            }
        }
        public override void PostUpdate(GameTime gameTime)
        { }
        /// <summary>
        /// Draw components on screen.
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="spritebatch"></param>
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Begin();

            spritebatch.Draw(background, Vector2.Zero,new Rectangle(0,0,Camera.SCREEN_WIDTH, Camera.SCREEN_HEIGHT), Color.White);

            foreach (var button in buttons)
                button.Draw(gametime, spritebatch);

            spritebatch.End();
        }
    }
}
