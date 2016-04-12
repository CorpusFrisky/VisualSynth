using CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators;
using System.Windows.Controls;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;

namespace CorpusFrisky.VisualSynth.SynthModules.Views.ShapeGenerators
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
