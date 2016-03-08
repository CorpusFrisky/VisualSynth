using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;

namespace CorpusFrisky.VisualSynth.SynthModules.Models
{
    public class ConnectedModule
    {
        public PinBase Pin { get; set; }
        public ISynthModule Module { get; set; }
    }
}