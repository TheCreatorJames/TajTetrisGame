using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class StringMethod : SingleReturnMethodElement
    {
        private StringVariableHolder holderA, holderB;
        private const int notWidth = 70;
        private static string[] modes = { "+", "==" };
        private int mode;

        public StringMethod()
        {
            this.height = 30;
            mode = 0;
            holderA = new StringVariableHolder();
            holderB = new StringVariableHolder();
        }

        public override string GetReturnType()
        {
            if (mode == 1) return "Boolean";
            return "String";
        }

        public override string GetCode()
        {


            return holderA.GetCode() + " " + holderB.GetCode() + " " + modes[mode];
        }

        public override DragDropElement GetInteracting(InputHandler handler)
        {
            if(handler.CheckMouseIn(holderA))
            {
                holderB.TellSelected(false);
                return holderA.GetInteracting(handler);
            }
            else
            if(handler.CheckMouseIn(holderB))
            {
                holderA.TellSelected(false);
                return holderB.GetInteracting(handler);
            }
            return this;
        }

        public override void TellSelected(bool selected)
        {

            if(selected && this.selected)
            {
                mode = (mode + 1) % modes.Length;
            }


            if(!selected)
            {
                this.holderA.TellSelected(selected);
                this.holderB.TellSelected(selected);
            }

            base.TellSelected(selected);
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            if (handler.CheckMouseIn(holderA))
            {
                return holderA;
            }
            else
                if (handler.CheckMouseIn(holderB))
                {
                    return holderB;
                }
            return this;
        }

        public override DragDropElement Clone()
        {
            var nOr = new StringMethod();
            nOr.holderA = (StringVariableHolder)holderA.Clone();
            nOr.holderA = (StringVariableHolder)holderB.Clone();
            return nOr;
        }

        public override DragDropElement[] GetOperations()
        {
            throw new NotImplementedException();
        }

        public override DragDropElement[] GetAssignments()
        {
            throw new NotImplementedException();
        }

        public override DragDropElement[] GetOptions()
        {
            throw new NotImplementedException();
        }
        private static Color storedColor = new Color(0x3E, 0x40, 0xBC);
       
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            Vector2 vec = this.GetVector();

            holderA.SetVector(ref vec);
            vec.X += holderA.GetWidth() + 1;

            Rectangle rect = this.ToRectangle();
            rect.X = (int)vec.X;
            rect.Y = (int)vec.Y;
            rect.Width = 28;
            vec.X += 29;

            spriteBatch.DrawString(fontHandler.GetVerdana(), modes[mode], new Vector2(rect.X + 4, rect.Y+3) + TetrisGameRunner.GetOffsetVector(), Color.White);

            holderB.SetVector(ref vec);

            this.width = holderA.GetWidth() + holderB.GetWidth() + 30;
            primitiveDrawer.DrawRoundedRectangle(graphicsDevice, rect, storedColor);
            holderA.Draw(spriteBatch, primitiveDrawer, fontHandler, graphicsDevice);
            
            holderB.Draw(spriteBatch, primitiveDrawer, fontHandler, graphicsDevice);

        }

        public override void Save(Saver saver)
        {
            saver.Header("StringMethod");
            saver.Save(holderA, "HolderA");
            saver.Save(holderB, "HolderB");
            saver.Save(mode, "Mode");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.End();
        }
    }
}
