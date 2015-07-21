using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
namespace TajTetrisGame
{
    abstract class DragDropTextbox : DragDropElement
    {
        protected int position;
        public DragDropTextbox()
        {
            this.width = 200;
            this.height = 30;
            this.text = "";
        }

        protected String text;

        virtual public void AddLetter(char let)
        {
            this.text = this.text.Substring(0, position) + let + this.text.Substring(position);
            position++;
        }



        abstract public void Enter();

        public String GetText()
        {
            return this.text;
        }
        

        public void RemoveLetter()
        {
            if (this.text.Length > 0)
            {
                this.text = this.text.Substring(0, position - 1) + this.text.Substring(position);
                position--;
            }
        }

        public override DragDropElement GetInteracting(InputHandler handler)
        {
            return this;
        }

        public override DragDropElement[] GetOperations()
        {
            return null;
        }

        public override DragDropElement[] GetAssignments()
        {
            return null;
        }

        public override DragDropElement[] GetOptions()
        {
            return null;
        }

        private LooseDragDropLink link;

        public void PositionDecrease()
        {
            if(position > 0)
            position--;
        }

        public void PositionIncrease()
        {
            if(position < this.text.Length)
            position++;
        }

        public override void SetLooseLink(LooseDragDropLink link)
        {
            this.link = link;
        }

        public override LooseDragDropLink GetLooseLink()
        {
            return link;
        }

        virtual public void SetText(string x)
        {
            this.text = x;
            this.position = 0;
        }

        private static Microsoft.Xna.Framework.Color colorA = new Color(0x67, 0xA3, 0x93),
                                                     colorB = new Color(0x6e, 0xCA, 0xBF);

        protected String formatText(FontHandler font)
        {
            String textA;

            if (selected)
            {
                #region Format the String with the Underscore in the person's current position of the text.
                textA = this.text.Substring(0, position) + "_" + this.text.Substring(position);
                #endregion
            }
            else
            {
                #region Shows the string from the end.
                textA = this.text.Substring(0, position) + "" + this.text.Substring(position);
                position = this.text.Length;
                #endregion
            }

            #region Measures the String Repeatedly and cuts off text until it will fit.
            Vector2 vector = font.GetVerdana().MeasureString(textA);
            int count = 0;
            while(vector.X > width)
            {

                if (position >= textA.Length - count - 1) break;
                
                vector = font.GetVerdana().MeasureString(textA.Substring(0, textA.Length - count++));
            }
            textA = textA.Substring(0, textA.Length - count);
           
            while(vector.X > width)
            {
                textA = textA.Substring(1);
                vector = font.GetVerdana().MeasureString(textA);
            }
            #endregion
            return textA;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
           
            if(selected)
            {
                primitiveDrawer.DrawRoundedRectangle(graphicsDevice, this, colorB);
            } else
            {
                primitiveDrawer.DrawRoundedRectangle(graphicsDevice, this, colorA);
            }
            Vector2 vector = this.GetVector() + new Vector2(TetrisGameRunner.GetOffsetX(), TetrisGameRunner.GetOffsetY());
            
            vector.X += 3;
            vector.Y += 3;

            spriteBatch.DrawString(fontHandler.GetVerdana(), formatText(fontHandler), vector, Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
