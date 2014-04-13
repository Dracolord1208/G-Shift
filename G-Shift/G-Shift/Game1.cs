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

        List<Item> allItems;
        Item aCrate;
        Item aBarrel;

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
        public List<Rectangle> BadGuys1aRect;
        public TimeSpan badGuyspawnTime;      
        public TimeSpan badGuycheckpoint;     
        public Random badGuyrandom;           

        public List<Enemy2a> badGuys2;  // enemies
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
        public Rectangle gManbase;
        public Rectangle enemy1Rec;
        public Rectangle enemy2Rec;

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

            // Setup window dimensions.
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferMultiSampling = false;
            graphics.ApplyChanges();

            // Initialize Gallagher
            gMan = new Player ();


            allItems = new List<Item>();

            aCrate = new Item();
            aCrate.initialize(Content, "crate");
            allItems.Add(aCrate);
            //aCrate.setItemPosition(new Rectangle(250, 300, 50, 100));

            aBarrel = new Item();
            aBarrel.initialize(Content, "barrel");
            allItems.Add(aBarrel);
            //aBarrel.setItemPosition(new Rectangle(100, 300, 50, 100));

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
            gMan.Initialize(gManTest, new Vector2(70, 200));

            //enemy2Rec;
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
            enemyCTexture = Content.Load<Texture2D>("gunEnemy 3a");



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
            gManbase = new Rectangle((int)gMan.Position.X-100, (int)gMan.Position.Y -30 , 200, 50);

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
                    gMan.Update(gameTime, currentKeyboardState, currentGamePadState, allItems, level);

                    for (int i = 0; i < allItems.Count; i++ )
                        allItems[i].Update(gMan, Content, graphics);
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
                enemy1Rec = new Rectangle((int)badGuys2[i].position.X - 60,
              (int)badGuys2[i].position.Y + 39,  40, 7);
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

            spriteBatch.Draw(backgroundTexture, -backgroundPos, Color.White);

            level.Draw(spriteBatch);

            if (gameState == GameState.Playing)
            {
                //spriteBatch.Draw(backgroundTexture, gManbase, Color.White);
                gMan.Draw(spriteBatch);
                for (int i = 0; i < projectiles.Count; i++)
                {
                    projectiles[i].Draw(spriteBatch);
                }

                // draw badGuys
                for (int i = 0; i < badGuys.Count; i++)
                {
                    badGuys[i].Draw(spriteBatch);
                  //  spriteBatch.Draw(backgroundTexture, BadGuys1aRect[i], Color.White);
                }
                // draw badGuys2
                for (int i = 0; i < badGuys2.Count; i++)
                {
                    badGuys2[i].Draw(spriteBatch);
                    spriteBatch.Draw(backgroundTexture, enemy1Rec, Color.White);
                }
                // draw badGuys3
                for (int i = 0; i < badGuys3.Count; i++)
                {
                    //badGuys3[i].Draw(spriteBatch);
                }
                spriteBatch.DrawString(font, "xPosition: " + gMan.Position.X, Vector2.Zero, Color.White);
                spriteBatch.DrawString(font, "yPosition: " + gMan.Position.Y, new Vector2(0, 30), Color.White);
            }

            //for (int i = 0; i < allItems.Count; i++)
            //{
                //allItems[i].Draw(spriteBatch);
            //}
                //spriteBatch.Draw(EnemyTexture, enemy1.rect, Color.White);
                //mainWeapon.Draw(spriteBatch);



            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
