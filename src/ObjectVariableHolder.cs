using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class ObjectVariableHolder : DragDropVariablePlaceholder
    {
         public ObjectVariableHolder()
         {
            this.typeOfVariable = "<Object>";
         }

        public override bool AddVariable(LooseDragDropLink link)
        {
            
            if(link.GetElement() is DragDropTextbox)
            {
                this.varLink = link;
                return true;
     
            }
            
            if (link.GetElement() is SingleReturnMethodElement && ((SingleReturnMethodElement)link.GetElement()).GetReturnType() != "Code")
            {
                this.varLink = link;
                return true;
            }

            return false;
        }

        public override DragDropElement Clone()
        {
            return new ObjectVariableHolder();
            
        }

        public override string GetCode()
        {


            if (varLink != null)
                return varLink.GetElement().GetCode();
            return "";
        }

        public override void Save(Saver saver)
        {
            saver.Header("ObjectVariableHolder");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.Save(varLink, "Link");
            saver.End();
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            if (varLink != null)
                return varLink.GetElement();
            else return this;
        }
    }
}
