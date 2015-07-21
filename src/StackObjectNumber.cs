using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class StackObjectNumber : StackObject, Saveable
    {
        private float value;
        public StackObjectNumber()
        {
            this.value = 0;
        }

        public StackObjectNumber(float x)
        {
            this.value = x;
        }

        public StackObjectNumber(StackObjectNumber x)
        {
            this.value = x.value;
        }

        public void ModifyValue(float z)
        {
            this.value += z;
        }

        public void MultiplyValue(float z)
        {
            this.value *= z;
        }

        public void DivideValue(float z)
        {
            this.value /= z;
        }

        public void ModuloValue(float z)
        {
            this.value /= z;
        }

        public void SetValue(float z)
        {
            this.value = z;
        }

        public float GetValue()
        {
            return value;
        }


        private void Add(Stacker stack)
        {
            stack.Pop();
            ((StackObjectNumber)stack.Peek()).ModifyValue(this.value);
            
        }

        private void Subtract(Stacker stack)
        {

            stack.Pop();
            ((StackObjectNumber)stack.Peek()).ModifyValue(-this.value);
        }

        private void Multiply(Stacker stack)
        {

            stack.Pop();
            ((StackObjectNumber)stack.Peek()).MultiplyValue(this.value);
        }

        private void Divide(Stacker stack)
        {

            stack.Pop();
            ((StackObjectNumber)stack.Peek()).DivideValue(this.value);

        }

        private void Modulo(Stacker stack)
        {
            stack.Pop();
            ((StackObjectNumber)stack.Peek()).ModuloValue(this.value);
        }



        private void EqualValueCheck(Stacker stack)
        {
            stack.Pop();
            bool e = ((StackObjectNumber)stack.Peek()).EqualsValue(this.value);
            //push it
            stack.Pop();

            stack.InsertTop(new StackObjectBoolean(e));
        }


        private bool EqualsValue(float p)
        {
            return p == value;
        }


        public bool ExecuteCommand(string command, Stacker stack)
        {

            switch (command)
            {
                case "int":
                    this.value = (int)this.value;
                    return true;
                case "neg":
                    this.value *= -1;
                    return true;
                case "round":
                    this.value = (float)Math.Round(this.value);
                    return true;
                case "floor":
                    this.value = (int)this.value;
                    return true;
                case "abs":
                    this.value = (float)Math.Abs(this.value);
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
                    case "+":
                        Add(stack);
                        break;
                    case "-":
                        Subtract(stack);
                        break;
                    case "*":
                        Multiply(stack);
                        break;
                    case "/":
                        Divide(stack);
                        break;
                    case "%":
                        Modulo(stack);
                        break;
                    case "==":
                        EqualValueCheck(stack);
                        break;
                    case "<":
                        LessThan(stack);
                        break;
                    case ">": //!( <= 5)
                        GreaterThan(stack);
                        break;
                    case "<=": //>
                        LessThanOrEqualTo(stack);
                        break;
                    case ">=": //<
                        GreaterThanOrEqualTo(stack);
                        break;
                    default:
                        return false;
                }
                return true;
            }




            return false;
        }

        private void LessThanOrEqualTo(Stacker stack)
        {
            stack.Pop();
            bool e = ((StackObjectNumber)stack.Peek()).value  <= this.value;
            //push it
            stack.Pop();

            stack.InsertTop(new StackObjectBoolean(e));
        }

        private void GreaterThanOrEqualTo(Stacker stack)
        {
            stack.Pop();
            bool e = ((StackObjectNumber)stack.Peek()).value >= this.value;
            //push it
            stack.Pop();

            stack.InsertTop(new StackObjectBoolean(e));
        }

        private void GreaterThan(Stacker stack)
        {
            stack.Pop();
            bool e = ((StackObjectNumber)stack.Peek()).value  > this.value ;
            //push it
            stack.Pop();

            stack.InsertTop(new StackObjectBoolean(e));
        }

        private void LessThan(Stacker stack)
        {
            stack.Pop();
            bool e = ((StackObjectNumber)stack.Peek()).value <  this.value;
            //push it
            stack.Pop();

            stack.InsertTop(new StackObjectBoolean(e));
        }




        public void Save(Saver saver)
        {
            saver.Header("StackNumber");
            saver.Save(value, "Value");
            saver.End();
        }
    }
}
