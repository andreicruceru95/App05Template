using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Manager
{
    /// <summary>
    /// Sprite manager is a singleton responsible with initializing, loading, updating and drawing sprites.
    /// </summary>
    public class SpriteManager
    {
        private static SpriteManager instance;

        public Ship Player { get; private set; }
        public List<Sprite> Sprites { get; private set; }
        public Scrolling Scrolling1 { get; set; }
        public Scrolling Scrolling2 { get; set; }

        public static SpriteManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SpriteManager();

                return instance;
            }
        }
        /// <summary>
        /// Initialize sprites.
        /// </summary>
        public void Initialize()
        {
            Sprites = new List<Sprite>();
            Scrolling1 = new Scrolling(TextureManager.Instance.GetTexture("background1").Texture,
                         new Rectangle(0, 0, Camera.SCREEN_WIDTH,
                                       Camera.SCREEN_WIDTH));

            Scrolling2 = new Scrolling(TextureManager.Instance.GetTexture("background2").Texture,
                         new Rectangle(Camera.SCREEN_WIDTH, 0,
                                       Camera.SCREEN_WIDTH,
                                       Camera.SCREEN_HEIGHT));

            AddPlayer();
            
        }

        /// <summary>
        /// Initialize player object.
        /// </summary>
        private void AddPlayer()
        {
            Player = new Ship()
            {
                Position = new Vector2(100, 100),
                Projectile = new Projectile()
            };

            Sprites.Add(Player);
        }
        /// <summary>
        /// Update background images.
        /// </summary>
        private void UpdateBackground()
        {
            // X is increasing img1
            if (Scrolling1.Rectangle.X + Scrolling1.Texture.Width <= Player.Position.X - 330)
                Scrolling1.Rectangle.X = Scrolling2.Rectangle.X + Scrolling2.Texture.Width;
            // Y is decreasing img1
            if (Scrolling1.Rectangle.Y + Scrolling1.Texture.Height <= Player.Position.Y - 200)
                Scrolling1.Rectangle.Y = Scrolling2.Rectangle.Y + Scrolling2.Texture.Height;

            // X is increasing img2
            if (Scrolling2.Rectangle.X + Scrolling2.Texture.Width <= Player.Position.X - 330)
                Scrolling2.Rectangle.X = Scrolling1.Rectangle.X + Scrolling1.Texture.Width;
            // Y is decreasing img2
            if (Scrolling2.Rectangle.Y + Scrolling2.Texture.Height <= Player.Position.Y - 200)
                Scrolling2.Rectangle.Y = Scrolling1.Rectangle.Y + Scrolling1.Texture.Height;
            // Y is decreasing
            if (Scrolling1.Rectangle.Y >= Player.Position.Y)
            {
                Scrolling1.Rectangle.Y -= 200;
                Scrolling2.Rectangle.Y -= 200;
            }
            // X is decreasing
            if (Scrolling1.Rectangle.X >= Player.Position.X - 200)
            {
                Scrolling1.Rectangle.X -= 200;
                Scrolling2.Rectangle.X -= 200;
            }
            
            Scrolling1.Update();
            Scrolling2.Update();
        }
        /// <summary>
        /// Update sprite list.
        /// </summary>
        private void UpdateSprites(GameTime gameTime)
        {
            foreach (var sprite in Sprites.ToArray())
                sprite.Update(gameTime, Sprites);

            for (int i = 0; i < Sprites.Count; i++)
            {
                if (Sprites[i].IsRemoved)
                {
                    Sprites.RemoveAt(i);
                    i--;
                }
            }
        }
        /// <summary>
        /// Load Content.
        /// </summary>
        public void LoadContent()
        { }
        /// <summary>
        /// Update content.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            UpdateBackground();
            UpdateSprites(gameTime);
        }

        /// <summary>
        /// Draw content.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Scrolling1.Draw(spriteBatch);
            Scrolling2.Draw(spriteBatch);

            foreach (var sprite in Sprites)
                sprite.Draw(spriteBatch);
        }
    }
}
