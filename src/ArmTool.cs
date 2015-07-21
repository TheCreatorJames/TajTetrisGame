using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class ArmTool : Saveable
    {
        private bool showMode;
        private bool neverEditAgain;
        private bool dragPointA, dragPointB, dragPointC;
        private GameRectangle a, b, c;
        private Point original;
        private Vector2 start;
        private const int size = 15;
        private List<short> points = new List<short>();

        public ArmTool()
        {
            a = new GameRectangle(0, 0, size, size);
            b = new GameRectangle(0, 0, size, size);
            c = new GameRectangle(0, 0, size, size);
            showMode = false;
        }

        public void EndEditing()
        {
            neverEditAgain = true;
        }

        public bool Using()
        {
            return dragPointA || dragPointB || dragPointC;
        }

        public void SaveArm()
        {
            points.Add((short)a.GetX()); //1
            points.Add((short)a.GetY()); //2

            points.Add((short)b.GetX()); //3
            points.Add((short)b.GetY()); //4

            points.Add((short)c.GetX()); //5
            points.Add((short)c.GetY()); //6
        }

        public void Clear(int from)
        {
            points.RemoveRange(from*6, points.Count - from*6);
        }

        public void Clear()
        {
            points.Clear();
        }

        public void LoadFrame(int frame)
        {
            frame %= points.Count / 6;

            a.SetX(points[frame*6 + 0]);
            a.SetY(points[frame * 6 + 1]);

            b.SetX(points[frame * 6 + 2]);
            b.SetY(points[frame * 6 + 3]);

            c.SetX(points[frame * 6 + 4]);
            c.SetY(points[frame * 6 + 5]);
        }

        public void Update(InputHandler handler, int x, int y)
        {


            if (!neverEditAgain)
            {
                if (handler.CheckJustPressedKey(Keys.H))
                {
                    showMode = !showMode;
                }

                if (handler.CheckLeftMouseJustPressed())
                {
                    if (handler.CheckMouseIn(c.ScalePosition(start)))
                    {
                        dragPointC = true;
                        original = c.GetPoint();
                    }
                    else
                        if (handler.CheckMouseIn(b.ScalePosition(start)))
                        {
                            dragPointB = true;

                            original = b.GetPoint();
                        }
                        else
                            if (handler.CheckMouseIn(a.ScalePosition(start)))
                            {
                                dragPointA = true;

                                original = a.GetPoint();
                            }
                }
                else
                    if (handler.CheckLeftMouseJustReleased())
                    {
                        dragPointA = dragPointB = dragPointC = false;
                    }

                    if (dragPointA)
                    {
                        a.SetPoint(original + handler.LeftMouseDraggedBy());
                    }
                    else
                        if (dragPointB)
                        {

                            b.SetPoint(original + handler.LeftMouseDraggedBy());
                        }
                        else
                            if (dragPointC)
                            {

                                c.SetPoint(original + handler.LeftMouseDraggedBy());
                            }
            }
            start.X = x;
            start.Y = y;

        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch batch, PrimitiveDrawer drawer, FontHandler font)
        {

            if(showMode && !neverEditAgain)
            {
                drawer.DrawRoundedRectangle(graphicsDevice, a.ScalePosition(start), Color.White);
                drawer.DrawRoundedRectangle(graphicsDevice, b.ScalePosition(start), Color.Red);
                drawer.DrawRoundedRectangle(graphicsDevice, c.ScalePosition(start), Color.Blue);
            }


            drawer.DrawCurve(graphicsDevice, start, a.ScalePosition(start).GetVector(), b.ScalePosition(start).GetVector(), c.ScalePosition(start).GetVector(), 1, 0, Color.Black);
        }


        internal void SaveAt(int frameNumber, bool replaceRest)
        {
            do
            {
                points[frameNumber * 6 + 0] = (short)a.GetX();
                points[frameNumber * 6 + 1] = (short)a.GetY();
                points[frameNumber * 6 + 2] = (short)b.GetX();
                points[frameNumber * 6 + 3] = (short)b.GetY();
                points[frameNumber * 6 + 4] = (short)c.GetX();
                points[frameNumber * 6 + 5] = (short)c.GetY();
                frameNumber++;
            } while (replaceRest && (frameNumber * 6 < points.Count));
        }



        public void SetPoints(short[] arr)
        {
            points.Clear();
            points.AddRange(arr);
        }

        public void Save(Saver saver)
        {
            saver.Header("ArmTool");
            saver.SaveArray<short>(points.ToArray(), "Points");
            saver.End();
        }


    }
}
