using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private int PointsToUpgrade = 2000;
        private int ImageIndex = 1;
        private bool IsUpgraded;
        private float messageTimer = 0;
        private float messagePositionY;

        #region Properties
        public Ship Player { get; private set; }
        public List<Sprite> Sprites { get; private set; }
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
        #endregion

        #region Load Content

        /// <summary>
        /// Initialize sprites.
        /// </summary>
        public void Initialize()
        {
            rand = new Random();
            messagePositionY = Camera.SCREEN_HEIGHT / 2;

            Sprites = new List<Sprite>();

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
        public void ChangeProjectile(Ammunition ammunition)
        {
            Player.Projectile.SetAnimation(ammunition.Animation);
            Player.Projectile.Damage = ammunition.Damage;
            Player.Projectile.Origin = ammunition.Origin;
            Player.Ammo += Player.AmmoToIncrease;
            Player.Score += 50;

            SoundManager.Instance.PlayEffect("reload");
        }
        private void AddCoins(Vector2 position)
        {
            int coinNumber = rand.Next(1, 3);
            if(position.X >= 0 && position.Y >= 0 && position.X < Camera.SCREEN_WIDTH && position.Y < Camera.SCREEN_HEIGHT)
                Sprites.Add(new Coin(TextureManager.Instance.GetTexture("coin" + coinNumber), position, 1, coinNumber * 100));
        }
        private void AddAmmo(Asteroid asteroid)
        {
            int ammoNumber = rand.Next(1, TextureManager.Instance.MaxBullets);

            Vector2 position = asteroid.Position;

            if (position.X >= 0 && position.Y >= 0 && position.X < Camera.SCREEN_WIDTH && position.Y < Camera.SCREEN_HEIGHT)
            {
                var ammo = new Ammunition(TextureManager.Instance.GetTexture("bullet" + ammoNumber), 1, ammoNumber * 10)
                {
                    Position = position,
                    Direction = asteroid.Direction,
                    Rotation = asteroid.Rotation,
                    RotationVelocity = 1f,
                    LinearVelocity = asteroid.LinearVelocity
                };
                Sprites.Add(ammo);
            }
        }
        /// <summary>
        /// This is a single image sprite that rotates
        /// and move at a constant speed in a fixed direction
        /// </summary>
        private void SetupAsteroid()
        {
            int asteroidNumber = rand.Next(1, TextureManager.Instance.MaxAsteroids);

            Sprites.Add(new Asteroid(TextureManager.Instance.GetTexture("Asteroid" + asteroidNumber), 200,50));
        }
        /// <summary>
        /// Initialize player object.
        /// </summary>
        public void AddPlayer()
        {
            PointsToUpgrade = 2000;

            Player = new Ship(1000, 0)
            {
                Position = new Vector2(100, 100),
                LifeSpan = int.MaxValue,
                Projectile = new Projectile(TextureManager.Instance.GetTexture("bullet1"), 1, 10),
                Ammo = 100,
                AmmoToIncrease = 15,
            };
            Sprite ship = new Sprite(1000,0)
            {
                Position = new Vector2(700, 400),
                LifeSpan = int.MaxValue
            };
            ship.SetAnimation(TextureManager.Instance.GetTexture("green ship"));
            ship.Initialize();

            Sprites.Add(Player);
            Sprites.Add(ship);
        }
        private void SpawnCoins(Asteroid asteroid)
        {
            int maxChance = 100;
            int chance = 70;

            if (rand.Next(0, maxChance) >= chance)
                AddCoins(asteroid.Position);
        }
        private void SpawnAmmo(Asteroid asteroid)
        {
            int maxChance = 100;
            int chance = 40;

            if (rand.Next(0, maxChance) >= chance)
                AddAmmo(asteroid);
        }
        /// <summary>
        /// Load Content.
        /// </summary>
        public void LoadContent()
        { }
        #endregion

        #region Update Sprites
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
                    //if(spriteA is Ship || spriteA is Projectile)
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
                        SpawnCoins(Sprites[i] as Asteroid);

                        SpawnAmmo(Sprites[i] as Asteroid);
                    }

                    Sprites.RemoveAt(i);
                    i--;
                }
            }
        }
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
            UpgradePlayer();
        }

        private void UpgradePlayer()
        {
            int pointsAdded = 2000;
            Animation lastAnimation = Player.Animation;

            if (Player.Score >= PointsToUpgrade)
            {
                PointsToUpgrade += pointsAdded;
                if (ImageIndex < TextureManager.Instance.MaxShips)
                {
                    ImageIndex++;
                    Animation ship = TextureManager.Instance.GetTexture("ship" + ImageIndex);
                    Player.SetAnimation(ship);
                    Player.Origin = new Vector2(ship.Texture.Width / 2, ship.Texture.Height / 2);
                }
            }
            if (Player.Animation != lastAnimation)
            {
                SoundManager.Instance.PlayEffect("upgrade");
                IsUpgraded = true;
                messageTimer = 0;
            }
        }
        #endregion

        /// <summary>
        /// Draw content.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            messageTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            Scrolling1.Draw(spriteBatch);
            Scrolling2.Draw(spriteBatch);

            foreach (var sprite in Sprites)
            {
                sprite.Draw(gameTime, spriteBatch);
            }

            if(IsUpgraded && messageTimer < 3f)
            {
                string message = "Ship Upgraded!";
                float messagePossitionX = Camera.SCREEN_WIDTH /2 - FontManager.Instance.Arial.MeasureString(message).X/2;

                spriteBatch.DrawString(FontManager.Instance.Arial,message, new Vector2(messagePossitionX, messagePositionY), Color.White) ;

                messagePositionY--;
            }
            else
            {
                IsUpgraded = false;
                messagePositionY = Camera.SCREEN_HEIGHT / 2;
            }
        }
    }
}
