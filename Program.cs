using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Lygiagrecios_a
{
    class Program
    {
        // Helpers
        public static Random rnd = new Random();

        // Changables
        public static int threadCount;

        // Results
        public static PeopleMonitor people;
        public static PeopleResultMonitor rezPeople;
        public static bool Finished = false;

        static void Main(string[] args)
        {

            // 1. Etapas: Nuskaitom failus

            Console.WriteLine("Nuskaitome failus");
            // Nuskaitom json i list
            List<People> personList = JsonConvert.DeserializeObject<List<People>>(File.ReadAllText(@"C:\Users\aisti\OneDrive - Kaunas University of Technology\3 metai\Lygiagrecios\Lygiagrecios_a\Lygiagrecios_a\failas.json"));

            // Random thread count
            threadCount = RandomThreadCounter(personList.Count);
            Console.WriteLine($"Paleisim {threadCount} gijas");

            people = new PeopleMonitor(personList.Count / 2);
            rezPeople = new PeopleResultMonitor(personList.Count);
            Console.WriteLine("Nuskaityta\n");

            // 2. Etapas: Sukuriam ir paleidziam gijas
            // Sukuriam gijas
            Console.WriteLine("Sukuriame gijas");
            List<Thread> th = new List<Thread>();
            for (int i = 0; i < threadCount; i++)
            {
                th.Add(new Thread(new ThreadStart(ThreadWork)));
            }

            // Paleidziam gijas
            Console.WriteLine("Paleidžiame gijas\n");
            foreach (Thread tr in th)
            {
                tr.Start();
            }

            // 3. Etapas: I duomenu struktura irasom duomenis
            // Perrasom duomenis i giju masyva
            Console.WriteLine("Pridedame duomenis");
            for (int i = 0; i < personList.Count; i++)
            {
                people.AddPerson(personList[i]);
            }
            Finished = true;
            Console.WriteLine("Pridėti visi duomenys\n");


            // 4. Etapas: Laukiam kol gijos baigs darba
            Console.WriteLine("Laukiame gijų pabaigos");
            foreach (Thread tr in th)
            {
                tr.Join();
            }
            Console.WriteLine("Gijos baigė darbą\n");

            // 5. Irasom i faila
            Console.WriteLine("Įrašome į failą\n");
            Write();
        }

        static void Write()
        {
            if (File.Exists("res.txt"))
                File.Delete("res.txt");
            using (StreamWriter sw = File.AppendText("res.txt"))
            {
                PeopleResult[] result = rezPeople.GetAllPeople();
                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i] != null)
                    {
                        sw.WriteLine("| Name:{0,-10} | Last Name:{1,-15} | Age:{2,3} | Gender:{3,-6} | Height:{4,-4} | Weight:{5,-4} | Kids:{6,-2} | Body fat:{7,-8} | BMI:{8,-8} |",
                                     result[i].Name, result[i].LastName, result[i].Age, result[i].Gender, result[i].Height, result[i].Weight, result[i].Kids,
                                     HealthCalculator.BFPStatus(result[i].Gender, result[i].Bodyfat), HealthCalculator.BMIStatus(result[i].BMI));

                    }
                }
                sw.Close();
            }
        }

        static void ThreadWork()
        {
            while (true)
            {
                People person = people.GetAndDeletePerson(Finished);

                // Jei null, stapdom gija
                if (person == null)
                {
                    break;
                }
                // Metodai
                else if (person != null)
                {
                    if (HealthCalculator.BMIStatus(HealthCalculator.BMICalculation(person.Weight, person.Height)).ToLower() == "normal")
                    {
                        PeopleResult newPeo = new PeopleResult(
                            person.Name,
                            person.LastName,
                            person.Age,
                            person.Height,
                            person.Weight,
                            person.Kids,
                            person.Gender,
                            HealthCalculator.BMICalculation(person.Weight, person.Height),
                            HealthCalculator.BFPCalculation(person.Weight, person.Height, person.Age, person.Gender.ToLower() == "male" ? 1 : 0));

                        rezPeople.AddPerson(newPeo);
                    }
                }
            }
        }

        public static int RandomThreadCounter(int personCount)
        {
            return rnd.Next(2, personCount / 4);
        }
    }
}
