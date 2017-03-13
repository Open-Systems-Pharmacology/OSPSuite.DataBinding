using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;
using OSPSuite.DataBinding.Core;

namespace OSPSuite.DataBinding.Tests
{
   public class When_changing_the_value_of_a_bound_property : ContextSpecification<IPropertyBinderNotifier<IAnInterface, string>>
   {
      private IAnInterface _source;
      private const string _firstName = "Michael";

      protected override void Context()
      {
         _source = new AnImplementationWithEvents {FirstName = "toto"};
         var property = typeof(IAnInterface).GetProperty("FirstName");
         sut = new PropertyBinderNotifier<IAnInterface, string>(property);
      }

      protected override void Because()
      {
         sut.SetValue(_source, _firstName);
      }

      [Observation]
      public void should_update_the_value_of_the_property_in_the_source()
      {
         _source.FirstName.ShouldBeEqualTo(_firstName);
      }
   }

   public class When_changing_the_value_of_a_readonly_bound_property : ContextSpecification<IPropertyBinderNotifier<IAnInterface, string>>
   {
      private IAnInterface _source;
      private string _orignalValue;

      protected override void Context()
      {
         _source = new AnImplementationWithEvents {FirstName = "toto"};
         _orignalValue = _source.ReadOnlyProp;
         var property = typeof(IAnInterface).GetProperty("ReadOnlyProp");
         sut = new PropertyBinderNotifier<IAnInterface, string>(property);
      }

      protected override void Because()
      {
         sut.SetValue(_source, "tralalala");
      }

      [Observation]
      public void should_not_change_the_value_of_the_source()
      {
         _source.ReadOnlyProp.ShouldBeEqualTo(_orignalValue);
      }
   }

   public class When_a_valid_event_handler_was_added_to_a_value_changed_property : ContextSpecification<IPropertyBinderNotifier<IAnInterface, string>>
   {
      private IAnInterface _source;
      private IPropertyBinderNotifier<IAnInterface, string> _otherPropertyBinder;
      private bool _listenerWasNotified;

      [Observation]
      public void should_notify_the_listener_when_a_value_is_set_via_another_property_binder()
      {
         _listenerWasNotified.ShouldBeTrue();
      }

      protected override void Because()
      {
         _otherPropertyBinder.SetValue(_source, "new value");
      }

      protected override void Context()
      {
         _source = new AnImplementationWithEvents {FirstName = "toto"};
         var property = typeof(AnImplementationWithEvents).GetProperty("LastName");
         sut = new PropertyBinderNotifier<IAnInterface, string>(property);

         _otherPropertyBinder = new PropertyBinderNotifier<IAnInterface, string>(property);
         sut.AddValueChangedListener(_source, onValueChanged);
      }

      private void onValueChanged()
      {
         _listenerWasNotified = true;
      }
   }

   public class the_binding_source_implements_the_i_notify_property_changed_and_a_value_is_set_via_code : ContextSpecification<IPropertyBinderNotifier<IAnInterfacePropertyChanged, string>>
   {
      private IAnInterfacePropertyChanged _source;
      private bool _listenerWasNotified;

      [Observation]
      public void should_notify_the_listener()
      {
         _listenerWasNotified.ShouldBeTrue();
      }

      protected override void Because()
      {
         _source.FirstName = "blah";
      }

      protected override void Context()
      {
         _source = new AnImplementationPropertyChanged {FirstName = "toto"};
         var property = typeof(AnImplementationWithEvents).GetProperty("FirstName");
         sut = new PropertyBinderNotifier<IAnInterfacePropertyChanged, string>(property);
         sut.AddValueChangedListener(_source, onValueChanged);
      }

      private void onValueChanged()
      {
         _listenerWasNotified = true;
      }
   }

   public class the_binding_source_implements_the_i_notify_property_changed_and_a_property_that_is_not_monitored_is_set_via_code :
      ContextSpecification<IPropertyBinderNotifier<IAnInterfacePropertyChanged, string>>
   {
      private IAnInterfacePropertyChanged _source;
      private bool _listenerWasNotified;

      [Observation]
      public void should_not_notify_the_listener()
      {
         _listenerWasNotified.ShouldBeFalse();
      }

      protected override void Because()
      {
         _source.LastName = "blah";
      }

      protected override void Context()
      {
         _source = new AnImplementationPropertyChanged {FirstName = "toto"};
         var property = typeof(AnImplementationWithEvents).GetProperty("FirstName");
         sut = new PropertyBinderNotifier<IAnInterfacePropertyChanged, string>(property);
         sut.AddValueChangedListener(_source, onValueChanged);
      }

      private void onValueChanged()
      {
         _listenerWasNotified = true;
      }
   }

   public class When_the_binder_is_being_released : ContextSpecification<IPropertyBinderNotifier<IAnInterface, string>>
   {
      private IAnInterface _source;
      private WeakReference _wr;

      protected override void Context()
      {
         _source = new AnImplementationWithEvents {FirstName = "toto"};
         var property = typeof(IAnInterface).GetProperty("LastName");
         sut = new PropertyBinderNotifier<IAnInterface, string>(property);
         sut.AddValueChangedListener(_source, MyEventHandler);
         _wr = new WeakReference(_source);
      }

      protected override void Because()
      {
         sut.RemoveValueChangedListener(_source);
      }

      [Observation]
      public void should_not_hold_any_references_to_the_source()
      {
         _source = null;
         GC.Collect();
         _wr.IsAlive.ShouldBeFalse();
      }

      private void MyEventHandler()
      {
      }
   }

   public class When_removing_the_event_handler : ContextSpecification<IPropertyBinderNotifier<IAnInterface, string>>
   {
      private IAnInterface _source;
      private bool _eventWasCalled;

      protected override void Context()
      {
         _source = new AnImplementationWithEvents {FirstName = "toto"};
         var property = typeof(IAnInterface).GetProperty("LastName");
         sut = new PropertyBinderNotifier<IAnInterface, string>(property);
         sut.AddValueChangedListener(_source, MyEventHandler);
      }

      protected override void Because()
      {
         sut.RemoveValueChangedListener(_source);
      }

      [Observation]
      public void should_not_notify_any_event_message()
      {
         _source.LastName = "tutu";
         _eventWasCalled.ShouldBeFalse();
      }

      private void MyEventHandler()
      {
         _eventWasCalled = true;
      }
   }

   public class AnImplementationWithEvents : IAnInterface
   {
      private string _firstName;
      private string _lastName;

      public event EventHandler FirstNameChanged = delegate { };

      public string FirstName
      {
         get { return _firstName; }
         set
         {
            _firstName = value;
            FirstNameChanged(this, EventArgs.Empty);
         }
      }

      public event EventHandler LastNameChanged = delegate { };

      public string LastName
      {
         get { return _lastName; }
         set
         {
            _lastName = value;
            LastNameChanged(this, EventArgs.Empty);
         }
      }

      public double Value { get; set; }
      public double? NullableValue { get; set; }
      public bool Disable { get; set; }

      public IAnInterface Child { get; set; }
      public string ReadOnlyProp { get; private set; }
      public string ValueFromList { get; set; }
      public IEnumerable<string> ListOfValues { get; private set; }
      public IEnumerable<string> ListOfDisplayValues { get; private set; }

      public AnImplementationWithEvents()
      {
         ReadOnlyProp = "tutu";
      }

      public IBusinessRuleSet Rules
      {
         get { return new BusinessRuleSet(); }
      }
   }
}