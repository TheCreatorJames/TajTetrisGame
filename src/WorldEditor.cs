using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class World : Saveable
    {
        private LevelObject[] objects;
        public World(LevelObject[] obj)
        {
            this.objects = obj;
        }
        public LevelObject[] GetObjects()
        {
            return objects;
        }

        public void Save(Saver saver)
        {
            saver.Header("World");
            saver.SaveArray<LevelObject>(objects, "Objects");
            saver.End();
        }
    }

    class WorldEditor 
    {
        private List<LevelObject> objects;
       
        private bool animationMode;
        private AnimationTool tool;
        private Point original;
        private bool modifyingOne;
        private bool dragging;
        private string fileName, safeFileName;
        private bool finished;
        private byte draggingColor;
        private bool hideEditing;
        private bool bringToFront;

        private FileTextBox textBox;
        private byte fileMode;
        private bool cullMode;

        private LevelObject dragged, modifying, couldDrag, colorize;
        private GameObject r, g, b;
        private int originX, originY;
       
        public WorldEditor()
        {
            cullMode = true;
            objects = new List<LevelObject>();
            objects.Add(new LevelRectangle(300, 10, 100, 100));
            draggingColor = 0;
            r = new GameRectangle(-15, -15, 15, 15);
            g = new GameRectangle(-15, -15, 15, 15);
            b = new GameRectangle(-15, -15, 15, 15);
            tool = new AnimationTool();

            objects[0].SetColor(Color.White);
            ((LevelRectangle)objects[0]).SetBorderColor(Color.Black);
            objects.Add(new LightLevelCircle(400, 100, 200));
            objects[1].SetColor(Color.White);
            textBox = new FileTextBox();
            textBox.SetX(400);
            textBox.SetY(100);
            
        }


        public void Update(InputHandler handler)
        {

            try
            {
                if (textBox.GetSelected())
                {
                    DragDropInterface.UpdateTextBox(textBox, handler);

                    return;
                }




                if (fileMode != 0)
                {
                    if (fileMode == 1)
                    {

                        SaveFileSystem.SaveObjectToFile(new World(objects.ToArray()), "Levels" + Path.DirectorySeparatorChar + textBox.GetText() + ".taj");
                        textBox.SetText("");
                    }
                    else if (fileMode == 2)
                    {
                        World n = SaveFileSystem.LoadObjectFromFile<World>("Levels" + Path.DirectorySeparatorChar + textBox.GetText() + ".taj", new GameLoader());
                        objects.Clear();
                        objects.AddRange(n.GetObjects());
                    }
                    else if (fileMode == 3)
                    {

                        ((LevelPerson)couldDrag).SetBodyAnimation(textBox.GetText() + ".taj");
                        ((LevelPerson)couldDrag).SetLoop(true);
                        ((LevelPerson)couldDrag).SetPlaying(true);

                    }
                    else if (fileMode == 4)
                    {

                        ((LevelPerson)couldDrag).SetName(textBox.GetText());

                    }
                    else if (fileMode ==5)
                    {
                        SaveFileSystem.SaveObjectToFile(new World(objects.ToArray()), "Levels" + Path.DirectorySeparatorChar + textBox.GetText() + ".taj");
                        

                        fileName = "Levels" + Path.DirectorySeparatorChar  + textBox.GetText() + ".taj";
                        safeFileName = textBox.GetText() + ".taj";
                        finished = true;
                    }

                    textBox.SetText("");
                    fileMode = 0;
                }



                if (animationMode)
                {

                    tool.Update(handler);

                    if (tool.GetDone()) animationMode = false;
                    return;
                }

                if (!modifyingOne)
                    foreach (LevelObject o in objects)
                    {
                        o.Update(handler);
                        o.SetEditing(!hideEditing);

                        if (o.Modifying())
                        {
                            modifying = o;
                            modifyingOne = true;
                            break;
                        }

                        if (!dragging && o.CheckInside(handler))
                        {

                            if (o is LightLevelCircle)
                            {
                                if (handler.CheckPressedKey(Keys.L))
                                {
                                    couldDrag = o;
                                }
                            }
                            else
                                couldDrag = o;
                        }

                    }
                else
                {
                    modifying.Update(handler);
                    if (!modifying.Modifying())
                    {
                        modifying = null;
                        modifyingOne = false;
                    }
                }

                if (handler.CheckJustPressedKey(Keys.H))
                {
                    hideEditing = !hideEditing;
                }


                if (handler.CheckJustPressedKey(Keys.K))
                {
                    cullMode = !cullMode;
                }

                if (handler.CheckJustPressedKey(Keys.F))
                {
                    bringToFront = !bringToFront;
                }

                if (handler.CheckJustPressedKey(Keys.Delete))
                {
                    textBox.TellSelected(true);
                    fileMode = 1;
                }

                if (handler.CheckJustPressedKey(Keys.Insert))
                {
                    textBox.TellSelected(true);
                    fileMode = 2;
                }

                if (handler.CheckJustPressedKey(Keys.Q))
                {
                    if (handler.CheckPressedKey(Keys.L))
                    {
                        LightLevelCircle c = new LightLevelCircle(400, 10, 110);
                        c.SetColor(Color.White);
                        c.SetBorderColor(Color.Black);
                        objects.Add(c);
                    }
                    else
                    {
                        LevelCircle c = new LevelCircle(300, 0, 10);
                        c.SetColor(Color.Red);
                        c.SetBorderColor(Color.Black);
                        objects.Add(c);
                    }
                }


                if (couldDrag != null && !couldDrag.CheckInside(handler))
                {
                    couldDrag = null;
                }




                if (colorize != null)
                {

                    if (handler.CheckJustPressedKey(Keys.C))
                    {
                        colorize = null;
                        r.SetX(-15);
                        r.SetY(-15);
                        g.SetX(-15);
                        g.SetY(-15);
                        b.SetX(-15);
                        b.SetY(-15);
                        originX = originY = -15;
                    }
                }
                else
                {

                    if (handler.CheckJustPressedKey(Keys.A))
                    {

                        if (couldDrag != null && couldDrag is LevelPerson)
                        {
                            tool.Use(couldDrag.GetX(), couldDrag.GetY(), couldDrag.GetWidth(), couldDrag.GetHeight());
                            animationMode = true;
                            return;
                        }

                    }

                    if (handler.CheckPressedKey(Keys.D3) && handler.CheckPressedKey(Keys.D4))
                    {
                        textBox.TellSelected(true);
                        fileMode = 5;
                    }

                    if (handler.CheckJustPressedKey(Keys.O))
                    {
                        if (couldDrag != null && couldDrag is LevelPerson)
                        {
                            ((LevelPerson)couldDrag).MoveTo(couldDrag.GetX() + 200, couldDrag.GetY(), 60);
                        }
                    }


                    if (handler.CheckJustPressedKey(Keys.N))
                    {
                        if (couldDrag != null && couldDrag is LevelPerson)
                        {
                            fileMode = 4;
                            textBox.TellSelected(true);
                        }
                    }
                    if (handler.CheckJustPressedKey(Keys.G))
                    {
                        //Load an animation onto a person.
                        //loop it to test.


                        if (couldDrag != null && couldDrag is LevelPerson)
                        {
                            fileMode = 3;
                            textBox.TellSelected(true);
                        }
                    }




                    if (handler.CheckJustPressedKey(Keys.C))
                        if (!modifyingOne && couldDrag != null && !dragging && colorize == null)
                        {
                            originX = handler.GetMouseX();
                            originY = handler.GetMouseY();

                            r.SetX(originX);
                            g.SetX(originX);
                            b.SetX(originX);

                            r.SetY(originY);
                            g.SetY(originY);
                            b.SetY(originY);
                            colorize = couldDrag;
                        }



                }

                if (handler.CheckLeftMouseJustPressed())
                {
                    if (handler.CheckMouseIn(r))
                    {
                        draggingColor = 1;
                        original = r.GetPoint();
                    }
                    else
                        if (handler.CheckMouseIn(g))
                        {
                            draggingColor = 2;

                            original = g.GetPoint();
                        }
                        else
                            if (handler.CheckMouseIn(b))
                            {
                                draggingColor = 3;

                                original = b.GetPoint();
                            }
                            else
                                if (!modifyingOne && couldDrag != null && !dragging && draggingColor == 0 && colorize == null)
                                {
                                    dragging = true;
                                    original = couldDrag.GetPoint();
                                    dragged = couldDrag;

                                    if (bringToFront)
                                    {
                                        objects.Remove(dragged);
                                        objects.Add(dragged);
                                    }
                                    dragged.SetEditing(!hideEditing);
                                }
                }


                if (handler.CheckRightMouseJustPressed())
                    if (!modifyingOne && couldDrag != null && !dragging)
                    {
                        objects.Remove(couldDrag);
                        couldDrag = null;
                    }




                if (handler.CheckLeftMouseJustReleased())
                {
                    draggingColor = 0;
                    if (dragged != null)
                    {
                        dragged.SetEditing(!hideEditing);
                        dragging = false;
                        dragged = null;
                        couldDrag = null;
                        original = Point.Zero;
                    }
                }

                if (draggingColor != 0 && colorize != null)
                {
                    Point n = handler.GetMouseXY();

                    original = new Point(originX, originY);

                    n -= original;
                    if (n.X >= 130)
                    {
                        n.X = 130;
                    }

                    if (n.Y >= 130)
                    {
                        n.Y = 130;
                    }


                    if (n.Y <= -130)
                    {
                        n.Y = -130;
                    }

                    if (n.X <= -130)
                    {
                        n.X = -130;
                    }

                    if (draggingColor == 1)
                    {
                        //r


                        r.SetPoint(original + n);


                    }
                    else
                        if (draggingColor == 2)
                        {
                            //g
                            g.SetPoint(original + n);
                        }
                        else
                        {
                            //b

                            b.SetPoint(original + n);
                        }

                    int rc = (int)Math.Sqrt((r.GetX() - originX) * (r.GetX() - originX) + (r.GetY() - originY) * (r.GetY() - originY));
                    int gc = (int)Math.Sqrt((g.GetX() - originX) * (g.GetX() - originX) + (g.GetY() - originY) * (g.GetY() - originY));
                    int bc = (int)Math.Sqrt((b.GetX() - originX) * (b.GetX() - originX) + (b.GetY() - originY) * (b.GetY() - originY));

                    colorize.SetColor(new Color(rc, gc, bc));
                }
                else
                    if (dragging)
                    {
                        dragged.SetPoint(original + handler.LeftMouseDraggedBy());
                    }

                if (handler.CheckJustPressedKey(Keys.R))
                {
                    LevelRectangle n = new LevelRectangle(300, 10, 100, 100);
                    n.SetBorderColor(Color.Black);
                    objects.Add(n);
                }

                if (handler.CheckJustPressedKey(Keys.P))
                {
                    LevelRectangle n = new LevelPerson(300, 10, 100, 100, "X");
                    n.SetBorderColor(Color.Black);
                    objects.Add(n);
                }

            } catch(Exception ex)
            {
                Logger.WriteLine(ex.ToString());
                fileMode = 0;
            }

        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch batch, PrimitiveDrawer drawer, FontHandler font)
        {
            if(animationMode)
            {
                tool.Draw(graphicsDevice, batch, drawer, font);
                return;
            }

          
            //batch.DrawString(font.GetLucidaSansTypewriter(), "Hey" + (couldDrag == null), new Vector2(0, 100), Color.Red);
            foreach (LevelObject o in objects)
            {
                if (o is LightLevelCircle) continue;
                o.Draw(drawer, graphicsDevice, batch, font);
            }


            if (colorize != null)
            {
                drawer.DrawFilledRectangle(graphicsDevice, new Rectangle(originX, originY, 5, 5), Color.Magenta);
                drawer.DrawFilledRectangle(graphicsDevice, r, Color.Red);
                drawer.DrawFilledRectangle(graphicsDevice, g, Color.Green);
                drawer.DrawFilledRectangle(graphicsDevice, b, Color.Blue);
            }

            drawer.DrawFilledRectangle(graphicsDevice, (Rectangle)(new GameRectangle(0, 0, 2000, 1000)), new Color(0, 0, 0, 220), false);

            
            foreach (LevelObject o in objects)
            {
                if (o is LightLevelCircle)
                {
                    o.Draw(drawer, graphicsDevice, batch, font);
                    ((LightLevelCircle)o).SetCulled(cullMode);
                }
            }

            if (textBox.GetSelected())
            {
                textBox.Draw(batch, drawer, font, graphicsDevice);
            }

        }



        public bool Finished()
        {
            if(finished)
            {
                finished = false;
                return true;
            }

            return false;
        }

        public  string GetFileName()
        {
            return fileName;
        }
        public string GetSafeFileName()
        {
            return safeFileName;
        }
    }
}
