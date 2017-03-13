using System;
using System.Collections.Generic;

namespace OSPSuite.DataBinding.Starter
{
    public class Population
    {
        public Population(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public string DisplayName { get { return string.Format("Display + {0}", Name);}}

        public override bool Equals(object obj)
        {
            var population = obj as Population;
            return population != null && population.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

    }

    public static class Constants
    {
        public static IEnumerable<string> ListOfSpecies()
        {
            return new[] {"Human", "Dog", "Rat"};
        }

        public static IEnumerable<Population> ListOfPopulationFor(string species)
        {
            if(species.Equals("Human"))
                return new[] { new Population("Europeen"), new Population("American"), new Population("Chinese") };

            if (species.Equals("Dog"))
                return new[] { new Population("Small dog"), new Population("Big dog")};

            if (species.Equals("Rat"))
                return new[] { new Population("Small rat"), new Population("Big rat")};

            throw new ArgumentOutOfRangeException(species);
        }


        public static IEnumerable<string> ListOfGenderFor(Population population)
        {
            if (population==null)
                return new List<string>();

            if (population.Name.Equals("Europeen"))
                return new[] { "Male", "Female"};

            if (population.Name.Equals("American"))
                return new[] { "Male"};

            if (population.Name.Equals("Chinese"))
                return new[] { "Male" };

            if (population.Name.Equals("Small dog"))
                return new[] {"Female" };

            if (population.Name.Equals("Big dog"))
                return new[] {"Female" };

            return new[] { "Male"};
        }
    }
}