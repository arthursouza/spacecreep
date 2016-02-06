using System;
using System.Collections.Generic;
using GravityEater.Lib.AI;
using GravityEater.Lib.Objects;
using Microsoft.Xna.Framework;

namespace GravityEater.Lib
{
    public class BehaviorEngine
    {
        private static Random rand;

        public BehaviorEngine(Character character)
        {
            Actions = new List<ActionBehavior>();
            Character = character;
            if (rand == null)
                rand = new Random((int) (DateTime.Now.Millisecond*character.Position.X*character.Position.Y));
        }

        public BehaviorEngine()
        {
            Actions = new List<ActionBehavior>();
            if (rand == null)
                rand = new Random(DateTime.Now.Millisecond);
        }

        public BehaviorType BehaviorType { get; set; }
        public int BehaviorId { get; set; }
        public string BehaviorName { get; set; }
        public Character Character { get; set; }
        public List<ActionBehavior> Actions { get; set; }

        public void Update(GameTime gameTime, Character character, List<GameObject> gameObjects)
        {
        }

        //public void Wandering()
        //{
        //    if (Character.MovementEngine.CurrentWaypoint == null)
        //    {
        //        Vector2 tile = MapHelper.GetTileFromPixels(Character.Position);
        //        int nextX = rand.Next(-1, 2);
        //        int nextY = rand.Next(-1, 2);
        //        var destination = new Vector2(tile.X + nextX, tile.Y + nextY);

        //        if (destination.X < 0)
        //            destination.X *= -1;
        //        if (destination.Y < 0)
        //            destination.Y *= -1;

        //        if (Character.MovementEngine.PathFinding.Collision.GetCollisionValue(destination) ==
        //            CollisionValues.Passable)
        //            Character.MovementEngine.MoveTo(destination, MovementMode.EnterAndWait);
        //    }
        //}

        
    }
}