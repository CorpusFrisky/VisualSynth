using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using System.Collections.ObjectModel;

namespace CorpusFrisky.VisualSynth.SynthModules.Interfaces
{
    public interface ISynthModule
    {
        void Initialize();
        void PreRender();
        void Render(bool fromFinalRenderCall = false);
        void PostRender();

        bool ConnectSynthModule(InputPin inputPin, OutputPin outputPin);
        void DisconnectSynthModule(InputPin inputPin, OutputPin outputPin);

        SynthModuleType ModuleType { get; }
        bool HasRenderedThisFrame { get; set; }

        ObservableCollection<PinBase> InputPins { get; set; } 
        ObservableCollection<PinBase> OutputPins { get; set; }
    }
}