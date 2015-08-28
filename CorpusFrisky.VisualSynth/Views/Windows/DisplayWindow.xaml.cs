using System.Timers;
using System.Windows;
using System.Windows.Forms;
using CorpusFrisky.VisualSynth.Common;
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
            foreach(var component in _designViewModel.SynthComponents)
            {
                component.Module.Render();
            }
            GlControl1.SwapBuffers();
        }
    }
}
