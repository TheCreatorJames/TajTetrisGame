using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class FileTextBox : DragDropTextbox
    {
        public override void Enter()
        {
            this.selected = false;
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            return this;
        }

        public override string GetCode()
        {
            throw new NotImplementedException();
        }
        public override void AddLetter(char let)
        {
            if(GetSelected())
            base.AddLetter(let);
        }

        public override void Save(Saver saver)
        {
            throw new NotImplementedException();
        }

        public override DragDropElement Clone()
        {
            return new FileTextBox();
        }

      
    }
}
