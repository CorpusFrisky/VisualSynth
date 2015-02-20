using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators;

namespace CorpusFrisky.VisualSynth.SynthModules.Views.ShapeGenerators
{
    /// <summary>
    /// Interaction logic for RectangleGeneratorView.xaml
    /// </summary>

    public partial class RectangleGeneratorView : UserControl
    {
        public RectangleGeneratorViewModel ViewModel
        {
            get { return DataContext as RectangleGeneratorViewModel; }
            set { DataContext = value; }
        }

        public RectangleGeneratorView(ISynthModule viewModel)
        {
            InitializeComponent();
            ViewModel = new RectangleGeneratorViewModel(viewModel);
        }
    }

}
