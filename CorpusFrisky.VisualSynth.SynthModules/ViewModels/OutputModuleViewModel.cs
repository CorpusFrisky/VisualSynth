using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.PubSubEvents;
using System.Collections.ObjectModel;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels
{
    public class OutputModuleViewModel : SynthModuleBaseViewModel
    {
        public OutputModuleViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            InputPins = new ObservableCollection<PinBase>();
            OutputPins = new ObservableCollection<PinBase>();
        }

        protected override void ToggleConnectedModule(PinConnection pinConnection, bool adding)
        {
            throw new System.NotImplementedException();
        }
    }
}