using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TajTetrisGame
{
    /// <summary>
    /// Singleton Implementation of a Logger.
    /// </summary>
    class Logger
    {
        #region Path Information
        private static readonly String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static readonly String GameFolder = "TajTetris";
        private static readonly String AnimationFolder = "Animations";
        private static readonly String LevelFolder = "Levels";
        private static readonly String LevelPackFolder = "LevelPacks";
        #endregion
       
        private static Logger instance;
        private StreamWriter writer; 


        private Logger()
        {
            CheckAndMakeFolder();
           
            #region Find a Name for the Log File that has yet to be used by using the day and year, and a counter number.
            String fileName = path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + "Log " + DateTime.Now.DayOfYear + " " + DateTime.Now.Year;
            int fixer = 0;

            while(File.Exists(fileName + "_" + fixer + ".txt"))
            {
                fixer++;
            }
            #endregion

            writer = new StreamWriter(fileName + "_" + fixer + ".txt");
            instance = this;
            WriteLine("Logging Begins at : " + DateTime.Now);
        }


        #region Load the Single Logger Instance.
        public static void Load()
        {
            if(instance != null)
            {
                instance.writer.Close();
            }
            new Logger();
        }
        #endregion

        #region Methods for Setting up the Directories
        private static Boolean CheckFolder()
        {
            return (Directory.Exists(path + Path.DirectorySeparatorChar + GameFolder));
        }

        private static void MakeFolder()
        {
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + GameFolder);
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + AnimationFolder);

            FileInfo[] files = new DirectoryInfo("Content/Animations").GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + AnimationFolder, file.Name);
                file.CopyTo(temppath, false);
            }

            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + LevelFolder);
            
            files = new DirectoryInfo("Content/Levels").GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + LevelFolder, file.Name);
                file.CopyTo(temppath, false);
            }
            
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + LevelPackFolder);

            files = new DirectoryInfo("Content/LevelPacks").GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + LevelPackFolder, file.Name);
                file.CopyTo(temppath, false);
            }
        }

        private static void CheckAndMakeFolder()
        {
            if (!CheckFolder()) MakeFolder();
        }
        #endregion

        #region Methods to Write to the Log File
        public static void WriteLine(String line)
        {
            instance.writer.WriteLine(line);
            instance.writer.Flush();
        }


        public static void Write(String line)
        {

            instance.writer.Write(line);
            instance.writer.Flush();
        }
        #endregion



    }
}
