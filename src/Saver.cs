using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    abstract class Saver
    {
        private string savedData;
        private SaveModes mode;


        public Saver()
        {
            savedData = "";
            mode = SaveModes.Boolean;
        }

        /// <summary>
        /// This method returns the serialized data.
        /// </summary>
        /// <returns>Serialized Data</returns>
        public string GetSavedData()
        {
            return savedData;
        }


        /// <summary>
        /// Creates the header for the current object being saved.
        /// It lets the loader know which object it is building.
        /// </summary>
        /// <param name="head">The name of the class being built</param>
        public void Header(String head)
        {
            savedData += "-|" + head + " ";
        }

        /// <summary>
        /// Tells the loader to build the object from the given information pulled from the loader.
        /// </summary>
        public void End()
        {
            savedData += "Build ";
        }

        /// <summary>
        /// Saves a Float.
        /// </summary>
        /// <param name="x">Float</param>
        /// <param name="variableName">Name of the Variable to Load As.</param>
        public void Save(float x, String variableName)
        {
            Save(x);
            savedData += "_ " + variableName + " ";
        }


        /// <summary>
        /// Saves a Byte.
        /// </summary>
        /// <param name="x">Byte</param>
        /// <param name="variableName">Name of the Variable to Load As</param>
        public void Save(byte x, String variableName)
        {
            Save(x);
            savedData += "_ " + variableName + " ";
        }

        /// <summary>
        /// This would save a double, however, it just saves a float.
        /// This game has no need for doubles.
        /// </summary>
        /// <param name="x">Double</param>
        /// <param name="variableName">Name of the Variable to Load As</param>
        public void Save(double x, String variableName)
        {
            Save(x);
            savedData += "_ " + variableName + " ";
        }

        /// <summary>
        /// This saves a Character.
        /// </summary>
        /// <param name="x">A character to be saved.</param>
        /// <param name="variableName">Name of the Variable to Load as.</param>
        public void Save(char x, String variableName)
        {
            Save(x);
            savedData += "_ " + variableName + " ";
        }
        /// <summary>
        /// This saves an integer.
        /// </summary>
        /// <param name="x">An integer to be saved.</param>
        /// <param name="variableName">Name to be loaded as.</param>
        public void Save(int x, String variableName)
        {
            Save(x);
            savedData += "_ " + variableName + " ";
        }

        /// <summary>
        /// Saves a string.
        /// </summary>
        /// <param name="x">The String to be Saved</param>
        /// <param name="variableName">Name to be loaded as.</param>
        public void Save(string x, String variableName)
        {
            Save(x);
            savedData += "_ " + variableName + " ";
        }

        /// <summary>
        /// Saved a Boolean.
        /// </summary>
        /// <param name="x">Boolean to be Saved.</param>
        /// <param name="variableName">Name to be loaded as.</param>
        public void Save(Boolean x, String variableName)
        {
            Save(x);
            savedData += "_ " + variableName + " ";
        }

        protected void Variable(string variableName)
        {
            savedData += "_ " + variableName + " ";
        }


        private void Save(float x)
        {
            if (!(mode == SaveModes.Float))
            {
                mode = SaveModes.Float;
                savedData += "<Float> ";
            }

            byte[] data = BitConverter.GetBytes(x);

            savedData += Convert.ToBase64String(data);

            savedData += " ";
        }

        private void Save(String x)
        {
            savedData += "[\" " + x + " \"] ";
        }

        private void Save(short x)
        {

            if (!(mode == SaveModes.Short))
            {
                mode = SaveModes.Short;
                savedData += "<Short> ";
            }

            byte[] data = BitConverter.GetBytes(x);

            savedData += Convert.ToBase64String(data);

            savedData += " ";
        }

        private void Save(int x)
        {

            if (!(mode == SaveModes.Int))
            {
                mode = SaveModes.Int;
                savedData += "<Int> ";
            }

            byte[] data = BitConverter.GetBytes(x);

            savedData += Convert.ToBase64String(data);

            savedData += " ";
        }

        private void Save(byte x)
        {
            if (!(mode == SaveModes.Byte))
            {
                mode = SaveModes.Byte;
                savedData += "<Byte> ";
            }
            byte[] data = BitConverter.GetBytes(x);

            savedData += Convert.ToBase64String(data);

            savedData += " ";
        }

        private void Save(char x)
        {

            if (!(mode == SaveModes.Char))
            {
                mode = SaveModes.Char;
                savedData += "<Char> ";
            }


            if (x == ' ') { x = (char)257; }
            savedData += x;

            savedData += " ";
        }

        private void Save(double x)
        {
            Save((float)x); //doubles will not be allowed in this project.
        }

        private void Save(Boolean x)
        {

            if (!(mode == SaveModes.Boolean))
            {
                mode = SaveModes.Boolean;
                savedData += "<Bool> ";
            }

            byte[] data = BitConverter.GetBytes(x);

            savedData += Convert.ToBase64String(data);

            savedData += " ";
        }

        private void Save(Saveable x)
        {
            x.Save(this);
        }



        /// <summary>
        /// Saves an Array as a Given Variable Name
        /// </summary>
        /// <typeparam name="TypeToSave"></typeparam>
        /// <param name="x"></param>
        /// <param name="variableName"></param>
        public void SaveArray<TypeToSave>(TypeToSave[] x, String variableName)
        {
            this.SaveArray<TypeToSave>(x);
            savedData += "_ " + variableName + " ";
        }

        /// <summary>
        /// Saves an Array.
        /// </summary>
        /// <typeparam name="TypeToSave">This is a Generic Type of an Array to Be Saved</typeparam>
        /// <param name="x"></param>
        private void SaveArray<TypeToSave>(TypeToSave[] x)
        {

            savedData += "[ ";
            foreach (Object element in x)
            {
                //If the Saveable object is assignable from the given type, convert it and save it.
                if (typeof(Saveable).IsAssignableFrom(typeof(TypeToSave)))
                {
                    Saveable elem = (Saveable)element;
                    this.Save(elem);
                }
                else
                            if (element is int)
                            {
                                this.Save((int)element);
                            }
                            else
                            if (element is float)
                            {
                                this.Save((float)element);
                            }
                            else
                            if (element is double)
                            {
                                this.Save((double)element);
                            }
                            else if (element is string)
                            {
                                this.Save((string)element);
                            }
                            else if (element is Boolean)
                            {
                                this.Save((bool)element);
                            }
                            else if (element is char)
                            {
                                this.Save((char)element);
                            } else if (element is byte)
                            {
                                this.Save((byte)element);
                            } else if (element is short)
                            {
                                this.Save((short)element);
                            }
            }

            savedData += "] ";

        }

        public void Save(Saveable board, string variableName)
        {
            if (board == null) return;
            board.Save(this);
            savedData += "_ " + variableName + " ";
        }
    }
}
