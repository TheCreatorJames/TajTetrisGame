using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class CreateClassTextbox : DragDropTextbox
    {
        private DragDropInterface dragDropInterface;
        public CreateClassTextbox(DragDropInterface dragDropInterface) : base()
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

        public override void AddLetter(char let)
        {
            if(char.IsLetter(let))
            base.AddLetter(let);
        }


        public override DragDropElement Clone()
        {
            return new CreateClassManagerButton(this.dragDropInterface);
            //throw new NotImplementedException();
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            return this;
            //throw new NotImplementedException();
        }
        
        public override void Enter()
        {
            this.dragDropInterface.RemoveElement(this.GetLooseLink());
            this.dragDropInterface.SetClassName(this.GetText());
        }
    }
}
