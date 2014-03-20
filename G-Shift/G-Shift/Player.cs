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
       public Animation PlayerAnimation;
       // Keyboard states used to determine key presses
       KeyboardState currentKeyboardState;
       KeyboardState previousKeyboardState;

       // Gamepad states used to determine button presses
       GamePadState currentGamePadState;
       GamePadState previousGamePadState;
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
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);
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

            // Keyboard input
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {

                Position.Y += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                //motion = new Vector2(0, -5);
                Position.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                Position.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                scrollPosition += moveFactorPerSecond;
                //far_scrollPosition += far_moveFactorPerSecond;
                //motion = new Vector2(5, 0);
                Position.X += playerMoveSpeed;
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

            /*
            if (Position.Y >= SCREEN_HEIGHT - (LowerBounderyHeight * (.33f)))
            {
                depth = 1;  // foreground
            }
            else if (Position.Y >= SCREEN_HEIGHT - (LowerBounderyHeight * (.66f)))
            {
                depth = 3;  // rear-ground
            }
            else
            {
                depth = 2;  // middle depth
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
*/        
        }
        public void levelup(int num)
        {
            PlayerAnimation.colorupdate(num);
        }
        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}
