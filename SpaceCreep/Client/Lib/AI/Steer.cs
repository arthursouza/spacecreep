using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceCreep.Client.Lib.Objects;

namespace SpaceCreep.Client.Lib.AI
{
    public static class Steering
    {
        /// <summary>
        ///     Retorna o vetor direção para a qual o modelo deve ser movimentado, em direção ao seu alvo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="target">Posição do alvo</param>
        /// <returns>Vetor direção</returns>
        public static Vector2 Seek(GameObject model, Vector2 target)
        {
            if (!(model.CollisionRadius < (model.Position - target).Length()))
                return Vector2.Zero;

            var diff = target - model.Position;
            diff.Normalize();

            return diff;
        }

        //public static Vector2 Seek(Character character, Character target, float minRange)
        //{
        //    if (minRange + target.CollisionRadius <= (character.Position - target.Position).Length())
        //    {
        //        var angle = MapHelper.GetAngleBetweenPoints(character, target);
        //        angle = angle < 0 ? angle + 360 : angle;
        //        return MapHelper.GetVectorFromDirection(MapHelper.GetDirectionFromAngle(angle));
        //        //return MapHelper.GetVectorFromAngle(angle);
        //    }
        //    return Vector2.Zero;
        //}

        public static Vector2 Flee(GameObject model, GameObject target, float distance = 500)
        {
            // Se o modelo estiver a mais de X pixels de seu alvo, ele não foge mais
            if ((model.Position - target.Position).Length() < distance)
            {
                var angle = MapHelper.GetAngleBetweenPoints(model, target);
                angle = angle < 0 ? angle + 360 : angle;
                return -1 * MapHelper.GetVectorFromDirection(MapHelper.GetDirectionFromAngle(angle));
            }
            return Vector2.Zero;
        }

        //public static Vector2 Separation(Character model, List<Character> neighbours)
        //{
        //    var steeringForce = Vector2.Zero;

        //    for (var i = 0; i < neighbours.Count; i++)
        //        if (neighbours[i] != model && neighbours[i].IsAlive)
        //            if (model.CollidesWithObject(neighbours[i]))
        //            {
        //                steeringForce += Flee(model, neighbours[i]);

        //                if (steeringForce != Vector2.Zero)
        //                    steeringForce.Normalize();
        //            }

        //    return steeringForce;
        //}

        //public static Vector2 KeepDistance(GameObject model, List<GameObject> neighbours, float distance)
        //{
        //    var steeringForce = Vector2.Zero;

        //    for (var i = 0; i < neighbours.Count; i++)
        //    {
        //        var enemy = neighbours[i] as Enemy;
        //        if (enemy != null && !enemy.IsAlive)
        //            continue;

        //        if (neighbours[i] != model)
        //            if ((model.Position - neighbours[i].Position).Length() < distance)
        //            {
        //                steeringForce += Flee(model, neighbours[i]);

        //                if (steeringForce != Vector2.Zero)
        //                    steeringForce.Normalize();
        //            }
        //    }

        //    return steeringForce;
        //}

        public static bool EnforcePenetrationConstraint(Character model, IEnumerable<GameObject> objects)
        {
            var CollisedWithNeighbour = false;

            // Checar colisão com todos os Inimigos
            foreach (var obj in objects)
            {
                // If is enemy and is dead
                if (obj is Character && !(obj as Character).IsAlive)
                    continue;

                // If this object has no collision
                if (!obj.Collision)
                    continue;

                if (model != obj && model.CollidesWithObject(obj))
                {
                    CollisedWithNeighbour = true;
                    var d = obj.Position - model.Position;

                    if (d != Vector2.Zero)
                        d.Normalize();

                    model.Position = obj.Position - d * (model.CollisionRadius + obj.CollisionRadius);
                }
            }
            return CollisedWithNeighbour;
        }

        //public static bool HasFutureCollision(Character character, Vector2 characterMovement,
        //    List<GameObject> gameObjects)
        //{
        //    foreach (var item in gameObjects)
        //    {
        //        if (item is Enemy && !(item as Enemy).IsAlive)
        //            continue;

        //        if (item.UniqueObjectId != character.UniqueObjectId)
        //            if (character.WillCollideWithObject(item, characterMovement))
        //                return true;
        //    }

        //    return false;
        //}

        //public static Vector2 KeepDistance(Enemy creature, Character character, int distance)
        //{
        //    var steeringForce = Vector2.Zero;
        //    if ((creature.Position - character.Position).Length() < distance)
        //    {
        //        steeringForce += Flee(creature, character);

        //        if (steeringForce != Vector2.Zero)
        //            steeringForce.Normalize();
        //    }
        //    return steeringForce;
        //}
    }
}