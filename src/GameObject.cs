using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    abstract class GameObject
    {
        protected int height, width;
        protected int x, y;

        virtual public Rectangle ToRectangle()
        {
            return new Rectangle(GetX(), GetY(), GetWidth(), GetHeight());
        }

        public static explicit operator Rectangle(GameObject rectangle)
        {
            return new Rectangle(rectangle.GetX(), rectangle.GetY(), rectangle.GetWidth(), rectangle.GetHeight());
        }

        virtual public int GetX()
        {
            return x;
        }

        virtual public void SetWidth(int width)
        {
            this.width = width;
        }

        virtual public void ModifyWidth(int width)
        {
            this.width += width;
        }

        virtual public void ModifyHeight(int height)
        {
            this.height += height;
        }

        virtual public void SetHeight(int height)
        {
            this.height = height;
        }

        virtual public Point GetPoint()
        {
            return new Point(GetX(), GetY());
        }

        virtual public Vector2 GetVector()
        {
            return new Vector2(GetX(), GetY());
        }

        virtual public Point GetSize()
        {
            return new Point(GetWidth(), GetHeight());
        }

        virtual public void SetX(int x)
        {
            this.x = x;
        }

        virtual public void ModifyX(int x)
        {
            this.x += x;
        }

        virtual public void ModifyY(int y)
        {
            this.y += y;
        }

        virtual public void SetY(int y)
        {
            this.y = y;
        }

        virtual public void SetPoint(ref Point point)
        {
            SetX(point.X);
            SetY(point.Y);
        }

        virtual public void SetPoint(Point point)
        {
            SetX(point.X);
            SetY(point.Y);
        }

        virtual public void SetSize(ref Point point)
        {
            SetWidth(point.X);
            SetHeight(point.Y);
        }


        virtual public void SetSize(Point point)
        {
            SetWidth(point.X);
            SetHeight(point.Y);
        }


        virtual public void SetVector(ref Vector2 vec)
        {
            SetX((int)vec.X);
            SetY((int)vec.Y);
        }

        virtual public void SetVector(Vector2 vec)
        {
            SetX((int)vec.X);
            SetY((int)vec.Y);
        }

        virtual public int GetY()
        {
            return y;
        }

        virtual public int GetHeight()
        {
            return height;
        }

        virtual public int GetWidth()
        {
            return width;
        }

        virtual public void SetSize(int width, int height)
        {
            SetWidth(width);
            SetHeight(height);
        }


        virtual public void ModifySize(int width, int height)
        {
            this.width += width;
            this.height += height;
        }
    }
}
