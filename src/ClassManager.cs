using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class DragDropClassManager
    {
        private Dictionary<String, List<String>> variables;
        private Dictionary<String, List<String>> methods; //This will disperse information about methods loosely
        private List<LooseDragDropLink> elements;

        private String name;

        public DragDropClassManager()
        {
            this.name = "<Default Class>";
        }

        /// <summary>
        /// This method is seemingly haphazard, however,
        /// most classes do not have too many variables.
        /// This is more than okay.
        /// </summary>
        public Boolean CheckForVariableName(String varName)
        {
            foreach (List<String> lst in variables.Values)
            {

                if (lst.Contains(varName)) return true;
            }
            return false;
        }


        public void AddVariable(String name, String variableType)
        {
            if(CheckForVariableName(name))
            {
                throw new GameException("Variable added to class already exists!"); 
            }

            if (variables[variableType] == null) variables[variableType] = new List<string>();
            variables[variableType].Add(name);
        }

        public void StoreElements(List<LooseDragDropLink> elements)
        {
            this.elements = elements;
        }

        public List<LooseDragDropLink> GetElements()
        {
            return elements;
        }

        public void RemoveVariable(String name)
        {
            foreach (List<String> lst in variables.Values)
            {

                if (lst.Contains(name)) lst.Remove(name);
            }
        }


        public string GetName()
        {
            return name;
        }

        internal void SetName(string name)
        {
            this.name = name;
        }
    }
}
