using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.Utility
{
    public class SummerViewModel : SynthModuleBaseViewModel
    {
        public SummerViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            SourcePins = new List<OutputHybridPin>();
        }

        public override void Initialize()
        {
            base.Initialize();

            SetupPins();
        }

        protected override void SetupPins()
        {
            InputPins.Add(new InputValuePin
            {
                Module = this,
                PinIndex = 0,
                Label = "Input",
                PinType = PinTypeEnum.Hybrid,
            });

            //Need to reference this.  May add more in the future.
            PrimaryOutputPin = new OutputHybridPin()
            {
                Module = this,
                CommandListOutput = new List<Action>(),
                IsOutputRendered = false
            };

            OutputPins.Add(PrimaryOutputPin);
        }

        private List<OutputHybridPin> SourcePins { get; set; }

        public OutputHybridPin PrimaryOutputPin { get; set; }

        public override void PreRender()
        {
        }

        public override void Render()
        {
            if (SourcePins.Any(x => x.IsOutputRendered))
            {
                //Render all command list source pins
                //Combine render result and rendered image source pins
            }
            else
            {
                PrimaryOutputPin.CommandListOutput.AddRange(SourcePins.SelectMany(x => x.CommandListOutput));
            }
        }

        public override void PostRender()
        {
        }

        protected override void ToggleConnectedModule(PinConnection pinConnection, bool adding)
        {
            var pin = pinConnection.InputPin;

            if (pin.PinType == PinTypeEnum.Value)
            {

            }
            else if (pin.PinType == PinTypeEnum.Hybrid ||
                     pin.PinType == PinTypeEnum.CommandList ||
                     pin.PinType == PinTypeEnum.Image)
            {
                ToggleInputImageModule(pinConnection.OutputPin as OutputHybridPin, adding);
            }
        }

        private void ToggleInputImageModule(OutputHybridPin outputHybridPin, bool adding)
        {
            if (adding &&
                !SourcePins.Contains(outputHybridPin))
            {
                SourcePins.Add(outputHybridPin);
            }
            else if (!adding &&
                     SourcePins.Contains(outputHybridPin))
            {
                SourcePins.Remove(outputHybridPin);
            }
        }
    }
}