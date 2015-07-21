using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class LevelCircle : LevelObject
    {
        protected Color color;
        private Color border;
        protected GameRectangle point;
        protected bool dragging, editing, borderMode;
        private Point original;
        private int oRadius;

        public LevelCircle() : this(0,0,10)
        {

        }

        public LevelCircle(int x, int y, int radius)
        {
            SetX(x);
            SetY(y);
            SetWidth(radius);
            point = new GameRectangle(0, 0, 15, 15);
        }

        public override void SetColor(Microsoft.Xna.Framework.Color col)
        {
            this.color = col;
        }

        public void SetBorderColor(Color col)
        {
            this.border = col;
            this.borderMode = true;
        }

        public override bool CheckInside(InputHandler handler)
        {
            int a = GetX() - handler.GetMouseX();
            int b = GetY() - handler.GetMouseY();
            return a * a + b * b <= GetWidth() * GetWidth();
        }

        public override void SetEditing(bool x)
        {
            editing = x;
        }

        public void SetBorderMode(bool x)
        {
            this.borderMode = x;
        }

        public override void Save(Saver saver)
        {
            saver.Header("LCircle");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.Save(width, "Radius");
            ((GameSaver)saver).Save(color, "Color");
            ((GameSaver)saver).Save(border, "Border");
            saver.Save(borderMode, "BorderMode");
            saver.End();
        }

        public override bool Modifying()
        {
            return dragging;
        }

        public int GetRadius()
        {
            return GetWidth();
        }

        public void SetRadius(int rad)
        {
            SetWidth(rad);
        }

        public override void Update(InputHandler handler)
        {
            this.height = GetWidth();

            if (editing)
            {


                point.SetX(this.GetX() + this.GetWidth());
                point.SetY(this.GetY());

                if(handler.CheckLeftMouseJustPressed() && handler.CheckMouseIn(point))
                {
                    dragging = true;
                    original = point.GetPoint();
                    oRadius = GetRadius();
                }

                if(handler.CheckLeftMouseJustReleased())
                {
                    dragging = false;

                }
                else
                if(dragging)
                {
                    point.SetX(original.X + handler.LeftMouseDraggedBy().X);
                    this.SetRadius(oRadius + handler.LeftMouseDraggedBy().X);
                }

            }

            
        }

        public override void Draw(PrimitiveDrawer drawer, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphics, SpriteBatch batch, FontHandler font)
        {
            if(borderMode)
            {
                drawer.DrawCircle(graphics, GetVector(), GetWidth(), border);
                drawer.DrawCircle(graphics, GetVector(), GetWidth()-1, color);
            }
            else
            {
                drawer.DrawCircle(graphics, GetVector(), GetWidth(), color);
            }

            if(editing)
            {
                drawer.DrawFilledRectangle(graphics, point, Color.White);
            }
        }
    }
}
