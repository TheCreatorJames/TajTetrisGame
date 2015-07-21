using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class SaveFileSystem
    {
        #region File Path Information
        private static readonly String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static readonly String GameFolder = "TajTetris";
        private static readonly String AnimationFolder = "Animations";
        private static readonly String LevelFolder = "Levels";
        private static readonly String LevelPackFolder = "LevelPacks";

        #endregion
        #region Methods for Setting Up the Folder
        private static Boolean CheckFolder()
        {
            return (Directory.Exists(path + Path.DirectorySeparatorChar + GameFolder));
        }

        private static void MakeFolder()
        {
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + GameFolder);
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + AnimationFolder);
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + LevelFolder);
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + LevelPackFolder);
        }

        private static void CheckAndMakeFolder()
        {
            if (!CheckFolder()) MakeFolder();
        }
        #endregion

        public static void SaveFile(Saver save, String fileName)
        {
            CheckAndMakeFolder();

            StreamWriter streamWriter = new StreamWriter(path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + fileName);
            streamWriter.WriteLine(save.GetSavedData());
            streamWriter.Flush();
            streamWriter.Close();
        }

        public static void SaveObjectToFile(Saveable obj, String fileName)
        {
            //This will change as we extend the Saver.
            Saver save = new GameSaver();
            obj.Save(save);
            SaveFile(save, fileName);
        }


        //I'm not quite sure whether the SaveFileSystem will store the loader, or if it will be passed in.
        public static LoadType LoadObjectFromFile<LoadType>(String fileName, Loader load)
        {
            CheckAndMakeFolder();

            String dat = File.ReadAllText(path + Path.DirectorySeparatorChar + GameFolder + Path.DirectorySeparatorChar + fileName);
            dat = dat.Substring(0, dat.LastIndexOf("Build")) + "Build";
            LoadType loadedObject;
            load.ParseLoad<LoadType>(dat, out loadedObject);
            return loadedObject;
        }

        


    }
}
