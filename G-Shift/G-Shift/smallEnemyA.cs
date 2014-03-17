using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace G_Shift
{
    public class smallEnemyA
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public Vector2 position { get; set; }
        public Vector2 velocity { get; set; }
        public Texture2D texture;
        float angle;
        float angularVelocity;
        Color color;
        
        //float size;
        public int ttl { get; set; }
        //public int health;
        public int health { get; set; }

        public int depth { get; set; }

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

        //public Projectile()
        //{
        //}

        public smallEnemyA(int height, int width, Vector2 pos, Vector2 vel, Texture2D tex, float theta, float thetaV)
        {
            health = 10;
            Height = height;
            Width = width;
            position = pos;
            velocity = vel;
            texture = tex;
            angle = theta;
            angularVelocity = thetaV;
            rect = new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);

            color = Color.White;
            //ttl = 20;
            //ttl = 40;
            //ttl = 80; // better
            //ttl = 320;
            ttl = 160;

            /*
            //Texture2D texture = textures[random.Next(textures.Count)];
            Texture2D texture = tex;
            position = EmitterLocation;
            Vector2 velocity = new Vector2(5f, 5f);
            float angle = gunAngle;
            float angularVelocity = angle;
            Color color = Color.White;
            float size = 1;
            int ttl = 20;  // +random.Next(40);
            */
        }

        public void Update()
        {
            ttl--;
            position += velocity;
            //angle += angularVelocity;

            //if(position.X > )
            //{
            //}

            // hard coded screen width, will need to fix !!!!!!!!!!!!!!!!!!!!!!!!
            /*
            if (position.X <= 500 || position.X >= 1400)
            {
                velocity = new Vector2(-velocity.X, velocity.Y);
            }
            */

            /*
            if (enemy1.position.X <= 500 || enemy1.position.X >=
            SCREEN_WIDTH + enemy1.Width * 4)
            {
                enemy1.motion = new Vector2(-enemy1.motion.X,
                enemy1.motion.Y); // reverse X direction
                //hitWallSound.Play();
            }
             */
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            //Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Rectangle sourceRectangle = new Rectangle(0, 0, Width, Height);
            Vector2 origin = new Vector2(Width / 2, Height / 2);  // .. rotating in place
            //Vector2 origin = new Vector2(miniGun.Width - 140, miniGun.Height - 35); // doesn't know minigun >_<
            //Vector2 origin = new Vector2(10, 20);


            //spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteEffects.None, 0f);  // !! Working
            
            /*
            if(depth == 1)
                spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteEffects.None, 0f);
            else if (depth == 2)
                spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteEffects.None, 0.33f);
            else
                spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteEffects.None, 0.66f);
            */

            //spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteSortMode.FrontToBack, 1f);
            //spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteEffects.None, 0.5f);  // !! LASt working
            spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteEffects.None, 0.1f*depth);
        }
    }
}
