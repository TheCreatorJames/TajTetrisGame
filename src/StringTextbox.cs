using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class StringTextbox : DragDropTextbox
    {
        private const int strWidth = 65;
        public override void Enter()
        {
            TellSelected(false);
            //throw new NotImplementedException();
        }

        public StringTextbox(string t)
        {
            this.text = t;
        }

        public StringTextbox() : this("")
        {


        }

        public override string GetCode()
        {
            return "\"" + text.Replace(' ', '`') + "\"";
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            return this;
        }

        public override DragDropElement Clone()
        {
            StringTextbox box = new StringTextbox();
            box.text = text;
            return box;

            //throw new NotImplementedException();
        }

        public override void AddLetter(char let)
        {
            if (!GetSelected()) return;
            base.AddLetter(let);
        }

        private static Microsoft.Xna.Framework.Color colorA = new Color(0x00, 0x9E, 0xCC),
                                                    colorB = new Color(0x00, 0xAF, 0xCF);

        private static Color storedColor = new Color(0x3D, 0x70, 0xBC);
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            Rectangle numRect = this.ToRectangle();
            numRect.Width = strWidth;

            Rectangle rect = this.ToRectangle();
            this.width = (rect.Width = (int)fontHandler.GetVerdana().MeasureString(" " + formatText(fontHandler) + " ").X + 3) + strWidth;
            rect.X += strWidth;


            if (this.text.Length > 0 || GetSelected())
                if (selected)
                {
                    primitiveDrawer.DrawRoundedRectangle(graphicsDevice, rect, colorB);
                }
                else
                {
                    primitiveDrawer.DrawRoundedRectangle(graphicsDevice, rect, colorA);
                }


            primitiveDrawer.DrawRoundedRectangle(graphicsDevice, numRect, storedColor);


            Vector2 vector = this.GetVector();

            vector.X += 3;

            vector.Y += 3;

            spriteBatch.DrawString(fontHandler.GetVerdana(), " String ", vector + TetrisGameRunner.GetOffsetVector(), Color.White);

            vector.X += strWidth;

            spriteBatch.DrawString(fontHandler.GetVerdana(), " " + formatText(fontHandler) + " ", vector + TetrisGameRunner.GetOffsetVector(), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
        }

        public override void Save(Saver saver)
        {
            saver.Header("StringTextbox");
            saver.Save(this.text, "Text");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.End();
        }
    }
}
