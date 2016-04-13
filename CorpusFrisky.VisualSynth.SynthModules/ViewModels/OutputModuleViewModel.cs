using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
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

        public override void Initialize()
        {
            base.Initialize();

            SetupPins();
        }

        protected override void SetupPins()
        {
            InputPins.Add(new InputHybridPin()
            {
                Module = this,
                PinIndex = 0,
                Label = "Input"
            });

            EventAggregator.GetEvent<PinSetupCompleteEvent>().Publish(new PinSetupCompleteEventArgs
            {
                SynthModule = this
            });
        }

        public OutputHybridPin RenderSource { get; set; }

        public override void Render()
        {
            if (RenderSource.IsOutputRendered)
            {
                
            }
        }

        protected override void ToggleConnectedModule(PinConnection pinConnection, bool adding)
        {
            var pin = pinConnection.InputPin;

            if (pin.IsInput)
            {
                if (pin.PinType == PinTypeEnum.Value)
                {

                }
                else if (pin.PinType == PinTypeEnum.Hybrid)
                {
                    ToggleInputImageModule(pinConnection, adding);
                }
            }
        }

        private void ToggleInputImageModule(PinConnection pinConnection, bool adding)
        {
            var inputPin = pinConnection.InputPin as InputHybridPin;
            var outputPin = pinConnection.OutputPin as OutputHybridPin;

            if (inputPin == null)
            {
                // TODO: log a message
                return;
            }

            RenderSource = outputPin;
        }

    }
}