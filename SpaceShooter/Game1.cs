using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Manager;
using SpaceShooter.Screens;

namespace SpaceShooter
{
    
    /// <summary>
    /// Space Shooting 60fps game that allows the user to control 
    /// the movement of a sprite and shoot projectiles.
    /// </summary>
    public class Game1 : Game
    {
        #region Fields

        private Screen _currentScreen;
        private Screen _nextScreen;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _fixedText;
       
        private Texture2D mouseActive;        

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
            SpriteManager.Instance.Initialize(_graphics.GraphicsDevice);
            SoundManager.Instance.PlayMusic("background1");

            mouseActive = TextureManager.Instance.GetTexture("Mouse Active").Texture;
            Mouse.SetCursor(MouseCursor.FromTexture2D(mouseActive, mouseActive.Width / 2, mouseActive.Height / 2));

            base.Initialize();
        }
        /// <summary>
        /// Load required game content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _fixedText = new SpriteBatch(GraphicsDevice);

            FontManager.Instance.Arial = Content.Load<SpriteFont>("Ariel");
            FontManager.Instance.TimesNewRoman = Content.Load<SpriteFont>("TimesRoman");

            _currentScreen = new MenuScreen(this, _graphics.GraphicsDevice, Content);
        }
        #endregion


        #region Methods
        /// <summary>
        /// Update Game and game objects.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            ///////////////////////////////////////
            if (_nextScreen != null)
            {
                _currentScreen = _nextScreen;
                _currentScreen.LoadContent();
                _nextScreen = null;
            }
            _currentScreen.Update(gameTime);
            _currentScreen.PostUpdate(gameTime);            
            //////////////////////////////////////            

            base.Update(gameTime);
        }        

        /// <summary>
        /// Draw on screen.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _currentScreen.Draw(gameTime,_spriteBatch);
        }
        #endregion

        public void ChangeScreen(Screen screen)
        {
            _nextScreen = screen;
        }
    }
}
