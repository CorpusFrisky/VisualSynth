using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class PinBase
    {
        public PinBase()
        {
            ConnectedPins = new List<PinBase>();
        }

        public ISynthModule Module { get; set; }

        public List<PinBase> ConnectedPins { get; set; }

        public int PinIndex { get; set; }

        public String Label { get; set; }

        public virtual bool IsInput
        {
            get { throw new NotImplementedException(); }
        }

        public virtual PinTypeEnum PinType { get; set; }

        public virtual Point PinDesignPos
        {
            get { throw new NotImplementedException(); }
            //get { return new Point(0, 10 + (PinIndex * 20)); }
        }

        protected virtual bool CanConnect(PinBase pin)
        {
            return true;
        }
    }
}