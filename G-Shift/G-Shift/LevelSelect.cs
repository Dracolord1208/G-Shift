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
    class LevelSelect
    {
        private Vector2 promptButtonPosition1;
        private Vector2 promptButtonPosition2;
        private Vector2 promptButtonPosition3;
        private Vector2 promptButtonPosition4;
        MouseState mouseState;
        MouseState previousMouseState;
        SpriteFont font;
        private Texture2D exitButton;
        private Texture2D badge;
        int width;
        int height;
        int choose=0;
        bool [] unlocked ;
        public int levelThatWasChosen=-1;
        public bool selected = true;
        
        public void Initialize(int x, int y)
        {
            width=x;
            height=y;
            promptButtonPosition1 = new Vector2((width / 4) - 50, 250);
            promptButtonPosition2 = new Vector2((width / 4) - 50, 290);
            promptButtonPosition3 = new Vector2((width / 4) - 50, 340);
            promptButtonPosition4 = new Vector2((width / 4) - 50, 390);
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
            unlocked = new bool[4] { true, false, false, false };
            levelThatWasChosen = -1;
            selected = true;
        }
        public void LoadCont(Texture2D texture,SpriteFont fon,Texture2D badg)
        {
            exitButton = texture;
            font = fon;
            badge = badg;
        }
        public void LoadContent(ContentManager content)
        {
            
           
        }

        public  void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            selected = true;
                if (currentKeyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
                {
                    choose--;
                    if (choose > 3)
                        choose = 0;
                    if (choose < 0)
                        choose = 3;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
                {
                    choose++;
                    if (choose > 3)
                        choose = 0;
                    if (choose < 0)
                        choose = 3;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                {
                    //level selected
                    if (unlocked[choose] == true)
                    {
                        levelThatWasChosen = choose;
                        selected = false;
                    }
                }

                //wait for mouseclick
                mouseState = Mouse.GetState();
                if ((previousMouseState.LeftButton == ButtonState.Pressed &&
                  mouseState.LeftButton == ButtonState.Released))
                {
                    //MediaPlayer.Pause();
                    MouseClicked(mouseState.X, mouseState.Y);
                }
                previousMouseState = mouseState;

        }

        void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
            Rectangle promptButtonRectangle1 = new Rectangle((int)promptButtonPosition1.X, (int)promptButtonPosition1.Y, 100, 15);
            Rectangle promptButtonRectangle2 = new Rectangle((int)promptButtonPosition2.X, (int)promptButtonPosition2.Y, 100, 15);
            Rectangle promptButtonRectangle3 = new Rectangle((int)promptButtonPosition3.X, (int)promptButtonPosition3.Y, 100, 15);
            Rectangle promptButtonRectangle4 = new Rectangle((int)promptButtonPosition4.X, (int)promptButtonPosition4.Y, 100, 15);

            if ((mouseClickRect.Intersects(promptButtonRectangle1))) //player clicked start button
            {
                if (unlocked[0] == true)
                {
                    levelThatWasChosen = choose;
                    selected = false;
                }
            }
            if (mouseClickRect.Intersects(promptButtonRectangle2)) //player clicked exit button
            {
                if (unlocked[1] == true)
                {
                    levelThatWasChosen = choose;
                    selected = false;
                }
            }
            if (mouseClickRect.Intersects(promptButtonRectangle3)) //player clicked exit button
            {
                if (unlocked[2] == true)
                {
                    levelThatWasChosen = choose;
                    selected = false;
                }
            }
            if ((mouseClickRect.Intersects(promptButtonRectangle4))) //player clicked start button
            {
                if (unlocked[3] == true)
                {
                    levelThatWasChosen = choose;
                    selected = false;
                }
            }
        }
        public void unlockLevel(int whattounlock)
        {
            unlocked[whattounlock] = true;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle cusor = new Rectangle((int)promptButtonPosition1.X-70, (int)promptButtonPosition1.Y-40, 50, 50); 
            if(choose==0)
                     cusor= new Rectangle((int)promptButtonPosition1.X-100, (int)promptButtonPosition1.Y-30, 70, 70);
            if(choose==1)
                cusor = new Rectangle((int)promptButtonPosition2.X - 100, (int)promptButtonPosition2.Y - 30, 70, 70);
            if(choose==2)
                cusor = new Rectangle((int)promptButtonPosition3.X - 100, (int)promptButtonPosition3.Y - 30,70, 70);
            if(choose==3)
                cusor = new Rectangle((int)promptButtonPosition4.X - 100, (int)promptButtonPosition4.Y - 30, 70, 70);
                    
                    spriteBatch.Draw(exitButton, promptButtonPosition1, Color.White);
                    spriteBatch.Draw(exitButton, promptButtonPosition2, Color.White);
                    spriteBatch.Draw(exitButton, promptButtonPosition3, Color.White);
                    spriteBatch.Draw(exitButton, promptButtonPosition4, Color.White);
                    spriteBatch.DrawString(font, "Level 1 ", new Vector2(promptButtonPosition1.X + 20, promptButtonPosition1.Y - 4), Color.White);
                    if (unlocked[1])
                        spriteBatch.DrawString(font, "Level 2 ", new Vector2(promptButtonPosition2.X + 20, promptButtonPosition2.Y - 4), Color.White);
                     else
                        spriteBatch.DrawString(font, "Locked ", new Vector2(promptButtonPosition2.X + 20, promptButtonPosition2.Y - 4), Color.White);
                    if (unlocked[2])
                        spriteBatch.DrawString(font, "Level 3 ", new Vector2(promptButtonPosition3.X + 20, promptButtonPosition3.Y - 4), Color.White);
                     else                        
                        spriteBatch.DrawString(font, "Locked ", new Vector2(promptButtonPosition3.X + 20, promptButtonPosition3.Y - 4), Color.White);
                    if (unlocked[3])
                        spriteBatch.DrawString(font, "Level 4 ", new Vector2(promptButtonPosition4.X + 20, promptButtonPosition4.Y - 4), Color.White);
                    else
                        spriteBatch.DrawString(font, "Locked ", new Vector2(promptButtonPosition4.X + 20, promptButtonPosition4.Y - 4), Color.White);
                    spriteBatch.Draw(badge, cusor, Color.White);
        }

    }
}
