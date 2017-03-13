using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.DataBinding.Tests
{
   public abstract class concern_for_ListElementBinder : ContextSpecification<IListElementBinder<IAnInterface, string>>
   {
      protected ScreenBinder<IAnInterface> _binder;
      protected ComboBox _comboBox;
      protected IAnInterface _source;

      protected override void Context()
      {
         _binder = new ScreenBinder<IAnInterface>();
         _comboBox = new ComboBox();
         sut = _binder.Bind(item => item.ValueFromList).To(_comboBox);
         _source = new AnImplementation {ValueFromList = "value1"};
      }

      public override void Cleanup()
      {
         sut.Dispose();
      }
   }

   public class When_told_to_bind_to_a_source_and_display_values_are_not_defined : concern_for_ListElementBinder
   {
      protected override void Context()
      {
         base.Context();
         sut.WithValues(item => item.ListOfValues);
         sut.Bind(_source);
      }

      [Observation]
      public void Should_tell_the_underlying_control_to_fill_up_with_the_list_of_values()
      {
         _source.ListOfValues.Count().ShouldBeEqualTo(_comboBox.Items.Count);
         foreach (var value in _source.ListOfValues)
         {
            _comboBox.Items.Contains(value).ShouldBeTrue();
         }
      }

      [Observation]
      public void Should_set_the_selected_item_to_the_value_of_the_bound_property()
      {
         _comboBox.SelectedItem.ShouldBeEqualTo(_source.ValueFromList);
      }
   }

   public class When_told_to_bind_to_a_source_with_display_values_item_not_equal_to_the_number_of_items : concern_for_ListElementBinder
   {
      private IList<string> _listOfValues;
      private List<string> _listOfDisplayValues;

      protected override void Context()
      {
         base.Context();
         _listOfValues = new List<string> {"aa", "bb", "bb"};
         _listOfDisplayValues = new List<string> {"aa", "bb"};
         sut.WithValues(_listOfValues).AndDisplays(_listOfDisplayValues);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Bind(_source)).ShouldThrowAn<ArgumentException>();
      }
   }

   public class When_the_item_list_is_not_defined : concern_for_ListElementBinder
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Bind(_source)).ShouldThrowAn<ArgumentException>();
      }
   }

   public class When_the_item_list_is_defined_but_does_not_represents_a_valid_set_of_values : concern_for_ListElementBinder
   {
      protected override void Context()
      {
         base.Context();
         sut.WithValues(x => null);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Bind(_source)).ShouldThrowAn<ArgumentException>();
      }
   }

   public class When_told_to_bind_to_a_source_with_display_values : concern_for_ListElementBinder
   {
      protected override void Context()
      {
         base.Context();
         var listOfValues = new List<string> {"value1", "value2", "value3"};
         var listOfDisplayValues = new List<string> {"aa", "bb", "cc"};
         sut.WithValues(listOfValues).AndDisplays(listOfDisplayValues);
      }

      protected override void Because()
      {
         sut.Bind(_source);
      }

      [Observation]
      public void Should_set_the_selected_item_to_the_value_of_the_bound_property()
      {
         _comboBox.SelectedItem.ShouldBeEqualTo("aa");
      }
   }

   public class When_told_to_bind_to_a_source_with_display_values_defined_as_a_function_of_the_actual_values : concern_for_ListElementBinder
   {
      protected override void Context()
      {
         base.Context();
         var listOfValues = new List<string> {"value1", "value2", "value3"};
         sut.WithValues(listOfValues).AndDisplays(getDisplayValue);
      }

      private string getDisplayValue(string input)
      {
         if (input == "value1")
            return "aa";

         if (input == "value2")
            return "bb";

         return "cc";
      }

      protected override void Because()
      {
         sut.Bind(_source);
      }

      [Observation]
      public void Should_set_the_selected_item_to_the_value_of_the_bound_property()
      {
         _comboBox.SelectedItem.ShouldBeEqualTo("aa");
      }
   }

   public class When_changing_the_selected_value : concern_for_ListElementBinder
   {
      protected override void Context()
      {
         base.Context();
         sut.WithValues("value1", "value2", "value3").AndDisplays("aa", "bb", "cc");
         sut.Bind(_source);
      }

      protected override void Because()
      {
         _comboBox.SelectedItem = "cc";
      }

      [Observation]
      public void Should_update_the_value_of_the_bound_property_in_the_source()
      {
         _source.ValueFromList.ShouldBeEqualTo("value3");
      }
   }

   public class When_refreshing_the_combo_box_with_a_new_list : concern_for_ListElementBinder
   {
      private readonly ComboBox _comboBox2 = new ComboBox();

      protected override void Context()
      {
         base.Context();
         sut.WithValues("value1", "value2", "value3").AndDisplays("aa", "bb", "cc");
         sut.Changed += _binder.RefreshListElements;
         _binder.Bind(item => item.FirstName).To(_comboBox2).WithValues(getComboBoxValue);

         _source.FirstName = "Joe";
         _source.ValueFromList = "value1";

         _binder.BindToSource(_source);
      }

      private IEnumerable<string> getComboBoxValue(IAnInterface item)
      {
         if (item.ValueFromList.Equals("value1"))
            return new[] {"Joe", "Juri", "Mike"};

         return new[] {"Maman", "Papa", "Oma"};
      }

      protected override void Because()
      {
         _comboBox.SelectedItem = "bb";
      }

      [Observation]
      public void should_update_the_second_combo_box_with_the_accurate_list()
      {
         _comboBox2.Items.Contains("Maman").ShouldBeTrue();
         _comboBox2.Items.Contains("Papa").ShouldBeTrue();
         _comboBox2.Items.Contains("Oma").ShouldBeTrue();
         _comboBox2.Items.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_notify_that_the_bound_value_for_the_first_name_is_not_valid_anymore()
      {
         _binder.HasError.ShouldBeTrue();
      }
   }

   public class When_binding_a_property_for_which_the_value_has_not_been_set_to_a_valid_element : concern_for_ListElementBinder
   {
      protected override void Context()
      {
         base.Context();
         _source.ValueFromList = string.Empty;
         sut.WithValues("value1", "value2", "value3").AndDisplays("aa", "bb", "cc");
      }

      protected override void Because()
      {
         _binder.BindToSource(_source);
      }

      [Observation]
      public void should_set_the_value_of_the_property_to_the_default_of_the_value_type()
      {
         _source.ValueFromList.ShouldBeEqualTo(string.Empty);
      }
   }
}