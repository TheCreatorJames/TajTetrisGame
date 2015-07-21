using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class StringVariableHolder : DragDropVariablePlaceholder
    {
        public StringVariableHolder()
        {
            this.typeOfVariable = "<String>";
        }

        public override string GetCode()
        {
            if (varLink != null)
                return varLink.GetElement().GetCode();


            return "";
        }

        public override bool AddVariable(LooseDragDropLink link)
        {
            if (link.GetElement() is StringTextbox || link.GetElement() is CodeTextbox)
            {
                this.varLink = link;

                return true;
            }


            if (link.GetElement() is SingleReturnMethodElement && ((SingleReturnMethodElement)link.GetElement()).GetReturnType() == "String")
            {
                this.varLink = link;
                return true;
            }

            return false;
        }

        public override DragDropElement Clone()
        {
            return new StringVariableHolder();
            //throw new NotImplementedException();
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            if (varLink != null)
                return varLink.GetElement();
            else return this;
        }

        public override void Save(Saver saver)
        {
            saver.Header("StringVariableHolder");
            saver.Save(this.varLink, "");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.End();
        }

    }
}
