namespace Lygiagrecios_a
{
    public class PeopleResult : People
    {
        public double BMI;
        public double Bodyfat;
        public PeopleResult(string name, string lastName, int age, double height, double weight, int kids, string gender,
                            double bmi, double bodyfat) : base(name, lastName, age, height, weight, kids, gender)
        {
            BMI = bmi;
            Bodyfat = bodyfat;
        }

    }
}
