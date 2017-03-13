using System;
using System.Windows.Forms;
using OSPSuite.DataBinding.Core;

namespace OSPSuite.DataBinding
{
   public interface IControlEnableBinder<TObject, TPropertyType> : IElementBinder<TObject, TPropertyType>
   {
      /// <summary>
      /// Specifies the conversion from property type to bool for enable
      /// </summary>
      /// <param name="func">The conversion func</param>
      IControlEnableBinder<TObject, TPropertyType> EnabledWhen(Func<TPropertyType, bool> func);
   }

   public abstract class ConversionElementBinder<TObjectType, TPropertyType> : ElementBinder<TObjectType, TPropertyType>
   {
      protected readonly Control _control;
      protected ConversionElementBinder(IPropertyBinderNotifier<TObjectType, TPropertyType> propertyBinder, Control control)
         : base(propertyBinder)
      {
         _control = control;
         _control.EnabledChanged += (o, e) => ValueInControlChanged();
      }

      public override void SetValueToSource(TPropertyType value)
      {
         //make sure that the control changes do not reflect back to the source   
      }

      public override TPropertyType GetValueFromControl()
      {
         // This binding is intended to be used only one way.
         return default(TPropertyType);
      }

      public override Control Control
      {
         get { return _control; }
      }
   }

   public class ControlEnableBinder<TObjectType, TPropertyType> : ConversionElementBinder<TObjectType, TPropertyType>, IControlEnableBinder<TObjectType, TPropertyType>
   {
      private Func<TPropertyType, bool> _conversion = property => property.Equals(true);

      public ControlEnableBinder(IPropertyBinderNotifier<TObjectType, TPropertyType> propertyBinder, Control control)
         : base(propertyBinder, control)
      {
      }

      public override void SetValueToControl(TPropertyType value)
      {
         _control.Enabled = _conversion(value);
      }

      public IControlEnableBinder<TObjectType, TPropertyType> EnabledWhen(Func<TPropertyType, bool> func)
      {
         _conversion = func;
         return this;
      }
   }
}
