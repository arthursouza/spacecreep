using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCreep.Client.Lib.AI;
using SpaceCreep.Client.Lib.Graphics;
using SpaceCreep.Client.Lib.Input;
using SpaceCreep.Client.Lib.Objects;
using SpaceCreep.Client.Lib.Sprite;

namespace SpaceCreep.Client.Lib.Scene
{
    public class GameStartedScene : Scene
    {
        public List<dynamic> EnemyTypes = new List<dynamic>
        {
            new
            {
                Id = 1,
                Name = "CreepShip",
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


        private List<GameObject> gameObjects;

        private readonly float trackDelay = 300f;
        private float trackTimer;

        public GameStartedScene(Game game)
        {
            Game = game;
            gameObjects = new List<GameObject>();
        }
        
        private List<Enemy> AliveEnemies
        {
            get { return Game.CurrentMap.MapEnemies.FindAll(x => x.IsAlive); }
        }
        
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                Camera.ScaleMatrix);
            Game.CurrentMap?.Draw(SpriteBatch, Game.CurrentMap.Tileset, true);
            SpriteBatch.End();

            var renderList = new List<GameObject>();
            renderList.AddRange(gameObjects);
            renderList.Sort();

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
                Camera.ScaleMatrix);

            Game.BasicEffect.Projection = Matrix.CreateOrthographicOffCenter(
                0 + Camera.Position.X,
                GameConfig.Config.WindowWidth + Camera.Position.X,
                GameConfig.Config.WindowHeight + Camera.Position.Y,
                0 + Camera.Position.Y,
                0, 1);

            Game.BasicEffect.CurrentTechnique.Passes[0].Apply();

            #region Visual Debug Info

            if (GameConfig.Config.DebugMode)
            {
                //    Game.CurrentMap.CollisionLayer.Draw(spriteBatch);
                renderList.ForEach(x => x.DrawCollisionBounds(SpriteBatch));

                SpriteBatch.Draw(GameGraphics.MovementCrosshair,
                    new Rectangle(
                        (int) (Game.Player.Position.X - GameGraphics.MovementCrosshair.Width / 2),
                        (int) (Game.Player.Position.Y - GameGraphics.MovementCrosshair.Height / 2),
                        GameGraphics.MovementCrosshair.Width, GameGraphics.MovementCrosshair.Height),
                    Color.White);

                if (InputManager.MovementVector != Vector2.Zero)
                    Drawing.DrawLine(Game.GraphicsDevice, Game.Player.Position, Game.Player.Position + InputManager.MovementVector * 100, Color.White);
            }

            #endregion

            Game.CurrentMap?.Animations.ForEach(x => x.Draw(SpriteBatch));

            renderList.ForEach(x => x.Draw(SpriteBatch));
            
            if (GameConfig.Config.DebugMode)
                DrawDebugInfo();

            SpriteBatch.End();

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            DrawResourceBar(new Vector2(30, 30), Game.Player.Hp / Game.Player.MaxHp * 100);

            SpriteBatch.DrawString(Fonts.Arial12, Game.Points.ToString(), new Vector2(30, GameConfig.Config.WindowHeight - 40), Color.LightBlue);

            SpriteBatch.End();
        }

        public override void MouseDown(MouseButton button)
        {
        }

        private void DrawDebugInfo()
        {
            Drawing.DrawText(SpriteBatch, Fonts.Arial12, "Pos:" + Game.Player.Position, Camera.Position + new Vector2(20, 20), Color.White, true);
        }

        private void DrawResourceBar(Vector2 position, float percentage)
        {
            //var background = GameGraphics.CharacterHpBarBg.Bounds;
            var bar = GameGraphics.CharacterHpBar.Bounds;

            var innerOffset = new Vector2(0, 0);
            //var font = Fonts.TrebuchetMS14;

            // Bar background
            var backgroundRect = new Rectangle(
                (int)position.X,
                (int)position.Y,
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
            SpriteBatch.Draw(GameGraphics.CharacterHpBarBg, backgroundRect, Color.White);

            // Draw bar
            SpriteBatch.Draw(GameGraphics.CharacterHpBar, innerRect, innerRectSource, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (!GameConfig.Config.GamePaused)
            {
                UpdateGameObjects();
                UpdatePlayer(gameTime);
                UpdateEnemies(gameTime);
                UpdateAnimations(gameTime);
            }
            
            Camera.LockToTarget(Game.Player.Position, GameConfig.Config.WindowWidth, GameConfig.Config.WindowHeight);

            Camera.Update(gameTime);

            Camera.ClampToArea(
                Game.CurrentMap.WidthInPixels - GameConfig.Config.WindowWidth,
                Game.CurrentMap.HeightInPixels - GameConfig.Config.WindowHeight);
        }

        public override void UpdateKeyboardInput()
        {
        }

        public override void UpdateMouseInput()
        {
            if (!GameConfig.Config.GamePaused)
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
                gameObjects.AddRange(Game.CurrentMap.MapObjects);
            }
            gameObjects.Add(Game.Player);
        }

        private void UpdateAnimations(GameTime gameTime)
        {
            for (var i = 0; i < Game.CurrentMap.Animations.Count; i++)
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
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            Game.Player.Update(gameTime);

            if (Game.Player.Hp <= 0)
                Game.Player.IsAlive = false;

            if (!Game.Player.IsAlive)
                Game.State = GameState.GameOver;

            InputManager.MovementVector = Steering.Seek(Game.Player, InputManager.MouseToMapVector);
            Game.Player.Move(InputManager.MovementVector);

            if (InputManager.MovementVector != Vector2.Zero)
            {
                trackTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

                if (trackTimer >= trackDelay)
                {
                    trackTimer = 0f;
                    Game.CurrentMap.Animations.Add(new Animation
                    {
                        Position = Game.Player.Position + new Vector2(-10, 10),
                        Collision = false,
                        Sprite = new SpriteAnimation(GameGraphics.MonsterTrack, 200, 8) {IsToggle = true}
                    });
                    Game.CurrentMap.Animations.Add(new Animation
                    {
                        Position = Game.Player.Position + new Vector2(10, 10),
                        Collision = false,
                        Sprite = new SpriteAnimation(GameGraphics.MonsterTrack, 200, 8) {IsToggle = true}
                    });
                    Game.CurrentMap.Animations.Add(new Animation
                    {
                        Position = Game.Player.Position + new Vector2(-10, -10),
                        Collision = false,
                        Sprite = new SpriteAnimation(GameGraphics.MonsterTrack, 200, 8) {IsToggle = true}
                    });
                    Game.CurrentMap.Animations.Add(new Animation
                    {
                        Position = Game.Player.Position + new Vector2(10, -10),
                        Collision = false,
                        Sprite = new SpriteAnimation(GameGraphics.MonsterTrack, 200, 8) {IsToggle = true}
                    });
                }
            }

            Steering.EnforcePenetrationConstraint(Game.Player, gameObjects);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            for (var i = 0; i < AliveEnemies.Count; i++)
            {
                var enemy = Game.CurrentMap.MapEnemies[i];
                enemy.Update(gameTime);

                if (enemy.IsAlive)
                {
                    //enemy.Behavior.Update(gameTime, enemy, gameObjects);
                    UpdateEnemyBehavior(enemy);

                    // ATTACK ENEMY
                    if (Game.Player.IsInRange(enemy.Position, enemy.CollisionRadius + Game.Player.CollisionRadius + 10) && enemy.PlayerKillable)
                    {
                        Game.Player.Hp -= enemy.Damage;
                        Game.Player.Hp += enemy.Heal;
                        Game.ChangePoints(enemy.Points);

                        Game.Player.StartAttack();

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
                            if (e.Priority > enemy.Priority)
                                KillEnemy(enemy, false);
                            else
                                KillEnemy(e, false);

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
                    
                    Game.CurrentMap.MapEnemies.RemoveAt(i);
                }
            }
        }

        private void SpawnEnemy(Enemy deadEnemy)
        {
            if (deadEnemy.UniqueObjectId == 1)
                SpawnCreepShips(1);
            else if (deadEnemy.UniqueObjectId == 2)
                SpawnBigShips(1);
            else if (deadEnemy.UniqueObjectId == 4)
                SpawnCreepShips2(1);
        }

        private void UpdateEnemyBehavior(Enemy enemy)
        {
            // MOVE CREATURE TO TARGET LOCATION
            // WILL BE OVERRIDEN BY THE FLEEING OR CHASING BEHAVIOR

            if ((enemy.Position - enemy.CurrentTargetPosition).Length() < enemy.CollisionRadius * 5)
                enemy.NewTargetPosition(Game.CurrentMap);

            var movement = Steering.Seek(enemy, enemy.CurrentTargetPosition);

            #region Idle

            switch (enemy.Behavior)
            {
                case Enemy.BehaviorType.Fleeing:
                    var fleeMovement = Steering.Flee(enemy, Game.Player, 200);
                    if (fleeMovement != Vector2.Zero)
                        movement = fleeMovement;
                    break;
                case Enemy.BehaviorType.Wandering:
                    var softFlee = Steering.Flee(enemy, Game.Player, 150f);
                    if (softFlee != Vector2.Zero)
                        movement = softFlee;
                    break;
            }

            enemy.Move(movement);
            
            #endregion
        }

        private void KillEnemy(Character enemy, bool byPlayer)
        {
            //var xpFont = Fonts.ArialBlack12;
            if (Game.Player.Target == enemy)
                Game.Player.Target = null;

            enemy.IsAlive = false;

            #region Creep ship

            if (enemy.UniqueObjectId == 1 || enemy.UniqueObjectId == 4)
            {
                if (byPlayer)
                    GameGraphics.SoundExplosion.Play();

                Game.CurrentMap.Animations.Add(new Animation
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
                    GameGraphics.SoundExplosionBig.Play();

                Game.CurrentMap.Animations.Add(new Animation
                {
                    Position = enemy.Position,
                    Collision = false,
                    Sprite = new SpriteAnimation(GameGraphics.Explosion1, 200, 4)
                    {
                        IsToggle = true
                    }
                });
                Game.CurrentMap.Animations.Add(new Animation
                {
                    Position = enemy.Position + new Vector2(30, 30),
                    Collision = false,
                    Sprite = new SpriteAnimation(GameGraphics.Explosion1, 200, 4)
                    {
                        IsToggle = true
                    }
                });
                Game.CurrentMap.Animations.Add(new Animation
                {
                    Position = enemy.Position + new Vector2(30, -30),
                    Collision = false,
                    Sprite = new SpriteAnimation(GameGraphics.Explosion1, 200, 4)
                    {
                        IsToggle = true
                    }
                });
                Game.CurrentMap.Animations.Add(new Animation
                {
                    Position = enemy.Position + new Vector2(-30, -30),
                    Collision = false,
                    Sprite = new SpriteAnimation(GameGraphics.Explosion1, 200, 4)
                    {
                        IsToggle = true
                    }
                });
                Game.CurrentMap.Animations.Add(new Animation
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
                if (byPlayer)
                    GameGraphics.SoundHeal.Play();

            #endregion

            Game.Player.Target = null;
            enemy.IsAlive = false;
        }

        private void CreateGameObjectList()
        {
            gameObjects = new List<GameObject>();
            if (Game.CurrentMap != null)
            {
                gameObjects.AddRange(Game.CurrentMap.MapEnemies);
                gameObjects.AddRange(Game.CurrentMap.MapObjects);
            }

            gameObjects.Add(Game.Player);
        }

        private void SetupPlayer()
        {
            Game.Player.IsPlayer = true;
        }
        
        public override void Load()
        {
            Game.NewGame();

            LoadGameObjects();
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
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
                Color.LightGoldenrodYellow
            };

            var r = new Random((int) (DateTime.Now.Millisecond + Game.TimePlayed.TotalMilliseconds));

            var tileList = new List<Vector2>();

            for (var i = 0; i < amount; i++)
            {
                var position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                while (tileList.Contains(position))
                    position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                tileList.Add(position);

                var a = new Animation
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
                InputManager.MovementVector = Vector2.Zero;
        }

        private void SpawnBigShips(int amount)
        {
            var ships = new List<Enemy>();

            var r = new Random(DateTime.Now.Millisecond);

            var tileList = new List<Vector2>();

            for (var i = 0; i < amount; i++)
            {
                var position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                while (tileList.Contains(position))
                    position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                tileList.Add(position);

                var bigShip = new Enemy
                {
                    CharSprite = new SpriteAnimation(GameGraphics.BigShip1, 300, 4),
                    Position = MapHelper.GetPixelsFromTileCenter(position),
                    Behavior = Enemy.BehaviorType.Wandering,
                    InitialSpeed = 4,
                    MaxSpeed = 4,
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
                Game.CurrentMap.MapEnemies.AddRange(ships);
        }

        private void SpawnCreepShips(int amount)
        {
            var ships = new List<Enemy>();

            var r = new Random(DateTime.Now.Millisecond);

            var tileList = new List<Vector2>();

            for (var i = 0; i < amount; i++)
            {
                var position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                while (tileList.Contains(position))
                    position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                tileList.Add(position);

                var ship = new Enemy
                {
                    CharSprite = new SpriteAnimation(GameGraphics.Ship1, 300, 4),
                    Position = MapHelper.GetPixelsFromTileCenter(position),
                    Behavior = Enemy.BehaviorType.Fleeing,
                    InitialSpeed = 3,
                    MaxSpeed = 3,
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
                Game.CurrentMap.MapEnemies.AddRange(ships);
        }

        private void SpawnCreepShips2(int amount)
        {
            var ships = new List<Enemy>();

            var r = new Random(DateTime.Now.Millisecond);

            var tileList = new List<Vector2>();

            for (var i = 0; i < amount; i++)
            {
                var position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                while (tileList.Contains(position))
                    position = new Vector2(r.Next(0, Game.CurrentMap.Width + 1), r.Next(0, Game.CurrentMap.Height + 1));

                tileList.Add(position);

                var ship = new Enemy
                {
                    CharSprite = new SpriteAnimation(GameGraphics.Ship2, 300, 4),
                    Position = MapHelper.GetPixelsFromTileCenter(position),
                    Behavior = Enemy.BehaviorType.Fleeing,
                    InitialSpeed = 4,
                    MaxSpeed = 4,
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
                Game.CurrentMap.MapEnemies.AddRange(ships);
        }

        private void SpawnHealthKits(int amount, Vector2 sourcePosition)
        {
            var kits = new List<Enemy>();

            var r = new Random(DateTime.Now.Millisecond);

            var tileList = new List<Vector2>();

            var spawnRange = 50;

            for (var i = 0; i < amount; i++)
            {
                var position =
                    new Vector2(r.Next((int) sourcePosition.X - spawnRange, (int) sourcePosition.X + spawnRange),
                        r.Next((int) sourcePosition.Y - spawnRange, (int) sourcePosition.Y + spawnRange));

                while (tileList.Contains(position))
                    position = new Vector2(
                        r.Next((int) sourcePosition.X - spawnRange, (int) sourcePosition.X + spawnRange),
                        r.Next((int) sourcePosition.Y - spawnRange, (int) sourcePosition.Y + spawnRange));

                tileList.Add(position);

                var kit = new Enemy
                {
                    CharSprite = new SpriteAnimation(GameGraphics.HealthKit, 300, 4),
                    Position = position,
                    Behavior = Enemy.BehaviorType.Wandering,
                    InitialSpeed = 2,
                    MaxSpeed = 2,
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