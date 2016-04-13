using System;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class OutputValuePin : OutputPin
    {
        public override bool IsInput { get { return false; } }

        public Func<double> GetValue_Function { get; set; }
    }
}