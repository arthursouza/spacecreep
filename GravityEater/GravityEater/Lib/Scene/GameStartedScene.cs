using System;
using System.Collections.Generic;
using System.Linq;
using GravityEater.Lib.AI;
using GravityEater.Lib.Graphics;
using GravityEater.Lib.Input;
using GravityEater.Lib.Map;
using GravityEater.Lib.Objects;
using GravityEater.Lib.Sprite;
using LotusLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GravityEater.Lib.Scene
{
    public class GameStartedScene : Scene
    {
        public List<dynamic> EnemyTypes = new List<dynamic>()
        {
            new
            {
                Id = 1,
                Name = "CreepShip"
            },
            new
            {
                Id = 2,
                Name = "BigShip"
            },
            new
            {
                Id = 3,
                Name = "HealthKit"
            }
        };

        private float trackDelay = 300f;
        private float trackTimer = 0f;


        private List<GameObject> gameObjects;

        private Character Player
        {
            get { return Game.Player; }
            set { Game.Player = value; }
        }

        private List<Enemy> AliveEnemies
        {
            get { return Game.CurrentMap.MapEnemies.FindAll(x => x.IsAlive); }
        }

        private List<Npc> AliveNpcs
        {
            get { return Game.CurrentMap.MapNpcs.FindAll(x => x.IsAlive); }
        }

        private List<Map.Map> Maps
        { get; set; }

        public GameStartedScene(Game game)
        {
            Game = game;
            gameObjects = new List<GameObject>();
            Maps = new List<Map.Map>();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.ScaleMatrix);
            Game.CurrentMap?.Draw(spriteBatch, Game.CurrentMap.Tileset, true);
            spriteBatch.End();

            var renderList = new List<GameObject>();
            renderList.AddRange(gameObjects);
            renderList.Sort();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.ScaleMatrix);

            Game.BasicEffect.Projection = Matrix.CreateOrthographicOffCenter(
                0 + Camera.Position.X,
                GameConfig.Config.WindowWidth + Camera.Position.X,
                GameConfig.Config.WindowHeight + Camera.Position.Y,
                0 + Camera.Position.Y,
                0, 1);

            Game.BasicEffect.CurrentTechnique.Passes[0].Apply();

            if (GameConfig.Config.DebugMode)
            {
                //    Game.CurrentMap.CollisionLayer.Draw(spriteBatch);
                renderList.ForEach(x => x.DrawCollisionBounds(spriteBatch));
            }

            Game.CurrentMap?.Animations.ForEach(x => x.Draw(spriteBatch));

            renderList.ForEach(x => x.Draw(spriteBatch));
            
            //Game.CurrentMap.Draw(spriteBatch, Game.CurrentMap.Tileset, true);

            //DrawFloatingText();
            //DrawDialogs();

            if (GameConfig.Config.DebugMode)
                DrawDebugInfo();

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            DrawResourceBar(spriteBatch, new Vector2(30, 30), (int)Player.MaxHp, (int)Player.Hp, Player.Hp / Player.MaxHp * 100, Color.White);

            spriteBatch.DrawString(Fonts.ArialBlack14, Game.Points.ToString(), new Vector2(30, GameConfig.Config.WindowHeight - 40), Color.LightBlue);

            spriteBatch.End();
        }

        public override void MouseDown(MouseButton button)
        {
            InputManager.MovementVector = Steering.Seek(Player, InputManager.MouseToMapVector);
        }

        private void DrawDebugInfo()
        {
            Drawing.DrawText(spriteBatch, Fonts.Arial12, "Pos:" + Player.Position.ToString(), Camera.Position + new Vector2(20, 20), Color.White, true);
            //Drawing.DrawText(spriteBatch, Fonts.Arial12, "Action:" + Player.ActionState.ToString(), Camera.Position + new Vector2(20, 20), Color.White, true);
            //Drawing.DrawText(spriteBatch, Fonts.Arial12, "Animation:" + Player.CharSprite.CurrentAnimationType.ToString(), Camera.Position + new Vector2(20, 40), Color.White, true);
            DrawOnScreenLog(spriteBatch);
        }

        private void DrawResourceBar(SpriteBatch spriteBatch, Vector2 position, int maxValue, int currentValue, float percentage, Color color)
        {
            Rectangle background = GameGraphics.CharacterHpBarBg.Bounds;
            Rectangle bar = GameGraphics.CharacterHpBar.Bounds;
            var innerOffset = new Vector2(0, 0);
            SpriteFont font = Fonts.TrebuchetMS14;

            // Bar background
            var backgroundRect = new Rectangle(
                (int)(position.X),
                (int)(position.Y),
                GameGraphics.CharacterHpBarBg.Width,
                GameGraphics.CharacterHpBarBg.Height);

            // Bar position and size
            var innerRect = new Rectangle(
                (int)(position.X + innerOffset.X),
                (int)(position.Y + innerOffset.Y),
                (int)(bar.Width * (percentage / 100)),
                bar.Height);

            // Actual drawn texture
            var innerRectSource = new Rectangle(
                0,
                0,
                (int)(bar.Width * (percentage / 100)),
                bar.Height);

            // Draw background
            spriteBatch.Draw(GameGraphics.CharacterHpBarBg, backgroundRect, Color.White);

            // Draw bar
            spriteBatch.Draw(GameGraphics.CharacterHpBar, innerRect, innerRectSource, Color.White);

            //string text = string.Format("{0} / {1}", currentValue, maxValue);

            //Vector2 textSize = font.MeasureString(text);
            //Drawing.DrawText(spriteBatch, font, text,
            //    new Vector2(
            //        position.X + background.Width + 5,
            //        position.Y + background.Height / 2 - textSize.Y / 2),
            //    Color.White,
            //    true);
        }

        public override void Update(GameTime gameTime)
        {
            //Game.GameStats.TotalTimePlayed =
            //    Game.GameStats.TotalTimePlayed.Add(new TimeSpan(0, 0, 0,
            //        (int) gameTime.ElapsedGameTime.TotalMilliseconds));
            //Game.CurrentSave.TimePlayedTicks += gameTime.ElapsedGameTime.Ticks;

            //if (Player.Stats.Level > GameStats.HighestLevel)
            //    GameStats.HighestLevel = Player.Stats.Level;

            //screenInfo.Update(gameTime, Player);
            //UpdateInterface(gameTime);

            if (!Game.Paused)
            {
                UpdateGameObjects();
                UpdatePlayer(gameTime);
                UpdateEnemies(gameTime);
                //UpdateNpcs(gameTime);
                //UpdateMapEvents(gameTime);
                UpdateAnimations(gameTime);
                //ProcessAttackQueue(gameTime);
            }
            else
            {
                //gamePausedMenu.Update(gameTime);
            }

            //for (int i = 0; i < Projectiles.Count; i++)
            //{
            //    Projectile projectile = Projectiles[i];
            //    projectile.Update(gameTime);
            //    bool hitTarget = false;

            //    foreach (Enemy e in AliveEnemies)
            //    {
            //        if (e.CollidesWithObject(projectile))
            //        {
            //            var args = new AttackEventArgs(Player, e, -10, true);
            //            e.ChangeHealth(args, true);
            //            Projectiles.RemoveAt(i);
            //            i--;
            //            hitTarget = true;
            //            break;
            //        }
            //    }

            //    if (hitTarget)
            //        break;

            //    if (projectile.CurrentRange > projectile.MaxRange)
            //    {
            //        Projectiles.RemoveAt(i);
            //        i--;
            //        break;
            //    }
            //}

            Camera.LockToTarget(Player.Position, GameConfig.Config.WindowWidth, GameConfig.Config.WindowHeight);

            Camera.Update(gameTime);

            Camera.ClampToArea(
                Game.CurrentMap.WidthInPixels - GameConfig.Config.WindowWidth,
                Game.CurrentMap.HeightInPixels - GameConfig.Config.WindowHeight);
        }

        public override void UpdateKeyboardInput()
        {

            //InputManager.MovementVector = Vector2.Zero;

            //if (InputManager.KeyPress(InputConfiguration.Config.Inventory))
            //{
            //    ToggleMenuWindow(inventoryWindow);
            //}
            //else if (InputManager.KeyPress(InputConfiguration.Config.QuestLog))
            //{
            //    ToggleMenuWindow(questWindow);
            //}
            //else if (InputManager.KeyPress(InputConfiguration.Config.CharacterInformation))
            //{
            //    ToggleMenuWindow(statsWindow);
            //}
            //else if (InputManager.KeyboardState.IsKeyDown(Keys.Z))
            //    Camera.ResetCamera();

            //if (InputManager.KeyboardState.IsKeyDown(Keys.LeftControl) && InputManager.KeyPress(Keys.D))
            //{
            //    if (GameConfig.Config.DebugMode)
            //        ToggleMenuWindow(debugWindow);
            //}

            //// Toggle fullscreen
            //if (InputManager.KeyPress(Keys.Enter) &&
            //    (InputManager.KeyboardState.IsKeyDown(Keys.LeftAlt) ||
            //     InputManager.KeyboardState.IsKeyDown(Keys.RightAlt)))
            //{
            //    GameConfig.Config.FullScreen = !GameConfig.Config.FullScreen;
            //}
            //else if (InputManager.KeyPress(Keys.Escape))
            //{
            //    if (gamePausedMenu.Visible)
            //    {
            //        gamePausedMenu.Visible = false;

            //        if (!AnyWindowsOnscreen())
            //        {
            //            Game.Paused = false;
            //        }
            //    }
            //    else
            //    {
            //        gamePausedMenu.Visible = true;
            //        Game.Paused = true;
            //    }
            //}
            //else
            //{
            //InputManager.MovementInput = InputManager.MovementVector;

            //    for (int i = 0; i < InputConfiguration.Config.Hotkeys.Count; i++)
            //    {
            //        if (InputManager.KeyPress(InputConfiguration.Config.Hotkeys[i]))
            //        {
            //            if (hotkeys.Count > i)
            //                PrepareSkillCast(hotkeys[i].Skill);
            //        }
            //    }
            //}
        }

        public override void UpdateMouseInput()
        {
            if (!Game.Paused)
            {
                #region Check enemy hover

                //currentTarget = null;
                //foreach (Npc npc in Game.CurrentMap.MapNpcs.FindAll(x => x.IsAlive))
                //{
                //    var rect = new Rectangle(
                //        (int)(npc.Position.X - npc.CollisionRadius),
                //        (int)(npc.Position.Y - npc.CollisionRadius),
                //        (int)npc.CollisionRadius * 2,
                //        (int)npc.CollisionRadius * 2);

                //    if (rect.Contains(InputManager.MouseToMapPoint))
                //        currentTarget = npc;
                //}

                //foreach (Enemy en in AliveEnemies)
                //{
                //    var rect = new Rectangle(
                //        (int)(en.Position.X - en.CollisionRadius),
                //        (int)(en.Position.Y - en.CollisionRadius),
                //        (int)en.CollisionRadius * 2,
                //        (int)en.CollisionRadius * 2);

                //    if (rect.Contains(InputManager.MouseToMapPoint))
                //        currentTarget = en;
                //}

                #endregion
            }
        }

        private void UpdateGameObjects()
        {
            gameObjects.Clear();
            if (Game.CurrentMap != null)
            {
                gameObjects.AddRange(Game.CurrentMap.MapEnemies);
                gameObjects.AddRange(Game.CurrentMap.MapNpcs);
                gameObjects.AddRange(Game.CurrentMap.MapObjects);
            }
            gameObjects.Add(Player);
        }

        private void UpdateAnimations(GameTime gameTime)
        {
            for (int i = 0; i < Game.CurrentMap.Animations.Count; i++)
            {
                Game.CurrentMap.Animations[i].Update(gameTime);
                if (!Game.CurrentMap.Animations[i].Sprite.IsAnimating)
                {
                    if (Game.CurrentMap.Animations[i].Name == "Star1")
                        SpawnRandomStars(1);

                    Game.CurrentMap.Animations.RemoveAt(i);
                    i--;
                }
            }

            //floatingText.ForEach(x => x.Update(gameTime));

            //if (lootLog.Count > 0) lootLog[0].Update(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            Player.Update(gameTime);

            if (Player.Hp <= 0)
            {
                Player.IsAlive = false;
            }

            if (!Player.IsAlive)
            {
                Game.State = GameState.GameOver;
            }

            if (InputManager.MovementVector != Vector2.Zero)
            {
                Player.Move(InputManager.MovementVector);

                trackTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

                if (trackTimer >= trackDelay)
                {
                    trackTimer = 0f;
                    Game.CurrentMap.Animations.Add(new Animation() { Position = Player.Position + new Vector2(-10, 10), Collision = false, Sprite = new SpriteAnimation(GameGraphics.MonsterTrack, 200, 8) { IsToggle = true } });
                    Game.CurrentMap.Animations.Add(new Animation() { Position = Player.Position + new Vector2(10, 10), Collision = false, Sprite = new SpriteAnimation(GameGraphics.MonsterTrack, 200, 8) { IsToggle = true } });
                    Game.CurrentMap.Animations.Add(new Animation() { Position = Player.Position + new Vector2(-10, -10), Collision = false, Sprite = new SpriteAnimation(GameGraphics.MonsterTrack, 200, 8) { IsToggle = true } });
                    Game.CurrentMap.Animations.Add(new Animation() { Position = Player.Position + new Vector2(10, -10), Collision = false, Sprite = new SpriteAnimation(GameGraphics.MonsterTrack, 200, 8) { IsToggle = true } });
                }
            }

            Steering.EnforcePenetrationConstraint(Player, gameObjects);

            //if (InputManager.MovementVector == Vector2.Zero)
            //{
            //    var nearbyEnemies = AliveEnemies.Where(e => Player.IsInRange(e.Position, 500));

            //    if (Player.Target == null)
            //    {
            //        if (nearbyEnemies.Any())
            //        {
            //            Player.Target = nearbyEnemies.FirstOrDefault();
            //        }
            //        else
            //        {
            //            Player.Target = null;
            //        }
            //    }

            //    if (Player.Target != null)
            //    {
            //        Player.Move(Steering.Seek(Player, Player.Target.Position));
            //    }
            //}
            //Player.ClampToArea(Game.CurrentMap.WidthInPixels, Game.CurrentMap.HeightInPixels);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            for (int i = 0; i < AliveEnemies.Count; i++)
            {
                var enemy = Game.CurrentMap.MapEnemies[i];
                enemy.Update(gameTime);

                if (enemy.IsAlive)
                {
                    //enemy.Behavior.Update(gameTime, enemy, gameObjects);
                    UpdateEnemyBehavior(enemy);

                    // ATTACK ENEMY
                    if (Player.IsInRange(enemy.Position, enemy.CollisionRadius + Player.CollisionRadius + 10) && enemy.PlayerKillable)
                    {
                        Player.Hp -= enemy.Damage;
                        Player.Hp += enemy.Heal;
                        Game.ChangePoints(enemy.Points);

                        Player.StartAttack();

                        KillEnemy(enemy, true);
                    }

                    if (enemy.IsAlive)
                    {
                        var collidedEnemies =
                            AliveEnemies.Where(
                                e =>
                                    e.IsInRange(enemy.Position, enemy.CollisionRadius + e.CollisionRadius + 10) &&
                                    e.Priority != enemy.Priority).ToList();

                        foreach (var e in collidedEnemies)
                        {
                            if (e.Priority > enemy.Priority)
                            {
                                KillEnemy(enemy, false);
                            }
                            else
                            {
                                KillEnemy(e, false);
                            }
                        }

                        if (enemy.IsAlive)
                        {
                            Steering.EnforcePenetrationConstraint(enemy, gameObjects);
                            enemy.ClampToArea(Game.CurrentMap.WidthInPixels, Game.CurrentMap.HeightInPixels);
                        }
                    }
                }
                else
                {
                    SpawnEnemy(enemy);

                    //enemy.DeathTimer += (int) gameTime.ElapsedGameTime.TotalMilliseconds;
                    //if (enemy.DeathTimer > Enemy.DeathTimerLimit)
                    //{
                    Game.CurrentMap.MapEnemies.RemoveAt(i);
                    //    i--;
                    //}
                }
            }
        }

        private void SpawnEnemy(Enemy deadEnemy)
        {
            if (deadEnemy.UniqueObjectId == 1)
            {
                SpawnCreepShips(1);
            }
            else if (deadEnemy.UniqueObjectId == 2)
            {
                SpawnBigShips(1);
            }
            else if (deadEnemy.UniqueObjectId == 4)
            {
                SpawnCreepShips2(1);
            }
        }

        private void UpdateEnemyBehavior(Enemy enemy)
        {
            //enemy.MovementEngine.PathFinding.GameObjects = gameObjects;
            Vector2 movement = Vector2.Zero;

            //var allies = new List<Character>();
            //allies.AddRange(AliveNpcs);
            //allies.Add(Player);

            //if (enemy.ActionState != ActionState.Attacking)
            //{
            //    //If creature has target, check behavior
            //    //if no target, wander til target is found
            //    foreach (ActionBehavior action in enemy.Behavior.Actions)
            //    {
            //        bool isValid = false;
            //        Character target = null;

            //        #region Check if condition is valid

            //        switch (action.Condition.Target)
            //        {
            //            case ConditionTarget.Self:
            //                target = enemy;
            //                break;
            //            case ConditionTarget.Enemy:
            //                if (allies.Count > 0)
            //                    target = allies[0];
            //                break;
            //            case ConditionTarget.Friend:
            //                List<Enemy> targets = AliveEnemies.FindAll(x => x.HealthPercentage <= action.Condition.Value);
            //                if (targets.Count > 0) target = targets[0];
            //                break;
            //        }

            //        if (action.Condition.Target == ConditionTarget.Friend)
            //        {
            //            switch (action.Condition.Type)
            //            {
            //                case ConditionType.HealthLower:
            //                    isValid = AliveEnemies.FindAll(x => x.Stats.Hp <= action.Condition.Value).Count > 0;
            //                    break;
            //                case ConditionType.HealthLowerPC:
            //                    isValid =
            //                        AliveEnemies.FindAll(x => x.HealthPercentage <= action.Condition.Value).Count > 0;
            //                    break;
            //                case ConditionType.HealthHigher:
            //                    isValid = AliveEnemies.FindAll(x => x.Stats.Hp >= action.Condition.Value).Count > 0;
            //                    break;
            //                case ConditionType.HealthHigherPC:
            //                    isValid =
            //                        AliveEnemies.FindAll(x => x.HealthPercentage >= action.Condition.Value).Count > 0;
            //                    break;
            //                case ConditionType.None:
            //                    isValid = true;
            //                    break;
            //            }
            //        }
            //        else
            //        {
            //            //if (target != null)
            //            //{
            //            //    switch (action.Condition.Type)
            //            //    {
            //            //        case ConditionType.HealthLower:
            //            //            isValid = target.Stats.Hp <= action.Condition.Value;
            //            //            break;
            //            //        case ConditionType.HealthLowerPC:
            //            //            isValid = target.HealthPercentage <= action.Condition.Value;
            //            //            break;
            //            //        case ConditionType.HealthHigher:
            //            //            isValid = target.Stats.Hp >= action.Condition.Value;
            //            //            break;
            //            //        case ConditionType.HealthHigherPC:
            //            //            isValid = target.HealthPercentage >= action.Condition.Value;
            //            //            break;
            //            //        case ConditionType.None:
            //            //            isValid = true;
            //            //            break;
            //            //    }
            //            //}
            //        }

            //        #endregion

            //        if (isValid)
            //        {
            //            switch (action.ActionType)
            //            {
            //                case ActionType.Attack:
            //                    break;
            //                case ActionType.Skill:
            //                    if (target != null)
            //                        CastEnemySkill(enemy, enemy.Skills.Find(x => x.Id == action.SkillId), target);
            //                    break;
            //                case ActionType.Behavior:
            //                    enemy.Behavior.BehaviorType = action.NewBehavior;
            //                    break;
            //            }
            //        }
            //    }


            //    if (enemy.HasTarget && enemy.Target.IsAlive)
            //    {
            //        #region Check behavior

            //        if (enemy.Behavior.BehaviorType == BehaviorType.Scared)
            //        {
            //            if (!enemy.IsInAttackRange(enemy.Target))
            //            {
            //                movement = Steering.KeepDistance(enemy, enemy.Target, 200);

            //                //if (enemy.DistanceToTarget > enemy.MaxTargetDistance)
            //                //    enemy.Target = null;
            //            }
            //        }
            //        else if (enemy.Behavior.BehaviorType == BehaviorType.Agressive)
            //        {
            //            if (enemy.HasTarget && !enemy.IsInAttackRange(enemy.Target))
            //            {
            //                movement = Steering.Seek(enemy, enemy.Target, enemy.CurrentWeapon.AttackRange);

            //                //if (enemy.DistanceToTarget > enemy.MaxTargetDistance)
            //                //    enemy.Target = null;
            //            }
            //        }
            //        else if (enemy.Behavior.BehaviorType == BehaviorType.Coward)
            //        {
            //            movement = Steering.KeepDistance(enemy, Player, 500);
            //        }
            //        else if (enemy.Behavior.BehaviorType == BehaviorType.Passive)
            //        {
            //            enemy.Behavior.Wandering();
            //        }

            //        #endregion
            //    }
            //    else // Wander
            //    {
            //        Character target =
            //            allies.Find(x => x.IsAlive && (enemy.CanHear(x) || enemy.IsInVisionRange(x.Position)));
            //            //gameObjects.Find(delegate(GameObject o) { return (o is Character) && ((o as Character).IsPlayer || (o is Npc && (o as Npc).IsAlly)); }) as Character;

            //        if (target != null)
            //        {
            //            enemy.Target = target;
            //        }
            //        else
            //        {
            //            enemy.Behavior.Wandering();
            //        }
            //    }

            // MOVE CREATURE TO TARGET LOCATION
            // WILL BE OVERRIDEN BY THE FLEEING OR CHASING BEHAVIOR

            if ((enemy.Position - enemy.CurrentTargetPosition).Length() < enemy.CollisionRadius * 5)
            {
                enemy.NewTargetPosition(Game.CurrentMap);
            }

            movement = Steering.Seek(enemy, enemy.CurrentTargetPosition);

            #region Idle

            switch (enemy.Behavior)
            {
                case Enemy.BehaviorType.Fleeing:
                    var fleeMovement = Steering.Flee(enemy, Player, 200);
                    if (fleeMovement != Vector2.Zero)
                    {
                        movement = fleeMovement;
                    }
                    break;
                case Enemy.BehaviorType.Wandering:
                    var softFlee = Steering.Flee(enemy, Player, 150f);
                    if (softFlee != Vector2.Zero)
                        movement = softFlee;
                    break;
            }

            if (movement != Vector2.Zero)
            {
                enemy.Move(movement);
            }
            //else if (enemy.MovementEngine.CurrentWaypoint == null && enemy.ActionState != ActionState.Attacking)
            //{
            //    Log.ScreenLog.Add(string.Format("{0}. {1} going idle", enemy.UniqueObjectId, enemy.Name));

            //    enemy.Idle();
            //    if (enemy.HasTarget)
            //    {
            //        enemy.Face(enemy.Target);
            //    }
            //}

            #endregion
            //}

            //if (enemy.HasTarget && enemy.MeleeReady && enemy.CanAttackCharacter(enemy.Target))
            //{
            //    if (enemy.IsInAttackRange(enemy.Target))
            //    {
            //        enemy.StartAttack();
            //        attackQueue.Add(new MeleeAttack(enemy, enemy.Target, enemy.AttackDelay, null));
            //    }
            //}
        }

        private void KillEnemy(Character enemy, bool byPlayer)
        {
            //var xpFont = Fonts.ArialBlack12;
            if (Player.Target == enemy)
            {
                Player.Target = null;
            }

            enemy.IsAlive = false;

            #region Creep ship

            if (enemy.UniqueObjectId == 1 || enemy.UniqueObjectId == 4)
            {
                if (byPlayer)
                { 
                    GameGraphics.SoundExplosion.Play();
                }

                Game.CurrentMap.Animations.Add(new Animation()
                {
                    Position = enemy.Position,
                    Collision = false,
                    Sprite = new SpriteAnimation(GameGraphics.Explosion1, 200, 4)
                    {
                        IsToggle = true
                    }
                });
            }

            #endregion

            #region BigShip1

            if (enemy.UniqueObjectId == 2)
            {
                if (byPlayer)
                {
                    GameGraphics.SoundExplosionBig.Play();
                }

                Game.CurrentMap.Animations.Add(new Animation()
                {
                    Position = enemy.Position,
                    Collision = false,
                    Sprite = new SpriteAnimation(GameGraphics.Explosion1, 200, 4)
                    {
                        IsToggle = true
                    }
                });
                Game.CurrentMap.Animations.Add(new Animation()
                {
                    Position = enemy.Position + new Vector2(30, 30),
                    Collision = false,
                    Sprite = new SpriteAnimation(GameGraphics.Explosion1, 200, 4)
                    {
                        IsToggle = true
                    }
                });
                Game.CurrentMap.Animations.Add(new Animation()
                {
                    Position = enemy.Position + new Vector2(30, -30),
                    Collision = false,
                    Sprite = new SpriteAnimation(GameGraphics.Explosion1, 200, 4)
                    {
                        IsToggle = true
                    }
                });
                Game.CurrentMap.Animations.Add(new Animation()
                {
                    Position = enemy.Position + new Vector2(-30, -30),
                    Collision = false,
                    Sprite = new SpriteAnimation(GameGraphics.Explosion1, 200, 4)
                    {
                        IsToggle = true
                    }
                });
                Game.CurrentMap.Animations.Add(new Animation()
                {
                    Position = enemy.Position + new Vector2(-30, 30),
                    Collision = false,
                    Sprite = new SpriteAnimation(GameGraphics.Explosion1, 200, 4)
                    {
                        IsToggle = true
                    }
                });

                SpawnHealthKits(2, enemy.Position);
            }

            #endregion
            
            #region Buff Kit
            if (enemy.UniqueObjectId == 3)
            {
                if (byPlayer)
                {
                    GameGraphics.SoundHeal.Play();
                }
            }
            #endregion

            AliveNpcs.FindAll(x => x.Target == enemy).ForEach(x => x.Target = null);

            Player.Target = null;
            enemy.IsAlive = false;
            //enemy.CharSprite.AnimateAction(AnimationType.Death);
        }

        private void CreateGameObjectList()
        {
            gameObjects = new List<GameObject>();
            if (Game.CurrentMap != null)
            {
                gameObjects.AddRange(Game.CurrentMap.MapEnemies);
                gameObjects.AddRange(Game.CurrentMap.MapNpcs);
                gameObjects.AddRange(Game.CurrentMap.MapObjects);
            }

            gameObjects.Add(Player);
        }

        public void SetupPlayer()
        {
            Player.IsPlayer = true;
        }

        public void DrawOnScreenLog(SpriteBatch batch)
        {
            //var position = new Vector2(GameConfig.Config.WindowWidth - 200, 10);
            //int messageCount = Log.ScreenLog.Count > 5 ? 5 : Log.ScreenLog.Count;
            //string lastMessage = string.Empty;
            //for (int i = 1; i < messageCount; i++)
            //{
            //    string currentMessage = Log.ScreenLog[Log.ScreenLog.Count - i];
            //    if (currentMessage != lastMessage)
            //    {
            //        Drawing.DrawText(batch, Fonts.Arial12, currentMessage, position + Camera.Position, Color.White, true);
            //        position.Y += Fonts.Arial12.LineSpacing;
            //        lastMessage = currentMessage;
            //    }
            //    else
            //    {
            //        Log.ScreenLog.RemoveAt(Log.ScreenLog.Count - i);
            //        messageCount--;
            //        i--;
            //    }
            //}
        }

        public override void Load()
        {
            this.Game.NewGame();

            LoadGameObjects();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            CreateGameObjectList();
            SetupPlayer();
            base.Load();
        }

        private void LoadGameObjects()
        {
            SpawnCreepShips(110);
            SpawnCreepShips2(110);
            SpawnBigShips(60);
            SpawnRandomStars(300);
        }

        private void SpawnRandomStars(int amount)
        {
            var stars = new List<Animation>();

            var starColors = new List<Color>
            {
                Color.CornflowerBlue,
                Color.LightCyan,
                Color.LightYellow,
                Color.LightGoldenrodYellow,
            };

            Random r = new Random((int)(DateTime.Now.Millisecond + Game.TimePlayed.TotalMilliseconds));

            var tileList = new List<Vector2>();

            for (int i = 0; i < amount; i++)
            {
                var position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                while (tileList.Contains(position))
                {
                    position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));
                }

                tileList.Add(position);

                var a = new Animation()
                {
                    Name = "Star1",
                    Position = MapHelper.GetPixelsFromTileCenter(position),
                    Sprite = new SpriteAnimation(GameGraphics.Star1, 200, 4)
                    {
                        Color = starColors[r.Next(0, starColors.Count)],
                        IsToggle = false
                    }
                };

                stars.Add(a);
            }

            Game.CurrentMap.Animations.AddRange(stars);
        }

        public override void MouseUp(MouseButton mouseButton)
        {
            if (mouseButton == MouseButton.Left)
            {
                InputManager.MovementVector = Vector2.Zero;

            }
        }

        private void SpawnBigShips(int amount)
        {
            var ships = new List<Enemy>();

            Random r = new Random(DateTime.Now.Millisecond);

            var tileList = new List<Vector2>();

            for (int i = 0; i < amount; i++)
            {
                var position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                while (tileList.Contains(position))
                {
                    position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));
                }

                tileList.Add(position);

                var bigShip = new Enemy()
                {
                    CharSprite = new SpriteAnimation(GameGraphics.BigShip1, 300, 4),
                    Position = MapHelper.GetPixelsFromTileCenter(position),
                    Behavior = Enemy.BehaviorType.Wandering,
                    StepSize = 4,
                    PlayerKillable = true,
                    CollisionRadius = 40,
                    Priority = 3,
                    UniqueObjectId = 2,
                    Name = "BigShip1",
                    Damage = 20,
                    Points = 100,
                    Heal = 0
                };

                bigShip.NewTargetPosition(Game.CurrentMap);

                ships.Add(bigShip);
            }

            if (Game.CurrentMap != null)
            {
                Game.CurrentMap.MapEnemies.AddRange(ships);
            }
        }

        private void SpawnCreepShips(int amount)
        {
            var ships = new List<Enemy>();

            Random r = new Random(DateTime.Now.Millisecond);

            var tileList = new List<Vector2>();

            for (int i = 0; i < amount; i++)
            {
                var position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                while (tileList.Contains(position))
                {
                    position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));
                }

                tileList.Add(position);

                var ship = new Enemy()
                {
                    CharSprite = new SpriteAnimation(GameGraphics.Ship1, 300, 4),
                    Position = MapHelper.GetPixelsFromTileCenter(position),
                    Behavior = Enemy.BehaviorType.Fleeing,
                    StepSize = 3,
                    PlayerKillable = true,
                    CollisionRadius = 20,
                    UniqueObjectId = 1,
                    Name = "Ship1",
                    Points = 10,
                    Damage = 2,
                    Heal = 0
                };

                ship.NewTargetPosition(Game.CurrentMap);

                ships.Add(ship);
            }

            if (Game.CurrentMap != null)
            {
                Game.CurrentMap.MapEnemies.AddRange(ships);
            }
        }

        private void SpawnCreepShips2(int amount)
        {
            var ships = new List<Enemy>();

            Random r = new Random(DateTime.Now.Millisecond);

            var tileList = new List<Vector2>();

            for (int i = 0; i < amount; i++)
            {
                var position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                while (tileList.Contains(position))
                {
                    position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));
                }

                tileList.Add(position);

                var ship = new Enemy()
                {
                    CharSprite = new SpriteAnimation(GameGraphics.Ship2, 300, 4),
                    Position = MapHelper.GetPixelsFromTileCenter(position),
                    Behavior = Enemy.BehaviorType.Fleeing,
                    StepSize = 4,
                    PlayerKillable = true,
                    CollisionRadius = 20,
                    UniqueObjectId = 4,
                    Name = "Ship2",
                    Points = 15,
                    Damage = 3,
                    Heal = 0
                };

                ship.NewTargetPosition(Game.CurrentMap);

                ships.Add(ship);
            }

            if (Game.CurrentMap != null)
            {
                Game.CurrentMap.MapEnemies.AddRange(ships);
            }
        }

        private void SpawnHealthKits(int amount, Vector2 sourcePosition)
        {
            var kits = new List<Enemy>();

            Random r = new Random(DateTime.Now.Millisecond);

            var tileList = new List<Vector2>();

            var spawnRange = 50;

            for (int i = 0; i < amount; i++)
            {
                var position = new Vector2(r.Next((int)sourcePosition.X - spawnRange, (int)sourcePosition.X + spawnRange), r.Next((int)sourcePosition.Y - spawnRange, (int)sourcePosition.Y + spawnRange));

                while (tileList.Contains(position))
                {
                    position = new Vector2(r.Next((int)sourcePosition.X - spawnRange, (int)sourcePosition.X + spawnRange), r.Next((int)sourcePosition.Y - spawnRange, (int)sourcePosition.Y + spawnRange));
                }

                tileList.Add(position);

                var kit = new Enemy()
                {
                    CharSprite = new SpriteAnimation(GameGraphics.HealthKit, 300, 4),
                    Position = position,
                    Behavior = Enemy.BehaviorType.Wandering,
                    StepSize = 2,
                    PlayerKillable = true,
                    CollisionRadius = 20,
                    Priority = 0,
                    UniqueObjectId = 3,
                    Name = "HealthKit",
                    Damage = 0,
                    Points = 5,
                    Heal = 10
                };

                kit.NewTargetPosition(Game.CurrentMap);

                kits.Add(kit);
            }

            Game.CurrentMap?.MapEnemies.AddRange(kits);
        }
    }
}