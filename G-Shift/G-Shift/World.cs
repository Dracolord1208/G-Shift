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
using System.Threading;
using System.Timers;

namespace G_Shift
{
    class World
    {
        public List<Rectangle> level;

        public World(int lvlNum)
        {
            level = new List<Rectangle>();
            StreamReader file = new StreamReader("testlevel.txt");

            string s = file.ReadToEnd();
            string[] data = s.Split(new string[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (int x = 0; x < data.Length; x++ )
            {
                if (data[x] == "Rect")
                {
                    Rectangle rect = new Rectangle();
                    rect.X = Convert.ToInt32(data[++x]);
                    rect.Y = Convert.ToInt32(data[++x]);
                    rect.Width = Convert.ToInt32(data[++x]);
                    rect.Height = Convert.ToInt32(data[++x]);

                    level.Add(rect);
                }
            }
        }
    }
}
