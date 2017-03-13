using System.Reflection;
using OSPSuite.DataBinding.Core;

namespace OSPSuite.DataBinding
{
   public interface IScreenToElementBinder<TObject, TPropertyType>
   {
      IPropertyBinderNotifier<TObject, TPropertyType> PropertyBinder { get; }
      ScreenBinder<TObject> ScreenBinder { get; }
   }

   public class ScreenToElementBinder<TObject, TPropertyType> : IScreenToElementBinder<TObject, TPropertyType>
   {
      public IPropertyBinderNotifier<TObject, TPropertyType> PropertyBinder { get; }
      public ScreenBinder<TObject> ScreenBinder { get; }

      public ScreenToElementBinder(ScreenBinder<TObject> screenBinder, PropertyInfo propertyInfo)
      {
         ScreenBinder = screenBinder;
         PropertyBinder = new PropertyBinderNotifier<TObject, TPropertyType>(propertyInfo);
      }
   }
}