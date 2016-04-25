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
            if(!BaseBeginRender(fromFinalRenderCall) ||
                InputPins[0].ConnectedPins.Count != 1)
            {
                return;
            }

            var inputConnectionAsHybrid = InputPins[0].ConnectedPins[0] as OutputHybridPin;
            if (inputConnectionAsHybrid != null)
            {
                if (inputConnectionAsHybrid.IsOutputRendered)
                {
                   RenderInputBuffers(inputConnectionAsHybrid.GetColorTextureId_Function.Invoke(),
                       inputConnectionAsHybrid.GetDepthTextureId_Function.Invoke());
                }
                else
                {
                    foreach (var command in inputConnectionAsHybrid.CommandListOutput)
                    {
                        command.Invoke(true);
                    }
                }
                return;
            }

            var inputConnectionAsFrame = InputPins[0].ConnectedPins[0] as OutputFramePin;
            if (inputConnectionAsFrame != null)
            {
                RenderInputBuffers(inputConnectionAsFrame.GetColorTextureId_Function.Invoke(),
                       inputConnectionAsFrame.GetDepthTextureId_Function.Invoke());
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

        private void RenderInputBuffers(int colorTextureId, int depthTextureId)
        {
            GL.PushMatrix();
            {
                // Draw the Color Texture
                GL.Translate(-1.1f, 0f, 0f);
                GL.BindTexture(TextureTarget.Texture2D, colorTextureId);
                GL.Begin(BeginMode.Quads);
                {
                    GL.TexCoord2(0f, 1f);
                    GL.Vertex2(-1.0f, 1.0f);
                    GL.TexCoord2(0.0f, 0.0f);
                    GL.Vertex2(-1.0f, -1.0f);
                    GL.TexCoord2(1.0f, 0.0f);
                    GL.Vertex2(1.0f, -1.0f);
                    GL.TexCoord2(1.0f, 1.0f);
                    GL.Vertex2(1.0f, 1.0f);
                }
                GL.End();

                // Draw the Depth Texture
                GL.Translate(+2.2f, 0f, 0f);
                GL.BindTexture(TextureTarget.Texture2D, depthTextureId);
                GL.Begin(BeginMode.Quads);
                {
                    GL.TexCoord2(0f, 1f);
                    GL.Vertex2(-1.0f, 1.0f);
                    GL.TexCoord2(0.0f, 0.0f);
                    GL.Vertex2(-1.0f, -1.0f);
                    GL.TexCoord2(1.0f, 0.0f);
                    GL.Vertex2(1.0f, -1.0f);
                    GL.TexCoord2(1.0f, 1.0f);
                    GL.Vertex2(1.0f, 1.0f);
                }
                GL.End();
            }
            GL.PopMatrix();
        }
    }
}