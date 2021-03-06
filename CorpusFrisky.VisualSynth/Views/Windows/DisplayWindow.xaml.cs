﻿using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.Models;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.ViewModels;
using CorpusFrisky.VisualSynth.ViewModels;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace CorpusFrisky.VisualSynth.Views.Windows
{
    /// <summary>
    /// Interaction logic for DisplayWindow.xaml
    /// </summary>
    public partial class DisplayWindow : Window
    {
        private readonly DesignViewModel _designViewModel;

        private Timer _repaintTimer;

        public DisplayWindow(DesignViewModel designViewModel)
        {
            InitializeComponent();
            _designViewModel = designViewModel;

            WinFormHost.Height = Height;
            WinFormHost.Width = Width;

            GlobalValues.DisplayHeight = GlControl1.Height = (int)Height;
            GlobalValues.DisplayWidth = GlControl1.Width = (int)Width;

            _repaintTimer = new Timer(1000.0 / Constants.FrameRate);
            _repaintTimer.Elapsed += Repaint;
            _repaintTimer.Start();
        }

        private void Repaint(object sender, ElapsedEventArgs e)
        {
            GlControl1.Invalidate();
        }

        private void Control_OnPaint(object sender, PaintEventArgs e)
        {
            var widthAsFloat = (float) Width;
            var heightAsFloat = (float) Height;
            var aspectRatio = widthAsFloat/heightAsFloat;


            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, aspectRatio,
                .1f, 100f);
            GL.LoadMatrix(ref perspectiveMatrix);
            GL.Viewport(0, 0, (int)Width, (int)Height);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            var synthComponenets = _designViewModel.SynthComponents.ToList();
            foreach (var module in synthComponenets.Select(x => x.Module))
            {
                module.HasRenderedThisFrame = false;
            }

            PreRenderModules(synthComponenets);

            RenderModules(synthComponenets);

            foreach (var component in _designViewModel.SynthComponents)
            {
                component.Module.PostRender();
            }

            GlControl1.SwapBuffers();
        }

        private void PreRenderModules(List<SynthComponentModel> synthComponenets)
        {
            //Be sure to calculate modifier values first.
            var modifierModules = synthComponenets.Select(x => x.Module).Where(x => x is IModifierModule);
            var otherModules = synthComponenets.Select(x => x.Module).Where(x => !(x is IModifierModule));

            foreach (var module in modifierModules)
            {
                module.PreRender();
            }

            foreach (var module in otherModules)
            {
                module.PreRender();
            }
        }

        private void RenderModules(List<SynthComponentModel> synthComponenets)
        {
            //Let's try recursive rendering from output module, see how it works.
            var outputModule = (synthComponenets.First(x => x.Module is OutputViewModel).Module) as OutputViewModel;
            outputModule?.Render();
        }
    }
}
