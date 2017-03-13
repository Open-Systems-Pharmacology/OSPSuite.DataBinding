using System.Windows.Forms;

namespace OSPSuite.DataBinding.Starter
{
    public partial class FormComboBox : Form
    {
        private ScreenBinder<Individual> _screenBinder;
        private readonly Individual _source;

        public FormComboBox()
        {
            InitializeComponent();
            _source = new Individual();

            _source.Species = "Human";
            _source.Population = new Population("American");
            _source.Gender = "Male";

            IntializeBinding();
        
            _screenBinder.BindToSource(_source);
        }

        private void IntializeBinding()
        {
            _screenBinder = new ScreenBinder<Individual>();
            _screenBinder.Bind(ind => ind.Species).To(cb1)
                .WithValues(Constants.ListOfSpecies())
                .Changed+=RefreshList;

            _screenBinder.Bind(ind => ind.Population).To(cb2)
                        .WithValues(ind => Constants.ListOfPopulationFor(ind.Species))
                        .AndDisplays(pop=>pop.DisplayName)
                        .Changed += RefreshList;

            _screenBinder.Bind(ind => ind.Gender).To(cb3)
                .WithValues(ind => Constants.ListOfGenderFor(ind.Population));
        }

        private void RefreshList()
        {
            _screenBinder.RefreshListElements();
        }
    }
}
