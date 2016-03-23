using CorpusFrisky.VisualSynth.SynthModules.Models.Pins;
using Microsoft.Practices.Prism.Mvvm;
using System.Drawing;

namespace CorpusFrisky.VisualSynth.Models
{
    public class ConnectionWire : BindableBase
    {
        private Point _pinPos1;
        private Point _pinPos2;
        private bool _isHighlighted;
        private bool _isDeletionTarget;

        public ConnectionWire()
        {
            IsHighlighted = false;
            IsDeletionTarget = false;
        }

        public PinBase OutputConnection { get; set; }

        public PinBase InputConnection { get; set; }

        public Point Pin1Pos
        {
            get { return _pinPos1; }
            set { SetProperty(ref _pinPos1, value); }
        }

        public Point Pin2Pos
        {
            get { return _pinPos2; }
            set { SetProperty(ref _pinPos2, value); }
        }

        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set { SetProperty(ref _isHighlighted, value); }
        }

        public bool IsDeletionTarget
        {
            get { return _isDeletionTarget; }
            set { SetProperty(ref _isDeletionTarget, value); }
        }


    }
}