using Microsoft.Xna.Framework;
using SpaceShooter.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    public class Coin : Sprite
    {
        public int Value { get; private set; }
        public Coin(Animation animation, Vector2 position, int value)
        {
            Animation = animation;
            Position = position;
            Rotation = 0;
            Damage = 0;
            Health = 0;
            Damage = value;

            Animation.IsPlaying = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Animation.CurrentFrame == Animation.Rows * Animation.Columns)
                Animation.CurrentFrame = 0;
        }
            
    }
}
