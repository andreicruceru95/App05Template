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
        public Projectile Projectile;
        public int Score { get; set; }
        public int Ammo { get; set; }
        public int AmmoToIncrease { get; set; }
        public bool IsControlable { get; set; }

        private float timer = 0;
        private float aiShootTimer = 0;
        private float aiMoveTimer = 0;
        private float aiRotateTimer = 0;
        private int maxAmmo = 999;
        private event EventHandler clickShoot;
        private MouseState currentState;
        private MouseState previousState;
        private List<KeyListener> keys;        

        public Ship(int maxhealth, int damage) : base(maxhealth, damage)
        {
            Animation = TextureManager.Instance.GetTexture("ship1");

            Score = 0;

            LoadKeys();
            clickShoot += Ship_clickShoot;
            base.Initialize();
        }
        #region Keyboard Response
        private void Ship_clickShoot(object sender, EventArgs e)
        {
            Shoot_Pressed(sender,e);
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
            else UpdateAIShip();

            base.Update(gameTime);
        }

        private void UpdateAIShip()
        {
            var playerDirection = Position - SpriteManager.Instance.Player.Position;

            LinearVelocity = 0.5f;

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

        private void UpdatePlayerShip(GameTime gameTime)
        {
            previousState = currentState;
            currentState = Mouse.GetState();
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            var mousePos = new Vector2(currentState.X, currentState.Y);
            Vector2 rotation = Position - mousePos;

            if (timer > 1.0f)
            {
                timer = 0;
                CurrentHealth++;
                Ammo++;
            }
            if (Ammo >= maxAmmo) Ammo = maxAmmo;

            foreach (var key in keys)
                    key.Update(gameTime);

            if (currentState.LeftButton == ButtonState.Pressed &&
                previousState.LeftButton == ButtonState.Released)
                    clickShoot?.Invoke(this, new EventArgs());

            Rotation = (float)Math.Atan2(rotation.Y, rotation.X) + MathHelper.ToRadians(180);
            Direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
        }

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
                    ChangeProjectile(sprite as Ammunition);
                    sprite.IsRemoved = true;
                    return;
                }
            }
            if(sprite == SpriteManager.Instance.Player) // if player colides with any ship
            {
                IsRemoved = true;
                CurrentHealth = 0;
                sprite.CurrentHealth -= CurrentHealth / 2;
                return;
            }
            if (sprite is Asteroid && (sprite != SpriteManager.Instance.Player))
            {
                //LinearVelocity = 0;

                //if (Position.X >= sprite.Position.X) Position.X++;
                //else Position.X--;
                //if (Position.Y >= sprite.Position.Y) Position.Y++;
                //else Position.Y--;

                //LinearVelocity = 4;

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
        public void ChangeProjectile(Ammunition ammunition)
        {
            Projectile.SetAnimation(ammunition.Animation);
            Projectile.Damage = ammunition.Damage;
            Projectile.Origin = ammunition.Origin;
            Ammo += AmmoToIncrease;
            Score += 50;

            SoundManager.Instance.PlayEffect("reload");
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
    }
}
