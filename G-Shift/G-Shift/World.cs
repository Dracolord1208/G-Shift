
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
        SoundEffect boxbreak;

        public struct Track
        {
            public Vector2 topLeft;
            public Vector2 topRight;
            public Vector2 botLeft;
            public Vector2 botRight;

            /*public bool direction1;
            public bool direction2;*/
        }
        public List<Track> tracks;
        public List<Rectangle> level;
        public List<Item> items;

        public World(int lvlNum, ContentManager Content)
        {
            level = new List<Rectangle>();
            items = new List<Item>();
            tracks = new List<Track>();
            StreamReader file = new StreamReader("level.txt");
            boxbreak = Content.Load<SoundEffect>("Music/Glass_Break-stephan_schutze-958181291");

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
                if (data[x] == "Track")
                {
                    Track track = new Track();
                    track.topLeft.X = Convert.ToInt32(data[++x]);
                    track.topLeft.Y = Convert.ToInt32(data[++x]);
                    track.topRight.X = Convert.ToInt32(data[++x]);
                    track.topRight.Y = Convert.ToInt32(data[++x]);
                    track.botLeft.X = Convert.ToInt32(data[++x]);
                    track.botLeft.Y = Convert.ToInt32(data[++x]);
                    track.botRight.X = Convert.ToInt32(data[++x]);
                    track.botRight.Y = Convert.ToInt32(data[++x]);
                    /*if (data[++x] == "true")
                        track.direction1 = true;
                    else
                        track.direction1 = false;
                    if (data[++x] == "true")
                        track.direction2 = true;
                    else
                        track.direction2 = false;*/

                    tracks.Add(track);
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
                item.setsound(boxbreak);
                item.Draw(sBatch);
            }
        }
    }
}
