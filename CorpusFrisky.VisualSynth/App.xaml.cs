using System.Windows;
using CorpusFrisky.VisualSynth.Bootstrap;
using CorpusFrisky.VisualSynth.SynthModules.Models.Modifiers;

namespace CorpusFrisky.VisualSynth
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            OnStartup();

            var bootstrapper = new GlobalBootstrapper();
            bootstrapper.Run();
        }

        private void OnStartup()
        {
            Oscillator.InitOscillatorTables();
        }
    }
}
