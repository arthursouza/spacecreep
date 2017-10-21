using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCreep.Client.Lib.Sprite;

namespace SpaceCreep.Client.Lib.Objects
{
    public class Character : GameObject
    {
        private bool attacking;

        private Vector2 lastDirection;

        public Character(SpriteAnimation spriteAnimation)
        {
            CharSprite = spriteAnimation;
            StartupVariables();
        }

        public Character()
        {
            StartupVariables();
        }

        public bool IsPlayer { get; set; }

        public bool IsAlive { get; set; }
        
        public float Hp { get; set; }

        public float MaxHp { get; set; }
        
        public SpriteAnimation CharSprite { get; set; }

        public SpriteAnimation AttackSprite { get; set; }

        public Direction Facing { get; set; }

        public Character Target { get; set; }

        private void StartupVariables()
        {
            CollisionRadius = 25f;

            Facing = Direction.Down;

            IsAlive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (IsAlive)
                UpdateMovement(gameTime);

            if (!attacking)
            {
                attackTimer = 0f;

                CharSprite.Update(gameTime);
                CharSprite.Animate();
            }
            else
            {
                attackTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

                if (attackTimer >= attackInterval)
                    attacking = false;

                AttackSprite.Update(gameTime);
                AttackSprite.Animate();
            }
        }

        public bool IsInRange(GameObject target)
        {
            var distance = target.Position - Position;
            // Faço os calculos apenas se o personagem estiver proximo o suficiente
            return distance.Length() <= target.CollisionRadius + CollisionRadius;
        }

        public bool IsInRange(Vector2 position, float range)
        {
            return (Position - position).Length() <= range;
        }

        public void StartAttack()
        {
            attacking = true;
        }

        #region Timers

        private float attackTimer;
        private readonly float attackInterval = 800f;

        private float movementTimer;
        private readonly float movementInterval = 20f;

        private float accelerationTimer;
        private readonly float accelerationInterval = 1000f;

        private float currentTurnTimer;
        private readonly float turnTimer = 300f;

        public float InitialSpeed = 5f;
        public float MaxSpeed = 10f;
        public float CurrentSpeed;

        #endregion

        #region Movement

        private void UpdateMovement(GameTime gameTime)
        {
            var elapsedTime = (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            movementTimer += elapsedTime;
            currentTurnTimer += elapsedTime;
            accelerationTimer += elapsedTime;

            if (accelerationTimer > accelerationInterval)
                accelerationTimer = accelerationInterval;
        }

        public void Move(Vector2 dir)
        {
            if (dir != Vector2.Zero)
            {
                dir.Normalize();

                var direction = SmoothTurns(dir);

                Turn(MapHelper.GetDirectionFromVector(direction));

                var currentSpeedPc = accelerationTimer / accelerationInterval;

                CurrentSpeed = InitialSpeed + (MaxSpeed - InitialSpeed) * currentSpeedPc;

                if (movementTimer >= movementInterval)
                {
                    Position = FuturePosition(direction);
                    movementTimer = 0f;
                }
            }
            else
            {
                accelerationTimer = 0f;
            }
        }

        public void Face(Character character)
        {
            var angle = MapHelper.GetAngleBetweenPoints(Position, character.Position);
            if (angle < 0)
                angle += 360;

            var dir = MapHelper.GetDirectionFromAngle(angle);
            Turn(dir);
        }

        private Vector2 SmoothTurns(Vector2 dir)
        {
            if (IsPlayer)
                return dir;

            Vector2 direction;
            if (dir != lastDirection)
                if (currentTurnTimer > turnTimer)
                {
                    currentTurnTimer = 0;
                    lastDirection = dir;
                    direction = dir;
                }
                else
                {
                    direction = lastDirection;
                }
            else
                direction = dir;

            return direction;
        }

        public void Turn(Direction d)
        {
            Facing = d;
            //CharSprite.Turn(Facing);
        }

        public Vector2 FuturePosition(Vector2 direction, float speedModifier = 1f)
        {
            if (movementTimer >= movementInterval)
            {
                var movement = direction * speedModifier * CurrentSpeed;

                movement = movement * speedModifier;

                return Position + movement;
            }
            return Position;
        }

        public bool CollidesWithObject(GameObject c)
        {
            var distance = c.Position - Position;
            if (c.CollisionRadius > 0)
                return distance.Length() < c.CollisionRadius + CollisionRadius;

            return false;
        }

        public bool WillCollideWithObject(GameObject obj, Vector2 movement)
        {
            var distance = obj.Position - FuturePosition(movement);
            if (obj.CollisionRadius > 0)
                return distance.Length() < obj.CollisionRadius + CollisionRadius;

            return false;
        }

        #endregion

        #region Drawing

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!attacking)
                CharSprite.Draw(spriteBatch, Position);
            else
                AttackSprite.Draw(spriteBatch, Position);
        }
        
        #endregion
    }
}