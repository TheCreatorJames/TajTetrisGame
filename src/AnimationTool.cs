using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace TajTetrisGame
{
    class BodyAnimation : Saveable
    {
        private ArmTool left, right, leg_left, leg_right; 
        private int frames;
        public BodyAnimation(ArmTool left, ArmTool right, ArmTool leg_left, ArmTool leg_right, int frames)
        {
            this.leg_left = leg_left;
            this.left = left;
            this.right = right;
            this.leg_right = leg_right;
            this.frames = frames;
        }


        public int GetFrames()
        {
            return frames;
        }

        public ArmTool GetLeft()
        {
            return left; 

        }

        public ArmTool GetRight()
        {
            return right;

        }
        public ArmTool GetLegRight()
        {
            return leg_right;

        }
        public ArmTool GetLegLeft()
        {
            return leg_left;

        }

        public void Save(Saver saver)
        {
            saver.Header("BodyAnimation");
            saver.Save(frames, "Frames");
            saver.Save(left, "Left");
            saver.Save(right, "Right");
            saver.Save(leg_left, "LegLeft");
            saver.Save(leg_right, "LegRight");
            saver.End();
        }
    }


    class AnimationTool
    {
        private const int SPEED = 3;
        private Point originalPoint;
        private bool dragging, mode, modeB;
        private int frames, ff;
        private int sides = 60;
        private GameRectangle testingRectangle;
        private bool dragBody;
        private Color color;
        private byte blink = 0;
        private ArmTool left, right, lleft, lright;
        private byte fileMode;
        private bool done;
        private FileTextBox textBox;
       
        

        private bool squareOrCircle = false;
        public AnimationTool()
        {
            testingRectangle = new GameRectangle(400, 200, 100, 100);
            color = Color.CadetBlue;
            left = new ArmTool();
            right = new ArmTool();
            lleft = new ArmTool();
            lright = new ArmTool();
            textBox = new FileTextBox();
            textBox.SetX(500);
            textBox.SetY(200);
        }

        

        public void Use(int x, int y, int width, int height)
        {
            testingRectangle.SetX(x);
            testingRectangle.SetY(y);
            testingRectangle.SetWidth(width);
            testingRectangle.SetHeight(height);
            frames = 0;
            ff = 0;
            left.Clear();
            right.Clear();
            lleft.Clear();
            lright.Clear();
        }

        
        public void Update(InputHandler handler)
        {
            try
            {
                if (done)
                {
                    done = false;
                    return;
                }

                if (modeB)
                {

                    if(frames == 0)
                    {
                        ff = 0;
                        modeB = false;
                        return;
                    }
                    ff++;
                    ff %= frames;
                    left.LoadFrame(ff);
                    right.LoadFrame(ff);
                    lright.LoadFrame(ff);
                    lleft.LoadFrame(ff);
                }




                DragDropInterface.UpdateTextBox(textBox, handler);
                if (textBox.GetSelected()) return;
                if (fileMode != 0)
                {
                    if (fileMode == 1)
                    {
                        SaveFileSystem.SaveObjectToFile(new BodyAnimation(left, right, lleft, lright, frames), "Animations" + Path.DirectorySeparatorChar + textBox.GetText() + ".taj");
                        textBox.SetText("");

                    }
                    else if (fileMode == 2)
                    {
                        BodyAnimation a = SaveFileSystem.LoadObjectFromFile<BodyAnimation>("Animations" + Path.DirectorySeparatorChar + textBox.GetText() + ".taj", new GameLoader());
                        this.left = a.GetLeft();
                        this.lleft = a.GetLegLeft();
                        this.lright = a.GetLegRight();
                        this.right = a.GetRight();

                        this.frames = a.GetFrames();


                        this.ff = 0;
                        textBox.SetText("");
                    }
                    fileMode = 0;
                }



                left.Update(handler, testingRectangle.GetX(), testingRectangle.GetY() + 20);
                right.Update(handler, testingRectangle.GetX() + testingRectangle.GetWidth(), testingRectangle.GetY() + 20);
                lleft.Update(handler, testingRectangle.GetX() + 20, testingRectangle.GetY() + testingRectangle.GetHeight());
                lright.Update(handler, testingRectangle.GetX() - 20 + testingRectangle.GetWidth(), testingRectangle.GetY() + testingRectangle.GetHeight());




                if (!dragging && handler.CheckJustPressedKey(Keys.M))
                {
                    mode = !mode;
                }

                if (handler.CheckJustPressedKey(Keys.Escape) || handler.CheckJustPressedKey(Keys.A))
                {
                    done = true;
                }


                if (handler.CheckJustPressedKey(Keys.OemSemicolon))
                {
                    if (frames == 0) return;
                    modeB = !modeB;
                }



                if (handler.CheckJustPressedKey(Keys.D1))
                {
                    textBox.TellSelected(true);
                    fileMode = 1;

                }

                if (handler.CheckJustPressedKey(Keys.D2))
                {

                    textBox.TellSelected(true);
                    fileMode = 2;

                }


                if (handler.CheckJustPressedKey(Keys.S))
                {
                    left.SaveArm();
                    right.SaveArm();
                    lright.SaveArm();
                    lleft.SaveArm();
                    frames++;
                    ff++;
                }

                if (handler.CheckJustPressedKey(Keys.D))
                {
                    left.Clear(ff);
                    right.Clear(ff);
                    lright.Clear(ff);
                    lleft.Clear(ff);
                    frames = ff;

                }

                if (handler.CheckJustPressedKey(Keys.C))
                {
                    if (frames == 0) return;
                    if (left.Using())
                    {
                        left.SaveAt(ff, true);
                    }

                    if (right.Using())
                    {
                        right.SaveAt(ff, true);
                    }
                    if (lleft.Using())
                    {
                        lleft.SaveAt(ff, true);
                    }
                    if (lright.Using())
                    {
                        lright.SaveAt(ff, true);
                    }


                    ff++;

                    ff %= frames;
                    left.LoadFrame(ff);
                    right.LoadFrame(ff);
                    lright.LoadFrame(ff);
                    lleft.LoadFrame(ff);
                }

                if (handler.CheckJustPressedKey(Keys.V))
                {
                    if (frames == 0) return;

                    if (left.Using())
                    {
                        left.SaveAt(ff, false);
                    }

                    if (right.Using())
                    {
                        right.SaveAt(ff, false);
                    }
                    if (lleft.Using())
                    {
                        lleft.SaveAt(ff, false);
                    }
                    if (lright.Using())
                    {
                        lright.SaveAt(ff, false);
                    }


                    ff++;

                    ff %= frames;
                    left.LoadFrame(ff);
                    right.LoadFrame(ff);
                    lright.LoadFrame(ff);
                    lleft.LoadFrame(ff);
                }



                if (handler.CheckJustPressedKey(Keys.B))
                {
                    dragBody = !dragBody;
                }


                if (handler.CheckJustPressedKey(Keys.Z))
                {
                    squareOrCircle = !squareOrCircle;
                }

                if (handler.CheckJustPressedKey(Keys.OemPlus))
                {
                    ff++;
                    ff %= frames;
                    left.LoadFrame(ff);
                    right.LoadFrame(ff);
                    lright.LoadFrame(ff);
                    lleft.LoadFrame(ff);

                }

                if (handler.CheckJustPressedKey(Keys.OemMinus))
                {
                    ff--;
                    if (ff < 0) ff = 0;
                    left.LoadFrame(ff);
                    right.LoadFrame(ff);
                    lright.LoadFrame(ff);
                    lleft.LoadFrame(ff);
                }


                if (handler.CheckPressedKey(Keys.LeftShift) || handler.CheckPressedKey(Keys.RightShift))
                {
                    if (handler.CheckPressedKey(Keys.Down))
                    {
                        testingRectangle.ModifyHeight(SPEED);
                    }

                    if (handler.CheckPressedKey(Keys.Up))
                    {
                        testingRectangle.ModifyHeight(-SPEED);
                    }

                    if (handler.CheckPressedKey(Keys.Left))
                    {
                        testingRectangle.ModifyWidth(-SPEED);
                    }

                    if (handler.CheckPressedKey(Keys.Right))
                    {
                        testingRectangle.ModifyWidth(SPEED);
                    }
                }
                else
                {
                    if (handler.CheckPressedKey(Keys.Down))
                    {
                        testingRectangle.ModifyY(SPEED);
                    }

                    if (handler.CheckPressedKey(Keys.Up))
                    {
                        testingRectangle.ModifyY(-SPEED);
                    }

                    if (handler.CheckPressedKey(Keys.Left))
                    {
                        testingRectangle.ModifyX(-SPEED);
                    }

                    if (handler.CheckPressedKey(Keys.Right))
                    {
                        testingRectangle.ModifyX(SPEED);
                    }
                }

                #region Check if the mouse is in the body.
                if (dragBody)
                    if (handler.CheckMouseIn(testingRectangle))
                    {

                        if (handler.CheckLeftMousePressed())
                        {
                            if (!dragging && handler.LeftMouseDraggedBy().X == 0 && handler.LeftMouseDraggedBy().Y == 0)
                            {
                                this.originalPoint = testingRectangle.GetPoint();
                                if (mode) this.originalPoint = testingRectangle.GetSize();
                                dragging = true;
                            }



                        }
                        else
                        {
                            color = Color.Red;
                            dragging = false;
                        }




                    }
                    else
                    {
                        color = Color.DarkGray;
                    }

                if (dragging)
                {
                    color = Color.DarkRed;
                    Point fix = originalPoint + handler.LeftMouseDraggedBy();
                    if (mode)
                    {
                        testingRectangle.SetSize(ref fix);

                    }
                    else
                    {

                        testingRectangle.SetPoint(ref fix);
                    }
                }

                #endregion




                blink++;
            } catch (Exception ex)
            {
                Logger.WriteLine(ex.ToString());
                fileMode = 0;
            }
        }

        private Vector2 Mirror(Vector2 n)
        {
            Vector2 a = n;

            a.X -= testingRectangle.GetX();
            a.X *= -1;
            a.X += testingRectangle.GetWidth();
            a.X += testingRectangle.GetX();  

            return a;
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch batch, PrimitiveDrawer drawer, FontHandler font)
        {
            Vector2 headPlacement = testingRectangle.GetVector(); 
            headPlacement.Y -= 46; 
            headPlacement.X += (testingRectangle.GetWidth() / 2);

            if(textBox.GetSelected())
            textBox.Draw(batch, drawer, font, graphicsDevice);
            
            drawer.DrawRoundedRectangle(graphicsDevice, testingRectangle.ScalePosition(-1,-1).ScaleSize(2,2), Color.Black);
            drawer.DrawRoundedRectangle(graphicsDevice, testingRectangle, color);

            drawer.DrawCircle(graphicsDevice, headPlacement, 50, Color.Black, sides);
            drawer.DrawCircle(graphicsDevice, headPlacement, 49, Color.Wheat, sides);

            Vector2 lower = testingRectangle.GetVector();
            lower.Y += 30;
           
            lower.X += testingRectangle.GetWidth();


            headPlacement.Y += -16;
            headPlacement.X += -28;

            // e     e\n , ____ ,
            string face = " l l\n";
            if (squareOrCircle) face = " n n\n";
            bool blinking = false;
            string face2 = " ____ ";
            if ((blink > 43 && blink < 56) )
            {
                face = "  _  _\n";
                blinking = true;
            }

            if(blink == 110)
            {
                blink = 0;
            }

            if(blink % 140 < 15 )
            {
                face2 = "  ____ ";
            }

            left.Draw(graphicsDevice, batch, drawer, font);
            right.Draw(graphicsDevice, batch, drawer, font);
            lleft.Draw(graphicsDevice, batch, drawer, font);
            lright.Draw(graphicsDevice, batch, drawer, font);
            Vector2 dis = new Vector2(TetrisGameRunner.GetOffsetX(), TetrisGameRunner.GetOffsetY());
            batch.DrawString(font.GetVerdana(), "Frame : " + ff + "     "+"S: Save Frame     C: Correct Frame        V: Correct Frame Dynamic\nH: Hide Dots", new Vector2(300, 0) + dis, Color.White);

            batch.DrawString(((!blinking) ? font.GetWingdings() : font.GetVerdana()), face, headPlacement + dis, Color.Black);
             
            batch.DrawString(font.GetVerdana(), "\n\n", testingRectangle.GetVector(), Color.Black, 0, dis + new Vector2(), .82f, SpriteEffects.None, 0);

            batch.DrawString(font.GetVerdana(), "\n"+face2, dis + headPlacement, Color.Black);
        }


        public bool GetDone()
        {
            return done;
        }
    }
}
