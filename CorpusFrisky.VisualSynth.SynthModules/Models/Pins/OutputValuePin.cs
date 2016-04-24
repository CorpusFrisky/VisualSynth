using System;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class OutputValuePin : OutputPin
    {
        public override bool IsInput => false;

        public Func<double> GetValue_Function { get; set; }

        public override bool IsOutputRendered
        {
            get { return false; }
            set { /*Ignore attempts to set this*/}
        }
    }
}