using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    /// <summary>
    /// This is implemented so elements can easily be swapped in and out of each other.
    /// The reason it is a GameObject is so that the Input Handlers can interface with it rather directly.
    /// Making it more efficient than always calling GetElement()
    /// </summary>
    class LooseDragDropLink : GameObject, Saveable
    {
        private DragDropElement element;

        public LooseDragDropLink(DragDropElement element)
        {
            this.element = element;
            this.element.SetLooseLink(this);
        }

        public DragDropElement GetElement()
        {
            return element;
        }

        public void ReplaceDragDropElement(DragDropElement element)
        {
            element.SetX(this.element.GetX());
            element.SetY(this.element.GetY());
            bool selected = this.element.GetSelected();
            this.element = element;
            this.element.TellSelected(selected);
            
            this.element.SetLooseLink(this);
           
        }

        public void Draw(SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, GraphicsDevice graphicsDevice)
        {
            element.Draw(spriteBatch, primitiveDrawer, fontHandler, graphicsDevice);
        }

        #region Inherited Methods to Adapt with the element
        public override void SetX(int x)
        {
            element.SetX(x);
        }

        public override void SetY(int y)
        {
            element.SetY(y);
        }

        public override int GetHeight()
        {
            return element.GetHeight();
        }

        public override int GetWidth()
        {
            return element.GetWidth();
        }

        public override int GetX()
        {
            return element.GetX();
        }

        public override int GetY()
        {
            return element.GetY();
        }
        #endregion

        public void Save(Saver saver)
        {
            saver.Header("LooseDragDropLink");
            saver.Save(element, "Element");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.End();
        }
    }
}
