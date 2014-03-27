﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace G_Shift
{
    public class Enemy3a
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public Vector2 position { get; set; }
        public Vector2 velocity { get; set; }
        public Texture2D texture { get; set; }
        float angle;
        float angularVelocity;
        Color color;

        //float size;
        public int ttl { get; set; }
        //public int health;
        public int health { get; set; }
        public int depth { get; set; }

        public bool jumpFlag { get; set; }
        public float gravity;

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


        public Enemy3a(int width, int height, Vector2 pos, Vector2 vel, Texture2D tex, float theta, float thetaV)
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
            //ttl = 160;
            ttl = 320;

            jumpFlag = false;
            gravity = 10f;

        }

        public void Update()
        {
            ttl--;
            if (jumpFlag == true)
                velocity = new Vector2(velocity.X, velocity.Y + gravity);
            position += velocity;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            //Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Rectangle sourceRectangle = new Rectangle(0, 0, Width, Height);
            Vector2 origin = new Vector2(Width / 2, Height / 2);  // .. rotating in place

            spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, 1, SpriteEffects.None, 0.1f * depth);
        }
    }
}
