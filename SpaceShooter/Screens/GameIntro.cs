using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Manager;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShooter.Screens
{
    /// <summary>
    /// Screen required to display game introduction and input player name.
    /// </summary>
    public class GameIntro : Screen
    {
        private string playerName;
        private string question = "Please enter player Name: ";
        private string acceptedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private bool nameOK = false;

        private List<Keys> lastPressedKeys;
        private Texture2D background;
        private Button okButton;
        private Button playButton;
        private SpriteFont font;
        private Vector2 questionPosition;
        private Vector2 namePosition;
        private List<string> tips = new List<string>()
        { 
            "\nWe are under attack!\n" +
            "It is not just the meteor rain, but foregn space shuttles are\n" +
            "trying to enter the atmosphere!\n" +
            "You are the last pilot left to protect the Earth!\n\n" +
            " " +
            "Be careful not to colide with other objects.\n" +
            "You can move in the mouse direction by just pressing 'W' key,\n" +
            "and you can use the left mouse click to lunch rockets.\n" +
            "Do not let anything go past our defence!!\n\n"
        };
        
        public GameIntro(Game1 game, GraphicsDevice graphics, ContentManager content)
            : base(game, graphics,content)
        {
            background = TextureManager.Instance.GetTexture("background-credits").Texture;
            playerName = string.Empty;
            lastPressedKeys = new List<Keys>();
            font = FontManager.Instance.Arial;
            questionPosition = new Vector2(200, Camera.SCREEN_HEIGHT - 100);
            namePosition = new Vector2(800, 600);

            okButton = new Button(TextureManager.Instance.GetTexture("ok").Texture, Color.Green);
            okButton.Position = new Vector2(700, Camera.SCREEN_HEIGHT - 100);
            okButton.Click += OkButton_Click;

            playButton = new Button(TextureManager.Instance.GetTexture("new game").Texture, Color.Red);
            playButton.Position = new Vector2(Camera.SCREEN_WIDTH/2, Camera.SCREEN_HEIGHT - 100);
            playButton.Width = 150;
            playButton.Click += PlayButton_Click;
        }
        
        private void PlayButton_Click(object sender, EventArgs e)
        {
            _game.ChangeScreen(new GameScreen(_game, _graphicsDevice, _content, playerName));
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (playerName.Length > 0)
                nameOK = true;
        }

        public override void LoadContent()
        { }

        public override void PostUpdate(GameTime gameTime)
        { }
        /// <summary>
        /// Update player input.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            okButton.Position = new Vector2(namePosition.X +100+ font.MeasureString(playerName).X, Camera.SCREEN_HEIGHT - 100);
            List<Keys> pressedKeys = Keyboard.GetState().GetPressedKeys().ToList();

            foreach(Keys key in lastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                    OnKeyUp(key);
            }
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key))
                    OnKeyDown(key);
            }
            lastPressedKeys = pressedKeys;

            okButton.Update(gameTime);
            if(nameOK)
                playButton.Update(gameTime);
        }
        /// <summary>
        /// Draw text on screen.
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="spritebatch"></param>
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            int x = 50;
            int y = 10;

            spritebatch.Begin();

            spritebatch.Draw(background, new Rectangle(0, 0, Camera.SCREEN_WIDTH, Camera.SCREEN_HEIGHT),
                Color.White);
            if (!nameOK)
            {
                spritebatch.DrawString(font, question, questionPosition, Color.White);
                spritebatch.DrawString(font, playerName, namePosition, Color.White);
                
                okButton.Draw(gametime, spritebatch);
            }       
            else
                playButton.Draw(gametime, spritebatch);

            foreach (string tip in tips)
            {
                spritebatch.DrawString(font, tip, new Vector2(x,100), Color.White);
                y += 20;
            }

            spritebatch.End();
        }
        /// <summary>
        /// Add characters to player name.
        /// </summary>
        /// <param name="key"></param>
        private void OnKeyDown(Keys key)
        {
            if(acceptedCharacters.Contains(key.ToString()) && playerName.Length <= 8)
                playerName += key.ToString();

            if (key == Keys.Back && playerName.Length > 0)
                playerName = playerName.Remove(playerName.Length-1);
        }

        private void OnKeyUp(Keys key)
        {
            //do stuff
        }
    }
}
