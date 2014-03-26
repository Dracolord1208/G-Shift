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
        //Texture2D baseRectangle;
        Rectangle throwableHitbox;
        Rectangle playerPosition;
        Rectangle bottomPlayerHitbox;
        Rectangle bottomObjectHitbox;
        int objectPositionX;
        int objectPositionY;
        SpriteFont font;
        float x;
        float y;
        bool pickedUp;
        bool goUp;
        bool inIfStatement;
        bool stillIntersects;
        bool spacePressed;
        bool goneUp;
        bool top;
        bool bottom;
        bool left;
        bool right;
        bool canMoveDown;
        bool canMoveUp;

        public void initialize(ContentManager Content, string name)
        {
            font = Content.Load<SpriteFont>("test");
            throwable = Content.Load<Texture2D>(name);
            //baseRectangle = Content.Load<Texture2D>("Rectangle");
            throwableHitbox = new Rectangle(0, 0, throwable.Width, throwable.Height);
            bottomObjectHitbox = new Rectangle(throwableHitbox.X, throwableHitbox.Y + throwableHitbox.Height - 40, throwableHitbox.Width, 10);
            playerPosition = new Rectangle(0, 0, 50, 250);
            bottomPlayerHitbox = new Rectangle(playerPosition.X, playerPosition.Y + playerPosition.Height - 150, 70, 10);
            pickedUp = false;
            goUp = false;
            inIfStatement = false;
            stillIntersects = false;
            spacePressed = false;
            goneUp = false;
            top = false;
            bottom = false;
            left = false;
            right = false;
            canMoveDown = true;
            canMoveUp = true;
        }

        public void setItemPosition(Rectangle position)
        {
            throwableHitbox = position;
        }

        public bool getUpMove()
        {
            return canMoveUp;
        }

        public bool getDownMove()
        {
            return canMoveDown;
        }

        public void Collision()
        {

            int distanceX = bottomPlayerHitbox.X - bottomObjectHitbox.X;
            int distanceY = bottomPlayerHitbox.Y - bottomObjectHitbox.Y;

            if (bottomPlayerHitbox.Intersects(bottomObjectHitbox))
            {
                if (distanceY < 0)
                {
                    canMoveDown = false;
                }
                else if (distanceY > 0)
                {
                    canMoveUp = false;
                }
            }
            else
            {
                canMoveDown = true;
                canMoveUp = true;
            }
        }

        public void Update(Player gMan)
        {
            playerPosition.X = (int)gMan.Position.X;
            playerPosition.Y = (int)gMan.Position.Y;
            objectPositionX = throwableHitbox.X;
            objectPositionY = throwableHitbox.Y;
            bottomPlayerHitbox.X = playerPosition.X - 40;
            bottomPlayerHitbox.Y = playerPosition.Y + playerPosition.Height - 180;
            bottomPlayerHitbox.Width = playerPosition.Width + 20;
            bottomObjectHitbox.X = throwableHitbox.X;
            bottomObjectHitbox.Y = throwableHitbox.Y + throwableHitbox.Height - 26;
            bottomObjectHitbox.Width = throwableHitbox.Width;


            int absDistanceX = Math.Abs(playerPosition.X - throwableHitbox.X);
            int absDistanceY = Math.Abs(playerPosition.Y - throwableHitbox.Y);

            Collision();

            //x = gMan.motion.X;
            //y = gMan.motion.Y;
            //int xDistance = Math.Abs(throwableHitbox.X - (int)gMan.motion.X);
            //int yDistance = Math.Abs(throwableHitbox.Y - (int)gMan.motion.Y);

            if(playerPosition.Intersects(throwableHitbox) && Keyboard.GetState().IsKeyDown(Keys.X))
            {
                //if (Keyboard.GetState().IsKeyDown(Keys.Space))
                //{
                    throwableHitbox.X = (int)gMan.Position.X + 10;
                    throwableHitbox.Y = (int)gMan.Position.Y - 80;
                    pickedUp = true;
                    stillIntersects = true;
                //}

                    
            }

            if (pickedUp && !spacePressed)
            {
                throwableHitbox.X = (int)gMan.Position.X + 10;
                throwableHitbox.Y = (int)gMan.Position.Y - 80;
                goneUp = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                goUp = true;
                stillIntersects = false;
                spacePressed = true;
            }

            if (goUp && pickedUp)
            {
                if (throwableHitbox.Y > 100 && goneUp)
                {
                    throwableHitbox.X += 4;
                    throwableHitbox.Y -= 4;
                    inIfStatement = true;
                }
                else
                {
                    goneUp = false;
                    if (throwableHitbox.Y < 350)
                    {
                        throwableHitbox.X += 4;
                        throwableHitbox.Y += 4;
                    }
                    else
                    {
                        goUp = false;
                        pickedUp = false;
                        spacePressed = false;
                    }
                } 
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(throwable, throwableHitbox, Color.White);
            //spriteBatch.Draw(baseRectangle, bottomPlayerHitbox, Color.Red);
            //spriteBatch.Draw(baseRectangle, bottomObjectHitbox, Color.Blue);
            spriteBatch.DrawString(font, "xPosition: " + playerPosition.X, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "yPosition: " + playerPosition.Y, new Vector2(0, 30), Color.White);
            spriteBatch.DrawString(font, "objectX: " + objectPositionX, new Vector2(0, 60), Color.White);
            spriteBatch.DrawString(font, "objectY: " + objectPositionY, new Vector2(0, 90), Color.White);
            spriteBatch.DrawString(font, "up: " + goUp, new Vector2(0, 110), Color.White);
            spriteBatch.DrawString(font, "if: " + inIfStatement, new Vector2(0, 130), Color.White);
            spriteBatch.DrawString(font, "intersects: " + stillIntersects, new Vector2(0, 160), Color.White);
            spriteBatch.DrawString(font, "pushUp: " + canMoveUp, new Vector2(0, 180), Color.White);
            spriteBatch.DrawString(font, "pushDown: " + canMoveDown, new Vector2(0, 200), Color.White);
        }
    }
}
