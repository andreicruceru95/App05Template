using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceShooter.Manager;
using SpaceShooter.Sprites;
using System.Collections.Generic;

namespace SpaceShooter
{
    public enum GameStates
    {
        Play,Pause
    }
    /// <summary>
    /// Space Shooting 60fps game that allows the user to control 
    /// the movement of a sprite and shoot projectiles.
    /// </summary>
    public class Game1 : Game
    {
        #region Fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _fixedText;
        private SpriteFont arialFont;
        private SpriteFont timesFont;
        private Rectangle healthRectangle;
        private Rectangle healthIconRectangle;
        private Texture2D healthTexture;
        private Vector2 healthPosition;
        private Button reload;


        private string gameOver = "Game Over! Do you want to retry?";
        private bool IsOver;
        private GameStates gameState;

        private List<Component> components;

        #endregion

        #region Initialize Game objects
        /// <summary>
        /// Initialize the graphics and content pipeline.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            gameState = GameStates.Play;
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
            _fixedText = new SpriteBatch(GraphicsDevice);

            arialFont = Content.Load<SpriteFont>("Ariel");
            timesFont = Content.Load<SpriteFont>("TimesRoman");

            healthPosition = new Vector2(100, Camera.SCREEN_HEIGHT - 100);
            healthTexture = TextureManager.Instance.GetTexture("Health").Texture;
            healthIconRectangle = new Rectangle((int)healthPosition.X, (int)healthPosition.Y, 50, 50);

            AssignEvents();
        }

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
            reload.Position = new Vector2(Camera.SCREEN_WIDTH / 2, Camera.SCREEN_HEIGHT / 2 - arialFont.MeasureString(gameOver).Y + reload.Rectangle.Height);
            reload.Click += Reload_Click;
            var points = new Button(TextureManager.Instance.GetTexture("Points").Texture, Color.Gold);
            points.Position = new Vector2(Camera.SCREEN_WIDTH / 2 - (music.Rectangle.Width / 2 + arialFont.MeasureString(SpriteManager.Instance.Player.Score.ToString()).X), 10);
            points.Click += Points_Click;   

            components = new List<Component>()
            {
                exit,
                pause,
                music,
                points,                
            };
        }
        #endregion 

        #region Event Handlers

        private void Points_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Reload_Click(object sender, System.EventArgs e)
        {
            SpriteManager.Instance.AddPlayer();
            IsOver = false;
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
            Exit();
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

        #region Methods
        /// <summary>
        /// Update Game and game objects.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (gameState == GameStates.Play)
            {
                healthRectangle = new Rectangle((int)healthPosition.X + healthIconRectangle.Width, (int)healthPosition.Y,
                    SpriteManager.Instance.Player.Health, healthIconRectangle.Height);

                if (SpriteManager.Instance.Player.Health <= 0)
                    IsOver = true;
                else
                {
                    IsOver = false;
                }

                SpriteManager.Instance.Update(gameTime);
                Camera.Instance.Update(SpriteManager.Instance.Player);
                
            }
            foreach(var btn in components)
            {
                btn.Update(gameTime);
            }

            reload.Update(gameTime);

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

            SpriteManager.Instance.Draw(gameTime,_spriteBatch);   
            _spriteBatch.End();

            _fixedText.Begin();

            foreach(var btn in components)
            {
                btn.Draw(gameTime, _fixedText);
            }
            _fixedText.Draw(healthTexture, healthIconRectangle, Color.MistyRose);
            _fixedText.Draw(TextureManager.Instance.GetTexture("healthTexture").Texture, healthRectangle, Color.White);

            _fixedText.DrawString(arialFont, $"{SpriteManager.Instance.Player.Score}",
                new Vector2(Camera.SCREEN_WIDTH / 2 + 10, 20), Color.White);

            _fixedText.DrawString(arialFont, $"Author : Andrei",
                new Vector2(220, 10), Color.White);

            if(IsOver)
            {
                _fixedText.DrawString(arialFont, gameOver, new Vector2(Camera.SCREEN_WIDTH / 2 - arialFont.MeasureString(gameOver).X/2,
                    Camera.SCREEN_HEIGHT / 2 - arialFont.MeasureString(gameOver).Y), Color.White);

                reload.Draw(gameTime, _fixedText);
            }

            _fixedText.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
