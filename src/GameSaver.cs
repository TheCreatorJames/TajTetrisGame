using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class GameSaver : Saver
    {

        public void Save(Color color, String varName)
        {
            this.Header("Color");
            this.Save(color.R, "R");
            this.Save(color.G, "G");
            this.Save(color.B, "B");
            this.Save(color.A, "A");
            this.End();

            this.Variable(varName);
        }

        
    }
}
