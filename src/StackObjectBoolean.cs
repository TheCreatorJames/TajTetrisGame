using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class StackObjectBoolean : StackObject, Saveable
    {
        private bool value;

        public StackObjectBoolean()
        {
            this.value = false;
        }

        public StackObjectBoolean(bool x)
        {
            this.value = x;
        }

        public StackObjectBoolean(StackObjectBoolean x)
        {
            this.value = x.value;
        }

        public bool GetValue()
        {
            return value;
        }

        public void SetValue(bool x)
        {
            this.value = x;
        }
       

        public bool ExecuteCommand(string command, Stacker stack)
        {
            switch (command)
            {
                case "!":
                    this.value = !value;
                    return true;
                case "string":
                    stack.Pop();
                    stack.InsertTop(new StackObjectString("" + this.value));
                    return true;
            }


            if (stack.GetSize() > 1)
            if(stack.CheckFromTop(1, this.GetType()))
            {
                switch (command)
                {
                    case "==":
                        EE(stack);
                        return true;
                    case "||":
                        OR(stack);
                        return true;
                    case "&&":
                        AND(stack);
                        return true;
                }
            }

            return false;
        }

        private void EE(Stacker stack)
        {
            stack.Pop();
            ((StackObjectBoolean)stack.Peek()).EqualsValue(this.value);
        }

        public void EqualsValue(bool p)
        {
            this.value = this.value == p;
        }

        private void OR(Stacker stack)
        {
            stack.Pop();
            ((StackObjectBoolean)stack.Peek()).OrValue(this.value);
        }

        public void AndValue(bool p)
        {
            this.value = this.value && p;
        }

        private void AND(Stacker stack)
        {
            stack.Pop();
            ((StackObjectBoolean)stack.Peek()).AndValue(this.value);
        }

        public void OrValue(bool p)
        {
            this.value = this.value || p;
        }

        public void Save(Saver saver)
        {
            saver.Header("StackBoolean");
            saver.Save(value, "Value");
            saver.End();
        }
    }
}
