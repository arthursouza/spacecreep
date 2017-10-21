using System;
using Microsoft.Xna.Framework;

namespace SpaceCreep.Client.Lib
{
    public static class Camera
    {
        private static float zoom = 1f;
        private static readonly Random random = new Random();
        private static bool shaking;

        private static float shakeMagnitude;

        // The total duration of the current shake
        private static float shakeDuration;

        // A timer that determines how far into our shake we are
        private static float shakeTimer;

        // The shake offset vector
        private static Vector2 shakeOffset;

        private static float Zoom
        {
            get { return zoom; }
            set { zoom = MathHelper.Min(MathHelper.Max(value, .1f), 5f); }
        }

        public static Vector2 Position { get; set; } = Vector2.Zero;

        private static Matrix TransformMatrix => Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0f));

        public static Matrix ScaleMatrix
        {
            get
            {
                Matrix m;
                Matrix.CreateScale(Zoom, out m);
                m = Matrix.Multiply(TransformMatrix, m);
                return m;
            }
        }

        public static void LockToTarget(Vector2 character, int screenWidth, int screenHeight)
        {
            Position = new Vector2(
                (int) character.X - screenWidth / 2,
                (int) character.Y - screenHeight / 2);
        }

        public static void ClampToArea(int width, int height)
        {
            if (Position.X > width)
                Position = new Vector2(width, Position.Y);
            if (Position.Y > height)
                Position = new Vector2(Position.X, height);

            if (Position.X < 0)
                Position = new Vector2(0, Position.Y);
            if (Position.Y < 0)
                Position = new Vector2(Position.X, 0);
        }

        public static void ResetCamera()
        {
            Zoom = 1f;
        }

        private static float NextFloat()
        {
            return (float) random.NextDouble() * 2f - 1f;
        }

        /// <summary>
        ///     Shakes the camera with a specific magnitude and duration.
        /// </summary>
        /// <param name="magnitude">The largest magnitude to apply to the shake.</param>
        /// <param name="duration">The length of time (in seconds) for which the shake should occur.</param>
        public static void Shake(float magnitude, float duration)
        {
            shaking = true;
            shakeMagnitude = magnitude;
            shakeDuration = duration;
            shakeTimer = 0f;
        }

        public static void Update(GameTime gameTime)
        {
            // If we're shaking...
            if (shaking)
            {
                // Move our timer ahead based on the elapsed time
                shakeTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;

                // If we're at the max duration, we're not going to be shaking anymore
                if (shakeTimer >= shakeDuration)
                {
                    shaking = false;
                    shakeTimer = shakeDuration;
                }

                // Compute our progress in a [0, 1] range
                var progress = shakeTimer / shakeDuration;

                // Compute our magnitude based on our maximum value and our progress. This causes
                // the shake to reduce in magnitude as time moves on, giving us a smooth transition
                // back to being stationary. We use progress * progress to have a non-linear fall 
                // off of our magnitude. We could switch that with just progress if we want a linear 
                // fall off.
                var magnitude = shakeMagnitude * (1f - progress * progress);

                // Generate a new offset vector with three random values and our magnitude
                shakeOffset = new Vector2(NextFloat(), NextFloat()) * magnitude;

                // If we're shaking, add our offset to our position and target
                Position += shakeOffset;
            }
        }
    }
}