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

// Test comment [by: Antonio]

namespace G_Shift
{
    //public class gMan : Interactable { }
    //public class Gun : Interactable { }

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
            Paused
        }
        // Keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        // Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;
        Texture2D enemyTexture;
        List<enemy> enemies;
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;
        Random random;
        Texture2D projectileTexture;
        List<Projectile> projectiles;
        List<EnemyProjectile> enemyProjectiles;
        TimeSpan fireTime;
        TimeSpan previousFireTime;
        TimeSpan fireTimeEnemy;
        TimeSpan previousFireTimeEnemy;
        private GameState gameState;
        private Thread backgroundThread;
        private bool isLoading = false;
        MouseState mouseState;
        MouseState previousMouseState;
        SpriteFont font;
        private bool paused = false;
        private bool pauseKeyDown = false;
        private bool pausedForGuide = false;
        Player gMan;
        //Interactable gMan;
        Interactable handGunA;
        Texture2D gManTest;
        Texture2D gManTexture;
        Texture2D gunATexture;
        Texture2D enemyATexture;
        Texture2D enemy1bTexture;
        Texture2D enemyBTexture;
        Texture2D enemy2bTexture;
        Texture2D enemyCTexture;
        Texture2D bulletATexture;

        Item aCrate;

        World level;

        float playerMoveSpeed = 5f;
        const int SCREEN_WIDTH = 1000;
        const int SCREEN_HEIGHT = 600;
        const float LowerBounderyHeight = 150;

        private float gunAngle;
        private Vector2 gunOrigin;

        //private Texture2D backgroundTexture;

        //************************

     //   private SpriteFont font;
        private int score = 0;
        private int lives = 3;

        public Texture2D backgroundTexture;
        public Texture2D backgroundTexture2;
        public float scrollPosition = 0;
        public Vector2 backgroundPos;
        public Vector2 backgroundPos2;

        public Texture2D far_backgroundTexture;
        public float far_scrollPosition = 0;

        public List<Enemy1a> badGuys;  // enemies
        public TimeSpan badGuyspawnTime;      
        public TimeSpan badGuycheckpoint;     
        public Random badGuyrandom;           

        public List<Enemy2a> badGuys2;  // enemies
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
            level = new World(0);

            // Setup window dimensions.
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferMultiSampling = false;
            graphics.ApplyChanges();

            // Initialize Gallagher
            gMan = new Player ();
            //gMan.Position = new Vector2(50, 250);
            gMan.motion = new Vector2(0f, 0f);
            gMan.Width = 100;
            gMan.Height = 250;

            aCrate = new Item();
            aCrate.initialize(Content, "crate");
            aCrate.setItemPosition(new Rectangle(250, 300, 50, 100));

            projectiles = new List<Projectile>();
            enemyProjectiles = new List<EnemyProjectile>();
            // Set the laser to fire every quarter second
            fireTime = TimeSpan.FromSeconds(.15f);
            fireTimeEnemy = TimeSpan.FromSeconds(.15f);
            // Set a constant player move speed
            // Initialize the gravies list
            enemies = new List<enemy>();
            // Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;
            // Used to determine how fast enemy respawns
            enemySpawnTime = TimeSpan.FromSeconds(2.0f);

            // Initialize our random number generator
            random = new Random();

            IsMouseVisible = true;
            //set the gamestate to start menu
            gameState = GameState.Playing;
            //get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
           
            /*
            // Initialize Gun
            handGunA = new Interactable();
            handGunA.position = new Vector2(gMan.position.X + 50, gMan.position.Y + 100);
            handGunA.motion = new Vector2(0f, 0f);
            handGunA.Width = 150;
            handGunA.Height = 55;

            gunAngle = 0;
            gunOrigin = new Vector2(handGunA.position.X + 10, handGunA.position.Y + 10);
            */

            badGuys = new List<Enemy1a>();  
            badGuyspawnTime = new TimeSpan();   
            badGuyspawnTime = TimeSpan.FromSeconds(2.0f);  // spawn within 5 seconds
            badGuycheckpoint = new TimeSpan();
            badGuycheckpoint = TimeSpan.FromSeconds(0.0);
            badGuyrandom = new Random();

            badGuys2 = new List<Enemy2a>();  // maybe new****!!!!
            badGuy2spawnTime = new TimeSpan();   // **** NEWLY ADDED ****!!!!
            badGuy2spawnTime = TimeSpan.FromSeconds(4.0f);  // spawn within 5 seconds
            badGuy2checkpoint = new TimeSpan();
            badGuy2checkpoint = TimeSpan.FromSeconds(0.0);
            badGuy2random = new Random();

            
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
            backgroundTexture = Content.Load<Texture2D>("LEVEL OUTSIDE copy");
            backgroundTexture2 = Content.Load<Texture2D>("backgroundB");
            gManTest=  Content.Load<Texture2D>("gspritesheattest");
            gMan.Initialize(gManTest, new Vector2(41,166));
            gManTexture = Content.Load<Texture2D>("gallagher_sprite_12");
            gunATexture = Content.Load<Texture2D>("handGun 2a");

            // Load the player resources
            projectileTexture = Content.Load<Texture2D>("laser");
            // Load the score font
            font = Content.Load<SpriteFont>("gameFont");
            // Load the laser and explosion sound effect
            enemyTexture = Content.Load<Texture2D>("mineAnimation");

            /*
            enemyATexture = Content.Load<Texture2D>("gunEnemy 1a");
            enemyBTexture = Content.Load<Texture2D>("gunEnemy 2a");
            enemy2bTexture = Content.Load<Texture2D>("gunEnemy 2b");
            enemyCTexture = Content.Load<Texture2D>("gunEnemy 3a");
            */

            enemyATexture = Content.Load<Texture2D>("smallRobot1a");
            enemy1bTexture = Content.Load<Texture2D>("smallRobot1b");
            enemyBTexture = Content.Load<Texture2D>("mediumRobot1a");
            enemy2bTexture = Content.Load<Texture2D>("mediumRobot1b");
            //enemyCTexture = Content.Load<Texture2D>("Boss1a");
            enemyCTexture = Content.Load<Texture2D>("gunEnemy 3a");

            //EnemyTexture = Content.Load<Texture2D>("Enemy 1a");
            //BulletTexture = Content.Load<Texture2D>("Bullet 1a");

            //font = Content.Load<SpriteFont>("Score");

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
                        scrollPosition -= moveFactorPerSecond;
                    }
                    if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.L) ||
                    currentGamePadState.DPad.Right == ButtonState.Pressed)
                    {
                        scrollPosition += moveFactorPerSecond;
                    }

                    //      MediaPlayer.Resume();
                    //Update the player
                    //UpdatePlayer(gameTime);
                    gMan.Update(gameTime,currentKeyboardState,currentGamePadState, aCrate.getUpMove(), aCrate.getDownMove(), level);

                    aCrate.Update(gMan);
                    // Update the gravies
                    //UpdateEnemies(gameTime);
                    // Update the collision
                    //UpdateCollision();
                    // Update the projectiles
                    UpdateProjectiles();
                    // Update the enemy projectiles
                    //UpdateEnemyProjectiles();
                }
            }


            //*********************
            // Update badGuys
            // (check if beaten?)
            //
            for (int i = 0; i < badGuys.Count; i++)
            {
                badGuys[i].Update();

                /*  // good start
                if (badGuys[i].position.X > gMan.Position.X)
                    badGuys[i].velocity = new Vector2(-2f, badGuys[i].velocity.Y);
                else if (badGuys[i].position.X <= gMan.Position.X)
                    badGuys[i].velocity = new Vector2(0f, badGuys[i].velocity.Y);

                if (badGuys[i].position.Y > gMan.Position.Y)
                    badGuys[i].velocity = new Vector2(badGuys[i].velocity.X, -2f);
                else if (badGuys[i].position.Y <= gMan.Position.Y)
                    badGuys[i].velocity = new Vector2(badGuys[i].velocity.X, 0f);
                */

                // 2nd try  // will work as temp
                /*
                if (badGuys[i].position.X > gMan.Position.X + badGuys[i].Width)
                    badGuys[i].velocity = new Vector2(-2f, badGuys[i].velocity.Y);
                else if (badGuys[i].position.X <= gMan.Position.X + badGuys[i].Width && badGuys[i].position.X >= gMan.Position.X - badGuys[i].Width)
                    badGuys[i].velocity = new Vector2(0f, badGuys[i].velocity.Y);
                else if (badGuys[i].position.X < gMan.Position.X - badGuys[i].Width)
                    badGuys[i].velocity = new Vector2(2f, badGuys[i].velocity.Y);
                */

                // 3rd try
                if (badGuys[i].position.X <= gMan.Position.X + badGuys[i].Width && badGuys[i].position.X >= gMan.Position.X + gMan.Width)
                    badGuys[i].velocity = new Vector2(0f, badGuys[i].velocity.Y);
                else if (badGuys[i].position.X > gMan.Position.X + badGuys[i].Width)
                    badGuys[i].velocity = new Vector2(-2f, badGuys[i].velocity.Y);                
                else if (badGuys[i].position.X < gMan.Position.X + gMan.Width)
                    badGuys[i].velocity = new Vector2(2f, badGuys[i].velocity.Y);


                /*
                if (badGuys[i].position.Y > gMan.Position.Y)
                    badGuys[i].velocity = new Vector2(badGuys[i].velocity.X, -2f);
                else if (badGuys[i].position.Y <= gMan.Position.Y)
                    badGuys[i].velocity = new Vector2(badGuys[i].velocity.X, 0f);
                */

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
                badGuys2[i].Update();

                /*  // good start
                if (badGuys[i].position.X > gMan.Position.X)
                    badGuys[i].velocity = new Vector2(-2f, badGuys[i].velocity.Y);
                else if (badGuys[i].position.X <= gMan.Position.X)
                    badGuys[i].velocity = new Vector2(0f, badGuys[i].velocity.Y);

                if (badGuys[i].position.Y > gMan.Position.Y)
                    badGuys[i].velocity = new Vector2(badGuys[i].velocity.X, -2f);
                else if (badGuys[i].position.Y <= gMan.Position.Y)
                    badGuys[i].velocity = new Vector2(badGuys[i].velocity.X, 0f);
                */

                // 2nd try  // will work as temp

                if (badGuys2[i].position.X > gMan.Position.X - badGuys2[i].Width)
                {
                    badGuys2[i].velocity = new Vector2(-5f, badGuys2[i].velocity.Y);
                }
                else if (badGuys2[i].position.X <= gMan.Position.X - (badGuys2[i].Width * .8f) && badGuys2[i].position.X >= gMan.Position.X - (badGuys2[i].Width + 10))
                {
                    badGuys2[i].texture = enemy2bTexture;
                    badGuys2[i].velocity = new Vector2(0f, badGuys2[i].velocity.Y);

                    if (badGuys2[i].position.Y > gMan.Position.Y - 10 && badGuys2[i].position.Y < gMan.Position.Y + 10)
                        badGuys2[i].velocity = new Vector2(badGuys2[i].velocity.X, 0f);
                    if (badGuys2[i].position.Y > gMan.Position.Y+10)
                        badGuys2[i].velocity = new Vector2(badGuys2[i].velocity.X, -5f);
                    else if (badGuys2[i].position.Y < gMan.Position.Y-10)
                        badGuys2[i].velocity = new Vector2(badGuys2[i].velocity.X, 5f);
                }
                else if (badGuys2[i].position.X < gMan.Position.X - (badGuys2[i].Width + 10))
                {
                    //enemyBTexture = Content.Load<Texture2D>("gunEnemy 2b");
                    badGuys2[i].texture = enemy2bTexture;
                    badGuys2[i].velocity = new Vector2(5f, badGuys2[i].velocity.Y);
                }
                /*
                if (badGuys[i].position.Y > gMan.Position.Y)
                    badGuys[i].velocity = new Vector2(badGuys[i].velocity.X, -2f);
                else if (badGuys[i].position.Y <= gMan.Position.Y)
                    badGuys[i].velocity = new Vector2(badGuys[i].velocity.X, 0f);
                */

                if (badGuys[i].health <= 0)
                {
                    badGuys.RemoveAt(i);
                    i--;
                    //scream_male.Play();
                }
            }

            //*********************
            // Update badGuys3
            // (check if beaten?)
            //
            for (int i = 0; i < badGuys3.Count; i++)
            {
                badGuys3[i].Update();

                Vector2 tempPos = badGuys3[i].position;

                ///*
                // decision making (on direction of movement)
                if(gameTime.TotalGameTime - badGuys3[i].lastDecisionTime >= badGuys3[i].decisionTime)
                {
                    badGuys3[i].decisionTimeFlag = true;
                    badGuys3[i].lastDecisionTime = gameTime.TotalGameTime;
                }

                if (badGuys3[i].decisionTimeFlag == true && gameTime.TotalGameTime - badGuys3[i].lastDecisionTime >= badGuys3[i].reverseDecisionTime)
                {
                    badGuys3[i].decisionTimeFlag = false;
                }
                //*/

                // general movement rules
                if (badGuys3[i].position.X > gMan.Position.X - badGuys3[i].Width)
                {
                    badGuys3[i].velocity = new Vector2(-5f, badGuys3[i].velocity.Y);
                }
                else if (badGuys3[i].position.X <= gMan.Position.X - (badGuys3[i].Width * .8f) && badGuys3[i].position.X >= gMan.Position.X - (badGuys3[i].Width + 10))
                {
                    badGuys3[i].texture = enemy1bTexture;
                    badGuys3[i].velocity = new Vector2(0f, badGuys3[i].velocity.Y);

                    if (badGuys3[i].position.Y > gMan.Position.Y - 10 && badGuys3[i].position.Y < gMan.Position.Y + 10)
                        badGuys3[i].velocity = new Vector2(badGuys3[i].velocity.X, 0f);
                    if (badGuys3[i].position.Y > gMan.Position.Y + 10)
                        badGuys3[i].velocity = new Vector2(badGuys3[i].velocity.X, -5f);
                    else if (badGuys3[i].position.Y < gMan.Position.Y - 10)
                        badGuys3[i].velocity = new Vector2(badGuys3[i].velocity.X, 5f);
                }
                else if (badGuys3[i].position.X < gMan.Position.X - (badGuys3[i].Width + 10))
                {
                    //enemyBTexture = Content.Load<Texture2D>("gunEnemy 2b");
                    badGuys3[i].texture = enemy1bTexture;
                    badGuys3[i].velocity = new Vector2(5f, badGuys3[i].velocity.Y);
                }

                if (badGuys3[i].ttl <= 0)
                {
                    badGuys3.RemoveAt(i);
                    i--;
                }
            }


            // badGuys spawn on random time interval
            if (gameTime.TotalGameTime - badGuycheckpoint > badGuyspawnTime && badGuys.Count <= 4)
            {
                Vector2 eMotion = new Vector2(-2.5f, 0f);

                float tempX = (float)badGuyrandom.Next(SCREEN_WIDTH, SCREEN_WIDTH + 100);
                float tempY = (float)badGuyrandom.Next(SCREEN_HEIGHT - SCREEN_HEIGHT / 3, SCREEN_HEIGHT - 50);

                Vector2 startPos = new Vector2(tempX, tempY);

                //badGuys.Add(new Enemy1a(72, 72, startPos, eMotion, enemyATexture, 0f, 0f));   // old placehoder size
                badGuys.Add(new Enemy1a(158,87, startPos, eMotion, enemyATexture, 0f, 0f));
                badGuycheckpoint = gameTime.TotalGameTime;

                badGuyspawnTime = TimeSpan.FromSeconds((float)badGuyrandom.Next(1, 5));
            }

            // badGuys2 spawn on random time interval
            if (gameTime.TotalGameTime - badGuy2checkpoint > badGuy2spawnTime && badGuys2.Count <= 4)
            {
                Vector2 eMotion = new Vector2(-2.5f, 0f);

                float tempX = (float)badGuy2random.Next(SCREEN_WIDTH, SCREEN_WIDTH + 100);
                float tempY = (float)badGuy2random.Next(SCREEN_HEIGHT - SCREEN_HEIGHT / 3, SCREEN_HEIGHT - 50);

                Vector2 startPos = new Vector2(tempX, tempY);

                //badGuys2.Add(new Enemy2a(72, 72, startPos, eMotion, enemyBTexture, 0f, 0f));
                badGuys2.Add(new Enemy2a(173, 207, startPos, eMotion, enemyBTexture, 0f, 0f));
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
            
                        


            base.Update(gameTime);
        }


           
        /*
        private void Addenemy()
        {
            // Create the animation object
            Animation enemyAnimation = new Animation();

            // Initialize the animation with the correct animation information
            enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);

            // Randomly generate the position of the enemy
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));
            // Create an enemy
            enemy enemy = new enemy();
            // Initialize the enemy
            enemy.Initialize(enemyAnimation, position);

            // Add the enemy to the active gravies list
            enemies.Add(enemy);
        }
        private void UpdateEnemies(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            //if (currentKeyboardState.IsKeyDown(Keys.Q))
            //{
          
                if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
                {
                    previousSpawnTime = gameTime.TotalGameTime;

                    // Add an enemy
                    Addenemy();
                }
           
            // Update the Enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {

                enemies[i].Update(gameTime);

                // Fire only every interval we set as the fireTime
                if (gameTime.TotalGameTime - previousFireTimeEnemy > fireTimeEnemy)
                {
                    // Reset our current time
                    previousFireTimeEnemy = gameTime.TotalGameTime;

                    // Add the projectile, but add it to the front and center of the player
                    AddEnemyProjectile(enemies[i].Position - new Vector2(enemies[i].Width / 2, 0));
                }

                if (enemies[i].Active == false)
                {
                    if (enemies[i].wallcheck == true)
                    {
                        // Play the explosion sound
                        // explosionSound.Play();
                        //Add to the player's score
                        //score += enemies[i].Value * (powerup + 1);

                        // If not active and health <= 0
                        if (enemies[i].Health <= 0)
                        {
                            // Add an explosion
                        }
                    }
                    enemies.RemoveAt(i);
                }

            }
        }
        */

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update();
                if (projectiles[i].Active == false)
                {

                    projectiles.RemoveAt(i);
                }


            }
        }

        private void AddProjectile(Vector2 position)
        {
            Projectile projectile = new Projectile();
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position);
            projectiles.Add(projectile);
        }
        /*
        private void UpdateEnemyProjectiles()
        {
            // Update the Projectiles
            for (int i = enemyProjectiles.Count - 1; i >= 0; i--)
            {
                enemyProjectiles[i].Update();

                if (enemyProjectiles[i].Active == false)
                {
                    enemyProjectiles.RemoveAt(i);
                }


            }
        }
        private void AddEnemyProjectile(Vector2 position)
        {
            EnemyProjectile enemyProjectile = new EnemyProjectile();
            enemyProjectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position);
            enemyProjectiles.Add(enemyProjectile);
        }
        */










        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //spriteBatch.Draw(background, new Rectangle(0, 0, 1000, 600), Color.White);    // !!WORKING

            //spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            int resolutionWidth = graphics.GraphicsDevice.Viewport.Width;
            int resolutionHeight = graphics.GraphicsDevice.Viewport.Height;
            for(int x = 0; x < level.level.Count; x++)
                spriteBatch.Draw(backgroundTexture, level.level[x], Color.White);
            ///*
            //for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
            //{
            //    for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
            //    {
            //        backgroundPos = new Vector2(x * backgroundTexture.Width +
            //            ((int)scrollPosition) % backgroundTexture.Width,
            //            y * backgroundTexture.Height);
            //        spriteBatch.Draw(backgroundTexture, -backgroundPos, Color.White);
            //    }
            //}
            // */

            ///*
            //if(scrollPosition<=2000)
            //{
            //    for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
            //    {
            //        for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
            //        {
            //            backgroundPos = new Vector2(x * backgroundTexture.Width +
            //                ((int)scrollPosition) % backgroundTexture.Width+000,
            //                y * backgroundTexture.Height);
            //            spriteBatch.Draw(backgroundTexture, -backgroundPos, Color.White);
            //        }
            //    }
            //}
            //if(scrollPosition>=1001)
            //{
            //    for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
            //    {
            //        for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
            //        {
            //            backgroundPos = new Vector2(x * backgroundTexture.Width +
            //                ((int)scrollPosition) % backgroundTexture.Width,
            //                y * backgroundTexture.Height);
            //            spriteBatch.Draw(backgroundTexture2, -backgroundPos, Color.White);
            //        }
            //    }
            //}
            // */

            //if (scrollPosition <= 1000)
            //{
            //    for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
            //    {
            //        for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
            //        {
            //            backgroundPos = new Vector2(x * backgroundTexture.Width +
            //                ((int)scrollPosition) % backgroundTexture.Width + 000,
            //                y * backgroundTexture.Height);
            //            spriteBatch.Draw(backgroundTexture, -backgroundPos, Color.White);
            //        }
            //    }
            //}
            //if (scrollPosition >= 1000 && scrollPosition <= 2000)
            //{
            //    for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
            //    {
            //        for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
            //        {
            //            backgroundPos = new Vector2(x * backgroundTexture.Width +
            //                ((int)scrollPosition) % backgroundTexture.Width,
            //                y * backgroundTexture.Height);
            //            backgroundPos2 = new Vector2(x * backgroundTexture.Width +
            //                ((int)scrollPosition) % backgroundTexture.Width+1000,
            //                y * backgroundTexture.Height);
            //            spriteBatch.Draw(backgroundTexture, -backgroundPos, Color.White);
            //            spriteBatch.Draw(backgroundTexture2, -backgroundPos2, Color.White);
            //        }
            //    }
            //}
            //if (scrollPosition >= 2000)
            //{
            //    for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
            //    {
            //        for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
            //        {
            //            backgroundPos = new Vector2(x * backgroundTexture.Width +
            //                ((int)scrollPosition) % backgroundTexture.Width + 000,
            //                y * backgroundTexture.Height);
            //            spriteBatch.Draw(backgroundTexture2, -backgroundPos, Color.White);
            //        }
            //    }
            //}
            
           // spriteBatch.Draw(gManTexture, gMan.Position, Color.White);
            aCrate.Draw(spriteBatch);

          //  Rectangle sourceRectangle = new Rectangle(0, 0, handGunA.Width, handGunA.Height);
           // gunOrigin = new Vector2(handGunA.Width - 140, handGunA.Height - 35);
         //   spriteBatch.Draw(gunATexture, handGunA.position, sourceRectangle, Color.White, gunAngle, gunOrigin, 1.0f, SpriteEffects.None, 1);


            if (gameState == GameState.Playing)
            {
                /*
                // Draw the Player
                //                player.Draw(spriteBatch);
                // Draw the Enemies
                //  boss.Draw(spriteBatch);
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Draw(spriteBatch);
                }
                // Draw the gravies
                // Draw the Projectiles

                // Draw the Projectiles
                for (int i = 0; i < enemyProjectiles.Count; i++)
                {
                    enemyProjectiles[i].Draw(spriteBatch);
                }
                */
                gMan.Draw(spriteBatch);
                for (int i = 0; i < projectiles.Count; i++)
                {
                    projectiles[i].Draw(spriteBatch);
                }

                // draw badGuys
                for (int i = 0; i < badGuys.Count; i++)
                {
                    badGuys[i].Draw(spriteBatch);
                }
                // draw badGuys2
                for (int i = 0; i < badGuys2.Count; i++)
                {
                    badGuys2[i].Draw(spriteBatch);
                }
                // draw badGuys3
                for (int i = 0; i < badGuys3.Count; i++)
                {
                    //badGuys3[i].Draw(spriteBatch);
                }

            }


            //spriteBatch.Draw(EnemyTexture, enemy1.rect, Color.White);
            //mainWeapon.Draw(spriteBatch);

            

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
