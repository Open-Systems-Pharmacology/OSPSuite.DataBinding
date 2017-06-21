using System;
using System.Windows.Forms;
using OSPSuite.DataBinding.Tests;

namespace OSPSuite.DataBinding.Starter
{
   public partial class ScreenBinderSimple : UserControl
   {
      private IAnInterface _objectToBind;
      private readonly ScreenBinder<IAnInterface> _screenBinder;


      public ScreenBinderSimple()
      {
         InitializeComponent();

         _screenBinder = new ScreenBinder<IAnInterface> {BindingMode = BindingMode.OneWay};
         intializeBinding();
      }

      public void BindTo(IAnInterface objectToBind)
      {
         _objectToBind = objectToBind;
         _screenBinder.BindToSource(_objectToBind);
      }


      private void intializeBinding()
      {
         _screenBinder.Bind(item => item.FirstName).To(tbFirstName)
                 .OnValueUpdating += onFirstNameSet;

         _screenBinder.Bind(item => item.FirstName).To(tbAnotherFirstName)
                 .OnValueUpdating += onFirstNameSet;

         _screenBinder.Bind(item => item.ValueFromList).To(cbComboBox).WithValues(item => item.ListOfValues)
             .AndDisplays(item => item.ListOfDisplayValues)
             .OnValueUpdating += onValueFromListSet;

         _screenBinder.Changed += () => addLine("Screen Binder received on change Value event");

         _screenBinder.OnValidated += onValidated;
         _screenBinder.OnValidationError += onError;
         _screenBinder.Changing += () => addLine("Changing");
         cmdReset.Click += (o, e) => _screenBinder.Reset();
         btnApplyLocalChange.Click += (o, e) => changeDirectValue();

         _screenBinder.Bind(x => x.Disable).ToEnableOf(rtbDump).EnabledWhen(x => !x);

      }


      private void onValueFromListSet(IAnInterface arg1, PropertyValueSetEventArgs<string> arg3)
      {
         addLine(string.Format("List Value changing old Value = {0}, new value = {1}", _objectToBind.ValueFromList, arg3));
         _objectToBind.ValueFromList = arg3.OldValue;
      }

      private void onFirstNameSet(IAnInterface arg1, PropertyValueSetEventArgs<string> arg3)
      {
         addLine(string.Format("First Name old Value = {0}, new value = {1}", _objectToBind.FirstName, arg3.OldValue));
         _objectToBind.FirstName = arg3.OldValue;
      }

      private void onError(Control control, string errorMessage)
      {
         errorProvider.SetError(control, errorMessage);
      }


      private void onValidated(Control control)
      {
         errorProvider.SetError(control, string.Empty);
      }

      private void addLine(string message)
      {
         rtbDump.AppendText(string.Format("{0}\n", message));
      }

      private void changeDirectValue()
      {
         _objectToBind.FirstName = tbValue.Text;
      }

      private void btnDumpClick(object sender, EventArgs e)
      {
         addLine(string.Format("First Name Value = {0}", _objectToBind.FirstName));
         addLine(string.Format("Value from list = {0}", _objectToBind.ValueFromList));
      }
   }
}
