using System.Linq;

namespace Lygiagrecios_a
{
    class PeopleResultMonitor
    {
        PeopleResult[] persons;
        int count = 0;
        object _lock = new object();

        public PeopleResultMonitor(int people)
        {
            persons = new PeopleResult[people];
        }

        public int Count()
        {
            lock (_lock)
            {
                return persons.Count();
            }
        }

        public PeopleResult[] GetAllPeople()
        {
            lock (_lock)
            {
                return persons;
            }
        }

        public void AddPerson(PeopleResult person)
        {
            lock (_lock)
            {
                if (count < persons.Length)
                {
                    persons[count++] = person;
                    Sort();
                }
            }
        }

        public void Sort()
        {
            PeopleResult temp;
            // bubble sort
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count - 1; j++)
                {
                    if (persons[j].Age > persons[j + 1].Age)
                    {
                        temp = persons[j + 1];
                        persons[j + 1] = persons[j];
                        persons[j] = temp;
                    }
                }
            }
        }
    }
}
