using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class NumberVariableHolder : DragDropVariablePlaceholder
    {
        public NumberVariableHolder()
        {
            this.typeOfVariable = "<Number>";
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            if (varLink != null)
                return varLink.GetElement();
            return this;
            //throw new NotImplementedException();
        }
        public override string GetCode()
        {
            if (varLink != null)
                return varLink.GetElement().GetCode();


            return "";
        }

        public override DragDropElement Clone()
        {
            return new NumberVariableHolder();
            //throw new NotImplementedException();
        }

        public override void Save(Saver saver)
        {
            saver.Header("NumberVariableHolder");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.Save(varLink, "Element");
            saver.End();
        }

        public override bool AddVariable(LooseDragDropLink link)
        {
            if (link.GetElement() is NumberTextbox)
            {
                this.varLink = link;

                return true;
            }

            if(link.GetElement() is SingleReturnMethodElement && ((SingleReturnMethodElement)link.GetElement()).GetReturnType() == "Number")
            {
                this.varLink = link;
                return true;
            }

            return false;
        }
    }
}
