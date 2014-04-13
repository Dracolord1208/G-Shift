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
        public enum Stance { 
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
        public int heavyHit=5;
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
        public Stance playerStance;
        Texture2D gManTest1; //standing
        Texture2D gManTest2;   //if (playerStance == Stance.Left)
        Texture2D gManTest3;//if (playerStance == Stance.Right)
        Texture2D gManTest5l;  //if (playerStance == Stance.lightAttack)
        Texture2D gManTest5r;  //if (playerStance == Stance.lightAttack)
        Texture2D gManTest4;   //if (playerStance == Stance.heavyAttack
        Texture2D gManTest6;//hit
        Texture2D gManTest7;//death
         
            
          
         
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
            
            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;
            playerAnimation = new Animation();
            // Set the player to be active
            Active = true;
            playerStance = Stance.Standing; 
            playerAnimation.Initialize(gManTest1, Vector2.Zero, 157, 200, 1, 30, Color.White, 1f, true);
            PlayerAnimation = playerAnimation;
            // Set the player health
            Health = 100;
            canMoveUp = true;
            canMoveDown = true;
        }

        // Update the player animation
        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, GamePadState currentGamePadState, List<Item> allItems, World level)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
           // Move background texture 400 pixels each second 
            StanceMoves();
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
<<<<<<< HEAD
=======
//ok
                Combo ++;
>>>>>>> c797f70cdcdf3a29ae9a2d5ea08e6836eb0d9752
            }
            // gMan y-boundaries
            if (Position.Y <= level.level[0].Y)
                Position = new Vector2(Position.X, level.level[0].Y );
            if (Position.Y >= level.level[0].Y + level.level[0].Height)
                Position = new Vector2(Position.X, level.level[0].Y + level.level[0].Height); //300);


            // gMan x-boundaries
            if (Position.X <= level.level[0].X)
                Position = new Vector2(level.level[0].X, Position.Y);
            if (Position.X >= level.level[0].X + level.level[0].Width)
                Position = new Vector2(level.level[0].X + level.level[0].Width, Position.Y);
           
            
        }
        
        public void StanceMoves()
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
            if (playerStance == Stance.heavyAttack)
            {
                if (facing)
                {
                    playerAnimation.change(gManTest5r, 186, 207, 1, 30, Color.White, 1f, true);
                }
                else
                {
                      playerAnimation.change(gManTest5l, 186, 207, 1, 30, Color.White, 1f, true);
                }
            }
        }
        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}
