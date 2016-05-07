using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Effects;
using System.Windows.Controls;

namespace CorpusFrisky.VisualSynth.SynthModules.Views.Effects
{
    public partial class ColorInverterView : UserControl
    {
        public ColorInverterViewModel ViewModel
        {
            get { return DataContext as ColorInverterViewModel; }
            set { DataContext = value; }
        }

        public ColorInverterView()
        {
            InitializeComponent();
        }

    }
}
