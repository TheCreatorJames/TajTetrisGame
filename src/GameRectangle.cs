using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class GameRectangle : GameObject
    {

        public GameRectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public GameRectangle(GameRectangle gameRectangle) : this(gameRectangle.x, gameRectangle.y, gameRectangle.width, gameRectangle.height)
        {
        }

        public GameRectangle() : this(0,0,0,0)
        {

        }

        public GameRectangle(Vector2 vector2, int width, int height) : this((int)vector2.X, (int)vector2.Y, width, height)
        {

        }

        public GameRectangle ScaleSize(int width, int height)
        {
            GameRectangle r = new GameRectangle(this);
            r.ModifySize(width, height);
            return r;
        }


        public GameRectangle ScalePosition(Vector2 s)
        {
            GameRectangle r = new GameRectangle(this);
            r.ModifyX((int)s.X);
            r.ModifyY((int)s.Y);
            return r;
        }

        public GameRectangle ScalePosition(int x, int y)
        {
            GameRectangle r = new GameRectangle(this);
            r.ModifyX(x);
            r.ModifyY(y);
            return r;
        }

        
        public static explicit operator GameRectangle(Rectangle rectangle)
        {
            return new GameRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }
}
