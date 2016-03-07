using System;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;

namespace CorpusFrisky.VisualSynth.SynthModules.Models
{
    public class Pin
    {
        public int PinIndex { get; set; }

        public String Label { get; set; }

        public object TargetObject { get; set; }

        public PinTargetTypeEnum TargetType { get; set; }

        public PinTagetPropertyEnum TargetProperty { get; set; }
    }
}