using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TajTetrisGame
{
    class LevelPerson : LevelRectangle
    {
        private byte blink;
        private ArmTool left, right, leg_left, leg_right;
        private bool loop, playing;
        private int max_frames;
        private string animation;
        private int frame;
        private string name;

        private int ticks;
        private float amountX, amountY;
        private float cx, cy;

        public LevelPerson(int x, int y, int width, int height, String animationFile)//: base(x, y, width, height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.animation = animationFile;
            if(animationFile != "X")
            SetBodyAnimation(animationFile);
            blink = 0;
        }

        public void SetLoop(bool loop)
        {
            this.loop = loop;
        }

        public void SetPlaying(bool play)
        {
            this.playing = play;
        }


        public void MoveTo(int x, int y, int time)
        {
            ticks = time;
            amountX = (float)(-this.x + x) / time;
            amountY = (float)(-this.y + y) / time;
            cx = this.x;
            cy = this.y;
        }
        
        public override void Update(InputHandler handler)
        {

            if(ticks != 0)
            {
                cx += amountX;
                cy += amountY;

                this.x = (int)cx;
                this.y = (int)cy;

                ticks--;
            }

            if (left != null)
            {

                left.Update(handler, GetX(), GetY() + 20);
                right.Update(handler, GetX() + GetWidth(), GetY() + 20);
                leg_left.Update(handler, GetX() + 20, GetY() + GetHeight());
                leg_right.Update(handler, GetX() - 20 + GetWidth(), GetY() + GetHeight());

                if (playing)
                {
                    left.LoadFrame(frame);
                    right.LoadFrame(frame);
                    leg_left.LoadFrame(frame);
                    leg_right.LoadFrame(frame);

                    frame++;

                }
            }

            if(frame >= max_frames)
            {
                frame = 0;
                playing = loop;
            }


            blink++;
            base.Update(handler);
        }


        public void SetBodyAnimation(String animationFile)
        {
            this.animation = animationFile;
            BodyAnimation ba = SaveFileSystem.LoadObjectFromFile<BodyAnimation>("Animations" + Path.DirectorySeparatorChar + animationFile, new GameLoader());

            left = ba.GetLeft();
            left.EndEditing();
            right = ba.GetRight();
            right.EndEditing();
            leg_left = ba.GetLegLeft();
            leg_left.EndEditing();
            leg_right = ba.GetLegRight();
            leg_right.EndEditing();
            this.frame = 0;
            this.max_frames = ba.GetFrames();
        }


        public override void Save(Saver saver)
        {
            saver.Header("LevelPerson");
            saver.Save(x, "X");
            ((GameSaver)saver).Save(color, "Color");
            ((GameSaver)saver).Save(colorB, "Border");
            saver.Save(y, "Y");
            saver.Save(width, "Width");
            saver.Save(height, "Height");
            saver.Save(blink, "Blink");
            saver.Save(animation, "BodyAnimation");
            saver.Save(frame, "Frame");
            saver.Save(playing, "Playing");
            saver.Save(loop, "Loop");
            if(name != "")
            {
                saver.Save(name, "Name");
            }
            saver.End();
        }

        public override void Draw(PrimitiveDrawer drawer, GraphicsDevice graphics, SpriteBatch batch, FontHandler font)
        {
            Vector2 headPlacement = GetVector();
            headPlacement.Y -= 46;
            headPlacement.X += (GetWidth() / 2);

            base.Draw(drawer, graphics, batch, font);
            
            drawer.DrawCircle(graphics, headPlacement, 50, Color.Black, 60);
            drawer.DrawCircle(graphics, headPlacement, 49, Color.Wheat, 60);

            Vector2 lower = GetVector();
            lower.Y += 30;

            lower.X += GetWidth();


            headPlacement.Y += -16;
            headPlacement.X += -28;

            // e     e\n , ____ ,
            string face = " l l\n";
            bool blinking = false;
            string face2 = " ____ ";
            if ((blink > 43 && blink < 56))
            {
                face = "  _  _\n";
                blinking = true;
            }

            if (blink == 110)
            {
                blink = 0;
            }

            if (blink % 140 < 15)
            {
                face2 = "  ____ ";
            }

            if (left != null)
            {
                left.Draw(graphics, batch, drawer, font);
                right.Draw(graphics, batch, drawer, font);
                leg_left.Draw(graphics, batch, drawer, font);
                leg_right.Draw(graphics, batch, drawer, font);
            }

            Vector2 dis = new Vector2(TetrisGameRunner.GetOffsetX(), TetrisGameRunner.GetOffsetY());
             
            batch.DrawString(((!blinking) ? font.GetWingdings() : font.GetVerdana()), face, headPlacement + dis, Color.Black);
            batch.DrawString(font.GetVerdana(), "\n\n", GetVector(), Color.Black, 0, dis + new Vector2(), .82f, SpriteEffects.None, 0);
            batch.DrawString(font.GetVerdana(), "\n" + face2, dis + headPlacement, Color.Black);
        }

        internal void SetFrame(int p)
        {
            this.frame = p;
        }

        public void SetName(string p)
        {
            this.name = p;
        }
    }
}
