using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class ModeToggleButton : DragDropElement
    {

        private String text;
        private Color colorOne;
        private Color colorTwo;
        private Vector2 vector;

        public override string GetCode()
        {
            throw new NotImplementedException();
        } 

        public ModeToggleButton(Color one, Color two, String text)
        {
            this.text = text;
            this.colorOne = one;
            this.colorTwo = two;
        }

        public override DragDropElement[] GetAssignments()
        {
            return null;
        }
        public override DragDropElement[] GetOperations()
        {
            return null;
        }

        public override DragDropElement Clone()
        {
            return new ModeToggleButton(colorOne, colorTwo, text);
            //throw new NotImplementedException();
        }

        public override DragDropElement[] GetOptions()
        {
            return null;
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            return this;
        }

        public override void SetLooseLink(LooseDragDropLink link)
        {
            Logger.WriteLine("Someone set a loose link to a toggle button. I hope that was for debugging purposes");
        }
        

      
        public override DragDropElement GetInteracting(InputHandler handler)
        {
            return this;
        }

        public void SetText(String g)
        {

            this.text = g;
        }

        public String GetText()
        {
            return text;
        }

        public override void Draw(SpriteBatch spriteBatch, PrimitiveDrawer pd, FontHandler fontHandler, GraphicsDevice graphicsDevice)
        {
            vector.X = x;
            vector.Y = y;

            if(true)
            {
                Vector2 measurement = fontHandler.GetSourceCodePro().MeasureString(" " + text + " ");
                width = (int)measurement.X;
                height = (int)measurement.Y;
            }
            
            if(selected)
                pd.DrawFilledRectangle(graphicsDevice, this, colorOne);
            else
                pd.DrawFilledRectangle(graphicsDevice, this, colorTwo);

            spriteBatch.DrawString(fontHandler.GetSourceCodePro(), " " + text + " ", vector + TetrisGameRunner.GetOffsetVector(), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
        }

        public override void Save(Saver saver)
        {
            throw new NotImplementedException();
        }

        public override LooseDragDropLink GetLooseLink()
        {
            return null;
        }
    }
}
