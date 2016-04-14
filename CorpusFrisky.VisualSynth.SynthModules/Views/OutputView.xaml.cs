using CorpusFrisky.VisualSynth.SynthModules.ViewModels;
using System.Windows.Controls;

namespace CorpusFrisky.VisualSynth.SynthModules.Views.Modifiers
{
    public partial class OutputView : UserControl
    {
        public OutputViewModel ViewModel
        {
            get { return DataContext as OutputViewModel; }
            set { DataContext = value; }
        }

        public OutputView()
        {
            InitializeComponent();
        }
    }
}
