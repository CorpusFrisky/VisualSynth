using System.Drawing;
using CorpusFrisky.VisualSynth.Common;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class OutputValuePin : PinBase
    {
        public override bool IsInput { get { return false; } }


        public override Point PinDesignPos
        {
            get
            {
                return new Point(DesignConstants.ModuleBodyWidth + DesignConstants.PinWidth, DesignConstants.PinsPadding + (PinIndex * (DesignConstants.PinHeight + DesignConstants.PinMargin)));
            }
        }
    }
}