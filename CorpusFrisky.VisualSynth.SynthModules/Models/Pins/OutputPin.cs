using System;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public abstract class OutputPin : PinBase
    {
        public abstract Func<double> GetValue_Function { get; set; }
    }
}