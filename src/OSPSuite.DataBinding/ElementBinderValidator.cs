using System;
using OSPSuite.Utility.Validation;

namespace OSPSuite.DataBinding
{
    public interface IElementBinderValidator<TOBject, TPropertyType> 
    {
        /// <summary>
        /// Validates the given element binder and update the error status on the parent screen binder
        /// </summary>
        /// <param name="elementToValidate">screen element to validate</param>
        void ValidateElementToScreen(IElementBinder<TOBject, TPropertyType> elementToValidate);


        /// <summary>
        /// Returns true if the element binder has an error otherwise false
        /// </summary>
        /// <param name="elementToValidate">screen element to check for error</param>
        /// <returns>true if the element binder has an error otherwise false</returns>
        bool ElementHasError(IElementBinder<TOBject, TPropertyType> elementToValidate);

        INotification GetValidationNotification(IElementBinder<TOBject, TPropertyType> elementToValidate);
    }

    public class ElementBinderValidator<TObject, TPropertyType> : IElementBinderValidator<TObject, TPropertyType>
    {
        private readonly IValidationEngine _validationEngine;

        public ElementBinderValidator() : this(new ValidationEngine())
        {
        }

        public ElementBinderValidator(IValidationEngine validationEngine)
        {
            _validationEngine = validationEngine;
        }

        public void ValidateElementToScreen(IElementBinder<TObject, TPropertyType> elementToValidate)
        {
            var screenBinder = elementToValidate.ParentBinder;
            var notification = GetValidationNotification(elementToValidate);

            if (notification.HasError())
            {
                screenBinder.SetError(elementToValidate, notification.ErrorNotification);
            }
            else
            {
                screenBinder.ClearError(elementToValidate);
            }
        }

        public bool ElementHasError(IElementBinder<TObject, TPropertyType> elementToValidate)
        {
            return GetValidationNotification(elementToValidate).HasError();
        }

        public INotification GetValidationNotification(IElementBinder<TObject, TPropertyType> elementToValidate)
        {
            try
            {
                return _validationEngine.Validate(elementToValidate.Source, elementToValidate.PropertyName, elementToValidate.GetValueFromControl());
            }
            catch (Exception e)
            {
                return new Notification(e.Message);
            }
        }
    }
}