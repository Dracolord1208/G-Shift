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

namespace G_Shift
{
    class Item
    {
        Texture2D throwable;
        Rectangle throwableHitbox;
        Rectangle playerPosition;
        SpriteFont font;
        float x;
        float y;

        public void initialize(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("test");
            throwable = Content.Load<Texture2D>("crate");
            throwableHitbox = new Rectangle(200, 300, 50, 100);
            playerPosition = new Rectangle(0, 0, 50, 250);
        }

        public void Update(Player gMan)
        {
            playerPosition.X = (int)gMan.Position.X;
            playerPosition.Y = (int)gMan.Position.Y;
            //x = gMan.motion.X;
            //y = gMan.motion.Y;
            //int xDistance = Math.Abs(throwableHitbox.X - (int)gMan.motion.X);
            //int yDistance = Math.Abs(throwableHitbox.Y - (int)gMan.motion.Y);

            if(playerPosition.Intersects(throwableHitbox))
            {
                //if (Keyboard.GetState().IsKeyDown(Keys.Space))
                //{
                    throwableHitbox.X = (int)gMan.Position.X + 10;
                    throwableHitbox.Y = (int)gMan.Position.Y - 80;
                //}
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(throwable, throwableHitbox, Color.White);
            spriteBatch.DrawString(font, "xPosition: " + playerPosition.X, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "yPosition: " + playerPosition.Y, new Vector2(0, 30), Color.White);
        }
    }
}
