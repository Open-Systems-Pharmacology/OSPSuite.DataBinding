using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;
using OSPSuite.DataBinding.Core;

namespace OSPSuite.DataBinding.Controls
{
   public class ComboBoxElementBinder<TObject, TPropertyType> : ListElementBinder<TObject, TPropertyType>
    {
        private readonly ComboBox _comboBox;

        public ComboBoxElementBinder(IPropertyBinderNotifier<TObject, TPropertyType> propertyBinder, ComboBox comboBox): base(propertyBinder)
        {
            _comboBox = comboBox;
            _comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
         }

        public override Control Control
        {
            get { return _comboBox; }
        }

        public override TPropertyType GetValueFromControl()
        {
            return ValueFromIndex(_comboBox.SelectedIndex);
        }

        public override void SetValueToControl(TPropertyType value)
        {
             _comboBox.SelectedIndex = IndexFromValue(value);
        }

        protected override void FillWith(IEnumerable<TPropertyType> listOfValues, IEnumerable<string> listOfDisplayValues)
        {
            _comboBox.SelectedValueChanged -= selectedValueChanged;
            _comboBox.TextChanged -= textChanged;
            _comboBox.SelectedItem = null;
 
            _comboBox.SuspendLayout();
            _comboBox.Items.Clear();
            listOfDisplayValues.Each(item => _comboBox.Items.Add(item));
            _comboBox.ResumeLayout();
            _comboBox.SelectedValueChanged += selectedValueChanged;
            _comboBox.TextChanged += textChanged;
        }

        private void textChanged(object sender, EventArgs e)
        {
           ValueInControlChanging();   
        }

        private void selectedValueChanged(object sender, EventArgs e)
        {
            ValueInControlChanged();
        }
    }
}