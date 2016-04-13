using CorpusFrisky.VisualSynth.Common;
using System.Drawing;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public abstract class OutputPin : PinBase
    {
        public override Point PinDesignPos
        {
            get
            {
                return new Point(DesignConstants.ModuleBodyWidth + DesignConstants.PinWidth, DesignConstants.PinsPadding + (PinIndex * (DesignConstants.PinHeight + DesignConstants.PinMargin)));
            }
        }

    }
}