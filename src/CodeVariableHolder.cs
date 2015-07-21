using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class CodeVariableHolder : DragDropVariablePlaceholder
    {
        public CodeVariableHolder()
        {
            this.typeOfVariable = "<Next>";
        }

        public override string GetCode()
        {
            if(varLink != null)
            return varLink.GetElement().GetCode();
            return "";
        }

        public override bool AddVariable(LooseDragDropLink link)
        {

            if(link.GetElement() is CodeTextbox)
            {

                this.varLink = link;
                return true;
            
            }

            if (link.GetElement() is SingleReturnMethodElement && ((SingleReturnMethodElement)link.GetElement()).GetReturnType() == "Code")
            {
                this.varLink = link;
                return true;
            }

            return false;
        }

        public override void Save(Saver saver)
        {
            saver.Header("CodeVariableHolder");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.Save(varLink, "Element");
            saver.End();
        }

        public override DragDropElement Clone()
        {
            return new CodeVariableHolder();
            //throw new NotImplementedException();
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            if (varLink != null)
                return varLink.GetElement();
            else return this;
        }
    }
}
