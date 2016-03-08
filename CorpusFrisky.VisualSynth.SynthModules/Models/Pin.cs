using System;
using System.Drawing;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using OpenTK;

namespace CorpusFrisky.VisualSynth.SynthModules.Models
{
    public class Pin
    {
        public int PinIndex { get; set; }

        public String Label { get; set; }

        public object TargetObject { get; set; }

        public PinTypeEnum PinType { get; set; }

        public PinTargetTypeEnum TargetType { get; set; }

        public PinTagetPropertyEnum TargetProperty { get; set; }

        public Point PinDesignPos
        {
            get { return new Point(0, 10 + (PinIndex*20)); }
        }
    }
}