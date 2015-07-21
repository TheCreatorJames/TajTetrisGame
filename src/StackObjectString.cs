using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class StackObjectString : StackObject, Saveable
    {
        private string value;

        public StackObjectString()
        {
            this.value = "";
        }

        public StackObjectString(String value)
        {
            this.value = value;
        }

        public StackObjectString(StackObjectString s)
        {
            this.value = s.value;
        }

        public string GetValue()
        {
            return this.value;
        }

        public void SetValue(string data)
        {
            this.value = data;
        }

        public void AddValue(string data)
        {
            this.value += data;
        }



        public void Save(Saver saver)
        {
            saver.Header("StackString");
            saver.Save(value, "Value");
            saver.End();
        }

        public bool ExecuteCommand(string command, Stacker stack)
        {
            if(stack.GetSize() > 1)
            if (stack.CheckFromTop(1, this.GetType()))
            {
                if (command == "+")
                {
                    Add(stack);
                    return true;
                }
                else
                if (command == "==")
                {
                    CheckEquals(stack);
                }
                
            }

            return false;


        }

        private void CheckEquals(Stacker stack)
        {
            stack.Pop();
            bool a = ((StackObjectString)stack.Peek()).GetValue() == this.value;
            stack.InsertTop(new StackObjectBoolean(a));
        }

        private void Add(Stacker stack)
        {
            stack.Pop();
            ((StackObjectString)stack.Peek()).AddValue(this.value);
        }
    }
}
