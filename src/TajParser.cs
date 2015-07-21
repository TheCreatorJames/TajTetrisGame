using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    /// <summary>
    /// This is a Semi-Complete Taj Scripting Language.
    /// </summary>
    class TajParser : Saveable
    {
        private Dictionary<String, StackObject> variables;
        private Stacker stack;
        private StackObject customHandler;


        public TajParser()
        {
            variables = new Dictionary<string, StackObject>();
            stack = new Stacker();
        }

        public void SetCustomHandler(StackObject o)
        {
            this.customHandler = o;
        }


        /// <summary>
        /// Sets a variable. Certain variables should not be changed until set.
        /// So it copies them.
        /// </summary>
        /// <param name="varName"></param>
        public void SetVariable(string varName)
        {
            #region Creates a copy of certain types of Variables.
            if (stack.Peek() is StackObjectBoolean)
            {
                variables[varName] = new StackObjectBoolean(((StackObjectBoolean)stack.Peek()));
            }
            else
            if (stack.Peek() is StackObjectNumber)
            {
                variables[varName] = new StackObjectNumber(((StackObjectNumber)stack.Peek()));
            }
            else
            if (stack.Peek() is StackObjectString)
            {
                variables[varName] = new StackObjectString(((StackObjectString)stack.Peek()));
            } 
            #endregion
            else
            {
                variables[varName] = stack.Peek();
            }
        }

        /// <summary>
        /// Gets a Variable from Saved Variables.
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public StackObject GetVariable(string varName)
        {
            #region Copy from the Variables if a certain type of object
            if (!variables.ContainsKey(varName))
            {
                return null;
            }
            if (variables[varName] is StackObjectBoolean)
            {
                return new StackObjectBoolean(((StackObjectBoolean)variables[varName]));
            }
            else
            if (variables[varName] is StackObjectNumber)
            {
                return new StackObjectNumber(((StackObjectNumber)variables[varName]));
            }
            else
            if (variables[varName] is StackObjectString)
            {
                return new StackObjectString(((StackObjectString)variables[varName]));
            } 
            #endregion
            else
            {
                return variables[varName];
            }

        }


        /// <summary>
        /// Parses the code passed in.
        /// </summary>
        /// <param name="code"></param>
        public void Parse(String code)
        {
            code = code.Replace('\n', ' ');

            for (int i = 0;i < 5; i++)
                code = code.Replace("  ", " ");

            code = code.Replace('\t', ' ');

            Stack<bool> ifMode = new Stack<bool>();

            String[] codeSplit = code.Split(' ');


            foreach (string data in codeSplit)
            {
                if (data.Length == 0) continue;


                if(ifMode.Count != 0)
                {
                    if(data.ToLower() == "else")
                    {
                        ifMode.Push(!ifMode.Pop());
                        continue;
                    }

                    if(data.ToLower() == "end")
                    {
                        ifMode.Pop();
                        continue;
                    }

                    if(ifMode.Peek())
                    {
                        //No issues, go on.

                    }
                    else
                    {
                        continue;
                    }
                }

                if (data == "del")
                {
                    stack.Pop();
                    continue;
                }

                if(data == "if")
                {
                    ifMode.Push(((StackObjectBoolean)stack.Pop()).GetValue());
                    continue;
                }

                if (data == "len")
                {
                    stack.InsertTop(new StackObjectNumber(stack.GetSize()));
                    continue;
                }

                if (data == "clr")
                {
                    stack.Clear();
                    variables.Clear();
                    continue;
                }

                //Set Variable
                if (data[0] == '%')
                {
                    string varName = data.Substring(1);
                    SetVariable(varName);
                    continue;
                }
                
                //Load Variable
                if (data[0] == '$')
                {
                    string varName = data.Substring(1);
                    stack.InsertTop(GetVariable(varName));
                    continue;
                }

                if (data == "True" || data == "true")
                {
                    stack.InsertTop(new StackObjectBoolean(true));
                    continue;
                }

                if (data == "False" || data == "false")
                {
                    stack.InsertTop(new StackObjectBoolean(false));
                    continue;
                }

                //I wanted to be lazy, therefore, spaces in strings will be converted x).
                if (data[0] == '\"' && data[data.Length - 1] == '\"')
                {
                    stack.InsertTop(new StackObjectString(data.Substring(1,data.Length-2).Replace('`', ' ').Replace("\\n", "\n")));
                    continue;
                }

                float t;
                if (float.TryParse(data, out t))
                {
                    stack.InsertTop(new StackObjectNumber(t));
                    continue;
                }

                if(stack.GetSize() == 0  || (!stack.Peek().ExecuteCommand(data, stack)))
                {
                    //custom execution passed on.
                    customHandler.ExecuteCommand(data, stack);
                }
            }
        }



        #region For serialization purposes.
        public void SetStack(Stacker s)
        {
            this.stack = s;
        }

        public void SetVariables(String[] s, StackObject[] b)
        {
            variables.Clear();
            for (int i = 0; i < s.Length; i++)
            {
                variables[s[i]] = b[i];
            }
        }
        #endregion


        public void Save(Saver saver)
        {
            saver.Header("TajParser");
            saver.Save(stack, "Stack");
            saver.SaveArray<String>(variables.Keys.ToArray(), "Keys");
            saver.SaveArray<StackObject>(variables.Values.ToArray(), "Values");
            saver.End();
        }
    }
}
