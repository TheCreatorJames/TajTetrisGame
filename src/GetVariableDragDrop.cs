using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class GetVariableDragDrop : SingleReturnMethodElement 
    {
        private StringVariableHolder holder;
        private const int notWidth = 110;
        private static string[] modes = { "Get Number", "Get String", "Get Boolean" };
        private int mode;

        public GetVariableDragDrop()
        {
            holder = new StringVariableHolder();
            this.height = 30;
            mode = 0;
        }

        public override string GetCode()
        {

            return "$" + holder.GetCode();
        }



        public override DragDropElement Clone()
        {
            var m = new GetVariableDragDrop();
            m.holder = (StringVariableHolder)holder.Clone();
            return m;
        }

        public override void TellSelected(bool selected)
        {
            if(this.selected && selected)
            {
                mode = (mode + 1) % modes.Length;
            }

            holder.TellSelected(selected);
            base.TellSelected(selected);
        }

        public override string GetReturnType()
        {
            if(mode == 0)
            {
                return "Number";
            } else if (mode == 1)
            {
                return "String";
            }
            else
            {
                return "Boolean";
            }
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

        public override void Save(Saver saver)
        {
            saver.Header("GetVariableDragDrop");
            saver.Save(holder, "Holder");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.Save(mode, "Mode");
            saver.End();
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
    }
}
