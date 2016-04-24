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
    public class ColorInverterViewModel : SynthModuleBaseViewModel, IDisposable
    {
        private int _inputFramebufferId;
        private int _outputFramebufferId;
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

        //TODO: stick this in a base class
        public void Dispose()
        {
            GL.DeleteTextures(1, ref _colorTextureId);
            GL.DeleteTextures(1, ref _depthTextureId);
            GL.Ext.DeleteFramebuffers(1, ref _inputFramebufferId);
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
                GetBufferId_Function = () => FramebufferId,
            });
        }

        public override SynthModuleType ModuleType { get { return SynthModuleType.Summer; } }

        private List<OutputHybridPin> SourcePins { get; set; }

        private int FramebufferId => _outputFramebufferId;

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
                InitializeTexturesAndFramebuffer();
            }
            else
            {
                // Create a FBO and attach the textures
                GL.Ext.GenFramebuffers(1, out _inputFramebufferId);
                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, _inputFramebufferId);
                GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, _colorTextureId, 0);
                GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, TextureTarget.Texture2D, _depthTextureId, 0);

                CheckForErrorsInitializingTexturesAndFramebuffer();
            }

            if (SourcePins.Any(x => x.IsOutputRendered))
            {
                //Render all command list source pins
                //Combine render result and rendered image source pins
            }
            else
            {
                GL.PushAttrib(AttribMask.ViewportBit);
                {
                    GL.Viewport(0, 0, GlobalValues.DisplayWidth, GlobalValues.DisplayHeight);

                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    //Do all the rendering leading up to this module into our inputframebuffer
                    var commandList = SourcePins.SelectMany(x => x.CommandListOutput);
                    foreach(var command in commandList)
                    {
                        command.Invoke(true);
                    }
                }
                GL.PopAttrib();
                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // disable rendering into the FBO

                GL.ClearColor(.1f, .2f, .3f, 0f);
                GL.Color3(1f, 1f, 1f);

                GL.Enable(EnableCap.Texture2D); // enable Texture Mapping
                GL.BindTexture(TextureTarget.Texture2D, 0); // bind default texture

                //Now draw this framebuffer into the output framebuffer using the effecting shader

                //The module connected to our output will then render the output framebuffer as a texture
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

        //TODO: Make these inheritable by all modules that could generate frames

        private void InitializeTexturesAndFramebuffer()
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
            GL.Ext.GenFramebuffers(1, out _inputFramebufferId);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, _inputFramebufferId);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, _colorTextureId, 0);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, TextureTarget.Texture2D, _depthTextureId, 0);

            CheckForErrorsInitializingTexturesAndFramebuffer();
        }

        private void CheckForErrorsInitializingTexturesAndFramebuffer()
        { 
            //TODO: log instead of console write
            switch (GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt))
            {
                case FramebufferErrorCode.FramebufferCompleteExt:
                    {
                        Console.WriteLine("FBO: The framebuffer is complete and valid for rendering.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteAttachmentExt:
                    {
                        Console.WriteLine("FBO: One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteMissingAttachmentExt:
                    {
                        Console.WriteLine("FBO: There are no attachments.");
                        break;
                    }
                /* case  FramebufferErrorCode.GL_FRAMEBUFFER_INCOMPLETE_DUPLICATE_ATTACHMENT_EXT: 
                     {
                         Console.WriteLine("FBO: An object has been attached to more than one attachment point.");
                         break;
                     }*/
                case FramebufferErrorCode.FramebufferIncompleteDimensionsExt:
                    {
                        Console.WriteLine("FBO: Attachments are of different size. All attachments must have the same width and height.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteFormatsExt:
                    {
                        Console.WriteLine("FBO: The color attachments have different format. All color attachments must have the same format.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteDrawBufferExt:
                    {
                        Console.WriteLine("FBO: An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteReadBufferExt:
                    {
                        Console.WriteLine("FBO: The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferUnsupportedExt:
                    {
                        Console.WriteLine("FBO: This particular FBO configuration is not supported by the implementation.");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("FBO: Status unknown. (yes, this is really bad.)");
                        break;
                    }
            }

            // using FBO might have changed states, e.g. the FBO might not support stereoscopic views or double buffering
            int[] queryinfo = new int[6];
            GL.GetInteger(GetPName.MaxColorAttachmentsExt, out queryinfo[0]);
            GL.GetInteger(GetPName.AuxBuffers, out queryinfo[1]);
            GL.GetInteger(GetPName.MaxDrawBuffers, out queryinfo[2]);
            GL.GetInteger(GetPName.Stereo, out queryinfo[3]);
            GL.GetInteger(GetPName.Samples, out queryinfo[4]);
            GL.GetInteger(GetPName.Doublebuffer, out queryinfo[5]);
            Console.WriteLine("max. ColorBuffers: " + queryinfo[0] + " max. AuxBuffers: " + queryinfo[1] + " max. DrawBuffers: " + queryinfo[2] +
                               "\nStereo: " + queryinfo[3] + " Samples: " + queryinfo[4] + " DoubleBuffer: " + queryinfo[5]);

            Console.WriteLine("Last GL Error: " + GL.GetError());
        }

        #endregion

    }
}