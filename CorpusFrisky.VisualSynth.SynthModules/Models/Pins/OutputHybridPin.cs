using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using System;
using System.Collections.Generic;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class OutputHybridPin : OutputPin
    {
        public override PinTypeEnum PinType
        {
            get { return IsOutputRendered ? PinTypeEnum.Image : PinTypeEnum.CommandList; }
        }

        public override bool IsInput { get { return false; } }

        public bool IsOutputRendered { get; set; }

        public Func<byte[]> GetRenderedOutput_Function { get; set; }

        public List<Action> GetCommandOutput_Function { get; set; }
    }
}