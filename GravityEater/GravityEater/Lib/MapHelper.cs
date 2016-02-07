using System;
using Microsoft.Xna.Framework;

namespace GravityEater.Lib
{
    public static class MapHelper
    {
        public const int TileSize = 64;

        public static Vector2 GetTileFromPixels(Vector2 Position)
        {
            return new Vector2(
                (int) (Position.X/TileSize),
                (int) (Position.Y/TileSize));
        }

        public static Vector2 GetTileFromPixels(int x, int y)
        {
            return GetTileFromPixels(new Vector2(x, y));
        }

        public static Vector2 GetPixelsFromTile(Vector2 Position)
        {
            return new Vector2(
                (int) (Position.X*TileSize),
                (int) (Position.Y*TileSize));
        }

        public static Vector2 GetPixelsFromTile(int x, int y)
        {
            return GetPixelsFromTile(new Vector2(x, y));
        }

        public static Vector2 GetPixelsFromTileCenter(Vector2 Position)
        {
            return new Vector2(
                (int) (Position.X*TileSize + TileSize/2),
                (int) (Position.Y*TileSize + TileSize/2));
        }

        public static Vector2 GetPixelsFromTileCenter(int x, int y)
        {
            return GetPixelsFromTileCenter(new Vector2(x, y));
        }

        public static Rectangle GetRectangleForTile(Vector2 tile)
        {
            Vector2 tilePix = GetPixelsFromTile(tile);
            return new Rectangle((int) tilePix.X, (int) tilePix.Y, TileSize, TileSize);
        }

        public static Direction GetDirectionFromVector(Vector2 directionV)
        {
            var directionE = Direction.Down;

            if (directionV.X > 0)
            {
                if (directionV.Y > 0)
                {
                    directionE = Direction.DownRight;
                }
                else if (directionV.Y < 0)
                {
                    directionE = Direction.UpRight;
                }
                else
                    directionE = Direction.Right;
            }
            else if (directionV.X < 0)
            {
                if (directionV.Y > 0)
                {
                    directionE = Direction.DownLeft;
                }
                else if (directionV.Y < 0)
                {
                    directionE = Direction.UpLeft;
                }
                else
                    directionE = Direction.Left;
            }
            else
            {
                if (directionV.Y > 0)
                {
                    directionE = Direction.Down;
                }
                else if (directionV.Y < 0)
                {
                    directionE = Direction.Up;
                }
            }
            return directionE;
        }

        /// <summary>
        ///     Retorna a direção a partir de um angulo. Cuidado com o eixo Y inverso.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Direction GetDirectionFromAngle(float angle)
        {
            var d = Direction.Down;
            if ((angle < 22.5f && angle >= 0) || (angle <= 360f && angle >= 337.5f))
                d = Direction.Right;
            if ((angle >= 22.5f) && (angle < 67.5f))
                d = Direction.DownRight;
            if ((angle >= 67.5f) && (angle < 112.5f))
                d = Direction.Down;
            if ((angle >= 112.5f) && (angle < 157.5f))
                d = Direction.DownLeft;
            if ((angle >= 157.5f) && (angle < 202.5f))
                d = Direction.Left;
            if ((angle >= 202.5f) && (angle < 247.5f))
                d = Direction.UpLeft;
            if ((angle >= 247.5f) && (angle < 292.5f))
                d = Direction.Up;
            if ((angle >= 292.5f) && (angle < 337.5f))
                d = Direction.UpRight;
            return d;
        }

        public static Vector2 GetVectorFromDirection(Direction d)
        {
            Vector2 orientation = Vector2.Zero;
            if (d == Direction.Down)
                orientation = new Vector2(0, 1);
            else if (d == Direction.Up)
                orientation = new Vector2(0, -1);
            else if (d == Direction.Right)
                orientation = new Vector2(1, 0);
            else if (d == Direction.Left)
                orientation = new Vector2(-1, 0);
            else if (d == Direction.UpLeft)
                orientation = new Vector2(-1, -1);
            else if (d == Direction.UpRight)
                orientation = new Vector2(1, -1);
            else if (d == Direction.DownLeft)
                orientation = new Vector2(-1, 1);
            else if (d == Direction.DownRight)
                orientation = new Vector2(1, 1);
            return orientation;
        }

        public static Vector2 GetVectorFromAngle(float angle)
        {
            var vector = new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));

            vector.Normalize();

            return vector;
        }

        public static float GetAngleFromVector(Vector2 direction)
        {
            float angle = -1;
            if (direction == new Vector2(0, 1))
                angle = 90f; //if (d == Direction.Down)
            else if (direction == new Vector2(0, -1))
                angle = 270f; //else if (d == Direction.Up)
            else if (direction == new Vector2(1, 0))
                angle = 0f; //else if (d == Direction.Right)
            else if (direction == new Vector2(-1, 0))
                angle = 180f; //else if (d == Direction.Left)
            else if (direction == new Vector2(-1, -1))
                angle = 225f; //else if (d == Direction.UpLeft)
            else if (direction == new Vector2(1, -1))
                angle = 315f; //else if (d == Direction.UpRight)
            else if (direction == new Vector2(-1, 1))
                angle = 135f; //else if (d == Direction.DownLeft)
            else if (direction == new Vector2(1, 1))
                angle = 45f; //else if (d == Direction.DownRight)


            return angle;
        }

        public static float GetAngleBetweenPoints(GameObject observer, GameObject target)
        {
            Vector2 diff = target.Position - observer.Position;
            float angle = MathHelper.ToDegrees((float) Math.Atan2(diff.Y, diff.X));
            return angle;
        }

        /// <summary>
        ///     Retorna o angulo entre um ponto com orientação (1,0) e um ponto no plano cartesiano.
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float GetAngleBetweenPoints(Vector2 observer, Vector2 target)
        {
            Vector2 diff = target - observer;
            float angle = MathHelper.ToDegrees((float) Math.Atan2(diff.Y, diff.X));

            angle = angle < 0 ? angle + 360 : angle;
            return angle;
        }

        public static Rectangle CreateRectangleForTile(Vector2 tile)
        {
            return new Rectangle(
                (int) tile.X*TileSize,
                (int) tile.Y*TileSize,
                TileSize,
                TileSize);
        }

        public static Point Vector2ToPoint(Vector2 v)
        {
            return new Point((int) Math.Floor(v.X), (int) Math.Floor(v.Y));
        }

        public static Vector2 CheckCellCollision(Vector2 cell, Vector2 playerPosition, float collisionRadius)
        {
            Vector2 cellCenter = GetPixelsFromTileCenter(cell);
            float distance = (playerPosition - cellCenter).Length();

            if (distance < TileSize/2 + collisionRadius)
            {
                Vector2 d = Vector2.Normalize(cellCenter - playerPosition);
                return cellCenter - (d*(collisionRadius + TileSize/2));
            }
            return playerPosition;
        }
    }

}

