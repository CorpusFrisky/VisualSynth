using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void SetDesignPos(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            if (canvas != null)
            {
                var point = e.GetPosition(canvas);
                ViewModel.CurrentDesignPos = new Point((int)point.X, (int)point.Y);
            }
        }

        private void SetMousePos(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(DesignCanvas);
            ViewModel.CurrentMousePos = new Point((int) point.X, (int) point.Y);
        }

        private void DesignCanvas_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (e.OriginalSource != DesignCanvas)
            {
                e.Handled = true;
            }
        }
    }
}
