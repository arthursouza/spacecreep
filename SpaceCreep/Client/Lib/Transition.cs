using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCreep.Client.Lib
{
    public class Transition
    {
        public float CurrentFrame;
        public bool FadeIn;
        public float Frames;
        public int Interval;
        public Texture2D Texture;
        public int Timer;


        public Transition()
        {
            Frames = 20;
            CurrentFrame = 0;
            Interval = 100;
            FadeIn = false;
        }


        internal void Draw(SpriteBatch spriteBatch)
        {
            var alpha = CurrentFrame / Frames;

            if (FadeIn)
                alpha = 1 - alpha;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Draw(Texture,
                new Rectangle(0, 0, GameConfig.Config.WindowWidth, GameConfig.Config.WindowHeight),
                Color.White * alpha);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            Timer += (int) gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Timer > Interval)
            {
                CurrentFrame++;

                //if (CurrentFrame >= Frames)
                //    Finish.Invoke();
            }
        }
    }
}