using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Documents;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using CorpusFrisky.VisualSynth.SynthModules.Models.Enums;

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

        public PinTypeEnum PinType { get; set; }

        public virtual Point PinDesignPos
        {
            get { throw new NotImplementedException(); }
            //get { return new Point(0, 10 + (PinIndex * 20)); }
        }

        public virtual bool ConnectSynthModule(PinBase pin)
        {
            if (!CanConnect(pin))
            {
                return false;
            }

            ConnectedPins.Add(pin);
            pin.ConnectedPins.Add(this);
            
            return Module.ConnectSynthModule(this, pin.Module);
        }

        public virtual void DisconnectSynthModule(PinBase pin)
        {
            ConnectedPins.Remove(pin);
            pin.ConnectedPins.Remove(this);

            Module.DisconnectSynthModule(this, pin.Module);
        }

        protected virtual bool CanConnect(PinBase pin)
        {
            return true;
        }
    }
}