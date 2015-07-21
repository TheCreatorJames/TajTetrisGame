using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace TajTetrisGame
{
    abstract class DragDropElement : GameObject, Saveable
    {
        //protected static Vector2 vector;
        protected Boolean selected;
        public abstract DragDropElement GetInteracting(InputHandler handler);
        public abstract DragDropElement GetTopInteracting(InputHandler handler);

        
        

        //Code here for interactions.
        
        //Hm, how should I transfer the suggestions?
        //Should I transfer them as an array of DragDropElements?
        //How?

        public abstract DragDropElement Clone();

        public abstract DragDropElement[] GetOperations();
        public abstract DragDropElement[] GetAssignments();
        public abstract DragDropElement[] GetOptions();

        public abstract void SetLooseLink(LooseDragDropLink link);
        
       
        //Code here for responsibilities.
        public virtual void TellSelected(Boolean selected)
        {
            this.selected = selected;
        }

        abstract public LooseDragDropLink GetLooseLink();

        public virtual Boolean GetSelected()
        {
            return selected;
        }

        public abstract void Draw(SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, GraphicsDevice graphicsDevice);


        abstract public string GetCode();

        abstract public void Save(Saver saver);
    }
}
