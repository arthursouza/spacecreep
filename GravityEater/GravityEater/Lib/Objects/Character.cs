using System.Diagnostics;
using GravityEater.Lib.Graphics;
using GravityEater.Lib.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityEater.Lib.Objects
{
    [DebuggerDisplay("Name = {Name}")]
    public class Character : GameObject
    {
        //public delegate void HealthChangedHandler(AttackEventArgs attack);

        //public event HealthChangedHandler HealthChanged;
        
        private float dashSpeed = 3f;
        private float dashTime = 200;
        private float dashTimer;
        private Vector2 lastDirection;

        #region Timers

        private float currentTurnTimer;
        private float distanceAttackTimer;
        private float distanceCooldown = 1;
        private float meleeAttackTimer;
        private float movementInterval;
        private float movementTimer;
        private float regenTimer;
        private float runningTimer;
        private float turnTimer = 300;

        public int StepSize = 6;

        private float runningSpeedModifier = 2.3f;

        #endregion
        
        public bool IsPlayer
        { get; set; }

        public bool IsAlive
        { get; set; }

        public Vector2 Origin
        {
            get;
            set;
        }

        public string Name
        { get; set; }

        public Vector2 BodyCenter
        {
            get { return new Vector2(Position.X, Position.Y - CharSprite.Height/2); }
        }
        
        public SpriteAnimation CharSprite
        { get; set; }

        public SpriteAnimation AttackSprite
        { get; set; }

        public Direction Facing
        { get; set; }

        public Character Target
        { get; set; }
        
        public Character(SpriteAnimation spriteAnimation)
        {
            CharSprite = spriteAnimation;
            StartupVariables();
        }

        public Character()
        {
            StartupVariables();
        }

        private void StartupVariables()
        {
            CollisionRadius = 25f;
            movementInterval = 20f;
            Facing = Direction.Down;
            Name = string.Empty;
            IsAlive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                //UpdateRegeneration(gameTime);
                //UpdateStatusEffects(gameTime);
                UpdateMovement(gameTime);
                //UpdateAttackAction(gameTime);
                //UpdateSkillsCooldown(gameTime);
            }

            CharSprite.Update(gameTime);
            CharSprite.Animate();
        }
        
        public bool IsInRange(GameObject target)
        {
            Vector2 distance = target.Position - Position;
            // Faço os calculos apenas se o personagem estiver proximo o suficiente
            return (distance.Length() <= target.CollisionRadius + CollisionRadius);
        }

        /// <summary>
        ///     Checks wheter the character is within hearing radius
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        
        //public void StartAttack()
        //{
        //    StopMovement();
        //    ActionState = ActionState.Attacking;

        //    if (IsPlayer)
        //    {
        //        CharSprite.AnimateAction(ComboManager.GetNextAttack());
        //        Stats.Stamina -= AttackStaminaCost;
        //    }
        //    else
        //        CharSprite.AnimateAction(AnimationType.Attack1);


        //    meleeAttackTimer = MeleeCooldown;
        //}

        //public void AttackTarget(Character attacked)
        //{
        //    int chance = BattleHelper.CalculateMeleeChance(this, attacked);
        //    int damage = BattleHelper.CalculatePhysicalDamage(this, attacked);
        //    damage = damage >= attacked.Stats.Hp ? attacked.Stats.Hp : damage;

        //    var rand = new Random(DateTime.Now.Millisecond);

        //    bool criticalHit = rand.Next(0, 100) <= BattleHelper.CalculateCriticalChance(this);

        //    if (rand.Next(100) < chance)
        //    {
        //        var args = new AttackEventArgs();
        //        args.Attacker = this;
        //        args.Target = attacked;

        //        StatusEffect lifeSteal = StatusEffects.Find(x => x.HealthOnAttack > 0);
        //        if (lifeSteal != null)
        //        {
        //            args.Damage = lifeSteal.HealthOnAttack;
        //            ChangeHealth(args, true);
        //        }

        //        if (criticalHit)
        //            damage *= 2;
        //        args.Damage = -damage;

        //        attacked.ChangeHealth(args, true);
        //    }
        //}

        //public void ChangeHealth(AttackEventArgs args, bool animate)
        //{
        //    if (Stats.Hp != Stats.MaxHp || args.Damage < 0)
        //    {
        //        Stats.Hp += args.Damage;

        //        if (animate)
        //        {
        //            HealthChanged.Invoke(args);
        //        }
        //    }
        //}

        public bool IsInRange(Vector2 position, float range)
        {
            return (Position - position).Length() <= range;
        }
        
        #region Movement

        private void UpdateMovement(GameTime gameTime)
        {
            movementTimer += gameTime.ElapsedGameTime.Milliseconds;
            currentTurnTimer += gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Move(Vector2 dir)
        {
            if (dir != Vector2.Zero)
            {
                dir.Normalize();
                Vector2 direction;

                if (!IsPlayer) // || MovementEngine.Waypoints.Count > 0)
                    direction = ValidateTurnInterval(dir);
                else
                    direction = dir;

                Turn(MapHelper.GetDirectionFromVector(direction));

                if (movementTimer >= movementInterval)
                {
                    Position = FuturePosition(direction);
                    movementTimer = 0f;
                }
            }
        }
        
        public void Face(Character character)
        {
            float angle = MapHelper.GetAngleBetweenPoints(Position, character.Position);
            if (angle < 0)
                angle += 360;

            Direction dir = MapHelper.GetDirectionFromAngle(angle);
            Turn(dir);
        }

        private Vector2 ValidateTurnInterval(Vector2 dir)
        {
            Vector2 direction;
            if (dir != lastDirection)
            {
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
            }
            else
            {
                direction = dir;
            }

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
                Vector2 movement = direction* speedModifier * StepSize;

                movement = movement * speedModifier;
                
                return Position + movement;
            }
            return Position;
        }

        public bool CollidesWithObject(GameObject c)
        {
            Vector2 distance = c.Position - Position;
            if (c.CollisionRadius > 0)
                return (distance.Length() < c.CollisionRadius + CollisionRadius);

            return false;
        }

        public bool WillCollideWithObject(GameObject obj, Vector2 movement)
        {
            Vector2 distance = obj.Position - FuturePosition(movement);
            if (obj.CollisionRadius > 0)
                return (distance.Length() < obj.CollisionRadius + CollisionRadius);

            return false;
        }

        #endregion

        #region Drawing

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (GameConfig.Config.ShowHud)
            {
                if (IsAlive)
                {
                    //DrawName(spriteBatch);
                    DrawHpBars(spriteBatch);
                }
            }

            CharSprite.Draw(spriteBatch, Position, 1, 0);
        }

        private void DrawHpBars(SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                int barWidth = 64;
                int barHeight = 7;
                //int healthPerc = MaxHp > 0 ? (Hp * 100) / MaxHp : 0;
                //int currentBarSize = (healthPerc*barWidth)/100;

                Texture2D healthBar = GameGraphics.CharacterHpBar;
                Texture2D healthBarBackground = GameGraphics.CharacterHpBar;

                Vector2 size = new Vector2(CharSprite.Width, CharSprite.Height);

                //spriteBatch.Draw(
                //    healthBarBackground,
                //    new Rectangle(
                //        (int) (Position).X - barWidth/2,
                //        (int) (Position).Y - (int) size.Y,
                //        barWidth,
                //        barHeight),
                //    Color.White);

                //spriteBatch.Draw(
                //    healthBar,
                //    new Rectangle(
                //        (int) Position.X - barWidth/2,
                //        (int) Position.Y - (int) size.Y,
                //        currentBarSize,
                //        barHeight),
                //    Color.White);
            }
        }

        //private void DrawName(SpriteBatch spriteBatch)
        //{
        //    Vector2 size = CharSprite.CurrentSpriteInfo.RealSize;
        //    Vector2 nameSize = Fonts.ArialBlack12.MeasureString(Name);
        //    Color backColor = this is Enemy ? Color.DarkRed : Color.DarkGreen;

        //    Drawing.DrawText(spriteBatch, Fonts.ArialBlack12, Name,
        //        new Vector2(
        //            (float) Math.Floor(Position.X) - (nameSize.X/2),
        //            (float) Math.Floor(Position.Y) - size.Y - nameSize.Y), Color.White, true, Color.Black);
        //}

        #endregion
    }
}