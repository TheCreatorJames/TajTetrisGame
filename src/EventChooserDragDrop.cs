using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class EventChooserDragDrop : DragDropElement
    {

        private string eventName;
        private LooseDragDropLink link;

        public EventChooserDragDrop(EventChooserDragDrop e)
        {
            this.eventName = e.eventName;
            SetWidth(100);
            SetHeight(20);
        }

        public override string GetCode()
        {
            throw new NotImplementedException();
        }
        public EventChooserDragDrop(string r)
        {
            this.eventName = r;
            SetWidth(100);
            SetHeight(20);
        }

        public override DragDropElement GetInteracting(InputHandler handler)
        {
            return this;
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            return this;
        }

        public override DragDropElement Clone()
        {
            return new EventChooserDragDrop(this);
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

        public override void SetLooseLink(LooseDragDropLink link)
        {
            this.link = link;
        }

        public override LooseDragDropLink GetLooseLink()
        {
            return this.link;
        }

        public override void Save(Saver saver)
        {
            saver.Header("EventChooser");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.Save(eventName, "Event");
            saver.End();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            SetWidth((int)fontHandler.GetVerdana().MeasureString(this.eventName).X + 8);
            Vector2 adjust = GetVector();
            adjust.X += 4;
            spriteBatch.DrawString(fontHandler.GetVerdana(), this.eventName, adjust + TetrisGameRunner.GetOffsetVector(), Color.White);
           
            primitiveDrawer.DrawRoundedRectangle(graphicsDevice, this, Color.Green);
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
