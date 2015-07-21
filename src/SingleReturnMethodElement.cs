using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    abstract class SingleReturnMethodElement : DragDropElement
    {
        abstract public string GetReturnType();


        private LooseDragDropLink link;
        public override void SetLooseLink(LooseDragDropLink link)
        {
            this.link = link;
        }


        public override LooseDragDropLink GetLooseLink()
        {
            return this.link;
        }

        
    }
}
