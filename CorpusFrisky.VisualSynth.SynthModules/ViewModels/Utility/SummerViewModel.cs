using CorpusFrisky.VisualSynth.Common;
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

            OutputPins.Add(new OutputHybridPin()
            {
                Module = this,
                CommandListOutput = new List<Action<bool>>(),
                IsOutputRendered = false
            });
        }

        public override SynthModuleType ModuleType { get { return SynthModuleType.Summer; } }

        private List<OutputHybridPin> SourcePins { get; set; }

        public override void PreRender()
        {
        }

        public override void Render(bool fromFinalRenderCall = false)
        {
            if(!BaseBeginRender(fromFinalRenderCall)) return;;

            if (SourcePins.Any(x => x.IsOutputRendered))
            {
                //Render all command list source pins
                //Combine render result and rendered image source pins
            }
            else
            {
                var outputPinAsHybrid = OutputPins[0] as OutputHybridPin;
                outputPinAsHybrid?.CommandListOutput.Clear();
                outputPinAsHybrid?.CommandListOutput.AddRange(SourcePins.SelectMany(x => x.CommandListOutput));
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