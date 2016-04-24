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

        public override bool IsInput => false;

        public override bool IsOutputRendered { get; set; }

        public uint RenderedOutputBufferId { get; set; }

        public List<Action<bool>> CommandListOutput { get; set; }
    }
}