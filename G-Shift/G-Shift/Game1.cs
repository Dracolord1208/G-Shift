using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using System.Timers;


namespace G_Shift
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        enum GameState
        {
            StartMenu,
            Loading,
            EndMenu,
            Playing,
            leveledup,
            Paused,
            levelSelect
        }
        // Keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        //menu stuff
        Texture2D levelupTexture;
        Texture2D EndMenuTexture;
        private Texture2D startButton;
        private Texture2D loadingScreen;
        private Thread backgroundThread;
        private Thread levelThread;
        private Thread EndThread;
        //private Vector2 orbPosition;
        private Vector2 startButtonPosition;
        private Vector2 exitButtonPosition;
        private Vector2 resumeButtonPosition;
        // Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;
        Texture2D enemyTexture;
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;
        Random random;
        Texture2D projectileTexture;
        List<Projectile> projectiles;
        List<EnemyProjectile> enemyProjectiles;
        TimeSpan fireTime;
        TimeSpan fireTimeEnemy;
        private GameState gameState;
        MouseState mouseState;
        MouseState previousMouseState;
        SpriteFont font;
        SpriteFont font1;
        Texture2D explosionTexture;
        List<Animation> explosions;
        private bool paused = false;
        private bool pauseKeyDown = false;
        private bool pausedForGuide = false;
        Player gMan;
        bool isLoading;
        bool finishend ;
        bool endbool;
        public bool translate = true;
        public Vector2 translation;
        //Interactable gMan;
        Texture2D gManTest;
        Texture2D gManTexture;
        Texture2D gunATexture;
        Texture2D enemyATexture;
        Texture2D enemy1bTexture;
        Texture2D enemyBTexture;
        Texture2D enemy2bTexture;
        Texture2D enemyCTexture;
        Texture2D bossATexture;

        List<Item> allItems;
        //Item aCrate;
        TimeSpan previouslyRemovedObject;
        TimeSpan removeDestroyedItem;

        World level;

     //  float playerMoveSpeed = 5f;
        const int SCREEN_WIDTH = 1000;
        const int SCREEN_HEIGHT = 600;
        const float LowerBounderyHeight = 150;
        Rectangle healthRectange;
        Rectangle fullHealthRect;
        Rectangle dgs;
        //private Texture2D backgroundTexture;

        //************************

     //   private SpriteFont font;
        //private int score = 0;
       //private int lives = 3;
        Texture2D badge;
        public Texture2D backgroundTexture;
        public Texture2D backgroundTexture2;
        public float scrollPosition = 0;
        public Vector2 backgroundPos;
        public Vector2 backgroundPos2;
        Texture2D baseRectangle;
        Texture2D baseRectangle2;
        public Texture2D far_backgroundTexture;
        public float far_scrollPosition = 0;

        public List<Enemy1a> badGuys;  // enemies
        public List<Rectangle> BadGuys1aRect;
        public TimeSpan badGuyspawnTime;      
        public TimeSpan badGuycheckpoint;     
        public Random badGuyrandom;           

        //public List<Enemy2a> badGuys2;  // enemies
        public List<Enemy2b> badGuys2;
        public List<Rectangle> BadGuys2aRect;
        public TimeSpan badGuy2spawnTime;      
        public TimeSpan badGuy2checkpoint;     
        public Random badGuy2random;           

        public List<Enemy3a> badGuys3;  // enemies
        public TimeSpan badGuy3spawnTime;      
        public TimeSpan badGuy3checkpoint;     
        public Random badGuy3random;           
        public TimeSpan badGuy3jumpTime;
        public TimeSpan badGuy3jumpcheckpoint;
        //public TimeSpan badGuy3decisionTime;
        //public TimeSpan badGuy3lastDecisionTime;

        public List<Enemy4a> badGuys4;  // enemies
        public TimeSpan badGuy4spawnTime;
        public TimeSpan badGuy4checkpoint;
        public Random badGuy4random;

        public Boss1a theBoss1;
        public Vector2 boss1HomePoint;
        public Rectangle boss1HealthBar;
        public Rectangle boss1MaxHealthBar;
        //public TimeSpan theBoss1spawnTime;
        //public TimeSpan theBoss1checkpoint;
        public Random theBoss1random;
        public bool bossFlag;   // if Boss exists or not
        Texture2D exitButton;
        //Vector2 healthPosition;
        public Rectangle gManbase;
        public Rectangle hitbase;
        public Rectangle enemy1Rec;
        public Rectangle enemy2Rec;
        int amountOfFightingEnemies=0;
        LevelSelect levelselectclass;

        public float laserDepth;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true; // default rate of 1/60 sec

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //world declaration
            level = new World(0, Content);

            allItems = new List<Item>();

            removeDestroyedItem = TimeSpan.FromSeconds(1f);
            previouslyRemovedObject = TimeSpan.Zero;

            explosions = new List<Animation>();
            for (int i = 0; i < level.getForUpdate().Count; i++)
            {
                allItems.Add(level.getForUpdate()[i]);
            }
                // Setup window dimensions.
                graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferMultiSampling = false;
            graphics.ApplyChanges();

            // Initialize Gallagher
            gMan = new Player ();
            gMan.Initialize(gManTest, new Vector2(500, 500));
    

            //aCrate = new Item();
            //aCrate.initialize(Content, "crate");
            //aCrate.setItemPosition(new Vector2(250, 300));

            projectiles = new List<Projectile>();
            enemyProjectiles = new List<EnemyProjectile>();
            // Set the laser to fire every quarter second
            fireTime = TimeSpan.FromSeconds(.15f);
            fireTimeEnemy = TimeSpan.FromSeconds(.15f);
            // Set a constant player move speed
            // Initialize the gravies list
            // Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;
            // Used to determine how fast enemy respawns
            enemySpawnTime = TimeSpan.FromSeconds(2.0f);

            // Initialize our random number generator
            random = new Random();

            IsMouseVisible = true;
            //set the gamestate to start menu
         //   gameState = GameState.Playing;
            //get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
           


            badGuys = new List<Enemy1a>();  
            badGuyspawnTime = new TimeSpan();   
            badGuyspawnTime = TimeSpan.FromSeconds(2.0f);  // spawn within 5 seconds
            badGuycheckpoint = new TimeSpan();
            badGuycheckpoint = TimeSpan.FromSeconds(0.0);
            badGuyrandom = new Random();

            //badGuys2 = new List<Enemy2a>();  // maybe new****!!!!
            badGuys2 = new List<Enemy2b>();
            badGuy2spawnTime = new TimeSpan();   // **** NEWLY ADDED ****!!!!
            badGuy2spawnTime = TimeSpan.FromSeconds(4.0f);  // spawn within 5 seconds
            badGuy2checkpoint = new TimeSpan();
            badGuy2checkpoint = TimeSpan.FromSeconds(0.0);
            badGuy2random = new Random();
            amountOfFightingEnemies++;

            
            badGuys3 = new List<Enemy3a>();  // maybe new****!!!!
            badGuy3spawnTime = new TimeSpan();   // **** NEWLY ADDED ****!!!!
            badGuy3spawnTime = TimeSpan.FromSeconds(3.0f);  // spawn within 5 seconds
            badGuy3checkpoint = new TimeSpan();
            badGuy3checkpoint = TimeSpan.FromSeconds(0.0);
            badGuy3random = new Random();

            badGuy3jumpTime = new TimeSpan();
            badGuy3jumpTime = TimeSpan.FromSeconds(2.0f);
            badGuy3jumpcheckpoint = new TimeSpan();

            //badGuy3decisionTime = new TimeSpan();
            //badGuy3decisionTime = TimeSpan.FromSeconds(0.0f);
            //badGuy3lastDecisionTime = new TimeSpan();

            badGuys4 = new List<Enemy4a>();  // maybe new****!!!!
            badGuy4spawnTime = new TimeSpan();   // **** NEWLY ADDED ****!!!!
            badGuy4spawnTime = TimeSpan.FromSeconds(3.0f);  // spawn within 5 seconds
            badGuy4checkpoint = new TimeSpan();
            badGuy4checkpoint = TimeSpan.FromSeconds(0.0);
            badGuy4random = new Random();

            //theBoss1 = new Boss1a();
            theBoss1random = new Random();
            boss1HomePoint = new Vector2(SCREEN_WIDTH*0.95f, SCREEN_HEIGHT*.5f);
            bossFlag = false;
            int resolutionWidth = graphics.GraphicsDevice.Viewport.Width;
            int resolutionHeight = graphics.GraphicsDevice.Viewport.Height;
            levelselectclass = new LevelSelect();
            levelselectclass.Initialize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 265);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 315);
            //set the gamestate to start menu
            gameState = GameState.StartMenu;
            //get the mouse state

            fullHealthRect = new Rectangle(10,
                            10, gMan.maxHealth, 20);

            //boss1MaxHealthBar = new Rectangle(500,10,theBoss1.maxHealth,20);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gMan.LoadContent(Content);
            // TODO: use this.Content to load your game content here
            backgroundTexture = Content.Load<Texture2D>("Map");
            backgroundTexture2 = Content.Load<Texture2D>("backgroundB");
            gManTest=  Content.Load<Texture2D>("gspritesheattest");
            baseRectangle = Content.Load<Texture2D>("Rectangle");
            baseRectangle2 = Content.Load<Texture2D>("RectangleRed");
            //enemy2Rec;
            gManTexture = Content.Load<Texture2D>("gallagher_sprite_12");
            gunATexture = Content.Load<Texture2D>("handGun 2a");
            exitButton = Content.Load<Texture2D>("exit1");
            badge = Content.Load<Texture2D>("US_Park_Police_badge");
            // Load the player resources
            projectileTexture = Content.Load<Texture2D>("laser");
            // Load the score font
            font1 = Content.Load<SpriteFont>("gameFont_2");
            font = Content.Load<SpriteFont>("gameFont");
            // Load the laser and explosion sound effect
            enemyTexture = Content.Load<Texture2D>("mineAnimation");
            levelselectclass.LoadCont(exitButton,font1,badge);
            /*
            enemyATexture = Content.Load<Texture2D>("gunEnemy 1a");
            enemyBTexture = Content.Load<Texture2D>("gunEnemy 2a");
            enemy2bTexture = Content.Load<Texture2D>("gunEnemy 2b");
            enemyCTexture = Content.Load<Texture2D>("gunEnemy 3a");
            */
            explosionTexture = Content.Load<Texture2D>("explosion");

            
            enemyATexture = Content.Load<Texture2D>("smallRobot1a");
            enemy1bTexture = Content.Load<Texture2D>("smallRobot1b");
            enemyBTexture = Content.Load<Texture2D>("mediumRobot1a");
            enemy2bTexture = Content.Load<Texture2D>("mediumRobot1b");
            enemyCTexture = Content.Load<Texture2D>("gunEnemy 3a");
            bossATexture = Content.Load<Texture2D>("Boss1a");

            levelupTexture = Content.Load<Texture2D>("levelupscreen");
            EndMenuTexture = Content.Load<Texture2D>("endMenu");
            startButton = Content.Load<Texture2D>(@"start");
            //exitButton = Content.Load<Texture2D>(@"exit");
            //load the loading screen
            loadingScreen = Content.Load<Texture2D>(@"loading");


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void BeginPause(bool UserInitiated)
        {
            paused = true;
            pausedForGuide = !UserInitiated;
            //TODO: Pause audio playback
            MediaPlayer.Pause();
            //TODO: Pause controller vibration
        }

        private void EndPause()
        {
            //TODO: Resume audio
            MediaPlayer.Resume();
            //TODO: Resume controller vibration
            pausedForGuide = false;
            paused = false;
        }
        private void checkPauseKey(KeyboardState keyboardState,
             GamePadState gamePadState)
        {
            bool pauseKeyDownThisFrame = (keyboardState.IsKeyDown(Keys.Enter) ||
                (gamePadState.Buttons.Start == ButtonState.Pressed));
            // If key was not down before, but is down now, we toggle the
            // pause setting
            if (!pauseKeyDown && pauseKeyDownThisFrame)
            {
                if (gameState == GameState.Playing)
                {
                    if (!paused)
                        BeginPause(true);
                    else
                        EndPause();
                }
            }
            pauseKeyDown = pauseKeyDownThisFrame;
        }
        private void checkPauseGuide()
        {
            // Pause if the Guide is up
            // if (!paused && Guide.IsVisible)
            // BeginPause(false);
            // If we paused for the guide, unpause if the guide
            // went away
            // else if (paused && pausedForGuide && !Guide.IsVisible)
            //  EndPause();
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // Allows the game to exit            
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();
            if (gameState == GameState.Loading && !isLoading) //isLoading bool is to prevent the LoadGame method from being called 60 times a seconds
            {
                //set backgroundthread
                backgroundThread = new Thread(LoadGame);
                isLoading = true;


                //start backgroundthread
                backgroundThread.Start();
            }
            if (gMan.getDirection())
            {
                gManbase = new Rectangle((int)gMan.Position.X - 65, (int)gMan.Position.Y - 30, 150, 50);
                hitbase = new Rectangle((int)gMan.Position.X - 85, (int)gMan.Position.Y - 200, 150, 200);
            }
            else 
            {
                gManbase = new Rectangle((int)gMan.Position.X - 100, (int)gMan.Position.Y - 30, 150, 50);
                hitbase = new Rectangle((int)gMan.Position.X - 65, (int)gMan.Position.Y - 200, 150, 200);
                
            }
            //dgs = new Rectangle((int)gMan.Position.X, (int)gMan.Position.Y, 200, 200);

            healthRectange = new Rectangle(10,
                            10, gMan.Health, 20);





            float moveFactorPerSecond = 400 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            // Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            checkPauseKey(currentKeyboardState, currentGamePadState);
            if (gameState == GameState.Playing)
            {
                // If the user hasn't paused, Update normally
                if (!paused)
                {
                    if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.J) ||
                        currentGamePadState.DPad.Left == ButtonState.Pressed)
                    {
                      //  scrollPosition -= moveFactorPerSecond;
                    }
                    if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.L) ||
                    currentGamePadState.DPad.Right == ButtonState.Pressed)
                    {
                      //  scrollPosition += moveFactorPerSecond;
                    }

                    //      MediaPlayer.Resume();
                    //Update the player
                    //UpdatePlayer(gameTime);

                    // gMan.Update(gameTime,currentKeyboardState, previousKeyboardState,currentGamePadState, aCrate.getUpMove(), aCrate.getDownMove(), level);

                    //gMan.Update(gameTime,currentKeyboardState, previousKeyboardState,currentGamePadState, aCrate.getUpMove(), aCrate.getDownMove(), level);

                    //aCrate.Update(gMan);
                    // Update the gravies
                    //UpdateEnemies(gameTime);
                    // Update the collision
                    UpdateCollision();
                    // Update the projectiles
                    gMan.Update(gameTime, currentKeyboardState, previousKeyboardState, currentGamePadState, allItems, level);

                    for (int i = 0; i < allItems.Count; i++)
                    {
                        allItems[i].Update(gMan, Content, graphics);

                        if (gameTime.TotalGameTime - previouslyRemovedObject > removeDestroyedItem && allItems[i].itemDestroyed())
                        {
                            previouslyRemovedObject = gameTime.TotalGameTime;
                            allItems.RemoveAt(i);
                        }
                    }

                    //aCrate.Update(gMan);
                    // Update the gravies
                    //UpdateEnemies(gameTime);
                    // Update the collision
                    // Update the projectiles
                    // Update the enemy projectiles
                    //UpdateEnemyProjectiles();
                    UpdateExplosions(gameTime);
                    UpdateEnemies(gameTime);

                    //spawnEnemies(gameTime);

                    if (currentKeyboardState.IsKeyDown(Keys.D1))
                    {
                        spawnSmallRobot1(gameTime);
                    }

                    if (currentKeyboardState.IsKeyDown(Keys.D2))
                    {
                        spawnMedRobot1(gameTime);
                    }

                    if (currentKeyboardState.IsKeyDown(Keys.D0))
                    {
                        if(bossFlag == false)
                            spawntheBoss1();
                    }

                    if (currentKeyboardState.IsKeyDown(Keys.Delete))
                    {
                        // boss1 Fire the Laser!!
                        if (bossFlag == true)
                        {
                            theBoss1.laserOn = true;

                            theBoss1.holdPosFlag = true;
                            //theBoss1.laserStartPos = new Vector2(theBoss1.position.X + 150, theBoss1.position.Y + 150);
                            theBoss1.laserCurrentPosX = (int)theBoss1.laserStartPos.X;
                            
                        }
                    }
                }
            
            }
            mouseState = Mouse.GetState();
            if ((previousMouseState.LeftButton == ButtonState.Pressed &&
                mouseState.LeftButton == ButtonState.Released))
            {
                //MediaPlayer.Pause();
                MouseClicked(mouseState.X, mouseState.Y);
            }
            if (gameState == GameState.StartMenu)
            {
                //check the startmenu
                if ((currentKeyboardState.IsKeyDown(Keys.Space) || 
                currentGamePadState.Buttons.A == ButtonState.Pressed)) //player clicked start button
                {
                    gameState = GameState.levelSelect;
                    
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Escape) ||
                currentGamePadState.Buttons.B == ButtonState.Pressed) //player clicked exit button
                {
                    Exit();
                }
            }
            previousMouseState = mouseState;

            if (gameState == GameState.Playing && isLoading)
            {
                LoadGame();
                isLoading = false;
            }

            //if (gameState == GameState.EndMenu && !endbool)
            //{
            //    //   MediaPlayer.Resume();
            //    EndThread = new Thread(Endshow);
            //    endbool = true;
            //    EndThread.Start();

            //}


            if (gameState == GameState.levelSelect)
            {
                levelselectclass.Update(gameTime,currentKeyboardState, previousKeyboardState);
                if (levelselectclass.selected == false)
                {
                    gameState = GameState.Loading;
                    isLoading = false;
                }
            }





            base.Update(gameTime);
        }
        void MouseClicked(int x, int y)
        {
            //creates a rectangle of 10x10 around the place where the mouse was clicked
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //check the startmenu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);
                if ((mouseClickRect.Intersects(startButtonRect))) //player clicked start button
                {
                    gameState = GameState.levelSelect;
                    isLoading = false;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    Exit();
                }
            }
            /*
            //check the pausebutton
            if (gameState == GameState.Playing)
            {
                Rectangle pauseButtonRect = new Rectangle(0, 0, 70, 70);

                if (mouseClickRect.Intersects(pauseButtonRect))
                {
                    gameState = GameState.Paused;
                }
            }

            //check the resumebutton
            if (gameState == GameState.Paused)
            {
                Rectangle resumeButtonRect = new Rectangle((int)resumeButtonPosition.X, (int)resumeButtonPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(resumeButtonRect))
                {
                    gameState = GameState.Playing;
                }
            }*/
        }

        public void UpdateEnemies(GameTime gameTime)
        {
            for (int i = 0; i < badGuys.Count; i++)
            {
                badGuys[i].Update();


                // 3rd try
                if (badGuys[i].position.X <= gMan.Position.X + badGuys[i].Width && badGuys[i].position.X >= gMan.Position.X + gMan.Width)
                    badGuys[i].velocity = new Vector2(0f, badGuys[i].velocity.Y);
                else if (badGuys[i].position.X > gMan.Position.X + badGuys[i].Width)
                    badGuys[i].velocity = new Vector2(-2f, badGuys[i].velocity.Y);
                else if (badGuys[i].position.X < gMan.Position.X + gMan.Width)
                    badGuys[i].velocity = new Vector2(2f, badGuys[i].velocity.Y);


                if (badGuys[i].position.Y > gMan.Position.Y - 10 && badGuys[i].position.Y < gMan.Position.Y + 10)
                    badGuys[i].velocity = new Vector2(badGuys[i].velocity.X, 0f);
                if (badGuys[i].position.Y > gMan.Position.Y + 10)
                    badGuys[i].velocity = new Vector2(badGuys[i].velocity.X, -2f);
                else if (badGuys[i].position.Y < gMan.Position.Y - 10)
                    badGuys[i].velocity = new Vector2(badGuys[i].velocity.X, 2f);


                if (badGuys[i].health <= 0)
                {
                    badGuys.RemoveAt(i);
                    i--;
                    //scream_male.Play();
                }
            }


            //*********************
            // Update badGuys2
            // (check if beaten?)
            //
            for (int i = 0; i < badGuys2.Count; i++)
            {
                if (badGuys2[i].ContentLoadedFlag == false)
                {
                    badGuys2[i].LoadContent(Content);
                    badGuys2[i].ContentLoadedFlag = true;
                }
                //badGuys4[i].Update();
                badGuys2[i].ResetValues();
                

                //************************
                // Complex behavior rules..
                // Summary: Move to specified range away from Player, then pause and gather steam, then charge!
                //
                if (badGuys2[i].stance != Enemy2b.Stance.Charging && badGuys2[i].stance != Enemy2b.Stance.Preparing)
                {

                    if (badGuys2[i].position.X + badGuys2[i].Width * (0.5f) > gMan.Position.X + gMan.Width * (0.5f))
                    {
                        badGuys2[i].isRightFlag = true;
                    }
                    else
                    {
                        badGuys2[i].isRightFlag = false;
                    }

                    if (badGuys2[i].position.X > gMan.Position.X + gMan.Width && badGuys2[i].position.X < gMan.Position.X + gMan.Width + badGuys2[i].Width * 1)
                    {
                        if (badGuys2[i].stance != Enemy2b.Stance.Charging && badGuys2[i].stance != Enemy2b.Stance.Preparing)
                        {
                            badGuys2[i].moveRightFlag = true;
                        }
                    }
                    else if (badGuys2[i].position.X > gMan.Position.X - badGuys2[i].Width * 2 && badGuys2[i].position.X < gMan.Position.X - badGuys2[i].Width * 1)
                    {
                        if (badGuys2[i].stance != Enemy2b.Stance.Charging && badGuys2[i].stance != Enemy2b.Stance.Preparing)
                        {
                            badGuys2[i].moveLeftFlag = true;
                        }
                    }


                    if (badGuys2[i].position.X < gMan.Position.X + gMan.Width + badGuys2[i].Width * 2 && badGuys2[i].position.X > gMan.Position.X + gMan.Width + badGuys2[i].Width * 1)
                    {
                        badGuys2[i].holdxPosFlag = true;
                        //badGuys2[i].stance = G_Shift.Enemy2b.Stance.Preparing;
                    }

                    else if (badGuys2[i].position.X < gMan.Position.X - badGuys2[i].Width * 2 && badGuys2[i].position.X > gMan.Position.X - badGuys2[i].Width * 3)
                    {
                        badGuys2[i].holdxPosFlag = true;
                        //badGuys2[i].stance = G_Shift.Enemy2b.Stance.Preparing;
                    }

                    else if (badGuys2[i].position.X > gMan.Position.X + gMan.Width + badGuys2[i].Width * 2)
                    {
                        //badGuys4[i].velocity = new Vector2(-5f, badGuys4[i].velocity.Y);
                        badGuys2[i].moveLeftFlag = true;
                        badGuys2[i].stance = G_Shift.Enemy2b.Stance.Move;
                    }
                    else if (badGuys2[i].position.X < gMan.Position.X - badGuys2[i].Width * 2)
                    {
                        badGuys2[i].moveRightFlag = true;
                        badGuys2[i].stance = G_Shift.Enemy2b.Stance.Move;
                    }

                    // Adjust Y-directional movement
                    if (badGuys2[i].position.Y + badGuys2[i].Height < gMan.Position.Y + gMan.Height + badGuys2[i].baseHeight && badGuys2[i].position.Y + badGuys2[i].Height > gMan.Position.Y + gMan.Height - badGuys2[i].baseHeight)
                    {
                        badGuys2[i].holdyPosFlag = true;
                    }
                    else if (badGuys2[i].position.Y + badGuys2[i].Height > gMan.Position.Y + gMan.Height + badGuys2[i].baseHeight)
                    {
                        badGuys2[i].moveUpFlag = true;
                    }
                    else if (badGuys2[i].position.Y + badGuys2[i].Height < gMan.Position.Y + gMan.Height - badGuys2[i].baseHeight)
                    {
                        badGuys2[i].moveDownFlag = true;
                    }

                    // go into Preparing stance if holding still
                    if (badGuys2[i].holdxPosFlag == true && badGuys2[i].holdyPosFlag == true)
                    {
                        if (badGuys2[i].attackFlag == false)
                        {
                            badGuys2[i].attackFlag = true;
                            badGuys2[i].stance = G_Shift.Enemy2b.Stance.Preparing;
                            badGuys2[i].attackCheckpoint = gameTime.TotalGameTime;
                        }
                    }
                }

                //if(badGuys2[i].stance == Enemy2b.Stance.Preparing && gameTime.TotalGameTime - badGuys2[i].attackCheckpoint > badGuys2[i].attackTimeSpan)
                if (badGuys2[i].stance == Enemy2b.Stance.Preparing && gameTime.TotalGameTime - badGuys2[i].attackCheckpoint > badGuys2[i].attackTimeSpan)
                {
                    // begin the charge-attack!
                    badGuys2[i].stance = Enemy2b.Stance.Charging;
                    badGuys2[i].ResetValues();
                    badGuys2[i].attackCheckpoint = gameTime.TotalGameTime;
                }
                else if (badGuys2[i].stance == Enemy2b.Stance.Charging && gameTime.TotalGameTime - badGuys2[i].attackCheckpoint > TimeSpan.FromSeconds(1f))
                {
                    // this is executed when the charge-attack completes
                    badGuys2[i].ResetValues();
                    badGuys2[i].stance = Enemy2b.Stance.Wait;
                    badGuys2[i].attackFlag = false;
                }

                if (badGuys2[i].stance == Enemy2b.Stance.Charging && badGuys2[i].isRightFlag == true)
                {

                    badGuys2[i].attackLeftRect = new Rectangle((int)badGuys2[i].position.X, (int)badGuys2[i].position.Y, (int)(badGuys2[i].Width * (.25)), badGuys2[i].Height);
                    //badGuys2[i].attackRightRect = new Rectangle((int)badGuys2[i].position.X + badGuys2[i].Width - (int)(badGuys2[i].Width * (.25)), (int)badGuys2[i].position.Y, (int)(badGuys2[i].Width * (.25)), badGuys2[i].Height);

                    //if (badGuys2[i].position.Y < gMan.Position.Y + 10 && badGuys2[i].position.Y > gMan.Position.Y - 10)
                    if(badGuys2[i].position.Y + badGuys2[i].Height < gMan.Position.Y + gMan.Height + 20 && badGuys2[i].position.Y + badGuys2[i].Height > gMan.Position.Y + gMan.Height - 20)
                    {
                        if (badGuys2[i].attackLeftRect.Intersects(hitbase))
                        {
                            //gMan.Health -= (int)(gMan.maxHealth*(.25f));    // - a quarter health in damage
                            gMan.Health -= 2;
                            //gMan.playerStance = Player.Stance.hurt;
                        }
                    }
                }

                if (badGuys2[i].stance == Enemy2b.Stance.Charging && badGuys2[i].isRightFlag == false)
                {

                    //badGuys2[i].attackLeftRect = new Rectangle((int)badGuys2[i].position.X, (int)badGuys2[i].position.Y, (int)(badGuys2[i].Width * (.25)), badGuys2[i].Height);
                    badGuys2[i].attackRightRect = new Rectangle((int)badGuys2[i].position.X + badGuys2[i].Width - (int)(badGuys2[i].Width * (.25)), (int)badGuys2[i].position.Y, (int)(badGuys2[i].Width * (.25)), badGuys2[i].Height);

                    //if (badGuys2[i].position.Y < gMan.Position.Y + 10 && badGuys2[i].position.Y > gMan.Position.Y - 10)
                    if (badGuys2[i].position.Y + badGuys2[i].Height < gMan.Position.Y + gMan.Height + 20 && badGuys2[i].position.Y + badGuys2[i].Height > gMan.Position.Y + gMan.Height - 20)                    
                    {
                        if (badGuys2[i].attackRightRect.Intersects(hitbase))
                        {
                            //gMan.Health -= (int)(gMan.maxHealth * (.25f));    // - a quarter health in damage
                            gMan.Health -= 2;
                            //gMan.playerStance = Player.Stance.hurt;
                        }
                    }
                }

                badGuys2[i].Update();

                if (badGuys2[i].ttl <= 0 || badGuys2[i].health <= 0)
                {
                    badGuys2.RemoveAt(i);
                    i--;
                }
            }


            //*********************
            // Update badGuys4
            // 
            for (int i = 0; i < badGuys4.Count; i++)
            {
                if (badGuys4[i].ContentLoadedFlag == false)
                {
                    badGuys4[i].LoadContent(Content);
                    badGuys4[i].ContentLoadedFlag = true;
                }
                badGuys4[i].ResetValues();


                if (badGuys4[i].position.X + badGuys4[i].Width * (0.5f) > gMan.Position.X + gMan.Width * (0.5f))
                {
                    badGuys4[i].isRightFlag = true;
                }
                else
                {
                    badGuys4[i].isRightFlag = false;
                }


                // general movement rules

                //*************************
                // Simple movement rules
                //
                if (badGuys4[i].position.X < gMan.Position.X + gMan.Width && badGuys4[i].position.X > gMan.Position.X - (badGuys4[i].Width))
                {
                    badGuys4[i].holdxPosFlag = true;
                }
                else if (badGuys4[i].position.X > gMan.Position.X + gMan.Width)
                {
                    //badGuys4[i].velocity = new Vector2(-5f, badGuys4[i].velocity.Y);
                    badGuys4[i].moveLeftFlag = true;
                }
                else if (badGuys4[i].position.X < gMan.Position.X - badGuys4[i].Width)
                {
                    badGuys4[i].moveRightFlag = true;
                }

                // Adjust Y-directional movement
                if (badGuys4[i].position.Y + badGuys4[i].Height < gMan.Position.Y + gMan.Height + badGuys4[i].baseHeight && badGuys4[i].position.Y + badGuys4[i].Height > gMan.Position.Y + gMan.Height - badGuys4[i].baseHeight)
                {
                    badGuys4[i].holdyPosFlag = true;
                }
                else if (badGuys4[i].position.Y + badGuys4[i].Height > gMan.Position.Y + gMan.Height + badGuys4[i].baseHeight)
                {
                    badGuys4[i].moveUpFlag = true;
                }
                else if (badGuys4[i].position.Y + badGuys4[i].Height < gMan.Position.Y + gMan.Height - badGuys4[i].baseHeight)
                {
                    badGuys4[i].moveDownFlag = true;
                }

                // go into attack stance if holding still
                if (badGuys4[i].holdxPosFlag == true && badGuys4[i].holdyPosFlag == true)
                {
                    badGuys4[i].attackFlag = true;
                }
                if (badGuys4[i].attackFlag == true)
                {
                    if (badGuys4[i].isRightFlag == true)
                    {
                        //attackLeftRect = new Rectangle((int)position.X, (int)position.Y, (int)(Width * (.25)), Height);
                        badGuys4[i].attackLeftRect = new Rectangle((int)badGuys4[i].position.X, (int)badGuys4[i].position.Y, (int)(badGuys4[i].Width * (.25)), badGuys4[i].Height);
                        if (badGuys4[i].attackLeftRect.Intersects(hitbase) && gameTime.TotalGameTime - badGuys4[i].attackCheckpoint > badGuys4[i].attackTimeSpan)
                        {
                            gMan.Health -= 10;
                            AddExplosion(new Vector2(gMan.StartPosition.X, gMan.Position.Y-40));
                            gMan.playerStance = G_Shift.Player.Stance.hurt;
                            badGuys4[i].attackCheckpoint = gameTime.TotalGameTime;
                        }
                    }
                    else
                    {
                        //attackRightRect = new Rectangle((int)(position.X + Width * (.75)), (int)position.Y, (int)(Width * (.25)), Height);
                        badGuys4[i].attackRightRect = new Rectangle((int)(badGuys4[i].position.X + badGuys4[i].Width * (.75)), (int)badGuys4[i].position.Y, (int)(badGuys4[i].Width * (.25)), badGuys4[i].Height);

                        if (badGuys4[i].attackRightRect.Intersects(hitbase) && gameTime.TotalGameTime - badGuys4[i].attackCheckpoint > badGuys4[i].attackTimeSpan)
                        {
                            gMan.Health -= 10;
                          //  AddSmallExplosion(new Vector2( gMan.Position.X-200,gMan.Position.Y-30));
                            AddExplosion(new Vector2(gMan.StartPosition.X, gMan.Position.Y-40));
//                            AddSmallExplosion(badGuys4[i].position);
                            gMan.playerStance = G_Shift.Player.Stance.hurt;
                            //badGuys4[i].stance = G_Shift.Enemy4a.Stance.Attack;
                            badGuys4[i].attackCheckpoint = gameTime.TotalGameTime;
                        }
                    }
                    //gMan.playerStance = G_Shift.Player.Stance.hurt;
                }

                badGuys4[i].Update();

                if (badGuys4[i].ttl <= 0 || badGuys4[i].health <= 0)
                {
                    badGuys4.RemoveAt(i);
                    i--;
                }
            }


            //**************************
            // Update theBoss1
            //
            if (bossFlag == true)
            {
                // load sprite sheets, setup animations
                if (theBoss1.ContentLoadedFlag == false)
                {
                    theBoss1.LoadContent(Content);
                    theBoss1.ContentLoadedFlag = true;
                }

                theBoss1.ResetValues();

                //theBoss1.laserPosX = (int)theBoss1.laserPos.X;

                // fire the laser!
                if (theBoss1.laserOn == true)
                {
                    //if (theBoss1.laserWidth > 4000)
                    if (theBoss1.laserWidth > 6000)
                    {
                        theBoss1.laserOn = false;
                        theBoss1.laserWidth = 0;
                        theBoss1.laserHeight = 20;
                        theBoss1.holdPosFlag = false;
                    }
                    else
                    {
                        theBoss1.holdPosFlag = true;
                        //theBoss1.laserStartPos = new Vector2(theBoss1.position.X + 150, theBoss1.position.Y + 150);
                        //theBoss1.laserStartPos = new Vector2(theBoss1.position.X, theBoss1.position.Y + 150); // good chest height
                        theBoss1.laserStartPos = new Vector2(theBoss1.position.X, theBoss1.position.Y + 200);   // waist height laserBeam
                        //theBoss1.laserPos.X -= 10;
                        theBoss1.laserCurrentPosX -= 50;
                        theBoss1.laserCurrentPos = new Vector2(theBoss1.laserCurrentPosX, theBoss1.position.Y + 150);
                        theBoss1.laserWidth += 50;
                        if (theBoss1.laserWidth > 2000)
                        {
                            //theBoss1.laserStartPos
                            theBoss1.laserCurrentPos = new Vector2(theBoss1.laserCurrentPosX, theBoss1.position.Y + 125);
                            theBoss1.laserStartPos = new Vector2(theBoss1.position.X, theBoss1.position.Y + 125);
                            theBoss1.laserHeight = 150;
                        }
                        theBoss1.laserBeam = new Rectangle((int)theBoss1.laserCurrentPosX, (int)theBoss1.laserStartPos.Y, (int)theBoss1.laserWidth, (int)theBoss1.laserHeight);
                        laserDepth = theBoss1.laserBeam.Bottom * 0.01f;
                    }

                    //if (theBoss1.attackLeftRect.Intersects(hitbase))
                    //if (theBoss1.laserBeam.Intersects(hitbase))   // works!
                    if (theBoss1.laserBeam.Intersects(hitbase))
                    {
                        if (theBoss1.position.Y + theBoss1.Height < gMan.Position.Y + gMan.Height + 50 && theBoss1.position.Y + theBoss1.Height > gMan.Position.Y + gMan.Height - 50)
                        {
                            gMan.Health -= 2;
                        }
                    }
                }

                // Is theBoss1 positiong to the right of the Player? (or left?)
                if (theBoss1.position.X + theBoss1.Width * (0.5f) > gMan.Position.X + gMan.Width * (0.5f))
                {
                    //theBoss1.isRightFlag = true;

                    // time delay before turning around
                    if (gameTime.TotalGameTime - theBoss1.onLeftCheckpoint > theBoss1.turnAroundTime)
                    {
                        theBoss1.isRightFlag = true;
                        //theBoss1.stance = Boss1a.Stance.Move;
                        theBoss1.onRightCheckpoint = gameTime.TotalGameTime;
                    }
                    else
                    {
                        theBoss1.stance = Boss1a.Stance.Wait;
                    }

                    //theBoss1.onRightCheckpoint = gameTime.TotalGameTime;

                    /*
                    if (gameTime.TotalGameTime - theBoss1.turnAroundCheckpoint > theBoss1.turnAroundTime)
                    {
                        theBoss1.isRightFlag = true;
                        theBoss1.turnAroundCheckpoint = gameTime.TotalGameTime;
                    }
                    */
                }
                else
                {
                    //theBoss1.isRightFlag = false;

                    // time delay before turning around
                    if (gameTime.TotalGameTime - theBoss1.onRightCheckpoint > theBoss1.turnAroundTime)
                    {
                        theBoss1.isRightFlag = false;
                        //theBoss1.stance = Boss1a.Stance.Move;
                        theBoss1.onLeftCheckpoint = gameTime.TotalGameTime;
                    }
                    else
                    {
                        theBoss1.stance = Boss1a.Stance.Wait;
                    }
                }


                
                //*************************
                // Boss1 movement rules
                //

                if (theBoss1.stance == Boss1a.Stance.Wait)
                    theBoss1.holdPosFlag = true;

                if (theBoss1.stance == Boss1a.Stance.Wait && gameTime.TotalGameTime - theBoss1.lastDecisionTime > TimeSpan.FromSeconds(1.5f))
                {
                    theBoss1.stance = Boss1a.Stance.Move;
                }                
                

                // if not in Wait mode, do something!
                if (theBoss1.stance != Boss1a.Stance.Wait)
                {
                    // will need to adjust this once Josh incorporates screen locking
                    // return to boss1HomePoint if travelled to far
                    //if (theBoss1.position.X < SCREEN_WIDTH * 0.45f && theBoss1.stance != Boss1a.Stance.Attack)
                    //if (theBoss1.position.X < boss1HomePoint.X - 150 && theBoss1.stance != Boss1a.Stance.Attack)
                    //{
                    //    theBoss1.stance = Boss1a.Stance.ReturnHome;
                    //}

                    if (theBoss1.healthHigh == true)
                    {

                        // Adjust X-directional movement
                        if (theBoss1.holdxPosFlag == false && theBoss1.holdPosFlag == false)
                        {
                            if (theBoss1.position.X > gMan.Position.X + gMan.Width)
                            {
                                //badGuys4[i].velocity = new Vector2(-5f, badGuys4[i].velocity.Y);
                                theBoss1.moveLeftFlag = true;
                            }
                            else if (theBoss1.position.X < gMan.Position.X - theBoss1.Width)
                            {
                                theBoss1.moveRightFlag = true;
                            }
                        }

                        // Adjust Y-directional movement
                        // if close enough to hit.. hold yPos
                        if (theBoss1.position.Y + theBoss1.Height < gMan.Position.Y + gMan.Height + theBoss1.baseHeight && theBoss1.position.Y + theBoss1.Height > gMan.Position.Y + gMan.Height - theBoss1.baseHeight)
                        {
                            theBoss1.holdyPosFlag = true;
                        }
                        else if (theBoss1.position.Y + theBoss1.Height > gMan.Position.Y + gMan.Height + theBoss1.baseHeight)
                        {
                            theBoss1.moveUpFlag = true;
                        }
                        else if (theBoss1.position.Y + theBoss1.Height < gMan.Position.Y + gMan.Height - theBoss1.baseHeight)
                        {
                            theBoss1.moveDownFlag = true;
                        }


                        // go into attack stance if holding still
                        /*
                        if (theBoss1.holdxPosFlag == true && theBoss1.holdyPosFlag == true && gameTime.TotalGameTime - theBoss1.lastAttackTime > theBoss1.minimumAttackTime)
                        {
                            theBoss1.attackFlag = true;
                            theBoss1.stance = Boss1a.Stance.Attack;
                            theBoss1.lastDecisionTime = gameTime.TotalGameTime;
                            theBoss1.lastAttackTime = gameTime.TotalGameTime;
                        }
                        */

                        // go into attack stance if Player is almost in range (within 25 pixels)
                        if (gameTime.TotalGameTime - theBoss1.lastAttackTime > theBoss1.minimumAttackTime && theBoss1.isRightFlag == true)
                            if (theBoss1.position.X - gMan.Position.X < gMan.Width + 25)
                            {
                                theBoss1.attackFlag = true;
                                theBoss1.stance = Boss1a.Stance.Attack;
                                theBoss1.lastDecisionTime = gameTime.TotalGameTime;
                                theBoss1.lastAttackTime = gameTime.TotalGameTime;
                            }
                        if (gameTime.TotalGameTime - theBoss1.lastAttackTime > theBoss1.minimumAttackTime && theBoss1.isRightFlag == false)
                            if (gMan.Position.X - theBoss1.position.X < theBoss1.Width + 25)
                            {
                                theBoss1.attackFlag = true;
                                theBoss1.stance = Boss1a.Stance.Attack;
                                theBoss1.lastDecisionTime = gameTime.TotalGameTime;
                                theBoss1.lastAttackTime = gameTime.TotalGameTime;
                            }

                        //if (theBoss1.attackFlag == true)
                        if (theBoss1.stance == Boss1a.Stance.Attack)
                        {

                            if (theBoss1.isRightFlag == true)
                            {
                                //attackLeftRect = new Rectangle((int)position.X, (int)position.Y, (int)(Width * (.25)), Height);
                                theBoss1.attackLeftRect = new Rectangle((int)theBoss1.position.X + 150, (int)theBoss1.position.Y, (int)(theBoss1.Width * (.25)), (int)(theBoss1.Height * (.75)));
                                //if (theBoss1.attackLeftRect.Intersects(hitbase) && gameTime.TotalGameTime - theBoss1.attackCheckpoint > theBoss1.attackTimeSpan)
                                if (theBoss1.attackLeftRect.Intersects(hitbase))
                                {
                                    gMan.Health -= 1;
                                    gMan.playerStance = G_Shift.Player.Stance.hurt;
                                    //theBoss1.attackCheckpoint = gameTime.TotalGameTime;

                                    theBoss1.holdxPosFlag = true;
                                }
                                else
                                    theBoss1.holdxPosFlag = false;
                            }
                            else
                            {
                                //attackRightRect = new Rectangle((int)(position.X + Width * (.75)), (int)position.Y, (int)(Width * (.25)), Height);
                                theBoss1.attackRightRect = new Rectangle((int)(theBoss1.position.X + theBoss1.Width * (.75)), (int)theBoss1.position.Y, (int)(theBoss1.Width * (.25)) - 150, (int)(theBoss1.Height * (.75)));

                                //if (theBoss1.attackRightRect.Intersects(hitbase) && gameTime.TotalGameTime - theBoss1.attackCheckpoint > theBoss1.attackTimeSpan)
                                if (theBoss1.attackRightRect.Intersects(hitbase))
                                {
                                    gMan.Health -= 1;
                                    gMan.playerStance = G_Shift.Player.Stance.hurt;
                                    //badGuys4[i].stance = G_Shift.Enemy4a.Stance.Attack;
                                    //theBoss1.attackCheckpoint = gameTime.TotalGameTime;
                                }
                            }


                            if (gameTime.TotalGameTime - theBoss1.lastAttackTime > theBoss1.sawBladeAttackTime)
                            {
                                theBoss1.lastAttackTime = gameTime.TotalGameTime;  // end of attack

                                theBoss1.stance = Boss1a.Stance.Wait;
                                theBoss1.lastDecisionTime = gameTime.TotalGameTime;
                            }

                        }
                    }   // end of if(healthHigh == true)

                    else if (theBoss1.healthLow == true)
                    {
                        //boss1HomePoint = new Vector2(gMan.Position.X + 200, gMan.Position.Y);
                        //boss1HomePoint = new Vector2(SCREEN_WIDTH-200, SCREEN_HEIGHT*.5f);
                        boss1HomePoint = new Vector2(SCREEN_WIDTH - 300, SCREEN_HEIGHT * .5f);
                        //boss1HomePoint = new Vector2(800, 350);

                        // Adjust X-directional movement
                        if (theBoss1.holdxPosFlag == false && theBoss1.holdPosFlag == false)
                        {
                            if (theBoss1.position.X > boss1HomePoint.X + 10)
                            {
                                //badGuys4[i].velocity = new Vector2(-5f, badGuys4[i].velocity.Y);
                                theBoss1.moveLeftFlag = true;
                            }
                            else if (theBoss1.position.X < boss1HomePoint.X - 10)
                            {
                                theBoss1.moveRightFlag = true;
                            }
                            else
                            {
                                theBoss1.holdxPosFlag = true;
                            }
                        }

                        // Adjust Y-directional movement
                        // move up and down, firing laser
                        // seems to work, more or less
                        if (theBoss1.position.Y + theBoss1.Height < SCREEN_HEIGHT - 10 && theBoss1.velocityChanged == false)
                        {
                            theBoss1.moveDownFlag = true;
                            theBoss1.moveUpFlag = false;

                            theBoss1.velocityChanged = false;
                        }
                        
                        if (theBoss1.position.Y + theBoss1.Height < SCREEN_HEIGHT - 10 && theBoss1.velocityChanged == true)
                        {
                            theBoss1.moveDownFlag = false;
                            theBoss1.moveUpFlag = true;

                            theBoss1.velocityChanged = true;
                        }

                        if (theBoss1.position.Y + theBoss1.Height > SCREEN_HEIGHT - 10 && theBoss1.velocityChanged == false)
                        {
                            theBoss1.moveDownFlag = false;
                            theBoss1.moveUpFlag = true;

                            theBoss1.velocityChanged = true;
                        }
                        if (theBoss1.position.Y <= 10 && theBoss1.velocityChanged == false)
                        {
                            theBoss1.moveDownFlag = true;
                            theBoss1.moveUpFlag = false;

                            theBoss1.velocityChanged = false;
                        }
                        if (theBoss1.position.Y <= 10 && theBoss1.velocityChanged == true)
                        {
                            theBoss1.moveDownFlag = true;
                            theBoss1.moveUpFlag = false;

                            theBoss1.velocityChanged = false;

                        }
                        
                        /*
                        else if(theBoss1.velocityChanged == true)
                        {
                            theBoss1.velocityChanged = false;
                        }
                        else if (theBoss1.velocityChanged == false)
                        {
                            theBoss1.velocityChanged = true;
                        }
                        */

                        // nothing happens >_<
                        /*
                        if (theBoss1.velocityChanged == false)
                        {
                            theBoss1.velocity = new Vector2(2, 0);
                            theBoss1.velocityChanged = true;
                        }
                        if (theBoss1.position.Y + theBoss1.Height >= SCREEN_HEIGHT - 10)
                        {
                            theBoss1.velocity *= -1;
                        }
                        else if(theBoss1.position.Y <= 0)
                        {
                            theBoss1.velocity *= -1;
                        }
                        */

                        /*
                        if (theBoss1.position.Y + theBoss1.Height >= SCREEN_HEIGHT-10)
                        {
                            theBoss1.moveDownFlag = false;
                            theBoss1.moveUpFlag = true;
                        }
                        else if (theBoss1.position.Y <= 0)
                        {
                            theBoss1.moveUpFlag = false;
                            theBoss1.moveDownFlag = true;
                        }
                        */

                        // laser Attack
                        if (gameTime.TotalGameTime - theBoss1.lastAttackTime > theBoss1.minimumAttackTime && theBoss1.holdxPosFlag == true)
                        {
                            theBoss1.laserOn = true;
                            theBoss1.holdPosFlag = true;
                            theBoss1.laserCurrentPosX = (int)theBoss1.laserStartPos.X;
                            //theBoss1.minimumAttackTime = TimeSpan.FromSeconds((float)theBoss1.randomDecisionTime.Next(1, 4));

                            theBoss1.lastAttackTime = gameTime.TotalGameTime;
                            theBoss1.minimumAttackTime = TimeSpan.FromSeconds((float)theBoss1.randomDecisionTime.Next(4, 7));
                        }

                        /*  // return to homePoint
                        if (theBoss1.position.Y + theBoss1.Height*.5f < boss1HomePoint.Y - 10)
                        {
                            theBoss1.moveDownFlag = true;
                        }
                        else if (theBoss1.position.Y + theBoss1.Height*.5f > boss1HomePoint.Y + 10)
                        {
                            theBoss1.moveUpFlag = true;
                        }
                        else if (theBoss1.position.Y + theBoss1.Height < gMan.Position.Y + gMan.Height - theBoss1.baseHeight)
                        {
                            theBoss1.holdyPosFlag = true;
                        }
                        */

                        // go into attack stance if holding still
                        /*
                        if (theBoss1.holdxPosFlag == true && theBoss1.holdyPosFlag == true && gameTime.TotalGameTime - theBoss1.lastAttackTime > theBoss1.minimumAttackTime)
                        {
                            theBoss1.attackFlag = true;
                            theBoss1.stance = Boss1a.Stance.Attack;
                            theBoss1.lastDecisionTime = gameTime.TotalGameTime;
                            theBoss1.lastAttackTime = gameTime.TotalGameTime;
                        }
                        */

                        /*
                        // go into attack stance if Player is almost in range (within 25 pixels)
                        if (gameTime.TotalGameTime - theBoss1.lastAttackTime > theBoss1.minimumAttackTime && theBoss1.isRightFlag == true)
                            if (theBoss1.position.X - gMan.Position.X < gMan.Width + 25)
                            {
                                theBoss1.attackFlag = true;
                                theBoss1.stance = Boss1a.Stance.Attack;
                                theBoss1.lastDecisionTime = gameTime.TotalGameTime;
                                theBoss1.lastAttackTime = gameTime.TotalGameTime;
                            }
                        if (gameTime.TotalGameTime - theBoss1.lastAttackTime > theBoss1.minimumAttackTime && theBoss1.isRightFlag == false)
                            if (gMan.Position.X - theBoss1.position.X < theBoss1.Width + 25)
                            {
                                theBoss1.attackFlag = true;
                                theBoss1.stance = Boss1a.Stance.Attack;
                                theBoss1.lastDecisionTime = gameTime.TotalGameTime;
                                theBoss1.lastAttackTime = gameTime.TotalGameTime;
                            }

                        //if (theBoss1.attackFlag == true)
                        if (theBoss1.stance == Boss1a.Stance.Attack)
                        {

                            if (theBoss1.isRightFlag == true)
                            {
                                //attackLeftRect = new Rectangle((int)position.X, (int)position.Y, (int)(Width * (.25)), Height);
                                theBoss1.attackLeftRect = new Rectangle((int)theBoss1.position.X + 150, (int)theBoss1.position.Y, (int)(theBoss1.Width * (.25)), (int)(theBoss1.Height * (.75)));
                                //if (theBoss1.attackLeftRect.Intersects(hitbase) && gameTime.TotalGameTime - theBoss1.attackCheckpoint > theBoss1.attackTimeSpan)
                                if (theBoss1.attackLeftRect.Intersects(hitbase))
                                {
                                    gMan.Health -= 1;
                                    gMan.playerStance = G_Shift.Player.Stance.hurt;
                                    //theBoss1.attackCheckpoint = gameTime.TotalGameTime;

                                    theBoss1.holdxPosFlag = true;
                                }
                                else
                                    theBoss1.holdxPosFlag = false;
                            }
                            else
                            {
                                //attackRightRect = new Rectangle((int)(position.X + Width * (.75)), (int)position.Y, (int)(Width * (.25)), Height);
                                theBoss1.attackRightRect = new Rectangle((int)(theBoss1.position.X + theBoss1.Width * (.75)), (int)theBoss1.position.Y, (int)(theBoss1.Width * (.25)) - 150, (int)(theBoss1.Height * (.75)));

                                //if (theBoss1.attackRightRect.Intersects(hitbase) && gameTime.TotalGameTime - theBoss1.attackCheckpoint > theBoss1.attackTimeSpan)
                                if (theBoss1.attackRightRect.Intersects(hitbase))
                                {
                                    gMan.Health -= 1;
                                    gMan.playerStance = G_Shift.Player.Stance.hurt;
                                    //badGuys4[i].stance = G_Shift.Enemy4a.Stance.Attack;
                                    //theBoss1.attackCheckpoint = gameTime.TotalGameTime;
                                }
                            }


                            if (gameTime.TotalGameTime - theBoss1.lastAttackTime > theBoss1.sawBladeAttackTime)
                            {
                                theBoss1.lastAttackTime = gameTime.TotalGameTime;  // end of attack

                                theBoss1.stance = Boss1a.Stance.Wait;
                                theBoss1.lastDecisionTime = gameTime.TotalGameTime;
                            }

                        }
                         */ // removing normal attack while healthLow == true
                    }   // end of if(healthLow == true)

                } // end of:  if (theBoss1.stance != Boss1a.Stance.Wait)


                // prepare to enable.. replace above code
                //************************************************************
                //****************************
                /*
                if (theBoss1.stance == Boss1a.Stance.Wait)
                {
                    theBoss1.holdPosFlag = true;
                }
                else if (theBoss1.stance == Boss1a.Stance.Attack)
                {
                    //theBoss1.holdPosFlag = true;

                    // this will negate the above if from firing again.. 
                    // .. maybe not.. 
                    //theBoss1.holdyPosFlag = false;

                    if (theBoss1.isRightFlag == true)
                    {
                        //attackLeftRect = new Rectangle((int)position.X, (int)position.Y, (int)(Width * (.25)), Height);
                        theBoss1.attackLeftRect = new Rectangle((int)theBoss1.position.X, (int)theBoss1.position.Y, (int)(theBoss1.Width * (.25)), theBoss1.Height);
                        //if (theBoss1.attackLeftRect.Intersects(hitbase) && gameTime.TotalGameTime - theBoss1.attackCheckpoint > theBoss1.attackTimeSpan)
                        if (theBoss1.attackLeftRect.Intersects(hitbase))
                        {
                            gMan.Health -= 10;
                            gMan.playerStance = G_Shift.Player.Stance.hurt;
                            //theBoss1.attackCheckpoint = gameTime.TotalGameTime;
                        }
                    }
                    else
                    {
                        //attackRightRect = new Rectangle((int)(position.X + Width * (.75)), (int)position.Y, (int)(Width * (.25)), Height);
                        theBoss1.attackRightRect = new Rectangle((int)(theBoss1.position.X + theBoss1.Width * (.75)), (int)theBoss1.position.Y, (int)(theBoss1.Width * (.25)), theBoss1.Height);

                        //if (theBoss1.attackRightRect.Intersects(hitbase) && gameTime.TotalGameTime - theBoss1.attackCheckpoint > theBoss1.attackTimeSpan)
                        if (theBoss1.attackRightRect.Intersects(hitbase))
                        {
                            gMan.Health -= 10;
                            gMan.playerStance = G_Shift.Player.Stance.hurt;
                            //badGuys4[i].stance = G_Shift.Enemy4a.Stance.Attack;
                            //theBoss1.attackCheckpoint = gameTime.TotalGameTime;
                        }
                    }


                    if (gameTime.TotalGameTime - theBoss1.lastAttackTime > theBoss1.sawBladeAttackTime)
                    {
                        theBoss1.lastAttackTime = gameTime.TotalGameTime;  // end of attack

                        theBoss1.stance = Boss1a.Stance.Wait;
                        theBoss1.lastDecisionTime = gameTime.TotalGameTime;
                    }
                }
                else if (theBoss1.stance == Boss1a.Stance.Move)
                {
                    // if close enough to hit.. hold xPos
                    if (theBoss1.position.X < gMan.Position.X + gMan.Width && theBoss1.position.X > gMan.Position.X - (theBoss1.Width))
                    {
                        theBoss1.holdxPosFlag = true;
                    }
                    else if (theBoss1.position.X > gMan.Position.X + gMan.Width)
                    {
                        //badGuys4[i].velocity = new Vector2(-5f, badGuys4[i].velocity.Y);
                        theBoss1.moveLeftFlag = true;
                    }
                    else if (theBoss1.position.X < gMan.Position.X - theBoss1.Width)
                    {
                        theBoss1.moveRightFlag = true;
                    }

                    // Adjust Y-directional movement
                    // if close enough to hit.. hold yPos
                    if (theBoss1.position.Y + theBoss1.Height < gMan.Position.Y + gMan.Height + theBoss1.baseHeight && theBoss1.position.Y + theBoss1.Height > gMan.Position.Y + gMan.Height - theBoss1.baseHeight)
                    {
                        theBoss1.holdyPosFlag = true;
                    }
                    else if (theBoss1.position.Y + theBoss1.Height > gMan.Position.Y + gMan.Height + theBoss1.baseHeight)
                    {
                        theBoss1.moveUpFlag = true;
                    }
                    else if (theBoss1.position.Y + theBoss1.Height < gMan.Position.Y + gMan.Height - theBoss1.baseHeight)
                    {
                        theBoss1.moveDownFlag = true;
                    }

                    // go into attack stance if holding still
                    if (theBoss1.holdxPosFlag == true && theBoss1.holdyPosFlag == true && gameTime.TotalGameTime - theBoss1.lastAttackTime > theBoss1.minimumAttackTime)
                    {
                        theBoss1.attackFlag = true;
                        theBoss1.stance = Boss1a.Stance.Attack;
                        theBoss1.lastDecisionTime = gameTime.TotalGameTime;
                        theBoss1.lastAttackTime = gameTime.TotalGameTime;
                    }
                }
                */
                //*******************************************
                //***********************************************************************



                boss1HealthBar = new Rectangle(500, 25, theBoss1.health, 20);


                theBoss1.Update();
                //theBoss1.Update(gameTime);

                if (theBoss1.ttl <= 0 || theBoss1.health <= 0)
                {
                    //theBoss1.Remove();
                    //i--;

                    bossFlag = false;
                }
            }
        }
        private void AddExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
            explosions.Add(explosion);
        }
        private void AddSmallExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, .5f, false);
            explosions.Add(explosion);
        }
        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Active == false)
                {
                    explosions.RemoveAt(i);
                }


            }
        }
        private void UpdateCollision()
        {
            // Use the Rectangle's built-in intersect function to 
            // determine if two objects are overlapping
            Rectangle rectangle1;
            Rectangle rectangle2;

            // use this for collision with enemy attacks; same as gMan.base
            rectangle1 = new Rectangle((int)gMan.Position.X - 100, (int)gMan.Position.Y - 30, 200, 50);

            // Do the collision between the player and the gravies
            for (int i = 0; i < badGuys4.Count; i++)
            {
                rectangle2 = new Rectangle((int)badGuys4[i].position.X + 60,
                (int)badGuys4[i].position.Y + 40,
                badGuys4[i].Width - 90,
                badGuys4[i].Height - 40);
                enemy1Rec = new Rectangle((int)badGuys4[i].position.X+60,
                (int)badGuys4[i].position.Y+40 ,
                badGuys4[i].Width-90,
                badGuys4[i].Height-40);
                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    //the player can hit the enemy
                    if (gMan.playerStance == G_Shift.Player.Stance.heavyAttack)// && badGuys[i].enemyStance == G_Shift.Enemy1a.Stance.Fighting)
                    {
                        //the player hit the robot
                        badGuys4[i].health -= gMan.heavyHit;
                        //badGuys[i].enemyStance = G_Shift.Enemy1a.Stance.Hurt;
                    }
                    // If the player health is less than zero we died
                    if (gMan.Health <= 0)
                        gMan.Active = false;
                }

                if (badGuys4[i].rect.Intersects(allItems[i].itemHitbox()) && allItems[i].itemBeingThrown())
                {
                    badGuys4[i].health = 0;
                }

            }
                // Do the collision between the player and the gravies
                for (int i = 0; i < badGuys2.Count; i++)
                {
                    rectangle2 = new Rectangle((int)badGuys2[i].position.X + 50,
                    (int)badGuys2[i].position.Y + 150,
                    badGuys2[i].Width - 80,
                    badGuys2[i].Height - 160);
                    enemy2Rec = new Rectangle((int)badGuys2[i].position.X+50,
                    (int)badGuys2[i].position.Y+150,
                    badGuys2[i].Width-80,
                    badGuys2[i].Height-160);
                    // Determine if the two objects collided with each
                    // other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        //the player can hit the enemy
                        if (gMan.playerStance == G_Shift.Player.Stance.heavyAttack )//&& badGuys2[i].enemyStance == G_Shift.Enemy1a.Stance.Fighting)
                        {
                            //the player hit the robot
                            badGuys2[i].health -= gMan.heavyHit;
                            //badGuys[i].enemyStance = G_Shift.Enemy1a.Stance.Hurt;
                        }
                        // If the player health is less than zero we died
                    }
<<<<<<< HEAD

                    if (badGuys2[i].rect.Intersects(allItems[i].itemHitbox()) && allItems[i].itemBeingThrown())
                    {
                        badGuys2[i].health = 0;
                    }

                }

                if (bossFlag)
                {
                    for (int i = 0; i < allItems.Count; i++)
                    {
                        if (allItems[i].itemBeingThrown())
                        {
                            if (theBoss1.hitBox.Intersects(allItems[i].itemHitbox()))
                            {
                                theBoss1.health = 0;
                            }
                        }
=======
                }
                //theBoss1.hitBox; //= new Rectangle((int)theBoss1.position.X, (int)theBoss1.position.Y, theBoss1.Width, theBoss1.Height);
                if (bossFlag)
                {
                    if (rectangle1.Intersects(theBoss1.hitBox))
                    {
                        //the player can hit the enemy
                        if (gMan.playerStance == G_Shift.Player.Stance.heavyAttack)//&& badGuys2[i].enemyStance == G_Shift.Enemy1a.Stance.Fighting)
                        {
                            //the player hit the robot
                            theBoss1.health -= gMan.heavyHit;
                            //badGuys[i].enemyStance = G_Shift.Enemy1a.Stance.Hurt;
                        }
                        // If the player health is less than zero we died
>>>>>>> FETCH_HEAD
                    }
                }
        }

        public void spawnEnemies(GameTime gameTime)
        {
            // badGuys spawn on random time interval
            if (gameTime.TotalGameTime - badGuycheckpoint > badGuyspawnTime && badGuys.Count <= 4)
            {
                Vector2 eMotion = new Vector2(-2.5f, 0f);

                float tempX = (float)badGuyrandom.Next(SCREEN_WIDTH, SCREEN_WIDTH + 100);
                float tempY = (float)badGuyrandom.Next(SCREEN_HEIGHT - SCREEN_HEIGHT / 3, SCREEN_HEIGHT - 50);

                Vector2 startPos = new Vector2(tempX, tempY);

                //badGuys.Add(new Enemy1a(72, 72, startPos, eMotion, enemyATexture, 0f, 0f));   // old placehoder size
                badGuys.Add(new Enemy1a(158, 87, startPos, eMotion, enemyATexture, 0f, 0f));
                badGuycheckpoint = gameTime.TotalGameTime;

                badGuyspawnTime = TimeSpan.FromSeconds((float)badGuyrandom.Next(1, 5));
            }

            // badGuys2 spawn on random time interval
            if (gameTime.TotalGameTime - badGuy2checkpoint > badGuy2spawnTime && badGuys2.Count <= 2)
            {
                Vector2 eMotion = new Vector2(-2.5f, 0f);

                float tempX = (float)badGuy2random.Next(SCREEN_WIDTH, SCREEN_WIDTH + 100);
                float tempY = (float)badGuy2random.Next(SCREEN_HEIGHT - SCREEN_HEIGHT / 3, SCREEN_HEIGHT - 50);

                Vector2 startPos = new Vector2(tempX, tempY);

                //badGuys2.Add(new Enemy2a(72, 72, startPos, eMotion, enemyBTexture, 0f, 0f));
                badGuys2.Add(new Enemy2b(173, 207, startPos, eMotion, enemyBTexture, 0f, 0f));
                badGuy2checkpoint = gameTime.TotalGameTime;

                badGuy2spawnTime = TimeSpan.FromSeconds((float)badGuy2random.Next(1, 5));
            }

            // badGuys3 spawn on random time interval
            if (gameTime.TotalGameTime - badGuy3checkpoint > badGuy3spawnTime && badGuys3.Count <= 4)
            {
                Vector2 eMotion = new Vector2(-2.5f, 0f);

                float tempX = (float)badGuy3random.Next(SCREEN_WIDTH, SCREEN_WIDTH + 100);
                float tempY = (float)badGuy3random.Next(SCREEN_HEIGHT - SCREEN_HEIGHT / 3, SCREEN_HEIGHT - 50);

                Vector2 startPos = new Vector2(tempX, tempY);

                //badGuys3.Add(new Enemy3a(72, 72, startPos, eMotion, enemyCTexture, 0f, 0f));
                badGuys3.Add(new Enemy3a(158, 87, startPos, eMotion, enemyATexture, 0f, 0f));
                badGuy3checkpoint = gameTime.TotalGameTime;

                badGuy3spawnTime = TimeSpan.FromSeconds((float)badGuy3random.Next(1, 5));
            }

            // badGuys4 spawn on random time interval
            if (gameTime.TotalGameTime - badGuy4checkpoint > badGuy4spawnTime && badGuys4.Count <= 3)
            {
                Vector2 eMotion = new Vector2(-6f, 0f);

                float tempXright = (float)badGuy4random.Next(SCREEN_WIDTH, SCREEN_WIDTH + 100);
                float tempXleft = (float)badGuy4random.Next(-258, -158);
                //float tempY = (float)badGuy2random.Next(SCREEN_HEIGHT - SCREEN_HEIGHT / 3, SCREEN_HEIGHT - 50);
                float tempY = (float)badGuy2random.Next(0, SCREEN_HEIGHT - 87); // whole screen

                Vector2 startPos = new Vector2(0, 0);
                if (random.Next(1, 3) == 1)
                {
                    startPos = new Vector2(tempXleft, tempY);
                    eMotion = new Vector2(6f, 0f);
                    //badGuys4[i].texture = enemy1bTexture;
                    badGuys4.Add(new Enemy4a(158, 87, startPos, eMotion, enemy1bTexture, 0f, 0f));
                }
                else
                {
                    startPos = new Vector2(tempXright, tempY);
                    eMotion = new Vector2(-6f, 0f);
                    badGuys4.Add(new Enemy4a(158, 87, startPos, eMotion, enemyATexture, 0f, 0f));
                }


                //badGuys4.Add(new Enemy4a(158, 87, startPos, eMotion, enemyATexture, 0f, 0f));
                badGuy4checkpoint = gameTime.TotalGameTime;

                badGuy4spawnTime = TimeSpan.FromSeconds((float)badGuy4random.Next(1, 5));
            }
        }

        public void spawnSmallRobot1(GameTime gameTime)
        {
            // badGuys4 spawn on button press, (ideal for level editor)
            if (gameTime.TotalGameTime - badGuy4checkpoint > badGuy4spawnTime && badGuys4.Count <= 3)
            {
                Vector2 eMotion = new Vector2(-6f, 0f);

                float tempXright = (float)badGuy4random.Next(SCREEN_WIDTH, SCREEN_WIDTH + 100);
                float tempXleft = (float)badGuy4random.Next(-258, -158);
                //float tempY = (float)badGuy2random.Next(SCREEN_HEIGHT - SCREEN_HEIGHT / 3, SCREEN_HEIGHT - 50);
                float tempY = (float)badGuy2random.Next(0, SCREEN_HEIGHT - 87); // whole screen

                //float tempX = (100f);
                float tempX = gMan.Position.X + 500;

                Vector2 startPos = new Vector2(0, 0);
            /*
                if (random.Next(1, 3) == 1)
                {
                    startPos = new Vector2(tempXleft, tempY);
                    eMotion = new Vector2(6f, 0f);
                    //badGuys4[i].texture = enemy1bTexture;
                    badGuys4.Add(new Enemy4a(158, 87, startPos, eMotion, enemy1bTexture, 0f, 0f));
                }
                else
                {
                    startPos = new Vector2(tempXright, tempY);
                    eMotion = new Vector2(-6f, 0f);
                    badGuys4.Add(new Enemy4a(158, 87, startPos, eMotion, enemyATexture, 0f, 0f));
                }
             */

                startPos = new Vector2(tempX, tempY);
                eMotion = new Vector2(0f, 0f);
                badGuys4.Add(new Enemy4a(158, 87, startPos, eMotion, enemyATexture, 0f, 0f));


                //badGuys4.Add(new Enemy4a(158, 87, startPos, eMotion, enemyATexture, 0f, 0f));
                badGuy4checkpoint = gameTime.TotalGameTime;

                //badGuy4spawnTime = TimeSpan.FromSeconds((float)badGuy4random.Next(1, 5));  // not necessary
                badGuy4spawnTime = TimeSpan.FromSeconds(1f);
            }
        }

        public void spawnMedRobot1(GameTime gameTime)
        {
            // badGuys2 spawn on random time interval
            if (gameTime.TotalGameTime - badGuy2checkpoint > badGuy2spawnTime && badGuys2.Count <= 2)
            {
                Vector2 eMotion = new Vector2(0f, 0f);

                //float tempX = (float)badGuy2random.Next(SCREEN_WIDTH, SCREEN_WIDTH + 100);
                float tempX = gMan.Position.X + 500;
                float tempY = (float)badGuy2random.Next(SCREEN_HEIGHT - SCREEN_HEIGHT / 3, SCREEN_HEIGHT - 50);

                Vector2 startPos = new Vector2(tempX, tempY);

                //badGuys2.Add(new Enemy2a(72, 72, startPos, eMotion, enemyBTexture, 0f, 0f));
                badGuys2.Add(new Enemy2b(173, 207, startPos, eMotion, enemyBTexture, 0f, 0f));
                badGuy2checkpoint = gameTime.TotalGameTime;

                //badGuy2spawnTime = TimeSpan.FromSeconds((float)badGuy2random.Next(1, 5));
                badGuy2spawnTime = TimeSpan.FromSeconds(1f);
            }
        }

        public void spawntheBoss1()
        {
            // theBoss1 spawn on button press, (ideal for level editor)
            //if (gameTime.TotalGameTime - badGuy4checkpoint > badGuy4spawnTime && badGuys4.Count <= 3)
            //if(theBoss1..)
            //{
                Vector2 eMotion = new Vector2(0f, 0f);

                //float tempXright = (float)badGuy4random.Next(SCREEN_WIDTH, SCREEN_WIDTH + 100);
                //float tempXright = (float)badGuy4random.Next((int)gMan.scrollPosition + 300, (int)gMan.scrollPosition + 350);
                float tempXleft = (float)badGuy4random.Next(-258, -158);
                //float tempY = (float)badGuy2random.Next(SCREEN_HEIGHT - SCREEN_HEIGHT / 3, SCREEN_HEIGHT - 50);
                //float tempY = (float)badGuy2random.Next(0, SCREEN_HEIGHT - 87); // whole screen
                float tempY = (float) theBoss1random.Next(0, SCREEN_HEIGHT - 446); // whole screen

                //float tempX = (400f);
                //float tempX = gMan.scrollPosition + 100;
                float tempX = gMan.Position.X + 500;

                Vector2 startPos = new Vector2(0, 0);

                startPos = new Vector2(tempX, tempY);
                eMotion = new Vector2(0f, 0f);

                //badGuys4.Add(new Enemy4a(158, 87, startPos, eMotion, enemyATexture, 0f, 0f));
                theBoss1 = new Boss1a(381,446, startPos, eMotion, bossATexture, 0f, 0f);

                bossFlag = true;

                //badGuys4.Add(new Enemy4a(158, 87, startPos, eMotion, enemyATexture, 0f, 0f));
                //theBoss1acheckpoint = gameTime.TotalGameTime;

                //badGuy4spawnTime = TimeSpan.FromSeconds((float)badGuy4random.Next(1, 5));  // not necessary
                //badGuy4spawnTime = TimeSpan.FromSeconds(1f);
            //}

                boss1MaxHealthBar = new Rectangle(500, 25, theBoss1.maxHealth, 20);
                

        }

        void LoadGame()
        {
            //load the game images into the content pipeline
            // orb = Content.Load<Texture2D>(@"orb");
            //pauseButton = Content.Load<Texture2D>(@"pause");
           // resumeButton = Content.Load<Texture2D>(@"resume");
            //resumeButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (resumeButton.Width / 2),
               //                                (GraphicsDevice.Viewport.Height / 2) - (resumeButton.Height / 2));

            //set the position of the orb in the middle of the gamewindow
            // orbPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (OrbWidth / 2), (GraphicsDevice.Viewport.Height / 2) - (OrbHeight / 2));

            //since this will go to fast for this demo's purpose, wait for 3 seconds
            Thread.Sleep(3000);

            //start playing
            gameState = GameState.Playing;
            isLoading = false;
        }

        void Endshow()
        {
            //load the game images into the content pipeline
            // orb = Content.Load<Texture2D>(@"orb");

            //set the position of the orb in the middle of the gamewindow
            // orbPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (OrbWidth / 2), (GraphicsDevice.Viewport.Height / 2) - (OrbHeight / 2));

            //since this will go to fast for this demo's purpose, wait for 3 seconds
            Thread.Sleep(1000);

            if ((currentKeyboardState.IsKeyDown(Keys.Space) ||
                  currentGamePadState.Buttons.X == ButtonState.Pressed)) //player clicked start button
            {
                gameState = GameState.StartMenu;

            }

            //start playing
            finishend = true;
            endbool = false;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //spriteBatch.Draw(background, new Rectangle(0, 0, 1000, 600), Color.White);    // !!WORKING

            //spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            int resolutionWidth = graphics.GraphicsDevice.Viewport.Width;
            int resolutionHeight = graphics.GraphicsDevice.Viewport.Height;

            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Draw(startButton, Vector2.Zero, Color.White);
            }
            if (gameState == GameState.Loading)
            {
                spriteBatch.Draw(loadingScreen, new Vector2((GraphicsDevice.Viewport.Width / 2) - (loadingScreen.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (loadingScreen.Height / 2)), Color.White);
            }



            if (gameState == GameState.Playing)
            {
                spriteBatch.End();

                if(translate)
                    translation = gMan.Position - gMan.StartPosition;
                
                //spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateTranslation(-translation.X, 0, 0));
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateTranslation(-translation.X, 0, 0));
                //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                
                spriteBatch.Draw(backgroundTexture, -backgroundPos, Color.White);
                level.Draw(spriteBatch);

                if (bossFlag == true)
                {
                    //translate = false;

                    if (theBoss1.laserOn == true)
                    {               
                        /*
                        theBoss1.laserStartScreenPos = new Vector2(theBoss1.laserStartPos.X - translation.X, theBoss1.laserStartPos.Y);
                        //theBoss1.laserCurrentPosX = (int)(theBoss1.laserStartPos.X - translation.X);    // may or may not need translation
                        theBoss1.laserCurrentPosX = (int)(theBoss1.laserStartPos.X); 
                        //theBoss1.laserPosX = (int)theBoss1.laserStartPos.X;   // may have to update based on screenPos
                        theBoss1.laserBeam = new Rectangle((int)(theBoss1.laserCurrentPosX - translation.X), (int)theBoss1.laserStartPos.Y, (int)theBoss1.laserWidth, (int)theBoss1.laserHeight);
                        spriteBatch.Draw(baseRectangle, theBoss1.laserBeam, Color.Aquamarine);    
                        */

                        //theBoss1.laserStartScreenPos = new Vector2(theBoss1.laserStartPos.X - translation.X, theBoss1.laserStartPos.Y);
                        //theBoss1.laserCurrentPosX = (int)(theBoss1.laserStartPos.X - translation.X);    // may or may not need translation
                        //theBoss1.laserCurrentPosX = (int)(theBoss1.laserStartPos.X);
                        //theBoss1.laserPosX = (int)theBoss1.laserStartPos.X;   // may have to update based on screenPos
                        //theBoss1.laserBeam = new Rectangle((int)(theBoss1.laserCurrentPosX), (int)theBoss1.laserStartPos.Y, (int)theBoss1.laserWidth, (int)theBoss1.laserHeight);
                        
                        //spriteBatch.Draw(baseRectangle, theBoss1.laserBeam, Color.Aquamarine); // Working!!

                        /*  WORKING!1
                        if(theBoss1.laserBeam.Height < 75)
                            spriteBatch.Draw(baseRectangle, theBoss1.laserBeam, Color.Salmon);
                        else
                            spriteBatch.Draw(baseRectangle, theBoss1.laserBeam, Color.Red);
                        */

                        //spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, color, 0, origin, SpriteEffects.None, (0.1f) * (depth));
                        
                        Rectangle sourceRectangle = new Rectangle(theBoss1.laserBeam.X, theBoss1.laserBeam.Y, 508, 133);
                        Vector2 tempVec = new Vector2(0,0);
                        if (theBoss1.laserBeam.Height < 75)
                            spriteBatch.Draw(baseRectangle, theBoss1.laserCurrentPos, theBoss1.laserBeam, Color.Salmon, 0, tempVec, 1, SpriteEffects.None, (0.1f) * (laserDepth));
                        else
                            spriteBatch.Draw(baseRectangle, theBoss1.laserCurrentPos, theBoss1.laserBeam, Color.Red, 0, tempVec, 1, SpriteEffects.None, (0.1f) * (laserDepth));
                            //spriteBatch.Draw(baseRectangle2, theBoss1.laserBeam, sourceRectangle, Color.Red, 0, tempVec, SpriteEffects.None, (0.1f) * (laserDepth));
                        
                        //spriteBatch.Draw(baseRectangle, theBoss1.laserBeam, Color.Red);
                        //spriteBatch.Draw(baseRectangle, theBoss1.laserCurrentPos, theBoss1.laserBeam, Color.Red, 0, tempVec, 1, SpriteEffects.None, (0.1f) * (laserDepth));
                    }
                }
                else
                    translate = true;
                
                spriteBatch.End();

                //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
               //spriteBatch.Draw(backgroundTexture, hitbase, Color.White);
             //  spriteBatch.Draw(backgroundTexture, gMan.hitBox, Color.White);
               
               

                // draw badGuys2
                for (int i = 0; i < badGuys2.Count; i++)
                {
                    /*
                    if (badGuys2[i].position.Y < gMan.Position.Y)
                    {
                        badGuys2[i].depth = .2f;
                    }
                    else
                    {
                        badGuys2[i].depth = .8f;
                    }
                    */
                    badGuys2[i].screenPosition = new Vector2(badGuys2[i].position.X - translation.X, badGuys2[i].position.Y);
                    badGuys2[i].Draw(spriteBatch);
              //      spriteBatch.Draw(backgroundTexture, enemy2Rec, Color.White);
                }
                // draw badGuys3
                for (int i = 0; i < badGuys3.Count; i++)
                {
                    //badGuys3[i].Draw(spriteBatch);
                }

                gMan.Draw(spriteBatch, .5f);
                for (int i = 0; i < explosions.Count; i++)
                {
                    explosions[i].Draw(spriteBatch,.6f);
                }
                // draw badGuys4
                for (int i = 0; i < badGuys4.Count; i++)
                {
                    badGuys4[i].screenPosition = new Vector2(badGuys4[i].position.X - translation.X, badGuys4[i].position.Y);
                    badGuys4[i].Draw(spriteBatch);
                 //   spriteBatch.Draw(backgroundTexture, enemy1Rec, Color.White);

                    //spriteBatch.Draw(baseRectangle, badGuys4[i].attackLeftRect, Color.Green);   // debug purposes
                    //spriteBatch.Draw(baseRectangle, badGuys4[i].attackRightRect, Color.Green);  // debug purposes
                }

                if (bossFlag == true)
                {
                    theBoss1.screenPosition = new Vector2(theBoss1.position.X - translation.X, theBoss1.position.Y);
                    theBoss1.Draw(spriteBatch);
                }

                Vector2 origin = new Vector2(0, 0);

                Vector2 gpos = new Vector2(fullHealthRect.X, fullHealthRect.Y); // position of gMan health bar
                //spriteBatch.Draw(baseRectangle, fullHealthRect, Color.Red);
                //spriteBatch.Draw(baseRectangle, healthRectange, Color.Green);

                spriteBatch.Draw(baseRectangle, gpos, fullHealthRect, Color.Red, 0, origin, 1, SpriteEffects.None, 0.98f);
                spriteBatch.Draw(baseRectangle, gpos, healthRectange, Color.Green, 0, origin, 1, SpriteEffects.None, 0.99f);


                if (bossFlag == true)
                {
                    // WORKING!!
                    //spriteBatch.Draw(baseRectangle, boss1MaxHealthBar, Color.Red);
                    //spriteBatch.Draw(baseRectangle, boss1HealthBar, Color.Blue);

                    //Vector2 pos = new Vector2(boss1HealthBar.Location.X, boss1HealthBar.Location.Y);
                    Vector2 pos = new Vector2(boss1MaxHealthBar.X, boss1MaxHealthBar.Y);
                    //Vector2 origin = new Vector2(0,0);

                    //Color blue = new Color(0, 0, 255, 0);
                    //Color red = new Color(255, 0, 0, 0);
                    spriteBatch.Draw(baseRectangle2, pos, boss1MaxHealthBar, Color.Red, 0, origin, 1, SpriteEffects.None, 0.98f);
                    spriteBatch.Draw(baseRectangle, pos, boss1HealthBar, Color.Blue, 0, origin, 1, SpriteEffects.None, 0.99f);

                    //spriteBatch.Draw(baseRectangle, boss1MaxHealthBar, boss1MaxHealthBar, Color.Red, 0, origin, SpriteEffects.None, 0.98f);
                    //spriteBatch.Draw(baseRectangle2, boss1HealthBar, boss1HealthBar, Color.Blue, 0, origin, SpriteEffects.None, 0.99f);


                    //spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteEffects.None, 0.1f * depth);

                    //if (theBoss1.laserOn == true)
                    //{
                    //}
                }
                

            }
            if (gameState == GameState.levelSelect)
                levelselectclass.Draw(spriteBatch);

            //spriteBatch.Draw(EnemyTexture, enemy1.rect, Color.White);
            //mainWeapon.Draw(spriteBatch);



            //spriteBatch.Draw(baseRectangle, gMan.hitBox, Color.Red);  // debug purposes
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
