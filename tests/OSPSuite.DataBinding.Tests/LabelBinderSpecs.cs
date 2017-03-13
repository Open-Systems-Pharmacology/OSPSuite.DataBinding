using System.Windows.Forms;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.DataBinding.Tests
{
    public abstract class concern_for_LabelBinder : ContextSpecification<IElementBinder<IAnInterface, IAnInterface>>
    {
        protected Label _label;
        protected ScreenBinder<IAnInterface> _binder;
        protected AnImplementation _child1;
        protected AnImplementation _child2;
        protected AnImplementation _source;

        public override void Cleanup()
        {
            _binder.Dispose();
        }

        protected override void Context()
        {
            _binder = new ScreenBinder<IAnInterface>();
            _label = new Label();
            sut = _binder.Bind(item => item.Child).To(_label);

            _child1 = new AnImplementation() { FirstName = "Juri", LastName = "Solodenko" };
            _child2 = new AnImplementation() { FirstName = "Joerg", LastName = "Lippert" };

            _source = new AnImplementation() { FirstName = "Michael", LastName = "Sevestre",Child = _child1 };
        }
    }

    public class When_binding_to_a_reference_property_type : concern_for_LabelBinder
    {
        protected override void Because()
        {
            sut.Bind(_source);
        }

        [Observation]
        public void should_use_the_to_string_method_of_the_property_for_the_binding()
        {
            _label.Text.ShouldBeEqualTo(_child1.ToString());
        }
    }
}