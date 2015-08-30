using CorpusFrisky.VisualSynth.Common;

namespace CorpusFrisky.VisualSynth.SynthModules
{
    public interface ISynthModule
    {
        void PreRender();
        void Render();
        void PostRender();

        bool ConnectSynthModule(/*int pin,*/ ISynthModule module);
        bool DisconnectSynthModule(/*int pin,*/ ISynthModule module);

        SynthModuleType ModuleType { get; }
    }
}