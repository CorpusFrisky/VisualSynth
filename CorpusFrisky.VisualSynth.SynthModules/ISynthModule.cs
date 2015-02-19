using CorpusFrisky.VisualSynth.Common;

namespace CorpusFrisky.VisualSynth.SynthModules
{
    public interface ISynthModule
    {
        void PreRender();
        void Render();
        void PostRender();

        SynthModuleType ModuleType { get; }
    }
}