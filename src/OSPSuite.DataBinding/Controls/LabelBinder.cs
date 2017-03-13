using System.Windows.Forms;
using OSPSuite.DataBinding.Core;

namespace OSPSuite.DataBinding.Controls
{
    public abstract class LabelBinderBase<TObject, TPropertyType,TControlType> : ElementBinder<TObject, TPropertyType> where TControlType:Control
    {
        protected readonly TControlType _label;

        protected LabelBinderBase(IPropertyBinderNotifier<TObject, TPropertyType> propertyBinder, TControlType label): base(propertyBinder)
        {
            _label = label;
        }

        public override Control Control
        {
            get { return _label; }
        }

        public override TPropertyType GetValueFromControl()
        {
            //nothing to do here
            return GetValueFromSource();
        }

        public override void SetValueToControl(TPropertyType value)
        {
            _label.Text = Formatter.Format(value);
        }
    }

    public class LabelBinder<TObject, TPropertyType> : LabelBinderBase<TObject, TPropertyType,Label>
    {
        public LabelBinder(IPropertyBinderNotifier<TObject, TPropertyType> propertyBinder, Label label) : base(propertyBinder, label)
        {
        }
    }
}