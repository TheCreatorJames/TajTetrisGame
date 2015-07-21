using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    interface Saveable
    {
        void Save(Saver saver);
    }
}
