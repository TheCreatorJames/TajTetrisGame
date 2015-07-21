using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class CodeDragDropHolder : SingleReturnMethodElement
    {


        /*
                suggestions.Add(new LooseDragDropLink(new EventChooserDragDrop("Start")));
                suggestions.Add(new LooseDragDropLink(new EventChooserDragDrop("Cleared Row")));
                suggestions.Add(new LooseDragDropLink(new EventChooserDragDrop("Cleared Green")));
                suggestions.Add(new LooseDragDropLink(new EventChooserDragDrop("Cleared Yellow")));
                suggestions.Add(new LooseDragDropLink(new EventChooserDragDrop("Cleared Red")));
                suggestions.Add(new LooseDragDropLink(new EventChooserDragDrop("Cleared Blue")));
                suggestions.Add(new LooseDragDropLink(new EventChooserDragDrop("Cleared Orange")));
                suggestions.Add(new LooseDragDropLink(new EventChooserDragDrop("Cleared Teal")));
                suggestions.Add(new LooseDragDropLink(new EventChooserDragDrop("Cleared Purple")));
                suggestions.Add(new LooseDragDropLink(new EventChooserDragDrop("Cleared Any Color")));
         */
        private static string[] modes = { "Code", "If", "Else", "EndIf", "Start", "Cleared Row", "Cleared Red", "Cleared Blue", "Cleared Teal", "Cleared Purple", "Cleared Orange", "Cleared Yellow", "Cleared Green", "Cleared Any Color" };
        private int mode;

         private   CodeVariableHolder holderA, holderB;
         private BooleanVariableHolder holderC;
        private const int notWidth = 70;
       
        public CodeDragDropHolder()
        {
            this.height = 30;
            this.mode = 0;
            holderA = new CodeVariableHolder();
            holderB = new CodeVariableHolder();
            holderC = new BooleanVariableHolder();
        }

        public override string GetCode()
        {
            if(mode == 1)
            {
                return " " + holderC.GetCode() + " if "  + holderA.GetCode() + " " + holderB.GetCode();
            } else
                if(mode == 2)
                {
                    return " Else " +  holderA.GetCode() + " " + holderB.GetCode();
                }
            else if(mode ==3 )
            {
                return " End " + holderA.GetCode() + " " + holderB.GetCode();

            }
            else
            {
                return " " + holderA.GetCode() + " " + holderB.GetCode();
            }
        }

        public override string GetReturnType()
        {
            return "Code";
        }

        public override DragDropElement GetInteracting(InputHandler handler)
        {
            if(handler.CheckMouseIn(holderA))
            {
                holderB.TellSelected(false);

                holderC.TellSelected(false);
                return holderA.GetInteracting(handler);
            }
            else
            if(handler.CheckMouseIn(holderB))
            {
                holderA.TellSelected(false);
                holderC.TellSelected(false);
                return holderB.GetInteracting(handler);
            }
            else if (handler.CheckMouseIn(holderC))
            {
                holderA.TellSelected(false);
                holderB.TellSelected(false);
                return holderC.GetInteracting(handler);
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
                this.holderC.TellSelected(selected);
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
                else if (handler.CheckMouseIn(holderC))
                {
                    return holderC;
                }
            return this;
        }

        public override DragDropElement Clone()
        {
            var nOr = new CodeDragDropHolder();
            nOr.holderA = (CodeVariableHolder)holderA.Clone();
            nOr.holderB = (CodeVariableHolder)holderB.Clone();
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


        public override void Save(Saver saver)
        {
            saver.Header("CodeHolder");

            saver.Save(x, "X");
            saver.Save(y, "Y");
            
            saver.Save(mode, "Mode");

            saver.Save(holderA, "A");
            saver.Save(holderB, "B");
            saver.Save(holderC, "C");

            saver.End();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            Vector2 vec = this.GetVector();

            if(mode == 1)
            {

                vec.Y += 33;
                holderC.SetVector(ref vec);

            }

            vec.Y += 33;
            holderA.SetVector(ref vec);
            
            
            Rectangle rect = this.ToRectangle();
            
            rect.Y = (int)vec.Y -33;
            if (mode == 1) rect.Y -= 33;
            this.width = rect.Width = Math.Max(holderA.GetWidth(), Math.Max(holderB.GetWidth(), 130));
            if (mode == 1) this.width = rect.Width = Math.Max(holderC.GetWidth(), rect.Width);
            this.height = rect.Height = 37 + holderA.GetHeight() + holderB.GetHeight();
            if(mode == 1)
            {
                this.height = rect.Height += holderC.GetHeight();

            }
            
            spriteBatch.DrawString(fontHandler.GetVerdana(), modes[mode], GetVector() + TetrisGameRunner.GetOffsetVector(), Color.White);

            vec.Y += 33;
            holderB.SetVector(ref vec);

          
            primitiveDrawer.DrawRoundedRectangle(graphicsDevice, rect, storedColor);
            holderA.Draw(spriteBatch, primitiveDrawer, fontHandler, graphicsDevice);
            
            holderB.Draw(spriteBatch, primitiveDrawer, fontHandler, graphicsDevice);
            if (mode == 1) holderC.Draw(spriteBatch, primitiveDrawer, fontHandler, graphicsDevice);
        }

        public int GetMode()
        {
            return mode;
        }
    }
}
