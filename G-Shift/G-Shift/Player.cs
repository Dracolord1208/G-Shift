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
        public enum Stance
        {
            Standing,
            Left,
            Right,
            lightAttack,
            heavyAttack,
            hurt,
            death,
            revive,
            live,
            fall,
            moving
        }
        public Stance playerStance;
        public bool withinRect = true;
        public int location = 0;
        public float scrollPosition = 0;
        float playerMoveSpeed = 4f;
        const int SCREEN_WIDTH = 1000;
        const int SCREEN_HEIGHT = 600;
        const float LowerBounderyHeight = 150;
        public Animation PlayerAnimation;
        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;
        public Vector2 StartPosition;
        public bool hasBeenHit = false;
        bool canMoveUp;
        bool canMoveDown;
        // State of the player
        public bool Active;
        public bool ismoveing = false;
        public int Height { get; set; }
        public int Width { get; set; }
       public bool facing = true;//true==left false == right
        // Amount of hit points that player has
        public int Health;
        public bool gotlife;
        public int maxHealth;
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
        float WaitTimeToShowCard = 0;
        SoundEffect gunshot;
        public int depth { get; set; }
        // Initialize the player
        public Animation AStillleft; public Animation AStillRight;
        Texture2D Attack1Right; public Animation AAttack1Right;
        Texture2D Attack1Left; public Animation AAttack1Left;
        Texture2D DeathLeft; public Animation ADeathLeft;
        Texture2D DeathRight; public Animation ADeathRight;
        Texture2D FallLeft; public Animation AFallLeft;
        Texture2D FallRight; public Animation AFallRight;
        Texture2D LifeLeft; public Animation ALifeLeft;
        Texture2D LifeRight; public Animation ALifeRight;
        Texture2D RiseRight; public Animation ARiseRight;
        Texture2D RiseLeft; public Animation ARiseLeft;
        Texture2D WalkNoGunLeft; public Animation AWalkNoGunLeft;
        Texture2D WalkNoGunRight; public Animation AWalkNoGunRight;
        Texture2D WalkGunLeft; public Animation AWalkGunLeft;
        Texture2D WalkGunRight; public Animation AWalkGunRight;
        bool fire = false;
        bool hitonce;
        bool shotonce;
        Texture2D HurtLeft; Texture2D HurtRight;
        public Animation AHurtLeft; public Animation AHurtRight;
        public int heavyHit = 5;
        int Combo = 0;
        float attackTime;
        bool isAttacking;
        bool gun;
        TimeSpan fireTime;
        TimeSpan previousFireTime;
        int elapsedTime = 0;
        public Vector2 galPosition;//= new Vector2((int)Position.X - 85, (int)Position.Y - 200);
        public Rectangle hitBox;
        //public Rectangle hitbox { get; set; }
        //GamePadState previousGamePadState;
        bool isPunching;
        bool pressed3;
        public Vector2 screenPosition;
        //   float MaxAttackTime=2;
        //Content.RootDirectory = "Content";
        List<Animation> AttackRightList;
        List<Animation> AttackLeftList;
        List<Animation> RiseReftList;
        List<Animation> RiseRightList;
        List<Animation> DeathRightList;
        List<Animation> DeathLeftList;
        SoundEffect hitbyrobot;
        private void UpdateAR(GameTime gameTime)
        {
            for (int i = AttackRightList.Count - 1; i >= 0; i--)
            {
                AttackRightList[i].Update(gameTime);
                if (AttackRightList[i].Active == false)
                {
                    AttackRightList.RemoveAt(i);
                }
            }
        }
        private void AddAR(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(Attack1Right, new Vector2(StartPosition.X, Position.Y), 225, 250, 8, 60, Color.White, 1f, false);
            AttackRightList.Add(explosion);
        }
        private void UpdateAL(GameTime gameTime)
        {
            for (int i = AttackLeftList.Count - 1; i >= 0; i--)
            {
                AttackLeftList[i].Update(gameTime);
                if (AttackLeftList[i].Active == false)
                {
                    AttackLeftList.RemoveAt(i);
                }
            }
        }
        private void AddAL(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(Attack1Left, new Vector2(StartPosition.X, Position.Y), 225, 250, 8, 200, Color.White, 1f, false);
            AttackLeftList.Add(explosion);
        }
        private void UpdateRL(GameTime gameTime)
        {
            for (int i = RiseReftList.Count - 1; i >= 0; i--)
            {
                RiseReftList[i].Update(gameTime);
                if (RiseReftList[i].Active == false)
                {
                    RiseReftList.RemoveAt(i);
                }
            }
        }
        private void AddRL(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(Attack1Left, new Vector2(StartPosition.X, Position.Y), 225, 250, 8, 60, Color.White, 1f, false);
            RiseReftList.Add(explosion);
        }
        private void UpdateRR(GameTime gameTime)
        {
            for (int i = RiseRightList.Count - 1; i >= 0; i--)
            {
                RiseRightList[i].Update(gameTime);
                if (RiseRightList[i].Active == false)
                {
                    RiseRightList.RemoveAt(i);
                }
            }
        }
        private void AddRR(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(Attack1Left, new Vector2(StartPosition.X, Position.Y), 225, 250, 8, 60, Color.White, 1f, false);
            RiseRightList.Add(explosion);
        }
        private void UpdateDL(GameTime gameTime)
        {
            for (int i = AttackLeftList.Count - 1; i >= 0; i--)
            {
                AttackLeftList[i].Update(gameTime);
                if (AttackLeftList[i].Active == false)
                {
                    AttackLeftList.RemoveAt(i);
                }
            }
        }
        private void AddDL(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(Attack1Left, new Vector2(StartPosition.X, Position.Y), 225, 250, 8, 60, Color.White, 1f, false);
            AttackLeftList.Add(explosion);
        }
        private void UpdateDR(GameTime gameTime)
        {
            for (int i = AttackLeftList.Count - 1; i >= 0; i--)
            {
                AttackLeftList[i].Update(gameTime);
                if (AttackLeftList[i].Active == false)
                {
                    AttackLeftList.RemoveAt(i);
                }
            }
        }
        private void AddDR(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(Attack1Left, new Vector2(StartPosition.X, Position.Y), 225, 250, 8, 60, Color.White, 1f, false);
            AttackLeftList.Add(explosion);
        }
        public void LoadContent(ContentManager content)
        {

            galPosition = new Vector2((int)Position.X - 85, (int)Position.Y - 200);
            Attack1Right = content.Load<Texture2D>("Galager/Attack1");
            Attack1Left = content.Load<Texture2D>("Galager/Attack2");
            DeathLeft = content.Load<Texture2D>("Galager/DEATHLEft");
            DeathRight = content.Load<Texture2D>("Galager/DEATHRight");
            FallLeft = content.Load<Texture2D>("Galager/FALLANIMATON");
            FallRight = content.Load<Texture2D>("Galager/FALLANIMATONright");
            LifeLeft = content.Load<Texture2D>("Galager/LIFELEFT");
            LifeRight = content.Load<Texture2D>("Galager/LIFEright");
            RiseRight = content.Load<Texture2D>("Galager/LIFTANIMATONright");
            RiseLeft = content.Load<Texture2D>("Galager/LIFTANIMATON");
            WalkNoGunLeft = content.Load<Texture2D>("Galager/WALKINGNOGUN");
            WalkNoGunRight = content.Load<Texture2D>("Galager/WALKINGNOGUNRight1");
            WalkGunLeft = content.Load<Texture2D>("Galager/WALKINGWITHGUN");
            WalkGunRight = content.Load<Texture2D>("Galager/WALKINGWITHGUNRight1");
            HurtLeft = content.Load<Texture2D>("Galager/hurtleft");
            HurtRight = content.Load<Texture2D>("Galager/hurtright");
            gunshot = content.Load<SoundEffect>("Music/Skorpion-Kibblesbob-1109158827");
            hitbyrobot = content.Load<SoundEffect>("Music/punch_or_whack_-Vladimir-403040765");

            AHurtLeft.Initialize(HurtRight, galPosition, 225, 225, 1, 60, Color.White, 1f, true);
            AHurtRight.Initialize(HurtLeft, galPosition, 225, 225, 1, 60, Color.White, 1f, true);
            AAttack1Right.Initialize(Attack1Left, galPosition, 225, 250, 8, 90, Color.White, 1f, true);
            AAttack1Left.Initialize(Attack1Right, galPosition, 225, 250, 8, 90, Color.White, 1f, true);
            ADeathLeft.Initialize(DeathRight, galPosition, 225, 250, 8, 60, Color.White, 1f, true);
            ADeathRight.Initialize(DeathLeft, galPosition, 225, 250, 8, 60, Color.White, 1f, true);
            AFallLeft.Initialize(FallLeft, galPosition, 225, 225, 7, 60, Color.White, 1f, true);
            AFallRight.Initialize(FallRight, galPosition, 225, 225, 7, 60, Color.White, 1f, true);
            ALifeLeft.Initialize(LifeLeft, galPosition, 225, 250, 8, 60, Color.White, 1f, true);
            ALifeRight.Initialize(LifeRight, galPosition, 225, 250, 8, 60, Color.White, 1f, true);
            ARiseRight.Initialize(RiseRight, galPosition, 225, 225, 7, 60, Color.White, 1f, true);
            ARiseLeft.Initialize(RiseLeft, galPosition, 225, 225, 7, 60, Color.White, 1f, true);
            AWalkNoGunLeft.Initialize(WalkNoGunLeft, galPosition, 225, 225, 6, 60, Color.White, 1f, true);
            AWalkNoGunRight.Initialize(WalkNoGunRight, galPosition, 225, 225, 6, 60, Color.White, 1f, true);
            AWalkGunLeft.Initialize(WalkGunLeft, galPosition, 225, 225, 6, 60, Color.White, 1f, true);
            AWalkGunRight.Initialize(WalkGunRight, galPosition, 225, 225, 6, 60, Color.White, 1f, true);
            AStillleft.Initialize(Attack1Left, galPosition, 225, 250, 1, 60, Color.White, 1f, true);
            AStillRight.Initialize(Attack1Right, galPosition, 225, 250, 1, 60, Color.White, 1f, true);
        }
        public void Initialize(Texture2D playerTexture, Vector2 position)
        {

            AttackRightList = new List<Animation>();
            AttackLeftList = new List<Animation>();
            RiseReftList = new List<Animation>();
            RiseRightList = new List<Animation>();
            DeathRightList = new List<Animation>();
            DeathLeftList = new List<Animation>();
            pressed3 = true;
            //set animation
            fireTime = TimeSpan.FromSeconds(.30f);
            // Set the starting position of the player around the middle of the screen and to the back
            StartPosition = Position = position;
            AStillleft = new Animation();
            AStillRight = new Animation();
            AAttack1Right = new Animation();
            AAttack1Left = new Animation();
            ADeathLeft = new Animation();
            ADeathRight = new Animation();
            AFallLeft = new Animation();
            AFallRight = new Animation();
            ALifeLeft = new Animation();
            ALifeRight = new Animation();
            ARiseRight = new Animation();
            ARiseLeft = new Animation();
            AWalkNoGunLeft = new Animation();
            AWalkNoGunRight = new Animation();
            AWalkGunLeft = new Animation();
            AWalkGunRight = new Animation();
            // Set the player to be active
            Active = true;
            playerStance = Stance.Standing;
            //  playerAnimation.Initialize(gManTest1, Vector2.Zero, 225, 225, 6, 30, Color.White, 1f, true);
            AHurtRight = new Animation();
            AHurtLeft = new Animation();
            //  PlayerAnimation = playerAnimation;
            // Set the player health
            maxHealth = 500;
            Health = 5;
            canMoveUp = true;
            canMoveDown = true;
            hitBox = new Rectangle((int)Position.X - 100, (int)Position.Y - 30, 200, 50);
        }

        // Update the player animation
        //public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, GamePadState currentGamePadState, bool canMoveUp, bool canMoveDown, List<Item> allItems, World level)
        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, GamePadState currentGamePadState, GamePadState previousGamePadState, List<Item> allItems, World level)
        {
            if (Health > 0)
            {
                ismoveing = false;
                screenPosition = new Vector2(StartPosition.X, Position.Y);
                AAttack1Right.Position = new Vector2(StartPosition.X, Position.Y);
                AAttack1Left.Position = new Vector2(StartPosition.X, Position.Y);
                ADeathLeft.Position = new Vector2(StartPosition.X, Position.Y);
                ADeathRight.Position = new Vector2(StartPosition.X, Position.Y);
                AFallLeft.Position = new Vector2(StartPosition.X, Position.Y);
                AFallRight.Position = new Vector2(StartPosition.X, Position.Y);
                ALifeLeft.Position = new Vector2(StartPosition.X, Position.Y);
                ALifeRight.Position = new Vector2(StartPosition.X, Position.Y);
                ARiseRight.Position = new Vector2(StartPosition.X, Position.Y);
                ARiseLeft.Position = new Vector2(StartPosition.X, Position.Y);
                AWalkNoGunLeft.Position = new Vector2(StartPosition.X + 43, Position.Y);
                AWalkNoGunRight.Position = new Vector2(StartPosition.X, Position.Y);
                AWalkGunLeft.Position = new Vector2(StartPosition.X, Position.Y);
                AWalkGunRight.Position = new Vector2(StartPosition.X, Position.Y);
                AStillleft.Position = new Vector2(StartPosition.X, Position.Y);
                AStillRight.Position = new Vector2(StartPosition.X, Position.Y);
                AHurtLeft.Position = new Vector2(StartPosition.X, Position.Y);
                AHurtRight.Position = new Vector2(StartPosition.X, Position.Y);
                ADeathLeft.Update(gameTime);
                ADeathRight.Update(gameTime);
                AFallLeft.Update(gameTime);
                AFallRight.Update(gameTime);
                ALifeLeft.Update(gameTime);
                ALifeRight.Update(gameTime);
                ARiseRight.Update(gameTime);
                ARiseLeft.Update(gameTime);
                AWalkNoGunLeft.Update(gameTime);
                AWalkNoGunRight.Update(gameTime);
                AWalkGunLeft.Update(gameTime);
                AWalkGunRight.Update(gameTime);
                AStillleft.Update(gameTime);
                AStillRight.Update(gameTime);
                AAttack1Right.Update(gameTime);
                AAttack1Left.Update(gameTime);
                //      public Animation AHurtLeft; public Animation AHurtRight;
                AHurtLeft.Update(gameTime);
                AHurtRight.Update(gameTime);
                //  currentKeyboardState=   Keyboard.GetState();
                // Move background texture 400 pixels each second 
                StanceMoves(gameTime);
                UpdateAR(gameTime);
                UpdateAL(gameTime);
                UpdateRR(gameTime);
                UpdateRL(gameTime);
                UpdateDL(gameTime);
                UpdateDR(gameTime);
                motion = new Vector2(0, 0);
                //Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
                //Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;
                //////handGunA.position = new Vector2(position.X + 50, position.Y + 100);
                galPosition = new Vector2((int)Position.X - 85, (int)Position.Y - 200);
                // Use the Keyboard / Dpad
                bool animators = false;
                //   previousGamePadState = currentGamePadState;
                if (AttackRightList.Count == 0)
                    animators = true;
                if (AttackLeftList.Count == 0)
                    animators = true;
                if (RiseReftList.Count == 0)
                    animators = true;
                if (RiseRightList.Count == 0)
                    animators = true;
                if (DeathRightList.Count == 0)
                    animators = true;
                if (DeathLeftList.Count == 0)
                    animators = true;

                if ((currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.J) ||
                currentGamePadState.DPad.Left == ButtonState.Pressed || currentGamePadState.ThumbSticks.Left.X < 0) && !previousKeyboardState.IsKeyDown(Keys.C) && pressed3 == false)
                {
                    if (animators)
                        Position.X -= playerMoveSpeed;
                    facing = false;
                    playerStance = Stance.Left;
                }
                if ((currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.L) ||
                currentGamePadState.DPad.Right == ButtonState.Pressed || currentGamePadState.ThumbSticks.Left.X > 0) && !previousKeyboardState.IsKeyDown(Keys.C) && pressed3 == false)
                {
                    if (animators)
                        Position.X += playerMoveSpeed;
                    facing = true;
                    playerStance = Stance.Right;
                }

                canMoveUp = true;

                if ((currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W) || currentKeyboardState.IsKeyDown(Keys.I) ||
                    currentGamePadState.DPad.Up == ButtonState.Pressed || currentGamePadState.ThumbSticks.Left.Y > 0) && !previousKeyboardState.IsKeyDown(Keys.C) && pressed3 == false)
                {
                    ismoveing = true;
                    playerStance = Stance.moving;
                    for (int i = 0; i < allItems.Count; i++)
                    {
                        if (!(allItems[i].getUpMove()))
                        {
                            canMoveUp = false;
                        }
                    }

                    if (canMoveUp && animators)
                        Position.Y -= playerMoveSpeed;

                }

                canMoveDown = true;
                //if (currentKeyboardState.IsKeyDown(Keys.Down))
                  //  Health=0;
                if ((currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S) || currentKeyboardState.IsKeyDown(Keys.K) ||
                currentGamePadState.DPad.Down == ButtonState.Pressed || currentGamePadState.ThumbSticks.Left.Y < 0) && !previousKeyboardState.IsKeyDown(Keys.C)&&pressed3==false)
                {
                    ismoveing = true;
                    playerStance = Stance.moving;
                    for (int i = 0; i < allItems.Count; i++)
                    {
                        if (!(allItems[i].getDownMove()))
                        {
                            canMoveDown = false;
                        }
                    }

                    if (canMoveDown && animators)
                        Position.Y += playerMoveSpeed;

                }
                if (currentGamePadState.Buttons.A == ButtonState.Released)
                    pressed3 = false;

                if ((currentKeyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space)) ||
                     (currentGamePadState.Buttons.A == ButtonState.Pressed && pressed3 == false))
                {
                    pressed3 = true;

                    if (!facing)
                    {
                        if (AttackRightList.Count < 1)
                            AddAR(new Vector2(StartPosition.X, Position.Y));
                    }
                    else
                    {
                        if (AttackLeftList.Count < 1)
                            AddAL(new Vector2(StartPosition.X, Position.Y));
                    }
                    playerStance = Stance.heavyAttack;
                }


                CollisionDetection(level);

                hitBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }
        public void CollisionDetection(World level)
        {
            if (withinRect)
            {
                // gMan y-boundaries
                if (Position.Y < level.level[location / 2].Y)
                    Position.Y = level.level[location / 2].Y;
                else
                    if (Position.Y > level.level[location / 2].Y + level.level[location / 2].Height)
                        Position.Y = level.level[location / 2].Y + level.level[location / 2].Height;

                // gMan x-boundaries
                if (Position.X < level.level[location / 2].X)
                {
                    if (location == 0)
                    {
                        Position = new Vector2(level.level[0].X, Position.Y);
                    }
                    else
                    {
                        location--;
                        withinRect = false;
                        return;
                    }
                }
                else
                    if (Position.X > level.level[location / 2].X + level.level[location / 2].Width)
                    {
                        if (location / 2 == level.level.Count - 1)
                        {
                            Position = new Vector2(level.level[location / 2].X + level.level[location / 2].Width, Position.Y);
                        }
                        else
                        {
                            location++;
                            withinRect = false;
                            return;
                        }
                    }
            }
            else
            {
                float topB, botB, topM, botM, topY, botY;
                topM = (level.tracks[location / 2].topLeft.Y - level.tracks[location / 2].topRight.Y)
                    / (level.tracks[location / 2].topLeft.X - level.tracks[location / 2].topRight.X);
                topB = (topM * (-level.tracks[location / 2].topLeft.X)) + level.tracks[location / 2].topLeft.Y;
                topY = topM * Position.X + topB;

                botM = (level.tracks[location / 2].botLeft.Y - level.tracks[location / 2].botRight.Y)
                   / (level.tracks[location / 2].botLeft.X - level.tracks[location / 2].botRight.X);
                botB = (botM * (-level.tracks[location / 2].botLeft.X)) + level.tracks[location / 2].botLeft.Y;
                botY = botM * Position.X + botB;

                if (Position.X <= level.tracks[location / 2].topLeft.X)
                {
                    withinRect = true;
                    location--;
                    return;
                }
                else
                    if (Position.X >= level.tracks[location / 2].topRight.X)
                    {
                        withinRect = true;
                        location++;
                        return;
                    }

                if (Position.Y < topY)
                    Position.Y = topY;
                else
                    if (Position.Y > botY)
                        Position.Y = botY;
            }

        }
        public void StanceMoves(GameTime gameTime)
        {
            if (playerStance == Stance.hurt&&hitonce)
            {
                hitbyrobot.Play();
                hitonce = false;
            }



            if (playerStance == Stance.heavyAttack && shotonce)
            {
                gunshot.Play();
                shotonce = false;
            }        




            ADeathLeft.Update(gameTime);
            ADeathRight.Update(gameTime);
            AFallLeft.Update(gameTime);
            AFallRight.Update(gameTime);
            ALifeLeft.Update(gameTime);
            ALifeRight.Update(gameTime);
            ARiseRight.Update(gameTime);
            ARiseLeft.Update(gameTime);
            if (facing)
            {
                if (playerStance == Stance.Standing)
                {
                    hitonce = true;
                    shotonce = true;
                    AStillleft.Update(gameTime);

                }
                if (playerStance == Stance.Right || playerStance == Stance.moving)
                {
                    if (gun)
                    {
                        AWalkGunRight.Update(gameTime);

                    }
                    else
                    {
                        AWalkNoGunRight.Update(gameTime);

                    }
                }

                if (AttackLeftList.Count == 0)
                    playerStance = Stance.Standing;
            }
            else
            {
                if (playerStance == Stance.Standing)
                {
                    hitonce = true;
                    shotonce = true;
                    AStillRight.Update(gameTime);
                }
                if (playerStance == Stance.Left || playerStance == Stance.moving)
                {
                    if (gun)
                    {
                        AWalkGunLeft.Update(gameTime);
                    }
                    else
                    {
                        AWalkNoGunLeft.Update(gameTime);
                    }
                }

                if (AttackRightList.Count == 0)
                    playerStance = Stance.Standing;
            }

        }




        public bool getDirection()
        {
            return facing;
        }

        public void Draw(SpriteBatch spriteBatch, float depth)
        {
            bool animator = true;
            depth = .4f;
            if (Health > 0)
            {
                if (AttackRightList.Count != 0)
                {
                    animator = false;
                }
                if (AttackLeftList.Count != 0)
                {
                    animator = false;
                }
                animator = true;

                

                if (facing)
                {
                    if (playerStance == Stance.Standing)
                    {
                        AStillleft.Draw(spriteBatch, depth,Position);
                    }
                    if (playerStance == Stance.Right || playerStance == Stance.moving)
                    {
                        if (!gun)
                        {
                            AWalkGunRight.Draw(spriteBatch, depth,Position);
                        }
                        else
                        {
                            AWalkNoGunRight.Draw(spriteBatch, depth,Position);
                        }
                    }

                    if (playerStance == Stance.hurt)
                    {
                        //AHurtLeft.Draw(spriteBatch, depth);
                        if(!Game1.translate)
                            spriteBatch.Draw(HurtRight, new Vector2(Position.X - Game1.translation.X - 100, Position.Y - 225), Color.White);
                        else
                            spriteBatch.Draw(HurtRight, new Vector2(StartPosition.X - 100, Position.Y - 225), Color.White);
                    }
                    if (playerStance == Stance.heavyAttack || fire == true)
                    {
                        //         AAttack1Right.Draw(spriteBatch, depth);
                        for (int i = 0; i < AttackLeftList.Count; i++)
                            AttackLeftList[i].Draw(spriteBatch, depth,Position);
    
                        fire = false;

                    }
                }
                else
                {
                    if (playerStance == Stance.Standing)
                    {
                        AStillRight.Draw(spriteBatch, depth,Position);
                    }
                    if (playerStance == Stance.Left || playerStance == Stance.moving)
                    {
                        if (!gun)
                        {
                            AWalkGunLeft.Draw(spriteBatch, depth,Position);
                        }
                        else
                        {
                            AWalkNoGunLeft.Draw(spriteBatch, depth,Position);
                        }
                    }

                    if (playerStance == Stance.hurt)
                    {
                        //AHurtRight.Draw(spriteBatch, depth); ;
                        if (!Game1.translate)
                            spriteBatch.Draw(HurtLeft, new Vector2(Position.X - Game1.translation.X - 100, Position.Y - 225), Color.White);
                        else
                            spriteBatch.Draw(HurtLeft, new Vector2(StartPosition.X - 100, Position.Y - 225), Color.White);
                    }
                    if (playerStance == Stance.heavyAttack || fire == true)
                    {
                        //      AAttack1Left.Draw(spriteBatch, depth);
                        for (int i = 0; i < AttackRightList.Count; i++)
                            AttackRightList[i].Draw(spriteBatch, depth,Position);

                        fire = false;
                    }
                }
            }
        }
    }
}