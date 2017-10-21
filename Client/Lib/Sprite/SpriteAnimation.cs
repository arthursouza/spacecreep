using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCreep.Client.Lib.Objects;

namespace SpaceCreep.Client.Lib.Sprite
{
    /// <summary>
    ///     Define a animação de um Sprite em uma única direção
    /// </summary>
    public class SpriteAnimation
    {
        private readonly int frameCount;

        private int currentFrame;
        private AnimationDirection direcaoDaAnimacao;
        private GameTime gameTime;
        private Rectangle sourceRect;
        private float timer;


        public SpriteAnimation(Texture2D sprite, float interval, int frames)
        {
            Texture = sprite;
            timer = 0;
            currentFrame = 0;
            Interval = interval;
            Height = sprite.Height;
            Width = sprite.Width / frames;
            sourceRect = new Rectangle(currentFrame * Width, 0, Width, Height);
            Origin = new Vector2(Width / 2, Height / 2);

            IsAnimating = false;
            frameCount = frames;
            SpriteFormat = new Vector2(frames, 1);
            Color = Color.White;
        }
        
        public bool On { get; set; }
        public Vector2 Position { get; set; }
        public bool IsToggle { get; set; }
        public Vector2 Origin { get; set; }

        public bool IsAnimating { get; private set; }

        public int Width { get; }

        public int Height { get; }

        public Texture2D Texture { get; }

        public Vector2 SpriteFormat { get; }

        public Color Color { get; set; }
        public Character Target { get; set; }

        public float Interval { get; }

        public void Update(GameTime gameTimeParam)
        {
            gameTime = gameTimeParam;
            var currentCol = currentFrame % (int) SpriteFormat.X;
            var currentLine = currentFrame / (int) SpriteFormat.X;
            sourceRect = new Rectangle(currentCol * Width, currentLine * Height, Width, Height);
            Animate();
        }

        public void Draw(SpriteBatch batch, Vector2 position)
        {
            //batch.Draw(sprite, position, sourceRect, Color*alpha, rotation, Origin, 1.0f, SpriteEffects.None, 0);
            batch.Draw(Texture, new Rectangle((int) position.X, (int) position.Y, Width, Height), sourceRect, Color, 0f,
                Origin, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch batch)
        {
            if (Target != null)
                    Position = new Vector2(Target.Position.X, Target.Position.Y);

            batch.Draw(Texture,
                new Rectangle((int) Position.X, (int) Position.Y, Texture.Width * 3, Texture.Height * 3), sourceRect,
                Color, 0f, Origin, SpriteEffects.None, 0);
        }

        public void Animate()
        {
            IsAnimating = true;

            timer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > Interval)
            {
                if (!IsToggle)
                {
                    if (currentFrame == frameCount - 1)
                    {
                        currentFrame = 0;
                        IsAnimating = false;
                    }
                    else
                    {
                        currentFrame++;
                    }
                }
                else
                {
                    if (direcaoDaAnimacao == AnimationDirection.Normal)
                        if (currentFrame == frameCount - 1)
                        {
                            On = true;
                            IsAnimating = false;
                        }
                        else
                        {
                            currentFrame++;
                        }
                    else if (direcaoDaAnimacao == AnimationDirection.Inversa)
                        if (currentFrame == 0)
                        {
                            On = false;
                            IsAnimating = false;
                        }
                        else
                        {
                            currentFrame--;
                        }
                }
                timer = 0f;
            }
        }

        public void Start()
        {
            IsAnimating = true;
            if (IsToggle)
                if (currentFrame == 0)
                    direcaoDaAnimacao = AnimationDirection.Normal;
                else if (currentFrame == frameCount - 1)
                    direcaoDaAnimacao = AnimationDirection.Inversa;
        }

        private enum AnimationDirection
        {
            Normal,
            Inversa
        }
    }
}