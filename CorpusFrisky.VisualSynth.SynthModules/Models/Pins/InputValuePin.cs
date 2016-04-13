using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class InputValuePin : InputPin
    {
        public object TargetObject { get; set; }

        public PinTargetTypeEnum TargetType { get; set; }

        public PinTargetPropertyEnum TargetProperty { get; set; }

        public override bool IsInput { get { return true; } }


    }
}