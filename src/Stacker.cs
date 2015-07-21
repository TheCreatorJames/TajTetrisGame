using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class Stacker : Saveable
    {
        private Stack<StackObject> stack;

        public Stacker()
        {
            stack = new Stack<StackObject>();
        }

        public bool CheckTop(Type type)
        {
            if (stack.Peek().GetType() == type)
            {
                return true;
            }
            return false;
        }

        public bool CheckFromTop(int pos, Type type)
        {
            if (stack.ElementAt(pos).GetType() == type)
            {
                return true;
            }
            return false;
        }


        public void InsertTop(StackObject obj)
        {
            stack.Push(obj);
        }

        public StackObject Pop()
        {
            return stack.Pop();
        }

        public StackObject Peek()
        {
            return stack.Peek();
        }

        public StackObject FromTop(int pos)
        {
            return stack.ElementAt(pos);
        }

        internal void Clear()
        {
            this.stack.Clear();
        }

        internal float GetSize()
        {
            return stack.Count;
        }

        public void SetStack(StackObject[] stack)
        {
            this.stack.Clear();
            for (int i = stack.Length -1; i >= 0; i--)
            {
                this.stack.Push(stack[i]);
            }
        }

        public void Save(Saver saver)
        {
            saver.Header("Stacker");
            saver.SaveArray<StackObject>(stack.ToArray(), "Stack");
            saver.End();
        }
    }
}
