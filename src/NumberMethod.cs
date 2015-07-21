using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class NumberMethod : SingleReturnMethodElement
    {
        private NumberVariableHolder holder;
        private const int notWidth = 70;

        private static string[] modes = { "abs", "floor", "round", "neg", "string" };
        private int mode = 0;

        public NumberMethod()
        {
            holder = new NumberVariableHolder();
            this.height = 30;
        }

        public override string GetCode()
        {

            return holder.GetCode() + " " + modes[mode];
        }


        public override DragDropElement Clone()
        {
            var m = new NumberMethod();
            m.holder = (NumberVariableHolder)holder.Clone();
            return m;
        }

        public override void TellSelected(bool selected)
        {
            if(selected && this.selected)
            {
                mode = (mode + 1) % modes.Length;
            }

            holder.TellSelected(selected);
            base.TellSelected(selected);
        }

        public override string GetReturnType()
        {
            if(mode == 4)
            {
                return "String";
            }
            return "Number";
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

            spriteBatch.DrawString(fontHandler.GetVerdana(), modes[mode], vector + TetrisGameRunner.GetOffsetVector(), Color.White);

            vector.X += notWidth - 2;
            vector.Y -= 3;
           

            holder.SetVector(ref vector);
            holder.Draw(spriteBatch, primitiveDrawer, fontHandler, graphicsDevice);
            spriteBatch.End();
            spriteBatch.Begin();


        }

        public override void Save(Saver saver)
        {
            saver.Header("StringMethod");
            saver.Save(holder, "Holder");
            saver.Save(mode, "Mode");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.End();
        }
    }
}
