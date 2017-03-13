using System.ComponentModel;
using System.Windows.Forms;

namespace OSPSuite.DataBinding.Tests
{
   public class MyTextBox : TextBox
   {
      public override string Text
      {
         get { return base.Text; }
         set
         {
            base.Text = value;
            validate();
         }
      }

      private void validate()
      {
         OnValidating(new CancelEventArgs());
      }
   }
}