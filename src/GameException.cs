using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    [Serializable]
    class GameException : Exception
    {
        public GameException()
        {
            Logger.WriteLine("A Game Exception was Thrown.");
        }
        public GameException(string message)
            : base(message)
        {
            Logger.WriteLine("Game Exception Thrown: " + message);
        }
        public GameException(string message, Exception inner) : base(message, inner)
        {
            Logger.WriteLine("Game Exception Thrown: " + message);
        }

    }
}
