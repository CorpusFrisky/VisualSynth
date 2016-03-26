using CorpusFrisky.VisualSynth.Common;
using System;
using System.Drawing;

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

        public Func<double> GetValue_Function { get; set; }
    }
}