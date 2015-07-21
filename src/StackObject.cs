using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    interface StackObject : Saveable
    {
        bool ExecuteCommand(string command, Stacker stack);


        void Save(Saver save);
    }
}
