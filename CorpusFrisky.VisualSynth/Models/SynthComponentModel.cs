using System;
using System.Drawing;
using CorpusFrisky.VisualSynth.Common;
using CorpusFrisky.VisualSynth.SynthModules;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using Microsoft.Practices.Prism.Mvvm;

namespace CorpusFrisky.VisualSynth.Models
{
    public class SynthComponentModel : BindableBase
    {
        public ISynthModule Module { get; set; }
        public Point DesignPos { get; set; }

        public int Height
        {
            get
            {
                var maxNumPins = Math.Max(Module.InputPins.Count, Module.OutputPins.Count);
                //Height for each pin + margin between each pin + padding for top and bottom
                return (maxNumPins * DesignConstants.PinHeight) + ( (maxNumPins - 1) * ( DesignConstants.PinMargin) ) + (2 * DesignConstants.PinsPadding);
            }
        }

        public int Width
        {
            get { return DesignConstants.ModuleBodyWidth; }
        }

        public void UpdateDimensions()
        {
            OnPropertyChanged("Height");
            OnPropertyChanged("Width");
        }
    }
}