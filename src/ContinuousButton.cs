using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TajTetrisGame
{
    class ContinuousButton : DragDropElement
    {
        Color colorA, colorB;

        public ContinuousButton(Color colorA, Color colorB)
        {
            this.colorA = colorA;
            this.colorB = colorB;
            this.width = 60;
            this.height = 24;
        }
        public override DragDropElement GetInteracting(InputHandler handler)
        {
            return this;
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            return this;
            //throw new NotImplementedException();
        }

        public override string GetCode()
        {
            throw new NotImplementedException();
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

        public override DragDropElement Clone()
        {
            return new ContinuousButton(colorA, colorB);
            //throw new NotImplementedException();
        }

        public override void Save(Saver saver)
        {
            throw new NotImplementedException();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            if(selected)
                primitiveDrawer.DrawRoundedRectangle(graphicsDevice, this, colorB);
            else
            {
                primitiveDrawer.DrawRoundedRectangle(graphicsDevice, this, colorA);
            }
        }

        public override LooseDragDropLink GetLooseLink()
        {
            return null;
        }

        public override void SetLooseLink(LooseDragDropLink link)
        {
            throw new NotImplementedException();
        }
    }
}
