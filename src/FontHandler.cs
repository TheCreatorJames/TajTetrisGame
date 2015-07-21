using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace TajTetrisGame
{
    class FontHandler
    {
        private static readonly String FontFolder = "Fonts" + Path.DirectorySeparatorChar;
        

        private SpriteFont verdana12;
        private SpriteFont sourceCodePro;
        private SpriteFont lucidaSansTypewriter;
        private SpriteFont wingDings;
        
        
        public void LoadContent(ContentManager Content)
        {
            verdana12 = Content.Load<SpriteFont>(FontFolder + "Verdana");
            sourceCodePro = Content.Load<SpriteFont>(FontFolder + "SourceCodeProLight");
            lucidaSansTypewriter = Content.Load<SpriteFont>(FontFolder + "LucidaSansTypewriter");
            wingDings = Content.Load<SpriteFont>(FontFolder + "Wingdings");
        }

        public SpriteFont GetVerdana()
        {
            return verdana12;
        }

        public SpriteFont GetWingdings()
        {
            return wingDings;
        }

        public SpriteFont GetLucidaSansTypewriter()
        {
            return lucidaSansTypewriter;
        }

        public SpriteFont GetSourceCodePro()
        {
            return sourceCodePro;
        }

    }
}
