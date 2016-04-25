using System;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class OutputFramePin : OutputPin
    {
        public override bool IsInput { get { return false; } }

        public Func<int> GetColorTextureId_Function { get; set; }

        public Func<int> GetDepthTextureId_Function { get; set; }

        public override bool IsOutputRendered
        {
            get { return true; }
            set { /*ignore attempts to set this*/ }
        }
    }
}