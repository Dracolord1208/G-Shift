
// 1st Boss

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
    public class Boss1a
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public Vector2 screenPosition { get; set; }
        public Vector2 position { get; set; }
        public Vector2 startPosition { get; set; }
        public Vector2 velocity { get; set; }
        public Texture2D texture { get; set; }
        float angle;
        float angularVelocity;
        Color color;

        public int ttl { get; set; }
        public int health { get; set; }
        public int maxHealth { get; set; }
        public int depth { get; set; }

        public bool jumpFlag { get; set; }
        public float gravity;

        public Rectangle baseRect { get; set; }
        public Rectangle attackLeftRect { get; set; }
        public Rectangle attackRightRect { get; set; }
        public Rectangle hitBox { get; set; }

        
        public bool decisionTimeFlag { get; set; }

        public TimeSpan decisionTime { get; set; }
        public TimeSpan lastDecisionTime { get; set; }
        public TimeSpan minimumAttackTime { get; set; }
        public TimeSpan lastAttackTime { get; set; }
        public TimeSpan reverseDecisionTime { get; set; }
        public Random randomDecisionTime { get; set; }

        public TimeSpan turnAroundCheckpoint { get; set; }
        public TimeSpan turnAroundTime { get; set; }
        public bool turnAroundFlag { get; set; }
        public TimeSpan onRightCheckpoint { get; set; }
        public TimeSpan onLeftCheckpoint { get; set; }

        public TimeSpan sawBladeAttackTime { get; set; }

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
        
        public bool ContentLoadedFlag { get; set; }
        
        public Texture2D spriteSheet;
        //private AnimatedSprite moveAnimation;

        public Texture2D spriteSheetmoveRight;
        public Texture2D spriteSheetmoveLeft;
        private AnimatedSprite moveLeftAnimation;
        private AnimatedSprite moveRightAnimation;

        //Vector2 home;
        public Vector2 laserStartPos { get; set; }
        public Vector2 laserStartScreenPos { get; set; }    // scrolling
        public Vector2 laserCurrentPos { get; set; }
        public Vector2 laserCurrentScreenPos { get; set; }  // updated for scrolling
        public Rectangle laserBeam { get; set; }
        public bool laserOn { get; set; }
        public int laserPosX { get; set; }
        public float laserWidth { get; set; }
        public float laserHeight { get; set; }

        public enum Stance
        {
            Wait,
            Move,
            Attack,
            Stunned,
            ReturnHome
        }
        public Stance stance { get; set; }


        public Boss1a(int width, int height, Vector2 pos, Vector2 vel, Texture2D tex, float theta, float thetaV)
        {
            health = 500;
            maxHealth = 500;
            Height = height;
            Width = width;
            startPosition = pos;
            position = pos;
            velocity = vel;
            texture = tex;
            angle = theta;
            angularVelocity = thetaV;
            //rect = new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);

            //color = Color.White;
            color = new Color(255, 100, 100);
            //ttl = 160;
            //ttl = 320;
            //ttl = 960;
            ttl = 1920;

            jumpFlag = false;
            gravity = 10f;

            decisionTimeFlag = false;

            lastDecisionTime = TimeSpan.FromSeconds(0.0f);
            lastAttackTime = TimeSpan.FromSeconds(0.0f);
            minimumAttackTime = TimeSpan.FromSeconds(1.0f);
            reverseDecisionTime = TimeSpan.FromSeconds(1.0f);

            sawBladeAttackTime = TimeSpan.FromSeconds(2.0f);

            randomDecisionTime = new Random();
            //decisionTime = TimeSpan.FromSeconds((float)randomDecisionTime.Next(1, 5));
            decisionTime = TimeSpan.FromSeconds(1.0f);

            attackCheckpoint = TimeSpan.FromSeconds(0.0f);
            attackTimeSpan = TimeSpan.FromSeconds(2.0f);

            turnAroundCheckpoint = TimeSpan.FromSeconds(0.0f);
            turnAroundTime = TimeSpan.FromSeconds(1.5f);
            turnAroundFlag = false;
            onRightCheckpoint = TimeSpan.FromSeconds(0.0f);
            onLeftCheckpoint = TimeSpan.FromSeconds(0.0f);

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
            attackLeftRect = new Rectangle((int)position.X, (int)position.Y, (int)(Width * (.25)), Height);
            attackRightRect = new Rectangle((int)(position.X + Width * (.75)), (int)position.Y, (int)(Width * (.25)), Height);

            hitBox = new Rectangle((int)position.X, (int)position.Y, Width, Height);

            ContentLoadedFlag = false;

            isRightFlag = false;

            //stance = Stance.Wait;
            stance = Stance.Move;

            //home = new Vector2();
            laserStartPos = new Vector2(pos.X + 50, pos.Y + 50);
            laserCurrentPos = new Vector2(pos.X + 50, pos.Y + 50);
            laserBeam = new Rectangle((int)laserCurrentPos.X, (int)laserCurrentPos.Y, 10, 20);
            laserOn = false;
            laserPosX = (int)laserStartPos.X;
            laserWidth = 0;
            laserHeight = 20;
            //laserBeam = new Rectangle(500, 25, theBoss1.health, 20);
        }

        public void LoadContent(ContentManager content)
        {
            spriteSheetmoveRight = content.Load<Texture2D>("BossSheet1cRight");
            spriteSheetmoveLeft = content.Load<Texture2D>("BossSheet1c");
            //moveLeftAnimation = new AnimatedSprite(spriteSheetmoveLeft, 3, 3);
            //moveRightAnimation = new AnimatedSprite(spriteSheetmoveRight, 3, 3);
            moveLeftAnimation = new AnimatedSprite(spriteSheetmoveRight, 3, 3);
            moveRightAnimation = new AnimatedSprite(spriteSheetmoveLeft, 3, 3);
        }

        //
        public void Update()
        {
            ttl--;
            position += velocity;

            laserStartPos = new Vector2(position.X + 150, position.Y + 150);

            // move left
            if (moveLeftFlag == true)
            {
                if(stance == Stance.Attack)
                    velocity = new Vector2(-2f, velocity.Y);
                else
                    velocity = new Vector2(-2f, velocity.Y);
            }
            // move right
            else if (moveRightFlag == true)
            {
                //velocity = new Vector2(4f, velocity.Y);

                if (stance == Stance.Attack)
                    velocity = new Vector2(2f, velocity.Y);
                else
                    velocity = new Vector2(2f, velocity.Y);
            }
            // move up
            if (moveUpFlag == true)
            {
                //velocity = new Vector2(velocity.X, -4f);

                if (stance == Stance.Attack)
                    velocity = new Vector2(velocity.X, -2f);
                else
                    velocity = new Vector2(velocity.X, -2f);
            }
            // move down
            else if (moveDownFlag == true)
            {
                //velocity = new Vector2(velocity.X, 4f);

                if (stance == Stance.Attack)
                    velocity = new Vector2(velocity.X, 2f);
                else
                    velocity = new Vector2(velocity.X, 2f);
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
            }
            if (holdyPosFlag == true)
            {
                velocity = new Vector2(velocity.X, 0f);
            }

            if (holdyPosFlag == true && holdxPosFlag == true)
            {
                holdPosFlag = true;
            }

            // stop/hold position
            if (holdPosFlag == true)
            {
                velocity = new Vector2(0f, 0f);

                //moveLeftAnimation.Update();
                //moveRightAnimation.Update();
            }

            // attack
            /*
            if (attackFlag == true)
            {
                
            }
            */

            if(stance == Stance.Attack)
            {
                moveLeftAnimation.Update();
                moveRightAnimation.Update();
            }

            if (holdPosFlag == false)
            {
                //moveLeftAnimation.Update();
                //moveRightAnimation.Update();
            }

            hitBox = new Rectangle((int)position.X, (int)position.Y, Width, Height);
        }

        // not being used.. yet
        public void Update(GameTime gameTime)
        {
            ttl--;
            position += velocity;

            if (stance == Stance.Wait && gameTime.TotalGameTime - lastDecisionTime > decisionTime)
            {
                stance = Stance.Move;
            }

            if (stance != Stance.Wait)
            {

                // move left
                if (moveLeftFlag == true)
                {
                    velocity = new Vector2(-4f, velocity.Y);
                }
                // move right
                else if (moveRightFlag == true)
                {
                    velocity = new Vector2(4f, velocity.Y);
                }
                // move up
                if (moveUpFlag == true)
                {
                    velocity = new Vector2(velocity.X, -4f);
                }
                // move down
                else if (moveDownFlag == true)
                {
                    velocity = new Vector2(velocity.X, 4f);
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
                }
                if (holdyPosFlag == true)
                {
                    velocity = new Vector2(velocity.X, 0f);
                }

                if (holdyPosFlag == true && holdxPosFlag == true)
                {
                    holdPosFlag = true;
                }

                // stop/hold position
                if (holdPosFlag == true)
                {
                    velocity = new Vector2(0f, 0f);

                    moveLeftAnimation.Update();
                    moveRightAnimation.Update();
                }

                // attack
                if (attackFlag == true)
                {
                    moveLeftAnimation.Update();
                    moveRightAnimation.Update();
                    lastAttackTime = gameTime.TotalGameTime;
                }

                if (holdPosFlag == false)
                {
                    //moveLeftAnimation.Update();
                    //moveRightAnimation.Update();
                }

            }  // end of:  if (stance != Stance.Stunned)

            hitBox = new Rectangle((int)position.X, (int)position.Y, Width, Height);
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
                if (isRightFlag == true)
                {
                    moveRightAnimation.Draw(spriteBatch, screenPosition, Color.White, depth);
                }
                else
                {
                    moveLeftAnimation.Draw(spriteBatch, screenPosition, Color.White, depth);
                }
            }
        }
    }
}
