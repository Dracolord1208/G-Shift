
// Standard, small crawly robot

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//added for sprite sheets
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using System.Timers;

namespace G_Shift
{
    public class Enemy4a
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public Vector2 startPosition { get; set; }
        public Vector2 position { get; set; }
        public Vector2 velocity { get; set; }
        public Texture2D texture { get; set; }
        float angle;
        float angularVelocity;
        Color color;

        //float size;
        public int ttl { get; set; }
        public int health { get; set; }
        public float depth { get; set; }

        public bool jumpFlag { get; set; }
        public float gravity;

        public Rectangle baseRect { get; set; }
        public Rectangle attackLeftRect { get; set; }
        public Rectangle attackRightRect { get; set; }

        public Rectangle rect
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    (int)Width, (int)Height);
            }
            set
            {
                //rect = new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);
            }
        }

        public bool decisionTimeFlag { get; set; }

        public TimeSpan decisionTime { get; set; }
        public TimeSpan lastDecisionTime { get; set; }
        public TimeSpan reverseDecisionTime { get; set; }
        public Random randomDecisionTime { get; set; }

        public TimeSpan attackCheckpoint { get; set; }
        public TimeSpan attackTimeSpan { get; set; }

        // move left
        public bool moveLeftFlag { get; set; }
        // move right
        public bool moveRightFlag { get; set; }
        // move up
        public bool moveUpFlag { get; set; }
        // move down
        public bool moveDownFlag { get; set; }
        // stop/hold position
        public bool holdPosFlag { get; set; }
        // attack
        public bool attackFlag { get; set; }

        public bool holdxPosFlag { get; set; }
        public bool holdyPosFlag { get; set; }

        public int baseHeight { get; set; }

        public bool isRightFlag { get; set; }   //flag for position in comparison to player

        //public Animation PlayerAnimation;
        //Animation playerAnimation;

        public bool ContentLoadedFlag { get; set; }


        public Texture2D spriteSheet;
        private AnimatedSprite moveAnimation;
        //public AnimatedSprite moveAnimation { get; set; }

        public Texture2D spriteSheetmoveRight;
        public Texture2D spriteSheetmoveLeft;
        private AnimatedSprite moveLeftAnimation;
        private AnimatedSprite moveRightAnimation;

        public enum Stance
        {                        
            Wait,
            Move,
            Attack,
            Stunned
        }
        public Stance stance { get; set; }


        public Enemy4a(int width, int height, Vector2 pos, Vector2 vel, Texture2D tex, float theta, float thetaV)
        {
            health = 10;
            Height = height;
            Width = width;
            startPosition = pos;
            position = pos;
            velocity = vel;
            texture = tex;
            angle = theta;
            angularVelocity = thetaV;
            rect = new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);

            //color = Color.White;
            color = new Color(255, 100, 100);
            //ttl = 160;
            //ttl = 320;
            ttl = 960;

            jumpFlag = false;
            gravity = 10f;

            decisionTimeFlag = false;

            //decisionTime = TimeSpan.FromSeconds(5.0f);
            //lastDecisionTime = new TimeSpan();
            lastDecisionTime = TimeSpan.FromSeconds(0.0f);
            reverseDecisionTime = TimeSpan.FromSeconds(1.0f);

            randomDecisionTime = new Random();
            decisionTime = TimeSpan.FromSeconds((float)randomDecisionTime.Next(1, 5));

            attackCheckpoint = TimeSpan.FromSeconds(0.0f);
            attackTimeSpan = TimeSpan.FromSeconds(2.0f);

            moveLeftFlag = false;
            moveRightFlag = false;
            moveUpFlag = false;
            moveDownFlag = false;
            holdPosFlag = false;
            holdxPosFlag = false;
            holdyPosFlag = false;
            attackFlag = false;

            baseHeight = 20;

            baseRect = new Rectangle((int)position.X, (int)position.Y + Height - 15, Width, 30);
            attackLeftRect = new Rectangle((int)position.X, (int)position.Y, (int)(Width*(.25)), Height);
            //attackRightRect = new Rectangle((int)position.X + Width - (int)(Width * (.25)), (int)position.Y, (int)(Width * (.25)), Height);
            attackRightRect = new Rectangle((int)(position.X + Width*(.75)), (int)position.Y, (int)(Width * (.25)), Height);

            //spriteSheet = Content.Load<Texture2D>("robotSmallSheet1a");
            //moveAnimation = new AnimatedSprite();
            ContentLoadedFlag = false;

            isRightFlag = false;

            stance = Stance.Wait;
        }

        public void LoadContent(ContentManager content)
        {
            //gManTest1 = content.Load<Texture2D>("gst1");
            //spriteSheet = content.Load<Texture2D>("robotSmallSheet1a");   // Working!!
            //moveAnimation = new AnimatedSprite(spriteSheet, 7, 5);

            spriteSheetmoveRight = content.Load<Texture2D>("robotSmallSheet2b");
            spriteSheetmoveLeft = content.Load<Texture2D>("robotSmallSheet2a");
            moveLeftAnimation = new AnimatedSprite(spriteSheetmoveLeft, 7, 5);
            moveRightAnimation = new AnimatedSprite(spriteSheetmoveRight, 7, 5);
        }

        //(Update v.2)
        public void Update()
        {
            ttl--;
            position += velocity;

            // move left
            if (moveLeftFlag == true)
            {
                velocity = new Vector2(-4f, velocity.Y);
                //moveAnimation.Update();
                //moveLeftAnimation.Update();
                //moveRightAnimation.Update();
            }
            // move right
            else if (moveRightFlag == true)
            {
                velocity = new Vector2(4f, velocity.Y);
                //moveAnimation.Update();
                //moveLeftAnimation.Update();
                //moveRightAnimation.Update();
            }
            // move up
            if (moveUpFlag == true)
            {
                velocity = new Vector2(velocity.X, -4f);
                //moveAnimation.Update();
                //moveLeftAnimation.Update();
                //moveRightAnimation.Update();
            }
            // move down
            else if (moveDownFlag == true)
            {
                velocity = new Vector2(velocity.X, 4f);
                //moveAnimation.Update();
                //moveLeftAnimation.Update();
                //moveRightAnimation.Update();
            }

            /*
            if (holdPosFlag == true)
            {
                velocity = new Vector2(0f, 0f);
            }
            */
            if (holdxPosFlag == true)
            {
                velocity = new Vector2(0f, velocity.Y);
                //moveLeftAnimation.Update();
                //moveRightAnimation.Update();
            }
            if (holdyPosFlag == true)
            {
                velocity = new Vector2(velocity.X, 0f);
                //moveLeftAnimation.Update();
                //moveRightAnimation.Update();
            }

            if (holdyPosFlag == true && holdxPosFlag == true)
            {
                holdPosFlag = true;
            }

            // stop/hold position
            if (holdPosFlag == true)
            {
                velocity = new Vector2(0f, 0f);
            }

            // attack
            if (attackFlag == true)
            {
            }

            if (holdPosFlag == false)
            {
                moveLeftAnimation.Update();
                moveRightAnimation.Update();
            }
        }

        public void ResetValues()
        {
            moveLeftFlag = false;
            moveRightFlag = false;
            moveUpFlag = false;
            moveDownFlag = false;
            holdPosFlag = false;
            holdxPosFlag = false;
            holdyPosFlag = false;
            attackFlag = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Width, Height);
            Vector2 origin = new Vector2(Width / 2, Height / 2);  // .. rotating in place

            //Working!!
            //spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteEffects.None, 0.1f * depth);
            if (ContentLoadedFlag == true)
            {
                /*
                if (holdPosFlag == true)
                {
                    //spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteEffects.None, 0.1f * depth);
                    moveAnimation.Draw(spriteBatch, position, Color.White, depth);
                }
                else
                {
                    //spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteEffects.None, 0.1f * depth);                    
                    //animatedRex.Draw(spriteBatch, Rex.position, Color.White, Rex.depth);
                    moveAnimation.Draw(spriteBatch, position, Color.White, depth);
                }
                */

                if (isRightFlag == true)
                {
                    moveRightAnimation.Draw(spriteBatch, position, Color.White, depth);
                }
                else
                {
                    moveLeftAnimation.Draw(spriteBatch, position, Color.White, depth);
                }

            }

        }
    }
}
