using System.Drawing;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class InputValuePin : InputPin
    {
        public object TargetObject { get; set; }

        public PinTargetTypeEnum TargetType { get; set; }

        public PinTagetPropertyEnum TargetProperty { get; set; }

        public override bool IsInput { get { return true; } }


        public override Point PinDesignPos
        {
            get
            {
                return new Point(0, DesignConstants.PinsPadding + (PinIndex * (DesignConstants.PinHeight + DesignConstants.PinMargin)));
            }
        }
    }
}