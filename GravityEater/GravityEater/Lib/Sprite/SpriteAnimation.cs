using GravityEater.Lib.Graphics;
using GravityEater.Lib.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityEater.Lib.Sprite
{
    public enum AnimationPosition
    {
        Ground,
        Chest
    }

    /// <summary>
    ///     Define a animação de um Sprite em uma única direção
    /// </summary>
    public class SpriteAnimation
    {
        private readonly int frameCount;
        private readonly float interval;
        private readonly Texture2D sprite;
        private readonly int spriteHeight;
        private readonly int spriteWidth;
        private bool animating;
        private int currentFrame;

        private AnimationDirection direcaoDaAnimacao;
        private GameTime gameTime;
        public bool isToggle;
        private Rectangle sourceRect;
        private Vector2 spriteFormat;
        private float timer;

        public SpriteAnimation(Texture2D sprite, float interval, int frames)
        {
            this.sprite = sprite;
            timer = 0;
            currentFrame = 0;
            this.interval = interval;
            spriteHeight = sprite.Height;
            spriteWidth = sprite.Width/frames;
            sourceRect = new Rectangle(currentFrame*spriteWidth, 0, spriteWidth, spriteHeight);
            Origin = new Vector2(spriteWidth/2, spriteHeight/2);
            
            animating = false;
            frameCount = frames;
            spriteFormat = new Vector2(frames, 1);
            Color = Color.White;
        }

        public SpriteAnimation(Texture2D sprite, Vector2 origin, float interval, int frames)
        {
            this.sprite = sprite;
            timer = 0;
            currentFrame = 0;
            this.interval = interval;
            spriteHeight = sprite.Height;
            spriteWidth = sprite.Width/frames;
            sourceRect = new Rectangle(currentFrame*spriteWidth, 0, spriteWidth, spriteHeight);
            Origin = origin;
            animating = false;
            frameCount = frames;
            spriteFormat = new Vector2(frames, 1);
            Color = Color.White;
        }

        public SpriteAnimation(Texture2D sprite, Vector2 spriteFormat, float animationInterval)
        {
            this.sprite = sprite;
            this.spriteFormat = spriteFormat;
            spriteHeight = (int) (sprite.Height/spriteFormat.Y);
            spriteWidth = (int) (sprite.Width/spriteFormat.X);
            frameCount = (int) (spriteFormat.X*spriteFormat.Y);
            animating = false;
            Origin = new Vector2(spriteWidth/2, spriteHeight/2);
            interval = animationInterval;
            Color = Color.White;
        }

        public AnimationPosition AnimationPosition { get; set; }

        public bool On { get; set; }
        public Vector2 Position { get; set; }

        public bool IsToggle
        {
            get { return isToggle; }
            set { isToggle = value; }
        }

        public Vector2 Origin { get; set; }

        public bool IsAnimating
        {
            get { return animating; }
        }

        public int Width
        {
            get { return spriteWidth; }
        }

        public int Height
        {
            get { return spriteHeight; }
        }

        public Texture2D Texture
        {
            get { return sprite; }
        }

        public Vector2 SpriteFormat
        {
            get { return spriteFormat; }
        }

        public Color Color { get; set; }

        public Character Target { get; set; }

        public float Interval
        {
            get { return interval; }
        }

        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            this.gameTime = gameTime;
            int currentCol = currentFrame%(int) spriteFormat.X;
            int currentLine = currentFrame/(int) spriteFormat.X;
            sourceRect = new Rectangle(currentCol*spriteWidth, currentLine*spriteHeight, spriteWidth, spriteHeight);
            Animate();
        }

        public void Draw(SpriteBatch batch, Vector2 position, float alpha, float rotation = 0)
        {
            //batch.Draw(sprite, position, sourceRect, Color*alpha, rotation, Origin, 1.0f, SpriteEffects.None, 0);
            batch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y, spriteWidth, spriteHeight), sourceRect, Color, 0f, Origin, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch batch)
        {
            if (Target != null)
            {
                if (AnimationPosition == AnimationPosition.Ground)
                    Position = new Vector2(Target.Position.X, Target.Position.Y);
                else
                    Position = new Vector2(Target.Position.X, Target.Position.Y - Target.CharSprite.Height/2);
            }

            batch.Draw(sprite, new Rectangle((int)Position.X, (int)Position.Y, sprite.Width * 3, sprite.Height *3), sourceRect, Color, 0f, Origin, SpriteEffects.None, 0);
        }

        public void Animate()
        {
            animating = true;

            timer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                if (!isToggle)
                {
                    if (currentFrame == frameCount - 1)
                    {
                        currentFrame = 0;
                        animating = false;
                    }
                    else
                        currentFrame++;
                }
                else
                {
                    if (direcaoDaAnimacao == AnimationDirection.Normal)
                    {
                        if (currentFrame == frameCount - 1)
                        {
                            On = true;
                            animating = false;
                        }
                        else
                            currentFrame++;
                    }
                    else if (direcaoDaAnimacao == AnimationDirection.Inversa)
                        if (currentFrame == 0)
                        {
                            On = false;
                            animating = false;
                        }
                        else
                            currentFrame--;
                }
                timer = 0f;
            }
        }

        public void Start()
        {
            animating = true;
            if (isToggle)
            {
                if (currentFrame == 0)
                {
                    direcaoDaAnimacao = AnimationDirection.Normal;
                }
                else if (currentFrame == frameCount - 1)
                {
                    direcaoDaAnimacao = AnimationDirection.Inversa;
                }
            }
        }

        private enum AnimationDirection
        {
            Normal,
            Inversa
        };
    }
}