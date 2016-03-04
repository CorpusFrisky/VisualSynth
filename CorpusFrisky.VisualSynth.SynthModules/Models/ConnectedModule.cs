using CorpusFrisky.VisualSynth.SynthModules.Interfaces;

namespace CorpusFrisky.VisualSynth.SynthModules.Models
{
    public class ConnectedModule
    {
        public int Pin { get; set; }
        public ISynthModule Module { get; set; }
    }
}