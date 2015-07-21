using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TajTetrisGame
{
    class TestButton : GameObject, Saveable
    {
        static Vector2 vector;
        public TestButton(int x, int y)
        {
            width = 50;
            this.x = x;
            this.y = y;
            height = 50;
        }

        public void Draw(PrimitiveDrawer pd, GraphicsDevice graphics)
        {
            vector.X = x;
            vector.Y = y;

            pd.DrawFilledRectangle(graphics, this, Color.Red);
        }

        public void Save(Saver saver)
        {
            saver.Header("Button");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.End();
        }
        
    }
}
