using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    /// <summary>
    /// This class is used to trigger events.
    /// It'll be used by the Tetris game and 
    /// the Taj Parser.
    /// </summary>
    abstract class TetrisEventLinker
    {
        abstract public void RowCleared();

        abstract public void RedCleared(int amount);
        abstract public void GreenCleared(int amount);
        abstract public void BlueCleared(int amount);
        abstract public void OrangeCleared(int amount);
        abstract public void TealCleared(int amount);
        abstract public void PurpleCleared(int amount);
        abstract public void YellowCleared(int amount);
        abstract public void AnyColorCleared(int amount);
    }
}
