using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class NotBooleanMethod : SingleReturnMethodElement
    {
        private BooleanVariableHolder holder;
        private const int notWidth = 50;

        public NotBooleanMethod()
        {
            holder = new BooleanVariableHolder();
            this.height = 30;
        }


        public override string GetCode()
        {
            return holder.GetCode() + " " + "!";
        }

        public override DragDropElement Clone()
        {
            var m = new NotBooleanMethod();
            m.holder = (BooleanVariableHolder)holder.Clone();
            return m;
        }

        public override void TellSelected(bool selected)
        {
            holder.TellSelected(selected);
            base.TellSelected(selected);
        }

        public override string GetReturnType()
        {
            return "Boolean";
        }

        public override DragDropElement GetInteracting(InputHandler handler)
        {
            if(handler.CheckMouseIn(holder))
            {
                return holder.GetInteracting(handler);
            }
            return this;
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            if(handler.CheckMouseIn(holder))
            return holder;

            return this;
            //throw new NotImplementedException();
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

        public override void Save(Saver saver)
        {
            saver.Header("NotBooleanMethod");
            saver.Save(holder, "Holder");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.End();
        }

        private static Color storedColor = new Color(0x3D, 0x70, 0xBC);
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            Rectangle numRect = this.ToRectangle();
            numRect.Width = notWidth;
            this.width = notWidth + holder.GetWidth();

            primitiveDrawer.DrawRoundedRectangle(graphicsDevice, numRect, storedColor);


            Vector2 vector = this.GetVector();

            vector.X += 3;

            vector.Y += 3;

            spriteBatch.DrawString(fontHandler.GetVerdana(), " Not ", vector + TetrisGameRunner.GetOffsetVector(), Color.White);

            vector.X += notWidth - 2;
            vector.Y -= 3;
           

            holder.SetVector(ref vector);
            holder.Draw(spriteBatch, primitiveDrawer, fontHandler, graphicsDevice);
            spriteBatch.End();
            spriteBatch.Begin();


        }
    }
}
