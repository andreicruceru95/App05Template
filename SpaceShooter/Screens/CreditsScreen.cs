using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Screens
{
    /// <summary>
    /// Dynamic text used to display credits..
    /// </summary>
    internal class CreditText
    {
        public string Text;
        public Vector2 Origin;
        public bool IsRemoved = false;
        public int orderLevel;

        private Vector2 direction;
        private Vector2 position;
        private SpriteFont font;
        
        public CreditText(string text)
        {            
            font = FontManager.Instance.Arial;
            var originX = Camera.SCREEN_WIDTH/2 - font.MeasureString(text).X/2;
            Text = text;

            Origin = new Vector2((float)originX, Camera.SCREEN_HEIGHT + orderLevel * 50 + font.MeasureString(text).Y);
            direction = new Vector2(0, -1);
        }
        /// <summary>
        /// Update text.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (!IsRemoved)
                position += direction;

            if(position.Y < - font.MeasureString(Text).Y)
            {
                Text = string.Empty;
                IsRemoved = true;
            }
        }

        public void UpdatePosition()
        {
            var originX = Camera.SCREEN_WIDTH / 2 - font.MeasureString(Text).X / 2;

            position = new Vector2((float)originX, Camera.SCREEN_HEIGHT + orderLevel * 50 + font.MeasureString(Text).Y);
        }

        /// <summary>
        /// Draw text
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if(!IsRemoved)
                spriteBatch.DrawString(font, Text, position, Color.White);
        }
    }
    /// <summary>
    /// This screen is required to display the text credits.
    /// </summary>
    public class CreditsScreen : Screen
    {
        private Texture2D background;
        private List<CreditText> credits;
        public CreditsScreen(Game1 game, GraphicsDevice graphics, ContentManager content)
            : base(game, graphics, content)
        {
            background = TextureManager.Instance.GetTexture("background-credits").Texture;
            
            credits = new List<CreditText>()
            {
                new CreditText("CO453 Module App05 - Space Shooter"),
                new CreditText("by Andrei Cruceru"),
                new CreditText(" "),
                new CreditText("Music by Eric Matyas"),
                new CreditText("www.soundimage.org"),
                new CreditText(" "),
                new CreditText("Sound Effects Unknown Artist"),
                new CreditText("www.gamesupply.itch.io/"),
                new CreditText(" "),
                new CreditText("Images and 2D Assets by Unknown Artist"),
                new CreditText("www.gamesupply.itch.io/"),
                new CreditText(" "),
                new CreditText("Special Thanks to my lecturers"),
                new CreditText("Derek Peacock &"),
                new CreditText("Nicholas Day"),
                new CreditText(" "),
                new CreditText("Thank you for playing!")
            };

            for (int i = 0; i < credits.Count; i++)
            {
                credits[i].orderLevel = i;
                credits[i].UpdatePosition();
            }
        }
        
        public override void LoadContent()
        {}

        public override void PostUpdate(GameTime gameTime)
        {}
        /// <summary>
        /// Update screen;
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var left = 0;
            var keys = Keyboard.GetState().GetPressedKeys();

            foreach (CreditText credit in credits)
            {
                credit.Update(gameTime);

                if (!credit.IsRemoved) left++;
            }

            if (keys.Length > 0 || left == 0)
                _game.ChangeScreen(new MenuScreen(_game, _graphicsDevice, _content));
        }
        /// <summary>
        /// Draw text on screen
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, Camera.SCREEN_WIDTH, Camera.SCREEN_HEIGHT), Color.White);

            foreach (CreditText credit in credits)
                credit.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
