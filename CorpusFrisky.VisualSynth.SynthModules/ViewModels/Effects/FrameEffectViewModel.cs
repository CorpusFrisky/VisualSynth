using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Models;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels.Effects.Shaders;
using Microsoft.Practices.Prism.PubSubEvents;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.Effects
{
    public class FrameEffectViewModel : SynthModuleBaseViewModel, IDisposable
    {
        private int _framebufferId;
        private int _colorTextureId;
        private int _depthTextureId;


        public FrameEffectViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        public void Dispose()
        {
            GL.DeleteTextures(1, ref _colorTextureId);
            GL.DeleteTextures(1, ref _depthTextureId);
            GL.Ext.DeleteFramebuffers(1, ref _framebufferId);
        }

        private bool TexturesAndBufferAreInitialized { get; set; }

        private List<OutputHybridPin> SourcePins { get; set; }

        protected override void SetupPins()
        {
            throw new NotImplementedException();
        }

        public override void PreRender()
        {
        }

        public override void Render(bool fromFinalRenderCall = false)
        {
            if (!BaseBeginRender(fromFinalRenderCall))
            {
                return;
            }

            if (!TexturesAndBufferAreInitialized)
            {
                InitializeTexturesAndFramebuffer();
            }

            // Create a FBO and attach the textures
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, _framebufferId);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext,
                TextureTarget.Texture2D, _colorTextureId, 0);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt,
                TextureTarget.Texture2D, _depthTextureId, 0);

            CheckForErrorsInitializingTexturesAndFramebuffer();

            if (SourcePins.Any(x => x.IsOutputRendered))
            {
                //Render all command list source pins
                //Combine render result and rendered image source pins
            }
            else
            {
                GL.PushAttrib(AttribMask.ViewportBit);
                GL.Viewport(0, 0, GlobalValues.DisplayWidth, GlobalValues.DisplayHeight);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                Shader shader = new Shader(null,
                    System.IO.File.ReadAllText(
                        @"C:\Projects\VisualSynth\CorpusFrisky.VisualSynth.SynthModules\ViewModels\FrameEffects\Shaders\ColorInverterFragmentShader.glsl"));
                Shader.Bind(shader);

                //Do all the rendering leading up to this module into our inputframebuffer
                var commandList = SourcePins.SelectMany(x => x.CommandListOutput);
                foreach (var command in commandList)
                {
                    command.Invoke(true);
                }

                GL.PopAttrib();
                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // disable rendering into the FBO

                Shader.Bind(null);
                GL.Enable(EnableCap.Texture2D); // enable Texture Mapping
                GL.BindTexture(TextureTarget.Texture2D, 0); // bind default texture
            }
        }

        public override void PostRender()
        {
        }

        protected override void ToggleConnectedModule(PinConnection pinConnection, bool adding)
        {
            throw new NotImplementedException();
        }

        #region Helper Methods

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
            GL.Ext.GenFramebuffers(1, out _framebufferId);
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