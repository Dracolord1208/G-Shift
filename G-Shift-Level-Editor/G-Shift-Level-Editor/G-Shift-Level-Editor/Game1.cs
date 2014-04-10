using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace G_Shift_Level_Editor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Vector2 topLeft, botRight;

        List<Texture2D> mapTiles;
        List<Rectangle> mapRects;

        Vector2 translation;

        MouseState prevMouseState;
        KeyboardState prevKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1000;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            mapTiles = new List<Texture2D>();
            mapRects = new List<Rectangle>();
            translation = new Vector2();

            topLeft = new Vector2(-1);
            botRight = new Vector2(-1);

            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            String s = "Map";
            Texture2D temp = Content.Load<Texture2D>(s);
            mapTiles.Add(temp);
             temp = Content.Load<Texture2D>(s);
            mapTiles.Add(temp);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState mState = Mouse.GetState();
            KeyboardState kState = Keyboard.GetState();

            if (mState.X < 40 && mState.X >= 0)
                translation.X -= 5;
            else if (mState.X > 960 && mState.X <= 1000)
                translation.X += 5;

            if (mState.Y < 40 && mState.Y >= 0)
                translation.Y -= 2;
            else if (mState.Y > 560 && mState.Y <= 600)
                translation.Y += 2;

            if (mState.LeftButton == ButtonState.Pressed &&
                prevMouseState.LeftButton == ButtonState.Released &&
                topLeft.X == -1)
            {
                topLeft.X = mState.X + translation.X;
                topLeft.Y = mState.Y + translation.Y;
            }
            else
            if (mState.LeftButton == ButtonState.Pressed &&
                prevMouseState.LeftButton == ButtonState.Released &&
                topLeft.X != -1)
            {
                botRight.X = mState.X + translation.X;
                botRight.Y = mState.Y + translation.Y;

                Rectangle temp = new Rectangle();
                temp.X = (int)topLeft.X;
                temp.Y = (int)topLeft.Y;
                temp.Width = (int)(botRight.X - topLeft.X);
                temp.Height = (int)(botRight.Y - topLeft.Y);

                mapRects.Add(temp);

                topLeft.X = -1;
            }

            if (prevKeyboardState.IsKeyUp(Keys.Enter)
                && kState.IsKeyDown(Keys.Enter))
            {
                String s = "";

                foreach (Rectangle rect in mapRects)
                {
                    s += "Rect " + rect.X + " " + rect.Y + " " +
                        rect.Width + " " + rect.Height + Environment.NewLine;
                }

                File.WriteAllText("level.txt", s);
            }

            prevMouseState = mState;
            prevKeyboardState = kState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null,
                Matrix.CreateTranslation(-translation.X, -translation.Y, 0));

            spriteBatch.Draw(mapTiles[0], new Vector2(0, 0), Color.White);
            int prevXVal = 0;
            for (int x = 1; x < mapTiles.Count; x++)
            {
                spriteBatch.Draw(mapTiles[x], new Vector2(prevXVal + mapTiles[x].Width, 0), Color.White);
                prevXVal += mapTiles[x].Width;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
