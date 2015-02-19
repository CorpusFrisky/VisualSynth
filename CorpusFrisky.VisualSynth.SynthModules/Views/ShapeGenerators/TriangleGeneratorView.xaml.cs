using CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators;
using System.Windows.Controls;

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
