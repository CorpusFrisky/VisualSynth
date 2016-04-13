using CorpusFrisky.VisualSynth.Common;
using System.Drawing;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class InputPin : PinBase
    {
        public virtual bool ConnectSynthModule(OutputPin pin)
        {
            if (!CanConnect(pin))
            {
                return false;
            }

            ConnectedPins.Add(pin);
            pin.ConnectedPins.Add(this);

            return Module.ConnectSynthModule(this, pin);
        }

        public virtual void DisconnectSynthModule(OutputPin pin)
        {
            ConnectedPins.Remove(pin);
            pin.ConnectedPins.Remove(this);

            Module.DisconnectSynthModule(this, pin);
        }

        public override Point PinDesignPos
        {
            get
            {
                return new Point(0, DesignConstants.PinsPadding + (PinIndex * (DesignConstants.PinHeight + DesignConstants.PinMargin)));
            }
        }
    }
}