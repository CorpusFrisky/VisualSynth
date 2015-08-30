using System.Drawing;
using CorpusFrisky.VisualSynth.SynthModules;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;

namespace CorpusFrisky.VisualSynth.Models
{
    public class SynthComponentModel
    {
        public ISynthModule Module { get; set; }
        public Point DesignPos { get; set; }
    }
}