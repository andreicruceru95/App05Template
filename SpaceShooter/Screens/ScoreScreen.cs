using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Manager;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceShooter.Screens
{
    public class ScoreScreen : Screen
    {
        private GameScreen gameScreen;
        private ScoreManager scoreManager;
        private SpriteFont font;
        private Texture2D background;
        private Button backButton;

        public ScoreScreen (Game1 game, GraphicsDevice graphics, ContentManager content, GameScreen gameScreen)
            : base(game, graphics,content)
        {
            this.gameScreen = gameScreen;
            font = FontManager.Instance.Arial;
            background = TextureManager.Instance.GetTexture("background-credits").Texture;

            scoreManager = ScoreManager.Load();

            backButton = new Button(TextureManager.Instance.GetTexture("ok").Texture, Color.White);
            backButton.Position = new Vector2(Camera.SCREEN_WIDTH - 300,100);
            backButton.Click += BackButton_Click;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeScreen(gameScreen);
        }

        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Begin();

            spritebatch.Draw(background, new Rectangle(0, 0, Camera.SCREEN_WIDTH, Camera.SCREEN_HEIGHT),
                Color.White);

            spritebatch.DrawString(font, "Highscores:\n" + string.Join("\n", scoreManager.Highscores.Select(c => c.PlayerName + ": " + c.Value).ToArray()), 
                new Vector2(300, 10), Color.Red);

            backButton.Draw(gametime, spritebatch);

            spritebatch.End();
        }

        public override void LoadContent()
        {
            
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            backButton.Update(gameTime);
        }
    }
}
