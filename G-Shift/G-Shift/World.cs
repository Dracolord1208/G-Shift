
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
        public List<Item> items;

        public World(int lvlNum, ContentManager Content)
        {
            level = new List<Rectangle>();
            items = new List<Item>();
            StreamReader file = new StreamReader("testlevel.txt");
<<<<<<< HEAD
            //StreamReader file = new StreamReader("level.txt");
=======
>>>>>>> b4dc8a0a04f3678f83b5f9c480e906fa2fdd72fb

            string s = file.ReadToEnd();
            string[] data = s.Split(new string[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (int x = 0; x < data.Length; x++)
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
                if (data[x] == "Item")
                {
                    Item item = new Item();
                    Vector2 vec = new Vector2();
                    vec.X = Convert.ToInt32(data[++x]);
                    vec.Y = Convert.ToInt32(data[++x]);

                    item.initialize(Content, data[++x]);
                    item.setItemPosition(vec);

                    items.Add(item);
                }
                if (data[x] == "/*")
                {
                    while (data[++x] != "*/") ;
                }
            }
        }

        public List<Item> getForUpdate()
        {
            return items;
        }

        public void Draw(SpriteBatch sBatch)
        {
            foreach (Item item in items)
            {
                item.Draw(sBatch);
            }
        }
    }
}
