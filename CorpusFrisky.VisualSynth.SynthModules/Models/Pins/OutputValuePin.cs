using System.Drawing;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class OutputValuePin : PinBase
    {
        public override bool IsInput { get { return false; } }


        public override Point PinDesignPos
        {
            get
            {
                return new Point(80, 10 + (PinIndex * 20));
            }
        }
    }
}