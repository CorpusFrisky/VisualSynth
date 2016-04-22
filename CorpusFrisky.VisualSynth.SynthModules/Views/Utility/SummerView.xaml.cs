using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Utility;
using System.Windows.Controls;

namespace CorpusFrisky.VisualSynth.SynthModules.Views.Utility
{
    public partial class SummerView : UserControl
    {
        public SummerViewModel ViewModel
        {
            get { return DataContext as SummerViewModel; }
            set { DataContext = value; }
        }

        public SummerView()
        {
            InitializeComponent();
        }

    }
}
