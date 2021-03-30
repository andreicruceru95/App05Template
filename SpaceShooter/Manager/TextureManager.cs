using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpaceShooter.Manager
{
    /// <summary>
    /// Texture manager is a singleton responsible with managing texture2D type files.
    /// </summary>
    public class TextureManager
    {
        private static TextureManager instance;

        private ContentManager content;
        private Dictionary<string, Animation> atlases;

        public int MaxAsteroids { get; private set; } = 28;
        public int MaxBullets { get; private set; } = 13;
        public int MaxShips { get; private set; } = 13;

        public static TextureManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TextureManager();

                return instance;
            }
        }
        /// <summary>
        /// Initialize Dictionary.
        /// </summary>
        private void InitializeAtlases()
        {
            atlases = new Dictionary<string, Animation>();

            atlases.Add("green ship", new Animation(content.Load<Texture2D>("Images/greenShip"), 1, 1));
            atlases.Add("explosion1", new Animation(content.Load<Texture2D>("Images/explosion"), 3, 3));
            atlases.Add("explosion2", new Animation(content.Load<Texture2D>("Images/explosion2"), 1, 8));
            atlases.Add("explosion3", new Animation(content.Load<Texture2D>("Images/explosion3"), 1, 14));
            atlases.Add("background1", new Animation(content.Load<Texture2D>("Backgrounds/bg1"), 1, 1));
            atlases.Add("background2", new Animation(content.Load<Texture2D>("Backgrounds/bg2"), 1, 1));
            atlases.Add("stone50", new Animation(content.Load<Texture2D>("Images/Asteroids/stone50"), 1, 1));
            atlases.Add("stone30", new Animation(content.Load<Texture2D>("Images/Asteroids/stone30"), 1, 1));

            LoadGUI();
            LoadCoins();
            LoadAsteroids();
            LoadBullets();
            LoadShips();
        }

        #region Load Textures
        private void LoadShips()
        {
            for (int i = 1; i < MaxShips; i++)
            {
                atlases.Add("ship" + i, new Animation(content.Load<Texture2D>("Images/Ships/ship" + i), 1, 1));
            }
        }
        private void LoadGUI()
        {
            atlases.Add("healthTexture", new Animation(content.Load<Texture2D>("Images/GUI/healthTexture"), 1, 1));
            atlases.Add("Play", new Animation(content.Load<Texture2D>("Images/GUI/Play"), 1, 1));
            atlases.Add("Pause", new Animation(content.Load<Texture2D>("Images/GUI/Pause"), 1, 1));
            atlases.Add("Music", new Animation(content.Load<Texture2D>("Images/GUI/Music"), 1, 1));
            atlases.Add("Points", new Animation(content.Load<Texture2D>("Images/GUI/Points"), 1, 1));
            atlases.Add("Reload", new Animation(content.Load<Texture2D>("Images/GUI/Reload"), 1, 1));
            atlases.Add("Health", new Animation(content.Load<Texture2D>("Images/GUI/Health"), 1, 1));
            atlases.Add("Exit", new Animation(content.Load<Texture2D>("Images/GUI/Exit"), 1, 1));
            atlases.Add("Mouse", new Animation(content.Load<Texture2D>("Images/GUI/mouse"), 1, 1));
            atlases.Add("Mouse Active", new Animation(content.Load<Texture2D>("Images/GUI/mouse_active"), 1, 1));
        }

        private void LoadCoins()
        {
            atlases.Add("coin3", new Animation(content.Load<Texture2D>("Images/Coins/coin_gold"), 1, 8));
            atlases.Add("coin2", new Animation(content.Load<Texture2D>("Images/Coins/coin_silver"), 1, 8));
            atlases.Add("coin1", new Animation(content.Load<Texture2D>("Images/Coins/coin_copper"), 1, 8));
        }

        private void LoadBullets()
        {
            for (int i = 1; i < MaxBullets; i++)
            {
                atlases.Add("bullet" + i, new Animation(content.Load<Texture2D>("Images/Projectiles/bullet" + i), 1, 1));
            }
        }

        private void LoadAsteroids()
        {
            for (int i = 1; i < MaxAsteroids; i++)
            {
                atlases.Add("Asteroid" + i, new Animation(content.Load<Texture2D>("Images/Asteroids/Stones2Filled_" + i), 1, 1));
            }
        }
        #endregion
                
        /// <summary>
        /// Initialize singleton.
        /// </summary>
        public void Initialize()
        {
            content = ContentLoader.Instance.Content;

            InitializeAtlases();
        }
        /// <summary>
        /// Get a Texture atlas type object.
        /// </summary>
        /// <param name="texture">name of texture atlas in the dictionary</param>
        /// <returns>texture atlas</returns>
        public Animation GetTexture(string texture)
        {
            return atlases[texture];
        }
    }
}