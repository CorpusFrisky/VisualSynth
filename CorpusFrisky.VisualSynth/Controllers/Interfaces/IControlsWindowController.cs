using Autofac.Core;

namespace CorpusFrisky.VisualSynth.Controllers.Interfaces
{
    public interface IControlsWindowController
    {
        void Show();
        void Show(params Parameter[] parameters);
    }
}
