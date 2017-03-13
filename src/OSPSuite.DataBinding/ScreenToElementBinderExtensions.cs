using System.Windows.Forms;
using OSPSuite.DataBinding.Controls;

namespace OSPSuite.DataBinding
{
   public static class ScreenToElementBinderExtensions
   {

      public static IControlEnableBinder<TObject, TPropertyType> ToEnableOf<TObject, TPropertyType>(this IScreenToElementBinder<TObject, TPropertyType> screenToElementBinder, Control simpleButton)
      {
         var element = new ControlEnableBinder<TObject, TPropertyType>(screenToElementBinder.PropertyBinder, simpleButton);
         screenToElementBinder.ScreenBinder.AddElement(element);
         return element;
      }

      public static IElementBinder<TObject, TPropertyType> To<TObject, TPropertyType>(this IScreenToElementBinder<TObject, TPropertyType> screenToElementBinder, TextBox textbox)
      {
         var element = new TextBoxBinder<TObject, TPropertyType>(screenToElementBinder.PropertyBinder, textbox);
         screenToElementBinder.ScreenBinder.AddElement(element);
         return element;
      }

      public static IListElementBinder<TObject, TPropertyType> To<TObject, TPropertyType>(this IScreenToElementBinder<TObject, TPropertyType> screenToElementBinder, ComboBox comboBox)
      {
         var element = new ComboBoxElementBinder<TObject, TPropertyType>(screenToElementBinder.PropertyBinder, comboBox);
         screenToElementBinder.ScreenBinder.AddElement(element);
         return element;
      }

      public static IElementBinder<TObject, TPropertyType> To<TObject, TPropertyType>(this IScreenToElementBinder<TObject, TPropertyType> screenToElementBinder, Label label)
      {
         var element = new LabelBinder<TObject, TPropertyType>(screenToElementBinder.PropertyBinder, label);
         screenToElementBinder.ScreenBinder.AddElement(element);
         return element;
      }
   }
}