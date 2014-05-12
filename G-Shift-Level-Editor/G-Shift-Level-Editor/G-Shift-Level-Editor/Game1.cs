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

        struct BattleEvent
        {
            public List<Item> locs;
        }

        struct Item
        {
            public Vector2 loc;
            public String name;
        }

        struct Track
        {
            public Vector2 topLeft;
            public Vector2 topRight;
            public Vector2 botLeft;
            public Vector2 botRight;
        }

        struct MenuBox
        {
            public Texture2D img;
            public Rectangle rect;
        }

        List<Texture2D> mapTiles;
        List<Rectangle> mapRects;
        List<Rectangle> eventRects;
        List<Item> items;
        List<Track> tracks;
        List<MenuBox> menuChoices;
        List<BattleEvent> battleEvents;

        Texture2D outline;
        Texture2D pathUp;
        Texture2D pathDown;

        Vector2 translation;

        MouseState prevMouseState;
        KeyboardState prevKeyboardState;

        bool itemDrag;
        bool isEnemy;
        Item tempItem;

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
            items = new List<Item>();
            tracks = new List<Track>();
            menuChoices = new List<MenuBox>();
            battleEvents = new List<BattleEvent>();
            eventRects = new List<Rectangle>();

            translation = new Vector2();

            topLeft = new Vector2(-1);
            botRight = new Vector2(-1);

            tempItem = new Item();
            itemDrag = false;
            isEnemy = false;

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

            outline = Content.Load<Texture2D>("RectOutline");
            pathUp = Content.Load<Texture2D>("PathUp");
            pathDown = Content.Load<Texture2D>("PathDown");

            String s = "Map";
            Texture2D temp = Content.Load<Texture2D>(s);
            mapTiles.Add(temp);

            MenuBox tempMenu = new MenuBox();

            tempMenu.img = Content.Load<Texture2D>("BOXSMALL1");
            tempMenu.rect = new Rectangle(0, 0, 40, 40);
            menuChoices.Add(tempMenu);

            tempMenu.img = Content.Load<Texture2D>("barrel");
            tempMenu.rect = new Rectangle(40, 0, 40, 40);
            menuChoices.Add(tempMenu);

            tempMenu.img = Content.Load<Texture2D>("BOXLARGE1");
            tempMenu.rect = new Rectangle(80, 0, 40, 40);
            menuChoices.Add(tempMenu);

            tempMenu.img = Content.Load<Texture2D>("SmallBot");
            tempMenu.rect = new Rectangle(120, 0, 40, 40);
            menuChoices.Add(tempMenu);

            tempMenu.img = Content.Load<Texture2D>("MediumBot");
            tempMenu.rect = new Rectangle(160, 0, 40, 40);
            menuChoices.Add(tempMenu);

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
            if (mState.Y > 40)
            {
                if (mState.X < 40 && mState.X >= 0)
                    translation.X -= 5;
                else if (mState.X > 960 && mState.X <= 1000)
                    translation.X += 5;
            }
            /*if (mState.Y < 40 && mState.Y >= 0)
                translation.Y -= 2;
            else if (mState.Y > 560 && mState.Y <= 600)
                translation.Y += 2;*/

            if (itemDrag)
            {
                tempItem.loc.X = mState.X;
                tempItem.loc.Y = mState.Y;

                if (mState.LeftButton == ButtonState.Pressed &&
                    prevMouseState.LeftButton == ButtonState.Released &&
                    !isEnemy)
                {
                    tempItem.loc.X += translation.X;
                    tempItem.loc.Y += translation.Y;
                    itemDrag = false;
                    items.Add(tempItem);
                    tempItem.name = "";
                }
                else if (mState.LeftButton == ButtonState.Pressed &&
                    prevMouseState.LeftButton == ButtonState.Released)
                {
                    tempItem.loc.X += translation.X;
                    tempItem.loc.Y += translation.Y;
                    itemDrag = false;
                    isEnemy = false;
                    battleEvents[battleEvents.Count-1].locs.Add(tempItem);
                    tempItem.name = "";
                }
            }
            else
            {
                if (kState.IsKeyDown(Keys.E) &&
                    prevKeyboardState.IsKeyDown(Keys.E))
                {
                    BattleEvent temp = new BattleEvent();
                    temp.locs = new List<Item>();
                    battleEvents.Add(temp);

                    Rectangle rect = new Rectangle();
                    rect.X = (int)translation.X;
                    rect.Y = (int)translation.Y;
                    rect.Width = graphics.GraphicsDevice.Viewport.Width;
                    rect.Height = graphics.GraphicsDevice.Viewport.Height;
                    eventRects.Add(rect);
                }

                if (mState.LeftButton == ButtonState.Pressed &&
                    prevMouseState.LeftButton == ButtonState.Released &&
                    topLeft.X == -1 && mState.Y > 40)
                {
                    topLeft.X = mState.X + translation.X;
                    topLeft.Y = mState.Y + translation.Y;
                }
                else
                    if (mState.LeftButton == ButtonState.Pressed &&
                        prevMouseState.LeftButton == ButtonState.Released &&
                        topLeft.X != -1 && mState.Y > 40)
                    {
                        botRight.X = mState.X + translation.X;
                        botRight.Y = mState.Y + translation.Y;

                        Rectangle temp = new Rectangle();
                        temp.X = (int)topLeft.X;
                        temp.Y = (int)topLeft.Y;
                        temp.Width = (int)(botRight.X - topLeft.X);
                        temp.Height = (int)(botRight.Y - topLeft.Y);

                        mapRects.Add(temp);

                        if (mapRects.Count > 1)
                        {
                            Pathify();
                        }

                        topLeft.X = -1;
                    }

                if (prevMouseState.LeftButton == ButtonState.Released &&
                    mState.LeftButton == ButtonState.Pressed &&
                    mState.Y <= 40 && !itemDrag)
                {
                    if (mState.X <= 40)
                    {
                        itemDrag = true;
                        tempItem.loc = new Vector2(mState.X, mState.Y);
                        tempItem.name = "BOXSMALL1";
                    }
                    else
                        if (mState.X <= 80 && mState.X > 40)
                        {
                            itemDrag = true;
                            tempItem.loc = new Vector2(mState.X, mState.Y);
                            tempItem.name = "barrel";
                        }
                        else if (mState.X <= 120 && mState.X > 80)
                        {
                            itemDrag = true;
                            tempItem.loc = new Vector2(mState.X, mState.Y);
                            tempItem.name = "BOXLARGE1";
                        }
                        else
                            if (mState.X <= 160 && mState.X > 120 && battleEvents.Count > 0)
                            {
                                itemDrag = true;
                                isEnemy = true;
                                tempItem.loc = new Vector2(mState.X, mState.Y);
                                tempItem.name = "SmallBot";
                            }
                            else
                                if (mState.X <= 200 && mState.X > 160 && battleEvents.Count > 0)
                                {
                                    itemDrag = true;
                                    isEnemy = true;
                                    tempItem.loc = new Vector2(mState.X, mState.Y);
                                    tempItem.name = "MediumBot";
                                }
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

                    foreach (Track track in tracks)
                    {
                        s += "Track " + track.topLeft.X + " " + track.topLeft.Y + " " +
                            track.topRight.X + " " + track.topRight.Y + " " +
                            track.botLeft.X + " " + track.botLeft.Y + " " +
                            track.botRight.X + " " + track.botRight.Y + Environment.NewLine;
                    }

                    foreach (Item item in items)
                    {
                        s += "Item " + item.loc.X + " " + item.loc.Y +
                            " " + item.name + Environment.NewLine;
                    }

                    File.WriteAllText("level.txt", s);
                }
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

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null,
                Matrix.CreateTranslation(-translation.X, -translation.Y, 0));

            spriteBatch.Draw(mapTiles[0], new Vector2(0, 0), Color.White);
            int prevXVal = 0;
            for (int x = 1; x < mapTiles.Count; x++)
            {
                spriteBatch.Draw(mapTiles[x], new Vector2(prevXVal + mapTiles[x].Width, 0), Color.White);
                prevXVal += mapTiles[x].Width;
            }

            foreach(Rectangle rect in mapRects)
            {
                spriteBatch.Draw(outline, rect, Color.Pink);
            }

            for(int x = 0; x < mapRects.Count-1; x++)
            {
                
                Rectangle rect = new Rectangle();
                if (mapRects[x].Y > mapRects[x + 1].Y)
                {
                    rect.X = (int)(tracks[x].topLeft.X);
                    rect.Y = (int)(tracks[x].topLeft.Y - (tracks[x].topLeft.Y - tracks[x].topRight.Y));
                    rect.Width = (int)(tracks[x].topRight.X - tracks[x].topLeft.X);
                    rect.Height = (int)(tracks[x].topLeft.Y - tracks[x].topRight.Y);

                    spriteBatch.Draw(pathUp, rect, Color.Blue);

                    if (mapRects[x].Bottom > mapRects[x + 1].Bottom)
                    {
                        rect.X = (int)(tracks[x].topLeft.X);
                        rect.Y = (int)(tracks[x].botLeft.Y - (tracks[x].botLeft.Y - tracks[x].botRight.Y));
                        rect.Width = (int)(tracks[x].topRight.X - tracks[x].topLeft.X);
                        rect.Height = (int)(tracks[x].botLeft.Y - tracks[x].botRight.Y);

                        spriteBatch.Draw(pathUp, rect, Color.Blue);
                    }
                    else
                    {
                        rect.X = (int)(tracks[x].topLeft.X);
                        rect.Y = (int)(tracks[x].botRight.Y - (tracks[x].botRight.Y - tracks[x].botLeft.Y));
                        rect.Width = (int)(tracks[x].topRight.X - tracks[x].topLeft.X);
                        rect.Height = (int)(tracks[x].botRight.Y - tracks[x].botLeft.Y);

                        spriteBatch.Draw(pathDown, rect, Color.Blue);
                    }
                }
                else
                {
                    rect.X = (int)(tracks[x].topLeft.X);
                    rect.Y = (int)(tracks[x].topLeft.Y - (tracks[x].topRight.Y - tracks[x].topRight.Y));
                    rect.Width = (int)(tracks[x].topRight.X - tracks[x].topLeft.X);
                    rect.Height = (int)(tracks[x].topRight.Y - tracks[x].topLeft.Y);

                    spriteBatch.Draw(pathDown, rect, Color.Blue);

                    if (mapRects[x].Bottom > mapRects[x + 1].Bottom)
                    {
                        rect.X = (int)(tracks[x].topLeft.X);
                        rect.Y = (int)(tracks[x].botLeft.Y - (tracks[x].botLeft.Y - tracks[x].botRight.Y));
                        rect.Width = (int)(tracks[x].topRight.X - tracks[x].topLeft.X);
                        rect.Height = (int)(tracks[x].botLeft.Y - tracks[x].botRight.Y);

                        spriteBatch.Draw(pathUp, rect, Color.Blue);
                    }
                    else
                    {
                        rect.X = (int)(tracks[x].topLeft.X);
                        rect.Y = (int)(tracks[x].botRight.Y - (tracks[x].botRight.Y - tracks[x].botLeft.Y));
                        rect.Width = (int)(tracks[x].topRight.X - tracks[x].topLeft.X);
                        rect.Height = (int)(tracks[x].botRight.Y - tracks[x].botLeft.Y);

                        spriteBatch.Draw(pathDown, rect, Color.Blue);
                    }
                }
            }

            foreach (Item item in items)
            {
                if(item.name == "BOXSMALL1")
                {
                    spriteBatch.Draw(menuChoices[0].img, item.loc, Color.White);
                }
                else
                if (item.name == "barrel")
                {
                    spriteBatch.Draw(menuChoices[1].img, item.loc, Color.White);
                }
                else
                if (item.name == "BOXLARGE1")
                {
                    spriteBatch.Draw(menuChoices[2].img, item.loc, Color.White);
                }
            }

            foreach (Rectangle rect in eventRects)
            {
                spriteBatch.Draw(outline, rect, Color.Tomato);
            }

            foreach (BattleEvent bEvent in battleEvents)
            {
                foreach (Item item in bEvent.locs)
                {
                    if (item.name == "SmallBot")
                    {
                        spriteBatch.Draw(menuChoices[3].img, item.loc, Color.White);
                    }
                    else
                    if (item.name == "MediumBot")
                    {
                        spriteBatch.Draw(menuChoices[4].img, item.loc, Color.White);
                    }
                }
            }

            spriteBatch.End();

            spriteBatch.Begin();

            foreach (MenuBox choice in menuChoices)
            {
                spriteBatch.Draw(choice.img, choice.rect, Color.White);
            }

            if (itemDrag)
            {
                if(tempItem.name == "BOXSMALL1")
                {
                    spriteBatch.Draw(menuChoices[0].img, tempItem.loc, Color.White);
                }
                if (tempItem.name == "barrel")
                {
                    spriteBatch.Draw(menuChoices[1].img, tempItem.loc, Color.White);
                }
                if (tempItem.name == "BOXLARGE1")
                {
                    spriteBatch.Draw(menuChoices[2].img, tempItem.loc, Color.White);
                }
                if (tempItem.name == "SmallBot")
                {
                    spriteBatch.Draw(menuChoices[3].img, tempItem.loc, Color.White);
                }
                if (tempItem.name == "MediumBot")
                {
                    spriteBatch.Draw(menuChoices[4].img, tempItem.loc, Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Pathify()
        {
            Track temp = new Track();

            

            temp.topLeft = new Vector2(
                            mapRects[mapRects.Count - 2].Right,
                            mapRects[mapRects.Count - 2].Top);
            temp.botLeft = new Vector2(
                            mapRects[mapRects.Count - 2].Right,
                            mapRects[mapRects.Count - 2].Bottom);
            temp.topRight = new Vector2(
                            mapRects[mapRects.Count - 1].Left,
                            mapRects[mapRects.Count - 1].Top);
            temp.botRight = new Vector2(
                            mapRects[mapRects.Count - 1].Left,
                            mapRects[mapRects.Count - 1].Bottom);

            tracks.Add(temp);
        }
    }
}
