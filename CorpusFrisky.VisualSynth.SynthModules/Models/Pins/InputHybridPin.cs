using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class InputHybridPin : InputPin
    {
        public override PinTypeEnum PinType
        {
            get { return IsInputRendered ? PinTypeEnum.Image : PinTypeEnum.CommandList; }
        }

        public override bool IsInput { get { return true; } }

        public bool IsInputRendered { get; set; }

    }
}