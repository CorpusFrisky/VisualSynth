using Microsoft.Practices.Prism.Mvvm;
using OpenTK;
using OpenTK.Graphics;

namespace CorpusFrisky.VisualSynth.SynthModules.Models
{
    public class VertexModel : BindableBase
    {
        private Vector3 _position;
        private Color4 _color;


        public Vector3 Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        public Color4 Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }
    }
}