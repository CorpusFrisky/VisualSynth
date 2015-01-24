using Autofac.Core;

namespace CorpusFrisky.VisualSynth.Controllers.Interfaces
{
    public interface IDisplayWindowController
    {
        void Show();
        void Show(params Parameter[] parameters);
    }
}
