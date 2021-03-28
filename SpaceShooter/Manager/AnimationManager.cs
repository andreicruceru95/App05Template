//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace SpaceShooter.Manager
//{
//    public class AnimationManager
//    {
//        private Animation animation;
//        private float timer;

//        public AnimationManager(Animation animation)
//        {
//            this.animation = animation;
//        }

//        public void Play(Animation animation)
//        {
//            if (this.animation == animation)
//                return;

//            this.animation = animation;
//            animation.CurrentFrame = 0;
//            timer = 0;
//        }

//        public void Stop()
//        {
//            timer = 0;
//            animation.CurrentFrame = 0;
//        }

//        public void Update(GameTime gameTime)
//        {
//            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

//            if (timer > animation.FrameSpeed)
//            {
//                animation.CurrentFrame++;

//                if (animation.CurrentFrame > animation.FrameCount)
//                    animation.CurrentFrame = 0;
//            }
//        }

//        public void Draw(SpriteBatch spriteBatch)
//        {
//            //spriteBatch.Draw();
//        }
//    }
//}
