using System;
using System.Drawing;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class PinBase
    {
        public ISynthModule Module { get; set; }

        public int PinIndex { get; set; }

        public String Label { get; set; }

        public virtual bool IsInput
        {
            get { throw new NotImplementedException(); }
        }

        public PinTypeEnum PinType { get; set; }

        public virtual Point PinDesignPos
        {
            get { throw new NotImplementedException(); }
            //get { return new Point(0, 10 + (PinIndex * 20)); }
        }
    }
}