using OSPSuite.Utility.Validation;
using System.Collections.Generic;

namespace OSPSuite.DataBinding.Starter;

public interface IAnInterface : IValidatable
{
   string FirstName { get; set; }
   string LastName { get; set; }
   IAnInterface Child { get; set; }
   string ReadOnlyProp { get; }
   string ValueFromList { get; set; }
   IEnumerable<string> ListOfValues { get; }
   IEnumerable<string> ListOfDisplayValues { get; }
   double Value { get; set; }
   double? NullableValue { get; set; }

   bool Disable { set; get; }
}