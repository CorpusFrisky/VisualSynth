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
                return new Point(0, 10 + (DesignSequence * 20));
            }
        }
    }
}