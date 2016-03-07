using System.Drawing;
using CorpusFrisky.VisualSynth.SynthModules;
using CorpusFrisky.VisualSynth.SynthModules.Interfaces;
using Microsoft.Practices.Prism.Mvvm;

namespace CorpusFrisky.VisualSynth.Models
{
    public class SynthComponentModel : BindableBase
    {
        private int _height;

        public ISynthModule Module { get; set; }
        public Point DesignPos { get; set; }

        public int Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }
    }
}