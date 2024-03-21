using System;
using System.Windows.Forms;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;

namespace OSPSuite.DataBinding.Tests
{
   public abstract class concern_for_ElementBinderValidator : ContextSpecification<IElementBinderValidator<IAnInterface, string>>
   {
      protected IElementBinder<IAnInterface, string> _elementToValidate;
      protected IValidationEngine _validationEngine;
      protected IAnInterface _source;
      private ScreenBinder<IAnInterface> _screenBinder;

      protected override void Context()
      {
         _source = A.Fake<IAnInterface>();
         _screenBinder = A.Fake<ScreenBinder<IAnInterface>>();

         _elementToValidate = A.Fake<IElementBinder<IAnInterface, string>>();
         A.CallTo(() => _elementToValidate.Source).Returns(_source);
         A.CallTo(() => _elementToValidate.PropertyName).Returns("FirstName");
         A.CallTo(() => _elementToValidate.GetValueFromControl()).Returns("Toto");
         A.CallTo(() => _elementToValidate.ParentBinder).Returns(_screenBinder);
         A.CallTo(() => _elementToValidate.Control).Returns(new Label());

         _validationEngine = A.Fake<IValidationEngine>();
         sut = new ElementBinderValidator<IAnInterface, string>(_validationEngine);
      }
   }

   public class When_told_to_validate_a_valid_element_binder : concern_for_ElementBinderValidator
   {
      protected override void Context()
      {
         base.Context();
         var notification = A.Fake<INotification>();
         A.CallTo(() => notification.HasError()).Returns(false);
         A.CallTo(() => _validationEngine.Validate(_elementToValidate.Source, _elementToValidate.PropertyName, _elementToValidate.GetValueFromControl())).Returns(notification);
      }

      protected override void Because()
      {
         sut.ValidateElementToScreen(_elementToValidate);
      }

      [Observation]
      public void should_use_the_validation_engine_to_validate_the_element()
      {
         A.CallTo(() => _validationEngine.Validate(_elementToValidate.Source, _elementToValidate.PropertyName, _elementToValidate.GetValueFromControl())).MustHaveHappened();
      }
   }

   public class When_asked_if_an_element_that_contains_error_has_an_error : concern_for_ElementBinderValidator
   {
      private bool _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _validationEngine.Validate(_elementToValidate.Source, _elementToValidate.PropertyName, _elementToValidate.GetValueFromControl())).Returns(new Notification("error"));
      }

      protected override void Because()
      {
         _result = sut.ElementHasError(_elementToValidate);
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_asked_if_a_valid_element_has_an_error : concern_for_ElementBinderValidator
   {
      private bool _result;

      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }

      protected override void Because()
      {
         _result = sut.ElementHasError(_elementToValidate);
      }

      protected override void Context()
      {
         base.Context();
         var notification = A.Fake<INotification>();
         A.CallTo(() => notification.HasError()).Returns(false);
         A.CallTo(() => _validationEngine.Validate(_elementToValidate.Source, _elementToValidate.PropertyName, _elementToValidate.GetValueFromControl())).Returns(notification);
      }
   }

   public class When_an_exception_is_thrown_while_validating_an_element : concern_for_ElementBinderValidator
   {
      private INotification _result;
      private string _errorMessage;

      [Observation]
      public void should_return_a_notification_with_a_message_set_to_the_exception_message()
      {
         _result.ErrorNotification.ShouldBeEqualTo(_errorMessage);
      }

      protected override void Because()
      {
         _result = sut.GetValidationNotification(_elementToValidate);
      }

      protected override void Context()
      {
         base.Context();
         _errorMessage = "toto tata titi";
         A.CallTo(() => _validationEngine.Validate(_elementToValidate.Source, _elementToValidate.PropertyName, _elementToValidate.GetValueFromControl())).Throws(new ArgumentException(_errorMessage));
      }
   }
}