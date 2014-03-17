using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public class AnimatedSprite
{
    public Texture2D Texture { get; set; }      // texture atlas
    public int Rows { get; set; }
    public int Columns { get; set; }
    private int currentFrame;
    private int totalFrames;


    public AnimatedSprite(Texture2D texture, int rows, int columns)
    {
        Texture = texture;
        Rows = rows;
        Columns = columns;
        currentFrame = 0;
        totalFrames = Rows * Columns;
    }

    public void Update()
    {
        currentFrame++;
        if (currentFrame == totalFrames)
            currentFrame = 0;
    }

    /*
    public void Draw(SpriteBatch spriteBatch, Vector2 location)
    {
        int width = Texture.Width / Columns;
        int height = Texture.Height / Rows;
        int row = (int)((float)currentFrame / (float)Columns);
        int column = currentFrame % Columns;

        Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
        Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

        //spriteBatch.Begin();
        spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        //spriteBatch.End();
    }
     */

    // going to have to change to accept depth/layer
    /*
    public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color)
    {
        int width = Texture.Width / Columns;
        int height = Texture.Height / Rows;
        int row = (int)((float)currentFrame / (float)Columns);
        int column = currentFrame % Columns;

        Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
        Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

        //spriteBatch.Begin();
        //spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        //spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, color);    // !! WORKING
        Vector2 origin = new Vector2(0,0);
        spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, color, 0, origin, SpriteEffects.None, 0.1f);
        //spriteBatch.End();
    }
     */

    public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color, int depth)
    {
        int width = Texture.Width / Columns;
        int height = Texture.Height / Rows;
        int row = (int)((float)currentFrame / (float)Columns);
        int column = currentFrame % Columns;

        Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
        Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

        //spriteBatch.Begin();
        //spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        //spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, color);    // !! WORKING
        Vector2 origin = new Vector2(0, 0);
        spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, color, 0, origin, SpriteEffects.None, (0.1f)*(depth));
        //spriteBatch.End();
    }


}
