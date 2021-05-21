using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Sprites
{
    /// <summary>
    /// Component class sits at the very top of the game components hierarchy.
    /// </summary>
    public abstract class Component
    {
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
