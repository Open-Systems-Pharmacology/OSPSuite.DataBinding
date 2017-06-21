using System.Windows.Forms;
using OSPSuite.DataBinding.Core;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.DataBinding.Controls
{
   public abstract class TextBoxBinderBase<TObject, TPropertyType, TControlType> : ElementBinder<TObject, TPropertyType> where TControlType : Control
   {
      protected readonly TControlType _textBox;

      protected TextBoxBinderBase(IPropertyBinderNotifier<TObject, TPropertyType> propertyBinder, TControlType textBox) : base(propertyBinder)
      {
         _textBox = textBox;
      }

      public override Control Control => _textBox;

      public override TPropertyType GetValueFromControl()
      {
         return _textBox.Text.ConvertedTo<TPropertyType>();
      }

      public override void SetValueToControl(TPropertyType value)
      {
         if (!typeof(TPropertyType).IsValueType && Equals(value, default(TPropertyType)))
         {
            _textBox.Text = string.Empty;
            return;
         }
         _textBox.Text = Formatter.Format(value);
      }
   }

   public class TextBoxBinder<TObject, TPropertyType> : TextBoxBinderBase<TObject, TPropertyType, TextBox>
   {
      public TextBoxBinder(IPropertyBinderNotifier<TObject, TPropertyType> propertyBinder, TextBox textBox) : base(propertyBinder, textBox)
      {
         _textBox.Validating += (o, e) => ValueInControlChanged();
         _textBox.TextChanged += (o, e) => ValueInControlChanging();
      }
   }
}