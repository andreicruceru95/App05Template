using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;

namespace SpaceShooter.Sprites
{
    /// <summary>
    /// Ship is an representation of a game player.
    /// It allows the user to move and shot projectiles.
    /// </summary>
    public class Ship : Sprite
    {
        #region Properties
        public Projectile Projectile;
        public int Score { get; set; }
        public int Ammo { get; set; }
        public int AmmoToIncrease { get; set; }
        public bool IsControlable { get; set; }
        #endregion

        #region Fields
        private float timer = 0;
        private float aiShootTimer = 0;
        private float aiMoveTimer = 0;
        private float aiRotateTimer = 0;
        private int maxAmmo = 999;
        private event EventHandler clickShoot;
        private MouseState currentState;
        private MouseState previousState;
        private List<KeyListener> keys;
        #endregion

        #region Base Methods
        public Ship(int maxhealth, int damage) : base(maxhealth, damage)
        {
            Animation = TextureManager.Instance.GetTexture("ship1");

            Score = 0;

            LoadKeys();
            clickShoot += Ship_clickShoot;
            base.Initialize();
        }

        /// <summary>
        /// Update Ship instance by listening and responding to user's input.
        /// </summary>
        /// <param name="gameTime">Time of game.</param>
        public override void Update(GameTime gameTime)
        {
            
            aiMoveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            aiRotateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            aiShootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (IsControlable) UpdatePlayerShip(gameTime);
            else UpdateGameShip();

            base.Update(gameTime);
        }
        /// <summary>
        /// Add projectile to list.
        /// Each projectile will have the same direction and origin as the shooting parent.
        /// Each projectile will have twice the speed of it's parent.
        /// </summary>
        private void AddProjectile()
        {
            var projectile = Projectile.Clone() as Projectile;

            if (IsControlable)
            {
                projectile.LinearVelocity = this.LinearVelocity * 2;
                projectile.LifeSpan = 1.5f;
            }
            else
            {
                projectile.LinearVelocity = 4;
                projectile.LifeSpan = 4f;
            }

            projectile.Direction = this.Direction;
            projectile.Parent = this;
            projectile.Position = this.Position;
            projectile.Rotation = this.Rotation;
            Children.Add(projectile);
        }
#endregion

        #region Keyboard Response
        private void Ship_clickShoot(object sender, EventArgs e)
        {
            Shoot_Pressed(sender, e);
        }

        private void LoadKeys()
        {
            var left = new KeyListener(Keys.A, true);
            left.Pressed += Left_Pressed;
            var right = new KeyListener(Keys.D, true);
            right.Pressed += Right_Pressed;
            var forward = new KeyListener(Keys.W, true);
            forward.Pressed += Forward_Pressed;
            var shoot = new KeyListener(Keys.Space, false);
            shoot.Pressed += Shoot_Pressed;

            keys = new List<KeyListener>()
            {
                left,
                right,
                forward,
                shoot
            };
        }

        private void Shoot_Pressed(object sender, EventArgs e)
        {
            if (Ammo > 0)
            {
                AddProjectile();
                SoundManager.Instance.PlayEffect("laser");

                Score++;
                Ammo--;
            }
            else SoundManager.Instance.PlayEffect("no ammo");
        }

        private void Forward_Pressed(object sender, EventArgs e)
        {
            Position += Direction * LinearVelocity;
        }

        private void Right_Pressed(object sender, EventArgs e)
        {
            Rotation += MathHelper.ToRadians(RotationVelocity);
        }

        private void Left_Pressed(object sender, EventArgs e)
        {
            Rotation -= MathHelper.ToRadians(RotationVelocity);
        }
        #endregion

        #region Player Ship  
        public void UpdateProjectile(Ammunition ammunition)
        {
            Projectile.SetAnimation(ammunition.Animation);
            Projectile.Damage = ammunition.Damage;
            Projectile.Origin = ammunition.Origin;
            Ammo += AmmoToIncrease;
            Score += 50;

            SoundManager.Instance.PlayEffect("reload");
        }
        private void UpdatePlayerShip(GameTime gameTime)
        {
            UpdateStatus(gameTime);
            UpdateKeyboardInput(gameTime);
            UpdateMouseRotation();
            UpdateScreenColision();

            Direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
        }

        private void UpdateKeyboardInput(GameTime gameTime)
        {
            foreach (var key in keys)
                key.Update(gameTime);
        }

        private void UpdateStatus(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > 1.0f)
            {
                timer = 0;
                CurrentHealth++;
                Ammo++;
            }
            if (Ammo >= maxAmmo) Ammo = maxAmmo;
        }

        private void UpdateMouseRotation()
        {
            previousState = currentState;
            currentState = Mouse.GetState();

            var mousePos = new Vector2(currentState.X, currentState.Y);

            Vector2 rotation = Position - mousePos;

            if (currentState.LeftButton == ButtonState.Pressed && 
                    previousState.LeftButton == ButtonState.Released)
                clickShoot?.Invoke(this, new EventArgs());

            if(currentState.X >=0 && currentState.X <= Camera.SCREEN_WIDTH && 
                    currentState.Y >=0 && currentState.Y <= Camera.SCREEN_HEIGHT)
                Rotation = (float)Math.Atan2(rotation.Y, rotation.X) + MathHelper.ToRadians(180);
        }
        #endregion

        #region Game Ship 
        private void UpdateGameShip()
        {
            var playerDirection = Position - SpriteManager.Instance.Player.Position;            

            if (aiMoveTimer > 2f)
            {
                Direction = new Vector2(0, rand.Next(-1, 1));
                aiMoveTimer = 0;
            }
            if (aiRotateTimer > 3f)
            {
                RotationVelocity = rand.Next(0, 1);
                Rotation = MathHelper.ToRadians(RotationVelocity);

                aiRotateTimer = 0;
            }
            if (aiShootTimer > 4f)
            {

                Direction = Vector2.Normalize(SpriteManager.Instance.Player.Position - Position);
                AddProjectile();
                aiShootTimer = 0;
            }

            Position += Direction * LinearVelocity * aiMoveTimer;
            Rotation = (float)Math.Atan2(playerDirection.Y, playerDirection.X) + MathHelper.ToRadians(180);
        }
        #endregion

        #region Collision
        public override void OnColide(Sprite sprite)
        {
            if (this == SpriteManager.Instance.Player)
            {
                if (sprite is Coin)
                {
                    Score += sprite.Damage;
                    return;
                }
                if (sprite is Ammunition)
                {
                    UpdateProjectile(sprite as Ammunition);
                    sprite.IsRemoved = true;
                    return;
                }
            }
            if (sprite == SpriteManager.Instance.Player) // if player colides with any ship
            {
                IsRemoved = true;
                CurrentHealth = 0;
                sprite.CurrentHealth -= CurrentHealth / 2;
                return;
            }
            if (sprite is Asteroid && (this == SpriteManager.Instance.Player))
            {
                CurrentHealth -= sprite.Damage;

                return;
            }
            if (sprite is Asteroid && (this != SpriteManager.Instance.Player))
            {
                return;
            }
            
            base.OnColide(sprite);

            if (CurrentHealth < 0 || IsRemoved) SoundManager.Instance.PlayEffect("flame");

        }
        public override bool Intersects(Sprite sprite)
        {
            if (this == SpriteManager.Instance.Player || (sprite is Projectile && sprite.Parent != this))
                return base.Intersects(sprite);
            else return false;
        }

        public void UpdateScreenColision()
        {
            if(this == SpriteManager.Instance.Player)
            {
                if (Rectangle.X <= 0) Position.X = Animation.Texture.Width/2; //left

                if (Position.X + Rectangle.Width/2 >= Camera.SCREEN_WIDTH) 
                    Position.X = Camera.SCREEN_WIDTH - Animation.Texture.Width/2; //right

                if (Rectangle.Y <= 0) Position.Y = Animation.Texture.Height / 2; ; //top

                if (Position.Y + Rectangle.Height/2 >= Camera.SCREEN_HEIGHT) Position.Y = Camera.SCREEN_HEIGHT - Animation.Texture.Height / 2;//bottom
            }
        }
        #endregion
    }

}
