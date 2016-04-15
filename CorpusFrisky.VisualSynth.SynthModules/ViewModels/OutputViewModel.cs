using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.PubSubEvents;
using System.Collections.ObjectModel;
using CorpusFrisky.VisualSynth.Common;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels
{
    public class OutputViewModel : SynthModuleBaseViewModel
    {
        public OutputViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
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

        public override SynthModuleType ModuleType { get { return SynthModuleType.Output; } }

        public override void PreRender()
        {
        }

        public override void Render()
        {
            if (RenderSource == null)
            {
                return;
            }

            if (RenderSource.IsOutputRendered)
            {
                
            }
            else
            {
                foreach (var command in RenderSource.CommandListOutput)
                {
                    command.Invoke();
                }
            }
        }

        public override void PostRender()
        {
        }

        protected override void ToggleConnectedModule(PinConnection pinConnection, bool adding)
        {
            var pin = pinConnection.InputPin;

            if (pin.IsInput)
            {
                if (pin.PinType == PinTypeEnum.Value)
                {

                }
                else if (pin.PinType == PinTypeEnum.Hybrid ||
                    pin.PinType == PinTypeEnum.CommandList ||
                    pin.PinType == PinTypeEnum.Image)
                {
                    ToggleInputImageModule(pinConnection, adding);
                }
            }
        }

        private void ToggleInputImageModule(PinConnection pinConnection, bool adding)
        {
            if (!adding)
            {
                RenderSource = null;
                return;
            }

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