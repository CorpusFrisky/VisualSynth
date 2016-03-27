using CorpusFrisky.VisualSynth.Common;
using System;
using System.Drawing;

namespace CorpusFrisky.VisualSynth.SynthModules.Models.Pins
{
    public class OutputValuePin : OutputPin
    {
        public override bool IsInput { get { return false; } }

        public override Point PinDesignPos
        {
            get
            {
                return new Point(DesignConstants.ModuleBodyWidth + DesignConstants.PinWidth, DesignConstants.PinsPadding + (PinIndex * (DesignConstants.PinHeight + DesignConstants.PinMargin)));
            }
        }

        public override Func<double> GetValue_Function { get; set; }
    }
}