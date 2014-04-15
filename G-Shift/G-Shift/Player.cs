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
    class Player 
    {
       public  enum Stance{ 
            Standing,
            Left,
            Right,
            lightAttack,
            heavyAttack,
            hurt
       }
       public bool withinRect = true;
       public int currentRect = 0, currentPath;
        public float scrollPosition = 0;
        float playerMoveSpeed = 4f;
        const int SCREEN_WIDTH = 1000;
        const int SCREEN_HEIGHT = 600;
        const float LowerBounderyHeight = 150;
        public Animation PlayerAnimation;
        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;
        public bool hasBeenHit = false;
        bool canMoveUp;
        bool canMoveDown;
        // State of the player
        public bool Active;
        public int Height { get; set; }
        public int Width { get; set; }
        bool facing=true;//true==left false == right
        // Amount of hit points that player has
        public int Health;
        public int maxHealth;
        public Vector2 motion { get; set; }
        // Get the width of the player ship
        /*public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        // Get the height of the player ship
        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }
        */
        float WaitTimeToShowCard = 0;

        public int depth { get; set; }
        // Initialize the player
        Animation playerAnimation;
        public Stance playerStance;
        Texture2D gManTest1; //standing
        Texture2D gManTest2;   //if (playerStance == Stance.Left)
        Texture2D gManTest3;//if (playerStance == Stance.Right)
        Texture2D gManTest5l;  //if (playerStance == Stance.lightAttack)
        Texture2D gManTest5r;  //if (playerStance == Stance.lightAttack)
        Texture2D gManTest4;   //if (playerStance == Stance.heavyAttack
        Texture2D gManTest6;//hit
        Texture2D gManTest7;//death
        public int heavyHit = 5;
        int Combo = 0;
        float attackTime;
        bool isAttacking;
        TimeSpan fireTime;
        TimeSpan previousFireTime;
        int elapsedTime;
        
        public Rectangle hitBox;
        //public Rectangle hitbox { get; set; }

        bool isPunching;
      //   float MaxAttackTime=2;
        //Content.RootDirectory = "Content";
        public void LoadContent(ContentManager content) 
        {
            gManTest1 = content.Load<Texture2D>("Galager/WALKING 2");
            gManTest2 = content.Load<Texture2D>("Galager/WALKING 2");
            gManTest3 = content.Load<Texture2D>("Galager/WALKING 2_2");
            gManTest4 = content.Load<Texture2D>("gst4");
            gManTest5l = content.Load<Texture2D>("Galager/SHOOT 4");
            gManTest5r = content.Load<Texture2D>("Galager/SHOOT 4r");
            gManTest6 = content.Load<Texture2D>("Galager/6");
            gManTest7 = content.Load<Texture2D>("Galager/DEATH 1");

        }
        public void Initialize(Texture2D playerTexture, Vector2 position)
        {
            
            //set animation
            fireTime = TimeSpan.FromSeconds(.30f);
            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;
            playerAnimation = new Animation();
            // Set the player to be active
            Active = true;
            playerStance = Stance.Standing; 
            playerAnimation.Initialize(gManTest1, Vector2.Zero, 157, 200, 1, 30, Color.White, 1f, true);
            PlayerAnimation = playerAnimation;
            // Set the player health
            maxHealth = 100;
            Health = 100;
            canMoveUp = true;
            canMoveDown = true;
            hitBox = new Rectangle((int)Position.X - 100, (int)Position.Y - 30, 200, 50);
        }

        // Update the player animation
        //public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, GamePadState currentGamePadState, bool canMoveUp, bool canMoveDown, List<Item> allItems, World level)
        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, GamePadState currentGamePadState, List<Item> allItems, World level)

        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
           //  currentKeyboardState=   Keyboard.GetState();
           // Move background texture 400 pixels each second 
            StanceMoves( gameTime);
            playerStance = Stance.Standing;
            motion = new Vector2(0, 0);
            Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;
            //////handGunA.position = new Vector2(position.X + 50, position.Y + 100);
            
            // Use the Keyboard / Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.J) ||
            currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                Position.X -= playerMoveSpeed;
                facing = false;
                playerStance = Stance.Left;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.L) ||
            currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                Position.X += playerMoveSpeed;
                facing = true;
                playerStance = Stance.Right;
            }

            canMoveUp = true;

            if (currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W) || currentKeyboardState.IsKeyDown(Keys.I) ||
                currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                for (int i = 0; i < allItems.Count; i++)
                {
                    if (!(allItems[i].getUpMove()))
                    {
                        canMoveUp = false;
                    }
                }

                if (canMoveUp)
                    Position.Y -= playerMoveSpeed;
            }

            canMoveDown = true;

            if (currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S) || currentKeyboardState.IsKeyDown(Keys.K) ||
            currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                for (int i = 0; i < allItems.Count; i++)
                {
                    if (!(allItems[i].getDownMove()))
                    {
                        canMoveDown = false;
                    }
                }

                if (canMoveDown)
                    Position.Y += playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Z) || currentGamePadState.Buttons.X == ButtonState.Pressed)
            {
                playerStance = Stance.lightAttack;
            }
            if (currentKeyboardState.IsKeyDown(Keys.C) || currentGamePadState.Buttons.Y == ButtonState.Pressed)
            {
                playerStance = Stance.heavyAttack;
            }

            //else
            if (currentKeyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space)&&Combo==0)
            {
             //   whipping = true;
                //attackTime = maxAttackTime;
               // animateWhip(gameTime);

                playerStance = Stance.heavyAttack;
//ok
                Combo ++;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space) && Combo == 1)
            {
                //   whipping = true;
                //attackTime = maxAttackTime;
                // animateWhip(gameTime);
                playerStance = Stance.heavyAttack;
                Combo++;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space) && Combo == 2)
            {
                //   whipping = true;
                //attackTime = maxAttackTime;
                // animateWhip(gameTime);
                playerStance = Stance.heavyAttack;
                Combo=0;
            }

            CollisionDetection(level);

            hitBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void CollisionDetection(World level)
        {
            if (withinRect)
            {
                // gMan y-boundaries
                if (Position.Y < level.level[currentRect].Y)
                    Position.Y = level.level[currentRect].Y;
                else
                if (Position.Y > level.level[currentRect].Y + level.level[currentRect].Height)
                    Position.Y = level.level[currentRect].Y + level.level[currentRect].Height;

                // gMan x-boundaries
                if (Position.X < level.level[currentRect].X)
                {
                    if (currentRect == 0)
                    {
                        Position = new Vector2(level.level[0].X, Position.Y);
                    }
                    else
                    {
                        currentPath = currentRect - 1;
                        withinRect = false;
                        return;
                    }
                }
                else
                if (Position.X > level.level[currentRect].X + level.level[currentRect].Width)
                {
                    if (currentRect == level.level.Count - 1)
                    {
                        Position = new Vector2(level.level[currentRect].X + level.level[currentRect].Width, Position.Y);
                    }
                    else
                    {
                        currentPath = currentRect;
                        withinRect = false;
                        return;
                    }
                }
            }
            else
            {
                float topB, botB, topM, botM, topY, botY;
                topM = (level.tracks[currentPath].topLeft.Y - level.tracks[currentPath].topRight.Y)
                    / (level.tracks[currentPath].topLeft.X - level.tracks[currentPath].topRight.X);
                topB = (topM * (-level.tracks[currentPath].topLeft.X)) + level.tracks[currentPath].topLeft.Y;
                topY = topM * Position.X + topB;

                botM = (level.tracks[currentPath].botLeft.Y - level.tracks[currentPath].botRight.Y)
                   / (level.tracks[currentPath].botLeft.X - level.tracks[currentPath].botRight.X);
                botB = (botM * (-level.tracks[currentPath].botLeft.X)) + level.tracks[currentPath].botLeft.Y;
                botY = botM * Position.X + botB;

                if (Position.X <= level.tracks[currentPath].topLeft.X)
                {
                    withinRect = true;
                    return;
                }
                else
                if (Position.X >= level.tracks[currentPath].topRight.X)
                {
                    withinRect = true;
                    currentRect = currentPath + 1;
                    return;
                }

                if (Position.Y < topY)
                    Position.Y = topY;
                else
                    if (Position.Y > botY)
                        Position.Y = botY;
            }

        }

        public void StanceMoves(GameTime gameTime)
        {
            if (playerStance == Stance.Standing)
            {
                playerAnimation.change(gManTest1, 157, 200, 1, 30, Color.White, 1f, true);
                if (facing)
                {
                    playerAnimation.change(gManTest3, 157, 200, 1, 30, Color.White, 1f, true);
                }
                else
                {
                    playerAnimation.change(gManTest1, 157, 200, 1, 30, Color.White, 1f, true);
                }
            }
            if (playerStance == Stance.Left)
            {
                playerAnimation.change(gManTest2, 157, 200, 1, 30, Color.White, 1f, true);
            }
            if (playerStance == Stance.Right)
            {
                playerAnimation.change(gManTest3, 157, 200, 1, 30, Color.White, 1f, true);
            }
            if (playerStance == Stance.lightAttack)
            {
               // playerAnimation.change(gManTest4, 157, 200, 1, 30, Color.White, 1f, true);
            }
            if (playerStance == Stance.hurt)
            {
            }
            if (playerStance == Stance.heavyAttack)
            {
                if (facing)
                {
                        isPunching = true;
                        if (isPunching)
                        {
                           //Update_Hit(gameTime);
                        }
                        if (gameTime.TotalGameTime - previousFireTime > fireTime)
                        {
                            // Reset our current time
                            previousFireTime = gameTime.TotalGameTime;

                            
                            // Add the projectile, but add it to the front and center of the player
                        }
                        elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (elapsedTime > 30)
                        {
                        
                            playerAnimation.change(gManTest5r, 186, 207, 1, 300, Color.White, 1f, true);
                            // Reset the elapsed time to zero
                            elapsedTime = 0;
                        }
                    isPunching = false;
                }
                else
                {
                      playerAnimation.change(gManTest5l, 186, 207, 1, 300, Color.White, 1f, true);
                }
            }
        }

        private void doAttack(GameTime gameTime)
        {
            if (isAttacking)
            {
                if (attackTime > 0.0f)
                {
                    attackTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    isAttacking = false;
                }
            }
            else
            {
                attackTime = 0.0f;
            }

        }


        public bool getDirection()
        {
            return facing;
        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch, float depth)
        {
            PlayerAnimation.Draw(spriteBatch, depth);
        }
    }
}
