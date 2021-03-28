using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    public class Button : Component
    {
        #region Fields

        private MouseState _currentMouse;

        private bool _isHovering;

        private MouseState _previousMouse;

        private SpriteFont _font;
        public Color InitialColour { get; set; }

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 50, 50);
            }
        }

        public string Text { get; set; }

        #endregion

        #region Methods

        public Texture2D Texture { get; set; }
        public Button(Texture2D texture, Color initialColour)
        {
            Texture = texture;
            this.InitialColour = initialColour;

            PenColour = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = InitialColour;

            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(Texture, Rectangle, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        #endregion
    }
}
