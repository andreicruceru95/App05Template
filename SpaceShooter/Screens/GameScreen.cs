using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SpaceShooter.Manager;
using SpaceShooter.Sprites;
using System.Collections.Generic;

namespace SpaceShooter.Screens

{
   /// <summary>
   /// 
   /// States of the game.
   /// </summary>
    public enum GameStates
    {
        Play, Pause
    }
    /// <summary>
    /// Game screen is required to play the game.
    /// </summary>
    public class GameScreen : Screen
    {

        #region Fields
        private string playerName;
        private Texture2D healthTexture;

        private Vector2 healthPosition;
        private Button reload, points, projectile;
        private Rectangle healthRectangle, healthIconRectangle;

        private string gameOver = "Game Over! Do you want to retry?";
        private bool isOver;
        private GameStates gameState;
        private ScoreManager scoreManager;

        private List<Component> components;

        #endregion

        public GameScreen(Game1 game,GraphicsDevice graphics, ContentManager content, string playerName) 
            :base(game, graphics,content)
        {
            gameState = GameStates.Play;
            this.playerName = playerName;
        }

        #region Initialize
        /// <summary>
        /// Load any content.
        /// </summary>
        public override void LoadContent()
        {
            scoreManager = ScoreManager.Load();

            healthPosition = new Vector2(100, Camera.SCREEN_HEIGHT - 100);
            healthTexture = TextureManager.Instance.GetTexture("Health").Texture;
            healthIconRectangle = new Rectangle((int)healthPosition.X, (int)healthPosition.Y, 50, 50);

            AssignEvents();
        }
        /// <summary>
        /// Create buttons and assign events.
        /// </summary>
        private void AssignEvents()
        {
            var exit = new Button(TextureManager.Instance.GetTexture("Exit").Texture, Color.Red);
            exit.Position = new Vector2(Camera.SCREEN_WIDTH - 1.5f * exit.Rectangle.Width, 10);
            exit.Click += Exit_Click;
            var pause = new Button(TextureManager.Instance.GetTexture("Pause").Texture, Color.MistyRose);
            pause.Position = new Vector2(Camera.SCREEN_WIDTH - 3 * pause.Rectangle.Width, 10);
            pause.Click += Pause_Click;
            var music = new Button(TextureManager.Instance.GetTexture("Music").Texture, Color.MistyRose);
            music.Position = new Vector2(Camera.SCREEN_WIDTH - 4.5f * music.Rectangle.Width, 10);
            music.Click += Music_Click;
            reload = new Button(TextureManager.Instance.GetTexture("Reload").Texture, Color.MistyRose);
            reload.Position = new Vector2(Camera.SCREEN_WIDTH / 2, Camera.SCREEN_HEIGHT / 2 - FontManager.Instance.Arial.MeasureString(gameOver).Y + reload.Rectangle.Height);
            reload.Click += Reload_Click;
            points = new Button(TextureManager.Instance.GetTexture("Points").Texture, Color.Gold);
            points.Position = new Vector2(Camera.SCREEN_WIDTH / 2 - (music.Rectangle.Width / 2 + FontManager.Instance.Arial.MeasureString(SpriteManager.Instance.Player.Score.ToString()).X), 10);
            points.Click += Points_Click;
            projectile = new Button(TextureManager.Instance.GetTexture("bullet1").Texture, Color.MistyRose);
            projectile.Position = new Vector2(0 + reload.Rectangle.Height, 10);
            projectile.Click += Projectile_Click;

            components = new List<Component>()
            {
                exit,
                pause,
                music,
                points
            };
        }
        private void Projectile_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Event Handlers

        private void Points_Click(object sender, System.EventArgs e)
        {
            _game.ChangeScreen(new ScoreScreen(_game, _graphicsDevice, _content, this));
        }

        private void Reload_Click(object sender, System.EventArgs e)
        {
            if (isOver)
            {
                SpriteManager.Instance.AddPlayer();
                isOver = false;
            }
        }
        private void Music_Click(object sender, System.EventArgs e)
        {
            var btn = sender as Button;

            if (MediaPlayer.IsMuted)
            {
                MediaPlayer.IsMuted = false;
                btn.InitialColour = Color.MistyRose;
            }

            else
            {
                MediaPlayer.IsMuted = true;
                btn.InitialColour = Color.DarkRed;
            }
        }
        private void Exit_Click(object sender, System.EventArgs e)
        {
            var score = new Score();

            score.PlayerName = playerName;
            score.Value = SpriteManager.Instance.Player.Score;
            scoreManager.Add(score);

            ScoreManager.Save(scoreManager);

            _game.Exit();
        }
        private void Pause_Click(object sender, System.EventArgs e)
        {
            var btn = sender as Button;

            if (gameState == GameStates.Play)
            {
                gameState = GameStates.Pause;

                btn.Texture = TextureManager.Instance.GetTexture("Play").Texture;
            }
            else
            {
                gameState = GameStates.Play;

                btn.Texture = TextureManager.Instance.GetTexture("Pause").Texture;
            }
        }
        #endregion
        /// <summary>
        /// Update screen objects based on the game state.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (gameState == GameStates.Play && !isOver)
            {
                healthRectangle = new Rectangle((int)healthPosition.X + healthIconRectangle.Width, (int)healthPosition.Y,
                    SpriteManager.Instance.Player.CurrentHealth, healthIconRectangle.Height);
                projectile.Texture = SpriteManager.Instance.Player.Projectile.Animation.Texture;

                if (SpriteManager.Instance.Player.CurrentHealth <= 0 )
                {
                    isOver = true;
                    MediaPlayer.IsMuted = true;
                    SoundManager.Instance.PlayEffect("gameover");
                }
                else
                {
                    isOver = false;
                }      

                SpriteManager.Instance.Update(gameTime);
                Camera.Instance.Update(SpriteManager.Instance.Player);

            }
            foreach (var btn in components)
            {
                btn.Update(gameTime);
            }

            reload.Update(gameTime);

        }
        public override void PostUpdate(GameTime gameTime)
        { }
        /// <summary>
        /// Draw game components.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            string playerHealth = $"{SpriteManager.Instance.Player.CurrentHealth} / {SpriteManager.Instance.Player.MaxHealth}";
            _graphicsDevice.Clear(Color.Black);
            // To activate/deactivate the camera that follows the player, replace the following statements: 
            //_spriteBatch.Begin(transformMatrix: Camera.Instance.Transform);
            spriteBatch.Begin();

            SpriteManager.Instance.Draw(gameTime, spriteBatch);
            

            foreach (var btn in components)
            {
                btn.Draw(gameTime, spriteBatch);
            }
            //draw ammo icon and ammo amount.
            projectile.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(FontManager.Instance.Arial, $"{SpriteManager.Instance.Player.Ammo}",
                new Vector2(2 * projectile.Rectangle.Width, projectile.Rectangle.Height / 2), Color.White);
            spriteBatch.DrawString(FontManager.Instance.Arial, $"Player: {playerName}",
                new Vector2(200, 20), Color.White);
            // draw health icon, rectangle and health amount.
            spriteBatch.Draw(healthTexture, healthIconRectangle, Color.MistyRose);
            spriteBatch.Draw(TextureManager.Instance.GetTexture("healthTexture").Texture, healthRectangle, Color.White);
            spriteBatch.DrawString(FontManager.Instance.Arial, playerHealth,
                new Vector2(Camera.SCREEN_WIDTH / 2 - FontManager.Instance.Arial.MeasureString(playerHealth).X / 2, healthPosition.Y), Color.MistyRose);
            //draw score.
            spriteBatch.DrawString(FontManager.Instance.Arial, $"{SpriteManager.Instance.Player.Score}",
                new Vector2(points.Position.X + points.Rectangle.Width, points.Rectangle.Height / 2), Color.White);

            spriteBatch.DrawString(FontManager.Instance.TimesNewRoman, $"Author : Andrei Cruceru",
                new Vector2(10, Camera.SCREEN_HEIGHT - 20), Color.White);

            if (isOver)
            {
                spriteBatch.DrawString(FontManager.Instance.Arial, gameOver, new Vector2(Camera.SCREEN_WIDTH / 2 - FontManager.Instance.Arial.MeasureString(gameOver).X / 2,
                    Camera.SCREEN_HEIGHT / 2 - FontManager.Instance.Arial.MeasureString(gameOver).Y), Color.White);

                reload.Draw(gameTime, spriteBatch);
            }            

            spriteBatch.End();
        }
    }
}
