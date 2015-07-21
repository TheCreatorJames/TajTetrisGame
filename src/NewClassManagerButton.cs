using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class CreateClassManagerButton : DragDropElement
    {
        private DragDropInterface dragDropInterface;
        private LooseDragDropLink link;
        public CreateClassManagerButton(DragDropInterface dragDropInterface)
        {
            this.dragDropInterface = dragDropInterface;
        
        }
        public override string GetCode()
        {
            throw new NotImplementedException();
        }

        public override void Save(Saver saver)
        {
            throw new NotImplementedException();
        }

        public override DragDropElement Clone()
        {
            return new CreateClassManagerButton(this.dragDropInterface);
            //throw new NotImplementedException();
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

        public override LooseDragDropLink GetLooseLink()
        {
            return link;
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            return this;
            //throw new NotImplementedException();
        }

        public override void TellSelected(bool selected)
        {
            base.TellSelected(selected);
            dragDropInterface.SetClassManager(new DragDropClassManager());
            this.GetLooseLink().ReplaceDragDropElement(new CreateClassTextbox(this.dragDropInterface));
            
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {

            if (width == 0)
            {
                Vector2 measure = fontHandler.GetLucidaSansTypewriter().MeasureString(" New Class ");

                this.width = (int)measure.X;
                this.height = (int)measure.Y;
            }


            
                primitiveDrawer.DrawRoundedRectangle(graphicsDevice, this, Color.Orange);
                spriteBatch.DrawString(fontHandler.GetLucidaSansTypewriter(), " New Class ", TetrisGameRunner.GetOffsetVector() +this.GetVector(), Color.White);
            
            spriteBatch.End();
            spriteBatch.Begin();
        }

        public override void SetLooseLink(LooseDragDropLink link)
        {
            this.link = link;
        }
    }
}
