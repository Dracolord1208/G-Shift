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
        Standing
        }
        public float scrollPosition = 0;
        float playerMoveSpeed = 4f;
        const int SCREEN_WIDTH = 1000;
        const int SCREEN_HEIGHT = 600;
        const float LowerBounderyHeight = 150;

        
        //float playerMoveSpeed = 5f;
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

        public void Initialize(Texture2D playerTexture, Vector2 position)
        {
            
            //set animation
            
            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;
            playerAnimation = new Animation();
            // Set the player to be active
            Active = true;
            Position = new Vector2(50, 250);
            motion = new Vector2(0f, 0f);
            Width = 100;
            Height = 250;

            Vector2 aTest;
            aTest = new Vector2();
            aTest.X = 0;
            aTest.Y = 497;
            playerAnimation.Initialize(playerTexture, aTest, 79, 162 , 3, 30, Color.White, 1f, true);
            PlayerAnimation = playerAnimation;
            // Set the player health
            Health = 100;
        }

        // Update the player animation
        public void Update(GameTime gameTime, KeyboardState currentKeyboardState,GamePadState currentGamePadState)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
         // Move background texture 400 pixels each second 
            float moveFactorPerSecond = 400 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            //Update(gameTime);
            //  Position += motion;
            motion = new Vector2(0, 0);

            //////handGunA.position = new Vector2(position.X + 50, position.Y + 100);

            // Get Thumbstick Controls
            Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;
            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.J) ||
            currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                Position.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.L) ||
            currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                Position.X += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W) || currentKeyboardState.IsKeyDown(Keys.I) ||
            currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                Position.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S) || currentKeyboardState.IsKeyDown(Keys.K) ||
            currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                Position.Y += playerMoveSpeed;
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
            playerActions( gameTime,  currentKeyboardState, currentGamePadState);
        
        }

        public void playerActions(GameTime gameTime, KeyboardState currentKeyboardState, GamePadState currentGamePadState)
        { 
        
        }
        public void levelup(int num)
        {
            //PlayerAnimation.colorupdate(num);
        }
        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}
