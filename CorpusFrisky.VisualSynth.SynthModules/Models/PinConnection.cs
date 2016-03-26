using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;

namespace CorpusFrisky.VisualSynth.SynthModules.Models
{
    public class PinConnection
    {
        public PinBase InputPin { get; set; }
        public PinBase OutputPin { get; set; }
    }
}