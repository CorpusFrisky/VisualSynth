using CorpusFrisky.VisualSynth.SynthModules.Interfaces;

namespace CorpusFrisky.VisualSynth.SynthModules.Models
{
    public class ConnectedModule
    {
        public Pin Pin { get; set; }
        public ISynthModule Module { get; set; }
    }
}