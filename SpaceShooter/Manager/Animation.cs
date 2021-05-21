using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Manager
{
    /// <summary>
    /// Animation class will split an atlas type image into separate 
    /// frames and iterate display them one at a time.
    /// </summary>
    public class Animation
    {

        public Texture2D Texture;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsLooping { get; set; }
        public Rectangle SourceRectangle { get; private set; }
        public float TimeBetweenFrames { get; set; } = 0;

        private int currentFrame;
        private int totalFrames;
        private float timer;

        public int CurrentFrame
        {
            set { currentFrame = value; }
            get
            {
                return currentFrame;
            }
        }
        /// <summary>
        /// Initialize Animation
        /// </summary>
        /// <param name="texture">2D image</param>
        /// <param name="rows">number of frames in the image's height</param>
        /// <param name="columns">number of frames in the image's width</param>
        public Animation(Texture2D texture, int rows, int columns)
        {
            this.Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            SourceRectangle = new Rectangle(0, 0, texture.Width/columns, texture.Height/rows);
        }

        /// <summary>
        /// Update animation.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (IsPlaying)
            { 
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timer >= TimeBetweenFrames)
                {
                    currentFrame++;
                    timer = 0;
                }
            }                

            if (currentFrame == totalFrames)
                currentFrame = 0;

            UpdateFrame();
        }
        /// <summary>
        /// Update animation frame.
        /// </summary>
        private void UpdateFrame()
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            SourceRectangle = new Rectangle(width * column, height * row, width, height);
        }
    }
}
