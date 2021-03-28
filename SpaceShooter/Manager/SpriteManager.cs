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
        public List<Texture2D> Asteroids { get; private set; }
        public Scrolling Scrolling1 { get; set; }
        public Scrolling Scrolling2 { get; set; }
        public Random rand;
        public float timer = 0;

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
            rand = new Random();

            Sprites = new List<Sprite>();
            Asteroids = new List<Texture2D>();

            Scrolling1 = new Scrolling(TextureManager.Instance.GetTexture("background1").Texture,
                         new Rectangle(0, 0, Camera.SCREEN_WIDTH,
                                       Camera.SCREEN_WIDTH));

            Scrolling2 = new Scrolling(TextureManager.Instance.GetTexture("background2").Texture,
                         new Rectangle(Camera.SCREEN_WIDTH, 0,
                                       Camera.SCREEN_WIDTH,
                                       Camera.SCREEN_HEIGHT));

            AddPlayer();
            SetupAsteroid();
        }
        private void AddCoins(Vector2 position)
        {
            int coinNumber = rand.Next(1, 3);

            Sprites.Add(new Coin(TextureManager.Instance.GetTexture("coin" + coinNumber), position, coinNumber * 100));
        }
        

        /// <summary>
        /// This is a single image sprite that rotates
        /// and move at a constant speed in a fixed direction
        /// </summary>
        private void SetupAsteroid()
        {
            int asteroidNumber = rand.Next(1, TextureManager.Instance.MaxAsteroids);

            Sprites.Add(new Asteroid(TextureManager.Instance.GetTexture("Asteroid" + asteroidNumber)));
        }

        /// <summary>
        /// Initialize player object.
        /// </summary>
        public void AddPlayer()
        {
            Player = new Ship()
            {
                Position = new Vector2(100, 100),
                Health = 1000,
                LifeSpan = int.MaxValue,
                Projectile = new Projectile()
            };
            Sprite ship = new Sprite()
            {                
                Position = new Vector2(700, 400),
                Health = 200,
                LifeSpan = int.MaxValue
            };
            ship.SetAnimation(TextureManager.Instance.GetTexture("green ship"));
            ship.Initialize();

            Sprites.Add(Player);
            Sprites.Add(ship);
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
                sprite.Update(gameTime);            
        }
        protected void PostUpdate(GameTime gameTime)
        {
            // 1. Check collision between all current "Sprites"
            // 2. Add "Children" to the list of "_sprites" and clear
            // 3. Remove all "IsRemoved" sprites

            foreach (var spriteA in Sprites)
            {
                foreach (var spriteB in Sprites)
                {
                    if (spriteA == spriteB)
                        continue;
                    if (spriteA.Animation == spriteA.Explosion ||
                        spriteB.Animation == spriteB.Explosion)
                    {
                        continue;
                    }                    
                    if (spriteA.Rectangle.Intersects(spriteB.Rectangle))
                    {
                        if (spriteA.Intersects(spriteB))
                        {
                            if (spriteB.Parent == spriteA)
                                continue;


                            spriteA.OnColide(spriteB);
                        }
                    }
                }
            }

            int count = Sprites.Count;
            for (int i = 0; i < count; i++)
            {
                foreach (var child in Sprites[i].Children)
                    Sprites.Add(child);

                Sprites[i].Children.Clear();
            }

            for (int i = 0; i < Sprites.Count; i++)
            {
                if (Sprites[i].IsRemoved)
                {
                    if(Sprites[i] is Asteroid)
                    {
                        AddCoins(Sprites[i].Position);
                    }

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
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > 2)
            {
                SetupAsteroid();
                timer = 0;
            }


            UpdateBackground();
            UpdateSprites(gameTime);
            PostUpdate(gameTime);
        }

        /// <summary>
        /// Draw content.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            Scrolling1.Draw(spriteBatch);
            Scrolling2.Draw(spriteBatch);

            foreach (var sprite in Sprites)
            {
                sprite.Draw(gameTime, spriteBatch);
            }
        }
    }
}
