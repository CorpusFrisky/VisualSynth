using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Effects.Shaders;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;

//This file contains code from the following online OpenTK tutorial:
// http://www.opentk.com/node/397

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.Effects
{
    public class ColorInverterViewModel : SynthModuleBaseViewModel
    {
        

        public ColorInverterViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            SourcePins = new List<OutputHybridPin>();
        }

        public override void Initialize()
        {
            base.Initialize();

            SetupPins();

            var binPath =
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Shader = new Shader(binPath + "\\ColorInverterVertexShader.glsl",
                binPath + "\\ColorInverterFragmentShader.glsl");
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

        public override SynthModuleType ModuleType => SynthModuleType.ColorInverter;

        private List<OutputHybridPin> SourcePins { get; set; }

        private Shader Shader { get; set; }

        public override void PreRender()
        {
        }

        public override void Render(bool fromFinalRenderCall = false)
        {
            if (!BaseBeginRender(fromFinalRenderCall))
            {
                return;
            }

            
            if (SourcePins.Any(x => x.IsOutputRendered))
            {
             
            }
            else
            {
                //set shader

                //add all commands prior to this
                var outputPinAsHybrid = OutputPins[0] as OutputHybridPin;
                if (outputPinAsHybrid != null)
                {
                    outputPinAsHybrid.CommandListOutput.Clear();
                    //outputPinAsHybrid.CommandListOutput.Add();
                    outputPinAsHybrid.CommandListOutput.AddRange(SourcePins.SelectMany(x => x.CommandListOutput));
                    //outputPinAsHybrid.CommandListOutput.Add();
                }

                //unset shader   
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