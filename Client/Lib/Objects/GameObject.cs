﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCreep.Client.Lib.Graphics;

namespace SpaceCreep.Client.Lib.Objects
{
    public abstract class GameObject : IComparable
    {
        public bool Collision = true;
        public int UniqueObjectId { get; set; }
        public Vector2 Position { get; set; }
        public float CollisionRadius { get; set; }

        public Rectangle CollisionBounds => new Rectangle((int) (Position.X - CollisionRadius),
            (int) (Position.Y - CollisionRadius),
            (int) CollisionRadius * 2, (int) CollisionRadius * 2);

        public int CompareTo(object obj)
        {
            var gameObject = obj as GameObject;

            if (gameObject == null)
            {
                throw new ArgumentException("The argument is not a GameObject");
            }

            var result = Position.Y.CompareTo(gameObject.Position.Y);
            return result == 0 ? Position.X.CompareTo(gameObject.Position.X) : result;
            
        }

        public abstract void Draw(SpriteBatch batch);

        public void ClampToArea(int width, int height)
        {
            if (Position.X < 0 + CollisionRadius)
                Position = new Vector2(width - CollisionRadius, Position.Y);
            if (Position.Y < 0 + CollisionRadius)
                Position = new Vector2(Position.X, height - CollisionRadius);

            if (Position.X > width - CollisionRadius)
                Position = new Vector2(0 + CollisionRadius, Position.Y);

            if (Position.Y > height - CollisionRadius)
                Position = new Vector2(Position.X, 0 + CollisionRadius);
        }

        public void DrawCollisionBounds(SpriteBatch batch)
        {
            batch.Draw(GameGraphics.CollisionRadius, CollisionBounds, new Color(255, 255, 255, 100));
        }
    }
}