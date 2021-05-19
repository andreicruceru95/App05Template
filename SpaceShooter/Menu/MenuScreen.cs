using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Manager;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Menu
{
    public class MenuScreen : Screen
    {
        private List<Button> buttons;
        private Texture2D background;
        private Vector2 possition = new Vector2(Camera.SCREEN_WIDTH/2, Camera.SCREEN_HEIGHT/2);
        
        public MenuScreen(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) 
            : base(game,graphicsDevice,content)
        {
            background = TextureManager.Instance.GetTexture("background-menu").Texture;
            var newGameTexture = TextureManager.Instance.GetTexture("new game").Texture;

            var newGameButton = new Button(newGameTexture,
                Color.White);
            newGameButton.Click += NewGameButton_Click;
            newGameButton.Position = possition;

            var optionsButton = new Button(TextureManager.Instance.GetTexture("options").Texture,
                Color.White);
            optionsButton.Click += OptionsButton_Click;
            optionsButton.Position = possition + new Vector2(0, 40);

            var quitButton = new Button(TextureManager.Instance.GetTexture("quit").Texture,
                Color.White);
            quitButton.Click += QuitButton_Click;
            quitButton.Position = possition + new Vector2(0, 80);

            var scoreButton = new Button(TextureManager.Instance.GetTexture("score").Texture,
                Color.White);
            scoreButton.Click += ScoreButton_Click;
            scoreButton.Position = possition + new Vector2(0, 120);

            buttons = new List<Button>()
            {
                newGameButton,
                optionsButton,
                quitButton,
                scoreButton
            };
        }

#region Buttons
        private void ScoreButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }        
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeScreen(new GameScreen(_game, _graphicsDevice, _content));
        }
        #endregion

        public override void LoadContent()
        {
            
        }       

        public override void Update(GameTime gameTime)
        {
            foreach(var button in buttons)
            {
                button.Update(gameTime);
            }
        }
        public override void PostUpdate(GameTime gameTime)
        {
            
        }
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
