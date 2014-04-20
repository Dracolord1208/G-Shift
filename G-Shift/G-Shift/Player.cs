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
        public enum Stance
        {
            Standing,
            Left,
            Right,
            lightAttack,
            heavyAttack,
            hurt
        }
        public bool withinRect = true;
        public int location = 0;
        public float scrollPosition = 0;
        float playerMoveSpeed = 4f;
        const int SCREEN_WIDTH = 1000;
        const int SCREEN_HEIGHT = 600;
        const float LowerBounderyHeight = 150;
        public Animation PlayerAnimation;
        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;
        public Vector2 StartPosition;
        public bool hasBeenHit = false;
        bool canMoveUp;
        bool canMoveDown;
        // State of the player
        public bool Active;
        public int Height { get; set; }
        public int Width { get; set; }
        bool facing = true;//true==left false == right
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
        public Animation playerAnimation;
        public Animation stillAni;
        public Animation walknogun;
        public Animation walkgun;
        public Animation attack1;
        public Animation deathAni;
        public Animation fall;
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
        bool gun;
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
            gManTest1 = content.Load<Texture2D>("Galager/WALKING2");
            gManTest2 = content.Load<Texture2D>("Galager/WALKINGNOGUN");//6
            gManTest3 = content.Load<Texture2D>("Galager/WALKINGWITHGUN");//6
            // gManTest4 = content.Load<Texture2D>("gst4");
            gManTest5l = content.Load<Texture2D>("Galager/Attack1");//8
            gManTest5r = content.Load<Texture2D>("Galager/Attack1");
            gManTest6 = content.Load<Texture2D>("Galager/FALLANIMATON");//7
            gManTest7 = content.Load<Texture2D>("Galager/DEATH");//8
            stillAni.Initialize(gManTest5l, Vector2.Zero, 225, 225, 1, 30, Color.White, 1f, true);
            walknogun.Initialize(gManTest2, Vector2.Zero, 225, 225, 6, 30, Color.White, 1f, true);
            walkgun.Initialize(gManTest3, Vector2.Zero, 225, 225, 6, 30, Color.White, 1f, true);
            attack1.Initialize(gManTest5l, Vector2.Zero, 225, 225, 8, 30, Color.White, 1f, true);
            deathAni.Initialize(gManTest7, Vector2.Zero, 225, 225, 8, 30, Color.White, 1f, true);
            fall.Initialize(gManTest6, Vector2.Zero, 225, 225, 7, 30, Color.White, 1f, true);
        }
        public void Initialize(Texture2D playerTexture, Vector2 position)
        {

            //set animation
            fireTime = TimeSpan.FromSeconds(.30f);
            // Set the starting position of the player around the middle of the screen and to the back
            StartPosition = Position = position;
            playerAnimation = new Animation();
            stillAni = new Animation();
            walknogun = new Animation();
            walkgun = new Animation();
            attack1 = new Animation();
            deathAni = new Animation();
            fall = new Animation();
            // Set the player to be active
            Active = true;
            playerStance = Stance.Standing;
          //  playerAnimation.Initialize(gManTest1, Vector2.Zero, 225, 225, 6, 30, Color.White, 1f, true);


          //  PlayerAnimation = playerAnimation;
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
          //  PlayerAnimation.Position = new Vector2(StartPosition.X, Position.Y);
            //PlayerAnimation.Update(gameTime);
            stillAni.Update(gameTime);
            walknogun.Update(gameTime);
            walkgun.Update(gameTime);
            attack1.Update(gameTime);
            deathAni.Update(gameTime);
            fall.Update(gameTime);
            stillAni.Position = new Vector2(StartPosition.X, Position.Y);
            walknogun.Position = new Vector2(StartPosition.X, Position.Y);
            walkgun.Position = new Vector2(StartPosition.X, Position.Y);
            attack1.Position = new Vector2(StartPosition.X, Position.Y);
            deathAni.Position = new Vector2(StartPosition.X, Position.Y);
            fall.Position = new Vector2(StartPosition.X, Position.Y);
            stillAni.Update(gameTime);
            //  currentKeyboardState=   Keyboard.GetState();
            // Move background texture 400 pixels each second 
            StanceMoves(gameTime);
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
            if (currentKeyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space) && Combo == 0)
            {
                //   whipping = true;
                //attackTime = maxAttackTime;
                // animateWhip(gameTime);

                playerStance = Stance.heavyAttack;
                //ok
                Combo++;
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
                Combo = 0;
            }

            CollisionDetection(level);

            hitBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void CollisionDetection(World level)
        {
            if (withinRect)
            {
                // gMan y-boundaries
                if (Position.Y < level.level[location / 2].Y)
                    Position.Y = level.level[location / 2].Y;
                else
                    if (Position.Y > level.level[location / 2].Y + level.level[location / 2].Height)
                        Position.Y = level.level[location / 2].Y + level.level[location / 2].Height;

                // gMan x-boundaries
                if (Position.X < level.level[location / 2].X)
                {
                    if (location == 0)
                    {
                        Position = new Vector2(level.level[0].X, Position.Y);
                    }
                    else
                    {
                        location--;
                        withinRect = false;
                        return;
                    }
                }
                else
                    if (Position.X > level.level[location / 2].X + level.level[location / 2].Width)
                    {
                        if (location / 2 == level.level.Count - 1)
                        {
                            Position = new Vector2(level.level[location / 2].X + level.level[location / 2].Width, Position.Y);
                        }
                        else
                        {
                            location++;
                            withinRect = false;
                            return;
                        }
                    }
            }
            else
            {
                float topB, botB, topM, botM, topY, botY;
                topM = (level.tracks[location / 2].topLeft.Y - level.tracks[location / 2].topRight.Y)
                    / (level.tracks[location / 2].topLeft.X - level.tracks[location / 2].topRight.X);
                topB = (topM * (-level.tracks[location / 2].topLeft.X)) + level.tracks[location / 2].topLeft.Y;
                topY = topM * Position.X + topB;

                botM = (level.tracks[location / 2].botLeft.Y - level.tracks[location / 2].botRight.Y)
                   / (level.tracks[location / 2].botLeft.X - level.tracks[location / 2].botRight.X);
                botB = (botM * (-level.tracks[location / 2].botLeft.X)) + level.tracks[location / 2].botLeft.Y;
                botY = botM * Position.X + botB;

                if (Position.X <= level.tracks[location / 2].topLeft.X)
                {
                    withinRect = true;
                    location--;
                    return;
                }
                else
                    if (Position.X >= level.tracks[location / 2].topRight.X)
                    {
                        withinRect = true;
                        location++;
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
            //    if (playerStance == Stance.Standing)
            //    {
            //        stillAni.Update(gameTime);
            //      //  playerAnimation.change(gManTest1, 225, 255, 6, 30, Color.White, 1f, true);

            //    }
            //    if (playerStance == Stance.Left)
            //    {
            //        //playerAnimation.change(gManTest2, 225, 255, 6, 30, Color.White, 1f, true);
            //        if (gun)
            //        {
            //            walknogun.Update(gameTime);
            //            // playerAnimation.change(gManTest3, 225, 255, 6, 30, Color.White, 1f, true);
            //        }
            //        else
            //        {
            //            walkgun.Update(gameTime);
            //            // playerAnimation.change(gManTest1, 225, 255, 6, 30, Color.White, 1f, true);
            //        }
            //    }
            //    if (playerStance == Stance.Right)
            //    {
            //        //playerAnimation.change(gManTest3, 225, 255, 6, 30, Color.White, 1f, true);
            //        if (gun)
            //        {
            //            walknogun.Update(gameTime);
            //            // playerAnimation.change(gManTest3, 225, 255, 6, 30, Color.White, 1f, true);
            //        }
            //        else
            //        {
            //            walkgun.Update(gameTime);
            //            // playerAnimation.change(gManTest1, 225, 255, 6, 30, Color.White, 1f, true);
            //        }
            //    }
            //    if (playerStance == Stance.lightAttack)
            //    {
            //        //playerAnimation.change(gManTest6, 225, 255, 7, 30, Color.White, 1f, true);
            //        // playerAnimation.change(gManTest4, 225,255, 1, 30, Color.White, 1f, true);
            //    } 
            //    if (playerStance == Stance.hurt)
            //    {
            //        fall.Update(gameTime);
            //    }
            //    if (playerStance == Stance.heavyAttack)
            //    {
            //            if (isPunching)
            //            {
            //                attack1.Update(gameTime);
            //                //Update_Hit(gameTime);
            //                //     playerAnimation.change(gManTest5r, 225, 255, 8, 30, Color.White, 1f, true);
            //            }
            //    }
            //}

            //private void doAttack(GameTime gameTime)
            //{
            //    if (isAttacking)
            //    {
            //        if (attackTime > 0.0f)
            //        {
            //            attackTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            //        }
            //        else
            //        {
            //            isAttacking = false;
            //        }
            //    }
            //    else
            //    {
            //        attackTime = 0.0f;
            //    }

        }


        public bool getDirection()
        {
            return facing;
        }

        public void Draw(SpriteBatch spriteBatch, float depth)
        {
            stillAni.Draw(spriteBatch, depth);
           // PlayerAnimation.Draw(spriteBatch, depth);
            stillAni.Draw(spriteBatch, depth);
            walknogun.Draw(spriteBatch, depth);
            walkgun.Draw(spriteBatch, depth);
            fall.Draw(spriteBatch, depth);
            attack1.Draw(spriteBatch, depth);
        }
    }
}
