using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class LightLevelCircle : LevelCircle
    {
        private bool normal;
        private bool culled;

        public LightLevelCircle(int x, int y, int radius) : base(x,y, radius)
        {
            culled = true;

        }

        public void SetCulled(bool culled)
        {
            this.culled = culled;
        }

        public override void Update(InputHandler handler)
        {
            normal = false;
            if(handler.CheckPressedKey(Keys.L) && editing)
            {
                normal = true;
            }
            base.Update(handler);
        }
        public override void Save(Saver saver)
        {
            saver.Header("LightLevelCircle");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.Save(width, "Radius");
            saver.Save(culled, "Culled");
            ((GameSaver)saver).Save(color, "Color");
            saver.End();
        }

        public override void Draw(PrimitiveDrawer drawer, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphics, SpriteBatch batch, FontHandler font)
        
         {
            if(!normal)
             graphics.BlendState = TetrisGameRunner.LightState();
            for (int i = 0;i < 2;i++)
            {
                if(culled)
                drawer.DrawCulledCircle(graphics, GetVector(), GetWidth(), color, Color.Black, 60, new GameRectangle(320, 0, 1024 - 320, 768));
                else
                {
                    drawer.DrawCircle(graphics, GetVector(), GetWidth(), color, Color.Black, 60);
                

                }
            }
            graphics.BlendState = TetrisGameRunner.OriginalState();
            if (editing)
            {
                drawer.DrawFilledRectangle(graphics, point, Color.White);
            }
        }
    }
}
