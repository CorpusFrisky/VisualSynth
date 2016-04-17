using System.Windows.Controls;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Generators;

namespace CorpusFrisky.VisualSynth.SynthModules.Views.Generators
{
    public partial class ShapeGeneratorView : UserControl
    {
        public ShapeGeneratorBaseViewModel ViewModel
        {
            get { return DataContext as ShapeGeneratorBaseViewModel; }
            set { DataContext = value; }
        }

        public ShapeGeneratorView()
        {
            InitializeComponent();
        }

    }
}
