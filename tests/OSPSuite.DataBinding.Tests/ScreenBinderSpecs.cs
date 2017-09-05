using System;
using System.Collections.Generic;
using System.ComponentModel;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace OSPSuite.DataBinding.Tests
{
   public abstract class ScreenBinderSpecs : ContextSpecification<ScreenBinder<IAnInterface>>
   {
      protected IAnInterface _source;
      protected IElementBinder<IAnInterface> _element1;
      protected IElementBinder<IAnInterface> _element2;
      protected IList<IElementBinder<IAnInterface>> _elementList;
      protected IValidationEngine _validationEngine;

      protected override void Context()
      {
         _source = new AnImplementation();
         _elementList = new List<IElementBinder<IAnInterface>>();
         _element1 = A.Fake<IElementBinder<IAnInterface>>();
         _element2 = A.Fake<IElementBinder<IAnInterface>>();
         _validationEngine = A.Fake<IValidationEngine>();
         sut = new ScreenBinder<IAnInterface>(_elementList);
         sut.AddElement(_element1);
         sut.AddElement(_element2);
      }
   }

   public class When_adding_screen_elements_to_the_binder : ScreenBinderSpecs
   {
      private int _onChangedFired;

      [Observation]
      public void should_set_itself_as_parent_property_into_each_screen_elements_added()
      {
         _element1.ParentBinder.ShouldBeEqualTo(sut);
         _element2.ParentBinder.ShouldBeEqualTo(sut);
      }

      [Observation]
      public void should_contains_the_list_of_element_added()
      {
         _elementList.ShouldOnlyContain(_element1, _element2);
      }

      [Test]
      public void should_register_to_the_on_changed_event()
      {
         _onChangedFired.ShouldBeEqualTo(2);
      }

      protected override void Because()
      {
         _onChangedFired = 0;
         sut.Changed += () => _onChangedFired++;
         _element1.Changed += Raise.FreeForm.With();
         _element2.Changed += Raise.FreeForm.With();
      }
   }

   public class When_binding_to_a_source : ScreenBinderSpecs
   {
      [Observation]
      public void should_iterate_through_all_screen_elements_and_trigger_the_binding_for_that_source()
      {
         A.CallTo(() => _element1.Bind(_source)).MustHaveHappened();
         A.CallTo(() => _element2.Bind(_source)).MustHaveHappened();
      }

      protected override void Because()
      {
         sut.BindToSource(_source);
      }
   }

   public class When_reseting : ScreenBinderSpecs
   {
      [Observation]
      public void should_iterate_through_all_screen_elements_and_reset_them()
      {
         A.CallTo(() => _element1.Reset()).MustHaveHappened();
         A.CallTo(() => _element2.Reset()).MustHaveHappened();
      }

      protected override void Because()
      {
         sut.Reset();
      }
   }

   public class When_updating : ScreenBinderSpecs
   {
      [Observation]
      public void should_iterate_through_all_screen_elements_and_reset_them()
      {
         A.CallTo(() => _element1.Update()).MustHaveHappened();
         A.CallTo(() => _element2.Update()).MustHaveHappened();
      }

      protected override void Because()
      {
         sut.Update();
      }
   }

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

   public interface IAnInterfacePropertyChanged : INotifyPropertyChanged
   {
      string FirstName { get; set; }
      string LastName { get; set; }
   }

   public class AnImplementationPropertyChanged : IAnInterfacePropertyChanged
   {
      public event PropertyChangedEventHandler PropertyChanged = delegate { };
      private string _firstName;

      public string FirstName
      {
         get { return _firstName; }
         set
         {
            _firstName = value;
            PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
         }
      }

      private string _lastName;

      public string LastName
      {
         get { return _lastName; }
         set
         {
            _lastName = value;
            PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
         }
      }
   }

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
         get { return new List<string> {"value1", "value2", "value3"}; }
      }

      public IEnumerable<string> ListOfDisplayValues
      {
         get { return new List<string> {"DisplayValue1", "DisplayValue2", "DisplayValue3"}; }
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
}