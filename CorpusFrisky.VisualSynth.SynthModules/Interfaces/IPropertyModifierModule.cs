using CorpusFrisky.VisualSynth.Common;
using OpenTK;
using OpenTK.Graphics;

namespace CorpusFrisky.VisualSynth.SynthModules
{
    public interface IPropertyModifierModule: IModifierModule
    {
        double GetValue();
    }
}