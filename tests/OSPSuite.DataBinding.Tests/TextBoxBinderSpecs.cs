using System;
using System.ComponentModel;
using System.Globalization;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.DataBinding.Tests
{
   public abstract class concern_for_ElementBinder<TObject> : ContextSpecification<IElementBinder<IAnInterface, TObject>>
   {
      protected ScreenBinder<IAnInterface> _binder;
      protected AnImplementation _source;
      protected MyTextBox _textBox;

      protected override void Context()
      {
         _binder = new ScreenBinder<IAnInterface>();
         _textBox = new MyTextBox();
         _source = new AnImplementation();
         _source.FirstName = "A valid first name";
      }

      public override void Cleanup()
      {
         sut.Dispose();
      }
   }

   public abstract class concern_for_element_binder_binding_to_a_nullable_double : concern_for_ElementBinder<double?>
   {
      protected override void Context()
      {
         base.Context();
         sut = _binder.Bind(item => item.NullableValue).To(_textBox);
         sut.Bind(_source);
      }
   }

   public abstract class concern_for_element_binder_binding_to_string : concern_for_ElementBinder<string>
   {
      protected override void Context()
      {
         base.Context();
         sut = _binder.Bind(item => item.FirstName).To(_textBox);
         sut.Bind(_source);
      }
   }

   public abstract class concern_for_element_binder_binding_to_double : concern_for_ElementBinder<double>
   {
      protected override void Context()
      {
         base.Context();
         sut = _binder.Bind(item => item.Value).To(_textBox);
      }
   }

   public class When_the_text_of_a_bound_text_box_was_set_in_binding_mode_two_ways_with_a_valid_value : concern_for_element_binder_binding_to_string
   {
      private bool _eventOnValidatedWasCalled;
      private string _newValue;
      private string _oldValue;
      private string _notifiedOldValue;
      private string _notifiedNewValue;
      private bool _eventOnValueSet;
      private string _nameOfProperty;

      protected override void Context()
      {
         base.Context();
         _binder.OnValidated += c => { _eventOnValidatedWasCalled = true; };
         sut.OnValueSet += (o, v) =>
         {
            _eventOnValueSet = true;
            _nameOfProperty = v.PropertyName;
            _notifiedOldValue = v.OldValue;
            _notifiedNewValue = v.NewValue;
         };

         _newValue = "toto";
         _oldValue = _source.FirstName;
      }

      protected override void Because()
      {
         _textBox.Text = _newValue;
      }

      [Observation]
      public void should_notify_the_screen_binder_that_the_value_has_changed()
      {
         _eventOnValidatedWasCalled.ShouldBeTrue();
      }

      [Observation]
      public void should_have_updated_the_value_of_the_property_that_the_textbox_is_bound_to()
      {
         _source.FirstName.ShouldBeEqualTo(_newValue);
      }

      [Observation]
      public void should_have_notify_the_value_set_event_to_listener()
      {
         _eventOnValueSet.ShouldBeTrue();
      }

      [Observation]
      public void should_notify_the_name_of_the_property_that_was_set()
      {
         _nameOfProperty.ShouldBeEqualTo("FirstName");
      }

      [Observation]
      public void should_notify_the_old_value_of_the_property()
      {
         _notifiedOldValue.ShouldBeEqualTo(_oldValue);
      }

      [Observation]
      public void should_notify_the_new_value_of_the_property()
      {
         _notifiedNewValue.ShouldBeEqualTo(_newValue);
      }
   }

   public class When_the_text_of_a_bound_text_box_was_set_in_binding_mode_one_ways_with_a_valid_value : concern_for_element_binder_binding_to_string
   {
      private bool _eventOnValidatedWasCalled;
      private string inputText;
      private string _originalValue;
      private bool _eventOnValueSet;

      protected override void Context()
      {
         base.Context();
         _binder.OnValidated += c => { _eventOnValidatedWasCalled = true; };
         _binder.BindingMode = BindingMode.OneWay;
         sut.OnValueSet += (o, v) => { _eventOnValueSet = true; };
         _originalValue = _source.FirstName;
         inputText = "toto";
      }

      protected override void Because()
      {
         _textBox.Text = inputText;
      }

      [Observation]
      public void should_notify_the_screen_binder_that_the_value_has_changed()
      {
         _eventOnValidatedWasCalled.ShouldBeTrue();
      }

      [Observation]
      public void should_not_have_updated_the_value_of_the_property_that_the_textbox_is_bound_to()
      {
         _source.FirstName.ShouldBeEqualTo(_originalValue);
      }

      [Observation]
      public void should_have_notify_the_value_set_event_to_listener()
      {
         _eventOnValueSet.ShouldBeTrue();
      }
   }

   public class When_binding_to_a_nullable_value_that_is_null : concern_for_element_binder_binding_to_a_nullable_double
   {
      protected override void Context()
      {
         base.Context();
         _source.NullableValue = null;
      }

      [Observation]
      public void the_text_in_the_bound_target_should_be_empty()
      {
         _textBox.Text.IsNullOrEmpty().ShouldBeTrue();
      }
   }

   public class When_setting_a_empty_string_into_the_text_box_when_bound_to_a_nullable_type : concern_for_element_binder_binding_to_a_nullable_double
   {
      protected override void Context()
      {
         base.Context();
         _source.NullableValue = 3;
      }

      protected override void Because()
      {
         _textBox.Text = string.Empty;
      }

      [Observation]
      public void should_set_the_bound_value_to_null()
      {
         _source.NullableValue.ShouldNotBeNull();
      }
   }

   public class when_binding_to_a_property_with_a_predefined_format : concern_for_element_binder_binding_to_double
   {
      private double _originalValue;

      protected override void Context()
      {
         base.Context();
         sut.WithFormat(d => d.ToString("0.00"));
         _source.Value = 2.12345;
         _originalValue = _source.Value;
         sut.Bind(_source);
      }

      [Observation]
      public void should_display_the_formated_property()
      {
         var decimalSep = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
         _textBox.Text.ShouldBeEqualTo(string.Format("2{0}12", decimalSep));
      }

      [Observation]
      public void should_not_have_updated_the_value_of_the_property_that_the_textbox_is_bound_to()
      {
         _source.Value.ShouldBeEqualTo(_originalValue);
      }
   }

   public class when_updating_the_text_of_a_bound_text_box_in_binding_mode_two_ways_with_an_invalid_value : concern_for_element_binder_binding_to_string
   {
      private bool _eventOnChangedWasCalled;
      private bool _eventOnErrorValidatingWasCalled;
      private string inputText;
      private string originalValue;
      private bool _eventOnValueSet;

      protected override void Context()
      {
         base.Context();
         _binder.Changed += () => { _eventOnChangedWasCalled = true; };
         _binder.OnValidationError += (c, e) => { _eventOnErrorValidatingWasCalled = true; };
         sut.OnValueSet += (o, v) => { _eventOnValueSet = true; };
         originalValue = _source.FirstName;
         inputText = string.Empty;
      }

      protected override void Because()
      {
         _textBox.Text = inputText;
      }

      [Observation]
      public void should_not_update_the_value_of_the_property_that_the_textbox_is_bound_to()
      {
         _source.FirstName.ShouldBeEqualTo(originalValue);
      }

      [Observation]
      public void should_not_notify_the_screen_binder_that_the_value_has_changed()
      {
         _eventOnChangedWasCalled.ShouldBeFalse();
      }

      [Observation]
      public void should_have_raised_the_on_error_event_for_the_screen_element()
      {
         _eventOnErrorValidatingWasCalled.ShouldBeTrue();
      }

      [Observation]
      public void should_not_have_notify_the_value_set_event_to_listener()
      {
         _eventOnValueSet.ShouldBeFalse();
      }
   }

   public class When_the_value_of_a_bound_property_was_changed_at_run_time : concern_for_element_binder_binding_to_string
   {
      private bool _eventOnChangedWasCalled;

      protected override void Because()
      {
         var prop = TypeDescriptor.GetProperties(_source.GetType())["FirstName"];
         prop.SetValue(_source, "tralala");
      }

      protected override void Context()
      {
         base.Context();
         _binder.Changed += () => { _eventOnChangedWasCalled = true; };
      }

      [Observation]
      public void should_update_the_value_into_the_control()
      {
         _textBox.Text.ShouldBeEqualTo(_source.FirstName);
      }

      [Observation]
      public void should_notify_the_screen_binder_that_the_value_has_changed()
      {
         _eventOnChangedWasCalled.ShouldBeTrue();
      }
   }

   public class When_disposing_an_element_binder_bound_to_an_element_implementing_none_of_the_notifying_interfaces : concern_for_element_binder_binding_to_double
   {
      private WeakReference _references;

      protected override void Context()
      {
         base.Context();
         _references = new WeakReference(_source);
         sut.Bind(_source);
      }

      protected override void Because()
      {
         sut.Dispose();
         _source = null;
         sut = null;
         GC.Collect();
      }

      [Observation]
      public void should_not_hold_the_objec_in_memory()
      {
         _references.IsAlive.ShouldBeFalse();
      }

      public override void Cleanup()
      {
      }
   }

   public class When_binding_to_another_element : concern_for_element_binder_binding_to_double
   {
      private WeakReference _references;
      private IAnInterface _anotherElement;

      protected override void Context()
      {
         base.Context();
         _references = new WeakReference(_source);
         sut.Bind(_source);

         _anotherElement = new AnImplementation();
      }

      protected override void Because()
      {
         sut.Bind(_anotherElement);
         _source = null;
         GC.Collect();
      }

      [Observation]
      public void should_not_hold_the_first_element_in_memory()
      {
         _references.IsAlive.ShouldBeFalse();
      }
   }

   public class When_deleting_the_binding_to_an_element : concern_for_element_binder_binding_to_double
   {
      private WeakReference _references;

      protected override void Context()
      {
         base.Context();
         _references = new WeakReference(_source);
         sut.Bind(_source);
      }

      protected override void Because()
      {
         sut.DeleteBinding();
         _source = null;
         GC.Collect();
      }

      [Observation]
      public void should_not_hold_the_element_in_memory()
      {
         _references.IsAlive.ShouldBeFalse();
      }
   }
}