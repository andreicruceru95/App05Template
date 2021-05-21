using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Manager
{
    /// <summary>
    /// Font manager stores information about the font types across the game.
    /// </summary>
    public class FontManager
    {
        private static FontManager instance;

        public SpriteFont Arial { get; set; }
        public SpriteFont TimesNewRoman { get; set; }

        public static FontManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new FontManager();
                return instance;
            }
        }
    }
}
