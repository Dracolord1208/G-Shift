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
        ContentManager gameContents;
        Texture2D throwable;
        Texture2D baseRectangle;
        Rectangle throwableHitbox;
        Rectangle playerPosition;
        Rectangle bottomPlayerHitbox;
        Rectangle bottomObjectHitbox;
        int objectPositionX;
        int objectPositionY;
        SpriteFont font;
        float x;
        float y;
        //float angle;
        bool pickedUp;
        bool goUp;
        bool inIfStatement;
        bool stillIntersects;
        bool xPressed;
        bool goneUp;
        bool top;
        bool bottom;
        bool left;
        bool right;
        bool canMoveDown;
        bool canMoveUp;
        bool turnNinetyDegrees;
        bool offScreen;
        bool charDirection;
        bool inAir;
        int throwCount;
        Vector2 speed;
        bool canThrow;

        public void initialize(ContentManager Content, string name)
        {
            gameContents = Content;
            font = Content.Load<SpriteFont>("test");
            throwable = Content.Load<Texture2D>(name);
            throwable.Name = name;
            baseRectangle = Content.Load<Texture2D>("Rectangle");
            //throwableHitbox = new Rectangle(0, 0, throwable.Width, throwable.Height);

            if (throwable.Name.CompareTo("crate") == 0)
            {
                throwableHitbox = new Rectangle(0, 0, 50, 100);
            }
            else if (throwable.Name.CompareTo("barrel") == 0)
            {
                throwableHitbox = new Rectangle(0, 0, 100, 100);
            }
            else
                throwableHitbox = new Rectangle(0, 0, throwable.Width, throwable.Height);

            bottomObjectHitbox = new Rectangle(throwableHitbox.X, throwableHitbox.Y + throwableHitbox.Height, throwableHitbox.Width, 10);
            playerPosition = new Rectangle(0, 0, 50, 250);
            bottomPlayerHitbox = new Rectangle(playerPosition.X, playerPosition.Y + playerPosition.Height - 150, 70, 10);
            pickedUp = false;
            goUp = false;
            inIfStatement = false;
            stillIntersects = false;
            xPressed = false;
            goneUp = false;
            top = false;
            bottom = false;
            left = false;
            right = false;
            canMoveDown = true;
            canMoveUp = true;
            //turnNinetyDegrees = false;
            offScreen = false;
            charDirection = false;
            inAir = false;
            canThrow = false;
            throwCount = 0;

            speed = new Vector2(25, 0);
        }

        public void setItemPosition(Vector2 position)
        {
            throwableHitbox.X = (int)position.X;
            throwableHitbox.Y = (int)position.Y;
        }

        public Rectangle itemHitbox()
        {
            return throwableHitbox;
        }

        public bool itemBeingThrown()
        {
            return inAir;
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

        public void Update(Player gMan, ContentManager content, GraphicsDeviceManager graphics)
        {
            playerPosition.X = (int)gMan.Position.X;
            playerPosition.Y = (int)gMan.Position.Y;
            objectPositionX = throwableHitbox.X;
            objectPositionY = throwableHitbox.Y;
            bottomPlayerHitbox.X = playerPosition.X - 40;
            bottomPlayerHitbox.Y = playerPosition.Y + playerPosition.Height - 250;
            bottomPlayerHitbox.Width = playerPosition.Width + 20;
            bottomObjectHitbox.X = throwableHitbox.X;
            bottomObjectHitbox.Y = throwableHitbox.Y + throwableHitbox.Height - 26;
            bottomObjectHitbox.Width = throwableHitbox.Width;


            //int absDistanceX = Math.Abs(playerPosition.X - throwableHitbox.X);
            //int absDistanceY = Math.Abs(playerPosition.Y - throwableHitbox.Y);

            Collision();

            //x = gMan.motion.X;
            //y = gMan.motion.Y;
            //int xDistance = Math.Abs(throwableHitbox.X - (int)gMan.motion.X);
            //int yDistance = Math.Abs(throwableHitbox.Y - (int)gMan.motion.Y);

            if (playerPosition.Intersects(throwableHitbox) && Keyboard.GetState().IsKeyDown(Keys.X))
            {
                //if (Keyboard.GetState().IsKeyDown(Keys.Space))
                //{
                if (throwable.Name.CompareTo("barrel") == 0)
                {
                    //turnNinetyDegrees = true;
                    //angle = (float)Math.PI / 2;
                    throwable = gameContents.Load<Texture2D>("barrel2");
                    throwable.Name = "barrel2";
                }

                if (throwable.Name.CompareTo("BOXLARGE1") == 0)
                {
                    gMan.Position.X = throwableHitbox.X;
                    gMan.Position.Y = throwableHitbox.Y;
                }
                else
                {
                    throwableHitbox.X = (int)gMan.Position.X + 10;
                    throwableHitbox.Y = (int)gMan.Position.Y - 300;
                    pickedUp = true;
                    xPressed = false;
                    stillIntersects = true;
                    inAir = false;
                    canThrow = false;
                }
                //}


            }

            if (pickedUp && !xPressed)
            {
                charDirection = gMan.getDirection();

                if (charDirection)
                {
                    if (throwable.Name.CompareTo("barrel2") == 0)
                    {
                        throwableHitbox.X = (int)gMan.Position.X;
                        throwableHitbox.Y = (int)gMan.Position.Y - 200;
                    }
                    else
                    {
                        throwableHitbox.X = (int)gMan.Position.X;
                        throwableHitbox.Y = (int)gMan.Position.Y - 250;
                    }
                }
                else
                {
                    if (throwable.Name.CompareTo("barrel2") == 0)
                    {
                        throwableHitbox.X = (int)gMan.Position.X - 100;
                        throwableHitbox.Y = (int)gMan.Position.Y - 200;
                    }
                    else
                    {
                        throwableHitbox.X = (int)gMan.Position.X - 100;
                        throwableHitbox.Y = (int)gMan.Position.Y - 250;
                    }
                }
                goneUp = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                if (canThrow)
                {
                    if (throwable.Name.CompareTo("BOXLARGE1") != 0)
                    {
                        goUp = true;
                        stillIntersects = false;
                        xPressed = true;
                        charDirection = gMan.getDirection();
                    }
                }
            }
            else
                canThrow = true;

            if (goUp && pickedUp && xPressed)
            {
                //if (throwableHitbox.Y > 100 && goneUp)
                //if (speed.Y > 1 && goneUp)
                /*if (goneUp)
                {

                    // if (!gMan.getDirection())

                    if (charDirection)
                    {
                        throwableHitbox.X += (int)speed.X;
                        throwableHitbox.Y -= (int)speed.Y;
                        inIfStatement = true;
                        speed.Y--;
                    }
                    else
                    {
                        throwableHitbox.X -= (int)speed.X;
                        throwableHitbox.Y -= (int)speed.Y;
                        //inIfStatement = true;
                        speed.Y--;
                    }

                    inAir = true;
                }
                else
                {
                    goneUp = false;*/
                //if (throwableHitbox.Y < 350)
                if (speed.Y < 15)
                {


                    //if (!gMan.getDirection())

                    if (charDirection)
                    {
                        throwableHitbox.X += (int)speed.X;
                        throwableHitbox.Y += (int)speed.Y;
                        inIfStatement = true;
                        speed.Y++;
                    }
                    else
                    {
                        throwableHitbox.X -= (int)speed.X;
                        throwableHitbox.Y += (int)speed.Y;
                        //inIfStatement = true;
                        speed.Y++;
                    }

                    //throwableHitbox.X += (int)speed.X;
                    //throwableHitbox.Y += (int)speed.Y;
                    //speed.Y++;
                    inAir = true;
                }
                else
                {
                    goUp = false;
                    pickedUp = false;
                    xPressed = false;
                    inAir = false;

                    if (throwable.Name.CompareTo("barrel2") != 0)
                        throwCount++;

                    speed.Y = 0;
                }

                if (throwable.Name.CompareTo("BOXSMALL1") == 0 || throwable.Name.CompareTo("BOXSMALL2") == 0)
                {
                    if (throwCount == 1)
                    {
                        throwable = content.Load<Texture2D>("BOXSMALL2");
                        throwable.Name = "BOXSMALL2";
                    }
                    else if (throwCount == 2)
                    {
                        throwable = content.Load<Texture2D>("BOXSMALL3");
                        throwable.Name = "BOXSMALL3";
                    }

                }
            }
            //}

            if (throwable.Name.CompareTo("barrel2") == 0 && !offScreen)
            {
                if (charDirection)
                {
                    throwableHitbox.X += 4;

                    if (throwableHitbox.X - throwableHitbox.Height > gMan.Position.X + 500)
                        offScreen = true;
                }
                else
                {
                    throwableHitbox.X -= 4;

                    if (throwableHitbox.X + throwableHitbox.Height < gMan.Position.X - 500)
                        offScreen = true;
                }
            }

        }

        public bool itemDestroyed()
        {
            if (throwCount == 2)
                return true;
            else
                return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (turnNinetyDegrees)
            //{
            //spriteBatch.Draw(throwable, throwableHitbox, null, Color.White, angle, new Vector2(throwableHitbox.X, throwableHitbox.Y), SpriteEffects.None, 0);
            //}
            //else
            spriteBatch.Draw(throwable, throwableHitbox, Color.White);
            /*spriteBatch.Draw(baseRectangle, bottomPlayerHitbox, Color.Red);
            spriteBatch.Draw(baseRectangle, bottomObjectHitbox, Color.Blue);
            spriteBatch.DrawString(font, "xPosition: " + playerPosition.X, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "yPosition: " + playerPosition.Y, new Vector2(0, 30), Color.White);
            spriteBatch.DrawString(font, "objectX: " + objectPositionX, new Vector2(0, 60), Color.White);
            spriteBatch.DrawString(font, "objectY: " + objectPositionY, new Vector2(0, 90), Color.White);
            //spriteBatch.DrawString(font, "up: " + goUp, new Vector2(0, 110), Color.White);*/
            //spriteBatch.DrawString(font, "if: " + inIfStatement, new Vector2(0, 130), Color.White);
            //spriteBatch.DrawString(font, "intersects: " + stillIntersects, new Vector2(0, 160), Color.White);
            //spriteBatch.DrawString(font, "pushUp: " + canMoveUp, new Vector2(0, 180), Color.White);
            //spriteBatch.DrawString(font, "pushDown: " + canMoveDown, new Vector2(0, 200), Color.White);
        }
    }
}
