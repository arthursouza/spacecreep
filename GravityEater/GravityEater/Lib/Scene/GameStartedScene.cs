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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GravityEater.Lib.Scene
{
    public class GameStartedScene : Scene
    {
        //private List<FloatingText> floatingText;
        //private List<LootScreenLog> lootLog;

        private int elapsedTime;
        private int framesPerSecond;
        private List<GameObject> gameObjects;
        private int totalFrames;

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
        
        private List<Map.Map> Maps { get; set; }

        public GameStartedScene(Game game)
        {
            Game = game;
            gameObjects = new List<GameObject>();
            Maps = new List<Map.Map>();

            var firstMap = new Map.Map
            {
                Height = 50,
                Width = 50,
                Tileset = new Tileset()
                {
                    Columns = 4,
                    TextureMap = Graphics.GameGraphics.SpaceTextures2,
                    TileSize = 64
                },
                Layers = new List<MapLayer>()
                {
                    new MapLayer(100, 100)
                }
            };

            var textureMap = new int[100,100];

            var rand = new Random(DateTime.Now.Millisecond);
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    textureMap[x, y] = rand.Next(0, 16);
                }
            }

            Game.CurrentMap = firstMap;

            firstMap.Layers[0].Map = textureMap;
        }

        //public override void Load()
        //{
        //    gamePausedMenu = new GamePausedMenu();

        //    //Game.ScreenShadows = new RenderTarget2D(Game.GraphicsDevice, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
        //    //Game.ShadowmapResolver = new ShadowmapResolver(Game.GraphicsDevice, Game.QuadRender, ShadowmapSize.Size256, ShadowmapSize.Size1024);
        //    //Game.ShadowmapResolver.LoadContent(Game.Content);
        //    //lightArea1 = new LightArea(Game.GraphicsDevice, ShadowmapSize.Size512);

        //    if (Game.CurrentSave.MapId == 0)
        //        LoadMap(Game.GameManager.StartupMapId);
        //    else
        //        LoadMap(Game.CurrentSave.MapId);

        //    LoadGameObjects();
        //    spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        //    LoadIngameMenus();
        //    LoadInterfaceInformation();
        //    CreateGameObjectList();
        //    SetupPlayer();
        //    attackQueue = new List<MeleeAttack>();
        //    Game.PlayerQuests = new List<Quest>();
        //}
        
        //public override void MouseDoubleClick(MouseButton button)
        //{
            
        //}

        //public override void MouseDrag(MouseButton button)
        //{
        //    if (GameConfig.Config.GamePaused)
        //    {
        //        foreach (BaseWindow window in menuWindows)
        //        {
        //            if (window.Visible)
        //            {
        //                window.MouseDrag(MouseButton.Left);
        //            }
        //        }
        //    }
        //}

        //public override void MouseClick(MouseButton button)
        //{
        //    if (button == MouseButton.Left)
        //    {
        //        #region Left

        //        if (Game.Paused)
        //        {
        //            foreach (BaseWindow window in menuWindows)
        //            {
        //                if (window.Visible)
        //                {
        //                    if (window.CloseButton.Contains(InputManager.MousePositionPoint))
        //                        ToggleMenuWindow(window);
        //                    else
        //                        window.MouseClick(MouseButton.Left);
        //                }
        //            }

        //            bool interacted = CheckNpcDialogInteraction();

        //            if (gamePausedMenu.Visible)
        //            {
        //                if (gamePausedMenu.SelectedIndex != -1)
        //                {
        //                    if (gamePausedMenu.SelectedOption == GamePausedOption.Continue)
        //                    {
        //                        gamePausedMenu.State = PauseMenuState.Main;
        //                        gamePausedMenu.Visible = false;

        //                        if (!AnyWindowsOnscreen())
        //                        {
        //                            Game.Paused = false;
        //                        }
        //                    }
        //                    else if (gamePausedMenu.SelectedOption == GamePausedOption.Exit)
        //                    {
        //                        if (gamePausedMenu.State == PauseMenuState.Main)
        //                        {
        //                            // go to exit confirmation
        //                            gamePausedMenu.State = PauseMenuState.ConfirmExit;
        //                        }
        //                        else
        //                        {
        //                            gamePausedMenu.State = PauseMenuState.Main;
        //                            gamePausedMenu.Visible = false;
        //                            Game.Paused = false;
        //                            Game.StartTransition(GameState.MainMenu);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        #region Playing

        //        else
        //        {
        //            // Check onscreen hotkeys                
        //            selectedHotkey = hotkeys.FindIndex(x => x.Bounds.Contains(InputManager.MousePositionPoint));

        //            if (currentPreCastSkill != null)
        //            {
        //                CastSkill(Player, currentPreCastSkill);
        //            }

        //            ////TODO: Create projectile - TEST
        //            //if (Player.RangedAttackReady())
        //            //{
        //            //    Player.ShootRangedAttack();
        //            //    Vector2 diff = InputManager.MouseToMapVector - Player.BodyCenter;
        //            //    if (diff != Vector2.Zero)
        //            //        diff.Normalize();

        //            //    Projectile p = new Projectile(Game.Content.Load<Texture2D>("Images/arrow"));
        //            //    p.Sprite.Origin = new Vector2(0, p.Sprite.Height / 2);
        //            //    p.Direction = diff;
        //            //    p.Position = Player.BodyCenter;
        //            //    Projectiles.Add(p);
        //            //}
        //        }

        //        #endregion

        //        #endregion
        //    }
        //    else if (button == MouseButton.Right)
        //    {
        //        #region Right

        //        if (!GameConfig.Config.GamePaused)
        //        {
        //            // If there's a skill already selected, deselect it
        //            if (currentPreCastSkill != null)
        //            {
        //                currentPreCastSkill = null;
        //            }
        //                // If there isnt, check other possibilities
        //            else
        //            {
        //                bool npcInteraction = false;

        //                foreach (Npc npc in Game.CurrentMap.MapNpcs)
        //                {
        //                    if (npc.CollisionBounds.Contains(InputManager.MouseToMapPoint))
        //                    {
        //                        float angle = MapHelper.GetAngleBetweenPoints(Player.Position,
        //                            InputManager.MouseToMapVector);
        //                        if (angle < 0)
        //                            angle += 360;

        //                        Direction dir = MapHelper.GetDirectionFromAngle(angle);
        //                        Player.Turn(dir);
        //                        if (npc.IsInTalkingRange(Player.Position))
        //                        {
        //                            npcInteraction = true;
        //                            StartNpcInteraction(npc);
        //                        }
        //                        break;
        //                    }
        //                }

        //                if (!npcInteraction)
        //                    CheckPlayerMouseAttack();
        //            }
        //        }

        //        #endregion
        //    }
        //}

        //public override void MouseUp(MouseButton mouseButton)
        //{
        //    if (Game.Paused)
        //    {
        //        foreach (BaseWindow window in menuWindows)
        //        {
        //            if (window.Visible)
        //            {
        //                window.MouseUp(MouseButton.Left);
        //            }
        //        }
        //    }
        //}
        
        //public override void MouseScroll()
        //{
        //    if (Game.Paused)
        //    {
        //        foreach (BaseWindow window in menuWindows)
        //        {
        //            if (window.Visible)
        //            {
        //                window.MouseScroll();
        //            }
        //        }
        //    }
        //}

        //#endregion

        //private void LoadInterfaceInformation()
        //{
        //    animations = new List<SpriteAnimation>();
        //    floatingText = new List<FloatingText>();
        //    lootLog = new List<LootScreenLog>();
        //    screenInfo = new OnScreenInfo(Player);

        //    Hud = new Hud(GameGraphics.CharacterHpBar, GameGraphics.CharacterHpBarBg, true);

        //    Hud.InventoryButton = new HudButton();

        //    Hud.InventoryButton.Texture = GameGraphics.InventoryButton1;
        //    Hud.InventoryButton.TextureHover = GameGraphics.InventoryButton1;
        //    Hud.InventoryButton.TexturePressed = GameGraphics.InventoryButton1;

        //    Hud.InventoryButton.Position = new Vector2(0, GameConfig.Config.WindowHeight / 2);

        //    Hud.InventoryButton.Clicked += InventoryButton_Clicked;
        //}

        //void InventoryButton_Clicked(object sender)
        //{
        //    if (!GameConfig.Config.GamePaused)
        //    {
        //        ToggleMenuWindow(inventoryWindow);
        //    }
        //}

        //#region Load Data
        
        //private void LoadGameObjects()
        //{
        //    //SetupPlayer();
        //    LoadEnemies();
        //    LoadNpcs();
        //    gameObjects = new List<GameObject>();
        //    Projectiles = new List<Projectile>();
        //}

        //private void LoadNpcs()
        //{
        //    foreach (Npc npc in Game.CurrentMap.MapNpcs)
        //    {
        //        npc.HealthChanged += CharacterHealthChanged;
        //        npc.Stats.Hp = npc.Stats.MaxHp;
        //        npc.IsAlive = true;
        //    }
        //}

        //private void LoadIngameMenus()
        //{
        //    inputConfigWindow = new InputConfigurationWindow();
        //    debugWindow = new DebugWindow(Game);
        //    statsWindow = new StatsWindow(Game.Player);
        //    inventoryWindow = new InventoryWindow(Game.Player);
        //    inventoryWindow.ItemSlotBackground = GameGraphics.ItemSlotBackground;
        //    questWindow = new QuestWindow(new List<Quest>());
        //    menuWindows = new List<BaseWindow>();
        //    menuWindows.Add(inputConfigWindow);
        //    menuWindows.Add(questWindow);
        //    menuWindows.Add(inventoryWindow);
        //    menuWindows.Add(statsWindow);
        //    menuWindows.Add(debugWindow);
        //}

        //private void LoadHotkeys()
        //{
        //    Hotkey.Width = 50;
        //    Hotkey.Height = 50;
        //    hotkeys = new List<Hotkey>();
        //    List<Skill> activeSkill = Player.AvailableSkills.FindAll(x => x.Type == SkillType.Active);
        //    for (int i = 0; i < activeSkill.Count; i++)
        //    {
        //        Skill s = activeSkill[i];
        //        var hotkey = new Hotkey(s, Vector2.Zero);
        //        hotkeys.Add(hotkey);
        //    }
        //}

        //private void LoadEnemies()
        //{
        //    foreach (Enemy e in Game.CurrentMap.MapEnemies)
        //    {
        //        e.HealthChanged += CharacterHealthChanged;
        //        e.Stats.Hp = e.Stats.MaxHp;
        //        e.IsAlive = true;
        //    }
        //}

        //private void LoadMapContent()
        //{
        //    Game.CurrentMap.Tileset.TextureMap =
        //        Game.Content.Load<Texture2D>("Textures/Tilesets/" + Game.CurrentMap.Tileset.Name);

        //    foreach (MapObject obj in Game.CurrentMap.MapObjects)
        //    {
        //        if (!string.IsNullOrEmpty(obj.SpriteInformation.Name))
        //        {
        //            var texture = Game.Content.Load<Texture2D>("Sprites/Objects/" + obj.SpriteInformation.Name);
        //            obj.Animation = new SpriteAnimation(texture, 100, 1);
        //            obj.Animation.Origin = obj.Origin;
        //        }
        //    }

        //    foreach (Enemy enemy in Game.CurrentMap.MapEnemies)
        //    {
        //        if (enemy.SpriteSet != null)
        //        {
        //            for (int i = 0; i < enemy.Skills.Count; i++)
        //            {
        //                enemy.Skills[i] = Skill.AllSkills.Find(x => x.Id == enemy.Skills[i].Id).Clone();
        //            }
        //            string set = enemy.SpriteSet.Name;
        //            enemy.CharSprite = new CharacterSprite(Game.SpriteCollection[set]);
        //            foreach (Loot item in enemy.Loot.LootItems)
        //            {
        //                item.Item = Item.AllItems.Find(x => x.Id == item.Id);
        //            }
        //        }
        //        enemy.MovementEngine = new MovementEngine(Game.CurrentMap.CollisionLayer, 10);
        //    }

        //    foreach (Event evt in Game.CurrentMap.MapEvents)
        //    {
        //        if (evt.Action == EventAction.TeleportCharacter)
        //        {
        //            evt.Animation = new SpriteAnimation(GameGraphics.GlowingFloor, 100, 4);
        //        }
        //    }

        //    foreach (Npc npc in Game.CurrentMap.MapNpcs)
        //    {
        //        if (npc.SpriteSet != null)
        //        {
        //            string set = npc.SpriteSet.Name;
        //            npc.CharSprite = new CharacterSprite(Game.SpriteCollection[set]);
        //        }
        //        npc.MovementEngine = new MovementEngine(Game.CurrentMap.CollisionLayer, 10);
        //    }
        //}

        //private void LoadMap(int map)
        //{
        //    //Character.Save(Player);
        //    Game.CurrentMap = Game.GameManager.Maps.Find(m => m.Id == map);
        //    Game.CurrentMap.MapEnemies = Map.Load(map).MapEnemies;
        //    LoadMapContent();
        //    LoadEnemies();
        //}

        //#endregion

        //private bool CheckNpcDialogInteraction()
        //{
        //    foreach (Dialog item in dialogs)
        //    {
        //        if (item.Bounds.Contains(InputManager.MouseToMapPoint))
        //        {
        //            if (item is NpcDialog)
        //            {
        //                var dialog = item as NpcDialog;

        //                foreach (NpcDialogOption option in dialog.Options)
        //                {
        //                    if (option.Bounds.Contains(InputManager.MouseToMapPoint))
        //                    {
        //                        dialog.Close();
        //                        ProcessNpcDialogResponse(dialog.Owner, dialog, option);
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
        
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.ScaleMatrix);

            Game.CurrentMap?.Draw(spriteBatch, Game.CurrentMap.Tileset, true);

            spriteBatch.End();

            var renderList = new List<GameObject>();
            renderList.AddRange(gameObjects);
            renderList.Sort();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.ScaleMatrix);

            //DrawFps(gameTime);
            Game.BasicEffect.Projection = Matrix.CreateOrthographicOffCenter(
                0 + Camera.Position.X,
                GameConfig.Config.WindowWidth + Camera.Position.X,
                GameConfig.Config.WindowHeight + Camera.Position.Y,
                0 + Camera.Position.Y,
                0, 1);
            Game.BasicEffect.CurrentTechnique.Passes[0].Apply();

            //if (GameConfig.Config.ShowPathFinding)
            //{
            //    Game.CurrentMap.CollisionLayer.Draw(spriteBatch);
                    renderList.ForEach(x => x.DrawCollisionBounds(spriteBatch));
            //}

            renderList.ForEach(x => x.Draw(spriteBatch));
            //animations.ForEach(x => x.Draw(spriteBatch));

            //Game.CurrentMap.Draw(spriteBatch, Game.CurrentMap.Tileset, true);

            //DrawFloatingText();
            //DrawDialogs();

            //if (GameConfig.Config.DebugMode)
                DrawDebugInfo();

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //DrawInterface();
            spriteBatch.End();
        }

        //private void DrawFps(GameTime gameTime)
        //{
        //    if (GameConfig.Config.DebugMode)
        //    {
        //        elapsedTime += (int) gameTime.ElapsedGameTime.TotalMilliseconds;
        //        totalFrames++;

        //        if (elapsedTime >= 1000)
        //        {
        //            framesPerSecond = totalFrames;
        //            totalFrames = 0;
        //            elapsedTime = 0;
        //        }

        //        spriteBatch.DrawString(Fonts.Verdana16, framesPerSecond.ToString(),
        //            new Vector2(20, 20) + Camera.Position, Color.White);
        //    }
        //}

        //private void DrawInterface()
        //{
        //    if (!Game.Paused)
        //    {
        //        DrawHotkeys();
        //        screenInfo.Draw(spriteBatch);

        //        DrawHud();
        //    }

        //    foreach (BaseWindow window in menuWindows)
        //    {
        //        if (window.Visible)
        //        {
        //            window.Draw(spriteBatch);
        //        }
        //    }

        //    if (gamePausedMenu.Visible)
        //    {
        //        spriteBatch.Draw(GameGraphics.GamePaused,
        //            new Rectangle(0, 0, GameConfig.Config.WindowWidth, GameConfig.Config.WindowHeight), Color.White);
        //        gamePausedMenu.Draw(spriteBatch);
        //    }
        //}

        //private void DrawHud()
        //{
        //    Hud.Draw(spriteBatch);
        //}
        
        //private void DrawFloatingText()
        //{
        //    for (int i = 0; i < floatingText.Count; i++)
        //    {
        //        if (!floatingText[i].Animating && floatingText[i].Currframe == 0)
        //        {
        //            floatingText[i].Animating = true;
        //        }
        //        else if (floatingText[i].Currframe >= floatingText[i].Frames)
        //        {
        //            floatingText.RemoveAt(i);
        //        }
        //        else
        //        {
        //            floatingText[i].Draw(spriteBatch);
        //        }
        //    }

        //    if (lootLog.Count > 0)
        //    {
        //        LootScreenLog log = lootLog[0];
        //        if (!log.Animating && log.Currframe == 0)
        //        {
        //            log.Animating = true;
        //        }
        //        else if (log.Currframe >= log.Frames)
        //        {
        //            lootLog.RemoveAt(0);
        //        }
        //        else
        //        {
        //            log.Draw(spriteBatch);
        //        }
        //    }
        //}

        //private void DrawDialogs()
        //{
        //    dialogs.RemoveAll(x => !x.Visible);
        //    dialogs.ForEach(x => x.Draw(spriteBatch));
        //}

        private void DrawDebugInfo()
        {
            Drawing.DrawText(spriteBatch, Fonts.Arial12, "Pos:" + Player.Position.ToString(), Camera.Position + new Vector2(20, 20), Color.White, true);
            //Drawing.DrawText(spriteBatch, Fonts.Arial12, "Action:" + Player.ActionState.ToString(), Camera.Position + new Vector2(20, 20), Color.White, true);
            //Drawing.DrawText(spriteBatch, Fonts.Arial12, "Animation:" + Player.CharSprite.CurrentAnimationType.ToString(), Camera.Position + new Vector2(20, 40), Color.White, true);
            DrawOnScreenLog(spriteBatch);
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
            InputManager.MovementInput = Vector2.Zero;

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
                InputManager.MovementInput = InputManager.MovementVector;
                
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
                gameObjects.AddRange(Game.CurrentMap.Animations);
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

            if (!Player.IsAlive)
            {
                Game.State = GameState.GameOver;
            }

            var nearbyEnemies = AliveEnemies.Where(e => Player.IsInRange(e.Position, 500));

            if (nearbyEnemies.Any())
            {
                Player.Move(Steering.Seek(Player, nearbyEnemies.FirstOrDefault().Position));
            }

            if (InputManager.MovementInput != Vector2.Zero)
            {
                Player.Move(InputManager.MovementInput);
            }

            Steering.EnforcePenetrationConstraint(Player, gameObjects);
            //Player.ClampToArea(Game.CurrentMap.WidthInPixels, Game.CurrentMap.HeightInPixels);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            for (int i = 0; i < Game.CurrentMap.MapEnemies.Count; i++)
            {
                Enemy enemy = Game.CurrentMap.MapEnemies[i];
                enemy.Update(gameTime);

                if (enemy.IsAlive)
                {
                    //enemy.Behavior.Update(gameTime, enemy, gameObjects);
                    UpdateEnemyBehavior(enemy);

                    if (Player.IsInRange(enemy.Position, enemy.CollisionRadius + Player.CollisionRadius + 10) && enemy.PlayerKillable)
                    {
                        enemy.IsAlive = false;
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

                    Steering.EnforcePenetrationConstraint(enemy, gameObjects);
                    enemy.ClampToArea(Game.CurrentMap.WidthInPixels, Game.CurrentMap.HeightInPixels);
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
                        var fleeMovement = Steering.Flee(enemy, Player);
                        if (fleeMovement != Vector2.Zero)
                        {
                            movement = fleeMovement;
                        }
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

        private void UpdateNpcs(GameTime gameTime)
        {
            foreach (Npc npc in Game.CurrentMap.MapNpcs)
            {
                npc.Update(gameTime);
                Vector2 movement = Vector2.Zero;
                //if (npc.IsAlive)
                //{
                //    npc.Behavior.Update(gameTime, npc, gameObjects);
                //    if (npc.IsAlly)
                //    {
                //        if (npc.Target == null)
                //        {
                //            List<Enemy> targets = AliveEnemies.FindAll(en => npc.CanHear(en));
                //            if (targets.Count > 0)
                //                npc.Target = targets[0];
                //        }
                //        else if (npc.HasTarget && !npc.IsInAttackRange(npc.Target))
                //        {
                //            movement = Steering.Seek(npc, npc.Target, npc.CurrentWeapon.AttackRange);
                //            npc.Move(movement);
                //        }

                //        if (npc.HasTarget && npc.MeleeReady && npc.CanAttackCharacter(npc.Target))
                //        {
                //            if (npc.IsInAttackRange(npc.Target))
                //            {
                //                npc.StartAttack();
                //                attackQueue.Add(new MeleeAttack(npc, npc.Target, npc.AttackDelay, null));
                //            }
                //        }
                //    }

                //    if (!npc.HasTarget && npc.Wander)
                //    {
                //        npc.Behavior.Wandering();
                //    }

                //    if (npc.MovementEngine.CurrentWaypoint == null && movement == Vector2.Zero &&
                //        npc.ActionState != ActionState.Attacking)
                //        npc.Idle();
                //}
            }
        }

        //private void CharacterHealthChanged(AttackEventArgs attack)
        //{
        //    SpriteFont font = Fonts.ArialBlack14;
        //    Vector2 textSize = font.MeasureString(attack.Damage.ToString());

        //    if (attack.Damage != 0)
        //    {
        //        if (attack.Damage > 0)
        //        {
        //            floatingText.Add(new TextAnimation(attack.Damage.ToString(), attack.Target.Position, font,
        //                Color.LightGreen));
        //        }
        //        else
        //        {
        //            if (attack.CriticalHit)
        //            {
        //                Camera.Shake(4, .3f);
        //                floatingText.Add(new TextAnimation((-attack.Damage) + "!", attack.Target.Position, font,
        //                    Color.Red));
        //            }
        //            else
        //            {
        //                floatingText.Add(new TextAnimation((-attack.Damage).ToString(), attack.Target.Position, font,
        //                    Color.Red));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        textSize = Fonts.ArialBlack14.MeasureString(LanguageConfig.Config.BlockedText);
        //        floatingText.Add(new TextAnimation(LanguageConfig.Config.BlockedText, attack.Target.Position, font,
        //            Color.LightBlue));
        //    }

        //    if (attack.Target.Stats.Hp <= 0 && attack.Target.IsAlive)
        //    {
        //        if (attack.Target.IsPlayer)
        //        {
        //            KillPlayer();
        //        }
        //        else if (attack.Target is Npc)
        //        {
        //            KillAlly(attack.Target);
        //            attack.Attacker.Target = null;
        //        }
        //        else if (attack.Target is Enemy)
        //        {
        //            KillEnemy(attack.Target, attack.Attacker.IsPlayer);
        //        }
        //    }
        //}
        
        private void KillPlayer()
        {
            Player.IsAlive = false;
            //Player.CharSprite.AnimateAction(AnimationType.Death);
            //Game.GameStats.TimesKilled++;
            Game.State = GameState.GameOver;
        }

        private void KillEnemy(Character enemy, bool byPlayer)
        {
            //var xpFont = Fonts.ArialBlack12;

            if (byPlayer)
            {
            //    #region user stats

            //    if (!Game.GameStats.EnemiesKilled.ContainsKey(enemy.Id))
            //        Game.GameStats.EnemiesKilled.Add(enemy.Id, 0);
            //    Game.GameStats.EnemiesKilled[enemy.Id]++;

            //    #endregion

            //    floatingText.Add(new TextAnimation(enemy.Stats.Experience + "exp", Player.Position, xpFont, Color.LightGray));
                
            //    Player.AddExperience(enemy.Stats.Experience);
                
            //    if (enemy is Enemy)
            //        LootEnemy(enemy);
                
            //    List<Quest> results =
            //        Game.PlayerQuests.FindAll(e => e.Goal.Type == QuestGoalType.KillCreatures && e.Goal.GoalObjectId == enemy.Id);

            //    if (results.Count > 0)
            //        results.ForEach(x => x.Goal.CurrentCount++);
            }

            AliveNpcs.FindAll(x => x.Target == enemy).ForEach(x => x.Target = null);

            Player.Target = null;
            enemy.IsAlive = false;
            //enemy.CharSprite.AnimateAction(AnimationType.Death);
        }
        
        private void TeleportPlayer(int mapId, Vector2 destination)
        {
            Player.Target = null;
            //LoadMap(mapId);
            CreateGameObjectList();
            //Game.SaveGame();

            Player.Position = MapHelper.GetPixelsFromTileCenter(destination);
            //Player.MovementEngine = new MovementEngine(Game.CurrentMap.CollisionLayer, 10);
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
            
            //Player.HealthChanged += CharacterHealthChanged;
            //Player.LevelChanged += PlayerLevelChanged;

            //if (Game.CurrentSave.Position == Vector2.Zero)
            //{
            //    Player.Position = MapHelper.GetPixelsFromTileCenter(Game.GameManager.StartupMapPosition);
            //}
            //else
            //{
            //    Player.Position = Game.CurrentSave.Position;
            //}

            //Player.MovementEngine = new MovementEngine(Game.CurrentMap.CollisionLayer, 10);

            //LoadHotkeys();
            //if (Player.Inventory.Count == 0)
            //{
            //    //Cheats.AddAllItems(Player);
            //    //Cheats.AddAllItems(Player);
            //    //Cheats.AddAllItems(Player);
            //}
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
            LoadGameObjects();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            //LoadIngameMenus();
            //LoadInterfaceInformation();
            CreateGameObjectList();
            SetupPlayer();
            //attackQueue = new List<MeleeAttack>();
            //Game.PlayerQuests = new List<Quest>();
            base.Load();
        }

        private void LoadGameObjects()
        {
            SpawnCreepShips();
            SpawnBigShips();
        }

        private void SpawnBigShips()
        {
            
        }

        private void SpawnCreepShips()
        {
            var ships = new List<Enemy>();

            Random r = new Random(DateTime.Now.Millisecond);

            var tileList = new List<Vector2>();

            for (int i = 0; i < 500; i++)
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
                    CollisionRadius = 20
                };

                ship.NewTargetPosition(Game.CurrentMap);

                ships.Add(ship);
            }

            if (Game.CurrentMap != null)
            {
                Game.CurrentMap.MapEnemies.AddRange(ships);
            }
        }
    }
}