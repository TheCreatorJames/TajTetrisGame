using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    abstract class DragDropVariablePlaceholder : DragDropElement
    {

        protected LooseDragDropLink varLink;
        protected String typeOfVariable;
        private static Color colorA = new Color(0x48, 0xB3, 0xD4);

        public DragDropVariablePlaceholder()
        {
            this.height = 30;
            this.width = 100;

        }

        public override void TellSelected(bool selected)
        {
            if(varLink != null)
            varLink.GetElement().TellSelected(selected);
            else
            base.TellSelected(selected);
        }

        public DragDropElement GetInnerMostDragDrop(InputHandler handler)
        {
            if (varLink != null && varLink.GetElement().GetTopInteracting(handler) is DragDropVariablePlaceholder)
                return ((DragDropVariablePlaceholder)varLink.GetElement().GetTopInteracting(handler)).GetInnerMostDragDrop(handler);
            else
                return this;
        }

        public override DragDropElement GetInteracting(InputHandler handler)
        {
            if (varLink != null)
                return varLink.GetElement().GetInteracting(handler);
            else
                return this;
        }

        abstract public Boolean AddVariable(LooseDragDropLink link);

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

        public override void SetLooseLink(LooseDragDropLink link)
        {
            return;
        }

        public override LooseDragDropLink GetLooseLink()
        {
            return null;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            if(varLink != null)
            {
                Vector2 vec = this.GetVector();
                this.varLink.SetVector(ref vec);
                this.SetWidth(varLink.GetWidth());
                this.SetHeight(varLink.GetHeight());

                varLink.Draw(spriteBatch, primitiveDrawer, fontHandler, graphicsDevice);
            }
            else
            {
                primitiveDrawer.DrawRoundedRectangle(graphicsDevice, this, colorA);
                Vector2 ve = fontHandler.GetVerdana().MeasureString(this.typeOfVariable);

                this.width = (int)ve.X + 3;
                this.height = 30;

                Vector2 vec = this.GetVector();
                vec.Y += 3;
                vec.X += 3;

                spriteBatch.DrawString(fontHandler.GetVerdana(), this.typeOfVariable, vec + TetrisGameRunner.GetOffsetVector(), Color.White);
                spriteBatch.End();
                spriteBatch.Begin();
            }
        }

        internal LooseDragDropLink PopVariable()
        {
            LooseDragDropLink temp = varLink;
            varLink = null;
            return temp;
        }

        internal bool HasVariable()
        {
            return (varLink != null);
        }
    }
}
