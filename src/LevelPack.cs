using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class LevelPack : Saveable
    {
        private List<String> levels;
        private int unlockedUpTo;
        private bool saveMode;

        public LevelPack()
        {
            levels = new List<string>();
            unlockedUpTo = 0;
        }

        public void SetProfileSaveMode(bool n)
        {
            saveMode = n;
        }


        public string GetLevel(int pos)
        {
            if(pos >= levels.Count)
            {
                return GetLevel(pos % levels.Count);
            }

            if(pos < 0)
            {
                return "Nonexistent";
            }

            if(pos < unlockedUpTo)
            {
                return levels[pos];
            }
            return "Locked";
        }

        public void AddLevel(string level)
        {
            this.levels.Add(level);
        }

        public void AddLevels(string[] levels)
        {
            this.levels.AddRange(levels);        
        }

        public string[] GetProtectedLevels()
        {
            string[] levelC = new string[levels.Count];


            for (int i = 0;i < levels.Count; i++)
            {
                levelC[i] = GetLevel(i);
            }

            return levelC;

        }

        public void RemoveLevel(string level)
        {
            this.levels.Remove(level);
        }

        public void Save(Saver saver)
        {
            saver.Header((saveMode) ? "ProfileLevelPack" : "LevelPack");
            if (saveMode) saver.Save(unlockedUpTo, "Unlocked");
            saver.SaveArray<String>(levels.ToArray(), "Levels");
            saver.End();
        }

        internal void UnlockUpTo(int p)
        {
            unlockedUpTo = p;
        }

        internal void UnlockNext()
        {
            unlockedUpTo++;
        }
    }
}
