using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Height { get; set; }
        public string City { get; set; }
        public List<string> Allergies { get; set; }
    }

    class City
    {
        public string Name { get; set; }
        public int Population { get; set; }
    }

    class Program
    {
        static void Main()
        {
            List<Person> people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe", Height = 180, City = "Houston", Allergies = new List<string> { "Pollen", "Peanuts" } },
            new Person { FirstName = "Alice", LastName = "Smith", Height = 165, City = "Helsinki", Allergies = new List<string> { "Dust", "Peanuts" } },
            new Person { FirstName = "Bob", LastName = "Brown", Height = 175, City = "Warsaw", Allergies = new List<string> { "Pollen", "Cats" } }
        };

            List<City> cities = new List<City>
        {
            new City { Name = "Houston", Population = 2300000 },
            new City { Name = "Helsinki", Population = 650000 },
            new City { Name = "Warsaw", Population = 1800000 }
        };

            
            Func<int, IEnumerable<Person>> getByHeightQuery = height =>
                from person in people
                where person.Height == height
                select person;

            Func<int, IEnumerable<Person>> getByHeightMethod = height =>
                people.Where(person => person.Height == height);

            Console.WriteLine("Persons with height 180 (Query): " + string.Join(", ", getByHeightQuery(180).Select(p => p.FirstName)));
            Console.WriteLine("Persons with height 180 (Method): " + string.Join(", ", getByHeightMethod(180).Select(p => p.FirstName)));

           
            var queryB = from person in people
                         select $"{person.FirstName[0]}. {person.LastName}";

            var methodB = people.Select(person => $"{person.FirstName[0]}. {person.LastName}");

            Console.WriteLine("Formatted Names (Query): " + string.Join(" | ", queryB));
            Console.WriteLine("Formatted Names (Method): " + string.Join(" | ", methodB));

            
            var queryC = (from person in people
                          from allergy in person.Allergies
                          select allergy).Distinct();

            var methodC = people.SelectMany(person => person.Allergies).Distinct();

            Console.WriteLine("Distinct Allergies (Query): " + string.Join(", ", queryC));
            Console.WriteLine("Distinct Allergies (Method): " + string.Join(", ", methodC));

            
            var queryD = (from city in cities
                          where city.Name.StartsWith("H")
                          select city).Count();

            Console.WriteLine("Cities that start with 'H': " + queryD);

            
            var queryE = from person in people
                         join city in cities on person.City equals city.Name
                         where city.Population > 100000
                         select person.FirstName;

            var methodE = people.Join(cities,
                                      person => person.City,
                                      city => city.Name,
                                      (person, city) => new { person, city })
                                .Where(pc => pc.city.Population > 100000)
                                .Select(pc => pc.person.FirstName);

            Console.WriteLine("People in cities with >100K population (Query): " + string.Join(", ", queryE));
            Console.WriteLine("People in cities with >100K population (Method): " + string.Join(", ", methodE));

           
            List<string> selectedCities = new List<string> { "Houston", "Warsaw", "Berlin" };

            var queryF1 = from person in people
                          where selectedCities.Contains(person.City)
                          select person.FirstName;

            var queryF2 = from person in people
                          where !selectedCities.Contains(person.City)
                          select person.FirstName;

            Console.WriteLine("People in selected cities (Query): " + string.Join(", ", queryF1));
            Console.WriteLine("People NOT in selected cities (Query): " + string.Join(", ", queryF2));
        }
    }
}
