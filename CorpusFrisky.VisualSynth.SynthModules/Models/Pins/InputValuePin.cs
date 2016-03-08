using System.Drawing;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class InputValuePin : PinBase
    {
        public object TargetObject { get; set; }

        public PinTargetTypeEnum TargetType { get; set; }

        public PinTagetPropertyEnum TargetProperty { get; set; }

        public override bool IsInput { get { return true; } }


        public override Point PinDesignPos
        {
            get
            {
                return new Point(0, 10 + (DesignSequence * 20));
            }
        }
    }
}