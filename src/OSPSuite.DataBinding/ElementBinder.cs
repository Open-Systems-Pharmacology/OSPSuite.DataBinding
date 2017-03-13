using System;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;
using OSPSuite.DataBinding.Core;

namespace OSPSuite.DataBinding
{
   public abstract class ElementBinder<TObjectType, TPropertyType> : IElementBinder<TObjectType, TPropertyType>
   {
      protected readonly IElementBinderValidator<TObjectType, TPropertyType> _elementBinderValidator;
      private readonly IPropertyBinderNotifier<TObjectType, TPropertyType> _propertyBinder;

      //The last value into the control
      protected TPropertyType LastValue { get; private set; }

      //the orginal value into the source
      private TPropertyType _originalValue;

      // Reference to source object
      public TObjectType Source { get; private set; }

      public event Action Changed = delegate { };
      public event Action Changing = delegate { };

      public event Action<TObjectType, PropertyValueSetEventArgs<TPropertyType>> OnValueSet = delegate { };

      public ScreenBinder<TObjectType> ParentBinder { get; set; }

      public IFormatterConfigurator<TObjectType, TPropertyType> FormatterConfigurator { get; set; }

      public bool IsLatched { get; set; }

      protected ElementBinder(IPropertyBinderNotifier<TObjectType, TPropertyType> propertyBinder)
         : this(propertyBinder, new ElementBinderValidator<TObjectType, TPropertyType>(), new FormatterConfigurator<TObjectType, TPropertyType>())
      {
      }

      protected ElementBinder(IPropertyBinderNotifier<TObjectType, TPropertyType> propertyBinder,
                              IElementBinderValidator<TObjectType, TPropertyType> elementBinderValidator,
                              IFormatterConfigurator<TObjectType, TPropertyType> formatterConfigurator)
      {
         _propertyBinder = propertyBinder;
         _elementBinderValidator = elementBinderValidator;
         FormatterConfigurator = formatterConfigurator;
      }

      protected virtual IFormatter<TPropertyType> Formatter => FormatterConfigurator.FormatterFor(Source);

      public string PropertyName => _propertyBinder.PropertyName;

      public virtual void Bind(TObjectType source)
      {
         DeleteBinding();
         UpdateSource(source);

         //save original value
         _originalValue = GetValueFromSource();

         this.DoWithinLatch(() => updateControl(_originalValue));

         //the function update to any change of the source
         _propertyBinder.AddValueChangedListener(source, Update);
      }

      protected virtual void UpdateSource(TObjectType source)
      {
         Source = source;
      }

      public virtual void Validate()
      {
         this.DoWithinLatch(validateElement);
      }

      private void validateElement()
      {
         _elementBinderValidator.ValidateElementToScreen(this);
      }

      public virtual bool HasError => _elementBinderValidator.ElementHasError(this);

      public virtual string ErrorMessage
      {
         get
         {
            var notification = _elementBinderValidator.GetValidationNotification(this);
            return notification.HasError() ? notification.ErrorNotification : string.Empty;
         }
      }

      private void applyChangeFromControlToSource()
      {
         var newValue = GetValueFromControl();
         var oldValue = GetValueFromSource();

         //before setting the value to the source, raise the on OnValueSet event 
         //to allow caller to take over the actual action of setting the value
         OnValueSet(Source, new PropertyValueSetEventArgs<TPropertyType>(PropertyName, oldValue, newValue));

         if (ParentBinder.BindingMode == BindingMode.TwoWay)
         {
            //set control value into source
            SetValueToSource(newValue);
         }
      }

      public virtual void Reset()
      {
         this.DoWithinLatch
            (
               () =>
                  {
                     updateControl(_originalValue);
                     SetValueToSource(_originalValue);
                  }
            );
      }

      public virtual void Update()
      {
         ////Update control run within latch so that the function 
         ////is only called when indeed triggered from the element itself
         this.DoWithinLatch
            (
               () =>
                  {
                     //Update control with the current value of the source
                     updateControl(GetValueFromSource());
                     NotifyChange();
                  }
            );
      }

      private void updateControl(TPropertyType valueToSetInControl)
      {
         LastValue = valueToSetInControl;

         //First set value into control
         SetValueToControl(valueToSetInControl);

         //Once value was set, validate value set into control
         validateElement();
      }

      public virtual TPropertyType GetValueFromSource()
      {
         return _propertyBinder.GetValue(Source);
      }

      public virtual void SetValueToSource(TPropertyType value)
      {
         _propertyBinder.SetValue(Source, value);
      }

      //these properties need to be implemented by each derived control
      public abstract Control Control { get; }
      public abstract TPropertyType GetValueFromControl();
      public abstract void SetValueToControl(TPropertyType value);

      protected virtual bool HasChanged()
      {
         var propertyValue = GetValueFromControl();
         return !Equals(propertyValue, LastValue);
      }

      protected void ValueInControlChanging()
      {
         this.DoWithinLatch(() =>
            {
               validateElement();
               Changing();
            });
      }

      protected void ValueInControlChanged()
      {
         //Our strategy is validation before change
         if (onlySaveWhenValid()) return;

         //the value did not change (same value was entered again. do not notify
         if (!HasChanged()) return;

         this.DoWithinLatch(applyChangeFromControlToSource);

         LastValue = GetValueFromControl();

         if (IsLatched) return;

         NotifyChange();
      }

      private bool onlySaveWhenValid()
      {
         return ParentBinder.SavingMode == SavingMode.OnlyWhenValid && HasError;
      }

      protected void NotifyChange()
      {
         Changed();
      }

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      public virtual void DeleteBinding()
      {
         _propertyBinder.RemoveValueChangedListener(Source);
         Source = default(TObjectType);
         LastValue = default(TPropertyType);
         _originalValue = default(TPropertyType);
      }

      protected virtual void Cleanup()
      {
         DeleteBinding();
         Changed = delegate { };
         OnValueSet = delegate { };
      }

      ~ElementBinder()
      {
         Cleanup();
      }
   }
}