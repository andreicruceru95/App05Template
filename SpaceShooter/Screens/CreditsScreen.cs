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

        private Vector2 direction;
        private Vector2 position;
        private SpriteFont font;
        public CreditText(string text, int order)
        {            
            font = FontManager.Instance.Arial;
            var originX = Camera.SCREEN_WIDTH/2 - font.MeasureString(text).X/2;

            Text = text;
            Origin = position = new Vector2((float)originX, Camera.SCREEN_HEIGHT + order * 50 + font.MeasureString(text).Y);
            direction = new Vector2(0, -1);
        }
        /// <summary>
        /// Update text.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (!IsRemoved)
                position += new Vector2(0, -1);

            if(position.Y < - font.MeasureString(Text).Y)
            {
                Text = string.Empty;
                IsRemoved = true;
            }
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
                new CreditText("CO453 Module App05 - Space Shooter",0),
                new CreditText("by Andrei Cruceru",1),
                new CreditText(" ",2),
                new CreditText("Music by Eric Matyas",3),
                new CreditText("www.soundimage.org",4),
                new CreditText(" ",5),
                new CreditText("Sound Effects Unknown Artist",6),
                new CreditText("www.gamesupply.itch.io/",7),
                new CreditText(" ",8),
                new CreditText("Images and 2D Assets by Unknown Artist",9),
                new CreditText("www.gamesupply.itch.io/",10),
                new CreditText(" ",11),
                new CreditText("Special Thanks to my lecturers",12),
                new CreditText("Derek Peacock &",13),
                new CreditText("Nicholas Day",14),
                new CreditText(" ",15),
                new CreditText("Thank you for playing!",16)
            };
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
