using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
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
    /// Interaction logic for TriangleGeneratorView.xaml
    /// </summary>
    public partial class TriangleGeneratorView : UserControl
    {
        public TriangleGeneratorViewModel ViewModel
        {
            get { return DataContext as TriangleGeneratorViewModel; }
            set { DataContext = value; }
        }

        public TriangleGeneratorView(ISynthModule viewModel)
        {
            InitializeComponent();
            ViewModel = new TriangleGeneratorViewModel(viewModel);
        }

    }
}
