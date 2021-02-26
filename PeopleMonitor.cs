using System.Threading;

namespace Lygiagrecios_a
{
    class PeopleMonitor
    {
        People[] persons;
        int count = 0;
        object _lock = new object();

        public PeopleMonitor(int people)
        {
            persons = new People[people];
        }

        public int Count()
        {
            return count;
        }

        public People GetAndDeletePerson(bool finished)
        {
            lock (_lock)
            {
                while (Count() == 0 && !finished)
                    Monitor.Wait(_lock);

                if (count > 0)
                {
                    Monitor.PulseAll(_lock);
                    return persons[--count];
                }

                if (finished)
                    Monitor.PulseAll(_lock);
            }
            return null;
        }

        public void AddPerson(People person)
        {
            lock (_lock)
            {
                while (count >= persons.Length)
                    Monitor.Wait(_lock);

                if (count < persons.Length)
                {
                    persons[count++] = person;
                    Monitor.PulseAll(_lock);
                }
            }
        }
    }
}
