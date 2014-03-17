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

        Texture2D gManTexture;
        Texture2D gunATexture;
        Texture2D enemyATexture;
        Texture2D bulletATexture;

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

            // Setup window dimensions.
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferMultiSampling = false;
            graphics.ApplyChanges();

            // Initialize Gallagher
            gMan = new Player();
            gMan.Position = new Vector2(50, 250);
            gMan.motion = new Vector2(0f, 0f);
            gMan.Width = 100;
            gMan.Height = 250;


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

            // TODO: use this.Content to load your game content here
            backgroundTexture = Content.Load<Texture2D>("backgroundA");
            backgroundTexture2 = Content.Load<Texture2D>("backgroundB");

            gManTexture = Content.Load<Texture2D>("gallagher_sprite_12");
            gunATexture = Content.Load<Texture2D>("handGun 2a");

            // Load the player resources
            projectileTexture = Content.Load<Texture2D>("laser");
            // Load the score font
            font = Content.Load<SpriteFont>("gameFont");
            // Load the laser and explosion sound effect
            enemyTexture = Content.Load<Texture2D>("mineAnimation");



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
                    //      MediaPlayer.Resume();
                    //Update the player
                    UpdatePlayer(gameTime);
                    // Update the gravies
                    UpdateEnemies(gameTime);
                    // Update the collision
                    //UpdateCollision();
                    // Update the projectiles
                    UpdateProjectiles();
                    // Update the enemy projectiles
                    UpdateEnemyProjectiles();
                }
            }
          


                        


            base.Update(gameTime);
        }


        private void UpdatePlayer(GameTime gameTime)
        {
            // Move background texture 400 pixels each second 
            float moveFactorPerSecond = 400 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            gMan.Update(gameTime);
          //  gMan.Position += gMan.motion;
            gMan.motion = new Vector2(0, 0);

            //////handGunA.position = new Vector2(gMan.position.X + 50, gMan.position.Y + 100);

            // Keyboard input
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                
                gMan.Position.Y += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                //gMan.motion = new Vector2(0, -5);
                gMan.Position.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                gMan.Position.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                scrollPosition += moveFactorPerSecond;
                //far_scrollPosition += far_moveFactorPerSecond;
                //gMan.motion = new Vector2(5, 0);
                gMan.Position.X += playerMoveSpeed;
            }

            // Allows the game to exit            
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();


            // gMan y-boundaries
            if (gMan.Position.Y <= 200)
                gMan.Position = new Vector2(gMan.Position.X, 200);
            if (gMan.Position.Y >= SCREEN_HEIGHT - 250)
                gMan.Position = new Vector2(gMan.Position.X, SCREEN_HEIGHT - gMan.Height); //300);


            // gMan x-boundaries
            if (gMan.Position.X <= 0)
                gMan.Position = new Vector2(0, gMan.Position.Y);
            if (gMan.Position.X >= SCREEN_WIDTH - gMan.Width)
                gMan.Position = new Vector2(SCREEN_WIDTH - gMan.Width, gMan.Position.Y);


            if (gMan.Position.Y >= SCREEN_HEIGHT - (LowerBounderyHeight * (.33f)))
            {
                gMan.depth = 1;  // foreground
            }
            else if (gMan.Position.Y >= SCREEN_HEIGHT - (LowerBounderyHeight * (.66f)))
            {
                gMan.depth = 3;  // rear-ground
            }
            else
            {
                gMan.depth = 2;  // middle depth
            }


            if (currentKeyboardState.IsKeyDown(Keys.Space) ||
    currentGamePadState.Buttons.A == ButtonState.Pressed)
            {

                    if (gameTime.TotalGameTime - previousFireTime > fireTime)
                    {
                        // Reset our current time
                        previousFireTime = gameTime.TotalGameTime;

                        // Add the projectile, but add it to the front and center of the player

                    }
                }
        }

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
            /*
            for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
            {
                for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
                {
                    backgroundPos = new Vector2(x * backgroundTexture.Width +
                        ((int)scrollPosition) % backgroundTexture.Width,
                        y * backgroundTexture.Height);
                    spriteBatch.Draw(backgroundTexture, -backgroundPos, Color.White);
                }
            }
             */

            /*
            if(scrollPosition<=2000)
            {
                for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
                {
                    for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
                    {
                        backgroundPos = new Vector2(x * backgroundTexture.Width +
                            ((int)scrollPosition) % backgroundTexture.Width+000,
                            y * backgroundTexture.Height);
                        spriteBatch.Draw(backgroundTexture, -backgroundPos, Color.White);
                    }
                }
            }
            if(scrollPosition>=1001)
            {
                for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
                {
                    for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
                    {
                        backgroundPos = new Vector2(x * backgroundTexture.Width +
                            ((int)scrollPosition) % backgroundTexture.Width,
                            y * backgroundTexture.Height);
                        spriteBatch.Draw(backgroundTexture2, -backgroundPos, Color.White);
                    }
                }
            }
             */

            if (scrollPosition <= 1000)
            {
                for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
                {
                    for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
                    {
                        backgroundPos = new Vector2(x * backgroundTexture.Width +
                            ((int)scrollPosition) % backgroundTexture.Width + 000,
                            y * backgroundTexture.Height);
                        spriteBatch.Draw(backgroundTexture, -backgroundPos, Color.White);
                    }
                }
            }
            if (scrollPosition >= 1000 && scrollPosition <= 2000)
            {
                for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
                {
                    for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
                    {
                        backgroundPos = new Vector2(x * backgroundTexture.Width +
                            ((int)scrollPosition) % backgroundTexture.Width,
                            y * backgroundTexture.Height);
                        backgroundPos2 = new Vector2(x * backgroundTexture.Width +
                            ((int)scrollPosition) % backgroundTexture.Width+1000,
                            y * backgroundTexture.Height);
                        spriteBatch.Draw(backgroundTexture, -backgroundPos, Color.White);
                        spriteBatch.Draw(backgroundTexture2, -backgroundPos2, Color.White);
                    }
                }
            }
            if (scrollPosition >= 2000)
            {
                for (int x = -1; x <= resolutionWidth / backgroundTexture.Width + 1; x++)
                {
                    for (int y = 0; y <= resolutionHeight / backgroundTexture.Height; y++)
                    {
                        backgroundPos = new Vector2(x * backgroundTexture.Width +
                            ((int)scrollPosition) % backgroundTexture.Width + 000,
                            y * backgroundTexture.Height);
                        spriteBatch.Draw(backgroundTexture2, -backgroundPos, Color.White);
                    }
                }
            }
            
            spriteBatch.Draw(gManTexture, gMan.Position, Color.White);

          //  Rectangle sourceRectangle = new Rectangle(0, 0, handGunA.Width, handGunA.Height);
           // gunOrigin = new Vector2(handGunA.Width - 140, handGunA.Height - 35);
         //   spriteBatch.Draw(gunATexture, handGunA.position, sourceRectangle, Color.White, gunAngle, gunOrigin, 1.0f, SpriteEffects.None, 1);


            if (gameState == GameState.Playing)
            {
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
                for (int i = 0; i < projectiles.Count; i++)
                {
                    projectiles[i].Draw(spriteBatch);
                }
                // Draw the Projectiles
                for (int i = 0; i < enemyProjectiles.Count; i++)
                {
                    enemyProjectiles[i].Draw(spriteBatch);
                }


            }


            //spriteBatch.Draw(EnemyTexture, enemy1.rect, Color.White);
            //mainWeapon.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
