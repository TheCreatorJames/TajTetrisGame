using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class LevelRectangle : LevelObject, Saveable
    {
        protected Color color, colorB;
        private bool border;
        private bool editing;
        private GameRectangle a, b, d;
        private const int POINTSIZE = 15;
        private int oWidth, oHeight;
        private byte dragging;
        private Point original;

        public LevelRectangle() : this(0,0,10,10)
        { }

        public LevelRectangle(int x, int y, int width, int height) 
        {
            SetX(x);
            SetY(y);
            SetWidth(width);
            SetHeight(height);
            dragging = 0;
            color = Color.Red;
            a = new GameRectangle(x, y, POINTSIZE, POINTSIZE);
            b = new GameRectangle(x, y, POINTSIZE, POINTSIZE);
            d = new GameRectangle(x, y, POINTSIZE, POINTSIZE);

        }

        public override bool CheckInside(InputHandler handler)
        {
            return handler.CheckMouseIn(this);
        }

        public override bool Modifying()
        {
            return dragging != 0;
        }

        public void SetBorderColor(Color b)
        {
            this.colorB = b;
            border = true;
        }

        public void DisableBorder()
        {
            border = false;
        }

        public override void Update(InputHandler handler)
        {
            if (editing)
            {
                if (true)
                {
                    a.SetPoint(this.GetPoint());
                    b.SetX(this.GetX() + this.GetWidth() - b.GetWidth());
                    b.SetY(this.GetY());
                    d.SetX(b.GetX());
                    d.SetY(this.GetY() + this.GetHeight() - d.GetHeight());
                }

                if (handler.CheckLeftMouseJustReleased())
                {
                    dragging = 0;
                }

                if (handler.CheckLeftMouseJustPressed()) 
                if (handler.CheckMouseIn(a))
                {
                    dragging = 1;
                    original = a.GetPoint();
                }
                else
                    if (handler.CheckMouseIn(b))
                    {

                        original = b.GetPoint();
                        dragging = 2;
                        oHeight = height;
                        oWidth = width;

                    }
                    else if (handler.CheckMouseIn(d))
                    {

                        original = d.GetPoint();
                        oHeight = height;
                        oWidth = width;
                        dragging = 4;
                    }
            }

            if (dragging == 1)
            {
                a.SetPoint(original + handler.LeftMouseDraggedBy());
                this.SetPoint(original + handler.LeftMouseDraggedBy());
            } 
            else
            if (dragging == 2)
            {
                b.SetPoint(original + handler.LeftMouseDraggedBy());
                this.SetWidth(oWidth + handler.LeftMouseDraggedBy().X);
                this.SetHeight(oHeight + handler.LeftMouseDraggedBy().Y);
                
            }
            else
            if (dragging == 4)
            {
                d.SetPoint(original + handler.LeftMouseDraggedBy());

                this.SetWidth(oWidth + handler.LeftMouseDraggedBy().X);
                this.SetHeight(oHeight + handler.LeftMouseDraggedBy().Y);
            }


        }

        

        public override void Save(Saver saver)
        {
            saver.Header("LRect");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.Save(width, "Width");
            saver.Save(height, "Height");
            ((GameSaver)saver).Save(color, "Color");
            ((GameSaver)saver).Save(colorB, "Border");
            saver.Save(border, "BorderB");
            saver.End();
        }

        public override void Draw(PrimitiveDrawer drawer, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphics, SpriteBatch batch, FontHandler font)
        {
           
            if (!border)
            {

                drawer.DrawFilledRectangle(graphics, this, color);
               
            }
            else
            {
                Rectangle bordify = (Rectangle)this;
                bordify.X++; bordify.Y++;
                bordify.Width -= 2;
                bordify.Height -= 2;

                drawer.DrawFilledRectangle(graphics, this, colorB);
                drawer.DrawFilledRectangle(graphics, bordify, color);
                
            }

            if (editing)
            {
                drawer.DrawFilledRectangle(graphics, a, Color.White);
                drawer.DrawFilledRectangle(graphics, b, Color.White);
                drawer.DrawFilledRectangle(graphics, d, Color.White);
            }
        }

        public override void SetColor(Color x)
        {
            this.color = x;
        }

        public override void SetEditing(bool x)
        {
            this.editing = x;
        }
    }
}
