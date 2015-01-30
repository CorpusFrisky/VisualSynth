using System.Drawing;
using OpenTK;

namespace CorpusFrisky.VisualSynth.SynthModules
{
    public interface ISynthModule
    {
        void PreRender();
        void Render();
        void PostRender();
    }
}