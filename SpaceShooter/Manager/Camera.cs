using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Manager
{
    /// <summary>
    /// Camera view of the game using a matrix.
    /// </summary>
    public class Camera
    {
        private static Camera instance;
        public const int SCREEN_HEIGHT = 720;
        public const int SCREEN_WIDTH = 1080;
        
        public Matrix Transform { get; protected set; }

        /// <summary>
        /// Force this class to a singleton. There can only be one camera view (player's perspective)
        /// </summary>
        public static Camera Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Camera();
                }
                return instance;
            }
        }
        /// <summary>
        /// Initialize.
        /// </summary>
        public Camera()
        {           
        }       

        /// <summary>
        /// set the matrix to follow the player.
        /// </summary>
        public void Update(Ship player)
        {
            var position = Matrix.CreateTranslation(
                -player.Position.X - (player.Texture.Width / 2),
                -player.Position.Y - (player.Texture.Height / 2),
                0);
            var offset = Matrix.CreateTranslation(SCREEN_WIDTH / 2,
                SCREEN_HEIGHT / 2, 0);

            Transform = position * offset;
        }
    }
}
