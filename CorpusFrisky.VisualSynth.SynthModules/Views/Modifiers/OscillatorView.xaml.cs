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
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Modifiers;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.ShapeGenerators;

namespace CorpusFrisky.VisualSynth.SynthModules.Views.Modifiers
{
    /// <summary>
    /// Interaction logic for OscillatorView.xaml
    /// </summary>
    public partial class OscillatorView : UserControl
    {
        public OscillatorViewModel ViewModel
        {
            get { return DataContext as OscillatorViewModel; }
            set { DataContext = value; }
        }

        public OscillatorView(ISynthModule viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel as OscillatorViewModel;
        }
    }
}
