using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.Events;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.PubSubEvents;
using OpenTK.Graphics.OpenGL;
using System.Collections.ObjectModel;

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
                Label = "Input",
                PinType = PinTypeEnum.Hybrid
            });

            EventAggregator.GetEvent<PinSetupCompleteEvent>().Publish(new PinSetupCompleteEventArgs
            {
                SynthModule = this
            });
        }


        public override SynthModuleType ModuleType { get { return SynthModuleType.Output; } }

        public override void PreRender()
        {
        }

        public override void Render(bool fromFinalRenderCall = false)
        {
            RenderInputs();

            if (InputPins[0].ConnectedPins.Count != 1)
            {
                return;
            }

            var inputAsHybrid = InputPins[0].ConnectedPins[0] as OutputHybridPin;
            if (inputAsHybrid != null)
            {
                if (inputAsHybrid.IsOutputRendered)
                {
                    //display contents of buffer with id == inputAsHybrid.RenderedOutputBufferId
                }
                else
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(0.0, 1000.0, 0.0, 1000.0, 0.0, 4.0);

                    foreach (var command in inputAsHybrid.CommandListOutput)
                    {
                        command.Invoke(true);
                    }
                }         
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

        private void ToggleInputImageModule(OutputHybridPin outputPin, bool adding)
        {
        }

    }
}