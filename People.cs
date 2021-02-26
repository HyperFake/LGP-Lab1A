using System.Runtime.Serialization;

namespace Lygiagrecios_a
{
    public class People
    {
        public string Name;

        public string LastName;

        public int Age;

        public double Height;

        public double Weight;

        public int Kids;

        public string Gender;

        public People(string name, string lastName, int age, double height, double weight, int kids, string gender)
        {
            Name = name;
            LastName = lastName;
            Age = age;
            Height = height;
            Weight = weight;
            Kids = kids;
            Gender = gender;
        }
    }
}
