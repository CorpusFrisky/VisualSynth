using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.Models;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.ViewModels;
using OpenTK.Graphics.OpenGL;
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

            GlControl1.Height = (int)Height;
            GlControl1.Width = (int)Width;

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
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var synthComponenets = _designViewModel.SynthComponents.ToList();

            PreRenderModules(synthComponenets);

            foreach (var component in _designViewModel.SynthComponents)
            {
                component.Module.Render();
            }

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
    }
}
