using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace G_Shift
{
    class Player
    {
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
        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;

            // Set the player to be active
            Active = true;

            // Set the player health
            Health = 100;
        }

        // Update the player animation
        public void Update(GameTime gameTime)
        {
            //PlayerAnimation.Position = Position;
          //  PlayerAnimation.Update(gameTime);
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
