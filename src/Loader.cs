using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    /// <summary>
    /// This Loader Class allows objects to be loaded from the stack.
    /// This is abstract, as it is meant to be extended per game.
    /// </summary>
    abstract class Loader
    {
        #region Stack Building Variables
        protected Stack<Object> buildStack;
        protected Stack<String> currentClass;
        protected Stack<Dictionary<String, Object>> variablesToBuild;
        protected SaveModes mode;
        #endregion

        public Loader()
        {
            buildStack = new Stack<object>();
            currentClass = new Stack<string>();
            variablesToBuild = new Stack<Dictionary<string, object>>();
            mode = SaveModes.Boolean;
        }

        public static ConvertType[] convertArray<ConvertType>(Object[] o)
        {
            return (o.Select(p => (ConvertType)p).ToArray());
        }

        abstract protected void BuildClass();

        #region Generic Method for Loading the Type of Object
        public void ParseLoad<TypeToBuild>(String data, out TypeToBuild builtObject)
        {
            String[] splitData = data.Split(' ');

            #region Boolean Modes
            bool assignValue = false;
            bool stringMode = false;
            bool arrayMode = false;
            bool complicatedArray = false;
            #endregion

            int complex = 0; //In case the built classes have classes nested inside of them.

            List<Object> buildArray = null;
            String buildString = "";

            #region Reads through each word.
            foreach (String dat in splitData)
            {
                if (dat.Length == 0) continue;

                #region Assign a Value to a Variable
                if (assignValue)
                {
                    variablesToBuild.Peek()[dat] = buildStack.Pop();
                    assignValue = false;
                    continue;
                }
                #endregion

                #region Builds the String.
                if (stringMode)
                {
                    if(dat.Equals("\"]"))
                    {
                        if (buildString.Length > 0)
                            buildStack.Push(buildString.Substring(0, buildString.Length - 1));
                        else buildStack.Push("");
                        buildString = "";
                        stringMode = false;

                        if (!complicatedArray && arrayMode)
                        {
                            buildArray.Add(buildStack.Pop());
                        }

                        continue;
                    }

                    buildString += dat + " ";
                    continue;
                }
                #endregion
                #region Builds the Array, if Array Mode.
                if (arrayMode)
                {
                    if (dat.Equals("]"))
                    {
                        buildStack.Push(buildArray.ToArray());
                        buildArray = null;
                        arrayMode = false;
                        continue;
                    }
               
                }
                #endregion

                #region Sets which Class it it Building
                if (dat.IndexOf("-|") == 0)
                {
                    if (arrayMode) complicatedArray = true;
                    if (complicatedArray) complex++;
                    variablesToBuild.Push(new Dictionary<string, object>());
                    currentClass.Push(dat.Substring(2));

                }
                #endregion
                #region Starts building an Array in ArrayMode.
                else if (dat.Equals("["))
                {
                    arrayMode = true;
                    buildArray = new List<object>();
                }
                #endregion
                #region Starts building a String.
                else if (dat.Equals("[\""))
                {
                    stringMode = true;
                }
                #endregion
                #region Setting the Loading Mode.
                else if (dat.Equals("<Bool>"))
                {
                    mode = SaveModes.Boolean;
                }
                else if (dat.Equals("<Int>"))
                {
                    mode = SaveModes.Int;
                }
                else if (dat.Equals("<Char>"))
                {
                    mode = SaveModes.Char;
                }
                else if (dat.Equals("<Float>"))
                {
                    mode = SaveModes.Float;
                }
                else if (dat.Equals("<Byte>"))
                {
                    mode = SaveModes.Byte;
                }
                else if (dat.Equals("<Short>") || dat.Equals("<short>"))
                {
                    mode = SaveModes.Short;
                }
                #endregion
                #region Builds the Current Class
                else if (dat.Equals("Build"))
                {
                    BuildClass();
                    if (complicatedArray && complex == 1)
                    {
                        buildArray.Add(buildStack.Pop());
                        complicatedArray = false;
                    }
                    complex--;
                    if(complex < 0)
                    {
                        complex = 0;
                    }
                }
                #endregion
                #region Tells it to assign the value to the next word variable.
                else if (dat.Equals("_"))
                {
                    assignValue = true;
                }
                #endregion
                #region Load one of the Primitive Variables
                else
                {

                    byte[] info = Convert.FromBase64String(dat);
                    #region Takes the Base64 String, and converts it to the specified type.
                    switch (mode)
                    {
                    case SaveModes.Boolean:
                        buildStack.Push(BitConverter.ToBoolean(info, 0));
                        break;
                    case SaveModes.Byte:
                        buildStack.Push(info[0]);
                        break;
                    case SaveModes.Char:
                        buildStack.Push(BitConverter.ToChar(info, 0));
                        break;
                    case SaveModes.Float:
                        buildStack.Push(BitConverter.ToSingle(info, 0));
                        break;
                    case SaveModes.Int:
                        buildStack.Push(BitConverter.ToInt32(info, 0));
                        break;
                    case SaveModes.Short:
                        buildStack.Push(BitConverter.ToInt16(info, 0));
                        break;
                    }
                    #endregion

                    #region Pulls from the build stack to place in building an array.
                    if (arrayMode)
                    {
                        if(!complicatedArray)
                        buildArray.Add(buildStack.Pop());
                    }
                    #endregion
                }
                #endregion


            }
            #endregion

            #region Checks to see if the object built is what was requested.
            if (buildStack.Peek() is TypeToBuild)
            {
                builtObject = (TypeToBuild)buildStack.Pop();
                return;
            }
            #endregion

            //Just in case.
            builtObject = default(TypeToBuild);
        }
        #endregion

    }
}
