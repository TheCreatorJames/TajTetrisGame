using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class TestPerson : Saveable
    {
        private String name;
        private int age;
        private int[] nums;

        public TestPerson(String name, int age)
        {
            this.age = age;
            this.name = name;
            this.nums = new int[]{ 10, 20, 30 };
          
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(String name)
        {
            this.name = name;
        }

        public void SetAge(int age)
        {
            this.age = age;
        }


        public int GetAge()
        {
            return age;
        }
        public void Save(Saver saver)
        {
            saver.Header("Person");
            saver.Save(name, "Name");
            saver.Save(age, "Age");
            //saver.SaveArray<int>(nums, "Nums");
            saver.End();
        }
    }
}
