using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace G_Shift
{
    public class Interactable
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public Vector2 position { get; set; }
        public Vector2 motion { get; set; }
        public int Health { get; set; }
        public int totalHealth { get; set; }
        public int depth { get; set; }
        public Rectangle rect
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    (int)Width, (int)Height);
            }
        }


        public Rectangle hitRect { get; set; }
    }
}
