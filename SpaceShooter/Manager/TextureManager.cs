using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

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

            atlases.Add("bullet1", new Animation(content.Load<Texture2D>("bullet1"), 1, 1));
            atlases.Add("green ship", new Animation(content.Load<Texture2D>("greenShip"), 1, 1));
            atlases.Add("explosion1", new Animation(content.Load<Texture2D>("explosion"), 3, 3));
            atlases.Add("explosion2", new Animation(content.Load<Texture2D>("Images/explosion2"), 1, 8));
            atlases.Add("explosion3", new Animation(content.Load<Texture2D>("Images/explosion3"), 1, 14));
            atlases.Add("background1", new Animation(content.Load<Texture2D>("Backgrounds/bg1"), 1, 1));
            atlases.Add("background2", new Animation(content.Load<Texture2D>("Backgrounds/bg2"), 1, 1));
        }
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
