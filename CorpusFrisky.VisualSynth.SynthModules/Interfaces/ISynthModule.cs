using System.Collections.ObjectModel;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Models;

namespace CorpusFrisky.VisualSynth.SynthModules.Interfaces
{
    public interface ISynthModule
    {
        void SetupPins();
        void PreRender();
        void Render();
        void PostRender();

        bool ConnectSynthModule(Pin pin, ISynthModule module);
        bool DisconnectSynthModule(Pin pin, ISynthModule module);


        SynthModuleType ModuleType { get; }

        ObservableCollection<Pin> Pins { get; set; } 
    }
}