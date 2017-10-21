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
        
        public static void EnforcePenetrationConstraint(Character model, IEnumerable<GameObject> objects)
        {
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
                    var d = obj.Position - model.Position;

                    if (d != Vector2.Zero)
                        d.Normalize();

                    model.Position = obj.Position - d * (model.CollisionRadius + obj.CollisionRadius);
                }
            }
        }
    }
}