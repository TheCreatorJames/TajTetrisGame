#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace TajTetrisGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                using (var game = new TetrisGameRunner())
                    game.Run();
            } catch(Exception ex)
            {
                Logger.WriteLine(ex.ToString());
            }
        }
    }
#endif
}
