using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace TajTetrisGame
{
    class VectorUtilities
    {
        /// <summary>
        /// This shifts a vector based on height and such, so that the x coordinates make more sense from
        /// the bottom of the screen, rather than the top.
        /// </summary>
        /// <param name="gO">The Game Object to be Drawn.</param>
        /// <returns>An Adjusted Vector</returns>
        public static Vector2 ShiftVector(GameObject gO, int windowHeight)
        {
            Vector2 vec = new Vector2(gO.GetX(), gO.GetY());
            vec.Y += -gO.GetHeight() + windowHeight;
            return vec;
        }

    }
}
