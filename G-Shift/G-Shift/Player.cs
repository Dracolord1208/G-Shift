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
        enum Stance { 
            Standing,
            Left,
            Right,
            lightAttack,
            heavyAttack
        }
        public float scrollPosition = 0;
        float playerMoveSpeed = 4f;
        const int SCREEN_WIDTH = 1000;
        const int SCREEN_HEIGHT = 600;
        const float LowerBounderyHeight = 150;
       public Animation PlayerAnimation;
        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;

        // State of the player
        public bool Active;
        public int Height { get; set; }
        public int Width { get; set; }
        // Amount of hit points that player has
        public int Health;
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
        public int depth { get; set; }
        // Initialize the player
        Animation playerAnimation;
        Stance playerStance;
        Texture2D gManTest1;
        Texture2D gManTest2;
        Texture2D gManTest3;
        Texture2D gManTest5;
        Texture2D gManTest4;
        //Content.RootDirectory = "Content";
        public void LoadContent(ContentManager content) 
        {
            gManTest1 = content.Load<Texture2D>("gst1");
            gManTest2 = content.Load<Texture2D>("gst2");
            gManTest3 = content.Load<Texture2D>("gst3");
            gManTest4 = content.Load<Texture2D>("gst4");
            gManTest5 = content.Load<Texture2D>("gst5");
        }
        public void Initialize(Texture2D playerTexture, Vector2 position)
        {
            
            //set animation
            
            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;
            playerAnimation = new Animation();
            // Set the player to be active
            Active = true;
            playerStance = Stance.Standing; 
            playerAnimation.Initialize(gManTest1, Vector2.Zero, 82, 166, 3, 30, Color.White, 1f, true);
            PlayerAnimation = playerAnimation;
            // Set the player health
            Health = 100;
        }

        // Update the player animation
        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, GamePadState currentGamePadState, bool canMoveUp, bool canMoveDown)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
         // Move background texture 400 pixels each second 
            StanceMoves();
            playerStance = Stance.Standing;
            //Update(gameTime);
            //  Position += motion;
            motion = new Vector2(0, 0);
            Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;
            //////handGunA.position = new Vector2(position.X + 50, position.Y + 100);
            
            // Use the Keyboard / Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.J) ||
            currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                Position.X -= playerMoveSpeed;
                playerStance = Stance.Left;
                //playerAnimation.Initialize(gManTest2, Vector2.Zero, 82, 166, 7, 30, Color.White, 1f, true);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.L) ||
            currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                Position.X += playerMoveSpeed;
                playerStance = Stance.Right;
                //playerAnimation.Initialize(gManTest3, Vector2.Zero, 82, 166, 7, 30, Color.White, 1f, true);
            }
            if (canMoveUp && (currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W) || currentKeyboardState.IsKeyDown(Keys.I) ||
            currentGamePadState.DPad.Up == ButtonState.Pressed))
            //if (canMoveUp && currentKeyboardState.IsKeyDown(Keys.W))
            {
                Position.Y -= playerMoveSpeed;
            }
            if (canMoveDown && (currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S) || currentKeyboardState.IsKeyDown(Keys.K) ||
            currentGamePadState.DPad.Down == ButtonState.Pressed))
            //if (canMoveDown && currentKeyboardState.IsKeyDown(Keys.S))
            {
                Position.Y += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Z) || currentGamePadState.Buttons.X == ButtonState.Pressed)
            {
                playerStance = Stance.lightAttack;
                //playerAnimation.Initialize(gManTest4, Vector2.Zero, 82, 166, 7, 30, Color.White, 1f, true);
            }
            if (currentKeyboardState.IsKeyDown(Keys.C) || currentGamePadState.Buttons.Y == ButtonState.Pressed)
            {
                playerStance = Stance.heavyAttack;
                //playerAnimation.Initialize(gManTest5, Vector2.Zero, 82, 166, 7, 30, Color.White, 1f, true);
            }
            // gMan y-boundaries
            if (Position.Y <= 200)
                Position = new Vector2(Position.X, 200);
            if (Position.Y >= SCREEN_HEIGHT - 250)
                Position = new Vector2(Position.X, SCREEN_HEIGHT - Height); //300);


            // gMan x-boundaries
            if (Position.X <= 0)
                Position = new Vector2(0, Position.Y);
            if (Position.X >= SCREEN_WIDTH - Width)
                Position = new Vector2(SCREEN_WIDTH - Width, Position.Y);
           
            
        }
        
        public void StanceMoves()
        {
            if (playerStance == Stance.Standing)
            {
               // playerAnimation.Initialize(gManTest1, Vector2.Zero, 82, 166, 3, 30, Color.White, 1f, true);
                playerAnimation.change(gManTest1);
            }
            if (playerStance == Stance.Left)
            {
               // playerAnimation.Initialize(gManTest2, Vector2.Zero, 82, 166, 7, 30, Color.White, 1f, false);
                playerAnimation.change(gManTest2);
            }
            if (playerStance == Stance.Right)
            {
             //   playerAnimation.Initialize(gManTest3, Vector2.Zero, 82, 166, 7, 30, Color.White, 1f, false);
                playerAnimation.change(gManTest3);
            }
            if (playerStance == Stance.lightAttack)
            {
            //    playerAnimation.Initialize(gManTest4, Vector2.Zero, 82, 166, 7, 30, Color.White, 1f, false);
                playerAnimation.change(gManTest4);
            }
            if (playerStance == Stance.heavyAttack)
            {
              //  playerAnimation.Initialize(gManTest5, Vector2.Zero, 82, 166, 7, 30, Color.White, 1f, false);
                playerAnimation.change(gManTest5);
            }
        }
        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}
