using System;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace OSPSuite.DataBinding.Starter;

public class AnImplementation : IAnInterface
{
   public event EventHandler FirstNameChanged = delegate { };
   public event EventHandler DisableChanged = delegate { };
   public AnImplementation()
   {
      ValueFromList = "value2";
   }

   private string _firstName;
   private bool _disable;

   public string FirstName
   {
      get { return _firstName; }
      set
      {
         _firstName = value;
         FirstNameChanged(this, EventArgs.Empty);
      }
   }

   public double Value { get; set; }
   public string LastName { get; set; }

   public IAnInterface Child { get; set; }
   public string ReadOnlyProp { get; private set; }
   public string ValueFromList { get; set; }
   public double? NullableValue { get; set; }

   public bool Disable
   {
      get
      {
         return _disable;

      }
      set
      {
         _disable = value;
         DisableChanged(this, EventArgs.Empty);
      }
   }

   public IEnumerable<string> ListOfValues
   {
      get { return new List<string> { "value1", "value2", "value3" }; }
   }

   public IEnumerable<string> ListOfDisplayValues
   {
      get { return new List<string> { "DisplayValue1", "DisplayValue2", "DisplayValue3" }; }
   }

   public override string ToString()
   {
      return FirstName + LastName;
   }

   public static class AllRules
   {
      public static IBusinessRule FirstNameNotEmpty
      {
         get
         {
            return CreateRule.For<IAnInterface>()
               .Property(p => p.FirstName)
               .WithRule((p, value) => !value.IsNullOrEmpty())
               .WithError("FirstName is required");
         }
      }

      public static IBusinessRule FirstNameLenghtSmallerThan15
      {
         get
         {
            return CreateRule.For<IAnInterface>()
               .Property(p => p.FirstName)
               .WithRule((p, value) => value.IsNullOrEmpty() || value.Length <= 15)
               .WithError("FirstName length should be smaller than 15");
         }
      }

      public static IBusinessRuleSet Default
      {
         get { return new BusinessRuleSet(FirstNameNotEmpty, FirstNameLenghtSmallerThan15); }
      }
   }

   public IBusinessRuleSet Rules
   {
      get { return AllRules.Default; }
   }
}