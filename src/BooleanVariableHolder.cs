using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class BooleanVariableHolder : DragDropVariablePlaceholder
    {
        public BooleanVariableHolder()
        {
            this.typeOfVariable = "<Boolean>";
        }

        public override string GetCode()
        {
            if (varLink != null)
                return varLink.GetElement().GetCode();

            return "";
        }

        public override DragDropElement Clone()
        {
            return new BooleanVariableHolder();
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {

            if (varLink != null)
                return varLink.GetElement();
            return this;


            //throw new NotImplementedException();
        }



        public override void Save(Saver saver)
        {
            saver.Header("BooleanHolder");

            saver.Save(varLink, "Link");
            saver.Save(x, "X");
            saver.Save(y, "Y");

            saver.End();
        }

        public override bool AddVariable(LooseDragDropLink link)
        {
            if (varLink != null) return false;

            if(link.GetElement() is BooleanDragDrop)
            {
                this.varLink = link;

                return true;
            }

            if(link.GetElement() is SingleReturnMethodElement)
            {
                if (((SingleReturnMethodElement)link.GetElement()).GetReturnType().Equals("Boolean"))
                {
                    this.varLink = link;
                    return true;
                }
            }
            
            return false;
        }
    }
}
