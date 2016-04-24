using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.PubSubEvents;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

//This file contains code from the following online OpenTK tutorial:
// http://www.opentk.com/node/397

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.FrameEffects
{
    public class ColorInverterViewModel : SynthModuleBaseViewModel
    {
        private int _bufferId;
        private int _colorTextureId;
        private int _depthTextureId;

        public ColorInverterViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
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

            OutputPins.Add(new OutputFramePin()
            {
                Module = this,
                GetBufferId_Function = () => BufferId,
            });
        }

        public override SynthModuleType ModuleType { get { return SynthModuleType.Summer; } }

        private List<OutputHybridPin> SourcePins { get; set; }

        private int BufferId => _bufferId;

        private bool TexturesAndBufferAreInitialized { get; set; }

        public override void PreRender()
        {
        }

        public override void Render(bool fromFinalRenderCall = false)
        {
            if(!BaseBeginRender(fromFinalRenderCall))
            {
                return;
            }

            if (!TexturesAndBufferAreInitialized)
            {
                InitializeTexturesAndBuffer();
            }

            if (SourcePins.Any(x => x.IsOutputRendered))
            {
                //Render all command list source pins
                //Combine render result and rendered image source pins
            }
            else
            {
                var commandList = SourcePins.SelectMany(x => x.CommandListOutput);
                
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

        #region Helper Methods

        private void InitializeTexturesAndBuffer()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearDepth(1.0f);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Disable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);

            // Create Color Tex
            GL.GenTextures(1, out _colorTextureId);
            GL.BindTexture(TextureTarget.Texture2D, _colorTextureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, GlobalValues.DisplayWidth, GlobalValues.DisplayHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            // Create Depth Tex
            GL.GenTextures(1, out _depthTextureId);
            GL.BindTexture(TextureTarget.Texture2D, _depthTextureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)All.DepthComponent32, GlobalValues.DisplayWidth, GlobalValues.DisplayHeight, 0, PixelFormat.DepthComponent, PixelType.UnsignedInt, IntPtr.Zero);
            // things go horribly wrong if DepthComponent's Bitcount does not match the main Framebuffer's Depth
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            // Create a FBO and attach the textures
            GL.Ext.GenFramebuffers(1, out _bufferId);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, _bufferId);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, _colorTextureId, 0);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, TextureTarget.Texture2D, _depthTextureId, 0);

        }

        #endregion
    }
}