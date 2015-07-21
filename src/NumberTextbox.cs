using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class NumberTextbox : DragDropTextbox
    {

        public override void Enter()
        {
            //throw new NotImplementedException();
            TellSelected(false);
        }

        public override string GetCode()
        {
            return this.text;
        }

        private const int numWidth = 30;

        private static Microsoft.Xna.Framework.Color colorA = new Color(0x00, 0x9E, 0xCC),
                                                     colorB = new Color(0x00, 0xAF, 0xCF);




        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            return this;
            //throw new NotImplementedException();
        }

        public override DragDropElement Clone()
        {
            var box = new NumberTextbox();
            box.text = text;

            return box;

            //throw new NotImplementedException();
        }

        public override void AddLetter(char let)
        {
            if (!selected) return;

            #region Check and Negate it
            if (let == '-')
            {
                #region Flip to Positive
                if (this.text.Length != 0 && this.text[0] == '-')
               {
                   if (this.text.Length > 1)
                       this.text = this.text.Substring(1);
                   else this.text = "";
               }
                #endregion
                #region Or Flip to Negative
                else
               {
                   this.text = '-' + this.text;
               }
                #endregion

            }
            #endregion

            position = this.text.Length;

            #region Add Decimal
            if (let == '.' && !this.text.Contains('.'))
            {
                base.AddLetter('.');
            }
            #endregion

            if (Char.IsDigit(let))
            base.AddLetter(let);

        }


        public override void Save(Saver saver)
        {

            saver.Header("NumberTextbox");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.Save(text, "Text");

            saver.End();
        }


        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            Rectangle numRect = this.ToRectangle();
            numRect.Width = numWidth;

            Rectangle rect = this.ToRectangle();
            this.width = (rect.Width = (int)fontHandler.GetVerdana().MeasureString(" " + this.text + " ").X + 3) + numWidth;
            rect.X += numWidth;

            
           
            if(this.text.Length > 0 || this.GetSelected())
            if (selected)
            {
                primitiveDrawer.DrawRoundedRectangle(graphicsDevice, rect, colorB);
            }
            else
            {
                primitiveDrawer.DrawRoundedRectangle(graphicsDevice, rect, colorA);
            }

            
            primitiveDrawer.DrawRoundedRectangle(graphicsDevice, numRect, new Color(0x3D, 0x70, 0xBC));


            Vector2 vector = this.GetVector();

            vector.X += 3;
            vector.Y += 3;

            spriteBatch.DrawString(fontHandler.GetVerdana(), " # ", vector + TetrisGameRunner.GetOffsetVector(), Color.White);

            vector.X += numWidth - 3;

            spriteBatch.DrawString(fontHandler.GetVerdana(), " " + this.text + " ", vector + TetrisGameRunner.GetOffsetVector(), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
