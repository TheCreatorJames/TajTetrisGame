using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class BooleanDragDrop : DragDropElement
    {
        private bool mode;

        public BooleanDragDrop()
        {
            this.height = 30;
        }

        private const int boolWidth = 50;
        public override void TellSelected(bool selected)
        {
            if (base.GetSelected() && selected) mode = !mode;
            base.TellSelected(selected);
        }

        public override string GetCode()
        {
            return ""+mode;
        }

        public override DragDropElement Clone()
        {
            return new BooleanDragDrop();
        }

        public override DragDropElement GetInteracting(InputHandler handler)
        {
            return this;
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


        private LooseDragDropLink link;
        public override void SetLooseLink(LooseDragDropLink link)
        {
            this.link = link;
        }

        public override LooseDragDropLink GetLooseLink()
        {
            return link;
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            return this;
            //throw new NotImplementedException();
        }

        private static Microsoft.Xna.Framework.Color colorA = new Color(0x00, 0x9E, 0xCC),
                                                    colorB = new Color(0x00, 0xAF, 0xCF);

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            String text = mode ? "True" : "False";
            Color color = !GetSelected() ? colorA : colorB;

            Rectangle numRect = this.ToRectangle();
            numRect.Width = boolWidth;

            Rectangle rect = this.ToRectangle();
            
            this.width = (rect.Width = (int)fontHandler.GetVerdana().MeasureString(" " + text + " ").X + 3) + boolWidth;
            
            rect.X += boolWidth;
            primitiveDrawer.DrawRoundedRectangle(graphicsDevice, rect, color);
            primitiveDrawer.DrawRoundedRectangle(graphicsDevice, numRect, new Color(0x3D, 0x70, 0xBC));
            Vector2 vector = this.GetVector();

            vector.X += 3;
            vector.Y += 3;

            spriteBatch.DrawString(fontHandler.GetVerdana(), " Bool ", vector + TetrisGameRunner.GetOffsetVector(), Color.White);

            vector.X += boolWidth - 3;

            spriteBatch.DrawString(fontHandler.GetVerdana(), " " + text + " ", vector + TetrisGameRunner.GetOffsetVector(), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
        }

        public override void Save(Saver saver)
        {
            saver.Header("BooleanDragDrop");
            saver.Save(this.mode, "Mode");
            saver.End();
        }
    }
}
