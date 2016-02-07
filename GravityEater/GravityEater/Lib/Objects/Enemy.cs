using System;
using System.Collections.Generic;
using GravityEater.Lib.AI;
using Microsoft.Xna.Framework;

namespace GravityEater.Lib.Objects
{
    public class Enemy : Character
    {
        public enum BehaviorType
        {
            Wandering,
            Chasing,
            Attacking,
            Fleeing,
            Idle
        }

        public static float DeathTimerLimit = 15000;

        public BehaviorType Behavior { get; set; }

        public Enemy()
        {
            var r = new Random(DateTime.Now.Millisecond);
            RespawnTime = r.Next(45000, 60001);
            Priority = 1;
        }

        public int MaxTargetDistance { get; set; }
        public int FollowRange { get; set; }
        public int DeathTimer { get; set; }
        public int RespawnTime { get; set; }
        public bool PlayerKillable { get; set; }
        public Vector2 CurrentTargetPosition { get; set; }

        public int Priority { get; set; }

        public void NewTargetPosition(Map.Map currentMap)
        {
            Random r = new Random((int)(DateTime.Now.Millisecond + Position.X));
            CurrentTargetPosition = MapHelper.GetPixelsFromTileCenter(new Vector2(r.Next(0, currentMap.Width + 1), r.Next(0, currentMap.Height + 1)));
        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}