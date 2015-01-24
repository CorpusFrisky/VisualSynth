using System.Windows.Controls;
using CorpusFrisky.VisualSynth.ViewModels;

namespace CorpusFrisky.VisualSynth.Views.ControlViews
{
    /// <summary>
    /// Interaction logic for DesignView.xaml
    /// </summary>
    public partial class DesignView : UserControl
    {
        public DesignViewModel ViewModel
        {
            get { return DataContext as DesignViewModel; }
            set { DataContext = value; }
        }

        public DesignView(DesignViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }
    }
}
