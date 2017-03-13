using System.Windows.Forms;
using OSPSuite.DataBinding.Tests;

namespace OSPSuite.DataBinding.Starter
{
   public partial class Form1 : Form
   {
      private readonly IAnInterface _object1;

      public Form1()
      {
         InitializeComponent();
         _object1 = new AnImplementation();

         _object1.FirstName = "Good name1";
         screenBinderDirect.BindTo(_object1);
      }
   }
}